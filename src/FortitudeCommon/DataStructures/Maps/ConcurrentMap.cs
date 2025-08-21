#region

using System.Collections;
using System.Collections.Concurrent;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeCommon.DataStructures.Maps;


public class ConcurrentMap<TK, TV> : IMap<TK, TV> where TK : notnull
{
    protected readonly ConcurrentDictionary<TK, TV> ConcurrentDictionary = new();

    // Do NOT make this static each dictionary needs it's own private instance as the enumerator must belong to this instant
    private readonly IRecycler myRecycler = new Recycler();

    // protected readonly object SyncLock = new();

    public ConcurrentMap() { }

    public ConcurrentMap(IMap<TK, TV> toClone) => ConcurrentDictionary = new ConcurrentDictionary<TK, TV>(toClone);

    public TV this[TK key]
    {
        get => ConcurrentDictionary[key];
        set
        {
            var (insert, update) = GetInsertUpdateCallbacks(key, value);
            ConcurrentDictionary.AddOrUpdate(key, insert.InsertCallback, update.UpdateCallback);
            insert.DecrementRefCount();
            update.DecrementRefCount();
        }
    }

    public ICollection<TK> Keys => ConcurrentDictionary.Keys;
    public ICollection<TV> Values => ConcurrentDictionary.Values;

    public TV? GetValue(TK key)
    {
        TryGetValue(key, out var value);
        return value;
    }

    public bool TryGetValue(TK key, out TV? value) => ConcurrentDictionary.TryGetValue(key, out value);

    public TV GetOrPut(TK key, Func<TK, TV> createFunc)
    {
        if (!TryGetValue(key, out var value))
        {
            value = createFunc(key);
            ConcurrentDictionary.TryAdd(key, value);
        }

        return value!;
    }

    public TV AddOrUpdate(TK key, TV value)
    {
        var (insert, update) = GetInsertUpdateCallbacks(key, value);
        
        var newValue = ConcurrentDictionary.AddOrUpdate(key, insert.InsertCallback, update.UpdateCallback);
        insert.DecrementRefCount();
        update.DecrementRefCount();
        return newValue;
    }

    // for dictionary initializer
    public void Add(TK key, TV value)
    {
        TryAdd(key, value);
    }

    public bool TryAdd(TK key, TV value)
    {
        if (ConcurrentDictionary.TryAdd(key, value))
        {
            OnUpdated(key, value, MapUpdateType.Create);
            return true;
        }

        return false;
    }

    public bool Remove(TK key)
    {
        if (ConcurrentDictionary.TryRemove(key, out var valueRemoved))
        {
            OnUpdated(key, valueRemoved, MapUpdateType.Remove);
            return true;
        }

        return false;
    }

    public void Clear()
    {
        var keyValuePairs = myRecycler.Borrow<ReusableList<KeyValuePair<TK, TV>>>();
        foreach (var keyValuePair in ConcurrentDictionary)
        {
            keyValuePairs.Add(keyValuePair);
        }
        ConcurrentDictionary.Clear();
        foreach (var keyValuePair in keyValuePairs)
        {
            OnUpdated(keyValuePair.Key, keyValuePair.Value, MapUpdateType.Remove);
            if (keyValuePair.Value is RecyclableObject valueRecyclableObject)
            {
                valueRecyclableObject.DecrementRefCount();
            }
        }
    }

    public int Count => ConcurrentDictionary.Count;

    public bool ContainsKey(TK key) => ConcurrentDictionary.ContainsKey(key);

    public virtual IEnumerator<KeyValuePair<TK, TV>> GetEnumerator()
    {
        var existingOrNew = myRecycler
            .Borrow<ReusableWrappingEnumerator<KeyValuePair<TK, TV>>>();
        existingOrNew.ProxiedEnumerator ??= ConcurrentDictionary.GetEnumerator();
        existingOrNew.Reset();
        return existingOrNew;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    object ICloneable.Clone() => Clone();

    public IMap<TK, TV> Clone() => new ConcurrentMap<TK, TV>(this);

    public event Action<TK, TV, MapUpdateType, IMap<TK, TV>>? Updated;

    protected  virtual void OnUpdated(TK key, TV value, MapUpdateType updateType)
    {
        Updated?.Invoke(key, value, updateType, this);
    }

    protected (InsertActionCallback, UpdateActionCallback) GetInsertUpdateCallbacks(TK key, TV value)
    {
        var insertCallback = myRecycler.Borrow<InsertActionCallback>().Initialize(this, key, value);
        var updateCallback = myRecycler.Borrow<UpdateActionCallback>().Initialize(this, key, value);
        return (insertCallback, updateCallback);
    }

    protected abstract class InsertOrUpdateEntryCallback : RecyclableObject
    {
        protected ConcurrentMap<TK, TV> Map = null!;

        protected bool WasCalled;
        protected TK   Key       = default!;
        protected TV   Value     = default!;

        public virtual InsertOrUpdateEntryCallback Initialize(ConcurrentMap<TK, TV> toInsertMap, TK keyToInsert, TV valueToInsert)
        {
            WasCalled = false;

            Map   = toInsertMap;
            Key   = keyToInsert;
            Value = valueToInsert;

            return this;
        }

        public override int DecrementRefCount()
        {
            if (WasCalled)
            {
                NotifyActionType();
            }
            return base.DecrementRefCount();
        }

        protected virtual void NotifyActionType()
        {
            Map.OnUpdated(Key, Value, MapUpdateType.Create);
        }

        public override void StateReset()
        {
            Map       = null!;
            WasCalled = false;

            base.StateReset();
        }
    }

    protected class InsertActionCallback : InsertOrUpdateEntryCallback
    {
        public readonly Func<TK, TV> InsertCallback;

        public InsertActionCallback()
        {
            InsertCallback = ExecuteInsert;
        }

        public override InsertActionCallback Initialize(ConcurrentMap<TK, TV> toInsertMap, TK keyToInsert, TV valueToInsert)
        {
            base.Initialize(toInsertMap, keyToInsert, valueToInsert);

            return this;
        }

        protected override void NotifyActionType()
        {
            Map.OnUpdated(Key, Value, MapUpdateType.Create);
        }

        private TV ExecuteInsert(TK keyToInsert)
        {
            WasCalled = true;
            if (Value is IRecyclableObject recyclableObject)
            {
                recyclableObject.IncrementRefCount();
            }
            return Value;
        }
    }

    protected class UpdateActionCallback : InsertOrUpdateEntryCallback
    {
        public readonly Func<TK, TV, TV> UpdateCallback;

        public UpdateActionCallback() => UpdateCallback = ExecuteUpdate;


        public override UpdateActionCallback Initialize(ConcurrentMap<TK, TV> toInsertMap, TK keyToInsert, TV valueToInsert)
        {
            base.Initialize(toInsertMap, keyToInsert, valueToInsert);

            return this;
        }

        protected override void NotifyActionType()
        {
            Map.OnUpdated(Key, Value, MapUpdateType.Update);
        }

        private TV ExecuteUpdate(TK keyToInsert, TV oldValue)
        {
            if (!ReferenceEquals(oldValue, Value))
            {
                WasCalled = true;
                if (Value is IRecyclableObject recyclableObjectNewValue)
                {
                    recyclableObjectNewValue.IncrementRefCount();
                }
                if (oldValue is IRecyclableObject recyclableObjectOldValue)
                {
                    recyclableObjectOldValue.DecrementRefCount();
                }
            }
            return Value;
        }
    }
}
