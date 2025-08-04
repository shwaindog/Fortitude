using FortitudeCommon.DataStructures.Memory;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldCollection;

public interface ISelectTypeCollectionField<out T> : IRecyclableObject where T : StyledTypeBuilder
{
    IAlwaysFieldIncludeAllCollection<T> AlwaysAddAll { get; }

    INonNullFieldIncludeAllCollection<T> WhenNonNullIncludeAll { get; }

    IPopulatedFieldIncludeAllCollection<T> WhenPopulatedIncludeAll { get; }

    IAlwaysFieldIncludeFilteredCollection<T> AlwaysAddFiltered { get; }

    INonNullFieldIncludeFilteredCollection<T> WhenNonNullIncludeFiltered { get; }

    IPopulatedFieldIncludeFilteredCollection<T> WhenPopulatedIncludeFiltered { get; }
}

public class SelectTypeCollectionField<TExt>: RecyclableObject, ISelectTypeCollectionField<TExt>
    where TExt : StyledTypeBuilder
{
    private IStyleTypeBuilderComponentAccess<TExt> stb = null!;

    
    public SelectTypeCollectionField<TExt> Initialize(IStyleTypeBuilderComponentAccess<TExt> styleTypeBuilder)
    {
        stb = styleTypeBuilder;

        return this;
    }

    private IAlwaysFieldIncludeAllCollection<TExt>?    alwaysIncludeAll;
    private INonNullFieldIncludeAllCollection<TExt>?   whenNonNullIncludeAll;
    private IPopulatedFieldIncludeAllCollection<TExt>? whenPopulatedIncludeAll;

    private IAlwaysFieldIncludeFilteredCollection<TExt>?    alwaysIncludeFiltered;
    private INonNullFieldIncludeFilteredCollection<TExt>?   whenNonNullIncludeFiltered;
    private IPopulatedFieldIncludeFilteredCollection<TExt>? whenPopulatedIncludeFiltered;

    public IAlwaysFieldIncludeAllCollection<TExt> AlwaysAddAll
    {
        get => alwaysIncludeAll  ??= stb.Recycler.Borrow<AlwaysFieldIncludeAllCollection<TExt>>().Initialize(stb);
        protected set => alwaysIncludeAll = value;
    }

    public INonNullFieldIncludeAllCollection<TExt> WhenNonNullIncludeAll
    {
        get => whenNonNullIncludeAll ??= stb.Recycler.Borrow<NonNullFieldIncludeAllCollection<TExt>>().Initialize(stb, AlwaysAddAll);
        protected set => whenNonNullIncludeAll = value;
    }

    public IPopulatedFieldIncludeAllCollection<TExt> WhenPopulatedIncludeAll
    {
        get => whenPopulatedIncludeAll ??= stb.Recycler.Borrow<PopulatedFieldIncludeAllCollection<TExt>>().Initialize(stb, AlwaysAddAll);
        protected set => whenPopulatedIncludeAll = value;
    }

    public IAlwaysFieldIncludeFilteredCollection<TExt> AlwaysAddFiltered
    {
        get => alwaysIncludeFiltered  ??= stb.Recycler.Borrow<AlwaysFieldIncludeFilteredCollection<TExt>>().Initialize(stb);
        protected set => alwaysIncludeFiltered = value;
    }

    public INonNullFieldIncludeFilteredCollection<TExt> WhenNonNullIncludeFiltered
    {
        get => whenNonNullIncludeFiltered ??= stb.Recycler.Borrow<NonNullFieldIncludeFilteredCollection<TExt>>().Initialize(stb, AlwaysAddFiltered);
        protected set => whenNonNullIncludeFiltered = value;
    }

    public IPopulatedFieldIncludeFilteredCollection<TExt> WhenPopulatedIncludeFiltered
    {
        get => whenPopulatedIncludeFiltered ??= stb.Recycler.Borrow<PopulatedFieldIncludeFilteredCollection<TExt>>().Initialize(stb);
        protected set => whenPopulatedIncludeFiltered = value;
    }
}
