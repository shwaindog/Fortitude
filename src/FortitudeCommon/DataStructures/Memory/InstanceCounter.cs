namespace FortitudeCommon.DataStructures.Memory;

public class InstanceCounter<T>
{
    private static int totalInstances;
    public static int NextInstanceNum => Interlocked.Increment(ref totalInstances);
}
