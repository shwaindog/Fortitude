using FortitudeCommon.DataStructures.Memory;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;

public interface IKeyedCollectionFilter : ICollectionFilter
{
    Type KeyType { get; }
    Type ValueType { get; }
}

public interface IKeyedCollectionFilter<TKey, TValue> : IRecyclableObject, IKeyedCollectionFilter
{
    void Initialize(KeyValuePair<TKey, TValue>[] forArray);
    void Initialize(IReadOnlyDictionary<TKey, TValue> forList);
    void Initialize(IReadOnlyList<KeyValuePair<TKey, TValue>> forList);
    void Initialize(IEnumerable<KeyValuePair<TKey, TValue>> forList);
    void Initialize(IEnumerator<KeyValuePair<TKey, TValue>> forList);
    
    List<TKey>? RestrictedKeyList { get; }

    CollectionItemResult CheckIncludeAndContinue(int retrieveCount, TKey key, TValue value);
    
    KeyValuePredicate<TKey, TValue> CheckPredicate { get; }
    
    IReadOnlyList<KeyValuePair<TKey, TValue>> AdditionalIncludes();
}

public class KeyedCollectionFilter<TKey, TValue> : RecyclableObject, IKeyedCollectionFilter<TKey, TValue>
{
    private static readonly IReadOnlyList<KeyValuePair<TKey, TValue>> EmptyList = new List<KeyValuePair<TKey, TValue>>().AsReadOnly();

    private KeyValuePredicate<TKey, TValue> kvpPredicate = null!;
    
    public KeyedCollectionFilter<TKey, TValue> Initialize(KeyValuePredicate<TKey, TValue> predicate)
    {
        kvpPredicate = predicate;

        return this;
    }
    
    public int? TotalItems { get; protected set; }
    public int ProcessedItems { get; protected set; }
    public int SkippedItems { get; protected set; }
    public int IncludedItems { get; protected set; }

    public Type KeyType { get; } = typeof(TKey);
    
    public Type ValueType { get; } = typeof(TValue);
    
    public void Initialize(KeyValuePair<TKey, TValue>[] forArray) 
    {
        TotalItems     = forArray.Length;
        ProcessedItems = 0;
        SkippedItems   = 0;
        IncludedItems  = 0;
    }

    public void Initialize(IReadOnlyDictionary<TKey, TValue> forList) 
    {
        TotalItems     = forList.Count;
        ProcessedItems = 0;
        SkippedItems   = 0;
        IncludedItems  = 0;
    }

    public void Initialize(IReadOnlyList<KeyValuePair<TKey, TValue>> forList) 
    {
        TotalItems     = forList.Count;
        ProcessedItems = 0;
        SkippedItems   = 0;
        IncludedItems  = 0;
    }

    public void Initialize(IEnumerable<KeyValuePair<TKey, TValue>> forList) 
    {
        ProcessedItems = 0;
        SkippedItems   = 0;
        IncludedItems  = 0;
    }

    public void Initialize(IEnumerator<KeyValuePair<TKey, TValue>> forList) 
    {
        ProcessedItems = 0;
        SkippedItems   = 0;
        IncludedItems  = 0;
    }

    public virtual List<TKey>? RestrictedKeyList  => null;
    
    public virtual CollectionItemResult CheckIncludeAndContinue(int retrieveCount, TKey key, TValue value) 
    {
        var result = kvpPredicate(retrieveCount, key, value);
        ProcessedItems++;
        SkippedItems  += result.IncludeItem ? 0 : 1;
        IncludedItems += result.IncludeItem ? 1 : 0;
        return result;
    }

    public KeyValuePredicate<TKey, TValue> CheckPredicate => kvpPredicate;
    
    public virtual IReadOnlyList<KeyValuePair<TKey, TValue>> AdditionalIncludes() => EmptyList;
}
