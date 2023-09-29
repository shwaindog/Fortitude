using FortitudeCommon.Extensions;

namespace FortitudeCommon.Configuration.KeyValueProperties
{
    public struct CompositeKey
    {

        public CompositeKey(string key) : this()
        {
            Key = key;
        }

        public CompositeKey(string key, string subKey) : this()
        {
            Key = key;
            SubKey = subKey;
        }

        public CompositeKey(string key, string subKey, string subKeyModifier) : this()
        {
            Key = key;
            SubKey = subKey;
            SubKeyModifier = subKeyModifier;
        }

        public string Key { get; }
        public string SubKey { get; }
        public string SubKeyModifier { get; }

        public bool MatchesQueryKey(CompositeKey queryCompositeKey)
        {
            if (queryCompositeKey.Key == null) return false;
            return Key.MatchesPatternQuery(queryCompositeKey.Key) &&
                   SubKey.MatchesPatternQuery(queryCompositeKey.SubKey) &&
                   SubKeyModifier.MatchesPatternQuery(queryCompositeKey.SubKeyModifier);

        }

        public bool Equals(CompositeKey other)
        {
            return string.Equals(Key, other.Key) && string.Equals(SubKey, other.SubKey) &&
                   string.Equals(SubKeyModifier, other.SubKeyModifier);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is CompositeKey key && Equals(key);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Key != null ? Key.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (SubKey != null ? SubKey.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (SubKeyModifier != null ? SubKeyModifier.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}