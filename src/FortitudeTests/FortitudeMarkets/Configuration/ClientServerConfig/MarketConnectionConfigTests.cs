// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Configuration.ClientServerConfig;
using FortitudeMarkets.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarkets.Configuration.ClientServerConfig.TradingConfig;
using FortitudeTests.FortitudeIO.Transports.Network.Config;

#endregion

namespace FortitudeTests.FortitudeMarkets.Configuration.ClientServerConfig;

public class MarketConnectionConfigTests
{
    public static MarketConnectionConfig DummyConnectionConfig =>
        new(23, "TestServerName", MarketConnectionType.Trading, new SourceTickersConfig(),
            new PricingServerConfig(NetworkTopicConnectionConfigTests.DummyTopicConnectionConfig
                                  , NetworkTopicConnectionConfigTests.DummyTopicConnectionConfig),
            new TradingServerConfig(NetworkTopicConnectionConfigTests.DummyTopicConnectionConfig));

    public static IList<IMarketConnectionConfig> ListOfSampleServerConfigs =>
        new List<IMarketConnectionConfig>
        {
            new MarketConnectionConfig(2, "TestServerName1", MarketConnectionType.Trading, new SourceTickersConfig(),
                                       new PricingServerConfig(NetworkTopicConnectionConfigTests.DummyTopicConnectionConfig
                                                             , NetworkTopicConnectionConfigTests.DummyTopicConnectionConfig),
                                       new TradingServerConfig(NetworkTopicConnectionConfigTests.DummyTopicConnectionConfig))
          , new MarketConnectionConfig(3, "TestServerName2", MarketConnectionType.Trading, new SourceTickersConfig(),
                                       new PricingServerConfig(NetworkTopicConnectionConfigTests.DummyTopicConnectionConfig
                                                             , NetworkTopicConnectionConfigTests.DummyTopicConnectionConfig),
                                       new TradingServerConfig(NetworkTopicConnectionConfigTests.DummyTopicConnectionConfig))
          , new MarketConnectionConfig(4, "TestServerName3", MarketConnectionType.Trading, new SourceTickersConfig(),
                                       new PricingServerConfig(NetworkTopicConnectionConfigTests.DummyTopicConnectionConfig
                                                             , NetworkTopicConnectionConfigTests.DummyTopicConnectionConfig),
                                       new TradingServerConfig(NetworkTopicConnectionConfigTests.DummyTopicConnectionConfig))
        };

    public static void UpdateServerConfigWithValues(IMarketConnectionConfig updateConfig, string name, MarketConnectionType marketConnectionType)
    {
        updateConfig.SourceName           = name;
        updateConfig.MarketConnectionType = marketConnectionType;
    }
}
