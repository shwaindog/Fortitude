using System;
using System.Collections.Generic;
using FortitudeCommon.Types;
using Generic = System.Collections.Generic;

namespace FortitudeCommon.DataStructures.Maps.IdMap
{
    public interface IIdLookup<T> : IEnumerable<Generic.KeyValuePair<int, T>>, ICloneable, 
        IInterfacesComparable<IIdLookup<T>>
    {
        T this[int id] { get; }
        int this[T name] { get; }
        T GetValue(int id);
        int GetId(T name);
        int Count { get; }
        new IIdLookup<T> Clone();
    }
}