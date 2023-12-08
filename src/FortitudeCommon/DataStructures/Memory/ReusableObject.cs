#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.DataStructures.Memory;

public interface IReusableObject : IRecyclableObject, IStoreState
{
    void Reset();
}

public interface IReusableObject<T> : IReusableObject, IStoreState<IReusableObject<T>>, ICloneable<T>, IStoreState<T>
    where T : class { }

public abstract class ReusableObject<T> : RecyclableObject, IReusableObject<T> where T : class
{
    public IStoreState CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags) =>
        (IStoreState)CopyFrom((T)source, copyMergeFlags);

    public IReusableObject<T> CopyFrom(IReusableObject<T> source, CopyMergeFlags copyMergeFlags) =>
        (IReusableObject<T>)CopyFrom((T)source, copyMergeFlags);

    public virtual void Reset()
    {
        refCount = 0;
    }

    public abstract T CopyFrom(T source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);

    object ICloneable.Clone() => Clone();

    public abstract T Clone();
}

public class ReusableObject : ReusableObject<ReusableObject>
{
    public override ReusableObject CopyFrom(ReusableObject source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        this;

    public override ReusableObject Clone() => Recycler?.Borrow<ReusableObject>() ?? new ReusableObject();
}
