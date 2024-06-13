// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Globalization;
using FortitudeCommon.Extensions;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;

public class TimeSeriesPathNameFormat
{
    public TimeSeriesPathNameFormat(TimeSeriesPathNameComponent pathPart, string? formatString = null)
    {
        PathPart = pathPart;
        if (formatString == null)
            FormatString = PathPart switch
                           {
                               TimeSeriesPathNameComponent.Decade                                                => "{0:YYYY}"
                             , TimeSeriesPathNameComponent.Year                                                  => "{0:YY}"
                             , TimeSeriesPathNameComponent.Month                                                 => "{0:MM-MMM}"
                             , TimeSeriesPathNameComponent.WeekOfYear or TimeSeriesPathNameComponent.WeekOfMonth => "Week_{0:00}"
                             , TimeSeriesPathNameComponent.Day                                                   => "{0:DD}"
                             , TimeSeriesPathNameComponent.Hour                                                  => "{0:HH}"
                             , TimeSeriesPathNameComponent.InstrumentName or TimeSeriesPathNameComponent.SourceName
                                                                          or TimeSeriesPathNameComponent.Category
                                                                          or TimeSeriesPathNameComponent.EntryPeriod
                                                                          or TimeSeriesPathNameComponent.FilePeriod
                                                                          or TimeSeriesPathNameComponent.TimeSeriesType => "{0}"
                             , _ => throw new Exception($"No default formatting defined for {pathPart}")
                           };
        else
            FormatString = formatString;
    }

    public TimeSeriesPathNameComponent PathPart { get; }

    public string FormatString { get; }

    public string? GetString(Instrument instrument, DateTime timeInPeriod)
    {
        switch (PathPart)
        {
            case TimeSeriesPathNameComponent.Decade:
                var decade = timeInPeriod.ToUniversalTime().TruncToDecadeBoundary().Year;
                return string.Format(FormatString, decade);
            case TimeSeriesPathNameComponent.Quarter:
                var quarter = timeInPeriod.ToUniversalTime().Month / 4 + 1;
                return string.Format(FormatString, quarter);
            case TimeSeriesPathNameComponent.WeekOfMonth:
                var periodStart = timeInPeriod.ToUniversalTime().TruncToWeekBoundary();
                var weekNumber  = 1;
                var weekMonth   = periodStart.Month;
                var currentWeek = periodStart.AddDays(-7);
                while (currentWeek.Month == weekMonth)
                {
                    weekNumber++;
                    currentWeek = currentWeek.AddDays(-7);
                }
                return string.Format(FormatString, weekNumber);
            case TimeSeriesPathNameComponent.WeekOfYear:
                var startPeriod    = timeInPeriod.ToUniversalTime().TruncToWeekBoundary();
                var currWeekNumber = 1;
                var weekYear       = startPeriod.Year;
                var checkWeek      = startPeriod.AddDays(-7);
                while (checkWeek.Year == weekYear)
                {
                    currWeekNumber++;
                    checkWeek = checkWeek.AddDays(-7);
                }
                return string.Format(FormatString, currWeekNumber);
            case TimeSeriesPathNameComponent.Year:
            case TimeSeriesPathNameComponent.Month:
            case TimeSeriesPathNameComponent.Day:
            case TimeSeriesPathNameComponent.Hour:
                return string.Format(FormatString, timeInPeriod);
            case TimeSeriesPathNameComponent.InstrumentName:
                return string.Format(FormatString, instrument.InstrumentName);
            case TimeSeriesPathNameComponent.Category:
                return instrument.Category != null ? string.Format(FormatString, instrument.Category) : null;
            case TimeSeriesPathNameComponent.SourceName:
                return string.Format(FormatString, instrument.SourceName);
            case TimeSeriesPathNameComponent.TimeSeriesType:
                return string.Format(FormatString, instrument.TimeSeriesType);
            case TimeSeriesPathNameComponent.EntryPeriod:
                return string.Format(FormatString, instrument.EntryPeriod.ShortName());
            case TimeSeriesPathNameComponent.None:
                return FormatString;
            case TimeSeriesPathNameComponent.FilePeriod:
            default: throw new Exception($"TimeSeriesFilePathType {PathPart} can not be converted to a TimeSeriesFilePathFormat");
        }
    }

