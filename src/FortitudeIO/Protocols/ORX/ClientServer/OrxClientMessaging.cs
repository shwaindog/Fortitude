﻿#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeIO.Protocols.ORX.Serialization.Deserialization;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Client;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.Publishing;
using FortitudeIO.Transports.Sockets.Subscription;

#endregion

namespace FortitudeIO.Protocols.ORX.ClientServer;

public class OrxClientMessaging : TcpSocketClient, IOrxSubscriber
{
    private readonly OrxSerializationFactory orxSerializationFactory;

    protected readonly object SyncLock = new();

    private IOrxPublisher? streamToPublisher;

    protected OrxClientMessaging(ISocketDispatcher dispatcher, IOSNetworkingController networkingController,
        IConnectionConfig connectionConfig,
        string socketUseDescription, int wholeMessagesPerReceive,
        bool keepalive = false)
        : base(
            FLoggerFactory.Instance.GetLogger("Orx"), dispatcher, networkingController,
            connectionConfig, socketUseDescription + "OrxClient", wholeMessagesPerReceive, null, keepalive)
    {
        RecyclingFactory = new Recycler();
        orxSerializationFactory = new OrxSerializationFactory(RecyclingFactory);
    }

    protected IBinaryDeserializationFactory DeserializationFactory => orxSerializationFactory;

    protected IBinarySerializationFactory SerializationFactory => orxSerializationFactory;

    public override int RecvBufferSize => 131072;

    public override IBinaryStreamPublisher StreamToPublisher =>
        streamToPublisher
            ??= new OrxServerStreamPublisher(Logger, Dispatcher, NetworkingController, SessionDescription, this);

    IOrxPublisher IOrxSubscriber.StreamToPublisher => (IOrxPublisher)StreamToPublisher;

    public IRecycler RecyclingFactory { get; }

    public void RegisterDeserializer<T>(Action<T, object?, ISession?>? msgHandler)
        where T : class, IVersionedMessage, new()
    {
        var instanceOfTypeToDeserialize = RecyclingFactory.Borrow<T>();
        RegisterDeserializer(instanceOfTypeToDeserialize.MessageId, msgHandler);
        instanceOfTypeToDeserialize.DecrementRefCount();
    }

    public override IStreamDecoder GetDecoder(IMap<uint, IBinaryDeserializer> decoderDeserializers) =>
        new OrxDecoder(decoderDeserializers);

    public override void Send(IVersionedMessage tradingMessage)
    {
        lock (SyncLock)
        {
            base.Send(tradingMessage);
        }
    }

    protected override IBinaryDeserializationFactory GetFactory() => DeserializationFactory;

    private class OrxServerStreamPublisher : TcpSocketPublisher, IOrxPublisher
    {
        private readonly OrxClientMessaging orxClientMessaging;

        public OrxServerStreamPublisher(IFLogger logger, ISocketDispatcher dispatcher,
            IOSNetworkingController networkingController, string sessionDescription,
            OrxClientMessaging orxClientMessaging) :
            base(logger, dispatcher, networkingController, 0, sessionDescription) =>
            this.orxClientMessaging = orxClientMessaging;

        public override IBinaryStreamSubscriber StreamFromSubscriber => orxClientMessaging;

        public IRecycler RecyclingFactory => orxClientMessaging.RecyclingFactory;

        public void RegisterSerializer<T>() where T : class, IVersionedMessage, new()
        {
            var instanceOfTypeToSerialize = RecyclingFactory.Borrow<T>();
            RegisterSerializer<T>(instanceOfTypeToSerialize.MessageId);
            instanceOfTypeToSerialize.DecrementRefCount();
        }

        public override int SendBufferSize => 131072;

        IOrxSubscriber IOrxPublisher.StreamFromSubscriber => orxClientMessaging;

        public override IBinarySerializationFactory GetFactory() => orxClientMessaging.SerializationFactory;
    }
}
