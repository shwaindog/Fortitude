// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;

public interface ITimeSeriesDirectoryStructure : ITimeSeriesPathStructure, IEnumerable<ITimeSeriesPathStructure>
{
    int DepthFromRoot { get; }

    List<ITimeSeriesPathStructure> Children { get; set; }
}

public class TimeSeriesDirectoryStructure : TimeSeriesPathStructure, ITimeSeriesDirectoryStructure
{
    private List<ITimeSeriesPathStructure> children = new();
    public TimeSeriesDirectoryStructure(params TimeSeriesPathNameFormat[] nameParts) : base(nameParts) { }

    public int DepthFromRoot => ParentDirectory != null ? ParentDirectory.DepthFromRoot + 1 : 0;

    public List<ITimeSeriesPathStructure> Children
    {
        get => children;
        set
        {
            foreach (var child in value) child.ParentDirectory = this;
            children = value;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<ITimeSeriesPathStructure> GetEnumerator() => Children.GetEnumerator();

    public override TimeSeriesFileStructureMatch PathMatch(TimeSeriesFileStructureMatch currentMatch)
    {
        var dateChildDir =
            Children
                .OfType<ITimeSeriesDirectoryStructure>()
                .FirstOrDefault(cd => cd.NameParts
                                        .Any(np => TimePeriodPart.Contains(np.PathPart)));
        if (dateChildDir != null && currentMatch.RemainingPath.Count >= 2)
        {
            var dateParts =
                NameParts
                    .Where(np => TimePeriodPart.Contains(np.PathPart)).ToList();

            var mostSpecific = dateParts
                .First(check => check.PathPart == dateParts.Max(tsfpf => tsfpf.PathPart));

            var checkDate = mostSpecific.ExtractDate(currentMatch.PeriodStart, currentMatch.RemainingPath[0]);
            if (checkDate != null)
            {
                base.PathMatch(currentMatch);
                currentMatch.DeepestDirectoryStructureMatch = this;
                currentMatch.PeriodStart                    = checkDate.Value;
                currentMatch.MatchedPath.Add(currentMatch.RemainingPath[0]);
                currentMatch.RemainingPath.RemoveAt(0);
                return dateChildDir.PathMatch(currentMatch);
            }
        }
        else
        {
            foreach (var timeSeriesPath in Children)
                if (timeSeriesPath.MatchesFormat(currentMatch.RemainingPath[0]))
                {
                    base.PathMatch(currentMatch);
                    currentMatch.DeepestDirectoryStructureMatch = this;
                    currentMatch.MatchedPath.Add(currentMatch.RemainingPath[0]);
                    currentMatch.RemainingPath.RemoveAt(0);
                    return timeSeriesPath.PathMatch(currentMatch);
                }
        }
        if (DepthFromRoot > currentMatch.DeepestDirectoryStructureMatch.DepthFromRoot)
        {
            base.PathMatch(currentMatch);
            currentMatch.DeepestDirectoryStructureMatch = this;
            currentMatch.MatchedPath.Add(currentMatch.RemainingPath[0]);
            currentMatch.RemainingPath.RemoveAt(0);
        }
        return currentMatch;
    }

    public override TimeSeriesInstrumentStructureMatch InstrumentMatch(TimeSeriesInstrumentStructureMatch currentMatch)
    {
        if (Children.Any(timeSeriesPath => timeSeriesPath.InstrumentMatch(currentMatch).HasTimeSeriesFileStructureMatch)) return currentMatch;
        return currentMatch;
    }

    public void Add(ITimeSeriesPathStructure child)
    {
        child.ParentDirectory = this;
        children.Add(child);
    }
}
