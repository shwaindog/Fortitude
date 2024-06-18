// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Extensions;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.TimeSeries.FileSystem;
using FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.TimeSeries.FileSystem.Session;
using FortitudeIO.TimeSeries.FileSystem.Session.Retrieval;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Generators.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Generators.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeTests.FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;
using static FortitudeIO.TimeSeries.MarketClassificationExtensions;
using static FortitudeMarketsApi.Pricing.Quotes.QuoteLevel;

#endregion

namespace FortitudeTests.FortitudeIO.TimeSeries.FileSystem;

[TestClass]
public class DymwiTimeSeriesDirectoryRepositoryTests
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(DymwiTimeSeriesDirectoryRepositoryTests));

    private readonly Func<ILevel3Quote> asPQLevel3QuoteFactory = () => new PQLevel3Quote();

    private readonly DateTime week1                = new(2024, 5, 24);
    private readonly string   week1ExpectedDirPath = "2020s/24/05-May/Week-3/FXMajor_Global/RepoTest";
    private readonly DateTime week2                = new(2024, 5, 31);
    private readonly string   week2ExpectedDirPath = "2020s/24/05-May/Week-4/FXMajor_Global/RepoTest";
    private readonly DateTime week3                = new(2024, 6, 6);
    private readonly string   week3ExpectedDirPath = "2020s/24/06-Jun/Week-1/FXMajor_Global/RepoTest";
    private readonly DateTime week4                = new(2024, 6, 13);
    private readonly string   week4ExpectedDirPath = "2020s/24/06-Jun/Week-2/FXMajor_Global/RepoTest";

    private string expectedFileNameFormat = "Price_tick_Spot_DymwiTest_RepoTest_{0:yyyy-MM-dd}_1W_Level3.tsf";

    private SourceTickerQuoteInfo  level3SrcTkrQtInfo = null!;
    private int                    newTestCount;
    private PQLevel3QuoteGenerator pqLevel3QuoteGenerator = null!;

    private PQRepoPathBuilder pqRepoBuilder = null!;

    private IReaderSession<ILevel3Quote>? readerSession;

    private DymwiTimeSeriesDirectoryRepository repo = null!;

    private DirectoryInfo           repoRootDir = null!;
    private RepositoryStructureInfo repositoryStructureInfo;

    [TestInitialize]
    public void Setup()
    {
        level3SrcTkrQtInfo =
            new SourceTickerQuoteInfo
                (19, "DymwiTest", 79, "RepoTest", Level3, FxMajor
               , 5, layerFlags: LayerFlags.SourceQuoteReference, lastTradedFlags: LastTradedFlags.PaidOrGiven, roundingPrecision: 0.000001m);

        var generateQuoteInfo = new GenerateQuoteInfo(level3SrcTkrQtInfo);
        generateQuoteInfo.MidPriceGenerator!.StartTime  = week1.Date;
        generateQuoteInfo.MidPriceGenerator!.StartPrice = 1.332211m;

        pqLevel3QuoteGenerator = new PQLevel3QuoteGenerator(generateQuoteInfo);

        var testRepoName     = GenerateUniqueDirectoryName();
        var testRepoFullPath = Path.Combine(Environment.CurrentDirectory, testRepoName);

        repoRootDir = new DirectoryInfo(testRepoFullPath);
        repositoryStructureInfo
            = new RepositoryStructureInfo("DymwiTimeSeriesDirectoryRepositoryTests", repoRootDir, RepositoryProximity.Local, true);

        pqRepoBuilder = new PQRepoPathBuilder(repositoryStructureInfo);
        repo          = DymwiTimeSeriesDirectoryRepository.OpenRepository(pqRepoBuilder);
    }

    [TestCleanup]
    public void TearDown()
    {
        readerSession?.Close();
        repo.CloseAllFilesAndSessions();
        repoRootDir.RecursiveDelete();
        var currDirInfo = new DirectoryInfo(Environment.CurrentDirectory);
        foreach (var existingTestDirectory in currDirInfo.GetDirectories("DymwiRepoTest_*")) existingTestDirectory.RecursiveDelete();
    }

    [TestMethod]
    public void NewRepo_AppendTwoDifferentWeekFiles_WrittenDataIsInExpectedStructureAndCanBeRetrieved()
    {
        AssertFileNameDoesNotExistsFor(week1ExpectedDirPath, week1);
        AssertFileNameDoesNotExistsFor(week2ExpectedDirPath, week2);
        AssertFileNameDoesNotExistsFor(week3ExpectedDirPath, week3);
        AssertFileNameDoesNotExistsFor(week4ExpectedDirPath, week4);
        var toPersistAndCheck = TestWeeklyDataGeneratorFixture.GenerateRepeatableQuotes<ILevel3Quote, PQLevel3Quote>
            (1, 10, 1, DayOfWeek.Wednesday, pqLevel3QuoteGenerator, week1).ToList();
        toPersistAndCheck.AddRange
            (TestWeeklyDataGeneratorFixture.GenerateRepeatableQuotes<ILevel3Quote, PQLevel3Quote>
                (1, 10, 1, DayOfWeek.Thursday, pqLevel3QuoteGenerator, week3));

        var repoWriter = repo.GetWriterSession<ILevel3Quote>(level3SrcTkrQtInfo);

        Assert.IsNotNull(repoWriter);
        foreach (var firstPeriod in toPersistAndCheck)
        {
            var result = repoWriter.AppendEntry(firstPeriod);
            Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result.StorageAttemptResult);
        }
        repoWriter.Close();

        AssertFileNameExistsFor(week1ExpectedDirPath, week1);
        AssertFileNameDoesNotExistsFor(week2ExpectedDirPath, week2);
        AssertFileNameExistsFor(week3ExpectedDirPath, week3);
        AssertFileNameDoesNotExistsFor(week4ExpectedDirPath, week4);
        readerSession = repo.GetReaderSession<ILevel3Quote>(level3SrcTkrQtInfo)!;
        var allEntriesReader = readerSession.GetAllEntriesReader(EntryResultSourcing.FromFactoryFuncUnlimited, asPQLevel3QuoteFactory);
        var storedItems      = allEntriesReader.ResultEnumerable.ToList();
        Assert.AreEqual(toPersistAndCheck.Count, allEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, allEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, storedItems.Count);
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
        var newReaderSession = repo.GetReaderSession<ILevel3Quote>(level3SrcTkrQtInfo)!;
        Assert.AreNotSame(readerSession, newReaderSession);
        var newEntriesReader = newReaderSession.GetAllEntriesReader(EntryResultSourcing.FromFactoryFuncUnlimited, asPQLevel3QuoteFactory);
        newEntriesReader.ResultPublishFlags = ResultFlags.CopyToList;
        newEntriesReader.RunReader();
        var listResults = newEntriesReader.ResultList;
        Assert.AreEqual(toPersistAndCheck.Count, newEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, newEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, listResults.Count);
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
        newReaderSession.Close();
    }

    private void CompareExpectedToExtracted(List<ILevel3Quote> originalList, List<ILevel3Quote> toCompareList)
    {
        for (var i = 0; i < originalList.Count; i++)
        {
            var originalEntry = originalList[i];
            var compareEntry  = toCompareList[i];
            if (!originalEntry.AreEquivalent(compareEntry))
            {
                Logger.Warn("Entries at {0} differ test failed \ndiff {1}", i, originalEntry.DiffQuotes(compareEntry, false));
                FLoggerFactory.WaitUntilDrained();
                Assert.Fail($"Entries at {i} differ test failed \ndiff {originalEntry.DiffQuotes(compareEntry)}.");
            }
        }
    }

    private void AssertFileNameExistsFor(string repoPath, DateTime timeInWeek)
    {
        var expectedPath     = Path.Combine(repoRootDir.FullName, repoPath, GetExpectedFileName(timeInWeek));
        var expectedFileInfo = new FileInfo(expectedPath);
        Assert.IsTrue(expectedFileInfo.Exists, $"Did not find expected file {expectedFileInfo.FullName}");
    }

    private void AssertFileNameDoesNotExistsFor(string repoPath, DateTime timeInWeek)
    {
        var expectedPath     = Path.Combine(repoRootDir.FullName, repoPath, GetExpectedFileName(timeInWeek));
        var expectedFileInfo = new FileInfo(expectedPath);
        Assert.IsFalse(expectedFileInfo.Exists, $"Found unexpected file {expectedFileInfo.FullName} that shouldn't exist!");
    }

    private string GetExpectedFileName(DateTime timeInWeek)
    {
        var weekStart = timeInWeek.ToUniversalTime().TruncToWeekBoundary();
        return string.Format(expectedFileNameFormat, weekStart);
    }

    private string GenerateUniqueDirectoryName()
    {
        var now = DateTime.Now;
        Interlocked.Increment(ref newTestCount);
        var nowString = now.ToString("yyyy-MM-dd_HH-mm-ss_fff");
        return "DymwiRepoTest_" + nowString + "_" + newTestCount;
    }
}
