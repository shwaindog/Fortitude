using System;
using System.Reflection;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Sockets;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.Publishing;
using FortitudeIO.Transports.Sockets.SessionConnection;
using FortitudeIO.Transports.Sockets.Subscription;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription;

namespace FortitudeMarketsCore.Pricing.PQ.Publication
{
    public sealed class PQSnapshotServer : TcpSocketPublisher, IPQSnapshotServer
    {
        private readonly PQServerSerializationFactory factory = new PQServerSerializationFactory(PQFeedType.Snapshot);
        private readonly IConnectionConfig connectionConfig;

        private SnapshotClientStreamSubscriber snapshotClientStreamSubscriber;

        public PQSnapshotServer(ISocketDispatcher dispatcher, IOSNetworkingController networkingController, 
            IConnectionConfig connectionConfig, string socketUseDescription)
            : base(FLoggerFactory.Instance.GetLogger(MethodBase.GetCurrentMethod().DeclaringType), dispatcher, networkingController,
                connectionConfig.Port, socketUseDescription + " PQSnapshotServer")
        {
            this.connectionConfig = connectionConfig;
            RegisterSerializer<PQLevel0Quote>(0);
        }

        public override int SendBufferSize => 131072;

        public override IBinaryStreamSubscriber StreamFromSubscriber => snapshotClientStreamSubscriber ??
            (snapshotClientStreamSubscriber = new SnapshotClientStreamSubscriber(
                Logger, Dispatcher, NetworkingController, this, connectionConfig, 
                SessionDescription + " Client "));

        public IPQSnapshotStreamSubscriber SnapshotClientStreamFromSubscriber
            => (SnapshotClientStreamSubscriber) StreamFromSubscriber;

        public override IBinarySerializationFactory GetFactory()
        {
            return factory;
        }

        public class SnapshotClientStreamSubscriber : SocketSubscriber, IPQSnapshotStreamSubscriber
        {
            private readonly PQSnapshotServer pqSnapshotServer;

            public SnapshotClientStreamSubscriber(IFLogger logger, ISocketDispatcher dispatcher, 
                IOSNetworkingController networkingController, PQSnapshotServer pqSnapshotServer, 
                IConnectionConfig connectionConfig, string sessionDescription)
                : base(logger, dispatcher, networkingController, connectionConfig, 
                      sessionDescription, 0)
            {
                this.pqSnapshotServer = pqSnapshotServer;
                ZeroBytesReadIsDisconnection = false;
            }

            public override int RecvBufferSize => 131072;

            public override IBinaryStreamPublisher StreamToPublisher => pqSnapshotServer;

            protected override ISocketSessionConnection Connector
            {
                get => pqSnapshotServer.Acceptor;
                set => pqSnapshotServer.Acceptor = value;
            }

            public Action<ISocketSessionConnection, uint[]> OnSnapshotRequest { get; set; }

            protected override IBinaryDeserializationFactory GetFactory()
            {
                return (IBinaryDeserializationFactory) pqSnapshotServer.GetFactory();
            }

            public override IStreamDecoder GetDecoder(IMap<uint, IBinaryDeserializer> deserializersLookup)
            {
                return new PQServerDecoder(OnRequest);
            }

            private void OnRequest(ISocketSessionConnection cx, uint[] streamIDs)
            {
                OnSnapshotRequest?.Invoke(cx, streamIDs);
            }

            protected override IOSSocket CreateAndConnect(string host, int port)
            {
                return null;
            }
        }
    }
}