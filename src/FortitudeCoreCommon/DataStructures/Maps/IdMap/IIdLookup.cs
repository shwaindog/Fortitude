#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.DataStructures.Maps.IdMap;

public interface IIdLookup<T> : IEnumerable<System.Collections.Generic.KeyValuePair<int, T>>, ICloneable,
    IInterfacesComparable<IIdLookup<T>>
{
    T? this[int id] { get; }
    int this[T name] { get; }
    int Count { get; }
    T? GetValue(int id);
    int GetId(T name);
    new IIdLookup<T> Clone();
}
