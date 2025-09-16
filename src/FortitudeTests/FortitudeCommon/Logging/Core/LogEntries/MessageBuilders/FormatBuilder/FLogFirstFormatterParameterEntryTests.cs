// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.ExampleConfig;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains.PipelineSpies;
using FortitudeCommon.Logging.Core.LoggerViews;
using static FortitudeTests.FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.SingleMessageBuildingValues;

// ReSharper disable FormatStringProblem

namespace FortitudeTests.FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

[TestClass]
public class FLogFirstFormatterParameterEntryTests
{
    private static IVersatileFLogger logger = null!;

    private static LogEntrySnatchSpyRingInvestigation spyRing = null!;

    [ClassInitialize]
    public static void AllTestsInClassStaticSetup(TestContext testContext)
    {
        FLogConfigExamples.SyncColoredConsoleExample.LoadExampleAsCurrentContext();

        logger  = FLog.FLoggerForType.As<IVersatileFLogger>();
        spyRing = new LogEntrySnatchSpyRingInvestigation();
        var loggerSpy = spyRing.TrainNewSpy($"TestLoggerSpyFor{logger.Logger.FullName}");
        logger.Logger.PublishEndpoint.Insert(loggerSpy);
    }

    [TestMethod]
    public void WithOnlyDateTimeParamIsFormattedAsExpected()
    {
        logger.InfoFormat("Formatting DateTimeTicks with yyyy-MM-dd HH:mm:ss.ffffff : {0:yyyy-MM-dd HH:mm:ss.ffffff}")?
            .WithOnlyParam(ToFormatDateTimeTicks);
        var logEntries      = spyRing.DeadDropPurgeLatestIntelToHq();
        var writtenLogEntry = logEntries.First();
        Assert.AreEqual(1, writtenLogEntry.RefCount); // 
        Assert.AreEqual($"Formatting DateTimeTicks with yyyy-MM-dd HH:mm:ss.ffffff : {ToFormatDateTimeTicksStandardAsString}",
                        writtenLogEntry.Message.ToString());

        logger.InfoFormat("Formatting DateTime with yyyy-MM-dd : {0:yyyy-MM-dd}")?.WithOnlyParam(ToFormatDateTime);
        logEntries      = spyRing.DeadDropPurgeLatestIntelToHq();
        writtenLogEntry = logEntries.First();
        Assert.AreEqual(1, writtenLogEntry.RefCount); // 
        Assert.AreEqual($"Formatting DateTime with yyyy-MM-dd : {ToFormatDateStandardAsString}", writtenLogEntry.Message.ToString());

        logger.InfoFormat("Formatting Time with -14:HH:mm:ss : {0,-14:HH:mm:ss}")?.WithOnlyParam(ToFormatDateTime);
        logEntries      = spyRing.DeadDropPurgeLatestIntelToHq();
        writtenLogEntry = logEntries.First();
        Assert.AreEqual(1, writtenLogEntry.RefCount);
        Assert.AreEqual($"Formatting Time with -14:HH:mm:ss : {ToFormatTimeLeftAlignAsString}", writtenLogEntry.Message.ToString());

        logger.InfoFormat("Formatting Time with 14:HH:mm:ss : {0,14:HH:mm:ss}")?.WithOnlyParam(ToFormatDateTime);
        logEntries      = spyRing.DeadDropPurgeLatestIntelToHq();
        writtenLogEntry = logEntries.First();
        Assert.AreEqual(1, writtenLogEntry.RefCount);
        Assert.AreEqual($"Formatting Time with 14:HH:mm:ss : {ToFormatTimeRightAlignAsString}", writtenLogEntry.Message.ToString());
    }

