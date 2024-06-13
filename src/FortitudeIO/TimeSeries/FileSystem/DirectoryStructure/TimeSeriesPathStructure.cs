// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;

public interface ITimeSeriesPathStructure
{
    TimeSeriesPathNameFormat[] NameParts { get; set; }

    TimeSeriesPeriod               PathTimeSeriesPeriod { get; }
    ITimeSeriesDirectoryStructure? ParentDirectory      { get; set; }

    string NamePartSeparator { get; }
    string FullPath(Instrument instrument, DateTime timeInPeriod);
    bool   MatchesFormat(string pathPart);

    TimeSeriesFileStructureMatch       PathMatch(TimeSeriesFileStructureMatch currentMatch);
    TimeSeriesInstrumentStructureMatch InstrumentMatch(TimeSeriesInstrumentStructureMatch currentMatch);
}

public abstract class TimeSeriesPathStructure : ITimeSeriesPathStructure
{
    protected TimeSeriesPathNameComponent[] TimePeriodPart =
    {
        TimeSeriesPathNameComponent.Decade
      , TimeSeriesPathNameComponent.Year
      , TimeSeriesPathNameComponent.Quarter
      , TimeSeriesPathNameComponent.Month
      , TimeSeriesPathNameComponent.WeekOfMonth
      , TimeSeriesPathNameComponent.WeekOfYear
      , TimeSeriesPathNameComponent.Day
      , TimeSeriesPathNameComponent.Hour
    };

    protected TimeSeriesPathStructure(params TimeSeriesPathNameFormat[] nameParts) => NameParts = nameParts;

    public string NamePartSeparator { get; } = "_";

    public TimeSeriesPathNameFormat[] NameParts { get; set; }

    public virtual ITimeSeriesDirectoryStructure? ParentDirectory { get; set; }
    public virtual TimeSeriesPeriod PathTimeSeriesPeriod
    {
        get
        {
            if (this is ITimeSeriesDirectoryStructure)
            {
                var anyTimeParts = NameParts.Where(np => TimePeriodPart.Contains(np.PathPart));
                if (anyTimeParts.Any())
                {
                    var smallestPeriod = anyTimeParts.Select(tsfsf => tsfsf.PathPart).Max();
                    switch (smallestPeriod)
                    {
                        case TimeSeriesPathNameComponent.Decade: return TimeSeriesPeriod.OneDecade;
                        case TimeSeriesPathNameComponent.Year:   return TimeSeriesPeriod.OneYear;
                        case TimeSeriesPathNameComponent.Month:  return TimeSeriesPeriod.OneMonth;
                        case TimeSeriesPathNameComponent.WeekOfYear:
                        case TimeSeriesPathNameComponent.WeekOfMonth: return TimeSeriesPeriod.OneWeek;
                        case TimeSeriesPathNameComponent.Day:  return TimeSeriesPeriod.OneDay;
                        case TimeSeriesPathNameComponent.Hour: return TimeSeriesPeriod.OneHour;
                    }
                }
            }
            return ParentDirectory?.PathTimeSeriesPeriod ?? throw new Exception("No directory contains periods with which to store files");
        }
    }

    public virtual string FullPath(Instrument instrument, DateTime timeInPeriod) =>
        ParentDirectory != null
            ? Path.Combine(ParentDirectory.FullPath(instrument, timeInPeriod), PathName(instrument, timeInPeriod))
            : PathName(instrument, timeInPeriod);

    public bool MatchesFormat(string pathPart)
    {
        var partSplit = pathPart.Split(NamePartSeparator);
        for (var i = 0; i < partSplit.Length && i < NameParts.Length; i++)
            if (!NameParts[i].MatchesExpectedFormat(partSplit[i]))
                return false;
        return true;
    }

