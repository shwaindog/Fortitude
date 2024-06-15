// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;

public interface IPathDirectory : IPathPart, IEnumerable<IPathPart>
{
    int DepthFromRoot { get; }

    public List<IPathPart> Children { get; set; }
    void                   Add(IPathPart child);
}

public class PathDirectory : PathPart, IPathDirectory
{
    private List<IPathPart> children = new();
    public PathDirectory(params PathName[] nameParts) : base(nameParts) { }

    public int DepthFromRoot => ParentDirectory != null ? ParentDirectory.DepthFromRoot + 1 : 0;

    public List<IPathPart> Children
    {
        get => children;
        set
        {
            foreach (var child in value) child.ParentDirectory = this;
            children = value;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<IPathPart> GetEnumerator() => Children.GetEnumerator();

    public override PathFileMatch PathMatch(PathFileMatch currentMatch)
    {
        var dateChildDir =
            Children
                .OfType<IPathDirectory>()
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
                currentMatch.DeepestDirectoryMatch = this;
                currentMatch.PeriodStart           = checkDate.Value;
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
                    currentMatch.DeepestDirectoryMatch = this;
                    currentMatch.MatchedPath.Add(currentMatch.RemainingPath[0]);
                    currentMatch.RemainingPath.RemoveAt(0);
                    return timeSeriesPath.PathMatch(currentMatch);
                }
        }
        if (DepthFromRoot > currentMatch.DeepestDirectoryMatch.DepthFromRoot)
        {
            base.PathMatch(currentMatch);
            currentMatch.DeepestDirectoryMatch = this;
            currentMatch.MatchedPath.Add(currentMatch.RemainingPath[0]);
            currentMatch.RemainingPath.RemoveAt(0);
        }
        return currentMatch;
    }

    public override PathInstrumentMatch InstrumentMatch(PathInstrumentMatch currentMatch)
    {
        if (Children.Any(timeSeriesPath => timeSeriesPath.InstrumentMatch(currentMatch).HasTimeSeriesFileStructureMatch)) return currentMatch;
        return currentMatch;
    }

    public void Add(IPathPart child)
    {
        child.ParentDirectory = this;
        children.Add(child);
    }
}
