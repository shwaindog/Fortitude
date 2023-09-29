using System;
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

namespace FortitudeIO.Protocols.ORX.ClientServer
{
    public sealed class OrxServerMessaging : TcpSocketPublisher, IOrxPublisher
    {
        private readonly OrxSerializationFactory factory;
        private readonly OrxRecyclingFactory recyclingFactory;

        private OrxClientStreamSubscriber clientStreamSubscriber;

        public OrxServerMessaging(ISocketDispatcher dispatcher,
            IOSNetworkingController networkingController, int port, string socketUseDescription)
            : base(
                FLoggerFactory.Instance.GetLogger("OrxServer"), dispatcher, networkingController, port,
                socketUseDescription + " OrxServer")
        {
            recyclingFactory = new OrxRecyclingFactory();
            factory = new OrxSerializationFactory(recyclingFactory);
        }

        public override int SendBufferSize => 131072;

        public override IBinaryStreamSubscriber StreamFromSubscriber => clientStreamSubscriber ??
               (clientStreamSubscriber = new OrxClientStreamSubscriber( 
                   Logger, Dispatcher, NetworkingController, SessionDescription, this));

        IOrxSubscriber IOrxPublisher.StreamFromSubscriber => (IOrxSubscriber)StreamFromSubscriber;

        public override IBinarySerializationFactory GetFactory()
        {
            return factory;
        }
        
        internal OrxSerializationFactory Factory => factory;

        public OrxRecyclingFactory RecyclingFactory => recyclingFactory;
        public void RegisterSerializer<T>() where T : class, IVersionedMessage, new()
        {
            T instanceOfTypeToSerialize = RecyclingFactory.Borrow<T>();
            RegisterSerializer<T>(instanceOfTypeToSerialize.MessageId);
            RecyclingFactory.Recycle(instanceOfTypeToSerialize);
        }

        private class OrxClientStreamSubscriber : SocketSubscriber, IOrxSubscriber
        {
            private readonly OrxServerMessaging publisher;

            public OrxClientStreamSubscriber(IFLogger logger, ISocketDispatcher dispatcher, 
                IOSNetworkingController networkingController, string sessionDescription, OrxServerMessaging publisher) 
                : base(logger, dispatcher, networkingController, null, sessionDescription, 0)
            {
                this.publisher = publisher;
            }

            public override int RecvBufferSize => 131072;

            public override IBinaryStreamPublisher StreamToPublisher => publisher;

            public OrxRecyclingFactory RecyclingFactory => publisher.RecyclingFactory;
            public void RegisterDeserializer<T>(Action<T, object, ISession> msgHandler) where T : class, IVersionedMessage, new()
            {
                T instanceOfTypeToDeserialize = RecyclingFactory.Borrow<T>();
                RegisterDeserializer(instanceOfTypeToDeserialize.MessageId, msgHandler);
                RecyclingFactory.Recycle(instanceOfTypeToDeserialize);
            }

            IOrxPublisher IOrxSubscriber.StreamToPublisher => publisher;

            protected override IBinaryDeserializationFactory GetFactory()
            {
                return publisher.Factory;
            }

            public override IStreamDecoder GetDecoder(IMap<uint, IBinaryDeserializer> decoderDeserializers)
            {
                return new OrxDecoder(decoderDeserializers);
            }

            protected override IOSSocket CreateAndConnect(string host, int port)
            {
                throw new NotImplementedException(); // relies on the publish to establish connection
            }
        }
    }
}