// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem;

public class InstrumentRepoFile : IComparable<InstrumentRepoFile>
{
    public InstrumentRepoFile(Instrument instrument, TimeSeriesRepoFile timeSeriesRepoFile, TimeSeriesPeriodRange filePeriodRange)
    {
        Instrument         = instrument;
        TimeSeriesRepoFile = timeSeriesRepoFile;
        FilePeriodRange    = filePeriodRange;
    }

    public Instrument                Instrument         { get; }
    public TimeSeriesRepoFile        TimeSeriesRepoFile { get; }
    public TimeSeriesPeriodRange     FilePeriodRange    { get; }
    public ITimeSeriesFileStructure? FileStructure      { get; set; }

    public int CompareTo(InstrumentRepoFile? other) => FilePeriodRange.PeriodStartTime < other?.FilePeriodRange.PeriodStartTime ? -1 : 1;


    public bool FileIntersects(TimeRange? timePeriod = null) => timePeriod.IntersectsWith(FilePeriodRange);

    protected bool Equals(InstrumentRepoFile other) =>
        Instrument.Equals(other.Instrument) && TimeSeriesRepoFile.Equals(other.TimeSeriesRepoFile) &&
        FilePeriodRange.Equals(other.FilePeriodRange);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((InstrumentRepoFile)obj);
    }

    public override int GetHashCode() => HashCode.Combine(Instrument, TimeSeriesRepoFile, FilePeriodRange);
}
