// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Transports.Network.Config;
using FortitudeMarkets.Configuration;
using FortitudeMarkets.Configuration.PricingConfig;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeTests.TestEnvironment;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ;

public class LocalHostPQTestSetupCommon
{
    public const string ExchangeName = "LocalHostPQTestSetupCommon";
    public const ushort ExchangeId   = 1;
    public const string FirstTestTicker   = "EUR/USD";
    public const ushort FirstTickerId     = 1;
    public const string SecondTestTicker   = "USD/JPY";
    public const ushort SecondTickerId     = 2;
    public const CountryCityCodes HostLocation =  CountryCityCodes.AUinMEL;

    protected static readonly IFLogger Logger =
        FLoggerFactory.Instance.GetLogger(typeof(LocalHostPQTestSetupCommon));

    public IMarketConnectionConfig DefaultServerMarketConnectionConfig = null!;
    public IMarketsConfig          DefaultServerMarketsConfig          = null!;
    public ISourceTickerInfo       FirstTickerInfo                     = null!;
    public ISourceTickerInfo       SecondTickerInfo                     = null!;

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
                (CountryCityCodes.AUinSYD, new TickerConfig
                    (FirstTickerId, FirstTestTicker, TickerAvailability.PricingAndTradingEnabled, TickerQuoteDetailLevel.Level3Quote, MarketClassification.Unknown
                   , 0.000001m, 0.0001m, 0.1m, 100, 0.1m
                   , 250, 10_000, LayerDetails, 20, LastTradedFlags)
                ,new TickerConfig
                     (SecondTickerId, SecondTestTicker, TickerAvailability.PricingAndTradingEnabled, TickerQuoteDetailLevel.Level3Quote, MarketClassification.Unknown
                    , 0.000001m, 0.0001m, 0.1m, 100, 0.1m
                    , 250, 10_000, LayerDetails, 20, LastTradedFlags));
        PricingServerConfig ??=
            new PricingServerConfig
                (new NetworkTopicConnectionConfig
                     ("TestSnapshotServer", SocketConversationProtocol.TcpAcceptor
                    , new List<IEndpointConfig>
                      {
                          new EndpointConfig(TestMachineConfig.LoopBackIpAddress, TestMachineConfig.ServerSnapshotPort, CountryCityCodes.AUinMEL)
                      }, "TestSnapshotServerDescription")
               , new NetworkTopicConnectionConfig
                     ("TestUpdateServer", SocketConversationProtocol.UdpPublisher
                    , new List<IEndpointConfig>
                      {
                          new EndpointConfig(TestMachineConfig.LoopBackIpAddress
                                           , TestMachineConfig.ServerUpdatePort, CountryCityCodes.AUinMEL
                                           , subnetMask: TestMachineConfig.NetworkSubAddress)
                      }, "TestUpdateServerDescription"
                    , connectionAttributes: SocketConnectionAttributes.Fast | SocketConnectionAttributes.Multicast));
        FirstTickerInfo  ??= SourceTickersConfig.GetSourceTickerInfo(ExchangeId, ExchangeName,  FirstTestTicker, HostLocation, true)!;
        SecondTickerInfo ??= SourceTickersConfig.GetSourceTickerInfo(ExchangeId, ExchangeName,  SecondTestTicker, HostLocation, true)!;
        DefaultServerMarketConnectionConfig ??= new MarketConnectionConfig(1, ExchangeName, MarketConnectionType.Pricing, CountryCityCodes.AUinMEL
                                                                         , SourceTickersConfig, PricingServerConfig);
        DefaultServerMarketsConfig ??= new MarketsConfig("LocalHostPQTestSetupServer", DefaultServerMarketConnectionConfig);

        
    }
}
