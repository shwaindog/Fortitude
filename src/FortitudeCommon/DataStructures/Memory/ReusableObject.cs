#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.DataStructures.Memory;

public interface IReusableObject : IRecyclableObject, IStoreState, ICloneable { }

public interface IReusableObject<T> : IReusableObject, IStoreState<IReusableObject<T>>, ICloneable<T>, IStoreState<T>
    where T : class { }

public abstract class ReusableObject<T> : RecyclableObject, IReusableObject<T> where T : class
{
    protected readonly int InstanceNum = InstanceCounter<T>.NextInstanceNum;

    public IStoreState CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags) =>
        (IStoreState)CopyFrom((T)source, copyMergeFlags);

    public IReusableObject<T> CopyFrom(IReusableObject<T> source, CopyMergeFlags copyMergeFlags) =>
        (IReusableObject<T>)CopyFrom((T)source, copyMergeFlags);

    public abstract T CopyFrom(T source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);

    object ICloneable.Clone() => Clone();

    public abstract T Clone();
}
