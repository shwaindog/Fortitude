// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Rules.Common.TimeSeries;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem;
using FortitudeMarkets.Config;
using FortitudeMarkets.Indicators.Persistence;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeTests.FortitudeBusRules.BusMessaging;
using FortitudeTests.FortitudeMarkets.Indicators.Config;
using static FortitudeCommon.Chronometry.TimeBoundaryPeriod;
using static FortitudeIO.Transports.Network.Config.CountryCityCodes;
using static FortitudeTests.FortitudeCommon.Extensions.DirectoryInfoExtensionsTests;
using static FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Candles.CandleTests;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Indicators.Persistence;

[TestClass]
public class CandleFilePersisterRuleTests : OneOfEachMessageQueueTypeTestSetup
{
    private const string TestEntriesToPersistAddress = "TestEntriesToPersistAddress";

    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(CandleFilePersisterRuleTests));

    private readonly ISourceTickerInfo tickerInfo = new SourceTickerInfo
        (1, "SourceName", 1, "TickerName", Level1Quote, MarketClassification.Unknown
       , AUinMEL, AUinMEL, AUinMEL
       , 1, 0.001m, 0.1m, 10m, 100m, 10m);

    private decimal highLowSpread;
    private decimal mid1;
    private decimal mid2;

    private TimeBoundaryPeriod period;

    private CandlePersisterParams persisterParams;

    private CandleFilePersisterRule<Candle>? persisterRule;

    private DateTime      persistStartTime;
    private IRecycler     recycler    = null!;
    private DirectoryInfo repoRootDir = null!;
    private DateTime      seedDateTime;
    private decimal       spread;

    private ITimeSeriesRepository timeSeriesRepository = null!;

    private List<Candle> toPersistCandles = null!;

    [TestInitialize]
    public void Setup()
    {
        recycler     = new Recycler();
        seedDateTime = new DateTime(2024, 6, 19);

        persistStartTime = seedDateTime;
        period           = FiveMinutes;

        mid1 = 0.5000m;
        mid2 = 1.0000m;

        spread        = 0.2000m;
        highLowSpread = 0.8000m;

        toPersistCandles = new List<Candle>
        {
            CreateCandle(period, persistStartTime, mid1, spread, highLowSpread)
          , CreateCandle(period, persistStartTime = period.PeriodEnd(persistStartTime), mid2, highLowSpread)
          , CreateCandle(period, persistStartTime = period.PeriodEnd(persistStartTime), mid1, spread, highLowSpread)
          , CreateCandle(period, persistStartTime = period.PeriodEnd(persistStartTime), mid2, spread, highLowSpread)
          , CreateCandle(period, persistStartTime = period.PeriodEnd(persistStartTime), mid1, spread, highLowSpread)
          , CreateCandle(period, persistStartTime = period.PeriodEnd(persistStartTime), mid2, spread, highLowSpread)
          , CreateCandle(period, period.PeriodEnd(persistStartTime), mid1, spread, highLowSpread)
        };

        repoRootDir = UniqueNewTestDirectory("PersisterRuleTests");
        var serviceConfig = IndicatorServicesConfigTests.UnitTestConfig(repoRootDir);

        timeSeriesRepository      = serviceConfig.TimeSeriesFileRepositoryConfig!.BuildRepository();
        tickerInfo.CoveringPeriod = new DiscreetTimePeriod(FiveMinutes);
        tickerInfo.InstrumentType = InstrumentType.Candle;
        persisterParams = new CandlePersisterParams
            (new TimeSeriesRepositoryParams(timeSeriesRepository), tickerInfo, InstrumentType.Candle
           , new DiscreetTimePeriod(period), TestEntriesToPersistAddress, TimeSpan.FromSeconds(5));
    }

    [TestCleanup]
    public async Task TearDown()
    {
        if (persisterRule != null) await CustomQueue1.StopRuleAsync(persisterRule, persisterRule);
        timeSeriesRepository.CloseAllFilesAndSessions();
        repoRootDir.RecursiveDelete();
    }


    [TestMethod]
    [Timeout(20_000)]
    public async Task NewRepository_SendEntriesToPersister_CanRetrieveEntriesFromRepository()
    {
        persisterRule = new CandleFilePersisterRule<Candle>(persisterParams);
        await using var childDeployment = await CustomQueue1.LaunchRuleAsync(persisterRule, persisterRule, CustomQueue1SelectionResult);

        foreach (var candle in toPersistCandles)
            await MessageBus.PublishAsync(persisterRule, TestEntriesToPersistAddress, candle);

        var fileInfo = timeSeriesRepository.GetInstrumentFileEntryInfo(tickerInfo);
        Assert.IsTrue(fileInfo.HasInstrument);

        persisterRule = null;
    }
}
