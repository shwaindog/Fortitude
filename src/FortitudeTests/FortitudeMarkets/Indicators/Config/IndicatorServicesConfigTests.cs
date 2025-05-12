// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries.FileSystem.Config;
using FortitudeMarkets.Indicators.Config;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeTests.FortitudeMarkets.Pricing.PQ;

#endregion

namespace FortitudeTests.FortitudeMarkets.Indicators.Config;

public class IndicatorServicesConfigTests
{
    public static IndicatorServicesConfig UnitTestConfig(DirectoryInfo repositoryDir)
    {
        var serverConfig = new LocalHostPQTestSetupCommon
        {
            LayerDetails = LayerFlags.Price | LayerFlags.Volume | LayerFlags.OrderTraderName | LayerFlags.OrderSize
          , LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedPrice | LastTradedFlags.PaidOrGiven |
                              LastTradedFlags.LastTradedTime
        };
        var dymwiSingleLocalRepository = FileRepositoryConfig.CreateDymwiSingleLocalRepository<PQRepoPathBuilder>(repositoryDir, "UnitTest");
        dymwiSingleLocalRepository.RepositoryConfig.CreateIfNotExists = true;
        var config = new IndicatorServicesConfig
            (serverConfig.DefaultServerMarketsConfig
           , dymwiSingleLocalRepository
           , new PersistenceConfig(true));

        return config;
    }

    public static IndicatorServicesConfig UnitTestNoRepositoryConfig()
    {
        var serverConfig = new LocalHostPQTestSetupCommon
        {
            LayerDetails = LayerFlags.Price | LayerFlags.Volume | LayerFlags.OrderTraderName | LayerFlags.OrderSize
          , LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedPrice | LastTradedFlags.PaidOrGiven |
                              LastTradedFlags.LastTradedTime
        };
        var config = new IndicatorServicesConfig(serverConfig.DefaultServerMarketsConfig);

        return config;
    }
}
