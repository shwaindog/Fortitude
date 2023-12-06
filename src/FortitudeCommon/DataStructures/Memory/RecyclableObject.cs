namespace FortitudeCommon.DataStructures.Memory;

public interface IRecyclableObject
{
    int RefCount { get; }
    bool AutoRecycleAtRefCountZero { get; set; }
    bool IsInRecycler { get; set; }
    IRecycler? Recycler { get; set; }
    int DecrementRefCount();
    int IncrementRefCount();
    bool Recycle();
}

public class RecyclableObject : IRecyclableObject
{
    protected int refCount;
    public int RefCount => refCount;
    public bool AutoRecycleAtRefCountZero { get; set; } = true;
    public bool IsInRecycler { get; set; }
    public IRecycler? Recycler { get; set; }

    public virtual int DecrementRefCount()
    {
        if (Interlocked.Decrement(ref refCount) <= 0 && AutoRecycleAtRefCountZero) Recycle();
        return refCount;
    }

    public virtual int IncrementRefCount() => Interlocked.Increment(ref refCount);

    public virtual bool Recycle()
    {
        if (!IsInRecycler) Recycler?.Recycle(this);

        return IsInRecycler;
    }
}
