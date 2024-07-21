// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.TimeSeries.FileSystem.File.Header;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.TimeSeries.FileSystem.File;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;

public class PriceQuoteTimeSeriesFile<TFile, TBucket, TEntry> : TimeSeriesFile<TFile, TBucket, TEntry>, IPriceQuoteTimeSeriesFile
    where TFile : TimeSeriesFile<TFile, TBucket, TEntry>
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>, IPriceBucket
    where TEntry : ITimeSeriesEntry<TEntry>, ITickInstant
{
    public PriceQuoteTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header)
    {
        var instrumentType = header.InstrumentType;
        Header.SubHeaderFactory = (view, offset, writable) => new PriceFileSubHeader(instrumentType, view, offset, writable);
        PriceFileHeader         = (IPriceFileHeader)Header.SubHeader!;
    }

    public PriceQuoteTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
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
}
