using System;
using System.Collections.Generic;

namespace FortitudeCommon.DataStructures.Maps
{
    public interface IMap<Tk, Tv> : IEnumerable<KeyValuePair<Tk, Tv>>
    {
        Tv this[Tk key] { get; set; }
        int Count { get; }
        bool TryGetValue(Tk key, out Tv value);
        void Add(Tk key, Tv value);
        KeyValuePair<Tk, Tv> Remove(Tk key);
        void Clear();
        bool ContainsKey(Tk key);

        event Action<IEnumerable<KeyValuePair<Tk, Tv>>> OnUpdate;
    }
}