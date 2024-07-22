// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Transports.Network.Config;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes.LayeredBook;
using FortitudeTests.TestEnvironment;
using static FortitudeMarketsApi.Configuration.ClientServerConfig.MarketClassificationExtensions;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ;

public class LocalHostPQTestSetupCommon
{
    public const string ExchangeName = "LocalHostPQTestSetupCommon";
    public const ushort ExchangeId   = 1;
    public const string TestTicker   = "EUR/USD";
    public const ushort TickerId     = 1;

    protected static readonly IFLogger Logger =
        FLoggerFactory.Instance.GetLogger(typeof(LocalHostPQTestSetupCommon));

    public IMarketConnectionConfig DefaultServerMarketConnectionConfig = null!;
    public IMarketsConfig          DefaultServerMarketsConfig          = null!;
    public ISourceTickerInfo       FirstTickerInfo                     = null!;

    public LastTradedFlags        LastTradedFlags      = LastTradedFlags.None;
    public LayerFlags             LayerDetails         = LayerFlags.Price | LayerFlags.Volume | LayerFlags.SourceName;
    public OSNetworkingController NetworkingController = null!;
    public IPricingServerConfig   PricingServerConfig  = null!;
    public ISourceTickersConfig   SourceTickersConfig  = null!;
    public IUpdateableTimer       ThreadPoolTimer      = null!;

    public void InitializeCommonConfig()
    {
        NetworkingController ??= new OSNetworkingController();
        ThreadPoolTimer      ??= TimerContext.CreateUpdateableTimer("LocalHostPQTestSetupCommon");
        SourceTickersConfig ??=
            new SourceTickersConfig
                (new TickerConfig
                    (TickerId, TestTicker, TickerAvailability.PricingAndTradingEnabled, TickerDetailLevel.Level3Quote, Unknown
                   , 0.000001m, 0.0001m, 0.1m, 100, 0.1m
                   , 250, 10_000, LayerDetails, 20, LastTradedFlags));
        FirstTickerInfo ??= SourceTickersConfig.GetSourceTickerInfo(ExchangeId, ExchangeName, TestTicker)!;
        PricingServerConfig ??=
            new PricingServerConfig
                (new NetworkTopicConnectionConfig
                     ("TestSnapshotServer", SocketConversationProtocol.TcpAcceptor
                    , new List<IEndpointConfig>
                      {
                          new EndpointConfig(TestMachineConfig.LoopBackIpAddress
                                           , TestMachineConfig.ServerSnapshotPort)
                      }, "TestSnapshotServerDescription")
               , new NetworkTopicConnectionConfig
                     ("TestUpdateServer", SocketConversationProtocol.UdpPublisher
                    , new List<IEndpointConfig>
                      {
                          new EndpointConfig(TestMachineConfig.LoopBackIpAddress
                                           , TestMachineConfig.ServerUpdatePort
                                           , subnetMask: TestMachineConfig.NetworkSubAddress)
                      }, "TestUpdateServerDescription"
                    , connectionAttributes: SocketConnectionAttributes.Fast | SocketConnectionAttributes.Multicast));
        DefaultServerMarketConnectionConfig ??= new MarketConnectionConfig(1, ExchangeName, MarketConnectionType.Pricing
                                                                         , SourceTickersConfig, PricingServerConfig);
        DefaultServerMarketsConfig ??= new MarketsConfig("LocalHostPQTestSetupServer", DefaultServerMarketConnectionConfig);
    }
}
