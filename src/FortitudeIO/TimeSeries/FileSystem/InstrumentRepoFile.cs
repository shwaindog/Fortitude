// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem;

public class InstrumentRepoFile : IComparable<InstrumentRepoFile>, IEquatable<InstrumentRepoFile>, IInterfacesComparable<InstrumentRepoFile>
{
    public InstrumentRepoFile(IInstrument instrument, TimeSeriesRepoFile timeSeriesRepoFile, TimeSeriesPeriodRange filePeriodRange)
    {
        Instrument         = instrument;
        TimeSeriesRepoFile = timeSeriesRepoFile;
        FilePeriodRange    = filePeriodRange;
    }

    public IInstrument           Instrument         { get; }
    public TimeSeriesRepoFile    TimeSeriesRepoFile { get; }
    public TimeSeriesPeriodRange FilePeriodRange    { get; }
    public IPathFile?            FileStructure      { get; set; }
    public RepositoryProximity   Proximity          => TimeSeriesRepoFile.Proximity;

    public int CompareTo(InstrumentRepoFile? other) => FilePeriodRange.PeriodStartTime < other?.FilePeriodRange.PeriodStartTime ? -1 : 1;

    public bool Equals(InstrumentRepoFile? other) => AreEquivalent(other, true);

    public bool AreEquivalent(InstrumentRepoFile? other, bool exactTypes = false)
    {
        if (other == null) return false;
        var instrumentSame           = Instrument.Equals(other.Instrument);
        var periodRangeSame          = FilePeriodRange.Equals(other.FilePeriodRange);
        var repoFileSame             = true;
        if (exactTypes) repoFileSame = TimeSeriesRepoFile.Equals(other.TimeSeriesRepoFile);
        var allSame                  = instrumentSame && repoFileSame && periodRangeSame;
        return allSame;
    }


    public bool FileIntersects(TimeRange? timePeriod = null) => timePeriod.IntersectsWith(FilePeriodRange);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((InstrumentRepoFile)obj);
    }

    public override int GetHashCode() => HashCode.Combine(Instrument, TimeSeriesRepoFile, FilePeriodRange);
}

public class InstrumentRepoFileSet : IList<InstrumentRepoFile>
{
    private readonly List<InstrumentRepoFile> backingList;

    public InstrumentRepoFileSet() => backingList = new List<InstrumentRepoFile>();

    public InstrumentRepoFileSet(IEnumerable<InstrumentRepoFile> backingList) => this.backingList = backingList.ToList();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<InstrumentRepoFile> GetEnumerator() => backingList.GetEnumerator();

    public void Add(InstrumentRepoFile item)
    {
        if (!Contains(item)) backingList.Add(item);
    }

    public void Clear()
    {
        backingList.Clear();
    }

    public bool Contains(InstrumentRepoFile item) => backingList.Any(irf => irf.AreEquivalent(item));

    public void CopyTo(InstrumentRepoFile[] array, int arrayIndex)
    {
        backingList.CopyTo(array, arrayIndex);
    }

    public bool Remove(InstrumentRepoFile item)
    {
        var existing = backingList.FirstOrDefault(irf => irf.AreEquivalent(item));
        return existing != null && backingList.Remove(existing);
    }

    public int  Count      => backingList.Count;
    public bool IsReadOnly => false;

    public int IndexOf(InstrumentRepoFile item)
    {
        var existing = backingList.FirstOrDefault(irf => irf.AreEquivalent(item));
        return existing != null ? backingList.IndexOf(existing) : -1;
    }

    public void Insert(int index, InstrumentRepoFile item)
    {
        if (!Contains(item)) backingList.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        backingList.RemoveAt(index);
    }

    public InstrumentRepoFile this[int index]
    {
        get => backingList[index];
        set
        {
            if (!Contains(value))
                backingList[index] = value;
            else
                throw new Exception("Attempted to insert already equivalent item into Set");
        }
    }

    public static InstrumentRepoFileSet? operator +(InstrumentRepoFileSet? lhs, InstrumentRepoFileSet? rhs)
    {
        if (lhs == null && rhs == null) return null;
        if (lhs == null) return new InstrumentRepoFileSet(rhs!);
        if (rhs == null) return new InstrumentRepoFileSet(lhs);
        var result = new InstrumentRepoFileSet(lhs);
        foreach (var possibleDupe in rhs) result.Add(possibleDupe);
        return result;
    }

    public static InstrumentRepoFileSet? operator -(InstrumentRepoFileSet? lhs, InstrumentRepoFileSet? rhs)
    {
        if (lhs == null && rhs == null) return null;
        if (lhs == null) return null;
        if (rhs == null) return new InstrumentRepoFileSet(lhs);
        var result = new InstrumentRepoFileSet(lhs);
        foreach (var possibleMissing in rhs) result.Remove(possibleMissing);
        return result;
    }

    public void Sort() => backingList.Sort();
}

public static class InstrumentRepoFileSetExtensions
{
    public static InstrumentRepoFileSet ToInstrumentRepoFileSet(this IEnumerable<InstrumentRepoFile> toConvert) => new(toConvert);
}
