#region

using System.Collections;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.MemoryPools;

// ReSharper disable InconsistentlySynchronizedField

#endregion

namespace FortitudeCommon.DataStructures.Maps;

public class LinkedListMap<TK, TV> : IMap<TK, TV> where TK : notnull
{
    private readonly object sync = new();

    private static readonly IRecycler MyRecycler = new Recycler();

    protected DoublyLinkedList<
        DoublyLinkedListWrapperNode<KeyValuePair<TK, TV>>> Chain = 
        MyRecycler.Borrow<DoublyLinkedList<DoublyLinkedListWrapperNode<KeyValuePair<TK, TV>>>>();

    public LinkedListMap() { }

    public LinkedListMap(IMap<TK, TV> toClone)
    {        
        foreach (var keyValuePair in toClone) TryAdd(keyValuePair.Key, keyValuePair.Value);
    }

    public TV this[TK key]
    {
        get
        {
            var currentNode = Chain.Head;
            for (; currentNode != null; currentNode = currentNode.Next)
                if (currentNode.Payload.Key.Equals(key))
                    return currentNode.Payload.Value;

            throw new KeyNotFoundException($"Could not find '{key}' in SafeChainMap");
        }
        set => AddOrUpdate(key, value!);
    }

    public Func<ReusableList<TK>> KeysListFactory { get; set; } = MyRecycler.Borrow<ReusableList<TK>>;

    public Func<ReusableList<TV>> ValuesListFactory { get; set; } = MyRecycler.Borrow<ReusableList<TV>>;
    
    public ICollection<TK> Keys
    {
        get
        {
            var keysList    = KeysListFactory();
            var currentNode = Chain.Head;
            for (; currentNode != null; currentNode = currentNode.Next) keysList.Add(currentNode.Payload.Key);
            return keysList;
        }
    }

    public ICollection<TV> Values
    {
        get
        {
            var valuesList  = ValuesListFactory();
            var currentNode = Chain.Head;
            for (; currentNode != null; currentNode = currentNode.Next) valuesList.Add(currentNode.Payload.Value);
            return valuesList;
        }
    }

    public TV? GetValue(TK key)
    {
        TryGetValue(key, out var value);
        return value;
    }

    public virtual bool TryGetValue(TK key, out TV? value)
    {
        var currentNode = Chain.Head;
        for (; currentNode != null; currentNode = currentNode.Next)
            if (currentNode.Payload.Key.Equals(key))
            {
                value = currentNode.Payload.Value!;
                return true;
            }

        value = default;
        return false;
    }

    public TV GetOrPut(TK key, Func<TK, TV> createFunc)
    {
        if (!TryGetValue(key, out var value))
        {
            value = createFunc(key);
            TryAdd(key, value);
        }

        return value!;
    }

    public TV AddOrUpdate(TK key, TV value)
    {
        var foundInExisting = false;
        var wasChanged      = false;
        
        DoublyLinkedList<DoublyLinkedListWrapperNode<KeyValuePair<TK, TV>>> toReplace;
        lock (sync)
        {
            toReplace = Chain;
            var duplicate   = MyRecycler.Borrow<DoublyLinkedList<DoublyLinkedListWrapperNode<KeyValuePair<TK, TV>>>>();
            var currentNode = Chain.Head;
            for (; currentNode != null; currentNode = currentNode.Next)
                if (currentNode.Payload.Key.Equals(key))
                {
                    var updatedNode = MyRecycler.Borrow<DoublyLinkedListWrapperNode<KeyValuePair<TK, TV>>>()
                                                .Initialize(new KeyValuePair<TK, TV>(key, value));
                    wasChanged = !ReferenceEquals(currentNode.Payload.Value, value);
                    if (wasChanged && value is IRecyclableObject valueRecyclable)
                    {
                        valueRecyclable.IncrementRefCount();
                        if (currentNode.Payload.Value is IRecyclableObject oldValueRecyclable)
                        {
                            oldValueRecyclable.DecrementRefCount();
                        }
                    }
                    duplicate.AddFirst(updatedNode);
                    foundInExisting = true;
                }
                else
                {
                    var copyNode = MyRecycler.Borrow<DoublyLinkedListWrapperNode<KeyValuePair<TK, TV>>>().Initialize(currentNode.Payload);
                    duplicate.AddFirst(copyNode);
                }

            if (!foundInExisting)
            {
                var newNode = MyRecycler.Borrow<DoublyLinkedListWrapperNode<KeyValuePair<TK, TV>>>().Initialize(new KeyValuePair<TK, TV>(key, value));
                duplicate.AddFirst(newNode);
            }

            Chain = duplicate;
        }
        if (foundInExisting)
        {
            if(wasChanged) Updated?.Invoke(key, value, MapUpdateType.Update, this);
        }
        else
        {
            Updated?.Invoke(key, value, MapUpdateType.Create, this);
        }
        lock (toReplace)
        {
            var currentNode = toReplace.Head;
            for (; currentNode != null; currentNode = currentNode.Next)
            {
                currentNode.DecrementRefCount();
            }
        }
        toReplace.DecrementRefCount();

        return value;
    }

