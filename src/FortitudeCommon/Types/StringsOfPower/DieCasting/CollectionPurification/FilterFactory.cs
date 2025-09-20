namespace FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;

public interface IFilterFactory
{
    Func<IKeyedCollectionFilter<TKey, TValue>> CreateKeyCollectionFilter<TKey, TValue>();
    
    Func<IOrderedCollectionFilter<TItem>> CreateOrderedCollectionResolver<TItem>();
}

public class AddAllFilterFactory : IFilterFactory
{
    public Func<IKeyedCollectionFilter<TKey, TValue>> CreateKeyCollectionFilter<TKey, TValue>() => 
        () => new KeyedCollectionFilter<TKey, TValue>().Initialize(CollectionFilterExtensions.ResolveAcceptAllKeyedCollectionItemsPredicate<TKey, TValue>());

    public Func<IOrderedCollectionFilter<TItem>> CreateOrderedCollectionResolver<TItem>() => 
        () => new OrderedCollectionFilter<TItem>().Initialize(CollectionFilterExtensions.ResolveAcceptAllOrderedCollectionItemsPredicate<TItem>());
    
    public static AddAllFilterFactory Instance = new ();
}

public class AddFirstFilterFactory(int topIncludeAmount) : IFilterFactory
{
    public Func<IKeyedCollectionFilter<TKey, TValue>> CreateKeyCollectionFilter<TKey, TValue>() => 
        () => new KeyedCollectionFilter<TKey, TValue>().Initialize(CollectionFilterExtensions.ResolveAcceptFirstKeyedCollectionItemsPredicate<TKey, TValue>(topIncludeAmount));

    public Func<IOrderedCollectionFilter<TItem>> CreateOrderedCollectionResolver<TItem>() => 
        () => new OrderedCollectionFilter<TItem>().Initialize(CollectionFilterExtensions.ResolveAcceptFirstOrderedCollectionItemsPredicate<TItem>(topIncludeAmount));
}

public class AddEvenRetrieveCountFactory: IFilterFactory
{
    public Func<IKeyedCollectionFilter<TKey, TValue>> CreateKeyCollectionFilter<TKey, TValue>() => 
        () => new KeyedCollectionFilter<TKey, TValue>().Initialize(CollectionFilterExtensions.ResolveSampleEvenKeyedCollectionItemsPredicate<TKey, TValue>());

    public Func<IOrderedCollectionFilter<TItem>> CreateOrderedCollectionResolver<TItem>() => 
        () => new OrderedCollectionFilter<TItem>().Initialize(CollectionFilterExtensions.ResolveSampleEvenOrderedCollectionItemsPredicate<TItem>());
    
    public static AddEvenRetrieveCountFactory Instance = new ();
}

public class AddOddRetrieveCountFactory: IFilterFactory
{
    public Func<IKeyedCollectionFilter<TKey, TValue>> CreateKeyCollectionFilter<TKey, TValue>() => 
        () => new KeyedCollectionFilter<TKey, TValue>().Initialize(CollectionFilterExtensions.ResolveSampleOddKeyedCollectionItemsPredicate<TKey, TValue>());

    public Func<IOrderedCollectionFilter<TItem>> CreateOrderedCollectionResolver<TItem>() => 
        () => new OrderedCollectionFilter<TItem>().Initialize(CollectionFilterExtensions.ResolveSampleOddOrderedCollectionItemsPredicate<TItem>());
    
    public static AddOddRetrieveCountFactory Instance = new ();
}