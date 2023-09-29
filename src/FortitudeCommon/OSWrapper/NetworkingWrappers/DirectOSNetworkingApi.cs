using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security;

namespace FortitudeCommon.OSWrapper.NetworkingWrappers
{
    public class DirectOSNetworkingApi : IDirectOSNetworkingApi
    {
        public int Select(int ignoredParameter, IntPtr[] readfds,
            IntPtr[] writefds, IntPtr[] exceptfds, ref TimeValue timeout)
        {
            return select(ignoredParameter, readfds, writefds, exceptfds, ref timeout);
        }

        public int GetLastWin32Error()
        {
            return Marshal.GetLastWin32Error();
        }

        public unsafe int WsaRecvEx(IntPtr socketHandle, byte* pinnedBuffer, int len, ref bool partialMsg)
        {
            return WSARecvEx(socketHandle, pinnedBuffer, len, ref partialMsg);
        }

        public int IoCtlSocket(IntPtr socketHandle, ref int arg)
        {
            return ioctlsocket(socketHandle, IocCtlCommand.Fionread, ref arg);
        }
        
        private enum IocCtlCommand
        {
            Fionread = 1074030207
        }

        public unsafe int Send([In] IntPtr socketHandle, [In] byte* pinnedBuffer, [In] int len,
            [In] SocketFlags socketFlags)
        {
            return send(socketHandle, pinnedBuffer, len, socketFlags);
        }

        [DllImport("ws2_32.dll", SetLastError = true)]
        [SuppressUnmanagedCodeSecurity]
        private static extern unsafe int send([In] IntPtr socketHandle, [In] byte* pinnedBuffer, [In] int len,
            [In] SocketFlags socketFlags);

        [DllImport("ws2_32.dll", SetLastError = true), SuppressUnmanagedCodeSecurity]
        private static extern int select([In] int ignoredParameter, [In, Out] IntPtr[] readfds, 
            [In, Out] IntPtr[] writefds, [In, Out] IntPtr[] exceptfds, [In] ref TimeValue timeout);


        [DllImport("Mswsock.dll", SetLastError = true)]
        [SuppressUnmanagedCodeSecurity]
        private static extern unsafe int WSARecvEx([In] IntPtr socketHandle, [In] byte* pinnedBuffer, [In] int len,
            [In] [Out] ref bool partialMsg);



        [DllImport("ws2_32.dll", SetLastError = true)]
        [SuppressUnmanagedCodeSecurity]
        private static extern int ioctlsocket([In] IntPtr socketHandle, [In] IocCtlCommand command,
            [In] [Out] ref int arg);

    }
}
