// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.MemoryPools.Buffers.ByteBuffers.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.Storage.TimeSeries;
using FortitudeIO.Storage.TimeSeries.FileSystem.File;
using FortitudeIO.Storage.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.Storage.TimeSeries.FileSystem.File.Header;
using FortitudeIO.Storage.TimeSeries.FileSystem.File.Session;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.PQ.Storage.TimeSeries.FileSystem.File.Buckets;
using FortitudeMarkets.Pricing.TimeSeries.FileSystem.File;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Storage.TimeSeries.FileSystem.File;

public class CandleTimeSeriesFile<TFile, TBucket, TEntry> : TimeSeriesFile<TFile, TBucket, TEntry>, IPriceQuoteTimeSeriesFile
    where TFile : TimeSeriesFile<TFile, TBucket, TEntry>
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>, IPriceBucket
    where TEntry : ITimeSeriesEntry, ICandle
{
    public CandleTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header)
    {
        Header.SubHeaderFactory = (view, offset, writable) => new PriceFileSubHeader(header.InstrumentType, view, offset, writable);
        PriceFileHeader         = (IPriceFileHeader)Header.SubHeader!;
    }

    public CandleTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.TimeSeriesFileParameters)
    {
        Header.FileFlags        |= FileFlags.HasSubFileHeader | FileFlags.HasInternalIndexInHeader;
        Header.SubHeaderFactory =  (view, offset, writable) => new PriceFileSubHeader(sourceTickerTimeSeriesFileParams, view, offset, writable);
        PriceFileHeader         =  (IPriceFileHeader)Header.SubHeader!;
    }

    public override IBucketFactory<TBucket> RootBucketFactory
    {
        get { return FileBucketFactory ??= new PriceBucketFactory<TBucket>(PriceFileHeader.PricingInstrumentId, true); }
        set => FileBucketFactory = value;
    }

    public IPriceFileHeader PriceFileHeader { get; }

    public override ISessionAppendContext<TEntry, TBucket>
        CreateAppendContext() =>
        new PQCandleAppendContext<TEntry, TBucket>(PriceFileHeader.PricingInstrumentId);
}
