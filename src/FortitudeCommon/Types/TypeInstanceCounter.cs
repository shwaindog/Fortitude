#region

using FortitudeCommon.DataStructures.Maps;

#endregion

namespace FortitudeCommon.Types;

public class InstanceCountContainer
{
    private int instanceCount;
    public int Increment() => Interlocked.Increment(ref instanceCount);
}

public class TypeInstanceCounter
{
    private IMap<Type, InstanceCountContainer> registeredTypes = new ConcurrentMap<Type, InstanceCountContainer>();

    public static TypeInstanceCounter Instance { get; set; } = new();

    public int GetNextInstanceNumber(Type type)
    {
        if (registeredTypes.TryGetValue(type, out var instanceCountContainer))
            return instanceCountContainer!.Increment();
        instanceCountContainer = new InstanceCountContainer();
        registeredTypes.TryAdd(type, instanceCountContainer);
        return instanceCountContainer!.Increment();
    }

    public int GetNextInstanceNumber<T>()
    {
        if (registeredTypes.TryGetValue(typeof(T), out var instanceCountContainer))
            return instanceCountContainer!.Increment();
        instanceCountContainer = new InstanceCountContainer();
        registeredTypes.TryAdd(typeof(T), instanceCountContainer);
        return instanceCountContainer!.Increment();
    }
}
