namespace FortitudeCommon.DataStructures.Maps;

public class LinkedListCache<TK, TV> : LinkedListMap<TK, TV> where TK : notnull
{
    private IEnumerable<KeyValuePair<TK, TV>> values = Array.Empty<KeyValuePair<TK, TV>>();

    public LinkedListCache()
    {
        OnUpdate += v => { values = v.ToArray(); };
    }

    public override IEnumerator<KeyValuePair<TK, TV>> GetEnumerator() => values.GetEnumerator();
}
