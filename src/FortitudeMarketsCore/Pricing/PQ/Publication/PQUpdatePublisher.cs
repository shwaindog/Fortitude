using System.Collections.Generic;
using System.Reflection;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Sockets;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.Publishing;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription;

namespace FortitudeMarketsCore.Pricing.PQ.Publication
{
    public sealed class PQUpdatePublisher : UdpPublisher, IPQUpdateServer
    {
        private readonly IOSNetworkingController networkingController;
        internal readonly PQServerSerializationFactory Factory = new PQServerSerializationFactory(PQFeedType.Update);

        public PQUpdatePublisher(ISocketDispatcher dispatcher, IOSNetworkingController networkingController, 
            IConnectionConfig connectionConfig, string socketUseDescription, string networkAddress)
            : base(FLoggerFactory.Instance.GetLogger(MethodBase.GetCurrentMethod().DeclaringType), dispatcher,
                networkingController, connectionConfig, socketUseDescription + " PQUpdatePublisher", networkAddress)
        {
            this.networkingController = networkingController;
            RegisterSerializer<PQLevel0Quote>(0);
            RegisterSerializer<IEnumerable<PQLevel0Quote>>(1);
        }

        public override int SendBufferSize => 2097152;

        public void Send(IVersionedMessage message)
        {
            Enqueue(PublisherConnection, message);
        }

        public override IBinarySerializationFactory GetFactory()
        {
            return Factory;
        }

        protected override UdpSubscriber BuildSubscriber(UdpPublisher publisher)
        {
            return new PQUpdatePublisherSubscriber(this, Logger, Dispatcher, networkingController, 
                ConnectionConfig, SessionDescription, 0);
        }

        private class PQUpdatePublisherSubscriber : UdpSubscriber
        {
            public PQUpdatePublisherSubscriber(UdpPublisher udpPublisher, IFLogger logger, ISocketDispatcher dispatcher,
                IOSNetworkingController networkingController, 
                IConnectionConfig connectionConfig, string sessionDescription, int wholeMessagesPerReceive)
                : base(udpPublisher, logger, dispatcher, networkingController, connectionConfig, 
                      sessionDescription, wholeMessagesPerReceive)
            {
                ZeroBytesReadIsDisconnection = false;
            }

            public override int RecvBufferSize => 0;

            protected override IBinaryDeserializationFactory GetFactory()
            {
                return null;
            }

            public override IStreamDecoder GetDecoder(IMap<uint, IBinaryDeserializer> decoderDeserializers)
            {
                return null;
            }
        }
    }
}