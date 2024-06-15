// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem;

public interface ITimeSeriesRepositoryFileFactory
{
    Type EntryType { get; }
}

public interface ITimeSeriesRepositoryFileFactory<TEntry> : ITimeSeriesRepositoryFileFactory
    where TEntry : ITimeSeriesEntry<TEntry>
{
    ITimeSeriesEntryFile<TEntry>? OpenExisting(FileInfo fileInfo);

    ITimeSeriesEntryFile<TEntry> OpenOrCreate(FileInfo fileInfo, IInstrument instrument, TimeSeriesPeriod filePeriod
      , DateTime filePeriodTime);
}

public abstract class TimeSeriesRepositoryFileFactory<TEntry> : ITimeSeriesRepositoryFileFactory<TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>
{
    public Type EntryType => typeof(TEntry);

    public abstract ITimeSeriesEntryFile<TEntry>? OpenExisting(FileInfo fileInfo);

    public abstract ITimeSeriesEntryFile<TEntry> OpenOrCreate(FileInfo fileInfo, IInstrument instrument, TimeSeriesPeriod filePeriod
      , DateTime filePeriodTime);

    protected virtual TimeSeriesFileParameters CreateTimeSeriesFileParameters(FileInfo fileInfo, IInstrument instrument,
        TimeSeriesPeriod filePeriod, DateTime filePeriodTime)
    {
        var fileStart = filePeriod.ContainingPeriodBoundaryStart(filePeriodTime);
        return new TimeSeriesFileParameters(fileInfo, instrument, filePeriod, fileStart, initialFileFlags: FileFlags.WriterOpened);
    }
}

public class TimeSeriesRepositoryFileFactory<TFile, TBucket, TEntry> : TimeSeriesRepositoryFileFactory<TEntry>
  , ITimeSeriesRepositoryFileFactory<TEntry>
    where TFile : TimeSeriesFile<TFile, TBucket, TEntry>
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>
{
    public override ITimeSeriesEntryFile<TEntry>? OpenExisting(FileInfo fileInfo) =>
        TimeSeriesFile<TFile, TBucket, TEntry>.OpenExistingTimeSeriesFile(fileInfo.FullName);

    public override ITimeSeriesEntryFile<TEntry> OpenOrCreate(FileInfo fileInfo, IInstrument instrument, TimeSeriesPeriod filePeriod
      , DateTime filePeriodTime)
    {
        var openOrCreateParams = CreateTimeSeriesFileParameters(fileInfo, instrument, filePeriod, filePeriodTime);
        return new TimeSeriesFile<TFile, TBucket, TEntry>(openOrCreateParams);
    }
}
