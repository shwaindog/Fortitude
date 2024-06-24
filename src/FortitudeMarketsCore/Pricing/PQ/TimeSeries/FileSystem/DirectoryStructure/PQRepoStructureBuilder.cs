// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.Config;
using FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeMarketsApi.Pricing.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.DirectoryStructure;

public class PQRepoPathBuilder : RepoPathBuilder
{
    public PQRepoPathBuilder(ISingleRepositoryBuilderConfig repoConfig) : base(repoConfig) { }

    public override IPathFile PriceFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null)
    {
        var priceFileStructure = base.PriceFile(from, to);
        priceFileStructure.FileEntryFactoryRegistry[typeof(ILevel0Quote)] = new PQOneWeekQuoteRepoFileFactory<ILevel0Quote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(ILevel1Quote)] = new PQOneWeekQuoteRepoFileFactory<ILevel1Quote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(ILevel2Quote)] = new PQOneWeekQuoteRepoFileFactory<ILevel2Quote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(ILevel3Quote)] = new PQOneWeekQuoteRepoFileFactory<ILevel3Quote>();
        return priceFileStructure;
    }
}
