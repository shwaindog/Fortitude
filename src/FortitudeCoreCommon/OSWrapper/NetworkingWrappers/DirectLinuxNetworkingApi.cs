using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security;

namespace FortitudeCommon.OSWrapper.NetworkingWrappers
{
    public class DirectLinuxNetworkingApi : IDirectOSNetworkingApi
    {
        public DirectLinuxNetworkingApi()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                throw new PlatformNotSupportedException("This class should only be instantiated on .NET Core on Linux");
            }
        }

        public int Select(int ignoredParameter, IntPtr[] readfds,
            IntPtr[] writefds, IntPtr[] exceptfds, ref TimeValue timeout)
        {
            return select(ignoredParameter, readfds, writefds, exceptfds, ref timeout);
        }

        public int GetLastCallError()
        {
            return Marshal.GetLastPInvokeError();
        }

        public unsafe int Recv(IntPtr socketHandle, byte* pinnedBuffer, int len, ref bool partialMsg)
        {
            var flags = partialMsg ? RecvFlags.MSG_DONTWAIT : RecvFlags.MSG_WAITALL;
            return recv(socketHandle, pinnedBuffer, len, (int)flags);
        }
        private enum RecvFlags
        {
            MSG_WAITALL = 0x40,
            MSG_DONTWAIT = 0x80
        }

        public int IoCtlSocket(IntPtr socketHandle, ref int arg)
        {
            return 0;
        }

        public unsafe int Send([In] IntPtr socketHandle, [In] byte* pinnedBuffer, [In] int len,
            [In] SocketFlags socketFlags)
        {
            return send(socketHandle, pinnedBuffer, len, socketFlags);
        }

        [DllImport("libc", SetLastError = true)]
        [SuppressUnmanagedCodeSecurity]
        private static extern unsafe int send([In] IntPtr socketHandle, [In] byte* pinnedBuffer, [In] int len,
            [In] SocketFlags socketFlags);

        [DllImport("libc", SetLastError = true), SuppressUnmanagedCodeSecurity]
        private static extern int select([In] int ignoredParameter, [In, Out] IntPtr[] readfds,
            [In, Out] IntPtr[] writefds, [In, Out] IntPtr[] exceptfds, [In] ref TimeValue timeout);


        [DllImport("libc", SetLastError = true)]
        [SuppressUnmanagedCodeSecurity]
        private static extern unsafe int recv([In] IntPtr socketHandle, [In] byte* pinnedBuffer, [In] int len,
            [In] int flags);

    }
}