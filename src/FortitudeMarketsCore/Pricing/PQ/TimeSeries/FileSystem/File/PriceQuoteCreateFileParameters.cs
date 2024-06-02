// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;

public struct PriceQuoteCreateFileParameters
{
    public PriceQuoteCreateFileParameters(ISourceTickerQuoteInfo sourceTickerQuoteInfo, IDirectoryFileNameResolver fileNameResolver,
        string instrumentName, string sourceName, TimeSeriesPeriod filePeriod, DateTime fileStartPeriod,
        TimeSeriesEntryType timeSeriesEntryType, PQSerializationFlags serializationFlags = PQSerializationFlags.ForStorage,
        uint internalIndexSize = 0, FileFlags initialFileFlags = FileFlags.None, int initialFileSize = ushort.MaxValue * 2,
        ushort maxStringSizeBytes = byte.MaxValue, string? category = null, ushort maxTypeStringSizeBytes = 512)
    {
        CreateFileParameters = new CreateFileParameters(fileNameResolver, instrumentName, sourceName, filePeriod,
                                                        fileStartPeriod, timeSeriesEntryType, internalIndexSize, initialFileFlags,
                                                        initialFileSize, maxStringSizeBytes, category, maxTypeStringSizeBytes);
        SourceTickerQuoteInfo = sourceTickerQuoteInfo;
        SerializationFlags    = serializationFlags;
    }

    public ISourceTickerQuoteInfo SourceTickerQuoteInfo;

    public PQSerializationFlags SerializationFlags;

    public CreateFileParameters CreateFileParameters;
}

public static class PriceQuoteCreateFileParametersExtensions
{
    public static PriceQuoteCreateFileParameters SetFilePeriod(this PriceQuoteCreateFileParameters input, TimeSeriesPeriod filePeriod)
    {
        var updated = input;
        updated.CreateFileParameters = input.CreateFileParameters.SetFilePeriod(filePeriod);
        return updated;
    }

    public static PriceQuoteCreateFileParameters SetInternalIndexSize(this PriceQuoteCreateFileParameters input, uint internalIndexSize)
    {
        var updated = input;
        updated.CreateFileParameters = input.CreateFileParameters.SetInternalIndexSize(internalIndexSize);
        return updated;
    }

    public static PriceQuoteCreateFileParameters SetTimeSeriesEntryType(this PriceQuoteCreateFileParameters input
      , TimeSeriesEntryType timeSeriesEntryType)
    {
        var updated = input;
        updated.CreateFileParameters = input.CreateFileParameters.SetTimeSeriesEntryType(timeSeriesEntryType);
        return updated;
    }

    public static PriceQuoteCreateFileParameters SetInitialFileSize(this PriceQuoteCreateFileParameters input, int initialFileSize)
    {
        var updated = input;
        updated.CreateFileParameters = input.CreateFileParameters.SetInitialFileSize(initialFileSize);
        return updated;
    }
}
