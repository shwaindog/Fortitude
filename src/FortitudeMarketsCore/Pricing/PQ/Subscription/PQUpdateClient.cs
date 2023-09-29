using System.Reflection;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Sockets;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.Publishing;
using FortitudeIO.Transports.Sockets.Subscription;

namespace FortitudeMarketsCore.Pricing.PQ.Subscription
{
    public sealed class PQUpdateClient : UdpSubscriber, IPQUpdateClient
    {
        public PQUpdateClient(ISocketDispatcher dispatcher, IOSNetworkingController networkingController, 
            IConnectionConfig connectionConfig, string socketUseDescription, string networkAddress, 
            int wholeMessagesPerReceive, IPQQuoteSerializerFactory pqQuoteSerializerFactory)
            : base(FLoggerFactory.Instance.GetLogger(MethodBase.GetCurrentMethod().DeclaringType), dispatcher, 
                  networkingController, connectionConfig, socketUseDescription + " PQUpdateClient", networkAddress, 
                  wholeMessagesPerReceive, new LinkedListUintKeyMap<IBinaryDeserializer>())
        {
            factory = pqQuoteSerializerFactory ?? factory;
        }

        public const int PQReceiveBufferSize = 2097152;

        private readonly IPQQuoteSerializerFactory factory = new PQQuoteSerializerFactory();
        protected override IBinaryDeserializationFactory GetFactory()
        {
            return factory;
        }

        public override IStreamDecoder GetDecoder(IMap<uint, IBinaryDeserializer> decoderDeserializers)
        {
            return new PQClientDecoder(decoderDeserializers, PQFeedType.Update);
        }

        public override int RecvBufferSize => PQReceiveBufferSize;

        public override IBinaryStreamPublisher StreamToPublisher => null;
    }
}
