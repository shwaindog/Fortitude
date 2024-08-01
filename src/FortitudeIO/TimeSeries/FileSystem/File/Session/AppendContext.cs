// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries.FileSystem.File.Buckets;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File.Session;

public interface IAppendContext<TEntry> where TEntry : ITimeSeriesEntry
{
    IMutableBucket<TEntry>? LastAddedLeafBucket { get; set; }
    public DateTime         StorageTime         { get; set; }

    TEntry? PreviousEntry { get; set; }
    TEntry? CurrentEntry  { get; set; }
}

public interface ISessionAppendContext<TEntry, TBucket> : IAppendContext<TEntry>
    where TEntry : ITimeSeriesEntry
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
{
    TBucket? LastAddedRootBucket { get; set; }
}

public class AppendContext<TEntry, TBucket> : ISessionAppendContext<TEntry, TBucket>
    where TEntry : ITimeSeriesEntry
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
{
    public TBucket? LastAddedRootBucket { get; set; }
    public DateTime StorageTime         { get; set; }

    public IMutableBucket<TEntry>? LastAddedLeafBucket { get; set; }

    public TEntry? PreviousEntry { get; set; }
    public TEntry? CurrentEntry  { get; set; }
}
