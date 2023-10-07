#region

using System.Net.Sockets;

#endregion

namespace FortitudeCommon.OSWrapper.NetworkingWrappers;

public interface IDirectOSNetworkingApi
{
    int Select(int ignoredParameter, IntPtr[] readfds,
        IntPtr[]? writefds, IntPtr[]? exceptfds, ref TimeValue timeout);

    int GetLastCallError();

    unsafe int Recv(IntPtr socketHandle, byte* pinnedBuffer, int len, ref bool partialMsg);

    // TODO hide this inside the DirectWindowsNetworkingApi
    int IoCtlSocket(IntPtr socketHandle, ref int arg);

    unsafe int Send(IntPtr socketHandle, byte* pinnedBuffer, int len, SocketFlags socketFlags);
}
