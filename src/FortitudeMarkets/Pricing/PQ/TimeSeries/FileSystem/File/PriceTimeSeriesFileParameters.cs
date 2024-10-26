// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeMarkets.Pricing;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarkets.Pricing.PQ.TimeSeries.FileSystem.File;

public struct PriceTimeSeriesFileParameters
{
    public PriceTimeSeriesFileParameters
    (IPricingInstrumentId pricingInstrumentId, FileInfo fileInfo,
        IInstrument instrument, TimeBoundaryPeriod filePeriod, DateTime fileStartPeriod,
        PQSerializationFlags serializationFlags = PQSerializationFlags.ForStorage,
        uint internalIndexSize = 0, FileFlags initialFileFlags = FileFlags.None, int initialFileSize = ushort.MaxValue * 2,
        ushort maxStringSizeBytes = byte.MaxValue, ushort maxTypeStringSizeBytes = 512)
    {
        TimeSeriesFileParameters = new TimeSeriesFileParameters(fileInfo, instrument, filePeriod,
                                                                fileStartPeriod, internalIndexSize, initialFileFlags,
                                                                initialFileSize, maxStringSizeBytes, maxTypeStringSizeBytes);
        PricingInstrumentId = pricingInstrumentId;
        SerializationFlags  = serializationFlags;
    }

    public PriceTimeSeriesFileParameters
    (IPricingInstrumentId pricingInstrumentId, TimeSeriesFileParameters timeSeriesFileParameters,
        PQSerializationFlags serializationFlags = PQSerializationFlags.ForStorage)
    {
        TimeSeriesFileParameters = timeSeriesFileParameters;
        PricingInstrumentId      = pricingInstrumentId;
        SerializationFlags       = serializationFlags;
    }

    public IPricingInstrumentId PricingInstrumentId;

    public PQSerializationFlags SerializationFlags;

    public TimeSeriesFileParameters TimeSeriesFileParameters;
}

public static class PriceQuoteCreateFileParametersExtensions
{
    public static PriceTimeSeriesFileParameters SetFilePeriod(this PriceTimeSeriesFileParameters input, TimeBoundaryPeriod filePeriod)
    {
        var updated = input;
        updated.TimeSeriesFileParameters = input.TimeSeriesFileParameters.SetFilePeriod(filePeriod);
        return updated;
    }

    public static PriceTimeSeriesFileParameters SetInternalIndexSize(this PriceTimeSeriesFileParameters input, uint internalIndexSize)
    {
        var updated = input;
        updated.TimeSeriesFileParameters = input.TimeSeriesFileParameters.SetInternalIndexSize(internalIndexSize);
        return updated;
    }

    public static PriceTimeSeriesFileParameters SetFileFlags(this PriceTimeSeriesFileParameters input, FileFlags toSet)
    {
        var updated = input;
        updated.TimeSeriesFileParameters = input.TimeSeriesFileParameters.SetFileFlags(toSet);
        return updated;
    }

    public static PriceTimeSeriesFileParameters AssertTimeSeriesEntryType
    (this PriceTimeSeriesFileParameters input
      , InstrumentType instrumentType)
    {
        input.TimeSeriesFileParameters.AssertTimeSeriesEntryType(instrumentType);
        return input;
    }

    public static PriceTimeSeriesFileParameters SetInitialFileSize(this PriceTimeSeriesFileParameters input, int initialFileSize)
    {
        var updated = input;
        updated.TimeSeriesFileParameters = input.TimeSeriesFileParameters.SetInitialFileSize(initialFileSize);
        return updated;
    }
}
