// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.Config;
using FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Summaries;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Summaries;
using FortitudeMarketsCore.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Summaries;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.DirectoryStructure;

public class PQRepoPathBuilder : RepoPathBuilder
{
    public PQRepoPathBuilder(ISingleRepositoryBuilderConfig repoConfig) : base(repoConfig) { }

    public override IPathFile PriceFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null)
    {
        var priceFileStructure = base.PriceFile(from, to);
        priceFileStructure.FileEntryFactoryRegistry[typeof(ILevel0Quote)]     = new PQOneWeekQuoteRepoFileFactory<ILevel0Quote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(ILevel1Quote)]     = new PQOneWeekQuoteRepoFileFactory<ILevel1Quote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(ILevel2Quote)]     = new PQOneWeekQuoteRepoFileFactory<ILevel2Quote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(ILevel3Quote)]     = new PQOneWeekQuoteRepoFileFactory<ILevel3Quote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(Level0PriceQuote)] = new PQOneWeekQuoteRepoFileFactory<Level0PriceQuote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(Level1PriceQuote)] = new PQOneWeekQuoteRepoFileFactory<Level1PriceQuote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(Level2PriceQuote)] = new PQOneWeekQuoteRepoFileFactory<Level2PriceQuote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(Level3PriceQuote)] = new PQOneWeekQuoteRepoFileFactory<Level3PriceQuote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(PQLevel0Quote)]    = new PQOneWeekQuoteRepoFileFactory<PQLevel0Quote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(PQLevel1Quote)]    = new PQOneWeekQuoteRepoFileFactory<PQLevel1Quote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(PQLevel2Quote)]    = new PQOneWeekQuoteRepoFileFactory<PQLevel2Quote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(PQLevel3Quote)]    = new PQOneWeekQuoteRepoFileFactory<PQLevel3Quote>();
        return priceFileStructure;
    }

    public override IPathFile PriceSummaryFile(TimeSeriesPeriod? from = null, TimeSeriesPeriod? to = null)
    {
        var priceFileStructure = base.PriceSummaryFile(from, to);
        if (from == null || to == null) return priceFileStructure;
        switch (from.Value)
        {
            case >= TimeSeriesPeriod.FifteenSeconds and <= TimeSeriesPeriod.FiveMinutes:
                priceFileStructure.FileEntryFactoryRegistry[typeof(IPricePeriodSummary)]
                    = new PQOneWeekPricePeriodSummaryRepoFileFactory<IPricePeriodSummary>();
                priceFileStructure.FileEntryFactoryRegistry[typeof(PricePeriodSummary)]
                    = new PQOneWeekPricePeriodSummaryRepoFileFactory<PricePeriodSummary>();
                priceFileStructure.FileEntryFactoryRegistry[typeof(PQPricePeriodSummary)]
                    = new PQOneWeekPricePeriodSummaryRepoFileFactory<PQPricePeriodSummary>();
                priceFileStructure.FileEntryFactoryRegistry[typeof(PQPriceStoragePeriodSummary)]
                    = new PQOneWeekPricePeriodSummaryRepoFileFactory<PQPriceStoragePeriodSummary>();
                break;
            case >= TimeSeriesPeriod.TenMinutes and <= TimeSeriesPeriod.OneHour:
                priceFileStructure.FileEntryFactoryRegistry[typeof(IPricePeriodSummary)]
                    = new PQOneMonthPricePeriodSummaryRepoFileFactory<IPricePeriodSummary>();
                priceFileStructure.FileEntryFactoryRegistry[typeof(PricePeriodSummary)]
                    = new PQOneMonthPricePeriodSummaryRepoFileFactory<PricePeriodSummary>();
                priceFileStructure.FileEntryFactoryRegistry[typeof(PQPricePeriodSummary)]
                    = new PQOneMonthPricePeriodSummaryRepoFileFactory<PQPricePeriodSummary>();
                priceFileStructure.FileEntryFactoryRegistry[typeof(PQPriceStoragePeriodSummary)]
                    = new PQOneMonthPricePeriodSummaryRepoFileFactory<PQPriceStoragePeriodSummary>();
                break;
            case TimeSeriesPeriod.FourHours:
                priceFileStructure.FileEntryFactoryRegistry[typeof(IPricePeriodSummary)]
                    = new PQOneYearPricePeriodSummaryRepoFileFactory<IPricePeriodSummary>();
                priceFileStructure.FileEntryFactoryRegistry[typeof(PricePeriodSummary)]
                    = new PQOneYearPricePeriodSummaryRepoFileFactory<PricePeriodSummary>();
                priceFileStructure.FileEntryFactoryRegistry[typeof(PQPricePeriodSummary)]
                    = new PQOneYearPricePeriodSummaryRepoFileFactory<PQPricePeriodSummary>();
                priceFileStructure.FileEntryFactoryRegistry[typeof(PQPriceStoragePeriodSummary)]
                    = new PQOneYearPricePeriodSummaryRepoFileFactory<PQPriceStoragePeriodSummary>();
                break;
            case >= TimeSeriesPeriod.OneWeek and <= TimeSeriesPeriod.OneMonth:
                priceFileStructure.FileEntryFactoryRegistry[typeof(IPricePeriodSummary)]
                    = new PQDecenniallyPricePeriodSummaryRepoFileFactory<IPricePeriodSummary>();
                priceFileStructure.FileEntryFactoryRegistry[typeof(PricePeriodSummary)]
                    = new PQDecenniallyPricePeriodSummaryRepoFileFactory<PricePeriodSummary>();
                priceFileStructure.FileEntryFactoryRegistry[typeof(PQPricePeriodSummary)]
                    = new PQDecenniallyPricePeriodSummaryRepoFileFactory<PQPricePeriodSummary>();
                priceFileStructure.FileEntryFactoryRegistry[typeof(PQPriceStoragePeriodSummary)]
                    = new PQDecenniallyPricePeriodSummaryRepoFileFactory<PQPriceStoragePeriodSummary>();
                break;
            case TimeSeriesPeriod.OneYear:
                priceFileStructure.FileEntryFactoryRegistry[typeof(IPricePeriodSummary)]
                    = new PQUnlimitedPricePeriodSummaryRepoFileFactory<IPricePeriodSummary>();
                priceFileStructure.FileEntryFactoryRegistry[typeof(PricePeriodSummary)]
                    = new PQUnlimitedPricePeriodSummaryRepoFileFactory<PricePeriodSummary>();
                priceFileStructure.FileEntryFactoryRegistry[typeof(PQPricePeriodSummary)]
                    = new PQUnlimitedPricePeriodSummaryRepoFileFactory<PQPricePeriodSummary>();
                priceFileStructure.FileEntryFactoryRegistry[typeof(PQPriceStoragePeriodSummary)]
                    = new PQUnlimitedPricePeriodSummaryRepoFileFactory<PQPriceStoragePeriodSummary>();
                break;
        }

        return priceFileStructure;
    }
}
