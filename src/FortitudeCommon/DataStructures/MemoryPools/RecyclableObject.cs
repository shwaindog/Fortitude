// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;

#endregion

namespace FortitudeCommon.DataStructures.MemoryPools;

public interface IUsesRecycler
{
    [JsonIgnore] IRecycler? Recycler { get; set; }
}

public class UsesRecycler : IUsesRecycler
{
    [JsonIgnore] public virtual IRecycler? Recycler { get; set; }

    protected IRecycler AlwaysRecycler => Recycler ?? StaticAlwaysRecycler;

    protected static IRecycler StaticAlwaysRecycler => DataStructures.MemoryPools.Recycler.ThreadStaticRecycler;
}

public class ExplicitUsesRecycler : IUsesRecycler
{
    [JsonIgnore] IRecycler? IUsesRecycler.Recycler { get; set; }
    
    protected IRecycler AlwaysRecycler => ((IUsesRecycler)this).Recycler ?? Recycler.ThreadStaticRecycler;
}

public interface IResetable
{
    void StateReset();
}

public interface IRecyclableObject : IUsesRecycler, IResetable
{
    [JsonIgnore] bool AutoRecycleAtRefCountZero { get; set; }

    [JsonIgnore] int  RefCount     { get; }
    [JsonIgnore] bool IsInRecycler { get; set; }

    int  DecrementRefCount();
    int  NonRecyclingDecrementRefCount();
    int  IncrementRefCount();
    bool Recycle();
}

public class RecyclableObject : UsesRecycler, IRecyclableObject
{
    private const int NumRecentRecycleTimes = 5;

    private int isInRecycler;
    #if DEBUG
    protected DateTime[] LastRecycleTimes = new DateTime[NumRecentRecycleTimes];
    #endif
    protected int RecycleCount;

    // ReSharper disable once InconsistentNaming
    protected int refCount = 1;

    [JsonIgnore] public int RefCount => refCount;

    [JsonIgnore] public bool AutoRecycleAtRefCountZero { get; set; }

    [JsonIgnore]
    public bool IsInRecycler
    {
        get => isInRecycler != 0;
        set => isInRecycler = value ? 1 : 0;
    }

    public virtual int DecrementRefCount()
    {
        var count = 0;
        if (!IsInRecycler && (count = Interlocked.Decrement(ref refCount)) <= 0 && AutoRecycleAtRefCountZero) Recycle();
        return count;
    }

    public virtual int NonRecyclingDecrementRefCount()
    {
        var wouldHaveBeenCount = 0;
        if (!IsInRecycler && (wouldHaveBeenCount = Interlocked.Decrement(ref refCount)) <= 0)
        {
            Interlocked.Increment(ref refCount);
        }
        return wouldHaveBeenCount;
    }

    public virtual int IncrementRefCount() => Interlocked.Increment(ref refCount);

    public virtual bool Recycle()
    {
        if (IsInRecycler) return true;
        if (Recycler == null) return false;
        if (Interlocked.CompareExchange(ref isInRecycler, 1, 0) != 0) return false;

        #if DEBUG
        LastRecycleTimes[RecycleCount % NumRecentRecycleTimes] = DateTime.UtcNow;
        #endif
        Interlocked.Increment(ref RecycleCount);
        Recycler.Recycle(this);

        return true;
    }

    public virtual void StateReset()
    {
        refCount = 1;
    }
}

public class ExplicitRecyclableObject : ExplicitUsesRecycler, IRecyclableObject
{
    private const int NumRecentRecycleTimes = 5;

    private int isInRecycler;
    #if DEBUG
    protected DateTime[] LastRecycleTimes = new DateTime[NumRecentRecycleTimes];
    #endif
    protected int RecycleCount;

    // ReSharper disable once InconsistentNaming
    protected int refCount = 1;

    [JsonIgnore] int IRecyclableObject.RefCount => refCount;

    [JsonIgnore] bool IRecyclableObject.AutoRecycleAtRefCountZero { get; set; }

    [JsonIgnore]
    bool IRecyclableObject.IsInRecycler
    {
        get => isInRecycler != 0;
        set => isInRecycler = value ? 1 : 0;
    }

    protected IRecyclableObject MeRecyclable => this;

    int IRecyclableObject.DecrementRefCount()
    {
        return ExplicitDecrementRefCount();
    }

    protected virtual int ExplicitDecrementRefCount()
    {
        var count = 0;
        if (!MeRecyclable.IsInRecycler 
         && (count =Interlocked.Decrement(ref refCount)) <= 0 
         && MeRecyclable.AutoRecycleAtRefCountZero) MeRecyclable.Recycle();
        return count;
    }

    int IRecyclableObject.NonRecyclingDecrementRefCount()
    {
        if (!MeRecyclable.IsInRecycler && Interlocked.Decrement(ref refCount) <= 0)
        {
            Interlocked.Increment(ref refCount);
        }
        return refCount;
    }

    int IRecyclableObject.IncrementRefCount() => Interlocked.Increment(ref refCount);

    bool IRecyclableObject.Recycle()
    {
        if (MeRecyclable.IsInRecycler) return true;
        if (MeRecyclable.Recycler == null) return false;
        if (Interlocked.CompareExchange(ref isInRecycler, 1, 0) != 0) return false;

        #if DEBUG
        LastRecycleTimes[RecycleCount % NumRecentRecycleTimes] = DateTime.UtcNow;
        #endif
        Interlocked.Increment(ref RecycleCount);
        MeRecyclable.Recycler.Recycle(this);

        return true;
    }

    void IResetable.StateReset()
    {
        InheritedStateReset();
        refCount = 1;
    }

    protected virtual void InheritedStateReset()
    {
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
