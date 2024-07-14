// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

internal class SourceTickerQuoteInfoDeserializer : MessageDeserializer<ISourceTickerQuoteInfo>
{
    private readonly IRecycler recycler;

    public SourceTickerQuoteInfoDeserializer(IRecycler recycler) => this.recycler = recycler;

    public SourceTickerQuoteInfoDeserializer(SourceTickerQuoteInfoDeserializer toClone) : base(toClone) => recycler = toClone.recycler;

    public override unsafe ISourceTickerQuoteInfo? Deserialize(ISerdeContext readContext)
    {
        if ((readContext.Direction & ContextDirection.Read) == 0) throw new ArgumentException("Expected readContext to allow reading");
        if ((readContext.MarshalType & MarshalType.Binary) == 0) throw new ArgumentException("Expected readContext to be a binary buffer context");
        if (readContext is IMessageBufferContext messageBufferContext)
        {
            var       srcTkrQtInfo = recycler.Borrow<SourceTickerQuoteInfo>();
            using var fixedBuffer  = messageBufferContext.EncodedBuffer!;
            fixedBuffer.LimitNextDeserialize = byte.MaxValue;
            var ptr                                                   = fixedBuffer.ReadBuffer + fixedBuffer.BufferRelativeReadCursor;
            if (ReadMessageHeader) messageBufferContext.MessageHeader = ReadHeader(ref ptr);
            DeserializeSourceTickerQuoteInfo(srcTkrQtInfo, ref ptr, fixedBuffer);
            messageBufferContext.LastReadLength = (int)messageBufferContext.MessageHeader.MessageSize;
            OnNotify(srcTkrQtInfo, messageBufferContext);
            return srcTkrQtInfo;
        }

        throw new ArgumentException("Expected readContext to be of type IMessageBufferContext");
    }

    public static unsafe void DeserializeSourceTickerQuoteInfo(ISourceTickerQuoteInfo srcTkrQtInfo, ref byte* ptr, IBuffer fixedBuffer)
    {
        PricingInstrumentDeserializer.DeserializePricingInstrument(srcTkrQtInfo, ref ptr, fixedBuffer);
        DeserializeQuoteInfo(srcTkrQtInfo, ref ptr);
    }

    private static unsafe void DeserializeQuoteInfo(ISourceTickerQuoteInfo srcTkrQtInfo, ref byte* ptr)
    {
        srcTkrQtInfo.PublishedQuoteLevel    = (QuoteLevel)(*ptr++);
        srcTkrQtInfo.MaximumPublishedLayers = *ptr++;

        srcTkrQtInfo.RoundingPrecision = StreamByteOps.ToDecimal(ref ptr);
        srcTkrQtInfo.MinSubmitSize     = StreamByteOps.ToDecimal(ref ptr);
        srcTkrQtInfo.MaxSubmitSize     = StreamByteOps.ToDecimal(ref ptr);
        srcTkrQtInfo.IncrementSize     = StreamByteOps.ToDecimal(ref ptr);
        srcTkrQtInfo.MinimumQuoteLife  = StreamByteOps.ToUShort(ref ptr);
        srcTkrQtInfo.LayerFlags        = (LayerFlags)StreamByteOps.ToUInt(ref ptr);
        srcTkrQtInfo.LastTradedFlags   = (LastTradedFlags)StreamByteOps.ToUShort(ref ptr);
    }

    public override IMessageDeserializer Clone() => new SourceTickerQuoteInfoDeserializer(this);
}
