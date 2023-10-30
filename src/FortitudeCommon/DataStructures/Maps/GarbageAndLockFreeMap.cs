#region

using System.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeCommon.DataStructures.Maps;

public interface DisposableEnumerable<Tkvp> : IEnumerable<Tkvp>, IDisposable { }

public interface IGarbageFreeMap<TK, TV> : IMap<TK, TV> where TK : notnull
{
    DisposableEnumerable<KeyValuePair<TK, TV>> DisposableEnumerableEnumerator();
}

/// <summary>
/// Quick map for small collections less than 100 items
/// </summary>
/// <typeparam name="TK"></typeparam>
/// <typeparam name="TV"></typeparam>
public class GarbageAndLockFreeMap<TK, TV> : IGarbageFreeMap<TK, TV> where TK : notnull
{
    private readonly GarbageAndLockFreePooledFactory<KvpEnumerator> enumeratorPool =
        new(thisPool => new KvpEnumerator(thisPool));

    private readonly Func<TK, TK, bool> keyComparison;
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(GarbageAndLockFreeMap<TK, TV>));

    private readonly GarbageAndLockFreePooledFactory<Container> queueWithElements = new(() => new Container());

    private readonly List<KeyValuePair<TK, TV>> reusableListOfKeyValuePairs = new();

    private readonly GarbageAndLockFreePooledFactory<Container> surplusContainers = new(() => new Container());

    public GarbageAndLockFreeMap(Func<TK, TK, bool> keyComparison) => this.keyComparison = keyComparison;

    public TV this[TK key]
    {
        get
        {
            foreach (var container in queueWithElements)
                if (keyComparison(container.KeyValuePair.Key, key))
                    return container.KeyValuePair.Value;
            throw new KeyNotFoundException("Could not find item with key " + key);
        }
        set
        {
            foreach (var container in queueWithElements)
                if (keyComparison(container.KeyValuePair.Key, key))
                {
                    container.KeyValuePair = new KeyValuePair<TK, TV>(key, value, true);
                    return;
                }

            var newContainer = surplusContainers.Borrow();
            newContainer.KeyValuePair = new KeyValuePair<TK, TV>(key, value, true);
            queueWithElements.ReturnBorrowed(newContainer);
            var foundItemTime = 0;
            foreach (var container in queueWithElements)
                if (keyComparison(container.KeyValuePair.Key, key) && foundItemTime++ > 0)
                {
                    if (queueWithElements.Remove(container))
                        surplusContainers.ReturnBorrowed(container);
                    else
                        logger.Warn("Possible duplicate Keys as other source may not remove its duplicate Key");
                    return;
                }
        }
    }

    public int Count => queueWithElements.Count();

    public bool TryGetValue(TK key, out TV? value)
    {
        foreach (var container in queueWithElements)
            if (keyComparison(container.KeyValuePair.Key, key))
            {
                value = container.KeyValuePair.Value;
                return true;
            }

        value = default;
        return false;
    }

    public void Add(TK key, TV value)
    {
        foreach (var container in queueWithElements)
            if (keyComparison(container.KeyValuePair.Key, key))
                throw new ArgumentException("A key with the same name already exists.  " + key);
        var newContainer = surplusContainers.Borrow();
        var kvPair = new KeyValuePair<TK, TV>(key, value, true);
        newContainer.KeyValuePair = kvPair;
        reusableListOfKeyValuePairs.Clear();
        reusableListOfKeyValuePairs.Add(kvPair);
        OnUpdate?.Invoke(reusableListOfKeyValuePairs);
        queueWithElements.ReturnBorrowed(newContainer);
        var foundItemTime = 0;
        foreach (var container in queueWithElements)
            if (keyComparison(container.KeyValuePair.Key, key) && foundItemTime++ > 0)
            {
                if (queueWithElements.Remove(container)) surplusContainers.ReturnBorrowed(container);
                return;
            }
    }

    public bool Remove(TK key)
    {
        Container? foundItemToRemove = null;
        foreach (var container in queueWithElements)
            if (keyComparison(container.KeyValuePair.Key, key))
                foundItemToRemove = container;
        if (foundItemToRemove == null) return false;
        var foundContainer = queueWithElements.Remove(foundItemToRemove);
        return foundContainer;
    }

    public void Clear()
    {
        var count = Count;
        for (var i = 0; i < count; i++) surplusContainers.ReturnBorrowed(queueWithElements.Borrow());
    }

    public bool ContainsKey(TK key)
    {
        foreach (var container in queueWithElements)
            if (keyComparison(container.KeyValuePair.Key, key))
                return true;
        return false;
    }

    public event Action<IEnumerable<KeyValuePair<TK, TV>>>? OnUpdate;

    public DisposableEnumerable<KeyValuePair<TK, TV>> DisposableEnumerableEnumerator() =>
        enumeratorPool.Borrow().SourceEnumerator(queueWithElements.GetEnumerator());

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<TK, TV>> GetEnumerator() =>
        enumeratorPool.Borrow().SourceEnumerator(queueWithElements.GetEnumerator());

    private class KvpEnumerator : IEnumerator<KeyValuePair<TK, TV>>,
        DisposableEnumerable<KeyValuePair<TK, TV>>
    {
        private readonly GarbageAndLockFreePooledFactory<KvpEnumerator> thisPool;
        private IEnumerator<Container> toConvert = Container.SingletonContainer;

        public KvpEnumerator(GarbageAndLockFreePooledFactory<KvpEnumerator> thisPool) => this.thisPool = thisPool;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<KeyValuePair<TK, TV>> GetEnumerator() => this;

        public void Dispose()
        {
            thisPool.ReturnBorrowed(this);
            toConvert.Dispose();
        }

        public bool MoveNext() => toConvert.MoveNext();

        public void Reset()
        {
            toConvert?.Reset();
        }

        object IEnumerator.Current => Current;

        // ReSharper disable once PossibleNullReferenceException
        public KeyValuePair<TK, TV> Current => toConvert.Current.KeyValuePair;

        internal KvpEnumerator SourceEnumerator(IEnumerator<Container> sourcEnumerator)
        {
            toConvert = sourcEnumerator;
            return this;
        }
    }


    private class Container
    {
        public static readonly IEnumerator<Container> SingletonContainer
            = new List<Container>().GetEnumerator();

        public KeyValuePair<TK, TV> KeyValuePair { get; set; }
    }
}
