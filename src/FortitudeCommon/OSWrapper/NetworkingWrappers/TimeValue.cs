using System.Runtime.InteropServices;

namespace FortitudeCommon.OSWrapper.NetworkingWrappers
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TimeValue
    {
        public int Seconds;
        public int Microseconds;
        public TimeValue(int ms)
        {
            Seconds = ms / 1000;
            Microseconds = (ms % 1000) * 1000;
        }
    }
}