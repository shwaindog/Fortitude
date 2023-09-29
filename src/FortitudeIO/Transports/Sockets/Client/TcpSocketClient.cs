using System.Net;
using System.Net.Sockets;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.Subscription;

namespace FortitudeIO.Transports.Sockets.Client
{
    public abstract class TcpSocketClient : SocketSubscriber
    {
        private readonly bool keepalive;

        protected TcpSocketClient(IFLogger logger, ISocketDispatcher dispatcher,
            IOSNetworkingController networkingController, IConnectionConfig connectionConfig, 
            string sessionDescription, int wholeMessagesPerReceive, 
            IMap<uint, IBinaryDeserializer> serializerCache = null, 
            bool keepalive = false)
            : base(logger, dispatcher, networkingController, connectionConfig, sessionDescription,
                wholeMessagesPerReceive, serializerCache
            )
        {
            this.keepalive = keepalive;
        }

        protected override IOSSocket CreateAndConnect(string host, int port)
        {
            Logger.Info("Attempting TCP connection to {0} on {1}:{2}", SessionDescription, host, port);
            var socket = NetworkingController.CreateOSSocket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
            socket.NoDelay = true;

            if (keepalive)
            {
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, 1);

                KeepAlive k = new KeepAlive
                {
                    OnOff = 1,
                    KeepAliveTimeMillisecs = 14_000,
                    KeepAliveIntervalMillisecs = 2_000
                };
                var inValue = k.ToByteArray();

                socket.IOControl(IOControlCode.KeepAliveValues, inValue, null);
            }
            socket.Connect(new IPEndPoint(NetworkingController.GetIpAddress(host), port));
            return socket;
        }
    }
}