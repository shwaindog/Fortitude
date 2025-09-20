using System.Collections.Concurrent;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;

public delegate CollectionItemResult KeyValuePredicate<in TKey, in TValue>(int retrieveCount, TKey key, TValue value);

public delegate CollectionItemResult OrderedCollectionPredicate<in TItem>(int retrieveCount, TItem item);

public static class CollectionFilterExtensions
{
    private static ConcurrentDictionary<(Type, Type), Delegate>      acceptAllKeyedCollectionPredicates = new();
    private static ConcurrentDictionary<(Type, Type, int), Delegate> topXKeyedCollectionPredicates      = new();
    private static ConcurrentDictionary<(Type, Type, int, int), Delegate> sampleKeyedCollectionPredicates   = new();
    
    private static ConcurrentDictionary<Type, Delegate>      acceptAllOrderedCollectionPredicates = new();
    private static ConcurrentDictionary<(Type, int), Delegate> topXOrderedCollectionPredicates      = new();
    private static ConcurrentDictionary<(Type, int, int), Delegate> sampleOrderedCollectionPredicates   = new();


    public static KeyValuePredicate<TKey, TValue> ResolveAcceptAllKeyedCollectionItemsPredicate<TKey, TValue>()
    {
        var keyValueLookupKey = (typeof(TKey), typeof(TValue));
        if (!acceptAllKeyedCollectionPredicates.TryGetValue(keyValueLookupKey, out var predicateDelegate))
        {
            predicateDelegate = AcceptAllKeyedCollectionPredicate<TKey, TValue>;
            acceptAllKeyedCollectionPredicates.TryAdd(keyValueLookupKey, predicateDelegate);
        }
        
        return (KeyValuePredicate<TKey, TValue>)predicateDelegate;
    }
    
    public static KeyValuePredicate<TKey, TValue> ResolveAcceptFirstKeyedCollectionItemsPredicate<TKey, TValue>(int firstAmount)
    {
        var keyValueLookupKey = (typeof(TKey), typeof(TValue), firstAmount);
        if (!topXKeyedCollectionPredicates.TryGetValue(keyValueLookupKey, out var predicateDelegate))
        {
            predicateDelegate = AcceptFirst<TKey, TValue>(firstAmount);
            topXKeyedCollectionPredicates.TryAdd(keyValueLookupKey, predicateDelegate);
        }
        
        return (KeyValuePredicate<TKey, TValue>)predicateDelegate;
    }
    
    public static KeyValuePredicate<TKey, TValue> ResolveSampleKeyedCollectionItemsPredicate<TKey, TValue>(int sampleCount, int startOffset = 0)
    {
        var keyValueLookupKey = (typeof(TKey), typeof(TValue), sampleCount, startOffset);
        if (!sampleKeyedCollectionPredicates.TryGetValue(keyValueLookupKey, out var predicateDelegate))
        {
            predicateDelegate = SampleEveryRetrievedItemCount<TKey, TValue>(sampleCount, startOffset);
            sampleKeyedCollectionPredicates.TryAdd(keyValueLookupKey, predicateDelegate);
        }
        
        return (KeyValuePredicate<TKey, TValue>)predicateDelegate;
    }
    
    public static KeyValuePredicate<TKey, TValue> ResolveSampleEvenKeyedCollectionItemsPredicate<TKey, TValue>() => 
        ResolveSampleKeyedCollectionItemsPredicate<TKey, TValue>(2);
    
    public static KeyValuePredicate<TKey, TValue> ResolveSampleOddKeyedCollectionItemsPredicate<TKey, TValue>() => 
        ResolveSampleKeyedCollectionItemsPredicate<TKey, TValue>(2, 1);

    private static CollectionItemResult AcceptAllKeyedCollectionPredicate<TKey, TValue>(int retrieveCount, TKey key, TValue value) =>
        CollectionItemResult.IncludeContinueToNext;

    private static KeyValuePredicate<TKey, TValue> AcceptFirst<TKey, TValue>(int firstItemsToAccept)
    {
        return (retrieveCount, key, value) =>
        {
            if (retrieveCount < firstItemsToAccept) return CollectionItemResult.IncludeContinueToNext;
            return CollectionItemResult.DoNotIncludeAndComplete;
        };
    }