    [TestMethod]
    public void WithOnlyBoolParamIsFormattedAsExpected()
    {
        logger.InfoFormat("bool is true : {0}")?.WithOnlyParam(true);
        Assert.AreEqual("bool is true : true", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("bool is false : {0}")?.WithOnlyParam(false);
        Assert.AreEqual("bool is false : false", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("bool is null : {0}")?.WithOnlyParam((bool?)null);
        Assert.AreEqual("bool is null : null", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
    }

    [TestMethod]
    public void WithOnlyNumberParamIsFormattedAsExpected()
    {
        logger.InfoFormat("Formatting Number 2dp : {0:F2}")?.WithOnlyParam(ToFormatNumber);
        Assert.AreEqual($"Formatting Number 2dp : {Number2DecimalPlaceString}", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.InfoFormat("Formatting Number 3dp : {0:F3}")?.WithOnlyParam(ToFormatNumber);
        Assert.AreEqual($"Formatting Number 3dp : {Number3DecimalPlaceString}", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.InfoFormat("Formatting Number 4dp : {0:F4}")?.WithOnlyParam(ToFormatNumber);
        Assert.AreEqual($"Formatting Number 4dp : {Number4DecimalPlaceString}", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.InfoFormat("Formatting Number 5dp : {0:F5}")?.WithOnlyParam(ToFormatNumber);
        Assert.AreEqual($"Formatting Number 5dp : {Number5DecimalPlaceString}", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.InfoFormat("Formatting Number 6dp : {0:F6}")?.WithOnlyParam(ToFormatNumber);
        Assert.AreEqual($"Formatting Number 6dp : {Number6DecimalPlaceString}", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Formatting Number 2dp -14 : {0,-14:F2}")?.WithOnlyParam(ToFormatNumber);
        Assert.AreEqual($"Formatting Number 2dp -14 : {Number2DecimalPlacesLeftAlignString}", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.InfoFormat("Formatting Number 3dp -14 : {0,-14:F3}")?.WithOnlyParam(ToFormatNumber);
        Assert.AreEqual($"Formatting Number 3dp -14 : {Number3DecimalPlacesLeftAlignString}", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.InfoFormat("Formatting Number 4dp -14 : {0,-14:F4}")?.WithOnlyParam(ToFormatNumber);
        Assert.AreEqual($"Formatting Number 4dp -14 : {Number4DecimalPlacesLeftAlignString}", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.InfoFormat("Formatting Number 5dp -14 : {0,-14:F5}")?.WithOnlyParam(ToFormatNumber);
        Assert.AreEqual($"Formatting Number 5dp -14 : {Number5DecimalPlacesLeftAlignString}", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.InfoFormat("Formatting Number 6dp -14 : {0,-14:F6}")?.WithOnlyParam(ToFormatNumber);
        Assert.AreEqual($"Formatting Number 6dp -14 : {Number6DecimalPlacesLeftAlignString}", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Formatting Number 2dp 14 : {0,14:F2}")?.WithOnlyParam(ToFormatNumber);
        Assert.AreEqual($"Formatting Number 2dp 14 : {Number2DecimalPlacesRightAlignString}", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.InfoFormat("Formatting Number 3dp 14 : {0,14:F3}")?.WithOnlyParam(ToFormatNumber);
        Assert.AreEqual($"Formatting Number 3dp 14 : {Number3DecimalPlacesRightAlignString}", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.InfoFormat("Formatting Number 4dp 14 : {0,14:F4}")?.WithOnlyParam(ToFormatNumber);
        Assert.AreEqual($"Formatting Number 4dp 14 : {Number4DecimalPlacesRightAlignString}", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.InfoFormat("Formatting Number 5dp 14 : {0,14:F5}")?.WithOnlyParam(ToFormatNumber);
        Assert.AreEqual($"Formatting Number 5dp 14 : {Number5DecimalPlacesRightAlignString}", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.InfoFormat("Formatting Number 6dp 14 : {0,14:F6}")?.WithOnlyParam(ToFormatNumber);
        Assert.AreEqual($"Formatting Number 6dp 14 : {Number6DecimalPlacesRightAlignString}", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
    }

    [TestMethod]
    public void WithOnlyEnumParamIsFormattedAsExpected()
    {
        logger.InfoFormat("Formatting FLogLevel.Debug : {0}")?.WithOnlyParam(FLogLevel.Debug);
        Assert.AreEqual("Formatting FLogLevel.Debug : Debug", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Formatting FLogLevel.Error -14 : {0,-14}")?.WithOnlyParam(FLogLevel.Error);
        Assert.AreEqual("Formatting FLogLevel.Error -14 : Error         ", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Formatting FLogLevel.Info 14 : {0,14}")?.WithOnlyParam(FLogLevel.Info);
        Assert.AreEqual("Formatting FLogLevel.Info 14 :           Info", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
    }

    [TestMethod]
    public void WithOnlyCustomStylerParamIsFormattedAsExpected()
    {
        logger.InfoFormat("Formatting TimeSpan as ComplexType : {0}")?.WithOnlyParam(ToFormatTimeSpan, ToFormatTimeSpan.StylerComplexType());
        Assert.AreEqual($"Formatting TimeSpan as ComplexType : {TimeSpanStylerComplexAsString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Formatting TimeSpan as StringFormat : {0,-14}")
              ?.WithOnlyParam(ToFormatTimeSpan);
        Assert.AreEqual($"Formatting TimeSpan as StringFormat : {TimeSpanSpanUnformattedAsString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
    }

    [TestMethod]
    public void WithOnlyCharArrayParamIsExpectedResult()
    {
        logger.InfoFormat("Adding Small Char Array : {0}")?.WithOnlyParam(SmallCharsArray);
        Assert.AreEqual($"Adding Small Char Array : {SmallCharArrayAsString}", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Adding Small Char Array Range : {0}")?
            .WithOnlyParam(SmallCharsArray, SmallCharArraySubRange.Start.Value, SmallCharArraySubRange.Length());
        Assert.AreEqual($"Adding Small Char Array Range : {SmallCharArraySubRangeAsString}", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Adding Large Char Array : {0}")?.WithOnlyParam(LargeCharsArray);
        Assert.AreEqual($"Adding Large Char Array : {LargeCharsArrayAsString}", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.DebugFormat("Adding Large Char Array Range : {0}")?
            .WithOnlyParam(LargeCharsArray, LargeCharArraySubRange.Start.Value, LargeCharArraySubRange.Length());
        Assert.AreEqual($"Adding Large Char Array Range : {LargeCharArraySubRangeAsString}", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.TraceFormat("Adding Large Char Array : {0}")?.WithOnlyParam(LargeCharsArray);
        Assert.AreEqual($"Adding Large Char Array : {LargeCharsArrayAsString}", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
    }

    [TestMethod]
    public void WithOnlyStringParamIsExpectedResult()
    {
        logger.InfoFormat("Adding Full ShortString : \"{0}\"")?.WithOnlyParam(ShortString);
        Assert.AreEqual($"Adding Full ShortString : \"{ShortString}\"", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Adding ShortString Range : \"{0}\"")
              ?.WithOnlyParam(ShortString, ShortStringSubRange.Start.Value, ShortStringSubRange.Length());
        Assert.AreEqual($"Adding ShortString Range : \"{ShortStringSubRangeAsString}\"", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        var shortStringSpan = ShortString.AsSpan();
        logger.InfoFormat("Adding Full ShortSpan : \"{0}\"")?.WithOnlyParam(shortStringSpan);
        Assert.AreEqual($"Adding Full ShortSpan : \"{ShortString}\"", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Adding ShortSpan Range : \"{0}\"")
              ?.WithOnlyParam(shortStringSpan, ShortStringSubRange.Start.Value, ShortStringSubRange.Length());
        Assert.AreEqual($"Adding ShortSpan Range : \"{ShortStringSubRangeAsString}\"", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.DebugFormat("Adding LargeString Range : {0}")?
            .WithOnlyParam(LargeString, LargeStringSubRange.Start.Value, LargeStringSubRange.Length());
        Assert.AreEqual($"Adding LargeString Range : {LargeStringSubRangeAsString}", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.TraceFormat("Adding Full LargeString : {0}")?.WithOnlyParam(LargeString);
        Assert.AreEqual($"Adding Full LargeString : {LargeString}", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
    }

    [TestMethod]
    public void WithOnlyStringBuilderParamIsExpectedResult()
    {
        logger.InfoFormat("Adding Full SmallStringBuilder : \"{0}\"")?.WithOnlyParam(SmallStringBuilder);
        Assert.AreEqual($"Adding Full SmallStringBuilder : \"{ShortString}\"", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Adding SmallStringBuilder Range : \"{0}\"")?
            .WithOnlyParam(SmallStringBuilder, ShortStringSubRange.Start.Value, ShortStringSubRange.Length());
        Assert.AreEqual($"Adding SmallStringBuilder Range : \"{ShortStringSubRangeAsString}\""
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.DebugFormat("Adding LargeStringBuilder Range : \"{0}\"")?
            .WithOnlyParam(LargeStringBuilder, LargeStringSubRange.Start.Value, LargeStringSubRange.Length());
        Assert.AreEqual($"Adding LargeStringBuilder Range : \"{LargeStringSubRangeAsString}\""
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.TraceFormat("Adding Full LargeStringBuilder : \"{0}\"")?.WithOnlyParam(LargeStringBuilder);
        Assert.AreEqual($"Adding Full LargeStringBuilder : \"{LargeString}\"", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
    }

    [TestMethod]
    public void WithOnlyCharSequenceParamIsExpectedResult()
    {
        logger.InfoFormat("Adding Full SmallMutableString : \"{0}\"")?.WithOnlyParam(SmallMutableString);
        Assert.AreEqual($"Adding Full SmallMutableString : \"{ShortString}\"", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Adding SmallMutableString Range : \"{0}\"")
              ?.WithOnlyParam(SmallMutableString, ShortStringSubRange.Start.Value, ShortStringSubRange.Length());
        Assert.AreEqual($"Adding SmallMutableString Range : \"{ShortStringSubRangeAsString}\"",
                        spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Adding LargeMutableString Range : \"{0}\"")
              ?.WithOnlyParam(LargeMutableString, LargeStringSubRange.Start.Value, LargeStringSubRange.Length());
        Assert.AreEqual($"Adding LargeMutableString Range : \"{LargeStringSubRangeAsString}\"",
                        spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Adding Full LargeMutableString : \"{0}\"")?.WithOnlyParam(LargeMutableString);
        Assert.AreEqual($"Adding Full LargeMutableString : \"{LargeMutableString}\"", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
    }

    [TestMethod]
    public void WithDateTimeParamsIsFormattedAsExpected()
    {
        logger.InfoFormat("Formatting 2 DateTimeTicks with yyyy-MM-dd HH:mm:ss.ffffff : " +
                          "1) {0:yyyy-MM-dd HH:mm:ss.ffffff}, 2) {1:yyyy-MM-dd HH:mm:ss.ffffff}")?
            .WithParams(ToFormatDateTimeTicks)?.AndFinalParam(ToFormatDateTimeTicks);

        Assert.AreEqual($"Formatting 2 DateTimeTicks with yyyy-MM-dd HH:mm:ss.ffffff : " +
                        $"1) {ToFormatDateTimeTicksStandardAsString}, 2) {ToFormatDateTimeTicksStandardAsString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Formatting 2 DateTime with yyyy-MM-dd : 1) {0:yyyy-MM-dd}, 2) {1:yyyy-MM-dd}")?
            .WithParams(ToFormatDateTime)?.AndFinalParam(ToFormatDateTime);
        Assert.AreEqual($"Formatting 2 DateTime with yyyy-MM-dd : 1) {ToFormatDateStandardAsString}, 2) {ToFormatDateStandardAsString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Formatting 2 Time with -14:HH:mm:ss : 1) {0,-14:HH:mm:ss} 2) {1,-14:HH:mm:ss}")?
            .WithParams(ToFormatDateTime)?.AndFinalParam(ToFormatDateTime);
        Assert.AreEqual($"Formatting 2 Time with -14:HH:mm:ss : 1) {ToFormatTimeLeftAlignAsString} 2) {ToFormatTimeLeftAlignAsString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Formatting 2 Time with 14:HH:mm:ss : 1) {0,14:HH:mm:ss} 2) {1,14:HH:mm:ss}")?
            .WithParams(ToFormatDateTime)?.AndFinalParam(ToFormatDateTime);
        Assert.AreEqual($"Formatting 2 Time with 14:HH:mm:ss : 1) {ToFormatTimeRightAlignAsString} 2) {ToFormatTimeRightAlignAsString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
    }

    [TestMethod]
    public void WithBoolParamsIsFormattedAsExpected()
    {
        logger.InfoFormat("2 booleans true and false : 1) {0}, 2) {1}")?.WithParams(true)?.AndFinalParam(false);
        Assert.AreEqual("2 booleans true and false : 1) true, 2) false", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("2 null booleans : 1) {0}, 2) {1}")?.WithParams((bool?)null)?.AndFinalParam((bool?)null);
        Assert.AreEqual("2 null booleans : 1) null, 2) null", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
    }

    [TestMethod]
    public void WithNumberParamsIsFormattedAsExpected()
    {
        logger.InfoFormat("Formatting 2 Number 2dp : 1) {0:F2}, 2) {1:F2}")?.WithParams(ToFormatNumber)?.AndFinalParam(ToFormatNumber);
        Assert.AreEqual($"Formatting 2 Number 2dp : 1) {Number2DecimalPlaceString}, 2) {Number2DecimalPlaceString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.InfoFormat("Formatting 2 Number 3dp : 1) {0:F3}, 2) {1:F3}")?.WithParams(ToFormatNumber)?.AndFinalParam(ToFormatNumber);
        Assert.AreEqual($"Formatting 2 Number 3dp : 1) {Number3DecimalPlaceString}, 2) {Number3DecimalPlaceString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.DebugFormat("Formatting 2 Number 4dp : 1) {0:F4}, 2) {1:F4}")?.WithParams(ToFormatNumber)?.AndFinalParam(ToFormatNumber);
        Assert.AreEqual($"Formatting 2 Number 4dp : 1) {Number4DecimalPlaceString}, 2) {Number4DecimalPlaceString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.TraceFormat("Formatting 2 Number 5dp : 1) {0:F5}, 2) {1:F5}")?.WithParams(ToFormatNumber)?.AndFinalParam(ToFormatNumber);
        Assert.AreEqual($"Formatting 2 Number 5dp : 1) {Number5DecimalPlaceString}, 2) {Number5DecimalPlaceString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.TraceFormat("Formatting 2 Number 6dp : 1) {0:F6}, 2) {1:F6}")?.WithParams(ToFormatNumber)?.AndFinalParam(ToFormatNumber);
        Assert.AreEqual($"Formatting 2 Number 6dp : 1) {Number6DecimalPlaceString}, 2) {Number6DecimalPlaceString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Formatting 2 Number 2dp -14 : 1) {0,-14:F2}, 2) {1,-14:F2}")?.WithParams(ToFormatNumber)?.AndFinalParam(ToFormatNumber);
        Assert.AreEqual($"Formatting 2 Number 2dp -14 : 1) {Number2DecimalPlacesLeftAlignString}, 2) {Number2DecimalPlacesLeftAlignString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.InfoFormat("Formatting 2 Number 3dp -14 : 1) {0,-14:F3}, 2) {1,-14:F3}")?.WithParams(ToFormatNumber)?.AndFinalParam(ToFormatNumber);
        Assert.AreEqual($"Formatting 2 Number 3dp -14 : 1) {Number3DecimalPlacesLeftAlignString}, 2) {Number3DecimalPlacesLeftAlignString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.DebugFormat("Formatting 2 Number 4dp -14 : 1) {0,-14:F4}, 2) {1,-14:F4}")?.WithParams(ToFormatNumber)?.AndFinalParam(ToFormatNumber);
        Assert.AreEqual($"Formatting 2 Number 4dp -14 : 1) {Number4DecimalPlacesLeftAlignString}, 2) {Number4DecimalPlacesLeftAlignString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.TraceFormat("Formatting 2 Number 5dp -14 : 1) {0,-14:F5}, 2) {1,-14:F5}")?.WithParams(ToFormatNumber)?.AndFinalParam(ToFormatNumber);
        Assert.AreEqual($"Formatting 2 Number 5dp -14 : 1) {Number5DecimalPlacesLeftAlignString}, 2) {Number5DecimalPlacesLeftAlignString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.TraceFormat("Formatting 2 Number 6dp -14 : 1) {0,-14:F6}, 2) {1,-14:F6}")?.WithParams(ToFormatNumber)?.AndFinalParam(ToFormatNumber);
        Assert.AreEqual($"Formatting 2 Number 6dp -14 : 1) {Number6DecimalPlacesLeftAlignString}, 2) {Number6DecimalPlacesLeftAlignString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Formatting 2 Number 2dp -14 : 1) {0,14:F2}, 2) {1,14:F2}")?.WithParams(ToFormatNumber)?.AndFinalParam(ToFormatNumber);
        Assert.AreEqual($"Formatting 2 Number 2dp -14 : 1) {Number2DecimalPlacesRightAlignString}, 2) {Number2DecimalPlacesRightAlignString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.InfoFormat("Formatting 2 Number 3dp -14 : 1) {0,14:F3}, 2) {1,14:F3}")?.WithParams(ToFormatNumber)?.AndFinalParam(ToFormatNumber);
        Assert.AreEqual($"Formatting 2 Number 3dp -14 : 1) {Number3DecimalPlacesRightAlignString}, 2) {Number3DecimalPlacesRightAlignString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.DebugFormat("Formatting 2 Number 4dp -14 : 1) {0,14:F4}, 2) {1,14:F4}")?.WithParams(ToFormatNumber)?.AndFinalParam(ToFormatNumber);
        Assert.AreEqual($"Formatting 2 Number 4dp -14 : 1) {Number4DecimalPlacesRightAlignString}, 2) {Number4DecimalPlacesRightAlignString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.TraceFormat("Formatting 2 Number 5dp -14 : 1) {0,14:F5}, 2) {1,14:F5}")?.WithParams(ToFormatNumber)?.AndFinalParam(ToFormatNumber);
        Assert.AreEqual($"Formatting 2 Number 5dp -14 : 1) {Number5DecimalPlacesRightAlignString}, 2) {Number5DecimalPlacesRightAlignString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.TraceFormat("Formatting 2 Number 6dp -14 : 1) {0,14:F6}, 2) {1,14:F6}")?.WithParams(ToFormatNumber)?.AndFinalParam(ToFormatNumber);
        Assert.AreEqual($"Formatting 2 Number 6dp -14 : 1) {Number6DecimalPlacesRightAlignString}, 2) {Number6DecimalPlacesRightAlignString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
    }

    [TestMethod]
    public void WithEnumParamsIsFormattedAsExpected()
    {
        logger.InfoFormat("Formatting FLogLevel.Debug and Warn : 1) {0}, 2) {1}")?.WithParams(FLogLevel.Debug)?.AndFinalParam(FLogLevel.Warn);
        Assert.AreEqual("Formatting FLogLevel.Debug and Warn : 1) Debug, 2) Warn", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Formatting FLogLevel.Error -14 and Info 14 : 1) {0,-14}, 2) {1,14}")?
            .WithParams(FLogLevel.Error)?.AndFinalParam(FLogLevel.Info);
        Assert.AreEqual("Formatting FLogLevel.Error -14 and Info 14 : 1) Error         , 2)           Info"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Formatting FLogLevel.Info 14 and Trace -14 : 1) {0,14}, 2) {1,-14}")?
            .WithParams(FLogLevel.Info)?.AndFinalParam(FLogLevel.Trace);
        Assert.AreEqual("Formatting FLogLevel.Info 14 and Trace -14 : 1)           Info, 2) Trace         "
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
    }

    [TestMethod]
    public void WithCustomStylerParamsIsFormattedAsExpected()
    {
        logger.InfoFormat("Formatting 2 TimeSpan as ComplexType : 1) {0}, 2) {1}")
              ?.WithParams(ToFormatTimeSpan, ToFormatTimeSpan.StylerComplexType())
              ?.AndFinalParam(ToFormatTimeSpan, ToFormatTimeSpan.StylerComplexType());
        Assert.AreEqual($"Formatting 2 TimeSpan as ComplexType : 1) {TimeSpanStylerComplexAsString}, 2) {TimeSpanStylerComplexAsString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Formatting 2 TimeSpan as StringFormat : 1) {0,-14}, 2) {1,14}")?
            .WithParams(ToFormatTimeSpan)?.AndFinalParam(ToFormatTimeSpan);
        Assert.AreEqual($"Formatting 2 TimeSpan as StringFormat : 1) {TimeSpanSpanUnformattedAsString}, 2) {TimeSpanSpanUnformattedAsString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
    }

    [TestMethod]
    public void WithCharArrayParamsIsExpectedResult()
    {
        logger.InfoFormat("Adding 2 Small Char Array : 1) {0}, 2) {1}")?.WithParams(SmallCharsArray)?.AndFinalParam(SmallCharsArray);
        Assert.AreEqual($"Adding 2 Small Char Array : 1) {SmallCharArrayAsString}, 2) {SmallCharArrayAsString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Adding 2 Small Char Array Range : 1) {0}, 2) {1}")?
            .WithParams(SmallCharsArray, SmallCharArraySubRange.Start.Value, SmallCharArraySubRange.Length())?
            .AndFinalParam(SmallCharsArray, SmallCharArraySubRange.Start.Value, SmallCharArraySubRange.Length());
        Assert.AreEqual($"Adding 2 Small Char Array Range : 1) {SmallCharArraySubRangeAsString}, 2) {SmallCharArraySubRangeAsString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.DebugFormat("Adding 2 Large Char Array Range : 1) {0}, 2) {1}")?
            .WithParams(LargeCharsArray, LargeCharArraySubRange.Start.Value, LargeCharArraySubRange.Length())?
            .AndFinalParam(LargeCharsArray, LargeCharArraySubRange.Start.Value, LargeCharArraySubRange.Length());
        Assert.AreEqual($"Adding 2 Large Char Array Range : 1) {LargeCharArraySubRangeAsString}, 2) {LargeCharArraySubRangeAsString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.TraceFormat("Adding 2 Large Char Array : 1) {0}, 2) {1}")?.WithParams(LargeCharsArray)?.AndFinalParam(LargeCharsArray);
        Assert.AreEqual($"Adding 2 Large Char Array : 1) {LargeCharsArrayAsString}, 2) {LargeCharsArrayAsString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
    }

    [TestMethod]
    public void WithStringParamsIsExpectedResult()
    {
        logger.InfoFormat("Adding 2 Full ShortString : 1) \"{0}\", 2) \"{1}\"")?.WithParams(ShortString)?.AndFinalParam(ShortString);
        Assert.AreEqual($"Adding 2 Full ShortString : 1) \"{ShortString}\", 2) \"{ShortString}\""
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Adding 2 ShortString Range : 1) \"{0}\", 2) \"{1}\"")
              ?.WithParams(ShortString, ShortStringSubRange.Start.Value, ShortStringSubRange.Length())
              ?.AndFinalParam(ShortString, ShortStringSubRange.Start.Value, ShortStringSubRange.Length());
        Assert.AreEqual($"Adding 2 ShortString Range : 1) \"{ShortStringSubRangeAsString}\", 2) \"{ShortStringSubRangeAsString}\""
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        var shortStringSpan = ShortString.AsSpan();
        logger.InfoFormat("Adding 2 Full ShortSpan : 1) \"{0}\", 2) \"{1}\"")?.WithParams(shortStringSpan)?.AndFinalParam(shortStringSpan);
        Assert.AreEqual($"Adding 2 Full ShortSpan : 1) \"{ShortString}\", 2) \"{ShortString}\""
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Adding 2 ShortSpan Range : 1) \"{0}\", 2) \"{1}\"")
              ?.WithParams(shortStringSpan, ShortStringSubRange.Start.Value, ShortStringSubRange.Length())
              ?.AndFinalParam(shortStringSpan, ShortStringSubRange.Start.Value, ShortStringSubRange.Length());
        Assert.AreEqual($"Adding 2 ShortSpan Range : 1) \"{ShortStringSubRangeAsString}\", 2) \"{ShortStringSubRangeAsString}\""
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.DebugFormat("Adding 2 LargeString Range : 1) {0}, 2) {1}")
              ?.WithParams(LargeString, LargeStringSubRange.Start.Value, LargeStringSubRange.Length())
              ?.AndFinalParam(LargeString, LargeStringSubRange.Start.Value, LargeStringSubRange.Length());
        Assert.AreEqual($"Adding 2 LargeString Range : 1) {LargeStringSubRangeAsString}, 2) {LargeStringSubRangeAsString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.TraceFormat("Adding 2 Full LargeString : 1) {0}, 2) {1}")?.WithParams(LargeString)?.AndFinalParam(LargeString);
        Assert.AreEqual($"Adding 2 Full LargeString : 1) {LargeString}, 2) {LargeString}", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
    }

    [TestMethod]
    public void WithStringBuilderParamsIsExpectedResult()
    {
        logger.InfoFormat("Adding 2 Full SmallStringBuilder : 1) \"{0}\", 2) \"{1}\"")
              ?.WithParams(SmallStringBuilder)?.AndFinalParam(SmallStringBuilder);
        Assert.AreEqual($"Adding 2 Full SmallStringBuilder : 1) \"{ShortString}\", 2) \"{ShortString}\""
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Adding 2 SmallStringBuilder Range : 1) \"{0}\", 2) \"{1}\"")
              ?.WithParams(SmallStringBuilder, ShortStringSubRange.Start.Value, ShortStringSubRange.Length())
              ?.AndFinalParam(SmallStringBuilder, ShortStringSubRange.Start.Value, ShortStringSubRange.Length());
        Assert.AreEqual($"Adding 2 SmallStringBuilder Range : 1) \"{ShortStringSubRangeAsString}\", 2) \"{ShortStringSubRangeAsString}\"",
                        spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.DebugFormat("Adding 2 LargeStringBuilder Range : 1) \"{0}\", 2) \"{1}\"")
              ?.WithParams(LargeStringBuilder, LargeStringSubRange.Start.Value, LargeStringSubRange.Length())
              ?.AndFinalParam(LargeStringBuilder, LargeStringSubRange.Start.Value, LargeStringSubRange.Length());
        Assert.AreEqual($"Adding 2 LargeStringBuilder Range : 1) \"{LargeStringSubRangeAsString}\", 2) \"{LargeStringSubRangeAsString}\""
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.TraceFormat("Adding 2 Full LargeStringBuilder : 1) \"{0}\", 2) \"{1}\"")
              ?.WithParams(LargeStringBuilder)?.AndFinalParam(LargeStringBuilder);
        Assert.AreEqual($"Adding 2 Full LargeStringBuilder : 1) \"{LargeString}\", 2) \"{LargeString}\""
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
    }

    [TestMethod]
    public void WithCharSequenceParamsIsExpectedResult()
    {
        logger.InfoFormat("Adding 2 Full SmallMutableString : 1) \"{0}\", 2) \"{1}\"")
              ?.WithParams(SmallMutableString)?.AndFinalParam(SmallMutableString);
        Assert.AreEqual($"Adding 2 Full SmallMutableString : 1) \"{ShortString}\", 2) \"{ShortString}\""
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Adding 2 SmallMutableString Range : 1) \"{0}\", 2) \"{1}\"")
              ?.WithParams(SmallMutableString, ShortStringSubRange.Start.Value, ShortStringSubRange.Length())
              ?.AndFinalParam(SmallMutableString, ShortStringSubRange.Start.Value, ShortStringSubRange.Length());
        Assert.AreEqual($"Adding 2 SmallMutableString Range : 1) \"{ShortStringSubRangeAsString}\", 2) \"{ShortStringSubRangeAsString}\""
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.DebugFormat("Adding 2 LargeMutableString Range : 1) \"{0}\", 2) \"{1}\"")
              ?.WithParams(LargeMutableString, LargeStringSubRange.Start.Value, LargeStringSubRange.Length())
              ?.AndFinalParam(LargeMutableString, LargeStringSubRange.Start.Value, LargeStringSubRange.Length());
        Assert.AreEqual($"Adding 2 LargeMutableString Range : 1) \"{LargeStringSubRangeAsString}\", 2) \"{LargeStringSubRangeAsString}\""
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.TraceFormat("Adding 2 Full LargeMutableString : 1) \"{0}\", 2) \"{1}\"")
              ?.WithParams(LargeMutableString)?.AndFinalParam(LargeMutableString);
        Assert.AreEqual($"Adding 2 Full LargeMutableString : 1) \"{LargeMutableString}\", 2) \"{LargeMutableString}\""
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
    }

    [TestMethod]
    public void WithOnlyDateTimeParamThenToAppenderIsFormattedAsExpected()
    {
        logger.InfoFormat("Formatting 2 DateTimeTicks with yyyy-MM-dd HH:mm:ss.ffffff : " +
                          "1) {0:yyyy-MM-dd HH:mm:ss.ffffff}, 2) ")?
            .WithOnlyParamThenToAppender(ToFormatDateTimeTicks).FinalAppend(ToFormatDateTimeTicks);
        Assert.AreEqual($"Formatting 2 DateTimeTicks with yyyy-MM-dd HH:mm:ss.ffffff : " +
                        $"1) {ToFormatDateTimeTicksStandardAsString}, 2) {ToFormatDateTimeTicksUnformattedAsString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Formatting 2 DateTime with yyyy-MM-dd : 1) {0:yyyy-MM-dd}, 2) ")?
            .WithOnlyParamThenToAppender(ToFormatDateTime).FinalAppend(ToFormatDateTime);
        Assert.AreEqual($"Formatting 2 DateTime with yyyy-MM-dd : 1) {ToFormatDateStandardAsString}, 2) {ToFormatDateunformattedAsString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Formatting 2 Time with -14:HH:mm:ss : 1) {0,-14:HH:mm:ss} 2) ")?
            .WithOnlyParamThenToAppender(ToFormatDateTime).FinalAppend(ToFormatDateTime);
        Assert.AreEqual($"Formatting 2 Time with -14:HH:mm:ss : 1) {ToFormatTimeLeftAlignAsString} 2) {ToFormatDateunformattedAsString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Formatting 2 Time with 14:HH:mm:ss : 1) {0,14:HH:mm:ss} 2) ")?
            .WithOnlyParamThenToAppender(ToFormatDateTime).FinalAppend(ToFormatDateTime);
        Assert.AreEqual($"Formatting 2 Time with 14:HH:mm:ss : 1) {ToFormatTimeRightAlignAsString} 2) {ToFormatDateunformattedAsString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
    }

    [TestMethod]
    public void WithOnlyBoolParamThenToAppenderIsFormattedAsExpected()
    {
        logger.InfoFormat("2 booleans true and false : 1) {0}, 2) ")?.WithOnlyParamThenToAppender(true).FinalAppend(false);
        Assert.AreEqual("2 booleans true and false : 1) true, 2) false", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("2 null booleans : 1) {0}, 2) ")?.WithOnlyParamThenToAppender((bool?)null).FinalAppend((bool?)null);
        Assert.AreEqual("2 null booleans : 1) null, 2) null", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
    }

    [TestMethod]
    public void WithOnlyNumberParamThenToAppenderIsFormattedAsExpected()
    {
        logger.InfoFormat("Formatting 2 Number 2dp : 1) {0:F2}, 2) ")?.WithOnlyParamThenToAppender(ToFormatNumber).FinalAppend(ToFormatNumber);
        Assert.AreEqual($"Formatting 2 Number 2dp : 1) {Number2DecimalPlaceString}, 2) {ToFormatNumberUnformmatedString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.InfoFormat("Formatting 2 Number 3dp : 1) {0:F3}, 2) ")?.WithOnlyParamThenToAppender(ToFormatNumber).FinalAppend(ToFormatNumber);
        Assert.AreEqual($"Formatting 2 Number 3dp : 1) {Number3DecimalPlaceString}, 2) {ToFormatNumberUnformmatedString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.DebugFormat("Formatting 2 Number 4dp : 1) {0:F4}, 2) ")?.WithOnlyParamThenToAppender(ToFormatNumber).FinalAppend(ToFormatNumber);
        Assert.AreEqual($"Formatting 2 Number 4dp : 1) {Number4DecimalPlaceString}, 2) {ToFormatNumberUnformmatedString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.TraceFormat("Formatting 2 Number 5dp : 1) {0:F5}, 2) ")?.WithOnlyParamThenToAppender(ToFormatNumber).FinalAppend(ToFormatNumber);
        Assert.AreEqual($"Formatting 2 Number 5dp : 1) {Number5DecimalPlaceString}, 2) {ToFormatNumberUnformmatedString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.TraceFormat("Formatting 2 Number 6dp : 1) {0:F6}, 2) ")?.WithOnlyParamThenToAppender(ToFormatNumber).FinalAppend(ToFormatNumber);
        Assert.AreEqual($"Formatting 2 Number 6dp : 1) {Number6DecimalPlaceString}, 2) {ToFormatNumberUnformmatedString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Formatting 2 Number 2dp -14 : 1) {0,-14:F2}, 2) ")?.WithOnlyParamThenToAppender(ToFormatNumber)
              .FinalAppend(ToFormatNumber);
        Assert.AreEqual($"Formatting 2 Number 2dp -14 : 1) {Number2DecimalPlacesLeftAlignString}, 2) {ToFormatNumberUnformmatedString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.InfoFormat("Formatting 2 Number 3dp -14 : 1) {0,-14:F3}, 2) ")?.WithOnlyParamThenToAppender(ToFormatNumber)
              .FinalAppend(ToFormatNumber);
        Assert.AreEqual($"Formatting 2 Number 3dp -14 : 1) {Number3DecimalPlacesLeftAlignString}, 2) {ToFormatNumberUnformmatedString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.DebugFormat("Formatting 2 Number 4dp -14 : 1) {0,-14:F4}, 2) ")?.WithOnlyParamThenToAppender(ToFormatNumber)
              .FinalAppend(ToFormatNumber);
        Assert.AreEqual($"Formatting 2 Number 4dp -14 : 1) {Number4DecimalPlacesLeftAlignString}, 2) {ToFormatNumberUnformmatedString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.TraceFormat("Formatting 2 Number 5dp -14 : 1) {0,-14:F5}, 2) ")?.WithOnlyParamThenToAppender(ToFormatNumber)
              .FinalAppend(ToFormatNumber);
        Assert.AreEqual($"Formatting 2 Number 5dp -14 : 1) {Number5DecimalPlacesLeftAlignString}, 2) {ToFormatNumberUnformmatedString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.TraceFormat("Formatting 2 Number 6dp -14 : 1) {0,-14:F6}, 2) ")?.WithOnlyParamThenToAppender(ToFormatNumber)
              .FinalAppend(ToFormatNumber);
        Assert.AreEqual($"Formatting 2 Number 6dp -14 : 1) {Number6DecimalPlacesLeftAlignString}, 2) {ToFormatNumberUnformmatedString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Formatting 2 Number 2dp -14 : 1) {0,14:F2}, 2) ")?.WithOnlyParamThenToAppender(ToFormatNumber).FinalAppend(ToFormatNumber);
        Assert.AreEqual($"Formatting 2 Number 2dp -14 : 1) {Number2DecimalPlacesRightAlignString}, 2) {ToFormatNumberUnformmatedString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.InfoFormat("Formatting 2 Number 3dp -14 : 1) {0,14:F3}, 2) ")?.WithOnlyParamThenToAppender(ToFormatNumber).FinalAppend(ToFormatNumber);
        Assert.AreEqual($"Formatting 2 Number 3dp -14 : 1) {Number3DecimalPlacesRightAlignString}, 2) {ToFormatNumberUnformmatedString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.DebugFormat("Formatting 2 Number 4dp -14 : 1) {0,14:F4}, 2) ")?.WithOnlyParamThenToAppender(ToFormatNumber)
              .FinalAppend(ToFormatNumber);
        Assert.AreEqual($"Formatting 2 Number 4dp -14 : 1) {Number4DecimalPlacesRightAlignString}, 2) {ToFormatNumberUnformmatedString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.TraceFormat("Formatting 2 Number 5dp -14 : 1) {0,14:F5}, 2) ")?.WithOnlyParamThenToAppender(ToFormatNumber)
              .FinalAppend(ToFormatNumber);
        Assert.AreEqual($"Formatting 2 Number 5dp -14 : 1) {Number5DecimalPlacesRightAlignString}, 2) {ToFormatNumberUnformmatedString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
        logger.TraceFormat("Formatting 2 Number 6dp -14 : 1) {0,14:F6}, 2) ")?.WithOnlyParamThenToAppender(ToFormatNumber)
              .FinalAppend(ToFormatNumber);
        Assert.AreEqual($"Formatting 2 Number 6dp -14 : 1) {Number6DecimalPlacesRightAlignString}, 2) {ToFormatNumberUnformmatedString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
    }

    [TestMethod]
    public void WithOnlyEnumParamThenToAppenderIsFormattedAsExpected()
    {
        logger.InfoFormat("Formatting FLogLevel.Debug and Warn : 1) {0}, 2) ")?.WithOnlyParamThenToAppender(FLogLevel.Debug)
              .FinalAppend(FLogLevel.Warn);
        Assert.AreEqual("Formatting FLogLevel.Debug and Warn : 1) Debug, 2) Warn", spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Formatting FLogLevel.Error -14 and Info 14 : 1) {0,-14}, 2) ")?
            .WithOnlyParamThenToAppender(FLogLevel.Error).FinalAppend(FLogLevel.Info);
        Assert.AreEqual("Formatting FLogLevel.Error -14 and Info 14 : 1) Error         , 2) Info"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Formatting FLogLevel.Info 14 and Trace -14 : 1) {0,14}, 2) ")?
            .WithOnlyParamThenToAppender(FLogLevel.Info).FinalAppend(FLogLevel.Trace);
        Assert.AreEqual("Formatting FLogLevel.Info 14 and Trace -14 : 1)           Info, 2) Trace"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
    }

    [TestMethod]
    public void WithOnlyCustomStylerParamThenToAppenderIsFormattedAsExpected()
    {
        logger.InfoFormat("Formatting 2 TimeSpan as ComplexType : 1) {0}, 2) ")?
            .WithOnlyParamThenToAppender(ToFormatTimeSpan, ToFormatTimeSpan.StylerComplexType()).FinalAppend(ToFormatTimeSpan);
        Assert.AreEqual($"Formatting 2 TimeSpan as ComplexType : 1) {TimeSpanStylerComplexAsString}, 2) {TimeSpanSpanUnformattedAsString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Formatting 2 TimeSpan as StringFormat : 1) {0,-14}, 2) ")?
              .WithOnlyParamThenToAppender(ToFormatTimeSpan).FinalAppend(ToFormatTimeSpan);
        Assert.AreEqual($"Formatting 2 TimeSpan as StringFormat : 1) {TimeSpanSpanUnformattedAsString}, 2) {TimeSpanSpanUnformattedAsString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
    }

    [TestMethod]
    public void WithOnlyCharArrayParamThenToAppenderIsExpectedResult()
    {
        logger.InfoFormat("Adding 2 Small Char Array : 1) {0}, 2) ")?.WithOnlyParamThenToAppender(SmallCharsArray).FinalAppend(SmallCharsArray);
        Assert.AreEqual($"Adding 2 Small Char Array : 1) {SmallCharArrayAsString}, 2) {SmallCharArrayAsString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Adding 2 Small Char Array Range : 1) {0}, 2) ")?
              .WithOnlyParamThenToAppender(SmallCharsArray, SmallCharArraySubRange.Start.Value, SmallCharArraySubRange.Length())
              .FinalAppend(SmallCharsArray, SmallCharArraySubRange.Start.Value, SmallCharArraySubRange.Length());
        Assert.AreEqual($"Adding 2 Small Char Array Range : 1) {SmallCharArraySubRangeAsString}, 2) {SmallCharArraySubRangeAsString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.DebugFormat("Adding 2 Large Char Array Range : 1) {0}, 2) ")?
              .WithOnlyParamThenToAppender(LargeCharsArray, LargeCharArraySubRange.Start.Value, LargeCharArraySubRange.Length())
              .FinalAppend(LargeCharsArray, LargeCharArraySubRange.Start.Value, LargeCharArraySubRange.Length());
        Assert.AreEqual($"Adding 2 Large Char Array Range : 1) {LargeCharArraySubRangeAsString}, 2) {LargeCharArraySubRangeAsString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.TraceFormat("Adding 2 Large Char Array : 1) {0}, 2) ")?.WithOnlyParamThenToAppender(LargeCharsArray).FinalAppend(LargeCharsArray);
        Assert.AreEqual($"Adding 2 Large Char Array : 1) {LargeCharsArrayAsString}, 2) {LargeCharsArrayAsString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
    }

    [TestMethod]
    public void WithOnlyStringParamThenToAppenderIsExpectedResult()
    {
        logger.InfoFormat("Adding 2 Full ShortString : 1) \"{0}\", 2) ")?.WithOnlyParamThenToAppender(ShortString).FinalAppend(ShortString);
        Assert.AreEqual($"Adding 2 Full ShortString : 1) \"{ShortString}\", 2) {ShortString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Adding 2 ShortString Range : 1) \"{0}\", 2) ")
              ?.WithOnlyParamThenToAppender(ShortString, ShortStringSubRange.Start.Value, ShortStringSubRange.Length())
              .FinalAppend(ShortString, ShortStringSubRange.Start.Value, ShortStringSubRange.Length());
        Assert.AreEqual($"Adding 2 ShortString Range : 1) \"{ShortStringSubRangeAsString}\", 2) {ShortStringSubRangeAsString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        var shortStringSpan = ShortString.AsSpan();
        logger.InfoFormat("Adding 2 Full ShortSpan : 1) \"{0}\", 2) ")?.WithOnlyParamThenToAppender(shortStringSpan)
              .FinalAppend(shortStringSpan);
        Assert.AreEqual($"Adding 2 Full ShortSpan : 1) \"{ShortString}\", 2) {ShortString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Adding 2 ShortSpan Range : 1) \"{0}\", 2) ")
              ?.WithOnlyParamThenToAppender(shortStringSpan, ShortStringSubRange.Start.Value, ShortStringSubRange.Length())
              .FinalAppend(shortStringSpan, ShortStringSubRange.Start.Value, ShortStringSubRange.Length());
        Assert.AreEqual($"Adding 2 ShortSpan Range : 1) \"{ShortStringSubRangeAsString}\", 2) {ShortStringSubRangeAsString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.DebugFormat("Adding 2 LargeString Range : 1) \"{0}\", 2) ")
              ?.WithOnlyParamThenToAppender(LargeString, LargeStringSubRange.Start.Value, LargeStringSubRange.Length())
              .FinalAppend(LargeString, LargeStringSubRange.Start.Value, LargeStringSubRange.Length());
        Assert.AreEqual($"Adding 2 LargeString Range : 1) \"{LargeStringSubRangeAsString}\", 2) {LargeStringSubRangeAsString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.TraceFormat("Adding 2 Full LargeString : 1) \"{0}\", 2) ")?.WithOnlyParamThenToAppender(LargeString).FinalAppend(LargeString);
        Assert.AreEqual($"Adding 2 Full LargeString : 1) \"{LargeString}\", 2) {LargeString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
    }

    [TestMethod]
    public void WithOnlyStringBuilderParamThenToAppenderIsExpectedResult()
    {
        logger.InfoFormat("Adding 2 Full SmallStringBuilder : 1) \"{0}\", 2) ")
              ?.WithOnlyParamThenToAppender(SmallStringBuilder).FinalAppend(SmallStringBuilder);
        Assert.AreEqual($"Adding 2 Full SmallStringBuilder : 1) \"{ShortString}\", 2) {ShortString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Adding 2 SmallStringBuilder Range : 1) \"{0}\", 2) ")
              ?.WithOnlyParamThenToAppender(SmallStringBuilder, ShortStringSubRange.Start.Value, ShortStringSubRange.Length())
              .FinalAppend(SmallStringBuilder, ShortStringSubRange.Start.Value, ShortStringSubRange.Length());
        Assert.AreEqual($"Adding 2 SmallStringBuilder Range : 1) \"{ShortStringSubRangeAsString}\", 2) {ShortStringSubRangeAsString}",
                        spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.DebugFormat("Adding 2 LargeStringBuilder Range : 1) \"{0}\", 2) ")
              ?.WithOnlyParamThenToAppender(LargeStringBuilder, LargeStringSubRange.Start.Value, LargeStringSubRange.Length())
              .FinalAppend(LargeStringBuilder, LargeStringSubRange.Start.Value, LargeStringSubRange.Length());
        Assert.AreEqual($"Adding 2 LargeStringBuilder Range : 1) \"{LargeStringSubRangeAsString}\", 2) {LargeStringSubRangeAsString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.TraceFormat("Adding 2 Full LargeStringBuilder : 1) \"{0}\", 2) ")
              ?.WithOnlyParamThenToAppender(LargeStringBuilder).FinalAppend(LargeStringBuilder);
        Assert.AreEqual($"Adding 2 Full LargeStringBuilder : 1) \"{LargeString}\", 2) {LargeString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
    }

    [TestMethod]
    public void WithOnlyCharSequenceParamThenToAppenderIsExpectedResult()
    {
        logger.InfoFormat("Adding 2 Full SmallMutableString : 1) \"{0}\", 2) ")
              ?.WithOnlyParamThenToAppender(SmallMutableString).FinalAppend(SmallMutableString);
        Assert.AreEqual($"Adding 2 Full SmallMutableString : 1) \"{ShortString}\", 2) {ShortString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.InfoFormat("Adding 2 SmallMutableString Range : 1) \"{0}\", 2) ")
              ?.WithOnlyParamThenToAppender(SmallMutableString, ShortStringSubRange.Start.Value, ShortStringSubRange.Length())
              .FinalAppend(SmallMutableString, ShortStringSubRange.Start.Value, ShortStringSubRange.Length());
        Assert.AreEqual($"Adding 2 SmallMutableString Range : 1) \"{ShortStringSubRangeAsString}\", 2) {ShortStringSubRangeAsString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.DebugFormat("Adding 2 LargeMutableString Range : 1) \"{0}\", 2) ")
              ?.WithOnlyParamThenToAppender(LargeMutableString, LargeStringSubRange.Start.Value, LargeStringSubRange.Length())
              .FinalAppend(LargeMutableString, LargeStringSubRange.Start.Value, LargeStringSubRange.Length());
        Assert.AreEqual($"Adding 2 LargeMutableString Range : 1) \"{LargeStringSubRangeAsString}\", 2) {LargeStringSubRangeAsString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());

        logger.TraceFormat("Adding 2 Full LargeMutableString : 1) \"{0}\", 2) ")
              ?.WithOnlyParamThenToAppender(LargeMutableString).FinalAppend(LargeMutableString);
        Assert.AreEqual($"Adding 2 Full LargeMutableString : 1) \"{LargeMutableString}\", 2) {LargeMutableString}"
                      , spyRing.GetPurgeCacheLastLogEntry().Message.ToString());
    }
}
