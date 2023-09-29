using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FortitudeCommon.DataStructures.Maps
{
    [SuppressMessage("ReSharper", "InconsistentlySynchronizedField")]
    public class ConcurrentMap<Tk, Tv>: IMap<Tk, Tv>
    {
        private readonly object sync = new object();

        private readonly ConcurrentDictionary<Tk, Tv> concurrentDictionary = new ConcurrentDictionary<Tk, Tv>();

        public Tv this[Tk key]
        {
            get => concurrentDictionary[key];
            set
            {
                lock (sync)
                {
                    concurrentDictionary.AddOrUpdate(key, value, (oldKey, oldValue) => value);
                    OnUpdate?.Invoke(concurrentDictionary.Select(kvp => new KeyValuePair<Tk, Tv>(kvp)));
                }
            }
        }

        public bool TryGetValue(Tk key, out Tv value)
        {
            return concurrentDictionary.TryGetValue(key, out value);
        }

        public void Add(Tk key, Tv value)
        {
            lock (sync)
            {
                if (concurrentDictionary.TryAdd(key, value))
                {
                    OnUpdate?.Invoke(concurrentDictionary.Select(kvp => new KeyValuePair<Tk, Tv>(kvp)));
                }
            }
        }

        public KeyValuePair<Tk, Tv> Remove(Tk key)
        {
            lock (sync)
            {
                if (concurrentDictionary.TryRemove(key, out var removedValue))
                {
                    OnUpdate?.Invoke(concurrentDictionary.Select(kvp => new KeyValuePair<Tk, Tv>(kvp)));
                    return new KeyValuePair<Tk, Tv>(key, removedValue, true);
                }
            }
            return new KeyValuePair<Tk, Tv>(false);
        }

        public void Clear()
        {
            lock (sync)
            {
                concurrentDictionary.Clear();
                OnUpdate?.Invoke(concurrentDictionary.Select(kvp => new KeyValuePair<Tk, Tv>(kvp)));
            }
        }

        public int Count => concurrentDictionary.Count;

        public bool ContainsKey(Tk key)
        {
            return concurrentDictionary.ContainsKey(key);
        }

        public event Action<IEnumerable<KeyValuePair<Tk, Tv>>> OnUpdate;

        public virtual IEnumerator<KeyValuePair<Tk, Tv>> GetEnumerator()
        {
            return concurrentDictionary.Select(kvp => new KeyValuePair<Tk, Tv>(kvp.Key, kvp.Value, true))
                .GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
