// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections.Concurrent;
using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.DataStructures.MemoryPools;

public interface IRecycler
{
    T Borrow<T>() where T : class, new();

    object Borrow(Type type);

    void Recycle(object trash);
}

public class SingletonRecycler
{
    public static IRecycler Instance { get; set; } = new Recycler();
}

public class Recycler : IRecycler
{
    [ThreadStatic] private static IRecycler? threadStaticRecycler;

    private readonly bool acceptNonCreatedObjects;

    private readonly ConcurrentDictionary<Type, IPooledFactory> poolFactoryMap = new();

    private readonly ConcurrentDictionary<Type, Delegate> newItemFactory = new();

    private readonly bool shouldAutoRecycleOnRefCountZero;
    private readonly bool throwWhenAttemptToRecycleRefCountNoZero;

    public Recycler() : this(true, true, false) { }

    public Recycler(string name) : this(true, true, false)
    {
        Name = name;
    }

    public Recycler
    (bool shouldAutoRecycleOnRefCountZero = true, bool acceptNonCreatedObjects = true,
        bool throwWhenAttemptToRecycleRefCountNoZero = true)
    {
        this.acceptNonCreatedObjects                 = acceptNonCreatedObjects;
        this.shouldAutoRecycleOnRefCountZero         = shouldAutoRecycleOnRefCountZero;
        this.throwWhenAttemptToRecycleRefCountNoZero = throwWhenAttemptToRecycleRefCountNoZero;
    }

    public Recycler RegisterFactory<T>(Func<T> constructionFactory)
    {
        var forType = typeof(T);
        newItemFactory.AddOrUpdate(forType, _ => constructionFactory, (_, _) => constructionFactory);

        return this;
    }

    public bool HasFactory(Type type) => newItemFactory.ContainsKey(type);
    public bool HasFactory<T>() => newItemFactory.ContainsKey(typeof(T));

    public int ReleasePooledItemsWithNewFactory<T>(Func<T> constructionFactory)
    {
        var type             = typeof(T);
        newItemFactory.AddOrUpdate(type, _ => constructionFactory, (_, _) => constructionFactory);
        var hasExisting = poolFactoryMap.TryGetValue(type, out var poolFactoryContainer);
        if (hasExisting)
        {
            var newInstanceFunc = constructionFactory;
            poolFactoryContainer = (IPooledFactory)ReflectionHelper.InstantiateGenericType
                (typeof(GarbageAndLockFreePooledFactory<>), [type], newInstanceFunc, 4);
            poolFactoryMap.TryAdd(type, poolFactoryContainer);
            return poolFactoryContainer.Count;
        }
        return 0;
    }

    public static IRecycler ThreadStaticRecycler
    {
        get => threadStaticRecycler ??= new Recycler();
        set => threadStaticRecycler = value;
    }

    public string? Name { get; }

    public T Borrow<T>() where T : class, new()
    {
        if (!poolFactoryMap.TryGetValue(typeof(T), out var poolFactoryContainer))
        {
            Func<T>? buildFunc = null;
            if (newItemFactory.TryGetValue(typeof(T), out var constructFunc))
            {
                buildFunc = constructFunc as Func<T>;
            }
            poolFactoryContainer = new GarbageAndLockFreePooledFactory<T>(buildFunc ?? (() => new T()));
            poolFactoryMap.TryAdd(typeof(T), poolFactoryContainer);
        }

        var borrowed = (T)poolFactoryContainer!.Borrow();
        if (borrowed is IRecyclableObject recyclable)
        {
            recyclable.Recycler = this;

            recyclable.IsInRecycler = false;
            while (shouldAutoRecycleOnRefCountZero && recyclable.RefCount < 1) recyclable.IncrementRefCount();
            while (shouldAutoRecycleOnRefCountZero && recyclable.RefCount > 1) recyclable.DecrementRefCount();
            recyclable.AutoRecycleAtRefCountZero = shouldAutoRecycleOnRefCountZero;

        }

        return borrowed;
    }

    public object Borrow(Type type)
    {
        if (!poolFactoryMap.TryGetValue(type, out var poolFactoryContainer))
        {
            Delegate? buildFunc = null;
            if (newItemFactory.TryGetValue(type, out var constructFunc))
            {
                buildFunc = constructFunc;
            }
            var newInstanceFunc = buildFunc ?? ReflectionHelper.CreateEmptyConstructorFactoryAsFuncType(type);
            poolFactoryContainer = (IPooledFactory)ReflectionHelper.InstantiateGenericType
                (typeof(GarbageAndLockFreePooledFactory<>), new[] { type }, newInstanceFunc, 4);
            poolFactoryMap.TryAdd(type, poolFactoryContainer);
        }

        var borrowed = poolFactoryContainer!.Borrow();
        if (borrowed is IRecyclableObject recyclable)
        {
            recyclable.Recycler = this;
            while (shouldAutoRecycleOnRefCountZero && recyclable.RefCount < 1) recyclable.IncrementRefCount();
            while (shouldAutoRecycleOnRefCountZero && recyclable.RefCount > 1) recyclable.DecrementRefCount();
            recyclable.AutoRecycleAtRefCountZero = shouldAutoRecycleOnRefCountZero;

            recyclable.IsInRecycler = false;
        }

        return borrowed;
    }

    public void Recycle(object trash)
    {
        var type = trash.GetType();

        var recyclable = trash as IRecyclableObject;
        if (recyclable != null)
        {
            if (!recyclable.IsInRecycler) throw new ArgumentException("Attempted to recycle object without setting IsInRecycler to true.");
            if (recyclable.RefCount != 0 && recyclable.AutoRecycleAtRefCountZero &&
                throwWhenAttemptToRecycleRefCountNoZero)
                throw new ArgumentException("Attempted to recycle ref counted object that is not at RefCount == 0");
            recyclable.StateReset();
        }

        if (!poolFactoryMap.TryGetValue(type, out var poolFactoryContainer))
            if (acceptNonCreatedObjects || recyclable?.Recycler == this)
            {
                var newInstanceFunc = ReflectionHelper.CreateEmptyConstructorFactoryAsFuncType(type);

                poolFactoryContainer = (IPooledFactory)ReflectionHelper.InstantiateGenericType
                    (typeof(GarbageAndLockFreePooledFactory<>), new[] { type }, newInstanceFunc, 4);
                poolFactoryMap.TryAdd(trash.GetType(), poolFactoryContainer);
            }
            else
            {
                throw new ArgumentException("Attempted to return an object that was never created from recycler");
            }

        poolFactoryContainer!.ReturnBorrowed(trash);
    }
}
