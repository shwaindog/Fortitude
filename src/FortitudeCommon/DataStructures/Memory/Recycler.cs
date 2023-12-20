#region

using System.Collections.Concurrent;
using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.DataStructures.Memory;

public interface IRecycler
{
    T Borrow<T>() where T : class, new();
    object Borrow(Type type);

    void Recycle(object trash);
}

public class Recycler : IRecycler
{
    private readonly bool acceptNonCreatedObjects;
    private readonly ConcurrentDictionary<Type, IPooledFactory> poolFactoryMap = new();
    private readonly bool shouldAutoRecycleOnRefCountZero;
    private readonly bool throwWhenAttemptToRecycleRefCountNoZero;

    public Recycler() : this(true, true, false) { }

    public Recycler(bool shouldAutoRecycleOnRefCountZero = true, bool acceptNonCreatedObjects = true,
        bool throwWhenAttemptToRecycleRefCountNoZero = true)
    {
        this.acceptNonCreatedObjects = acceptNonCreatedObjects;
        this.shouldAutoRecycleOnRefCountZero = shouldAutoRecycleOnRefCountZero;
        this.throwWhenAttemptToRecycleRefCountNoZero = throwWhenAttemptToRecycleRefCountNoZero;
    }

    public T Borrow<T>() where T : class, new()
    {
        if (!poolFactoryMap.TryGetValue(typeof(T), out var poolFactoryContainer))
        {
            poolFactoryContainer = new GarbageAndLockFreePooledFactory<T>(() => new T());
            poolFactoryMap.TryAdd(typeof(T), poolFactoryContainer);
        }

        var borrowed = (T)poolFactoryContainer!.Borrow();
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

    public object Borrow(Type type)
    {
        if (!poolFactoryMap.TryGetValue(type, out var poolFactoryContainer))
        {
            var newInstanceFunc = ReflectionHelper.CreateEmptyConstructorFactoryAsFuncType(type);
            poolFactoryContainer = (IPooledFactory)ReflectionHelper
                .InstantiateGenericType(typeof(GarbageAndLockFreePooledFactory<>)
                    , new[] { type }, newInstanceFunc, 4);
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
            if (!recyclable.IsInRecycler)
                throw new ArgumentException("Attempted to recycle object without setting IsInRecycler to true.");
            if (recyclable.RefCount != 0 && recyclable.AutoRecycleAtRefCountZero &&
                throwWhenAttemptToRecycleRefCountNoZero)
                throw new ArgumentException("Attempted to recycle ref counted object that is not at RefCount == 0");
            recyclable.StateReset();
        }

        if (!poolFactoryMap.TryGetValue(type, out var poolFactoryContainer))
            if (acceptNonCreatedObjects || recyclable?.Recycler == this)
            {
                var newInstanceFunc = ReflectionHelper.CreateEmptyConstructorFactoryAsFuncType(type);

                poolFactoryContainer = (IPooledFactory)ReflectionHelper
                    .InstantiateGenericType(typeof(GarbageAndLockFreePooledFactory<>)
                        , new[] { type }, newInstanceFunc, 4);
                poolFactoryMap.TryAdd(trash.GetType(), poolFactoryContainer);
            }
            else
            {
                throw new ArgumentException("Attempted to return an object that was never created from recycler");
            }

        poolFactoryContainer!.ReturnBorrowed(trash);
    }
}
