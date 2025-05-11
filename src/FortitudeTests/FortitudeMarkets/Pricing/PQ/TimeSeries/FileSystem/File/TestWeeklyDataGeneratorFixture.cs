// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Summaries;
using FortitudeMarkets.Pricing.Generators.Quotes;
using FortitudeMarkets.Pricing.Generators.Summaries;
using FortitudeTests.FortitudeCommon.Extensions;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.TimeSeries.FileSystem.File;

public static class TestWeeklyDataGeneratorFixture
{
    public static IEnumerable<TQuoteLevel> GenerateQuotesForEachDayAndHourOfCurrentWeek<TQuoteLevel, TEntry>
        (int start, int amount, ITickGenerator<TEntry> tickGenerator, DateTime? forWeekWithDate = null)
        where TEntry : class, TQuoteLevel, IMutablePublishableTickInstant
    {
        var dateToGenerate   = forWeekWithDate?.Date ?? DateTime.UtcNow.Date;
        var currentDayOfWeek = dateToGenerate.DayOfWeek;
        var dayDiff          = DayOfWeek.Sunday - currentDayOfWeek;
        var startOfWeek      = dateToGenerate.AddDays(dayDiff);
        var currentDay       = startOfWeek;
        for (var i = 0; i < 7; i++)
        {
            for (var j = 0; j < 24; j++)
                foreach (var l1QuoteStruct in GenerateRepeatableQuotes<TQuoteLevel, TEntry>
                             (start, amount, j, currentDay.DayOfWeek, tickGenerator))
                    yield return l1QuoteStruct;
            currentDay = currentDay.AddDays(1);
        }
    }

    public static IEnumerable<TQuoteLevel> GenerateRepeatableQuotes<TQuoteLevel, TEntry>
        (int start, int amount, int hour, DayOfWeek forDayOfWeek, ITickGenerator<TEntry> tickGenerator, DateTime? forWeekWithDate = null)
        where TEntry : class, IMutablePublishableTickInstant, TQuoteLevel
    {
        var dateToGenerate   = forWeekWithDate?.Date ?? DateTime.UtcNow.Date;
        var currentDayOfWeek = dateToGenerate.DayOfWeek;
        var dayDiff          = forDayOfWeek - currentDayOfWeek;
        var generateDate     = dateToGenerate.AddDays(dayDiff);
        var hourToGenerate   = TimeSpan.FromHours(hour);
        var generateDateHour = generateDate.Add(hourToGenerate);

        for (var i = start; i < start + amount; i++)
        {
            var startTime = generateDateHour + TimeSpan.FromMilliseconds(i);
            yield return tickGenerator.Quotes(startTime, TimeSpan.FromMilliseconds(1), 1, i).First();
        }
    }

    public static IEnumerable<IPricePeriodSummary> GeneratePriceSummaryForEachDayAndHourOfCurrentWeek
    (int start, int amount, TimeBoundaryPeriod summaryPeriod, IPricePeriodSummaryGenerator<IMutablePricePeriodSummary> quoteGenerator
      , DateTime? forWeekWithDate = null)
    {
        var dateToGenerate   = forWeekWithDate?.Date ?? DateTime.UtcNow.Date;
        var currentDayOfWeek = dateToGenerate.DayOfWeek;
        var dayDiff          = DayOfWeek.Sunday - currentDayOfWeek;
        var startOfWeek      = dateToGenerate.AddDays(dayDiff);
        var currentDay       = startOfWeek;
        for (var i = 0; i < 7; i++)
        {
            for (var j = 0; j < 24; j++)
                foreach (var l1QuoteStruct in GenerateRepeatablePriceSummaries
                             (start, amount, j, currentDay.DayOfWeek, summaryPeriod, quoteGenerator))
                    yield return l1QuoteStruct;
            currentDay = currentDay.AddDays(1);
        }
    }

    public static IEnumerable<IPricePeriodSummary> GenerateRepeatablePriceSummaries
    (int start, int amount, int hour, DayOfWeek forDayOfWeek, TimeBoundaryPeriod summaryPeriod
      , IPricePeriodSummaryGenerator<IMutablePricePeriodSummary> quoteGenerator
      , DateTime? forWeekWithDate = null)
    {
        var dateToGenerate   = forWeekWithDate?.Date ?? DateTime.UtcNow.Date;
        var currentDayOfWeek = dateToGenerate.DayOfWeek;
        var dayDiff          = forDayOfWeek - currentDayOfWeek;
        var generateDate     = dateToGenerate.AddDays(dayDiff);
        var hourToGenerate   = TimeSpan.FromHours(hour);
        var generateDateHour = generateDate.Add(hourToGenerate);

        for (var i = start; i < start + amount; i++)
        {
            var startTime = generateDateHour + TimeSpan.FromSeconds(i);
            yield return quoteGenerator.PriceSummaries(startTime, TimeBoundaryPeriod.OneSecond, 1).First();
        }
    }

    public static FileInfo GenerateUniqueFileNameOffDateTime() => FileInfoExtensionsTests.UniqueNewTestFileInfo("PriceTimeSeriesTestFile");

    public static void DeleteTestFiles()
    {
        FileInfoExtensionsTests.DeleteTestFiles("PriceTimeSeriesTestFile");
    }
}
