// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeIO.Storage.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeIO.Storage.TimeSeries;
using FortitudeIO.Storage.TimeSeries.FileSystem;
using FortitudeIO.Storage.TimeSeries.FileSystem.File;

#endregion

namespace FortitudeIO.Storage.TimeSeries.FileSystem.DirectoryStructure;

public enum PathUpdateType
{
    Available
  , Unavailable
}

public class RepositoryInstrumentFileUpdateEvent : EventArgs
{
    public RepositoryInstrumentFileUpdateEvent
    (IInstrument instrument, IPathFile pathFile,
        FileInfo file, TimeBoundaryPeriodRange filePeriodRange, ITimeSeriesFile? timeSeriesFile, PathUpdateType pathUpdateType)
    {
        Instrument      = instrument;
        PathFile        = pathFile;
        File            = file;
        FilePeriodRange = filePeriodRange;
        TimeSeriesFile  = timeSeriesFile;
        PathUpdateType  = pathUpdateType;
    }

    public IInstrument Instrument { get; }
    public IPathFile   PathFile   { get; }
    public FileInfo    File       { get; }

    public TimeBoundaryPeriodRange FilePeriodRange { get; }
    public ITimeSeriesFile?        TimeSeriesFile  { get; }
    public PathUpdateType          PathUpdateType  { get; }
}

public class InstrumentRepoFileUpdateEventArgs : EventArgs
{
    public InstrumentRepoFileUpdateEventArgs(InstrumentRepoFile instrumentRepoFile, PathUpdateType pathUpdateType)
    {
        InstrumentRepoFile = instrumentRepoFile;
        PathUpdateType     = pathUpdateType;
    }

    public InstrumentRepoFile InstrumentRepoFile { get; }
    public PathUpdateType     PathUpdateType     { get; }
}
