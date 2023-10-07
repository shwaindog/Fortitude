#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols.ORX.Serialization.Deserialization;
using FortitudeIO.Protocols.ORX.Serialization.ObjectRecycling;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.Publishing;
using FortitudeIO.Transports.Sockets.Subscription;
using FortitudeMarketsCore.Trading.ORX.Serialization;

#endregion

namespace FortitudeIO.Protocols.ORX.ClientServer;

public sealed class OrxServerMessaging : TcpSocketPublisher, IOrxPublisher
{
    private OrxClientStreamSubscriber? clientStreamSubscriber;

    public OrxServerMessaging(ISocketDispatcher dispatcher,
        IOSNetworkingController networkingController, int port, string socketUseDescription)
        : base(
            FLoggerFactory.Instance.GetLogger("OrxServer"), dispatcher, networkingController, port,
            socketUseDescription + " OrxServer")
    {
        RecyclingFactory = new OrxRecyclingFactory();
        Factory = new OrxSerializationFactory(RecyclingFactory);
    }

    public override IBinaryStreamSubscriber StreamFromSubscriber =>
        clientStreamSubscriber ??= new OrxClientStreamSubscriber(
            Logger, Dispatcher, NetworkingController, SessionDescription, this);

    internal OrxSerializationFactory Factory { get; }

    public override int SendBufferSize => 131072;

    IOrxSubscriber IOrxPublisher.StreamFromSubscriber => (IOrxSubscriber)StreamFromSubscriber;

    public OrxRecyclingFactory RecyclingFactory { get; }

    public void RegisterSerializer<T>() where T : class, IVersionedMessage, new()
    {
        var instanceOfTypeToSerialize = RecyclingFactory.Borrow<T>();
        RegisterSerializer<T>(instanceOfTypeToSerialize.MessageId);
        RecyclingFactory.Recycle(instanceOfTypeToSerialize);
    }

    public override IBinarySerializationFactory GetFactory() => Factory;

    private class OrxClientStreamSubscriber : SocketSubscriber, IOrxSubscriber
    {
        private readonly OrxServerMessaging publisher;

        public OrxClientStreamSubscriber(IFLogger logger, ISocketDispatcher dispatcher,
            IOSNetworkingController networkingController, string sessionDescription, OrxServerMessaging publisher)
            : base(logger, dispatcher, networkingController, null, sessionDescription, 0) =>
            this.publisher = publisher;

        public override int RecvBufferSize => 131072;

        public override IBinaryStreamPublisher StreamToPublisher => publisher;

        public OrxRecyclingFactory RecyclingFactory => publisher.RecyclingFactory;

        public void RegisterDeserializer<T>(Action<T, object?, ISession?> msgHandler)
            where T : class, IVersionedMessage, new()
        {
            var instanceOfTypeToDeserialize = RecyclingFactory.Borrow<T>();
            RegisterDeserializer(instanceOfTypeToDeserialize.MessageId, msgHandler);
            RecyclingFactory.Recycle(instanceOfTypeToDeserialize);
        }

        IOrxPublisher IOrxSubscriber.StreamToPublisher => publisher;

        public override IStreamDecoder GetDecoder(IMap<uint, IBinaryDeserializer> decoderDeserializers) =>
            new OrxDecoder(decoderDeserializers);

        protected override IBinaryDeserializationFactory GetFactory() => publisher.Factory;

        protected override IOSSocket CreateAndConnect(string host, int port) =>
            throw new NotImplementedException(); // relies on the publish to establish connection
    }
}
