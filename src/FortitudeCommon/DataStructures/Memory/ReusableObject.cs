#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.DataStructures.Memory;

public interface IReusableObject : IRecyclableObject, IStoreState
{
    void Reset();
}

public interface IReusableObject<T> : IReusableObject, IStoreState<T>, ICloneable<T>
    where T : class, IReusableObject<T> { }

public abstract class ReusableObject<T> : RecyclableObject, IReusableObject<T> where T : class, IReusableObject<T>
{
    public IStoreState CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((T)source, copyMergeFlags);

    public virtual void Reset()
    {
        refCount = 0;
    }

    public abstract T CopyFrom(T source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);

    object ICloneable.Clone() => Clone();

    public abstract T Clone();
}
