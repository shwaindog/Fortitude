using FortitudeCommon.DataStructures.Memory;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;

public interface IOrderedCollectionFilter : ICollectionFilter
{
    Type ItemType { get; }
}

public interface IOrderedCollectionFilter<TItem> : IRecyclableObject, IOrderedCollectionFilter
{
    void Initialize(ReadOnlySpan<TItem> forArray);
    void Initialize(TItem[] forArray);
    void Initialize(IReadOnlyList<TItem> forList);
    void Initialize(IEnumerable<TItem> forEnumerable);
    void Initialize(IEnumerator<TItem> forEnumerator);

    List<int>? RestrictedRetrieveIndexes { get; }

    CollectionItemResult CheckIncludeAndContinue(int retrieveCount, TItem item);

    OrderedCollectionPredicate<TItem?> CheckPredicate { get; }

    IReadOnlyList<TItem> AdditionalIncludes();
}

public class OrderedCollectionFilter<TItem> : RecyclableObject, IOrderedCollectionFilter<TItem>
{
    private static readonly IReadOnlyList<TItem> EmptyList = new List<TItem>().AsReadOnly();

    private OrderedCollectionPredicate<TItem> itemPredicate;

    public OrderedCollectionFilter<TItem> Initialize(OrderedCollectionPredicate<TItem> predicate)
    {
        itemPredicate = predicate;

        return this;
    }

    public Type ItemType { get; } = typeof(TItem);

    public int? TotalItems { get; protected set; }

    public int ProcessedItems { get; protected set; }
    public int SkippedItems { get; protected set; }
    public int IncludedItems { get; protected set; }

    public void Initialize(ReadOnlySpan<TItem> forArray)
    {
        TotalItems     = forArray.Length;
        ProcessedItems = 0;
        SkippedItems   = 0;
        IncludedItems  = 0;
    }

    public void Initialize(TItem[] forArray)
    {
        TotalItems     = forArray.Length;
        ProcessedItems = 0;
        SkippedItems   = 0;
        IncludedItems  = 0;
    }

    public void Initialize(IReadOnlyList<TItem> forList)
    {
        TotalItems     = forList.Count;
        ProcessedItems = 0;
        SkippedItems   = 0;
        IncludedItems  = 0;
    }

    public void Initialize(IEnumerable<TItem> forEnumerable)
    {
        ProcessedItems = 0;
        SkippedItems   = 0;
        IncludedItems  = 0;
    }

    public void Initialize(IEnumerator<TItem> forEnumerator)
    {
        ProcessedItems = 0;
        SkippedItems   = 0;
        IncludedItems  = 0;
    }

    public virtual List<int>? RestrictedRetrieveIndexes { get; protected set; }

    public virtual CollectionItemResult CheckIncludeAndContinue(int retrieveCount, TItem item)
    {
        var result = itemPredicate(retrieveCount, item);
        ProcessedItems++;
        SkippedItems  += result.IncludeItem ? 0 : 1;
        IncludedItems += result.IncludeItem ? 1 : 0;
        return result;
    }

    public OrderedCollectionPredicate<TItem?> CheckPredicate => itemPredicate!;

    public IReadOnlyList<TItem> AdditionalIncludes() => EmptyList;
}
