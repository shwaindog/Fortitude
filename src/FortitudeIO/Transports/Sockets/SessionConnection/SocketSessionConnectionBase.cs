using System;
using System.Globalization;
using System.Threading;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols.Authentication;

namespace FortitudeIO.Transports.Sockets.SessionConnection
{
    public class SocketSessionConnectionBase
    {
        protected readonly IDirectOSNetworkingApi DirectOSNetworkingApi;

        public SocketSessionConnectionBase(IOSSocket socket, IDirectOSNetworkingApi directOSNetworkingApi,
            string sessionDescription)
        {
            DirectOSNetworkingApi = directOSNetworkingApi;
            Socket = socket;
            Id = Interlocked.Increment(ref idGen);
            SessionDescription = sessionDescription;
            Socket = socket;
            Handle = socket.Handle;
        }

        public ISocketSessionConnection Parent { get; set; }

        private static long idGen;
        public bool Active { get; set; }
        public long Id { get; }
        public IOSSocket Socket { get; }
        public IntPtr Handle { get; }
        public string SessionDescription { get; }
        public IUserData AuthData { get; set; }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Id.ToString(CultureInfo.InvariantCulture) + " " + SessionDescription;
        }

        public override bool Equals(object obj)
        {
            SocketSessionConnectionBase sid = obj as SocketSessionConnectionBase;
            return sid != null && sid.Id == Id;
        }
    }
}