    public virtual TimeSeriesFileStructureMatch PathMatch(TimeSeriesFileStructureMatch currentMatch)
    {
        currentMatch.InstrumentNameMatch = ExtractInstrumentName(currentMatch.MatchedPath[^1]) ?? currentMatch.InstrumentNameMatch;
        currentMatch.SourceNameMatch     = ExtractSourceName(currentMatch.MatchedPath[^1]) ?? currentMatch.SourceNameMatch;
        currentMatch.InstrumentTypeMatch = ExtractInstrumentType(currentMatch.MatchedPath[^1]) ?? currentMatch.InstrumentTypeMatch;
        currentMatch.EntryPeriodMatch    = ExtractEntryPeriod(currentMatch.MatchedPath[^1]) ?? currentMatch.EntryPeriodMatch;
        currentMatch.CategoryMatch       = ExtractCategory(currentMatch.MatchedPath[^1]) ?? currentMatch.CategoryMatch;
        currentMatch.FilePeriodMatch     = ExtractFilePeriod(currentMatch.MatchedPath[^1]) ?? currentMatch.FilePeriodMatch;

        return currentMatch;
    }

    public virtual TimeSeriesInstrumentStructureMatch InstrumentMatch(TimeSeriesInstrumentStructureMatch currentMatch) => currentMatch;

    public virtual string PathName(Instrument instrument, DateTime timeInPeriod) =>
        string.Join(NamePartSeparator, NameParts.Select(tsfpf => tsfpf.GetString(instrument, timeInPeriod)));

    public InstrumentType? ExtractInstrumentType(string pathPart)
    {
        var fileNameSplit = pathPart.Split(NamePartSeparator);
        for (var i = 0; i < fileNameSplit.Length && i < NameParts.Length; i++)
            if (NameParts[i].PathPart == TimeSeriesPathNameComponent.TimeSeriesType && NameParts[i].MatchesExpectedFormat(fileNameSplit[i]))
                return Enum.Parse<InstrumentType>(fileNameSplit[i]);
        return null;
    }

    public TimeSeriesPeriod? ExtractEntryPeriod(string pathPart)
    {
        var fileNameSplit = pathPart.Split(NamePartSeparator);
        for (var i = 0; i < fileNameSplit.Length && i < NameParts.Length; i++)
            if (NameParts[i].PathPart == TimeSeriesPathNameComponent.EntryPeriod && NameParts[i].MatchesExpectedFormat(fileNameSplit[i]))
                return NameParts[i].ExtractTimeSeriesPeriod(fileNameSplit[i]);
        return null;
    }

    public string? ExtractSourceName(string pathPart)
    {
        var fileNameSplit = pathPart.Split(NamePartSeparator);
        for (var i = 0; i < fileNameSplit.Length && i < NameParts.Length; i++)
            if (NameParts[i].PathPart == TimeSeriesPathNameComponent.SourceName && NameParts[i].MatchesExpectedFormat(fileNameSplit[i]))
                return fileNameSplit[i];
        return null;
    }

    public string? ExtractInstrumentName(string pathPart)
    {
        var fileNameSplit = pathPart.Split(NamePartSeparator);
        for (var i = 0; i < fileNameSplit.Length && i < NameParts.Length; i++)
            if (NameParts[i].PathPart == TimeSeriesPathNameComponent.InstrumentName && NameParts[i].MatchesExpectedFormat(fileNameSplit[i]))
                return fileNameSplit[i];
        return null;
    }

    public string? ExtractCategory(string pathPart)
    {
        var fileNameSplit = pathPart.Split(NamePartSeparator);
        for (var i = 0; i < fileNameSplit.Length && i < NameParts.Length; i++)
            if (NameParts[i].PathPart == TimeSeriesPathNameComponent.Category && NameParts[i].MatchesExpectedFormat(fileNameSplit[i]))
                return fileNameSplit[i];
        return null;
    }

    public TimeSeriesPeriod? ExtractFilePeriod(string pathPart)
    {
        var fileNameSplit = pathPart.Split(NamePartSeparator);
        for (var i = 0; i < fileNameSplit.Length && i < NameParts.Length; i++)
            if (NameParts[i].PathPart == TimeSeriesPathNameComponent.EntryPeriod && NameParts[i].MatchesExpectedFormat(fileNameSplit[i]))
                return NameParts[i].ExtractTimeSeriesPeriod(fileNameSplit[i]);
        return null;
    }
}
