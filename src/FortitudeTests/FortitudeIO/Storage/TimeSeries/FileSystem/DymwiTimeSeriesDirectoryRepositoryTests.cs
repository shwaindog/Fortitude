﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Storage.TimeSeries.FileSystem.Config;
using FortitudeIO.Storage.TimeSeries.FileSystem;
using FortitudeIO.Storage.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.Storage.TimeSeries.FileSystem.Session;
using FortitudeIO.Storage.TimeSeries.FileSystem.Session.Retrieval;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.Generators.Quotes;
using FortitudeMarkets.Pricing.PQ.Generators.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Storage.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeTests.FortitudeCommon.Extensions;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Storage.TimeSeries.FileSystem.File;
using static FortitudeIO.Transports.Network.Config.CountryCityCodes;
using static FortitudeMarkets.Config.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;

#endregion

namespace FortitudeTests.FortitudeIO.Storage.TimeSeries.FileSystem;

[TestClass]
public class DymwiTimeSeriesDirectoryRepositoryTests
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(DymwiTimeSeriesDirectoryRepositoryTests));

    private readonly Func<IPublishableLevel3Quote> asPQLevel3QuoteFactory = () => new PQPublishableLevel3Quote();

    private readonly string expectedFileNameFormat = "Price_tick_Spot_DymwiTest_RepoTest_{0:yyyy-MM-dd}_1W_AUinMEL_Level3Quote.tsf";

    private readonly DateTime week1                = new(2024, 5, 24);
    private readonly string   week1ExpectedDirPath = "2020s/24/05-May/Week-3/FX_Global/RepoTest";
    private readonly DateTime week2                = new(2024, 5, 31);
    private readonly string   week2ExpectedDirPath = "2020s/24/05-May/Week-4/FX_Global/RepoTest";
    private readonly DateTime week3                = new(2024, 6, 6);
    private readonly string   week3ExpectedDirPath = "2020s/24/06-Jun/Week-1/FX_Global/RepoTest";
    private readonly DateTime week4                = new(2024, 6, 13);
    private readonly string   week4ExpectedDirPath = "2020s/24/06-Jun/Week-2/FX_Global/RepoTest";

    private SourceTickerInfo       level3SrcTkrInfo       = null!;
    private PQPublishableLevel3QuoteGenerator pqLevel3QuoteGenerator = null!;

    private IReaderSession<IPublishableLevel3Quote>? readerSession;

    private DymwiTimeSeriesDirectoryRepository repo = null!;

    private DirectoryInfo repoRootDir = null!;

    private SingleRepositoryBuilderConfig repositoryLocationConfig = null!;

    [TestInitialize]
    public void Setup()
    {
        level3SrcTkrInfo =
            new SourceTickerInfo
                (19, "DymwiTest", 79, "RepoTest", Level3Quote, FxMajor
               , AUinMEL, AUinMEL, AUinMEL
               , 5, layerFlags: LayerFlags.SourceQuoteReference, lastTradedFlags: LastTradedFlags.PaidOrGiven, roundingPrecision: 0.000001m,
                 minSubmitSize: 0.01m, incrementSize: 0.01m);

        var generateQuoteInfo = new GenerateQuoteInfo(level3SrcTkrInfo)
        {
            MidPriceGenerator =
            {
                StartTime = week1.Date, StartPrice = 1.332211m
            }
        };

        pqLevel3QuoteGenerator = new PQPublishableLevel3QuoteGenerator(new CurrentQuoteInstantValueGenerator(generateQuoteInfo));

        repoRootDir = GenerateUniqueDirectoryName();
        repositoryLocationConfig
            = new SingleRepositoryBuilderConfig(repoRootDir.FullName, RepositoryProximity.Local, typeof(PQRepoPathBuilder), RepositoryType.Dymwi, true
                                              , "DymwiTests");

        repo = (DymwiTimeSeriesDirectoryRepository)repositoryLocationConfig.BuildRepository();
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
        var toPersistAndCheck = TestWeeklyDataGeneratorFixture.GenerateRepeatableQuotes<IPublishableLevel3Quote, PQPublishableLevel3Quote>
            (1, 10, 1, DayOfWeek.Wednesday, pqLevel3QuoteGenerator, week1).ToList();
        toPersistAndCheck.AddRange
            (TestWeeklyDataGeneratorFixture.GenerateRepeatableQuotes<IPublishableLevel3Quote, PQPublishableLevel3Quote>
                (1, 10, 1, DayOfWeek.Thursday, pqLevel3QuoteGenerator, week3));

        var repoWriter = repo.GetWriterSession<IPublishableLevel3Quote>(level3SrcTkrInfo);

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
        readerSession = repo.GetReaderSession<IPublishableLevel3Quote>(level3SrcTkrInfo)!;
        var allEntriesReader = readerSession.AllChronologicalEntriesReader
            (new Recycler(), EntryResultSourcing.FromFactoryFuncUnlimited, ReaderOptions.ReadFastAsPossible, asPQLevel3QuoteFactory);
        var storedItems = allEntriesReader.ResultEnumerable.ToList();
        Assert.AreEqual(toPersistAndCheck.Count, allEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, allEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, storedItems.Count);
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
        var newReaderSession = repo.GetReaderSession<IPublishableLevel3Quote>(level3SrcTkrInfo)!;
        Assert.AreNotSame(readerSession, newReaderSession);
        var newEntriesReader = newReaderSession.AllChronologicalEntriesReader
            (new Recycler(), EntryResultSourcing.FromFactoryFuncUnlimited, ReaderOptions.ReadFastAsPossible, asPQLevel3QuoteFactory);
        newEntriesReader.ResultPublishFlags = ResultFlags.CopyToList;
        newEntriesReader.RunReader();
        var listResults = newEntriesReader.ResultList;
        Assert.AreEqual(toPersistAndCheck.Count, newEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, newEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, listResults.Count);
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
        newReaderSession.Close();
    }

    private void CompareExpectedToExtracted(List<IPublishableLevel3Quote> originalList, List<IPublishableLevel3Quote> toCompareList)
    {
        for (var i = 0; i < originalList.Count; i++)
        {
            var originalEntry = originalList[i];
            var compareEntry  = toCompareList[i];
            if (!originalEntry.AreEquivalent(compareEntry))
            {
                Logger.Warn("Entries at {0} differ test failed \ndiff {1}", i, originalEntry.DiffQuotes(compareEntry));
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

    private DirectoryInfo GenerateUniqueDirectoryName() => DirectoryInfoExtensionsTests.UniqueNewTestDirectory("DymwiRepoTest");
}
