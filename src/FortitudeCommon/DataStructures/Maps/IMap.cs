namespace FortitudeCommon.DataStructures.Maps;

public interface IMap<TK, TV> : IEnumerable<KeyValuePair<TK, TV>> where TK : notnull
{
    TV this[TK key] { get; set; }
    int Count { get; }
    bool TryGetValue(TK key, out TV? value);
    void Add(TK key, TV value);
    bool Remove(TK key);
    void Clear();
    bool ContainsKey(TK key);

    event Action<IEnumerable<KeyValuePair<TK, TV>>> OnUpdate;
}
