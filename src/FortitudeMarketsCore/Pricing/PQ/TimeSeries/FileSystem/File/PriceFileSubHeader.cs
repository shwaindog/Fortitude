// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.TimeSeries.FileSystem.File;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;

public interface ISerializationPriceFileHeader : IPriceFileHeader
{
    PQSerializationFlags SerializationFlags { get; set; }
}

public struct PriceQuoteSubHeader
{
    public PQSerializationFlags SerializationFlags;

    public ushort PricingInstrumentIdSerializedSizeBytes;
    public ushort PricingInstrumentSubHeaderBytesOffset;
}

public unsafe class PriceFileSubHeader : ISerializationPriceFileHeader
{
    private static IRecycler recycler = new Recycler();

    private readonly InstrumentType                instrumentType;
    private readonly MemoryMappedFileBuffer        memoryMappedFileBuffer;
    private readonly IMessageBufferContext         messageBufferContext;
    private readonly PricingInstrumentDeserializer pricingInstrumentDeserializer = new(recycler);
    private readonly SourceTickerInfoDeserializer sourceTickerInfoDeserializer = new(recycler)
    {
        ReadMessageHeader = false
    };
    private readonly ushort subHeaderFileOffset;
    private readonly bool   writable;

    private IPricingInstrumentId?          cachedPricingInstrumentId;
    private PriceQuoteSubHeader            cachePriceQuoteSubHeader;
    private ShiftableMemoryMappedFileView? memoryMappedFileView;
    private PriceQuoteSubHeader*           priceQuoteSubHeader;
    private PricingInstrumentSerializer?   pricingInstrumentSerializer;
    private SourceTickerInfoSerializer?    sourceTickerInfoSerializer;

    public PriceFileSubHeader
        (InstrumentType instrumentType, ShiftableMemoryMappedFileView headerMappedFileView, ushort subHeaderFileOffset, bool writable)
    {
        this.instrumentType      = instrumentType;
        memoryMappedFileView     = headerMappedFileView;
        this.subHeaderFileOffset = subHeaderFileOffset;
        this.writable            = writable;
        priceQuoteSubHeader      = (PriceQuoteSubHeader*)(memoryMappedFileView.StartAddress + subHeaderFileOffset);
        memoryMappedFileBuffer   = new MemoryMappedFileBuffer(memoryMappedFileView, false);
        messageBufferContext     = new MessageBufferContext(memoryMappedFileBuffer);

        priceQuoteSubHeader->PricingInstrumentSubHeaderBytesOffset = 100;
    }

    public PriceFileSubHeader
    (PriceTimeSeriesFileParameters priceTimeSeriesFileParameters,
        ShiftableMemoryMappedFileView headerMappedFileView, ushort subHeaderFileOffset, bool writable)
        : this(priceTimeSeriesFileParameters.PricingInstrumentId.InstrumentType, headerMappedFileView, subHeaderFileOffset, writable)
    {
        PricingInstrumentId = priceTimeSeriesFileParameters.PricingInstrumentId;
        SerializationFlags  = priceTimeSeriesFileParameters.SerializationFlags;
    }

    public IPricingInstrumentId PricingInstrumentId
    {
        get
        {
            if (memoryMappedFileView == null && cachedPricingInstrumentId != null) return cachedPricingInstrumentId;
            if (priceQuoteSubHeader->PricingInstrumentIdSerializedSizeBytes == 0 || priceQuoteSubHeader->PricingInstrumentSubHeaderBytesOffset == 0)
                throw new Exception(
                                    "Expected PriceQuoteFileSubHeader to have a value for SourceTickerQuoteSerializedSizeBytes and SourceTickerQuoteSubHeaderBytesOffset");

            messageBufferContext.EncodedBuffer!.ReadCursor = subHeaderFileOffset + priceQuoteSubHeader->PricingInstrumentSubHeaderBytesOffset;
            messageBufferContext.EncodedBuffer!.WriteCursor = subHeaderFileOffset + priceQuoteSubHeader->PricingInstrumentSubHeaderBytesOffset
                                                                                  + priceQuoteSubHeader->PricingInstrumentIdSerializedSizeBytes;

            cachedPricingInstrumentId = instrumentType == InstrumentType.Price
                ? sourceTickerInfoDeserializer.Deserialize(messageBufferContext)
                : pricingInstrumentDeserializer.Deserialize(messageBufferContext);
            return cachedPricingInstrumentId!;
        }
        set
        {
            if (Equals(cachedPricingInstrumentId, value) || (!writable &&
                                                             cachedPricingInstrumentId != null) || memoryMappedFileView == null)
                return;
            messageBufferContext.EncodedBuffer!.WriteCursor = subHeaderFileOffset + priceQuoteSubHeader->PricingInstrumentSubHeaderBytesOffset;

            int serializedSize;
            if (instrumentType == InstrumentType.Price)
            {
                sourceTickerInfoSerializer ??= new SourceTickerInfoSerializer()
                {
                    AddMessageHeader = false
                };
                serializedSize = sourceTickerInfoSerializer.Serialize(messageBufferContext.EncodedBuffer, (ISourceTickerInfo)value);
            }
            else
            {
                pricingInstrumentSerializer ??= new PricingInstrumentSerializer();
                serializedSize              =   pricingInstrumentSerializer.Serialize(messageBufferContext.EncodedBuffer, value);
            }
            priceQuoteSubHeader->PricingInstrumentIdSerializedSizeBytes = (ushort)serializedSize;

            cachedPricingInstrumentId = value;
        }
    }

    public PQSerializationFlags SerializationFlags
    {
        get
        {
            if (memoryMappedFileView != null) return cachePriceQuoteSubHeader.SerializationFlags;
            cachePriceQuoteSubHeader.SerializationFlags = priceQuoteSubHeader->SerializationFlags;
            return cachePriceQuoteSubHeader.SerializationFlags;
        }

        set
        {
            if (Equals(cachePriceQuoteSubHeader.SerializationFlags, value) || !writable || memoryMappedFileView == null) return;
            priceQuoteSubHeader->SerializationFlags     = value;
            cachePriceQuoteSubHeader.SerializationFlags = value;
        }
    }

    public void CloseFileView()
    {
        memoryMappedFileView = null;
        memoryMappedFileBuffer.ClearFileView();
    }

    public bool ReopenFileView(ShiftableMemoryMappedFileView headerMappedFileView, FileFlags fileFlags = FileFlags.None)
    {
        memoryMappedFileView = headerMappedFileView;
        memoryMappedFileBuffer.SetFileWriterAt(memoryMappedFileView, subHeaderFileOffset);
        priceQuoteSubHeader = (PriceQuoteSubHeader*)(memoryMappedFileView.LowerViewFileCursorOffset + subHeaderFileOffset);
        return true;
    }
}
