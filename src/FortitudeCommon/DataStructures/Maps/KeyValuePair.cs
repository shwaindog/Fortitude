namespace FortitudeCommon.DataStructures.Maps
{
    public struct KeyValuePair<Tk, Tv>
    {
        public Tk Key;
        public Tv Value;
        public bool FoundItem;
        
        public KeyValuePair(bool foundItem)
        {
            Key = default(Tk);
            Value = default(Tv);
            FoundItem = foundItem;
        }
        public KeyValuePair(Tk key, Tv value, bool foundItem)
        {
            Key = key;
            Value = value;
            FoundItem = foundItem;
        }
        public KeyValuePair(System.Collections.Generic.KeyValuePair<Tk, Tv> dictionaryKvp)
        {
            Key = dictionaryKvp.Key;
            Value = dictionaryKvp.Value;
            FoundItem = true;
        }
    }
}