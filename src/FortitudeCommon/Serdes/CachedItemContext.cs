namespace FortitudeCommon.Serdes;

public interface ICachedItemContext<T> : ISerdeContext
{
    T? LastItem { get; set; }
}

public class CachedItemContext<T> : ICachedItemContext<T>
{
    public CachedItemContext(T existing) => LastItem = existing;
    public ContextDirection Direction => ContextDirection.Both;
    public virtual MarshalType MarshalType => MarshalType.Object | MarshalType.Cached;

    public T? LastItem { get; set; }
}
