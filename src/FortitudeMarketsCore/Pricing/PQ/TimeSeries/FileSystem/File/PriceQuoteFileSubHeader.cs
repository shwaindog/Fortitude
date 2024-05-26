// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.OSWrapper.Memory;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.TimeSeries.FileSystem.File;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries.FileSystem.File;

public struct PriceQuoteSubHeader
{
    public PQSerializationFlags SerializationFlags;
    public ushort SourceTickerQuoteSerializedSizeBytes;
    public ushort SourceTickerQuoteSubHeaderBytesOffset;
}

public unsafe class PriceQuoteFileSubHeader<TEntry> : IMutablePriceQuoteFileHeader<TEntry> where TEntry : class, ILevel0Quote, IVersionedMessage
{
    private static IRecycler recycler = new Recycler();
    private readonly MemoryMappedFileBuffer memoryMappedFileBuffer;
    private readonly IMessageBufferContext messageBufferContext;
    private readonly SourceTickerQuoteInfoDeserializer sourceTickerQuoteInfoDeserializer = new(recycler);
    private readonly ushort subHeaderFileOffset;
    private readonly bool writable;
    private ISourceTickerQuoteInfo? cacheSourceTickerQuoteInfo;
    private IMessageSerializer<PQLevel0Quote>? indexEntrySerializer;
    private ShiftableMemoryMappedFileView? memoryMappedFileView;
    private IMessageDeserializer? messageDeserializer;
    private PriceQuoteSubHeader* priceQuoteSubHeader;
    private IMessageSerializer<PQLevel0Quote>? repeatedEntrySerializer;
    private SourceTickerQuoteInfoSerializer? sourceTickerQuoteInfoSerializer;

    public PriceQuoteFileSubHeader(ShiftableMemoryMappedFileView headerMappedFileView, ushort subHeaderFileOffset, bool writable)
    {
        memoryMappedFileView = headerMappedFileView;
        this.subHeaderFileOffset = subHeaderFileOffset;
        this.writable = writable;
        priceQuoteSubHeader = (PriceQuoteSubHeader*)(memoryMappedFileView.LowerViewFileCursorOffset + subHeaderFileOffset);
        memoryMappedFileBuffer = new MemoryMappedFileBuffer(memoryMappedFileView, false);
        messageBufferContext = new MessageBufferContext(memoryMappedFileBuffer);
        priceQuoteSubHeader->SourceTickerQuoteSubHeaderBytesOffset = 100;
    }

    public ISourceTickerQuoteInfo SourceTickerQuoteInfo
    {
        get
        {
            if (cacheSourceTickerQuoteInfo != null) return cacheSourceTickerQuoteInfo;
            if (priceQuoteSubHeader->SourceTickerQuoteSerializedSizeBytes == 0 || priceQuoteSubHeader->SourceTickerQuoteSubHeaderBytesOffset > 0)
                throw new Exception(
                    "Expected PriceQuoteFileSubHeader to have a value for SourceTickerQuoteSerializedSizeBytes and SourceTickerQuoteSubHeaderBytesOffset");

            messageBufferContext.EncodedBuffer!.ReadCursor = subHeaderFileOffset + priceQuoteSubHeader->SourceTickerQuoteSubHeaderBytesOffset;
            messageBufferContext.EncodedBuffer!.WriteCursor = subHeaderFileOffset + priceQuoteSubHeader->SourceTickerQuoteSubHeaderBytesOffset
                                                                                  + priceQuoteSubHeader->SourceTickerQuoteSerializedSizeBytes;
            cacheSourceTickerQuoteInfo = sourceTickerQuoteInfoDeserializer.Deserialize(messageBufferContext);
            return cacheSourceTickerQuoteInfo!;
        }
        set
        {
            if (Equals(cacheSourceTickerQuoteInfo, value) || !writable || memoryMappedFileView == null) return;
            messageBufferContext.EncodedBuffer!.WriteCursor = subHeaderFileOffset + priceQuoteSubHeader->SourceTickerQuoteSubHeaderBytesOffset;
            messageBufferContext.EncodedBuffer.LimitNextSerialize = byte.MaxValue;
            sourceTickerQuoteInfoSerializer ??= new SourceTickerQuoteInfoSerializer();
            var serializedSize = sourceTickerQuoteInfoSerializer.Serialize(messageBufferContext.EncodedBuffer, value);
            priceQuoteSubHeader->SourceTickerQuoteSerializedSizeBytes = (ushort)serializedSize;
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

    IMessageDeserializer IPriceQuoteFileHeader.DefaultMessageDeserializer => DefaultMessageDeserializer;
    IMessageSerializer IPriceQuoteFileHeader.IndexEntryMessageSerializer => IndexEntryMessageSerializer;
    IMessageSerializer IPriceQuoteFileHeader.RepeatedEntryMessageSerializer => RepeatedEntryMessageSerializer;

    public IMessageDeserializer<TEntry> DefaultMessageDeserializer
    {
        get
        {
            if (messageDeserializer == null)
            {
                var srcTkrQtInfo = SourceTickerQuoteInfo;
                switch (srcTkrQtInfo.PublishedQuoteLevel)
                {
                    case QuoteLevel.Level0:
                        messageDeserializer = new PQQuoteStorageDeserializer<PQLevel0Quote>(srcTkrQtInfo);
                        break;
                    case QuoteLevel.Level1:
                        messageDeserializer = new PQQuoteStorageDeserializer<PQLevel1Quote>(srcTkrQtInfo);
                        break;
                    case QuoteLevel.Level2:
                        messageDeserializer = new PQQuoteStorageDeserializer<PQLevel2Quote>(srcTkrQtInfo);
                        break;
                    default:
                        messageDeserializer = new PQQuoteStorageDeserializer<PQLevel3Quote>(srcTkrQtInfo);
                        break;
                }
            }

            return (IMessageDeserializer<TEntry>)messageDeserializer;
        }
    }

    public IMessageSerializer<TEntry> IndexEntryMessageSerializer =>
        (IMessageSerializer<TEntry>)(indexEntrySerializer ??= new PQQuoteSerializer(PQMessageFlags.Complete, PQSerializationFlags.ForStorage));

    public IMessageSerializer<TEntry> RepeatedEntryMessageSerializer =>
        (IMessageSerializer<TEntry>)(repeatedEntrySerializer ??= new PQQuoteSerializer(PQMessageFlags.Update, PQSerializationFlags.ForStorage));
}
