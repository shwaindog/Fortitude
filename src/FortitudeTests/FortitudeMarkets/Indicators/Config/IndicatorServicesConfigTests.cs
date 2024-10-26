// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries.FileSystem.Config;
using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeMarkets.Indicators.Config;
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
            LayerDetails = LayerFlags.Price | LayerFlags.Volume | LayerFlags.TraderName | LayerFlags.TraderSize
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
            LayerDetails = LayerFlags.Price | LayerFlags.Volume | LayerFlags.TraderName | LayerFlags.TraderSize
          , LastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedPrice | LastTradedFlags.PaidOrGiven |
                              LastTradedFlags.LastTradedTime
        };
        var config = new IndicatorServicesConfig(serverConfig.DefaultServerMarketsConfig);

        return config;
    }
}
