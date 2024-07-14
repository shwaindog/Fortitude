// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using static FortitudeIO.TimeSeries.FileSystem.DirectoryStructure.RepositoryPathName;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;

public interface IPathPart
{
    PathName[] NameParts { get; set; }

    TimeSeriesPeriod PathTimeSeriesPeriod { get; }
    IPathDirectory?  ParentDirectory      { get; set; }

    IRepositoryRootDirectory RepositoryRootDirectory { get; }

    string NamePartSeparator { get; }
    string FullPath(IInstrument instrument, DateTime timeInPeriod, TimeSeriesPeriod forPeriod);
    bool   MatchesFormat(string pathPart);

    PathFileMatch       PathMatch(PathFileMatch currentMatch);
    PathInstrumentMatch InstrumentMatch(PathInstrumentMatch currentMatch);
}

public abstract class PathPart : IPathPart
{
    protected RepositoryPathName[] TimePeriodPart =
    {
        Decade
      , Year
      , Quarter
      , Month
      , WeekOfMonth
      , WeekOfYear
      , Day
      , Hour
    };

    protected PathPart(params PathName[] nameParts) => NameParts = nameParts;

    public string NamePartSeparator { get; } = "_";

    public PathName[] NameParts { get; set; }

    public virtual IPathDirectory? ParentDirectory { get; set; }

    public IRepositoryRootDirectory RepositoryRootDirectory =>
        ParentDirectory != null ? ParentDirectory.RepositoryRootDirectory : (IRepositoryRootDirectory)this;

    public virtual TimeSeriesPeriod PathTimeSeriesPeriod
    {
        get
        {
            if (this is IPathDirectory)
            {
                var anyTimeParts = NameParts.Where(np => TimePeriodPart.Contains(np.PathPart));
                if (anyTimeParts.Any())
                {
                    var smallestPeriod = anyTimeParts.Select(tsfsf => tsfsf.PathPart).Max();
                    switch (smallestPeriod)
                    {
                        case Decade: return TimeSeriesPeriod.OneDecade;
                        case Year:   return TimeSeriesPeriod.OneYear;
                        case Month:  return TimeSeriesPeriod.OneMonth;
                        case WeekOfYear:
                        case WeekOfMonth:
                            return TimeSeriesPeriod.OneWeek;
                        case Day:  return TimeSeriesPeriod.OneDay;
                        case Hour: return TimeSeriesPeriod.OneHour;
                    }
                }
            }
            return ParentDirectory?.PathTimeSeriesPeriod ?? throw new Exception("No directory contains periods with which to store files");
        }
    }

    public virtual string FullPath(IInstrument instrument, DateTime timeInPeriod, TimeSeriesPeriod forPeriod) =>
        ParentDirectory != null
            ? Path.Combine(ParentDirectory.FullPath(instrument, timeInPeriod, forPeriod), PathName(instrument, timeInPeriod, forPeriod))
            : PathName(instrument, timeInPeriod, forPeriod);

    public bool MatchesFormat(string pathPart)
    {
        var partSplit = pathPart.Split(NamePartSeparator);
        for (var i = 0; i < partSplit.Length && i < NameParts.Length; i++)
            if (!NameParts[i].MatchesExpectedFormat(partSplit[i]))
                return false;
        return true;
    }

