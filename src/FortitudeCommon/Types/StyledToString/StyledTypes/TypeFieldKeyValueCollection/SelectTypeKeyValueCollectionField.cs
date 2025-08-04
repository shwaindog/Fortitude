namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldKeyValueCollection;

public interface ISelectTypeKeyValueCollectionField<out T> where T : StyledTypeBuilder
{
    IAlwaysFieldIncludeAllKeyValues<T> FieldAlwaysAddAll { get; }

    INonNullFieldIncludeAllKeyValues<T> WhenNonNullAddAll { get; }

    IPopulatedFieldIncludeAllKeyValues<T> WhenPopulatedAddAll { get; }

    IAlwaysFieldIncludeFilteredKeyValues<T> FieldAlwaysAddFiltered { get; }

    INonNullFieldIncludeFilteredKeyValues<T> WhenNonNullAddFiltered { get; }

    IPopulatedFieldIncludeFilteredKeyValues<T> WhenPopulatedAddFiltered { get; }

    IAlwaysFieldIncludeSelectKeys<T> FieldAlwaysAddSelectKeys { get; }

    INonNullFieldIncludeSelectKeys<T> WhenNonNullAddSelectKeys { get; }

    IPopulatedFieldIncludeSelectKeys<T> WhenPopulatedAddSelectKeys { get; }
}

public class SelectTypeKeyValueCollectionField<TExt> : ISelectTypeKeyValueCollectionField<TExt> 
    where TExt : StyledTypeBuilder
{
    private IStyleTypeBuilderComponentAccess<TExt> stb = null!;

    private IAlwaysFieldIncludeAllKeyValues<TExt>?    alwaysIncludeAll;
    private INonNullFieldIncludeAllKeyValues<TExt>?   whenNonNullIncludeAll;
    private IPopulatedFieldIncludeAllKeyValues<TExt>? whenPopulatedIncludeAll;

    private IAlwaysFieldIncludeFilteredKeyValues<TExt>?    alwaysIncludeFiltered;
    private INonNullFieldIncludeFilteredKeyValues<TExt>?   whenNonNullIncludeFiltered;
    private IPopulatedFieldIncludeFilteredKeyValues<TExt>? whenPopulatedIncludeFiltered;

    private IAlwaysFieldIncludeSelectKeys<TExt>?    alwaysIncludeSelectKeys;
    private INonNullFieldIncludeSelectKeys<TExt>?   whenNonNullIncludeSelectKeys;
    private IPopulatedFieldIncludeSelectKeys<TExt>? whenPopulatedIncludeSelectKeys;
    
    
    public SelectTypeKeyValueCollectionField<TExt> Initialize(IStyleTypeBuilderComponentAccess<TExt> styleTypeBuilder)
    {
        stb = styleTypeBuilder;

        return this;
    }

    public IAlwaysFieldIncludeAllKeyValues<TExt> FieldAlwaysAddAll
    {
        get => alwaysIncludeAll ??= stb.Recycler.Borrow<AlwaysFieldIncludeAllKeyValues<TExt>>().Initialize(stb);
        protected set => alwaysIncludeAll = value;
    }

    public INonNullFieldIncludeAllKeyValues<TExt> WhenNonNullAddAll
    {
        get => whenNonNullIncludeAll ??= stb.Recycler.Borrow<NonNullFieldIncludeAllKeyValues<TExt>>().Initialize(stb, FieldAlwaysAddAll);
        protected set => whenNonNullIncludeAll = value;
    }

    public IPopulatedFieldIncludeAllKeyValues<TExt> WhenPopulatedAddAll
    {
        get => whenPopulatedIncludeAll ??= stb.Recycler.Borrow<PopulatedFieldIncludeAllKeyValues<TExt>>().Initialize(stb, FieldAlwaysAddAll);
        protected set => whenPopulatedIncludeAll = value;
    }
    
    public IAlwaysFieldIncludeFilteredKeyValues<TExt> FieldAlwaysAddFiltered
    {
        get => alwaysIncludeFiltered ??= stb.Recycler.Borrow<AlwaysFieldIncludeFilteredKeyValues<TExt>>().Initialize(stb);
        protected set => alwaysIncludeFiltered = value;
    }

    public INonNullFieldIncludeFilteredKeyValues<TExt> WhenNonNullAddFiltered
    {
        get => whenNonNullIncludeFiltered ??= stb.Recycler.Borrow<NonNullFieldIncludeFilteredKeyValues<TExt>>().Initialize(stb, FieldAlwaysAddFiltered);
        protected set => whenNonNullIncludeFiltered = value;
    }

    public IPopulatedFieldIncludeFilteredKeyValues<TExt> WhenPopulatedAddFiltered
    {
        get => whenPopulatedIncludeFiltered ??= stb.Recycler.Borrow<PopulatedFieldIncludeFilteredKeyValues<TExt>>().Initialize(stb);
        protected set => whenPopulatedIncludeFiltered = value;
    }
    
    public IAlwaysFieldIncludeSelectKeys<TExt> FieldAlwaysAddSelectKeys
    {
        get => alwaysIncludeSelectKeys ??= stb.Recycler.Borrow<AlwaysFieldIncludeSelectKeys<TExt>>().Initialize(stb);
        protected set => alwaysIncludeSelectKeys = value;
    }

    public INonNullFieldIncludeSelectKeys<TExt> WhenNonNullAddSelectKeys
    {
        get => whenNonNullIncludeSelectKeys ??= stb.Recycler.Borrow<NonNullFieldIncludeSelectKeys<TExt>>().Initialize(stb, FieldAlwaysAddSelectKeys);
        protected set => whenNonNullIncludeSelectKeys = value;
    }

    public IPopulatedFieldIncludeSelectKeys<TExt> WhenPopulatedAddSelectKeys
    {
        get => whenPopulatedIncludeSelectKeys ??= stb.Recycler.Borrow<PopulatedFieldIncludeSelectKeys<TExt>>().Initialize(stb, FieldAlwaysAddSelectKeys);
        protected set => whenPopulatedIncludeSelectKeys = value;
    }
}
