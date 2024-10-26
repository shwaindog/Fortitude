﻿#region

using FortitudeBusRules.Connectivity.Network;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.Conversations;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.State;
using FortitudeMarkets.Pricing.PQ.Serdes;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Subscription.BusRules;

public sealed class PQPricingClientUpdatesConversationSubscriber : ConversationSubscriber
{
    private static ISocketFactoryResolver? socketFactories;

    public PQPricingClientUpdatesConversationSubscriber(ISocketSessionContext socketSessionContext, IStreamControls streamControls
        , IMessageDeserializationRepository sharedDeserializationRepo)
        : base(socketSessionContext, streamControls)
    {
        socketSessionContext.SocketFactoryResolver.SocketReceiverFactory.ConfigureNewSocketReceiver += socketReceiver =>
        {
            var currentDeserializerRepo = socketReceiver!.Decoder!.MessageDeserializationRepository;
            currentDeserializerRepo.AttachToEndOfConnectedFallbackRepos(sharedDeserializationRepo);
        };
    }

    public static ISocketFactoryResolver SocketFactories
    {
        get => socketFactories ??= SocketFactoryResolver.GetRealSocketFactories();
        set => socketFactories = value;
    }

    public static PQPricingClientUpdatesConversationSubscriber BuildUdpSubscriber(INetworkTopicConnectionConfig networkConnectionConfig
        , ISocketDispatcherResolver socketDispatcherResolver, IMessageDeserializationRepository sharedDeserializationRepo)
    {
        var conversationType = ConversationType.Subscriber;
        var conversationProtocol = SocketConversationProtocol.UdpSubscriber;

        var sockFactories = SocketFactories;

        var serdesFactory = new PQClientClientSerdesRepositoryFactory();

        var socketSessionContext = new BusSocketSessionContext(networkConnectionConfig.TopicName + "Subscriber", conversationType
            , conversationProtocol,
            networkConnectionConfig, sockFactories, serdesFactory, socketDispatcherResolver);

        var streamControls = sockFactories.StreamControlsFactory.ResolveStreamControls(socketSessionContext);

        return new PQPricingClientUpdatesConversationSubscriber(socketSessionContext, streamControls, sharedDeserializationRepo);
    }
}
