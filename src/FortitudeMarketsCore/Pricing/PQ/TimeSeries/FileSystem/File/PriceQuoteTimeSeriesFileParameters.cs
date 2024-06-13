// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;

public struct PriceQuoteTimeSeriesFileParameters
{
    public PriceQuoteTimeSeriesFileParameters(ISourceTickerQuoteInfo sourceTickerQuoteInfo, FileInfo fileInfo,
        Instrument instrument, TimeSeriesPeriod filePeriod, DateTime fileStartPeriod,
        PQSerializationFlags serializationFlags = PQSerializationFlags.ForStorage,
        uint internalIndexSize = 0, FileFlags initialFileFlags = FileFlags.None, int initialFileSize = ushort.MaxValue * 2,
        ushort maxStringSizeBytes = byte.MaxValue, ushort maxTypeStringSizeBytes = 512)
    {
        TimeSeriesFileParameters = new TimeSeriesFileParameters(fileInfo, instrument, filePeriod,
                                                                fileStartPeriod, internalIndexSize, initialFileFlags,
                                                                initialFileSize, maxStringSizeBytes, maxTypeStringSizeBytes);
        SourceTickerQuoteInfo = sourceTickerQuoteInfo;
        SerializationFlags    = serializationFlags;
    }

    public PriceQuoteTimeSeriesFileParameters(ISourceTickerQuoteInfo sourceTickerQuoteInfo, TimeSeriesFileParameters timeSeriesFileParameters,
        PQSerializationFlags serializationFlags = PQSerializationFlags.ForStorage)
    {
        TimeSeriesFileParameters = timeSeriesFileParameters;
        SourceTickerQuoteInfo    = sourceTickerQuoteInfo;
        SerializationFlags       = serializationFlags;
    }

    public ISourceTickerQuoteInfo SourceTickerQuoteInfo;

    public PQSerializationFlags SerializationFlags;

    public TimeSeriesFileParameters TimeSeriesFileParameters;
}

public static class PriceQuoteCreateFileParametersExtensions
{
    public static PriceQuoteTimeSeriesFileParameters SetFilePeriod(this PriceQuoteTimeSeriesFileParameters input, TimeSeriesPeriod filePeriod)
    {
        var updated = input;
        updated.TimeSeriesFileParameters = input.TimeSeriesFileParameters.SetFilePeriod(filePeriod);
        return updated;
    }

    public static PriceQuoteTimeSeriesFileParameters SetInternalIndexSize(this PriceQuoteTimeSeriesFileParameters input, uint internalIndexSize)
    {
        var updated = input;
        updated.TimeSeriesFileParameters = input.TimeSeriesFileParameters.SetInternalIndexSize(internalIndexSize);
        return updated;
    }

    public static PriceQuoteTimeSeriesFileParameters SetTimeSeriesEntryType(this PriceQuoteTimeSeriesFileParameters input
      , InstrumentType instrumentType)
    {
        var updated = input;
        updated.TimeSeriesFileParameters = input.TimeSeriesFileParameters.SetTimeSeriesEntryType(instrumentType);
        return updated;
    }

    public static PriceQuoteTimeSeriesFileParameters SetInitialFileSize(this PriceQuoteTimeSeriesFileParameters input, int initialFileSize)
    {
        var updated = input;
        updated.TimeSeriesFileParameters = input.TimeSeriesFileParameters.SetInitialFileSize(initialFileSize);
        return updated;
    }
}