    private static KeyValuePredicate<TKey, TValue> SampleEveryRetrievedItemCount<TKey, TValue>(int sampleCount, int startOffset = 0)
    {
        if (sampleCount <= 1) return AcceptAllKeyedCollectionPredicate;
        return (retrieveCount, key, value) =>
        {
            if ((retrieveCount + startOffset) % sampleCount == 0) return CollectionItemResult.IncludeAndSkipNext(sampleCount - 1);
            return CollectionItemResult.DoNotIncludeAndGoToNext;
        };
    }
    
    
    public static OrderedCollectionPredicate<TItem> ResolveAcceptAllOrderedCollectionItemsPredicate<TItem>()
    {
        var collectionKey = typeof(TItem);
        if (!acceptAllOrderedCollectionPredicates.TryGetValue(collectionKey, out var predicateDelegate))
        {
            predicateDelegate = AcceptAllOrderedCollectionPredicate<TItem>;
            acceptAllOrderedCollectionPredicates.TryAdd(collectionKey, predicateDelegate);
        }
        
        return (OrderedCollectionPredicate<TItem>)predicateDelegate;
    }
    
    public static OrderedCollectionPredicate<TItem> ResolveAcceptFirstOrderedCollectionItemsPredicate<TItem>(int firstAmount)
    {
        var collectionKey = (typeof(TItem), firstAmount);
        if (!topXOrderedCollectionPredicates.TryGetValue(collectionKey, out var predicateDelegate))
        {
            predicateDelegate = AcceptFirst<TItem>(firstAmount);
            topXOrderedCollectionPredicates.TryAdd(collectionKey, predicateDelegate);
        }
        
        return (OrderedCollectionPredicate<TItem>)predicateDelegate;
    }
    
    public static OrderedCollectionPredicate<TItem> ResolveSampleOrderedCollectionItemsPredicate<TItem>(int sampleCount, int startOffset = 0)
    {
        var keyValueLookupKey = (typeof(TItem), sampleCount, startOffset);
        if (!sampleOrderedCollectionPredicates.TryGetValue(keyValueLookupKey, out var predicateDelegate))
        {
            predicateDelegate = SampleEveryRetrievedItemCount<TItem>(sampleCount, startOffset);
            sampleOrderedCollectionPredicates.TryAdd(keyValueLookupKey, predicateDelegate);
        }
        
        return (OrderedCollectionPredicate<TItem>)predicateDelegate;
    }
    
    public static OrderedCollectionPredicate<TItem> ResolveSampleEvenOrderedCollectionItemsPredicate<TItem>() => 
        ResolveSampleOrderedCollectionItemsPredicate<TItem>(2);
    
    public static OrderedCollectionPredicate<TItem> ResolveSampleOddOrderedCollectionItemsPredicate<TItem>() => 
        ResolveSampleOrderedCollectionItemsPredicate<TItem>(2, 1);

    private static CollectionItemResult AcceptAllOrderedCollectionPredicate<TItem>(int retrieveCount, TItem item) =>
        CollectionItemResult.IncludeContinueToNext;

    private static OrderedCollectionPredicate<TItem> AcceptFirst<TItem>(int firstItemsToAccept)
    {
        return (retrieveCount, item) =>
        {
            if (retrieveCount < firstItemsToAccept) return CollectionItemResult.IncludeContinueToNext;
            return CollectionItemResult.DoNotIncludeAndComplete;
        };
    }

    private static OrderedCollectionPredicate<TItem> SampleEveryRetrievedItemCount<TItem>(int sampleCount, int startOffset = 0)
    {
        if (sampleCount <= 1) return AcceptAllOrderedCollectionPredicate;
        return (retrieveCount, item) =>
        {
            if ((retrieveCount + startOffset) % sampleCount == 0) return CollectionItemResult.IncludeAndSkipNext(sampleCount - 1);
            return CollectionItemResult.DoNotIncludeAndGoToNext;
        };
    }
}

public interface ICollectionFilter
{
    int? TotalItems { get; }
    int ProcessedItems { get; }
    int SkippedItems { get; }
    int IncludedItems { get; }
}
