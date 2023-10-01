namespace FortitudeCommon.DataStructures.Maps;

public class ConcurrentCache<TK, TV> : ConcurrentMap<TK, TV> where TK : notnull
{
    private IEnumerable<KeyValuePair<TK, TV>> values = Array.Empty<KeyValuePair<TK, TV>>();

    public ConcurrentCache()
    {
        OnUpdate += v => { values = v.ToArray(); };
    }

    public override IEnumerator<KeyValuePair<TK, TV>> GetEnumerator() => values.GetEnumerator();
}
