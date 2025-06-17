#region

using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.DataStructures.Maps;

public interface IMap<TK, TV> : IEnumerable<KeyValuePair<TK, TV>>, ICloneable<IMap<TK, TV>> where TK : notnull
{
    ICollection<TK> Keys { get; }
    ICollection<TV> Values { get; }
    TV this[TK key] { get; set; }
    int  Count { get; }
    TV?  GetValue(TK key);
    bool TryGetValue(TK key, out TV? value);
    TV   AddOrUpdate(TK key, TV value);
    bool Add(TK key, TV value);
    bool Remove(TK key);
    void Clear();
    bool ContainsKey(TK key);

    TV GetOrPut(TK key, Func<TK, TV> createFunc);

    event Action<IEnumerable<KeyValuePair<TK, TV>>> OnUpdate;
}
