using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;

namespace FortitudeCommon.DataStructures.Maps
{

    public interface DisposableEnumerable<Tkvp> : IEnumerable<Tkvp>,  IDisposable
    {
    }

    public interface IGarbageFreeMap<Tk, Tv> : IMap<Tk, Tv>
    {
        DisposableEnumerable<KeyValuePair<Tk, Tv>> DisposableEnumerableEnumerator();
    }

    /// <summary>
    /// Quick map for small collections less than 100 items
    /// </summary>
    /// <typeparam name="Tk"></typeparam>
    /// <typeparam name="Tv"></typeparam>
    public class GarbageAndLockFreeMap<Tk, Tv> : IGarbageFreeMap<Tk, Tv>
    {
        private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(GarbageAndLockFreeMap<Tk, Tv>));

        private readonly GarbageAndLockFreePooledFactory<Container> queueWithElements = 
            new GarbageAndLockFreePooledFactory<Container>(() => new Container());
        private readonly GarbageAndLockFreePooledFactory<Container> surplusContainers = 
            new GarbageAndLockFreePooledFactory<Container>(() => new Container());
        private readonly GarbageAndLockFreePooledFactory<KvpEnumerator> enumeratorPool = 
            new GarbageAndLockFreePooledFactory<KvpEnumerator>(thisPool => new KvpEnumerator(thisPool));

        private readonly List<KeyValuePair<Tk, Tv>> reusableListOfKeyValuePairs = new List<KeyValuePair<Tk, Tv>>();

        private class KvpEnumerator : IEnumerator<KeyValuePair<Tk, Tv>>, DisposableEnumerable<KeyValuePair<Tk, Tv>>
        {
            private readonly GarbageAndLockFreePooledFactory<KvpEnumerator> thisPool;
            private IEnumerator<Container> toConvert;

            public KvpEnumerator(GarbageAndLockFreePooledFactory<KvpEnumerator> thisPool)
            {
                this.thisPool = thisPool;
            }

            internal KvpEnumerator SourceEnumerator(IEnumerator<Container> sourcEnumerator)
            {
                toConvert = sourcEnumerator;
                return this;
            }

            public void Dispose()
            {
                thisPool.ReturnBorrowed(this);
                toConvert.Dispose();
            }

            public bool MoveNext()
            {
                return toConvert.MoveNext();
            }

            public void Reset()
            {
                toConvert.Reset();
            }

            object IEnumerator.Current => Current;
            // ReSharper disable once PossibleNullReferenceException
            public KeyValuePair<Tk, Tv> Current => toConvert.Current.KeyValuePair;

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public IEnumerator<KeyValuePair<Tk, Tv>> GetEnumerator()
            {
                return this;
            }
        }


        private class Container
        {
            public KeyValuePair<Tk, Tv> KeyValuePair { get; set; }
        }

        private readonly Func<Tk, Tk, bool> keyComparison;

        public GarbageAndLockFreeMap(Func<Tk, Tk, bool> keyComparison)
        {
            this.keyComparison = keyComparison;
        }

        public Tv this[Tk key]
        {
            get {
                foreach (var container in queueWithElements)
                {
                    if (keyComparison(container.KeyValuePair.Key, key))
                    {
                        return container.KeyValuePair.Value;
                    }
                }
                throw new KeyNotFoundException("Could not find item with key " + key);
            }
            set
            {
                foreach (var container in queueWithElements)
                {
                    if (keyComparison(container.KeyValuePair.Key, key))
                    {
                        container.KeyValuePair = new KeyValuePair<Tk, Tv>(key, value, true);
                        return;
                    }
                }

                var newContainer = surplusContainers.Borrow();
                newContainer.KeyValuePair = new KeyValuePair<Tk, Tv>(key, value, true);
                queueWithElements.ReturnBorrowed(newContainer);
                int foundItemTime = 0;
                foreach (var container in queueWithElements)
                {
                    if (keyComparison(container.KeyValuePair.Key, key) && foundItemTime++ > 0)
                    {
                        if (queueWithElements.Remove(container))
                        {
                            surplusContainers.ReturnBorrowed(container);
                        }
                        else
                        {
                            logger.Warn("Possible duplicate Keys as other source may not remove its duplicate Key");
                        }
                        return;
                    }
                }
            }
        }

        public int Count => queueWithElements.Count();
        public bool TryGetValue(Tk key, out Tv value)
        {
            foreach (var container in queueWithElements)
            {
                if (keyComparison(container.KeyValuePair.Key, key))
                {
                    value = container.KeyValuePair.Value;
                    return true;
                }
            }
            value = default(Tv);
            return false;
        }

        public void Add(Tk key, Tv value)
        {
            foreach (var container in queueWithElements)
            {
                if (keyComparison(container.KeyValuePair.Key, key))
                {
                    throw new ArgumentException("A key with the same name already exists.  " + key);
                }
            }
            var newContainer = surplusContainers.Borrow();
            var kvPair = new KeyValuePair<Tk, Tv>(key, value, true);
            newContainer.KeyValuePair = kvPair;
            reusableListOfKeyValuePairs.Clear();
            reusableListOfKeyValuePairs.Add(kvPair);
            OnUpdate?.Invoke(reusableListOfKeyValuePairs);
            queueWithElements.ReturnBorrowed(newContainer);
            int foundItemTime = 0;
            foreach (var container in queueWithElements)
            {
                if (keyComparison(container.KeyValuePair.Key, key) && foundItemTime++ > 0)
                {
                    if (queueWithElements.Remove(container))
                    {
                        surplusContainers.ReturnBorrowed(container);
                    }
                    return;
                }
            }
        }

        public KeyValuePair<Tk, Tv> Remove(Tk key)
        {
            Container foundItemToRemove = null;
            foreach (var container in queueWithElements)
            {
                if (keyComparison(container.KeyValuePair.Key, key))
                {
                    foundItemToRemove = container;
                }
            }
            if(foundItemToRemove == null) return new KeyValuePair<Tk, Tv>(false);
            KeyValuePair<Tk, Tv> foundItems = foundItemToRemove.KeyValuePair;
            var foundContainer = queueWithElements.Remove(foundItemToRemove);
            return new KeyValuePair<Tk, Tv>(foundItems.Key, foundItems.Value, foundContainer);
        }

        public void Clear()
        {
            int count = Count;
            for (int i = 0; i < count; i++)
            {
                surplusContainers.ReturnBorrowed(queueWithElements.Borrow());
            }
        }

        public bool ContainsKey(Tk key)
        {
            foreach (var container in queueWithElements)
            {
                if (keyComparison(container.KeyValuePair.Key, key))
                {
                    return true;
                }
            }
            return false;
        }

        public event Action<IEnumerable<KeyValuePair<Tk, Tv>>> OnUpdate;

        public DisposableEnumerable<KeyValuePair<Tk, Tv>> DisposableEnumerableEnumerator()
        {
            return enumeratorPool.Borrow().SourceEnumerator(queueWithElements.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<Tk, Tv>> GetEnumerator()
        {
            return enumeratorPool.Borrow().SourceEnumerator(queueWithElements.GetEnumerator());
        }
    }
}
