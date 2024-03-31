#region

using System.Runtime.InteropServices;

#endregion

namespace FortitudeIO.Transports.Network.Sockets;

[StructLayout(LayoutKind.Explicit)]
internal struct KeepAlive
{
    [FieldOffset(0)] public uint OnOff;
    [FieldOffset(4)] public uint KeepAliveTimeMillisecs;
    [FieldOffset(8)] public uint KeepAliveIntervalMillisecs;

    public byte[] ToByteArray()
    {
        var size = 12;
        var arr = new byte[size];
        var ptr = Marshal.AllocHGlobal(size);

        Marshal.StructureToPtr((object)this, ptr, true);
        Marshal.Copy(ptr, arr, 0, size);
        Marshal.FreeHGlobal(ptr);

        return arr;
    }
}
