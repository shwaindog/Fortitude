#region

using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.TradingConfig;
using FortitudeTests.FortitudeIO.Transports.Network.Config;

#endregion

namespace FortitudeTests.FortitudeMarketsApi.Configuration.ClientServerConfig;

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
        updateConfig.Name = name;
        updateConfig.MarketConnectionType = marketConnectionType;
    }
}
