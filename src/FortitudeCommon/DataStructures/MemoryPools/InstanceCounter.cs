namespace FortitudeCommon.DataStructures.MemoryPools;

public class InstanceCounter<T>
{
    private static int totalInstances;
    public static int NextInstanceNum => Interlocked.Increment(ref totalInstances);
}
