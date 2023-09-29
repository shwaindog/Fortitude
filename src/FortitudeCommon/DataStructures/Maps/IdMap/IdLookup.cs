using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Generic = System.Collections.Generic;

namespace FortitudeCommon.DataStructures.Maps.IdMap
{
    /// <summary>
    /// A readonly concert implementation of INameIdLookup.  To populate override and access the cache member directly.
    /// see NameIdLookupGenerator.
    /// </summary>
    public class IdLookup<T> : IIdLookup<T>
    {
        protected readonly IDictionary<int, T> Cache = new Dictionary<int, T>();
        protected readonly IDictionary<T, int> ReverseLookup = new Dictionary<T, int>();
        
        public IdLookup()
        {
        }

        public IdLookup(IDictionary<int, T> copyDict)
        {
            if (copyDict == null) return;
            Cache = new Dictionary<int, T>(copyDict);
            ReverseLookup = Cache.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        }

        public IdLookup(IIdLookup<T> toClone) : this( (toClone as IdLookup<T>)?.Cache)
        {
        }

        public virtual T this[int id] => Cache.TryGetValue(id, out var name) ? name : default(T);

        public virtual int this[T name]
        {
            get
            {
                if (name == null) return 0;
                return ReverseLookup.ContainsKey(name)
                    ? ReverseLookup[name]
                    : 0;
            }
        }

        public int Count => Cache.Count;

        public T GetValue(int id)
        {
            return this[id];
        }

        public int GetId(T name)
        {
            return this[name];
        }

        public virtual object Clone()
        {
            return new IdLookup<T>(this);
        }

        IIdLookup<T> IIdLookup<T>.Clone()
        {
            return (IIdLookup<T>)Clone();
        }

        public virtual bool AreEquivalent(IIdLookup<T> other, bool exactTypes = false)
        {
            if (other == null) return false;
            var containsAtLeast = !other.Except(this).Any();
            var countSame = true;
            if (exactTypes)
            {
                var exactNameIdLookup = other as IdLookup<T>;
                countSame = Cache.Count == (exactNameIdLookup?.Cache?.Count ?? 0);
            }
            return containsAtLeast && countSame;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || AreEquivalent(obj as IIdLookup<T>, true);
        }

        public override int GetHashCode()
        {
            var hashCode = 0;
            foreach(Generic.KeyValuePair<int, T> kvp in Cache)
            {
                hashCode = (hashCode * 397) ^ kvp.Key.GetHashCode();
                hashCode = (hashCode * 397) ^ kvp.Value.GetHashCode();
            }
            return hashCode;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<Generic.KeyValuePair<int, T>> GetEnumerator()
        {
            return Cache.GetEnumerator();
        }
    }
}