    public bool MatchesExpectedFormat(string check)
    {
        switch (PathPart)
        {
            case TimeSeriesPathNameComponent.Decade:
                var checkDecade = check.SafeExtractInt();
                return checkDecade is > 1900 and < 3000;
            case TimeSeriesPathNameComponent.Quarter:
                var checkQuarter = check.SafeExtractInt();
                return checkQuarter is > 0 and < 5;
            case TimeSeriesPathNameComponent.WeekOfMonth:
                var weekOfMonth = check.SafeExtractInt();
                return weekOfMonth is > 0 and < 5;
            case TimeSeriesPathNameComponent.WeekOfYear:
                var weekOfYear = check.SafeExtractInt();
                return weekOfYear is > 0 and < 53;
            case TimeSeriesPathNameComponent.Year:
                var checkYear = check.SafeExtractInt();
                return checkYear is > 1900 and < 3000;
            case TimeSeriesPathNameComponent.Month:
                var checkMonth = check.SafeExtractInt();
                return checkMonth is > 0 and < 13;
            case TimeSeriesPathNameComponent.Day:
                var checkDay = check.SafeExtractInt();
                return checkDay is > 0 and < 32;
            case TimeSeriesPathNameComponent.Hour:
                var checkHour = check.SafeExtractInt();
                return checkHour is >= 0 and < 25;
            case TimeSeriesPathNameComponent.InstrumentName:
            case TimeSeriesPathNameComponent.Category:
            case TimeSeriesPathNameComponent.SourceName:
            case TimeSeriesPathNameComponent.None:
                return true;
            case TimeSeriesPathNameComponent.TimeSeriesType:
                return Enum.TryParse<InstrumentType>(check, true, out var ignored);
            case TimeSeriesPathNameComponent.FilePeriod:
            case TimeSeriesPathNameComponent.EntryPeriod:
                return check.FromShortName() != TimeSeriesPeriod.None;
            default: throw new Exception($"TimeSeriesFilePathType {PathPart} interpreted as a valid type");
        }
    }

    public DateTime? ExtractDate(string namePart)
    {
        switch (PathPart)
        {
            case TimeSeriesPathNameComponent.Year:
            case TimeSeriesPathNameComponent.Decade:
                var succeeded = DateTime.TryParseExact(namePart, FormatString, null, DateTimeStyles.AdjustToUniversal, out var year);
                return succeeded ? year : null;
            default: return null;
        }
    }

    public DateTime? ExtractDate(DateTime baseDate, string namePart)
    {
        switch (PathPart)
        {
            case TimeSeriesPathNameComponent.Year:
                var yearInt = namePart.SafeExtractInt();
                if (yearInt < 100)
                {
                    var timeYearsSucceeded = TimeSpan.TryParseExact(namePart, FormatString, null, out var yearsTimeSpan);
                    return timeYearsSucceeded ? baseDate.Add(yearsTimeSpan) : null;
                }
                var yearSucceeded = DateTime.TryParseExact(namePart, FormatString, null, DateTimeStyles.AdjustToUniversal, out var year);
                return yearSucceeded ? year : null;
            case TimeSeriesPathNameComponent.Day:
            case TimeSeriesPathNameComponent.Hour:
                var timeSpanSucceeded = TimeSpan.TryParseExact(namePart, FormatString, null, out var timeSpan);
                return timeSpanSucceeded ? baseDate.Add(timeSpan) : null;
            case TimeSeriesPathNameComponent.Month:
                var parseMonth = namePart.SafeExtractInt();
                return parseMonth != null ? new DateTime(baseDate.Year, parseMonth.Value, 0).ToUniversalTime() : null;
            case TimeSeriesPathNameComponent.WeekOfMonth:
                var monthWeekNum       = namePart.SafeExtractInt();
                var firstDayOfMonth    = baseDate.DayOfWeek;
                var firstSundayInMonth = firstDayOfMonth == DayOfWeek.Sunday ? baseDate : baseDate.AddDays(7 - (int)firstDayOfMonth);
                return monthWeekNum != null ? firstSundayInMonth.AddDays(7 * (monthWeekNum.Value - 1)) : null;
            case TimeSeriesPathNameComponent.WeekOfYear:
                var yearWeekNum       = namePart.SafeExtractInt();
                var firstDayOfYear    = baseDate.DayOfWeek;
                var firstSundayInYear = firstDayOfYear == DayOfWeek.Sunday ? baseDate : baseDate.AddDays(7 - (int)firstDayOfYear);
                return yearWeekNum != null ? firstSundayInYear.AddDays(7 * (yearWeekNum.Value - 1)) : null;
            default: return null;
        }
    }

    public TimeSeriesPeriod ExtractTimeSeriesPeriod(string namePart)
    {
        switch (PathPart)
        {
            case TimeSeriesPathNameComponent.FilePeriod:
            case TimeSeriesPathNameComponent.EntryPeriod:
                return namePart.FromShortName();
            default: return TimeSeriesPeriod.None;
        }
    }
}
