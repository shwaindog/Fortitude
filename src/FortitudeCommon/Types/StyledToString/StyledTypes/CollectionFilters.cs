
namespace FortitudeCommon.Types.StyledToString.StyledTypes;

public delegate bool KeyValuePredicate<in TKey, in TValue>(int retrieveCount, TKey key, TValue value);

public delegate bool OrderedCollectionPredicate<in TItem>(int retrieveCount, TItem item);