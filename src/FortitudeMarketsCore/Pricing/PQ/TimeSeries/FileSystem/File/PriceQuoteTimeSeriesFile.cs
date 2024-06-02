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

public class PriceQuoteTimeSeriesFile<TBucket, TEntry> : TimeSeriesFile<TBucket, TEntry>, IPriceQuoteTimeSeriesFile
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>, IPriceQuoteBucket
    where TEntry : ITimeSeriesEntry<TEntry>, ILevel0Quote
{
    public PriceQuoteTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header)
    {
        Header.SubHeaderFactory = (view, offset, writable) => new PriceQuoteFileSubHeader(view, offset, writable);
        PriceQuoteFileHeader    = (IPriceQuoteFileHeader)Header.SubHeader!;
    }

    public PriceQuoteTimeSeriesFile(PriceQuoteCreateFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.CreateFileParameters)
    {
        Header.SubHeaderFactory = (view, offset, writable) => new PriceQuoteFileSubHeader(sourceTickerTimeSeriesFileParams, view, offset, writable);
        PriceQuoteFileHeader    = (IPriceQuoteFileHeader)Header.SubHeader!;
    }

    public override IBucketFactory<TBucket> RootBucketFactory
    {
        get { return FileBucketFactory ??= new PriceQuoteBucketFactory<TBucket>(PriceQuoteFileHeader.SourceTickerQuoteInfo, true); }
        set => FileBucketFactory = value;
    }

    public IPriceQuoteFileHeader PriceQuoteFileHeader { get; }
}
