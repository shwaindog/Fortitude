#region

using System.Reflection;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.Publishing;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Publication;

public sealed class PQUpdatePublisher : UdpPublisher, IPQUpdateServer
{
    internal readonly PQServerSerializationFactory Factory = new(PQFeedType.Update);
    private readonly IOSNetworkingController networkingController;

    public PQUpdatePublisher(ISocketDispatcher dispatcher, IOSNetworkingController networkingController,
        IConnectionConfig connectionConfig, string socketUseDescription, string networkAddress)
        : base(FLoggerFactory.Instance.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType!), dispatcher,
            networkingController, connectionConfig, socketUseDescription + " PQUpdatePublisher", networkAddress)
    {
        this.networkingController = networkingController;
        RegisterSerializer<PQLevel0Quote>((uint)PricingMessageIds.PricingMessage);
        RegisterSerializer<PQHeartBeatQuotesMessage>((uint)PricingMessageIds.HeartBeatMessage);
    }

    public override int SendBufferSize => 2097152;

    public void Send(IVersionedMessage message)
    {
        Enqueue(PublisherConnection!, message);
    }

    public override IBinarySerializationFactory GetFactory() => Factory;

    protected override UdpSubscriber BuildSubscriber(UdpPublisher publisher) =>
        new PQUpdatePublisherSubscriber(this, Logger, Dispatcher, networkingController,
            ConnectionConfig, SessionDescription, 0);

    private class PQUpdatePublisherSubscriber : UdpSubscriber
    {
        public PQUpdatePublisherSubscriber(UdpPublisher udpPublisher, IFLogger logger, ISocketDispatcher dispatcher,
            IOSNetworkingController networkingController,
            IConnectionConfig connectionConfig, string sessionDescription, int wholeMessagesPerReceive)
            : base(udpPublisher, logger, dispatcher, networkingController, connectionConfig,
                sessionDescription, wholeMessagesPerReceive) =>
            ZeroBytesReadIsDisconnection = false;

        public override int RecvBufferSize => 0;

        protected override IBinaryDeserializationFactory? GetFactory() => null;

        public override IMessageStreamDecoder?
            GetDecoder(IMap<uint, IMessageDeserializer> decoderDeserializers) =>
            null;
    }
}
