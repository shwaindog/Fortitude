// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Quotes.Generators;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;

public static class TestWeeklyDataGeneratorFixture
{
    private static int newTestCount;

    public static IEnumerable<TQuoteLevel> GenerateForEachDayAndHourOfCurrentWeek<TQuoteLevel, TEntry>(int start, int amount
      , IQuoteGenerator<TEntry> quoteGenerator)
        where TEntry : class, TQuoteLevel, IMutableLevel0Quote
    {
        var dateToGenerate   = DateTime.UtcNow.Date;
        var currentDayOfWeek = dateToGenerate.DayOfWeek;
        var dayDiff          = DayOfWeek.Sunday - currentDayOfWeek;
        var startOfWeek      = dateToGenerate.AddDays(dayDiff);
        var currentDay       = startOfWeek;
        for (var i = 0; i < 7; i++)
        {
            for (var j = 0; j < 24; j++)
                foreach (var l1QuoteStruct in GenerateRepeatableL1QuoteStructs<TQuoteLevel, TEntry>
                             (start, amount, j, currentDay.DayOfWeek, quoteGenerator))
                    yield return l1QuoteStruct;
            currentDay = currentDay.AddDays(1);
        }
    }

    public static IEnumerable<TQuoteLevel> GenerateRepeatableL1QuoteStructs<TQuoteLevel, TEntry>(int start, int amount,
        int hour, DayOfWeek forDayOfWeek, IQuoteGenerator<TEntry> quoteGenerator)
        where TEntry : class, IMutableLevel0Quote, TQuoteLevel
    {
        var dateToGenerate   = DateTime.UtcNow.Date;
        var currentDayOfWeek = dateToGenerate.DayOfWeek;
        var dayDiff          = forDayOfWeek - currentDayOfWeek;
        var generateDate     = dateToGenerate.AddDays(dayDiff);
        var hourToGenerate   = TimeSpan.FromHours(hour);
        var generateDateHour = generateDate.Add(hourToGenerate);

        for (var i = start; i < start + amount; i++)
        {
            var startTime = generateDateHour + TimeSpan.FromMilliseconds(i);
            yield return quoteGenerator.Quotes(startTime, TimeSpan.FromMilliseconds(1), 1, i).First();
        }
    }

    public static string GenerateUniqueFileNameOffDateTime()
    {
        var now = DateTime.Now;
        Interlocked.Increment(ref newTestCount);
        var nowString = now.ToString("yyyy-MM-dd_HH-mm-ss_fff");
        return "WeeklyQuoteTimeSeriesFile_" + nowString + "_" + newTestCount + ".bin";
    }
}
