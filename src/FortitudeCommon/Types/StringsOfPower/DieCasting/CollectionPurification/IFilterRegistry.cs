using System.Collections.Concurrent;
using FortitudeCommon.DataStructures.MemoryPools;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;


public interface IFilterRegistry
{
    IKeyedCollectionFilter<TKey, TValue> ResolveKeyedCollectionFilterOrDefault<TKey,TValue>(KeyValuePair<TKey, TValue>[] forArray);
    IKeyedCollectionFilter<TKey, TValue> ResolveKeyedCollectionFilterOrDefault<TKey,TValue>(IReadOnlyDictionary<TKey, TValue> forDictionary);
    IKeyedCollectionFilter<TKey, TValue> ResolveKeyedCollectionFilterOrDefault<TKey,TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>> forList);
    IKeyedCollectionFilter<TKey, TValue> ResolveKeyedCollectionFilterOrDefault<TKey,TValue>(IEnumerable<KeyValuePair<TKey, TValue>> forEnumerable);
    IKeyedCollectionFilter<TKey, TValue> ResolveKeyedCollectionFilterOrDefault<TKey,TValue>(IEnumerator<KeyValuePair<TKey, TValue>> forEnumerator);
    IKeyedCollectionFilter<TKey, TValue> ResolveKeyedCollectionFilterOrDefault<TKey,TValue>();
    
    void RegisterKeyedCollectionFilter<TKey,TValue>(Func<IKeyedCollectionFilter<TKey, TValue>> filterFactory);
    void RegisterOrderedCollectionFilter<TItem>(Func<IOrderedCollectionFilter<TItem>> filterFactory);
    
    IOrderedCollectionFilter<TItem> OrderedCollectionFilterDefault<TItem>(ReadOnlySpan<TItem> forList);

    IOrderedCollectionFilter<TItem?> OrderedCollectionFilterDefault<TItem>(TItem[] forList);
    IOrderedCollectionFilter<TItem?> OrderedCollectionFilterDefault<TItem>(TItem?[] forList) where TItem : struct;
    IOrderedCollectionFilter<TItem> OrderedCollectionFilterDefault<TItem>(IReadOnlyList<TItem> forList);
    IOrderedCollectionFilter<TItem> OrderedCollectionFilterDefault<TItem>(IEnumerable<TItem> forEnumerable);
    IOrderedCollectionFilter<TItem> OrderedCollectionFilterDefault<TItem>(IEnumerator<TItem> forEnumerator);
    IOrderedCollectionFilter<TItem> OrderedCollectionFilterDefault<TItem>();
}

public class FilterRegistry : IFilterRegistry
{
    private IFilterFactory defaultFilterFactory;

    public FilterRegistry(IFilterFactory defaultFilterFactory) => this.defaultFilterFactory = defaultFilterFactory;

    private Recycler filterRecycler = new Recycler();
    
    public IKeyedCollectionFilter<TKey, TValue> ResolveKeyedCollectionFilterOrDefault<TKey, TValue>(KeyValuePair<TKey, TValue>[] forArray) => 
        ResolveKeyedCollectionFilterOrDefault<TKey, TValue>();

    public IKeyedCollectionFilter<TKey, TValue> ResolveKeyedCollectionFilterOrDefault<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> forDictionary) =>  
        ResolveKeyedCollectionFilterOrDefault<TKey, TValue>();

    public IKeyedCollectionFilter<TKey, TValue> ResolveKeyedCollectionFilterOrDefault<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>> forList) =>  
        ResolveKeyedCollectionFilterOrDefault<TKey, TValue>();

    public IKeyedCollectionFilter<TKey, TValue> ResolveKeyedCollectionFilterOrDefault<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> forEnumerable) =>  
        ResolveKeyedCollectionFilterOrDefault<TKey, TValue>();

    public IKeyedCollectionFilter<TKey, TValue> ResolveKeyedCollectionFilterOrDefault<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>> forEnumerator) =>  
        ResolveKeyedCollectionFilterOrDefault<TKey, TValue>();

    public IKeyedCollectionFilter<TKey, TValue> ResolveKeyedCollectionFilterOrDefault<TKey, TValue>()
    {
        var filterType = typeof(IKeyedCollectionFilter<TKey, TValue>);
        
        if (!filterRecycler.HasFactory(filterType))
        {
            filterRecycler.RegisterFactory(defaultFilterFactory.CreateKeyCollectionFilter<TKey, TValue>());
        }
        return (IKeyedCollectionFilter<TKey, TValue>)filterRecycler.Borrow(filterType);
    }

    public void RegisterKeyedCollectionFilter<TKey, TValue>(Func<IKeyedCollectionFilter<TKey, TValue>> filterFactory)
    {
        filterRecycler.ReleasePooledItemsWithNewFactory(filterFactory);
    }
    
    public void RegisterOrderedCollectionFilter<TItem>(Func<IOrderedCollectionFilter<TItem>> filterFactory)
    {
        filterRecycler.ReleasePooledItemsWithNewFactory(filterFactory);
    }
    
    public IOrderedCollectionFilter<TItem> OrderedCollectionFilterDefault<TItem>(ReadOnlySpan<TItem> forList) =>  
        OrderedCollectionFilterDefault<TItem>();

    public IOrderedCollectionFilter<TItem?> OrderedCollectionFilterDefault<TItem>(TItem[] forList) => OrderedCollectionFilterDefault<TItem?>();

    public IOrderedCollectionFilter<TItem?> OrderedCollectionFilterDefault<TItem>(TItem?[] forList) where TItem : struct => 
        OrderedCollectionFilterDefault<TItem?>();

    public IOrderedCollectionFilter<TItem> OrderedCollectionFilterDefault<TItem>(IReadOnlyList<TItem> forList) => 
        OrderedCollectionFilterDefault<TItem>();

    public IOrderedCollectionFilter<TItem> OrderedCollectionFilterDefault<TItem>(IEnumerable<TItem> forEnumerable) => 
        OrderedCollectionFilterDefault<TItem>();

    public IOrderedCollectionFilter<TItem> OrderedCollectionFilterDefault<TItem>(IEnumerator<TItem> forEnumerator) => 
        OrderedCollectionFilterDefault<TItem>();

    public IOrderedCollectionFilter<TItem> OrderedCollectionFilterDefault<TItem>() 
    {
        var orderedCollectionKey = typeof(TItem);
        if (!filterRecycler.HasFactory(orderedCollectionKey))
        {
            filterRecycler.RegisterFactory(defaultFilterFactory.CreateOrderedCollectionResolver<TItem>());
        }
        return (IOrderedCollectionFilter<TItem>)filterRecycler.Borrow(orderedCollectionKey);
    }

}