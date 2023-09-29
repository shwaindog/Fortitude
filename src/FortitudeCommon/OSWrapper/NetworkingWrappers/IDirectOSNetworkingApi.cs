using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace FortitudeCommon.OSWrapper.NetworkingWrappers
{
    public interface IDirectOSNetworkingApi
    {
        int Select(int ignoredParameter, IntPtr[] readfds,
            IntPtr[] writefds, IntPtr[] exceptfds, ref TimeValue timeout);

        int GetLastWin32Error();

        unsafe int WsaRecvEx(IntPtr socketHandle, byte* pinnedBuffer, int len, ref bool partialMsg);
        int IoCtlSocket(IntPtr socketHandle, ref int arg);

        unsafe int Send(IntPtr socketHandle, byte* pinnedBuffer, int len, SocketFlags socketFlags);
    }
}