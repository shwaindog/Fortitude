using System;

namespace FortitudeIO.Transports.Sockets.Client
{
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    internal struct KeepAlive
    {
        [System.Runtime.InteropServices.FieldOffset(0)]
        public uint OnOff;
        [System.Runtime.InteropServices.FieldOffset(4)]
        public uint KeepAliveTimeMillisecs;
        [System.Runtime.InteropServices.FieldOffset(8)]
        public uint KeepAliveIntervalMillisecs;

        public byte[] ToByteArray()
        {
            int size = 12;
            byte[] arr = new byte[size];
            IntPtr ptr = System.Runtime.InteropServices.Marshal.AllocHGlobal(size);

            System.Runtime.InteropServices.Marshal.StructureToPtr(this, ptr, true);
            System.Runtime.InteropServices.Marshal.Copy(ptr, arr, 0, size);
            System.Runtime.InteropServices.Marshal.FreeHGlobal(ptr);

            return arr;
        }
    }
}