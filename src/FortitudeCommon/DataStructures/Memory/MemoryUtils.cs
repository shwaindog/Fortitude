using System.Runtime.InteropServices;
using System.Security;

namespace FortitudeCommon.DataStructures.Memory
{
    public static class MemoryUtils
    {
        public const int CacheLineSize = 128;

        public static int CeilingNextPowerOfTwo(int value)
        {
            var ceiling = 2;
            while (ceiling < value)
            {
                ceiling *= 2;
            }
            return ceiling;
        }

        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl,
            SetLastError = false), SuppressUnmanagedCodeSecurity]
        public static extern unsafe void* memcpy([In] void* dest, [In] void* src, [In] ulong count);
    }
}