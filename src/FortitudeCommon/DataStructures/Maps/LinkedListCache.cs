using FortitudeCommon.DataStructures.Lists;

namespace FortitudeCommon.DataStructures.Maps;

public class LinkedListCache<TK, TV> : LinkedListMap<TK, TV> where TK : notnull
{
    private object syncLock = new object();
    private ReusableList<KeyValuePair<TK, TV>> keyValues = new ();

    public LinkedListCache()
    {
        Updated += (key, value, action, map) =>
        {
            lock (syncLock)
            {
                keyValues.Clear();
                foreach (var kvp in map)
                {
                    keyValues.Add(kvp);
                }    
            }
        };
    }

    public override IEnumerator<KeyValuePair<TK, TV>> GetEnumerator()
    {
        lock (syncLock)
        {
            return keyValues.GetEnumerator();
        }
    }
    
}
