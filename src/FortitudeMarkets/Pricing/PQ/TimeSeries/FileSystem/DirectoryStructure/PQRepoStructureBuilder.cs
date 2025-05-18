// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeIO.TimeSeries.FileSystem.Config;
using FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Candles;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.PQ.TimeSeries.FileSystem.DirectoryStructure;

public class PQRepoPathBuilder : RepoPathBuilder
{
    public PQRepoPathBuilder(ISingleRepositoryBuilderConfig repoConfig) : base(repoConfig) { }

    public override IPathFile PriceFile(DiscreetTimePeriod? from = null, DiscreetTimePeriod? to = null)
    {
        var priceFileStructure = base.PriceFile(from, to);
        priceFileStructure.FileEntryFactoryRegistry[typeof(ITickInstant)]     = new PQOneWeekQuoteRepoFileFactory<ITickInstant>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(ILevel1Quote)]     = new PQOneWeekQuoteRepoFileFactory<ILevel1Quote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(ILevel2Quote)]     = new PQOneWeekQuoteRepoFileFactory<ILevel2Quote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(ILevel3Quote)]     = new PQOneWeekQuoteRepoFileFactory<ILevel3Quote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(IPublishableTickInstant)]     = new PQOneWeekQuoteRepoFileFactory<IPublishableTickInstant>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(IPublishableLevel1Quote)]     = new PQOneWeekQuoteRepoFileFactory<IPublishableLevel1Quote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(IPublishableLevel2Quote)]     = new PQOneWeekQuoteRepoFileFactory<IPublishableLevel2Quote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(IPublishableLevel3Quote)]     = new PQOneWeekQuoteRepoFileFactory<IPublishableLevel3Quote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(TickInstant)]      = new PQOneWeekQuoteRepoFileFactory<TickInstant>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(TickInstant)]      = new PQOneWeekQuoteRepoFileFactory<TickInstant>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(Level1PriceQuote)] = new PQOneWeekQuoteRepoFileFactory<Level1PriceQuote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(Level2PriceQuote)] = new PQOneWeekQuoteRepoFileFactory<Level2PriceQuote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(Level3PriceQuote)] = new PQOneWeekQuoteRepoFileFactory<Level3PriceQuote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(PublishableTickInstant)]      = new PQOneWeekQuoteRepoFileFactory<PublishableTickInstant>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(PublishableLevel1PriceQuote)] = new PQOneWeekQuoteRepoFileFactory<PublishableLevel1PriceQuote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(PublishableLevel2PriceQuote)] = new PQOneWeekQuoteRepoFileFactory<PublishableLevel2PriceQuote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(PublishableLevel3PriceQuote)] = new PQOneWeekQuoteRepoFileFactory<PublishableLevel3PriceQuote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(IPQTickInstant)]     = new PQOneWeekQuoteRepoFileFactory<IPQTickInstant>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(IPQLevel1Quote)]     = new PQOneWeekQuoteRepoFileFactory<IPQLevel1Quote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(IPQLevel2Quote)]     = new PQOneWeekQuoteRepoFileFactory<IPQLevel2Quote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(IPQLevel3Quote)]     = new PQOneWeekQuoteRepoFileFactory<IPQLevel3Quote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(IPQPublishableTickInstant)]     = new PQOneWeekQuoteRepoFileFactory<IPQPublishableTickInstant>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(IPQPublishableLevel1Quote)]     = new PQOneWeekQuoteRepoFileFactory<IPQPublishableLevel1Quote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(IPQPublishableLevel2Quote)]     = new PQOneWeekQuoteRepoFileFactory<IPQPublishableLevel2Quote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(IPQPublishableLevel3Quote)]     = new PQOneWeekQuoteRepoFileFactory<IPQPublishableLevel3Quote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(PQTickInstant)]    = new PQOneWeekQuoteRepoFileFactory<PQTickInstant>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(PQLevel1Quote)]    = new PQOneWeekQuoteRepoFileFactory<PQLevel1Quote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(PQLevel2Quote)]    = new PQOneWeekQuoteRepoFileFactory<PQLevel2Quote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(PQLevel3Quote)]    = new PQOneWeekQuoteRepoFileFactory<PQLevel3Quote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(PQPublishableTickInstant)]    = new PQOneWeekQuoteRepoFileFactory<PQPublishableTickInstant>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(PQPublishableLevel1Quote)]    = new PQOneWeekQuoteRepoFileFactory<PQPublishableLevel1Quote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(PQPublishableLevel2Quote)]    = new PQOneWeekQuoteRepoFileFactory<PQPublishableLevel2Quote>();
        priceFileStructure.FileEntryFactoryRegistry[typeof(PQPublishableLevel3Quote)]    = new PQOneWeekQuoteRepoFileFactory<PQPublishableLevel3Quote>();
        return priceFileStructure;
    }

    public override IPathFile CandleFile(DiscreetTimePeriod? from = null, DiscreetTimePeriod? to = null)
    {
        var priceFileStructure = base.CandleFile(from, to);
        if (from == null || to == null) return priceFileStructure;
        var timeSpan = from.Value.TimeSpan;
        if (timeSpan >= TimeBoundaryPeriod.FifteenSeconds.AveragePeriodTimeSpan() &&
            timeSpan <= TimeBoundaryPeriod.FiveMinutes.AveragePeriodTimeSpan())
        {
            priceFileStructure.FileEntryFactoryRegistry[typeof(ICandle)]
                = new PQOneWeekCandleRepoFileFactory<ICandle>();
            priceFileStructure.FileEntryFactoryRegistry[typeof(Candle)]
                = new PQOneWeekCandleRepoFileFactory<Candle>();
            priceFileStructure.FileEntryFactoryRegistry[typeof(PQCandle)]
                = new PQOneWeekCandleRepoFileFactory<PQCandle>();
            priceFileStructure.FileEntryFactoryRegistry[typeof(PQStorageCandle)]
                = new PQOneWeekCandleRepoFileFactory<PQStorageCandle>();
        }
        else if (timeSpan >= TimeBoundaryPeriod.TenMinutes.AveragePeriodTimeSpan() &&
                 timeSpan <= TimeBoundaryPeriod.OneHour.AveragePeriodTimeSpan())
        {
            priceFileStructure.FileEntryFactoryRegistry[typeof(ICandle)]
                = new PQOneMonthCandleRepoFileFactory<ICandle>();
            priceFileStructure.FileEntryFactoryRegistry[typeof(Candle)]
                = new PQOneMonthCandleRepoFileFactory<Candle>();
            priceFileStructure.FileEntryFactoryRegistry[typeof(PQCandle)]
                = new PQOneMonthCandleRepoFileFactory<PQCandle>();
            priceFileStructure.FileEntryFactoryRegistry[typeof(PQStorageCandle)]
                = new PQOneMonthCandleRepoFileFactory<PQStorageCandle>();
        }
        else if (timeSpan >= TimeBoundaryPeriod.FourHours.AveragePeriodTimeSpan() && timeSpan < TimeBoundaryPeriod.OneWeek.AveragePeriodTimeSpan())
        {
            priceFileStructure.FileEntryFactoryRegistry[typeof(ICandle)]
                = new PQOneYearPriceCandleRepoFileFactory<ICandle>();
            priceFileStructure.FileEntryFactoryRegistry[typeof(Candle)]
                = new PQOneYearPriceCandleRepoFileFactory<Candle>();
            priceFileStructure.FileEntryFactoryRegistry[typeof(PQCandle)]
                = new PQOneYearPriceCandleRepoFileFactory<PQCandle>();
            priceFileStructure.FileEntryFactoryRegistry[typeof(PQStorageCandle)]
                = new PQOneYearPriceCandleRepoFileFactory<PQStorageCandle>();
        }
        else if (timeSpan >= TimeBoundaryPeriod.OneWeek.AveragePeriodTimeSpan() && timeSpan <= TimeBoundaryPeriod.OneMonth.AveragePeriodTimeSpan())
        {
            priceFileStructure.FileEntryFactoryRegistry[typeof(ICandle)]
                = new PQDecenniallyCandleRepoFileFactory<ICandle>();
            priceFileStructure.FileEntryFactoryRegistry[typeof(Candle)]
                = new PQDecenniallyCandleRepoFileFactory<Candle>();
            priceFileStructure.FileEntryFactoryRegistry[typeof(PQCandle)]
                = new PQDecenniallyCandleRepoFileFactory<PQCandle>();
            priceFileStructure.FileEntryFactoryRegistry[typeof(PQStorageCandle)]
                = new PQDecenniallyCandleRepoFileFactory<PQStorageCandle>();
        }
        else if (timeSpan >= TimeBoundaryPeriod.OneYear.AveragePeriodTimeSpan())
        {
            priceFileStructure.FileEntryFactoryRegistry[typeof(ICandle)]
                = new PQUnlimitedCandleRepoFileFactory<ICandle>();
            priceFileStructure.FileEntryFactoryRegistry[typeof(Candle)]
                = new PQUnlimitedCandleRepoFileFactory<Candle>();
            priceFileStructure.FileEntryFactoryRegistry[typeof(PQCandle)]
                = new PQUnlimitedCandleRepoFileFactory<PQCandle>();
            priceFileStructure.FileEntryFactoryRegistry[typeof(PQStorageCandle)]
                = new PQUnlimitedCandleRepoFileFactory<PQStorageCandle>();
        }

        return priceFileStructure;
    }
}
