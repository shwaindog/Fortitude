// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.Protocols;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.TimeSeries.FileSystem.File.Header;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.TimeSeries.FileSystem.File;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;

public class PriceQuoteTimeSeriesFile<TBucket, TEntry> : TimeSeriesFile<TBucket, TEntry>, IPriceQuoteTimeSeriesFile<TEntry>
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
    where TEntry : class, ITimeSeriesEntry<TEntry>, ILevel0Quote, IVersionedMessage
{
    public PriceQuoteTimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
        : base(pagedMemoryMappedFile, header)
    {
        Header.SubHeaderFactory = (view, offset, writable) => new PriceQuoteFileSubHeader<TEntry>(view, offset, writable);
        PriceQuoteFileHeader    = (IPriceQuoteFileHeader)Header.SubHeader!;
    }

    public PriceQuoteTimeSeriesFile(CreateSourceTickerQuoteFile sourceTickerTimeSeriesFileParams) : base(sourceTickerTimeSeriesFileParams
             .FileParameters)
    {
        Header.SubHeaderFactory = (view, offset, writable) => new PriceQuoteFileSubHeader<TEntry>(view, offset, writable);
        PriceQuoteFileHeader    = (IPriceQuoteFileHeader)Header.SubHeader!;
    }

    IPriceQuoteFileHeader IPriceQuoteTimeSeriesFile.PriceQuoteFileHeader => PriceQuoteFileHeader;

    public IPriceQuoteFileHeader PriceQuoteFileHeader { get; set; }
}
