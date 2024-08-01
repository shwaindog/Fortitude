// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using static FortitudeIO.TimeSeries.FileSystem.DirectoryStructure.RepositoryPathName;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;

public interface IPathPart
{
    PathName[] NameParts { get; set; }

    TimeBoundaryPeriod PathTimeBoundaryPeriod { get; }
    IPathDirectory?    ParentDirectory        { get; set; }

    IRepositoryRootDirectory RepositoryRootDirectory { get; }

    string NamePartSeparator { get; }
    string FullPath(IInstrument instrument, DateTime timeInPeriod, TimeBoundaryPeriod forPeriod);
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

    public virtual TimeBoundaryPeriod PathTimeBoundaryPeriod
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
                        case Decade: return TimeBoundaryPeriod.OneDecade;
                        case Year:   return TimeBoundaryPeriod.OneYear;
                        case Month:  return TimeBoundaryPeriod.OneMonth;
                        case WeekOfYear:
                        case WeekOfMonth:
                            return TimeBoundaryPeriod.OneWeek;
                        case Day:  return TimeBoundaryPeriod.OneDay;
                        case Hour: return TimeBoundaryPeriod.OneHour;
                    }
                }
            }
            return ParentDirectory?.PathTimeBoundaryPeriod ?? throw new Exception("No directory contains periods with which to store files");
        }
    }

    public virtual string FullPath(IInstrument instrument, DateTime timeInPeriod, TimeBoundaryPeriod forPeriod) =>
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
        currentMatch.CoveringPeriodMatch   = ExtractCoveringPeriod(currentMatch.MatchedPath[^1]) ?? currentMatch.CoveringPeriodMatch;
        currentMatch.FilePeriodMatch       = ExtractFilePeriod(currentMatch.MatchedPath[^1]) ?? currentMatch.FilePeriodMatch;
        foreach (var field in currentMatch.RequiredFields)
            currentMatch[field] = ExtractInstrumentField(field, currentMatch.MatchedPath[^1]) ?? currentMatch[field];
        foreach (var field in currentMatch.OptionalFields)
            currentMatch[field] = ExtractInstrumentField(field, currentMatch.MatchedPath[^1]) ?? currentMatch[field];
        return currentMatch;
    }

    public virtual PathInstrumentMatch InstrumentMatch(PathInstrumentMatch currentMatch) => currentMatch;

    public virtual string PathName(IInstrument instrument, DateTime timeInPeriod, TimeBoundaryPeriod forPeriod) =>
        string.Join(NamePartSeparator, NameParts.Select(tsfpf => tsfpf.GetString(instrument, timeInPeriod, forPeriod)));

    public InstrumentType? ExtractInstrumentType(string pathPart)
    {
        var fileNameSplit = pathPart.Split(NamePartSeparator);
        for (var i = 0; i < fileNameSplit.Length && i < NameParts.Length; i++)
            if (NameParts[i].PathPart == RepositoryPathName.InstrumentType && NameParts[i].MatchesExpectedFormat(fileNameSplit[i]))
                return Enum.Parse<InstrumentType>(fileNameSplit[i]);
        return null;
    }

    public DiscreetTimePeriod? ExtractCoveringPeriod(string pathPart)
    {
        var fileNameSplit = pathPart.Split(NamePartSeparator);
        for (var i = 0; i < fileNameSplit.Length && i < NameParts.Length; i++)
            if (NameParts[i].PathPart == CoveringPeriod && NameParts[i].MatchesExpectedFormat(fileNameSplit[i]))
                return NameParts[i].ExtractDiscreetTimePeriod(fileNameSplit[i]);
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

    public TimeBoundaryPeriod? ExtractFilePeriod(string pathPart)
    {
        var fileNameSplit = pathPart.Split(NamePartSeparator);
        for (var i = 0; i < fileNameSplit.Length && i < NameParts.Length; i++)
            if (NameParts[i].PathPart == CoveringPeriod && NameParts[i].MatchesExpectedFormat(fileNameSplit[i]))
                return NameParts[i].ExtractTimeBoundaryPeriod(fileNameSplit[i]);
        return null;
    }
}
