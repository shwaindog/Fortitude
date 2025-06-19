// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeIO.Storage.TimeSeries.FileSystem.File;
using FortitudeIO.Storage.TimeSeries.FileSystem.File.Buckets;

#endregion

namespace FortitudeIO.Storage.TimeSeries.FileSystem;

public interface ITimeSeriesRepositoryFileFactory
{
    Type EntryType { get; }
    bool IsBestFactoryFor(IInstrument instrument);

    ITimeSeriesFile? OpenExisting(FileInfo fileInfo);
}

public interface ITimeSeriesRepositoryFileFactory<TEntry> : ITimeSeriesRepositoryFileFactory
    where TEntry : ITimeSeriesEntry
{
    ITimeSeriesEntryFile<TEntry>? OpenExistingEntryFile(FileInfo fileInfo);

    ITimeSeriesEntryFile<TEntry> OpenOrCreate
    (FileInfo fileInfo, IInstrument instrument, TimeBoundaryPeriod filePeriod
      , DateTime filePeriodTime);
}

public abstract class TimeSeriesRepositoryFileFactory<TEntry> : ITimeSeriesRepositoryFileFactory<TEntry>
    where TEntry : ITimeSeriesEntry
{
    public Type EntryType => typeof(TEntry);

    public abstract ITimeSeriesEntryFile<TEntry>? OpenExistingEntryFile(FileInfo fileInfo);

    public abstract ITimeSeriesEntryFile<TEntry> OpenOrCreate
    (FileInfo fileInfo, IInstrument instrument, TimeBoundaryPeriod filePeriod
      , DateTime filePeriodTime);

    public abstract ITimeSeriesFile? OpenExisting(FileInfo fileInfo);
    public abstract bool             IsBestFactoryFor(IInstrument instrument);

    protected virtual TimeSeriesFileParameters CreateTimeSeriesFileParameters
    (FileInfo fileInfo, IInstrument instrument,
        TimeBoundaryPeriod filePeriod, DateTime filePeriodTime)
    {
        var fileStart = filePeriod.ContainingPeriodBoundaryStart(filePeriodTime);
        return new TimeSeriesFileParameters(fileInfo, instrument, filePeriod, fileStart, initialFileFlags: FileFlags.WriterOpened);
    }
}

public class TimeSeriesRepositoryFileFactory<TFile, TBucket, TEntry> : TimeSeriesRepositoryFileFactory<TEntry>
  , ITimeSeriesRepositoryFileFactory<TEntry>
    where TFile : TimeSeriesFile<TFile, TBucket, TEntry>
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
    where TEntry : ITimeSeriesEntry
{
    public override ITimeSeriesEntryFile<TEntry>? OpenExistingEntryFile(FileInfo fileInfo) =>
        TimeSeriesFile<TFile, TBucket, TEntry>.OpenExistingTimeSeriesFile(fileInfo.FullName);

    public override bool IsBestFactoryFor(IInstrument instrument) => true;

    public override ITimeSeriesEntryFile<TEntry> OpenOrCreate
        (FileInfo fileInfo, IInstrument instrument, TimeBoundaryPeriod filePeriod, DateTime filePeriodTime)
    {
        var openOrCreateParams = CreateTimeSeriesFileParameters(fileInfo, instrument, filePeriod, filePeriodTime);
        return new TimeSeriesFile<TFile, TBucket, TEntry>(openOrCreateParams);
    }

    public override ITimeSeriesEntryFile<TEntry>? OpenExisting(FileInfo fileInfo) => OpenExistingEntryFile(fileInfo);
}
