// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections.Concurrent;

#endregion

namespace FortitudeCommon.DataStructures.Memory;

public interface ILimitedRecycler : IRecycler
{
    int     MaxTypeBorrowLimit { get; set; }
    T?      TryBorrow<T>() where T : class, new();
    object? TryBorrow(Type type);
}

public class LimitedBlockingRecycler : ILimitedRecycler
{
    private readonly IRecycler backingRecycler;

    private readonly ConcurrentDictionary<Type, Semaphore> borrowCountMap = new();

    public LimitedBlockingRecycler() : this(-1, true, true, false) { }

    public LimitedBlockingRecycler
    (int maxTypeBorrowLimit, bool shouldAutoRecycleOnRefCountZero = true, bool acceptNonCreatedObjects = true,
        bool throwWhenAttemptToRecycleRefCountNoZero = true)
    {
        MaxTypeBorrowLimit = maxTypeBorrowLimit;

        backingRecycler = new Recycler(shouldAutoRecycleOnRefCountZero, acceptNonCreatedObjects, throwWhenAttemptToRecycleRefCountNoZero);
    }

    public LimitedBlockingRecycler(int maxTypeBorrowLimit, IRecycler backingRecycler)
    {
        MaxTypeBorrowLimit = maxTypeBorrowLimit;

        this.backingRecycler = backingRecycler;
    }

    public int MaxTypeBorrowLimit { get; set; }

    public T Borrow<T>() where T : class, new()
    {
        var typeOfT = typeof(T);
        if (MaxTypeBorrowLimit <= 0) return backingRecycler.Borrow<T>();
        if (!borrowCountMap.TryGetValue(typeOfT, out var semaphore))
        {
            semaphore = new Semaphore(0, MaxTypeBorrowLimit);
            borrowCountMap.AddOrUpdate(typeOfT, _ => semaphore, (_, existing) => semaphore = existing);
        }
        semaphore?.WaitOne();
        return backingRecycler.Borrow<T>();
    }

    public T? TryBorrow<T>() where T : class, new()
    {
        var typeOfT = typeof(T);
        if (MaxTypeBorrowLimit <= 0) return backingRecycler.Borrow<T>();
        if (!borrowCountMap.TryGetValue(typeOfT, out var semaphore))
        {
            semaphore = new Semaphore(0, MaxTypeBorrowLimit);
            borrowCountMap.AddOrUpdate(typeOfT, _ => semaphore, (_, existing) => semaphore = existing);
        }
        if (!semaphore?.WaitOne(1) ?? false) return null;
        return backingRecycler.Borrow<T>();
    }

    public object Borrow(Type type)
    {
        if (MaxTypeBorrowLimit <= 0) return backingRecycler.Borrow(type);
        if (!borrowCountMap.TryGetValue(type, out var semaphore))
        {
            semaphore = new Semaphore(0, MaxTypeBorrowLimit);
            borrowCountMap.AddOrUpdate(type, _ => semaphore, (_, existing) => semaphore = existing);
        }
        semaphore?.WaitOne();
        return backingRecycler.Borrow(type);
    }

    public object? TryBorrow(Type type)
    {
        if (MaxTypeBorrowLimit <= 0) return backingRecycler.Borrow(type);
        if (!borrowCountMap.TryGetValue(type, out var semaphore))
        {
            semaphore = new Semaphore(0, MaxTypeBorrowLimit);
            borrowCountMap.AddOrUpdate(type, _ => semaphore, (_, existing) => semaphore = existing);
        }
        if (!semaphore?.WaitOne(1) ?? false) return null;
        return backingRecycler.Borrow(type);
    }

    public void Recycle(object trash)
    {
        if (borrowCountMap.TryGetValue(trash.GetType(), out var semaphore)) semaphore.Release();
        backingRecycler.Recycle(trash);
    }
}