    public virtual PathFileMatch PathMatch(PathFileMatch currentMatch)
    {
        currentMatch.InstrumentNameMatch   = ExtractInstrumentName(currentMatch.MatchedPath[^1]) ?? currentMatch.InstrumentNameMatch;
        currentMatch.InstrumentSourceMatch = ExtractInstrumentSource(currentMatch.MatchedPath[^1]) ?? currentMatch.InstrumentNameMatch;
        currentMatch.InstrumentTypeMatch   = ExtractInstrumentType(currentMatch.MatchedPath[^1]) ?? currentMatch.InstrumentTypeMatch;
        currentMatch.EntryPeriodMatch      = ExtractEntryPeriod(currentMatch.MatchedPath[^1]) ?? currentMatch.EntryPeriodMatch;
        currentMatch.FilePeriodMatch       = ExtractFilePeriod(currentMatch.MatchedPath[^1]) ?? currentMatch.FilePeriodMatch;
        foreach (var field in currentMatch.RequiredFields)
            currentMatch[field] = ExtractInstrumentField(field, currentMatch.MatchedPath[^1]) ?? currentMatch[field];
        foreach (var field in currentMatch.OptionalFields)
            currentMatch[field] = ExtractInstrumentField(field, currentMatch.MatchedPath[^1]) ?? currentMatch[field];
        return currentMatch;
    }

    public virtual PathInstrumentMatch InstrumentMatch(PathInstrumentMatch currentMatch) => currentMatch;

    public virtual string PathName(IInstrument instrument, DateTime timeInPeriod, TimeSeriesPeriod forPeriod) =>
        string.Join(NamePartSeparator, NameParts.Select(tsfpf => tsfpf.GetString(instrument, timeInPeriod, forPeriod)));

    public InstrumentType? ExtractInstrumentType(string pathPart)
    {
        var fileNameSplit = pathPart.Split(NamePartSeparator);
        for (var i = 0; i < fileNameSplit.Length && i < NameParts.Length; i++)
            if (NameParts[i].PathPart == RepositoryPathName.InstrumentType && NameParts[i].MatchesExpectedFormat(fileNameSplit[i]))
                return Enum.Parse<InstrumentType>(fileNameSplit[i]);
        return null;
    }

    public TimeSeriesPeriod? ExtractEntryPeriod(string pathPart)
    {
        var fileNameSplit = pathPart.Split(NamePartSeparator);
        for (var i = 0; i < fileNameSplit.Length && i < NameParts.Length; i++)
            if (NameParts[i].PathPart == EntryPeriod && NameParts[i].MatchesExpectedFormat(fileNameSplit[i]))
                return NameParts[i].ExtractTimeSeriesPeriod(fileNameSplit[i]);
        return null;
    }

    public string? ExtractInstrumentField(string instrumentFieldPart, string pathPart)
    {
        var fileNameSplit = pathPart.Split(NamePartSeparator);
        for (var i = 0; i < fileNameSplit.Length && i < NameParts.Length; i++)
            if (NameParts[i].PathPartString == instrumentFieldPart && NameParts[i].MatchesExpectedFormat(fileNameSplit[i]))
                return fileNameSplit[i];
        return null;
    }

    public string? ExtractInstrumentName(string pathPart)
    {
        var fileNameSplit = pathPart.Split(NamePartSeparator);
        for (var i = 0; i < fileNameSplit.Length && i < NameParts.Length; i++)
            if (NameParts[i].PathPart == InstrumentName && NameParts[i].MatchesExpectedFormat(fileNameSplit[i]))
                return fileNameSplit[i];
        return null;
    }

    public string? ExtractInstrumentSource(string pathPart)
    {
        var fileNameSplit = pathPart.Split(NamePartSeparator);
        for (var i = 0; i < fileNameSplit.Length && i < NameParts.Length; i++)
            if (NameParts[i].PathPart == InstrumentSource && NameParts[i].MatchesExpectedFormat(fileNameSplit[i]))
                return fileNameSplit[i];
        return null;
    }

    public TimeSeriesPeriod? ExtractFilePeriod(string pathPart)
    {
        var fileNameSplit = pathPart.Split(NamePartSeparator);
        for (var i = 0; i < fileNameSplit.Length && i < NameParts.Length; i++)
            if (NameParts[i].PathPart == EntryPeriod && NameParts[i].MatchesExpectedFormat(fileNameSplit[i]))
                return NameParts[i].ExtractTimeSeriesPeriod(fileNameSplit[i]);
        return null;
    }
}
