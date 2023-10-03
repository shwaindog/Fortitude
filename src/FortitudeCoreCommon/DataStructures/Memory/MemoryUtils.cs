using System.Runtime.InteropServices;
using System.Security;

namespace FortitudeCommon.DataStructures.Memory
{
    public static class MemoryUtils
    {
        public const int CacheLineSize = 128;

        public static IOSDirectMemoryAccess OsDirectMemoryAccess { get; set; } =
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? new DirectWindowsMemoryAccess()
                : new DirectLinuxMemoryAccess();

        public static int CeilingNextPowerOfTwo(int value)
        {
            var ceiling = 2;
            while (ceiling < value)
            {
                ceiling *= 2;
            }

            return ceiling;
        }

        public static unsafe void* memcpy(void* dest, void* src, ulong count)
        {
            return OsDirectMemoryAccess.memcpy(dest, src, count);
        }

    }

    public interface IOSDirectMemoryAccess
    {
        unsafe void* memcpy(void* dest, void* src, ulong count);
    }

    class DirectWindowsMemoryAccess : IOSDirectMemoryAccess
    {
        public DirectWindowsMemoryAccess()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new PlatformNotSupportedException("This class should only be instantiated on .NET on Windows");
            }
        }

        unsafe void* IOSDirectMemoryAccess.memcpy(void* dest, void* src, ulong count)
        {
            return memcpy(dest, src, count);
        }

        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl,
             SetLastError = false), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void* memcpy([In] void* dest, [In] void* src, [In] ulong count);

    }

    class DirectLinuxMemoryAccess : IOSDirectMemoryAccess
    {
        public DirectLinuxMemoryAccess()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                throw new PlatformNotSupportedException("This class should only be instantiated on .NET Core on Linux");
            }
        }

        unsafe void* IOSDirectMemoryAccess.memcpy(void* dest, void* src, ulong count)
        {
            return memcpy(dest, src, count);
        }

        [DllImport("libc", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl,
             SetLastError = false), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void* memcpy([In] void* dest, [In] void* src, [In] ulong count);

    }
}

