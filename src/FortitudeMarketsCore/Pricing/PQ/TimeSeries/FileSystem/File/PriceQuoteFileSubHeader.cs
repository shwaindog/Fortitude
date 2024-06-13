// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.TimeSeries.FileSystem.File;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;

public interface ISerializationPriceQuoteFileHeader : IPriceQuoteFileHeader
{
    PQSerializationFlags SerializationFlags { get; set; }
}

public struct PriceQuoteSubHeader
{
    public PQSerializationFlags SerializationFlags;

    public ushort SourceTickerQuoteSerializedSizeBytes;
    public ushort SourceTickerQuoteSubHeaderBytesOffset;
}

public unsafe class PriceQuoteFileSubHeader : ISerializationPriceQuoteFileHeader
{
    private static IRecycler recycler = new Recycler();

    private readonly MemoryMappedFileBuffer memoryMappedFileBuffer;
    private readonly IMessageBufferContext  messageBufferContext;
    private readonly SourceTickerQuoteInfoDeserializer sourceTickerQuoteInfoDeserializer = new(recycler)
    {
        ReadMessageHeader = false
    };
    private readonly ushort subHeaderFileOffset;
    private readonly bool   writable;

    private ISourceTickerQuoteInfo?          cachedSourceTickerQuoteInfo;
    private PriceQuoteSubHeader              cachePriceQuoteSubHeader;
    private ShiftableMemoryMappedFileView?   memoryMappedFileView;
    private PriceQuoteSubHeader*             priceQuoteSubHeader;
    private SourceTickerQuoteInfoSerializer? sourceTickerQuoteInfoSerializer;

    public PriceQuoteFileSubHeader(ShiftableMemoryMappedFileView headerMappedFileView, ushort subHeaderFileOffset, bool writable)
    {
        memoryMappedFileView     = headerMappedFileView;
        this.subHeaderFileOffset = subHeaderFileOffset;
        this.writable            = writable;
        priceQuoteSubHeader      = (PriceQuoteSubHeader*)(memoryMappedFileView.StartAddress + subHeaderFileOffset);
        memoryMappedFileBuffer   = new MemoryMappedFileBuffer(memoryMappedFileView, false);
        messageBufferContext     = new MessageBufferContext(memoryMappedFileBuffer);

        priceQuoteSubHeader->SourceTickerQuoteSubHeaderBytesOffset = 100;
    }

    public PriceQuoteFileSubHeader(PriceQuoteTimeSeriesFileParameters priceQuoteTimeSeriesFileParameters,
        ShiftableMemoryMappedFileView headerMappedFileView, ushort subHeaderFileOffset, bool writable)
        : this(headerMappedFileView, subHeaderFileOffset, writable)
    {
        SourceTickerQuoteInfo = priceQuoteTimeSeriesFileParameters.SourceTickerQuoteInfo;
        SerializationFlags    = priceQuoteTimeSeriesFileParameters.SerializationFlags;
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


    public ISourceTickerQuoteInfo SourceTickerQuoteInfo
    {
        get
        {
            if (memoryMappedFileView == null && cachedSourceTickerQuoteInfo != null) return cachedSourceTickerQuoteInfo;
            if (priceQuoteSubHeader->SourceTickerQuoteSerializedSizeBytes == 0 || priceQuoteSubHeader->SourceTickerQuoteSubHeaderBytesOffset == 0)
                throw new Exception(
                                    "Expected PriceQuoteFileSubHeader to have a value for SourceTickerQuoteSerializedSizeBytes and SourceTickerQuoteSubHeaderBytesOffset");

            messageBufferContext.EncodedBuffer!.ReadCursor = subHeaderFileOffset + priceQuoteSubHeader->SourceTickerQuoteSubHeaderBytesOffset;
            messageBufferContext.EncodedBuffer!.WriteCursor = subHeaderFileOffset + priceQuoteSubHeader->SourceTickerQuoteSubHeaderBytesOffset
                                                                                  + priceQuoteSubHeader->SourceTickerQuoteSerializedSizeBytes;

            cachedSourceTickerQuoteInfo = sourceTickerQuoteInfoDeserializer.Deserialize(messageBufferContext);
            return cachedSourceTickerQuoteInfo!;
        }
        set
        {
            if (Equals(cachedSourceTickerQuoteInfo, value) || (!writable &&
                                                               cachedSourceTickerQuoteInfo != null) || memoryMappedFileView == null) return;
            messageBufferContext.EncodedBuffer!.WriteCursor = subHeaderFileOffset + priceQuoteSubHeader->SourceTickerQuoteSubHeaderBytesOffset;

            sourceTickerQuoteInfoSerializer ??= new SourceTickerQuoteInfoSerializer
            {
                AddMessageHeader = false
            };
            var serializedSize = sourceTickerQuoteInfoSerializer.Serialize(messageBufferContext.EncodedBuffer, value);
            priceQuoteSubHeader->SourceTickerQuoteSerializedSizeBytes = (ushort)serializedSize;
            cachedSourceTickerQuoteInfo                               = value;
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
