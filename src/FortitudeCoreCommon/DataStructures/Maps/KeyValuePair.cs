namespace FortitudeCommon.DataStructures.Maps;

public struct KeyValuePair<TK, TV> where TK : notnull
{
    public TK Key;
    public TV Value;
    public bool FoundItem;

    public KeyValuePair(TK key, TV value, bool foundItem)
    {
        Key = key;
        Value = value;
        FoundItem = foundItem;
    }

    public KeyValuePair(System.Collections.Generic.KeyValuePair<TK, TV> dictionaryKvp)
    {
        Key = dictionaryKvp.Key;
        Value = dictionaryKvp.Value;
        FoundItem = true;
    }
}
