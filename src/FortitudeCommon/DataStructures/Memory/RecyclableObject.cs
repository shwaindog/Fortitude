// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeCommon.DataStructures.Memory;

public interface IUsesRecycler
{
    IRecycler? Recycler { get; set; }
}

public class UsesRecycler : IUsesRecycler
{
    public IRecycler? Recycler { get; set; }
}

public interface IRecyclableObject : IUsesRecycler
{
    bool AutoRecycleAtRefCountZero { get; set; }

    int  RefCount     { get; }
    bool IsInRecycler { get; set; }

    int  DecrementRefCount();
    int  IncrementRefCount();
    bool Recycle();
    void StateReset();
}

public class RecyclableObject : UsesRecycler, IRecyclableObject
{
    private const int NumRecentRecycleTimes = 5;

    private   int        isInRecycler;
    protected DateTime[] LastRecycleTimes = new DateTime[NumRecentRecycleTimes];
    protected int        RecycleCount;

    // ReSharper disable once InconsistentNaming
    protected int refCount = 1;

    public int  RefCount                  => refCount;
    public bool AutoRecycleAtRefCountZero { get; set; }

    public bool IsInRecycler
    {
        get => isInRecycler != 0;
        set => isInRecycler = value ? 1 : 0;
    }

    public virtual int DecrementRefCount()
    {
        if (!IsInRecycler && Interlocked.Decrement(ref refCount) <= 0 && AutoRecycleAtRefCountZero) Recycle();
        return refCount;
    }

    public virtual int IncrementRefCount() => Interlocked.Increment(ref refCount);

    public virtual bool Recycle()
    {
        if (IsInRecycler) return true;
        if (Recycler == null) return false;
        if (Interlocked.CompareExchange(ref isInRecycler, 1, 0) != 0) return false;

        LastRecycleTimes[RecycleCount % NumRecentRecycleTimes] = DateTime.UtcNow;
        Interlocked.Increment(ref RecycleCount);
        Recycler.Recycle(this);

        return true;
    }

    public virtual void StateReset()
    {
        refCount = 0;
    }
}

public interface IAutoRecycledObject : IRecyclableObject
{
    void EnableAutoRecycle();

    void DisableAutoRecycle();
}

public class AutoRecycledObject : RecyclableObject, IAutoRecycledObject
{
    public virtual void EnableAutoRecycle()
    {
        AutoRecycleAtRefCountZero = true;
    }

    public virtual void DisableAutoRecycle()
    {
        AutoRecycleAtRefCountZero = false;
    }
}
