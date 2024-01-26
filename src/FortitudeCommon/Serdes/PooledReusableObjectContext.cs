#region

using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeCommon.Serdes;

public interface IPooledRecyclableContext<T> : ICachedItemContext<T> where T : class, IReusableObject, new()
{
    IRecycler? Recycler { get; set; }
}

public class PooledReusableObjectContext<T> : CachedItemContext<T>, IPooledRecyclableContext<T>
    where T : class, IReusableObject, new()
{
    public PooledReusableObjectContext(T existing) : base(existing) { }
    public override MarshalType MarshalType => MarshalType.Object | MarshalType.Recycled;
    public IRecycler? Recycler { get; set; }
}
