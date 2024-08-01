// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Globalization;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Extensions;
using static FortitudeIO.TimeSeries.FileSystem.DirectoryStructure.RepositoryPathName;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;

public class PathName
{
    private string? pathPartString;

    public PathName(RepositoryPathName pathPart, string? formatString = null)
    {
        PathPart = pathPart;
        if (formatString == null)
            FormatString = PathPart switch
                           {
                               Decade             => "{0:yyyy}s"
                             , Year               => "{0:yy}"
                             , Month              => "{0:MM-MMM}"
                             , WeekOfMonth        => "Week-{0}"
                             , WeekOfYear         => "Week-{0:00}"
                             , Day                => "{0:dd}"
                             , DirectoryStartDate => "{0:yyyy-MM-dd}"
                             , Hour               => "{0:HH}"
                             , InstrumentName or InstrumentSource
                                              or Category
                                              or CoveringPeriod
                                              or FilePeriod
                                              or RepositoryPathName.InstrumentType
                                              or MarketRegion
                                              or MarketProductType
                                              or MarketType => "{0}"
                             , _ => throw new Exception($"No default formatting defined for {pathPart}")
                           };
        else
            FormatString = formatString;
    }

    public RepositoryPathName PathPart { get; }

    public string PathPartString => pathPartString ??= PathPart.ToString();

    public string FormatString { get; }

    public string? GetString(IInstrument instrument, DateTime timeInPeriod, TimeBoundaryPeriod forTimeBoundaryPeriod)
    {
        switch (PathPart)
        {
            case Decade:
                var decade = timeInPeriod.ToUniversalTime().TruncToDecadeBoundary();
                return string.Format(FormatString, decade);
            case Quarter:
                var quarter = timeInPeriod.ToUniversalTime().Month / 4 + 1;
                return string.Format(FormatString, quarter);
            case WeekOfMonth:
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
            case WeekOfYear:
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
            case DirectoryStartDate:
                var weekStart = timeInPeriod.ToUniversalTime().TruncToWeekBoundary();
                return string.Format(FormatString, weekStart);
            case Year:
            case Month:
            case Day:
            case Hour:
                return string.Format(FormatString, timeInPeriod);
            case InstrumentName: return string.Format(FormatString, instrument.InstrumentName);
            case InstrumentSource: return string.Format(FormatString, instrument.InstrumentSource);
            case RepositoryPathName.InstrumentType: return string.Format(FormatString, instrument.InstrumentType);
            case MarketProductType: return string.Format(FormatString, instrument[nameof(MarketProductType)]);
            case MarketRegion: return string.Format(FormatString, instrument[nameof(MarketRegion)]);
            case MarketType: return string.Format(FormatString, instrument[nameof(MarketType)]);
            case Category: return instrument[nameof(Category)] != null ? string.Format(FormatString, instrument[nameof(Category)]) : null;
            case CoveringPeriod: return string.Format(FormatString, instrument.CoveringPeriod.ShortName());
            case Constant: return FormatString;
            case FilePeriod: return string.Format(FormatString, forTimeBoundaryPeriod.ShortName());
            default: throw new Exception($"TimeSeriesFilePathType {PathPart} can not be converted to a TimeSeriesFilePathFormat");
        }
    }

    public bool MatchesExpectedFormat(string check)
    {
        switch (PathPart)
        {
            case Decade:
                var checkDecade = check.SafeExtractInt();
                return checkDecade is > 1900 and < 3000;
            case Quarter:
                var checkQuarter = check.SafeExtractInt();
                return checkQuarter is > 0 and < 5;
            case WeekOfMonth:
                var weekOfMonth = check.SafeExtractInt();
                return weekOfMonth is > 0 and < 5;
            case WeekOfYear:
                var weekOfYear = check.SafeExtractInt();
                return weekOfYear is > 0 and < 53;
            case Year:
                var checkYear = check.SafeExtractInt();
                return checkYear is > 1900 and < 3000;
            case Month:
                var checkMonth = check.SafeExtractInt();
                return checkMonth is > 0 and < 13;
            case Day:
                var checkDay = check.SafeExtractInt();
                return checkDay is > 0 and < 32;
            case Hour:
                var checkHour = check.SafeExtractInt();
                return checkHour is >= 0 and < 25;
            case InstrumentName:
            case Category:
            case InstrumentSource:
            case Constant:
                return true;
            case RepositoryPathName.InstrumentType: return Enum.TryParse<InstrumentType>(check, true, out var ignored);
            case FilePeriod:
            case CoveringPeriod:
                return check.ShortNameToDiscreetTimePeriod() != new DiscreetTimePeriod();
            default: throw new Exception($"TimeSeriesFilePathType {PathPart} interpreted as a valid type");
        }
    }

    public DateTime? ExtractDate(string namePart)
    {
        switch (PathPart)
        {
            case Year:
            case Decade:
                var succeeded = DateTime.TryParseExact(namePart, FormatString, null, DateTimeStyles.AdjustToUniversal, out var year);
                return succeeded ? year : null;
            default: return null;
        }
    }

    public DateTime? ExtractDate(DateTime baseDate, string namePart)
    {
        switch (PathPart)
        {
            case Year:
                var yearInt = namePart.SafeExtractInt();
                if (yearInt < 100)
                {
                    var timeYearsSucceeded = TimeSpan.TryParseExact(namePart, FormatString, null, out var yearsTimeSpan);
                    return timeYearsSucceeded ? baseDate.Add(yearsTimeSpan) : null;
                }
                var yearSucceeded = DateTime.TryParseExact(namePart, FormatString, null, DateTimeStyles.AdjustToUniversal, out var year);
                return yearSucceeded ? year : null;
            case Day:
            case Hour:
                var timeSpanSucceeded = TimeSpan.TryParseExact(namePart, FormatString, null, out var timeSpan);
                return timeSpanSucceeded ? baseDate.Add(timeSpan) : null;
            case Month:
                var parseMonth = namePart.SafeExtractInt();
                return parseMonth != null ? new DateTime(baseDate.Year, parseMonth.Value, 0).ToUniversalTime() : null;
            case WeekOfMonth:
                var monthWeekNum       = namePart.SafeExtractInt();
                var firstDayOfMonth    = baseDate.DayOfWeek;
                var firstSundayInMonth = firstDayOfMonth == DayOfWeek.Sunday ? baseDate : baseDate.AddDays(7 - (int)firstDayOfMonth);
                return monthWeekNum != null ? firstSundayInMonth.AddDays(7 * (monthWeekNum.Value - 1)) : null;
            case WeekOfYear:
                var yearWeekNum       = namePart.SafeExtractInt();
                var firstDayOfYear    = baseDate.DayOfWeek;
                var firstSundayInYear = firstDayOfYear == DayOfWeek.Sunday ? baseDate : baseDate.AddDays(7 - (int)firstDayOfYear);
                return yearWeekNum != null ? firstSundayInYear.AddDays(7 * (yearWeekNum.Value - 1)) : null;
            default: return null;
        }
    }

    public TimeBoundaryPeriod ExtractTimeBoundaryPeriod(string namePart)
    {
        switch (PathPart)
        {
            case FilePeriod: return namePart.ShortNameToTimeBoundaryPeriod();
            default:         return TimeBoundaryPeriod.None;
        }
    }

    public DiscreetTimePeriod ExtractDiscreetTimePeriod(string namePart)
    {
        switch (PathPart)
        {
            case CoveringPeriod: return namePart.ShortNameToDiscreetTimePeriod();
            default:             return new DiscreetTimePeriod();
        }
    }
}
