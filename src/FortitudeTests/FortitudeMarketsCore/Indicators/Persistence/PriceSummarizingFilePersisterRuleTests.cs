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
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Indicators.Persistence;
using FortitudeMarketsCore.Pricing.Summaries;
using FortitudeTests.FortitudeBusRules.BusMessaging;
using FortitudeTests.FortitudeMarketsCore.Indicators.Config;
using static FortitudeCommon.Chronometry.TimeBoundaryPeriod;
using static FortitudeMarketsApi.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeTests.FortitudeCommon.Extensions.DirectoryInfoExtensionsTests;
using static FortitudeTests.FortitudeMarketsCore.Pricing.Summaries.PricePeriodSummaryTests;
using static FortitudeMarketsApi.Pricing.Quotes.TickerDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Indicators.Persistence;

[TestClass]
public class PriceSummarizingFilePersisterRuleTests : OneOfEachMessageQueueTypeTestSetup
{
    private const string TestEntriesToPersistAddress = "TestEntriesToPersistAddress";

    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PriceSummarizingFilePersisterRuleTests));

    private readonly ISourceTickerInfo tickerInfo = new SourceTickerInfo
        (1, "SourceName", 1, "TickerName", Level1Quote, Unknown
       , 1, 0.001m, 0.1m, 10m, 100m, 10m);

    private decimal highLowSpread;
    private decimal mid1;
    private decimal mid2;

    private TimeBoundaryPeriod period;

    private SummarizingPricePersisterParams persisterParams;

    private PriceSummarizingFilePersisterRule<PricePeriodSummary>? persisterRule;

    private DateTime      persistStartTime;
    private IRecycler     recycler    = null!;
    private DirectoryInfo repoRootDir = null!;
    private DateTime      seedDateTime;
    private decimal       spread;

    private ITimeSeriesRepository timeSeriesRepository = null!;

    private List<PricePeriodSummary> toPersistPeriodSummaries = null!;

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

        toPersistPeriodSummaries = new List<PricePeriodSummary>
        {
            CreatePricePeriodSummary(period, persistStartTime, mid1, spread, highLowSpread)
          , CreatePricePeriodSummary(period, persistStartTime = period.PeriodEnd(persistStartTime), mid2, highLowSpread)
          , CreatePricePeriodSummary(period, persistStartTime = period.PeriodEnd(persistStartTime), mid1, spread, highLowSpread)
          , CreatePricePeriodSummary(period, persistStartTime = period.PeriodEnd(persistStartTime), mid2, spread, highLowSpread)
          , CreatePricePeriodSummary(period, persistStartTime = period.PeriodEnd(persistStartTime), mid1, spread, highLowSpread)
          , CreatePricePeriodSummary(period, persistStartTime = period.PeriodEnd(persistStartTime), mid2, spread, highLowSpread)
          , CreatePricePeriodSummary(period, period.PeriodEnd(persistStartTime), mid1, spread, highLowSpread)
        };

        repoRootDir = UniqueNewTestDirectory("PersisterRuleTests");
        var serviceConfig = IndicatorServicesConfigTests.UnitTestConfig(repoRootDir);

        timeSeriesRepository      = serviceConfig.TimeSeriesFileRepositoryConfig!.BuildRepository();
        tickerInfo.CoveringPeriod = new DiscreetTimePeriod(FiveMinutes);
        tickerInfo.InstrumentType = InstrumentType.PriceSummaryPeriod;
        persisterParams = new SummarizingPricePersisterParams
            (new TimeSeriesRepositoryParams(timeSeriesRepository), tickerInfo, InstrumentType.PriceSummaryPeriod
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
    public async Task NewRepository_SendEntriesToPersister_CanRetrieveEntriesFromRepository()
    {
        persisterRule = new PriceSummarizingFilePersisterRule<PricePeriodSummary>(persisterParams);
        await using var childDeployment = await CustomQueue1.LaunchRuleAsync(persisterRule, persisterRule, CustomQueue1SelectionResult);

        foreach (var periodSummary in toPersistPeriodSummaries)
            await MessageBus.PublishAsync(persisterRule, TestEntriesToPersistAddress, periodSummary);

        var fileInfo = timeSeriesRepository.GetInstrumentFileEntryInfo(tickerInfo);
        Assert.IsTrue(fileInfo.HasInstrument);

        persisterRule = null;
    }
}
