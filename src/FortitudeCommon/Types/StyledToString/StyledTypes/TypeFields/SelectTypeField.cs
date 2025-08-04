using FortitudeCommon.DataStructures.Memory;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFields;

public interface ISelectTypeField<out T> : IRecyclableObject, IDisposable where T : StyledTypeBuilder
{
    IAlwaysIncludeField<T> AlwaysInclude { get; }

    INonDefaultField<T> WhenNonDefault { get; }

    INonNullField<T> WhenNonNull { get; }

    INonNullOrDefaultField<T> WhenNonNullOrDefault { get; }
}

public class SelectTypeField<TExt> : RecyclableObject, ISelectTypeField<TExt>  
    where TExt : StyledTypeBuilder
{
    private IStyleTypeBuilderComponentAccess<TExt> stb = null!;

    private IAlwaysIncludeField<TExt>?     alwaysInclude;
    private INonDefaultField<TExt>? whenNonDefault;
    private INonNullField<TExt>?    whenNonNull;

    private INonNullOrDefaultField<TExt>? whenNonNullOrDefault;

    public SelectTypeField<TExt> Initialize(IStyleTypeBuilderComponentAccess<TExt> styleTypeBuilder)
    {
        stb = styleTypeBuilder;

        return this;
    }

    public IAlwaysIncludeField<TExt> AlwaysInclude
    {
        get => alwaysInclude ??= stb.Recycler.Borrow<AlwaysIncludeField<TExt>>().Initialize(stb);
        protected set => alwaysInclude = value;
    }

    public INonDefaultField<TExt> WhenNonDefault
    {
        get => whenNonDefault ??= stb.Recycler.Borrow<NonDefaultField<TExt>>().Initialize(stb, AlwaysInclude);
        protected set => whenNonDefault = value;
    }

    public INonNullField<TExt> WhenNonNull
    {
        get => whenNonNull ??= stb.Recycler.Borrow<NonNullField<TExt>>().Initialize(stb, AlwaysInclude);
        protected set => whenNonNull = value;
    }

    public INonNullOrDefaultField<TExt> WhenNonNullOrDefault
    {
        get => whenNonNullOrDefault ??= stb.Recycler.Borrow<NonNullOrDefaultField<TExt>>().Initialize(stb, AlwaysInclude);
        protected set => whenNonNullOrDefault = value;
    }

    public void Dispose()
    {
        alwaysInclude?.DecrementRefCount();
        alwaysInclude = null;
        whenNonDefault?.DecrementRefCount();
        whenNonDefault = null;
        whenNonNull?.DecrementRefCount();
        whenNonNull = null;
        whenNonNullOrDefault?.DecrementRefCount();
        whenNonNullOrDefault = null;
    }
}
