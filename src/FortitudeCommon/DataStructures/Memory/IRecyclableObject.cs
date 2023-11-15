#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.DataStructures.Memory;

public interface IRecyclableObject : IStoreState
{
    int RefCount { get; }
    bool RecycleOnRefCountZero { get; set; }
    bool AutoRecycledByProducer { get; set; }
    bool IsInRecycler { get; set; }
    IRecycler? Recycler { get; set; }
    int DecrementRefCount();
    int IncrementRefCount();
    bool Recycle();
}

public interface IRecyclableObject<T> : IRecyclableObject, IStoreState<T> where T : class { }
