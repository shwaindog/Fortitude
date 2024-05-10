#region

using System.Runtime.InteropServices;

#endregion

namespace FortitudeCommon.OSWrapper.Memory;

public static class MemoryUtils
{
    public const int CacheLineSize = 128;

    public static IOSDirectMemoryApi OsDirectMemoryAccess { get; set; } =
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? new WindowsDirectMemoryApi() : new LinuxDirectMemoryApi();

    public static int CeilingNextPowerOfTwo(int value)
    {
        var ceiling = 2;
        while (ceiling < value) ceiling *= 2;

        return ceiling;
    }

    public static unsafe void* memcpy(void* dest, void* src, ulong count) => OsDirectMemoryAccess.memcpy(dest, src, count);
}
