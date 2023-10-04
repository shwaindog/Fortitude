#region

using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security;

#endregion

namespace FortitudeCommon.OSWrapper.NetworkingWrappers;

public class DirectWindowsNetworkingApi : IDirectOSNetworkingApi
{
    public DirectWindowsNetworkingApi()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            throw new PlatformNotSupportedException("This class should only be instantiated on .NET on Windows");
    }

    public int Select(int ignoredParameter, IntPtr[] readfds,
        IntPtr[]? writefds, IntPtr[]? exceptfds, ref TimeValue timeout) =>
        select(ignoredParameter, readfds, writefds, exceptfds, ref timeout);

    public int GetLastCallError() => Marshal.GetLastPInvokeError();

    public unsafe int Recv(IntPtr socketHandle, byte* pinnedBuffer, int len, ref bool partialMsg) =>
        WSARecvEx(socketHandle, pinnedBuffer, len, ref partialMsg);

    public int IoCtlSocket(IntPtr socketHandle, ref int arg) =>
        ioctlsocket(socketHandle, IocCtlCommand.Fionread, ref arg);

    public unsafe int Send([In] IntPtr socketHandle, [In] byte* pinnedBuffer, [In] int len,
        [In] SocketFlags socketFlags) =>
        send(socketHandle, pinnedBuffer, len, socketFlags);

    [DllImport("ws2_32.dll", SetLastError = true)]
    [SuppressUnmanagedCodeSecurity]
    private static extern unsafe int send([In] IntPtr socketHandle, [In] byte* pinnedBuffer, [In] int len,
        [In] SocketFlags socketFlags);

    [DllImport("ws2_32.dll", SetLastError = true)]
    [SuppressUnmanagedCodeSecurity]
    private static extern int select([In] int ignoredParameter, [In] [Out] IntPtr[] readfds,
        [In] [Out] IntPtr[]? writefds, [In] [Out] IntPtr[]? exceptfds, [In] ref TimeValue timeout);


    [DllImport("Mswsock.dll", SetLastError = true)]
    [SuppressUnmanagedCodeSecurity]
    private static extern unsafe int WSARecvEx([In] IntPtr socketHandle, [In] byte* pinnedBuffer, [In] int len,
        [In] [Out] ref bool partialMsg);


    [DllImport("ws2_32.dll", SetLastError = true)]
    [SuppressUnmanagedCodeSecurity]
    private static extern int ioctlsocket([In] IntPtr socketHandle, [In] IocCtlCommand command,
        [In] [Out] ref int arg);

    private enum IocCtlCommand
    {
        Fionread = 1074030207
    }
}
