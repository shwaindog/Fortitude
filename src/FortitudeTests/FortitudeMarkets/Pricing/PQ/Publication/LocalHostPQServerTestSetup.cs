﻿#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.Receiving;
using FortitudeMarkets.Pricing.PQ.Publication;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Publication;

public class LocalHostPQServerTestSetupBase : LocalHostPQTestSetupCommon
{
    public PQServerHeartBeatSender HeartBeatSender = null!;
    public INameIdLookupGenerator NameIdLookupGenerator = null!;
    public Func<INetworkTopicConnectionConfig, ISocketDispatcherResolver, IPQSnapshotServer> PqSnapshotFactory = null!;
    public Func<INetworkTopicConnectionConfig, ISocketDispatcherResolver, IPQUpdateServer> PqUpdateFactory = null!;
    public ISocketDispatcherResolver ServerDispatcherResolver = null!;

    public void InitializeServerPrereqs()
    {
        InitializeCommonConfig();
        ServerDispatcherResolver ??= new SimpleSocketDispatcherResolver(new SocketDispatcher(
            new SimpleSocketAsyncValueTaskRingPollerListener("PQServerListener", 1
                , new SocketSelector(1, NetworkingController), ThreadPoolTimer),
            new SimpleAsyncValueTaskSocketRingPollerSender("PQServerSender", 1)));
        NameIdLookupGenerator ??= new NameIdLookupGenerator();
        PqSnapshotFactory ??= PQSnapshotServer.BuildTcpResponder;
        PqUpdateFactory ??= PQUpdatePublisher.BuildUdpMulticastPublisher;
        HeartBeatSender ??= new PQServerHeartBeatSender();
    }
}