    // for dictionary initializer
    public void Add(TK key, TV value)
    {
        TryAdd(key, value);
    }

    public bool TryAdd(TK key, TV value)
    {
        var foundInExisting = false;

        DoublyLinkedList<DoublyLinkedListWrapperNode<KeyValuePair<TK, TV>>> toReplace;
        lock (sync)
        {
            toReplace = Chain;
            var duplicate   = MyRecycler.Borrow<DoublyLinkedList<DoublyLinkedListWrapperNode<KeyValuePair<TK, TV>>>>();
            var currentNode = Chain.Head;
            for (; currentNode != null; currentNode = currentNode.Next)
            {
                if (currentNode.Payload.Key.Equals(key))
                {
                    foundInExisting = true;
                }

                var copyNode = MyRecycler.Borrow<DoublyLinkedListWrapperNode<KeyValuePair<TK, TV>>>().Initialize(currentNode.Payload);
                duplicate.AddFirst(copyNode);
            }
            if (foundInExisting)
            {
                currentNode = duplicate.Head;
                for (; currentNode != null; currentNode = currentNode.Next)
                {
                    currentNode.DecrementRefCount();
                }
                duplicate.DecrementRefCount();
                return false;
            }

            var newNode = MyRecycler.Borrow<DoublyLinkedListWrapperNode<KeyValuePair<TK, TV>>>().Initialize(new KeyValuePair<TK, TV>(key, value));
            duplicate.AddFirst(newNode);
            Chain = duplicate;
        }
        Updated?.Invoke(key, value, MapUpdateType.Create, this);
        
        lock (toReplace)
        {
            var currentNode = toReplace.Head;
            for (; currentNode != null; currentNode = currentNode.Next)
            {
                currentNode.DecrementRefCount();
            }
        }
        toReplace.DecrementRefCount();
        return !foundInExisting;
    }

    public bool Remove(TK key)
    {
        var foundKey   = false;
        TV? foundValue = default;

        DoublyLinkedList<DoublyLinkedListWrapperNode<KeyValuePair<TK, TV>>> toReplace;
        lock (sync)
        {
            toReplace = Chain;
            var duplicate   = MyRecycler.Borrow<DoublyLinkedList<DoublyLinkedListWrapperNode<KeyValuePair<TK, TV>>>>();
            var currentNode = toReplace.Head;
            for (; currentNode != null; currentNode = currentNode.Next)
                if (!(currentNode.Payload.Key.Equals(key)))
                {
                    var copyNode = MyRecycler.Borrow<DoublyLinkedListWrapperNode<KeyValuePair<TK, TV>>>().Initialize(currentNode.Payload);
                    duplicate.AddFirst(copyNode);
                }
                else
                {
                    foundValue = currentNode.Payload.Value;
                    if (foundValue is IRecyclableObject removeValueRecyclable)
                    {
                        removeValueRecyclable.DecrementRefCount();
                    }
                    foundKey   = true;
                }

            Chain = duplicate;
        }
        if (foundKey && !Equals(foundValue, null))
        {
            Updated?.Invoke(key, foundValue, MapUpdateType.Create, this);
        }
        lock (toReplace)
        {
            var currentNode = toReplace.Head;
            for (; currentNode != null; currentNode = currentNode.Next)
            {
                currentNode.DecrementRefCount();
            }
        }
        toReplace.DecrementRefCount();

        return foundKey;
    }

    public void Clear()
    {
        var removedKeyValuePairs = MyRecycler.Borrow<ReusableList<KeyValuePair<TK, TV>>>();
        lock (sync)
        {
            var currentNode          = Chain.Head;
            for (; currentNode != null; currentNode = currentNode.Next)
            {
                removedKeyValuePairs.Add(currentNode.Payload);
                currentNode.DecrementRefCount();
            }
            Chain = MyRecycler.Borrow<DoublyLinkedList<DoublyLinkedListWrapperNode<KeyValuePair<TK, TV>>>>();
        }
        foreach (var removedKvp in removedKeyValuePairs)
        {
            Updated?.Invoke(removedKvp.Key, removedKvp.Value, MapUpdateType.Remove, this);
            if (removedKvp.Value is IRecyclableObject removeValueRecyclable)
            {
                removeValueRecyclable.DecrementRefCount();
            }
        }
        removedKeyValuePairs.DecrementRefCount();
    }

    public int Count => Chain.Count();

    public bool ContainsKey(TK key)
    {
        return TryGetValue(key, out _);
    }

    public event Action<TK, TV, MapUpdateType, IMap<TK, TV>>? Updated;

    object ICloneable.Clone() => Clone();

    public IMap<TK, TV> Clone() => new LinkedListMap<TK, TV>(this);

    public virtual IEnumerator<KeyValuePair<TK, TV>> GetEnumerator()
    {
        return Chain.Select(wn => wn.Payload).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
