namespace FortitudeIO.TimeSeries;

public interface IStorageTimeResolver<in TEntry>
{
    DateTime ResolveStorageTime(TEntry entryTimeToResolve);
}

public interface ITimeSeriesEntry { }

public interface ITimeSeriesEntry<out TEntry> : ITimeSeriesEntry
{
    DateTime StorageTime(IStorageTimeResolver<TEntry>? resolver = null);
}
