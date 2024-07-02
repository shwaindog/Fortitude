// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.Extensions;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.TimeSeries.FileSystem.Session;
using FortitudeIO.TimeSeries.FileSystem.Session.Retrieval;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.Generators.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Generators.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;
using FortitudeMarketsCore.Pricing.Quotes;
using static FortitudeMarketsApi.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarketsApi.Pricing.Quotes.QuoteLevel;
using static FortitudeTests.FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.TestWeeklyDataGeneratorFixture;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;

[TestClass]
public class WeeklyLevel2QuoteTimeSeriesFileTests
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(WeeklyLevel2QuoteTimeSeriesFileTests));

    private readonly Func<ILevel2Quote> asLevel2PriceQuoteFactory = () => new Level2PriceQuote();
    private readonly Func<ILevel2Quote> asPQLevel2QuoteFactory    = () => new PQLevel2Quote();

    private WeeklyLevel2QuoteTimeSeriesFile level2OneWeekFile = null!;

    private Level2QuoteGenerator level2QuoteGenerator = null!;

    private IReaderSession<ILevel2Quote>? level2SessionReader;
    private IWriterSession<ILevel2Quote>  level2SessionWriter = null!;

    private SourceTickerQuoteInfo  level2SrcTkrQtInfo     = null!;
    private PQLevel2QuoteGenerator pqLevel2QuoteGenerator = null!;
    private DateTime               startOfWeek;

    [TestInitialize]
    public void Setup()
    {
        PagedMemoryMappedFile.LogMappingMessages = true;

        var dateToGenerate = DateTime.UtcNow.Date.TruncToMonthBoundary().AddDays(15);
        ;
        var currentDayOfWeek = dateToGenerate.DayOfWeek;
        var dayDiff          = DayOfWeek.Sunday - currentDayOfWeek;
        startOfWeek = dateToGenerate.AddDays(dayDiff);
    }

    private void CreateLevel2File
    (FileFlags fileFlags = FileFlags.WriterOpened | FileFlags.HasInternalIndexInHeader,
        LayerType layerType = LayerType.PriceVolume, byte numberOfLayers = 20)
    {
        level2SrcTkrQtInfo =
            new SourceTickerQuoteInfo
                (19, "WeeklyLevel2QuoteTimeSeriesFileTests", 79, "PersistTest", Level2, Unknown
               , numberOfLayers, layerFlags: layerType.SupportedLayerFlags(), lastTradedFlags: LastTradedFlags.None, roundingPrecision: 0.000001m);

        var generateQuoteInfo = new GenerateQuoteInfo(level2SrcTkrQtInfo);
        generateQuoteInfo.MidPriceGenerator!.StartTime  = startOfWeek;
        generateQuoteInfo.MidPriceGenerator!.StartPrice = 1.332211m;

        level2QuoteGenerator   = new Level2QuoteGenerator(generateQuoteInfo);
        pqLevel2QuoteGenerator = new PQLevel2QuoteGenerator(generateQuoteInfo);

        fileFlags |= FileFlags.WriterOpened | FileFlags.HasInternalIndexInHeader;

        var testTimeSeriesFilePath = Path.Combine(Environment.CurrentDirectory, GenerateUniqueFileNameOffDateTime());
        var timeSeriesFile         = new FileInfo(testTimeSeriesFilePath);
        if (timeSeriesFile.Exists) timeSeriesFile.Delete();
        var instrumentFields = new Dictionary<string, string>
        {
            { nameof(RepositoryPathName.SourceName), "TestSourceName" }, { nameof(RepositoryPathName.MarketType), "Unknown" }
          , { nameof(RepositoryPathName.MarketProductType), "Unknown" }, { nameof(RepositoryPathName.MarketRegion), "Unknown" }
        };
        var optionalInstrumentFields = new Dictionary<string, string>
        {
            { nameof(RepositoryPathName.Category), "TestInstrumentCategory" }
        };
        var createTestCreateFileParameters =
            new TimeSeriesFileParameters
                (timeSeriesFile
               , new Instrument("TestInstrumentName", InstrumentType.Price, TimeSeriesPeriod.Tick, instrumentFields, optionalInstrumentFields),
                 TimeSeriesPeriod.OneWeek, DateTime.UtcNow.Date, 7, fileFlags, 6);
        var createPriceQuoteFile = new PriceTimeSeriesFileParameters(level2SrcTkrQtInfo, createTestCreateFileParameters);
        level2OneWeekFile   = new WeeklyLevel2QuoteTimeSeriesFile(createPriceQuoteFile);
        level2SessionWriter = level2OneWeekFile.GetWriterSession()!;
    }

    [TestCleanup]
    public void TearDown()
    {
        try
        {
            level2SessionReader?.Close();
            level2SessionWriter.Close();
            level2OneWeekFile.Close();
        }
        catch (Exception ex)
        {
            Console.Out.WriteLine("Could not close all sessions. Got {0}", ex);
        }
        var dirInfo = new DirectoryInfo(Environment.CurrentDirectory);
        DeleteTestFiles(dirInfo);
    }

    [TestMethod]
    public void CreateNewPriceVolumeQuote_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        CreateLevel2File(layerType: LayerType.PriceVolume);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned(level2QuoteGenerator, asLevel2PriceQuoteFactory);
    }

    [TestMethod]
    public void CreateNewPQPriceVolumeQuote_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        CreateLevel2File(layerType: LayerType.PriceVolume);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned(pqLevel2QuoteGenerator, asPQLevel2QuoteFactory);
    }

    [TestMethod]
    public void CreateNewPriceVolumeQuote_TwoSmallCompressedPeriods_OriginalValuesAreReturned()
    {
        CreateLevel2File(FileFlags.WriteDataCompressed, LayerType.PriceVolume);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned(level2QuoteGenerator, asLevel2PriceQuoteFactory);
    }

    [TestMethod]
    public void CreateNewPQPriceVolumeQuote_TwoSmallCompressedPeriods_OriginalValuesAreReturned()
    {
        CreateLevel2File(FileFlags.WriteDataCompressed, LayerType.PriceVolume);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned(pqLevel2QuoteGenerator, asPQLevel2QuoteFactory);
    }

    [TestMethod]
    public void CreateNewSourcePriceVolumeQuote_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        CreateLevel2File(layerType: LayerType.SourcePriceVolume);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned(level2QuoteGenerator, asLevel2PriceQuoteFactory);
    }

    [TestMethod]
    public void CreateNewPQSourcePriceVolumeQuote_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        CreateLevel2File(layerType: LayerType.SourcePriceVolume);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned(pqLevel2QuoteGenerator, asPQLevel2QuoteFactory);
    }

    [TestMethod]
    public void CreateNewSourcePriceVolumeQuote_TwoSmallCompressedPeriods_OriginalValuesAreReturned()
    {
        CreateLevel2File(FileFlags.WriteDataCompressed, LayerType.SourcePriceVolume);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned(level2QuoteGenerator, asLevel2PriceQuoteFactory);
    }

    [TestMethod]
    public void CreateNewPQSourcePriceVolumeQuote_TwoSmallCompressedPeriods_OriginalValuesAreReturned()
    {
        CreateLevel2File(FileFlags.WriteDataCompressed, LayerType.SourcePriceVolume);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned(pqLevel2QuoteGenerator, asPQLevel2QuoteFactory);
    }

    [TestMethod]
    public void CreateNewSourceQuoteRefPriceVolumeQuote_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        CreateLevel2File(layerType: LayerType.SourceQuoteRefPriceVolume);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned(level2QuoteGenerator, asLevel2PriceQuoteFactory);
    }

    [TestMethod]
    public void CreateNewPQSourceQuoteRefPriceVolumeQuote_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        CreateLevel2File(layerType: LayerType.SourceQuoteRefPriceVolume);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned(pqLevel2QuoteGenerator, asPQLevel2QuoteFactory);
    }

    [TestMethod]
    public void CreateNewSourceQuoteRefPriceVolumeQuote_TwoSmallCompressedPeriods_OriginalValuesAreReturned()
    {
        CreateLevel2File(FileFlags.WriteDataCompressed, LayerType.SourceQuoteRefPriceVolume);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned(level2QuoteGenerator, asLevel2PriceQuoteFactory);
    }

    [TestMethod]
    public void CreateNewPQSourceQuoteRefPriceVolumeQuote_TwoSmallCompressedPeriods_OriginalValuesAreReturned()
    {
        CreateLevel2File(FileFlags.WriteDataCompressed, LayerType.SourceQuoteRefPriceVolume);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned(pqLevel2QuoteGenerator, asPQLevel2QuoteFactory);
    }

    [TestMethod]
    public void CreateNewTraderPriceVolumeQuote_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        CreateLevel2File(layerType: LayerType.TraderPriceVolume);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned(level2QuoteGenerator, asLevel2PriceQuoteFactory);
    }

    [TestMethod]
    public void CreateNewPQTraderPriceVolumeQuote_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        CreateLevel2File(layerType: LayerType.TraderPriceVolume);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned(pqLevel2QuoteGenerator, asPQLevel2QuoteFactory);
    }

    [TestMethod]
    public void CreateNewTraderPriceVolumeQuote_TwoSmallCompressedPeriods_OriginalValuesAreReturned()
    {
        CreateLevel2File(FileFlags.WriteDataCompressed, LayerType.TraderPriceVolume);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned(level2QuoteGenerator, asLevel2PriceQuoteFactory);
    }

    [TestMethod]
    public void CreateNewPQTraderPriceVolumeQuote_TwoSmallCompressedPeriods_OriginalValuesAreReturned()
    {
        CreateLevel2File(FileFlags.WriteDataCompressed, LayerType.TraderPriceVolume);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned(pqLevel2QuoteGenerator, asPQLevel2QuoteFactory);
    }

    [TestMethod]
    public void CreateNewValueDatePriceVolumeQuote_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        CreateLevel2File(layerType: LayerType.ValueDatePriceVolume);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned(level2QuoteGenerator, asLevel2PriceQuoteFactory);
    }

    [TestMethod]
    public void CreateNewPQValueDatePriceVolumeQuote_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        CreateLevel2File(layerType: LayerType.ValueDatePriceVolume);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned(pqLevel2QuoteGenerator, asPQLevel2QuoteFactory);
    }

    [TestMethod]
    public void CreateNewValueDatePriceVolumeQuote_TwoSmallCompressedPeriods_OriginalValuesAreReturned()
    {
        CreateLevel2File(FileFlags.WriteDataCompressed, LayerType.ValueDatePriceVolume);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned(level2QuoteGenerator, asLevel2PriceQuoteFactory);
    }

    [TestMethod]
    public void CreateNewPQValueDatePriceVolumeQuote_TwoSmallCompressedPeriods_OriginalValuesAreReturned()
    {
        CreateLevel2File(FileFlags.WriteDataCompressed, LayerType.ValueDatePriceVolume);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned(pqLevel2QuoteGenerator, asPQLevel2QuoteFactory);
    }

    [TestMethod]
    public void CreateSourceQuoteRefTraderValueDatePriceVolumeQuote_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        CreateLevel2File(layerType: LayerType.SourceQuoteRefTraderValueDatePriceVolume);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned(level2QuoteGenerator, asLevel2PriceQuoteFactory);
    }

    [TestMethod]
    public void CreatePQSourceQuoteRefTraderValueDatePriceVolumeQuote_TwoSmallPeriods_OriginalValuesAreReturned()
    {
        CreateLevel2File(layerType: LayerType.SourceQuoteRefTraderValueDatePriceVolume);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned(pqLevel2QuoteGenerator, asPQLevel2QuoteFactory);
    }

    [TestMethod]
    public void CreateSourceQuoteRefTraderValueDatePriceVolumeQuote_TwoSmallCompressedPeriods_OriginalValuesAreReturned()
    {
        CreateLevel2File(FileFlags.WriteDataCompressed, LayerType.SourceQuoteRefTraderValueDatePriceVolume);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned(level2QuoteGenerator, asLevel2PriceQuoteFactory);
    }

    [TestMethod]
    public void CreatePQSourceQuoteRefTraderValueDatePriceVolumeQuote_TwoSmallCompressedPeriods_OriginalValuesAreReturned()
    {
        CreateLevel2File(FileFlags.WriteDataCompressed, LayerType.SourceQuoteRefTraderValueDatePriceVolume);
        CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned(pqLevel2QuoteGenerator, asPQLevel2QuoteFactory);
    }

    public void CreateNewTyped_TwoSmallPeriods_OriginalValuesAreReturned<TEntry>
        (IQuoteGenerator<TEntry> quoteGenerator, Func<ILevel2Quote> retrievalFactory)
        where TEntry : class, IMutableLevel2Quote, ILevel2Quote
    {
        var toPersistAndCheck
            = GenerateRepeatableQuotes<ILevel2Quote, TEntry>
                (1, 10, 1, DayOfWeek.Wednesday, quoteGenerator).ToList();
        toPersistAndCheck.AddRange
            (GenerateRepeatableQuotes<ILevel2Quote, TEntry>
                (1, 10, 1, DayOfWeek.Thursday, quoteGenerator));

        foreach (var firstPeriod in toPersistAndCheck)
        {
            var result = level2SessionWriter.AppendEntry(firstPeriod);
            Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result.StorageAttemptResult);
        }
        level2OneWeekFile.AutoCloseOnZeroSessions = false;
        level2SessionWriter.Close();

        Assert.AreEqual((uint)toPersistAndCheck.Count, level2OneWeekFile.Header.TotalEntries);
        level2SessionReader = level2OneWeekFile.GetReaderSession();
        var allEntriesReader = level2SessionReader.GetAllEntriesReader(EntryResultSourcing.FromFactoryFuncUnlimited, retrievalFactory);
        var storedItems      = allEntriesReader.ResultEnumerable.ToList();
        Assert.AreEqual(toPersistAndCheck.Count, allEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, allEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, storedItems.Count);
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
    }

    //[TestMethod]
    public void LongRunningCreateNewPriceQuote_TwoLargeCompressedPeriods_OriginalValuesAreReturned()
    {
        CreateLevel2File(FileFlags.WriteDataCompressed);
        CreateNewTyped_TwoLargeCompressedPeriods_OriginalValuesAreReturned(level2QuoteGenerator, asLevel2PriceQuoteFactory);
    }

    //[TestMethod]
    public void LongRunningCreateNewPQPriceQuote_TwoLargeCompressedPeriods_OriginalValuesAreReturned()
    {
        CreateLevel2File(FileFlags.WriteDataCompressed);
        CreateNewTyped_TwoLargeCompressedPeriods_OriginalValuesAreReturned(pqLevel2QuoteGenerator, asPQLevel2QuoteFactory);
    }

    public void CreateNewTyped_TwoLargeCompressedPeriods_OriginalValuesAreReturned<TEntry>
        (IQuoteGenerator<TEntry> quoteGenerator, Func<ILevel2Quote> retrievalFactory)
        where TEntry : class, IMutableLevel2Quote, ILevel2Quote
    {
        var toPersistAndCheck
            = GenerateRepeatableQuotes<ILevel2Quote, TEntry>
                (1, 8000, 1, DayOfWeek.Wednesday, quoteGenerator).ToList();
        toPersistAndCheck.AddRange
            (GenerateRepeatableQuotes<ILevel2Quote, TEntry>
                (1, 8000, 1, DayOfWeek.Thursday, quoteGenerator));

        foreach (var firstPeriod in toPersistAndCheck)
        {
            var result = level2SessionWriter.AppendEntry(firstPeriod);
            Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result.StorageAttemptResult);
        }
        level2OneWeekFile.AutoCloseOnZeroSessions = false;
        level2SessionWriter.Close();

        Assert.AreEqual((uint)toPersistAndCheck.Count, level2OneWeekFile.Header.TotalEntries);
        level2SessionReader = level2OneWeekFile.GetReaderSession();
        var allEntriesReader = level2SessionReader.GetAllEntriesReader(EntryResultSourcing.FromFactoryFuncUnlimited, retrievalFactory);
        var storedItems      = allEntriesReader.ResultEnumerable.ToList();
        Assert.AreEqual(toPersistAndCheck.Count, allEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, allEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, storedItems.Count);
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
    }

    //[TestMethod]
    public void LongRunningCreateNewPriceQuoteCompressedDataFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned()
    {
        CreateLevel2File(FileFlags.WriteDataCompressed);
        NewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned(level2QuoteGenerator, asLevel2PriceQuoteFactory);
    }

    [TestMethod]
    public void CreateNewPriceQuoteFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned()
    {
        CreateLevel2File(layerType: LayerType.SourceQuoteRefTraderValueDatePriceVolume, numberOfLayers: 12);
        NewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned(level2QuoteGenerator, asLevel2PriceQuoteFactory);
    }

    //[TestMethod]
    public void LongRunningCreateNewPQQuoteCompressedDataFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned()
    {
        CreateLevel2File(FileFlags.WriteDataCompressed);
        NewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned(pqLevel2QuoteGenerator, asPQLevel2QuoteFactory);
    }

    [TestMethod]
    public void CreateNewPQQuoteFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned()
    {
        CreateLevel2File(layerType: LayerType.SourceQuoteRefPriceVolume, numberOfLayers: 10);
        NewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned(pqLevel2QuoteGenerator, asPQLevel2QuoteFactory);
    }

    public void NewFile_SavesEntriesCloseAndReopen_OriginalValuesAreReturned<TEntry>
        (IQuoteGenerator<TEntry> quoteGenerator, Func<ILevel2Quote> retrievalFactory)
        where TEntry : class, IMutableLevel2Quote, ILevel2Quote
    {
        var toPersistAndCheck =
            GenerateQuotesForEachDayAndHourOfCurrentWeek<ILevel2Quote, TEntry>
                (0, 10, quoteGenerator).ToList();

        foreach (var level2QuoteStruct in toPersistAndCheck)
        {
            var result = level2SessionWriter.AppendEntry(level2QuoteStruct);
            Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result.StorageAttemptResult);
        }
        level2OneWeekFile.AutoCloseOnZeroSessions = false;
        level2SessionWriter.Close();
        Assert.AreEqual((uint)toPersistAndCheck.Count, level2OneWeekFile.Header.TotalEntries);

        level2SessionReader = level2OneWeekFile.GetReaderSession();
        var allEntriesReader = level2SessionReader.GetAllEntriesReader(EntryResultSourcing.FromFactoryFuncUnlimited, retrievalFactory);
        var storedItems      = allEntriesReader.ResultEnumerable.ToList();
        Assert.AreEqual(toPersistAndCheck.Count, allEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, allEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, storedItems.Count);
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
        var newReaderSession = level2OneWeekFile.GetReaderSession();
        Assert.AreNotSame(level2SessionReader, newReaderSession);
        var newEntriesReader = level2SessionReader.GetAllEntriesReader(EntryResultSourcing.FromFactoryFuncUnlimited, retrievalFactory);
        newEntriesReader.ResultPublishFlags = ResultFlags.CopyToList;
        newEntriesReader.RunReader();
        var listResults = newEntriesReader.ResultList;
        Assert.AreEqual(toPersistAndCheck.Count, newEntriesReader.CountMatch);
        Assert.AreEqual(allEntriesReader.CountMatch, newEntriesReader.CountProcessed);
        Assert.AreEqual(toPersistAndCheck.Count, listResults.Count);
        CompareExpectedToExtracted(toPersistAndCheck, storedItems);
        newReaderSession.Close();
    }

    private void CompareExpectedToExtracted(List<ILevel2Quote> originalList, List<ILevel2Quote> toCompareList)
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

    [TestMethod]
    public void CreateNewFile_SetLargeStringsWhichAreTruncated_CloseReopenSafelyReturnsTruncatedStrings()
    {
        CreateLevel2File();
        var largeString = 999.CharIndexPosListedSizeString();
        var truncated   = largeString.Substring(0, 254); // byte.MaxValue -1 (largest storable value)  (-1 null terminator)
        var header      = level2OneWeekFile.Header;
        header.AnnotationFileRelativePath = largeString;
        Assert.AreEqual(truncated, header.AnnotationFileRelativePath);
        header.ExternalIndexFileRelativePath = largeString;
        Assert.AreEqual(truncated, header.ExternalIndexFileRelativePath);
        header.OriginSourceText = largeString;
        Assert.AreEqual(truncated, header.OriginSourceText);
        header.SourceName = largeString;
        Assert.AreEqual(truncated, header.SourceName);
        header.Category = largeString;
        Assert.AreEqual(truncated, header.Category);
        header.InstrumentName = largeString;
        Assert.AreEqual(truncated, header.InstrumentName);
        Assert.AreEqual(TimeSeriesPeriod.OneWeek, header.FilePeriod);
        Assert.AreEqual(TimeSeriesPeriod.OneWeek.ContainingPeriodBoundaryStart(DateTime.UtcNow.Date), header.FileStartPeriod);
        Assert.AreEqual(InstrumentType.Price, header.InstrumentType);
        Assert.AreEqual(typeof(DailyToHourlyLevel2QuoteSubBuckets<ILevel2Quote>), header.BucketType);
        Assert.AreEqual(typeof(ILevel2Quote), header.EntryType);
        Assert.AreEqual(typeof(WeeklyLevel2QuoteTimeSeriesFile), header.TimeSeriesFileType);
        level2OneWeekFile.Close();
        level2OneWeekFile = WeeklyLevel2QuoteTimeSeriesFile
            .OpenExistingTimeSeriesFile(level2OneWeekFile.FileName);
        header = level2OneWeekFile.Header;
        Assert.AreEqual(truncated, header.AnnotationFileRelativePath);
        Assert.AreEqual(truncated, header.ExternalIndexFileRelativePath);
        Assert.AreEqual(truncated, header.OriginSourceText);
        Assert.AreEqual(truncated, header.SourceName);
        Assert.AreEqual(truncated, header.Category);
        Assert.AreEqual(truncated, header.InstrumentName);
        Assert.AreEqual(TimeSeriesPeriod.OneWeek, header.FilePeriod);
        Assert.AreEqual(TimeSeriesPeriod.OneWeek.ContainingPeriodBoundaryStart(DateTime.UtcNow.Date), header.FileStartPeriod);
        Assert.AreEqual(InstrumentType.Price, header.InstrumentType);
        Assert.AreEqual(typeof(DailyToHourlyLevel2QuoteSubBuckets<ILevel2Quote>), header.BucketType);
        Assert.AreEqual(typeof(ILevel2Quote), header.EntryType);
        Assert.AreEqual(typeof(WeeklyLevel2QuoteTimeSeriesFile), header.TimeSeriesFileType);
    }

    [TestMethod]
    public void CreateNewFile_BeyondFileTime_ReturnsFileRangeNotSupported()
    {
        CreateLevel2File();
        Assert.AreEqual(InstrumentType.Price, level2OneWeekFile.InstrumentType);
        var singleQuoteMiddleOfWeek
            = GenerateRepeatableQuotes<ILevel2Quote, Level2PriceQuote>
                (1, 1, 12, DayOfWeek.Wednesday, level2QuoteGenerator);
        var nextWeekQuote = (IMutableLevel2Quote)singleQuoteMiddleOfWeek.First();
        nextWeekQuote.SourceTime = nextWeekQuote.SourceTime.AddDays(7);
        var result = level2SessionWriter.AppendEntry(nextWeekQuote);
        Assert.AreEqual(StorageAttemptResult.NextFilePeriod, result.StorageAttemptResult);
    }

    [TestMethod]
    public void CreateNewFile_AfterBucketClosesOnNextEntry_TryingToAddExistingEntryReturnsBucketClosedForAppend()
    {
        CreateLevel2File();
        var wednesdayQuotes =
            GenerateRepeatableQuotes<ILevel2Quote, Level2PriceQuote>
                (1, 1, 12, DayOfWeek.Wednesday, level2QuoteGenerator);
        var thursdayQuotes =
            GenerateRepeatableQuotes<ILevel2Quote, Level2PriceQuote>
                (1, 1, 12, DayOfWeek.Thursday, level2QuoteGenerator);
        var wednesdayQuote = wednesdayQuotes.First();
        var thursdayQuote  = thursdayQuotes.First();
        var result         = level2SessionWriter.AppendEntry(wednesdayQuote);
        Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result.StorageAttemptResult);
        result = level2SessionWriter.AppendEntry(thursdayQuote);
        Assert.AreEqual(StorageAttemptResult.PeriodRangeMatched, result.StorageAttemptResult);
        result = level2SessionWriter.AppendEntry(wednesdayQuote);
        Assert.AreEqual(StorageAttemptResult.BucketClosedForAppend, result.StorageAttemptResult);
    }
}
