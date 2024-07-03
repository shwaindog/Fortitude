// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries.FileSystem.Config;
using FortitudeMarketsApi.Pricing.Quotes.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes.LayeredBook;
using FortitudeMarketsCore.Indicators.Config;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeTests.FortitudeMarketsCore.Pricing.PQ;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Indicators.Config;

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
}
