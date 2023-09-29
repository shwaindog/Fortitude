using System;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols.ORX.Serialization.Deserialization;
using FortitudeIO.Protocols.ORX.Serialization.ObjectRecycling;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Sockets;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Client;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.Publishing;
using FortitudeIO.Transports.Sockets.Subscription;
using FortitudeMarketsCore.Trading.ORX.Serialization;

namespace FortitudeIO.Protocols.ORX.ClientServer
{
    public class OrxClientMessaging : TcpSocketClient, IOrxSubscriber
    {
        readonly OrxSerializationFactory orxSerializationFactory;

        protected readonly object SyncLock = new object();

        private IOrxPublisher streamToPublisher;

        protected OrxClientMessaging(ISocketDispatcher dispatcher, IOSNetworkingController networkingController, 
            IConnectionConfig connectionConfig, 
            string socketUseDescription, int wholeMessagesPerReceive, 
            bool keepalive = false)
            : base(
                FLoggerFactory.Instance.GetLogger("Orx"), dispatcher, networkingController,
                connectionConfig, socketUseDescription + "OrxClient", wholeMessagesPerReceive, null, keepalive)
        {
            RecyclingFactory = new OrxRecyclingFactory();
            orxSerializationFactory = new OrxSerializationFactory(RecyclingFactory);
        }

        public override int RecvBufferSize => 131072;

        public override IBinaryStreamPublisher StreamToPublisher =>
                streamToPublisher ?? (streamToPublisher =
                    new OrxServerStreamPublisher(Logger, Dispatcher, NetworkingController, SessionDescription, this));

        IOrxPublisher IOrxSubscriber.StreamToPublisher => (IOrxPublisher)StreamToPublisher;

        protected override IBinaryDeserializationFactory GetFactory()
        {
            return DeserializationFactory;
        }

        public OrxRecyclingFactory RecyclingFactory { get; }

        protected IBinaryDeserializationFactory DeserializationFactory => orxSerializationFactory;

        protected IBinarySerializationFactory SerializationFactory => orxSerializationFactory;

        public void RegisterDeserializer<T>(Action<T, object, ISession> msgHandler) where T : class, IVersionedMessage, new()
        {
            T instanceOfTypeToDeserialize = RecyclingFactory.Borrow<T>();
            RegisterDeserializer(instanceOfTypeToDeserialize.MessageId, msgHandler);
            RecyclingFactory.Recycle(instanceOfTypeToDeserialize);
        }

        public override IStreamDecoder GetDecoder(IMap<uint, IBinaryDeserializer> decoderDeserializers)
        {
            return new OrxDecoder(decoderDeserializers);
        }

        public override void Send(IVersionedMessage tradingMessage)
        {
            lock (SyncLock)
            {
                base.Send(tradingMessage);
            }
        }

        private class OrxServerStreamPublisher : TcpSocketPublisher, IOrxPublisher
        {
            private readonly OrxClientMessaging orxClientMessaging;

            public OrxServerStreamPublisher(IFLogger logger, ISocketDispatcher dispatcher,
                IOSNetworkingController networkingController, string sessionDescription,
                OrxClientMessaging orxClientMessaging) : 
                base(logger, dispatcher, networkingController, 0, sessionDescription)
            {
                this.orxClientMessaging = orxClientMessaging;
            }

            public OrxRecyclingFactory RecyclingFactory => orxClientMessaging.RecyclingFactory;

            public void RegisterSerializer<T>() where T : class, IVersionedMessage, new()
            {
                T instanceOfTypeToSerialize = RecyclingFactory.Borrow<T>();
                RegisterSerializer<T>(instanceOfTypeToSerialize.MessageId);
                RecyclingFactory.Recycle(instanceOfTypeToSerialize);
            }

            public override int SendBufferSize => 131072;

            IOrxSubscriber IOrxPublisher.StreamFromSubscriber => orxClientMessaging;
            public override IBinaryStreamSubscriber StreamFromSubscriber => orxClientMessaging;

            public override IBinarySerializationFactory GetFactory()
            {
                return orxClientMessaging.SerializationFactory;
            }
        }
    }
}