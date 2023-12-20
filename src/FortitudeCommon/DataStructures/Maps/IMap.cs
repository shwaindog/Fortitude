namespace FortitudeCommon.DataStructures.Maps;

public interface IMap<TK, TV> : IEnumerable<KeyValuePair<TK, TV>> where TK : notnull
{
    TV? this[TK key] { get; set; }
    int Count { get; }
    TV? GetValue(TK key);
    bool TryGetValue(TK key, out TV? value);
    TV AddOrUpdate(TK key, TV value);
    bool Add(TK key, TV value);
    bool Remove(TK key);
    void Clear();
    bool ContainsKey(TK key);

    TV GetOrPut(TK key, Func<TK, TV> createFunc);

    event Action<IEnumerable<KeyValuePair<TK, TV>>> OnUpdate;
}
