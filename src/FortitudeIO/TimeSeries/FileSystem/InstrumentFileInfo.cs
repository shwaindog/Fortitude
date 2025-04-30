// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeIO.TimeSeries.FileSystem.Config;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem;

public struct InstrumentFileInfo
{
    public InstrumentFileInfo(IInstrument instrument, TimeBoundaryPeriod filePeriod = TimeBoundaryPeriod.OneWeek)
    {
        HasInstrument = false;
        Instrument    = instrument;
        FilePeriod    = filePeriod;

        ResponseGenerationTime = DateTime.UtcNow;
    }

    public InstrumentFileInfo
        (IInstrument instrument, TimeBoundaryPeriod filePeriod, DateTime? earliestEntry, DateTime? latestEntry, List<DateTime> fileStartDates)
    {
        HasInstrument  = true;
        EarliestEntry  = earliestEntry;
        FilePeriod     = filePeriod;
        FileStartDates = fileStartDates;
        Instrument     = instrument;
        LatestEntry    = latestEntry;

        ResponseGenerationTime = DateTime.UtcNow;
    }

    public IInstrument        Instrument     { get; }
    public bool               HasInstrument  { get; }
    public TimeBoundaryPeriod FilePeriod     { get; }
    public List<DateTime>?    FileStartDates { get; }
    public DateTime?          EarliestEntry  { get; }
    public DateTime?          LatestEntry    { get; }

    public DateTime ResponseGenerationTime { get; }
}

public struct FileEntryInfo
{
    public FileEntryInfo
    (DateTime fileStartTime, RepositoryProximity proximity, long entryCount, DateTime earliestEntryTime = default
      , DateTime latestEntryTime = default)
    {
        EntryCount        = entryCount;
        Proximity         = proximity;
        FileStartTime     = fileStartTime;
        EarliestEntryTime = earliestEntryTime;
        LatestEntryTime   = latestEntryTime;
    }

    public DateTime            FileStartTime     { get; }
    public long                EntryCount        { get; }
    public DateTime            EarliestEntryTime { get; }
    public DateTime            LatestEntryTime   { get; }
    public RepositoryProximity Proximity         { get; }
}

public struct InstrumentFileEntryInfo
{
    public InstrumentFileEntryInfo(IInstrument instrument, TimeBoundaryPeriod filePeriod = TimeBoundaryPeriod.OneWeek)
    {
        HasInstrument = false;
        Instrument    = instrument;
        FilePeriod    = filePeriod;
    }

    public InstrumentFileEntryInfo
        (IInstrument instrument, TimeBoundaryPeriod filePeriod, List<FileEntryInfo> fileEntryCounts, long totalEntriesCount)
    {
        HasInstrument     = true;
        FileEntryCounts   = fileEntryCounts;
        FilePeriod        = filePeriod;
        Instrument        = instrument;
        TotalEntriesCount = totalEntriesCount;
    }

    public IInstrument          Instrument      { get; }
    public bool                 HasInstrument   { get; }
    public TimeBoundaryPeriod   FilePeriod      { get; }
    public List<FileEntryInfo>? FileEntryCounts { get; }

    public long TotalEntriesCount { get; }
}
