﻿#region

using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Transports.Network.Config;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeTests.TestEnvironment;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ;

public class LocalHostPQTestSetupCommon
{
    public const string ExchangeName = "LocalHostPQTestSetupCommon";
    public const ushort ExchangeId = 1;
    public const string TestTicker = "EUR/USD";
    public const ushort TickerId = 1;

    protected static readonly IFLogger Logger =
        FLoggerFactory.Instance.GetLogger(typeof(LocalHostPQTestSetupCommon));

    public IMarketConnectionConfig DefaultServerMarketConnectionConfig = null!;
    public IMarketsConfig DefaultServerMarketsConfig = null!;

    public LastTradedFlags LastTradedFlags = LastTradedFlags.None;
    public LayerFlags LayerDetails = LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName;
    public OSNetworkingController NetworkingController = null!;
    public IPricingServerConfig PricingServerConfig = null!;
    public ISourceTickerQuoteInfo SourceTickerQuoteInfo = null!;
    public ISourceTickersConfig SourceTickersConfig = null!;
    public UpdateableTimer ThreadPoolTimer = null!;

    public void InitializeCommonConfig()
    {
        NetworkingController ??= new OSNetworkingController();
        ThreadPoolTimer ??= new UpdateableTimer("LocalHostPQTestSetupCommon");
        SourceTickersConfig ??=
            new SourceTickersConfig(new TickerConfig(TickerId, TestTicker, TickerAvailability.AllEnabled, 0.00001m, 0.1m, 100, 0.1m, 250, LayerDetails
                , 20, LastTradedFlags));
        SourceTickerQuoteInfo ??= SourceTickersConfig.GetSourceTickerInfo(ExchangeId, ExchangeName, TestTicker)!;
        PricingServerConfig ??= new PricingServerConfig(
            new NetworkTopicConnectionConfig("TestSnapshotServer", SocketConversationProtocol.TcpAcceptor,
                new List<IEndpointConfig>
                {
                    new EndpointConfig(TestMachineConfig.LoopBackIpAddress
                        , TestMachineConfig.ServerSnapshotPort)
                }, "TestSnapshotServerDescription")
            , new NetworkTopicConnectionConfig("TestUpdateServer", SocketConversationProtocol.UdpPublisher,
                new List<IEndpointConfig>
                {
                    new EndpointConfig(TestMachineConfig.LoopBackIpAddress
                        , TestMachineConfig.ServerUpdatePort, subnetMask: TestMachineConfig.NetworkSubAddress)
                }, "TestUpdateServerDescription"
                , connectionAttributes: SocketConnectionAttributes.Fast | SocketConnectionAttributes.Multicast));
        DefaultServerMarketConnectionConfig ??= new MarketConnectionConfig(1, ExchangeName, MarketConnectionType.Pricing
            , SourceTickersConfig, PricingServerConfig);
        DefaultServerMarketsConfig ??= new MarketsConfig("LocalHostPQTestSetupServer", DefaultServerMarketConnectionConfig);
    }
}