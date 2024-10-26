// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.TickerInfo;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;

internal class SourceTickerInfoDeserializer : MessageDeserializer<ISourceTickerInfo>
{
    private readonly IRecycler recycler;

    public SourceTickerInfoDeserializer(IRecycler recycler) => this.recycler = recycler;

    public SourceTickerInfoDeserializer(SourceTickerInfoDeserializer toClone) : base(toClone) => recycler = toClone.recycler;

    public override unsafe ISourceTickerInfo? Deserialize(ISerdeContext readContext)
    {
        if ((readContext.Direction & ContextDirection.Read) == 0) throw new ArgumentException("Expected readContext to allow reading");
        if ((readContext.MarshalType & MarshalType.Binary) == 0) throw new ArgumentException("Expected readContext to be a binary buffer context");
        if (readContext is IMessageBufferContext messageBufferContext)
        {
            var       srcTkrInfo  = recycler.Borrow<SourceTickerInfo>();
            using var fixedBuffer = messageBufferContext.EncodedBuffer!;
            fixedBuffer.LimitNextDeserialize = byte.MaxValue;
            var ptr                                                   = fixedBuffer.ReadBuffer + fixedBuffer.BufferRelativeReadCursor;
            if (ReadMessageHeader) messageBufferContext.MessageHeader = ReadHeader(ref ptr);
            DeserializeSourceTickerInfo(srcTkrInfo, ref ptr, fixedBuffer);
            messageBufferContext.LastReadLength = (int)messageBufferContext.MessageHeader.MessageSize;
            OnNotify(srcTkrInfo, messageBufferContext);
            return srcTkrInfo;
        }

        throw new ArgumentException("Expected readContext to be of type IMessageBufferContext");
    }

    public static unsafe void DeserializeSourceTickerInfo(ISourceTickerInfo srcTkrInfo, ref byte* ptr, IBuffer fixedBuffer)
    {
        PricingInstrumentDeserializer.DeserializePricingInstrument(srcTkrInfo, ref ptr, fixedBuffer);
        DeserializeQuoteInfo(srcTkrInfo, ref ptr);
    }

    private static unsafe void DeserializeQuoteInfo(ISourceTickerInfo srcTkrInfo, ref byte* ptr)
    {
        srcTkrInfo.PublishedTickerDetailLevel = (TickerDetailLevel)(*ptr++);
        srcTkrInfo.MaximumPublishedLayers     = *ptr++;

        srcTkrInfo.RoundingPrecision = StreamByteOps.ToDecimal(ref ptr);
        srcTkrInfo.Pip               = StreamByteOps.ToDecimal(ref ptr);
        var booleanFlags = (SourceTickerInfoBooleanFlags)StreamByteOps.ToUInt(ref ptr);
        srcTkrInfo.SubscribeToPrices = booleanFlags.HasSubscribeToPricesFlag();
        srcTkrInfo.TradingEnabled    = booleanFlags.HasTradingEnabledFlag();
        srcTkrInfo.MinSubmitSize     = StreamByteOps.ToDecimal(ref ptr);
        srcTkrInfo.MaxSubmitSize     = StreamByteOps.ToDecimal(ref ptr);
        srcTkrInfo.IncrementSize     = StreamByteOps.ToDecimal(ref ptr);
        srcTkrInfo.MinimumQuoteLife  = StreamByteOps.ToUShort(ref ptr);
        srcTkrInfo.DefaultMaxValidMs = StreamByteOps.ToUInt(ref ptr);
        srcTkrInfo.LayerFlags        = (LayerFlags)StreamByteOps.ToUInt(ref ptr);
        srcTkrInfo.LastTradedFlags   = (LastTradedFlags)StreamByteOps.ToUShort(ref ptr);
    }

    public override IMessageDeserializer Clone() => new SourceTickerInfoDeserializer(this);
}
