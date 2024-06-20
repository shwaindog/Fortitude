// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.TimeSeries.FileSystem.File.Header;
using FortitudeIO.TimeSeries.FileSystem.File.Session;
using FortitudeMarketsApi.Pricing.Summaries;
using FortitudeMarketsApi.Pricing.TimeSeries.FileSystem.File;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File.Buckets;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;

public class PriceSummaryTimeSeriesFile<TFile, TBucket> : TimeSeriesFile<TFile, TBucket, IPricePeriodSummary>, IPriceQuoteTimeSeriesFile
    where TFile : TimeSeriesFile<TFile, TBucket, IPricePeriodSummary>
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<IPricePeriodSummary>, IPriceBucket
{
    public PriceSummaryTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header)
    {
        Header.SubHeaderFactory = (view, offset, writable) => new PriceFileSubHeader(view, offset, writable);
        PriceFileHeader         = (IPriceFileHeader)Header.SubHeader!;
    }

    public PriceSummaryTimeSeriesFile(PriceTimeSeriesFileParameters sourceTickerTimeSeriesFileParams)
        : base(sourceTickerTimeSeriesFileParams.TimeSeriesFileParameters)
    {
        Header.FileFlags        |= FileFlags.HasSubFileHeader | FileFlags.HasInternalIndexInHeader;
        Header.SubHeaderFactory =  (view, offset, writable) => new PriceFileSubHeader(sourceTickerTimeSeriesFileParams, view, offset, writable);
        PriceFileHeader         =  (IPriceFileHeader)Header.SubHeader!;
    }

    public override IBucketFactory<TBucket> RootBucketFactory
    {
        get { return FileBucketFactory ??= new PriceBucketFactory<TBucket>(PriceFileHeader.SourceTickerQuoteInfo, true); }
        set => FileBucketFactory = value;
    }

    public IPriceFileHeader PriceFileHeader { get; }

    public override ISessionAppendContext<IPricePeriodSummary, TBucket>
        CreateAppendContext() =>
        new PQPricePeriodSummaryAppendContext<IPricePeriodSummary, TBucket>(PriceFileHeader.SourceTickerQuoteInfo);
}
