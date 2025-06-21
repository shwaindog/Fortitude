// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeIO.Storage.TimeSeries;

public interface IStorageTimeResolver { }

public interface IStorageTimeResolver<in TEntry> : IStorageTimeResolver
{
    DateTime ResolveStorageTime(TEntry entryTimeToResolve);
}

public interface ITimeSeriesEntry
{
    DateTime StorageTime(IStorageTimeResolver? resolver = null);
}
