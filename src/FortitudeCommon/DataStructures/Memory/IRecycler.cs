#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.DataStructures.Memory;

public interface IRecycler
{
    T Borrow<T>() where T : class, new();

    void Recycle(object recyclableObject);
}

public class Recycler : IRecycler
{
    private readonly bool acceptNonCreatedObjects;
    private readonly IMap<Type, IPooledFactory> poolFactoryMap = new ConcurrentMap<Type, IPooledFactory>();
    private readonly bool shouldAutoRecycleOnRefCountZero;
    private readonly bool throwWhenAttemptToRecycleRefCountNoZero;

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
            poolFactoryMap.Add(typeof(T), poolFactoryContainer);
        }

        var borrowed = (T)poolFactoryContainer!.Borrow();
        if (borrowed is IRecyclableObject recycleable)
        {
            recycleable.Recycler = this;
            if (shouldAutoRecycleOnRefCountZero) recycleable.IncrementRefCount();
            recycleable.AutoRecycleAtRefCountZero = shouldAutoRecycleOnRefCountZero;
            recycleable.IsInRecycler = false;
        }

        return borrowed;
    }

    public void Recycle(object recyclableObject)
    {
        var type = recyclableObject.GetType();
        var recyclable = recyclableObject as IRecyclableObject;
        if (!poolFactoryMap.TryGetValue(type, out var poolFactoryContainer))
            if (acceptNonCreatedObjects || (recyclable != null && recyclable.Recycler == this))
            {
                poolFactoryContainer = (IPooledFactory)ReflectionHelper
                    .InstantiateGenericType(typeof(GarbageAndLockFreePooledFactory<>)
                        , new[] { type }, new[]
                        {
                            ReflectionHelper.CreateEmptyConstructorFactoryAsFuncType(type)
                        });
                poolFactoryMap.Add(recyclableObject.GetType(), poolFactoryContainer);
            }
            else
            {
                throw new ArgumentException("Attempted to return an object that was never created from recycler");
            }

        if (recyclable != null)
        {
            if (recyclable.RefCount != 0 && recyclable.AutoRecycleAtRefCountZero &&
                throwWhenAttemptToRecycleRefCountNoZero)
                throw new ArgumentException("Attempted to recycle ref counted object that is not at RefCount == 0");
            recyclable.IsInRecycler = true;
        }

        poolFactoryContainer!.ReturnBorrowed(recyclableObject);
    }
}
