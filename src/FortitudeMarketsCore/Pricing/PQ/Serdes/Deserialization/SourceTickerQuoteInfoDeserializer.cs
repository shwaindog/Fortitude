// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

internal class SourceTickerQuoteInfoDeserializer : MessageDeserializer<ISourceTickerQuoteInfo>
{
    private readonly IRecycler recycler;

    public SourceTickerQuoteInfoDeserializer(IRecycler recycler) => this.recycler = recycler;

    public SourceTickerQuoteInfoDeserializer(SourceTickerQuoteInfoDeserializer toClone) : base(toClone) => recycler = toClone.recycler;

    public override unsafe ISourceTickerQuoteInfo? Deserialize(ISerdeContext readContext)
    {
        if ((readContext.Direction & ContextDirection.Read) == 0)
            throw new ArgumentException("Expected readContext to allow reading");
        if ((readContext.MarshalType & MarshalType.Binary) == 0)
            throw new ArgumentException("Expected readContext to be a binary buffer context");
        if (readContext is IMessageBufferContext messageBufferContext)
        {
            var deserializedSourceTickerQuoteInfo = recycler.Borrow<SourceTickerQuoteInfo>();
            using var fixedBuffer = messageBufferContext.EncodedBuffer!;
            fixedBuffer.LimitNextDeserialize = byte.MaxValue;
            var currPtr = fixedBuffer.ReadBuffer + fixedBuffer.BufferRelativeReadCursor;
            if (ReadMessageHeader) messageBufferContext.MessageHeader = ReadHeader(ref currPtr);
            deserializedSourceTickerQuoteInfo.SourceId = StreamByteOps.ToUShort(ref currPtr);
            deserializedSourceTickerQuoteInfo.TickerId = StreamByteOps.ToUShort(ref currPtr);
            deserializedSourceTickerQuoteInfo.PublishedQuoteLevel = (QuoteLevel)(*currPtr++);
            deserializedSourceTickerQuoteInfo.RoundingPrecision = StreamByteOps.ToDecimal(ref currPtr);
            deserializedSourceTickerQuoteInfo.MinSubmitSize = StreamByteOps.ToDecimal(ref currPtr);
            deserializedSourceTickerQuoteInfo.MaxSubmitSize = StreamByteOps.ToDecimal(ref currPtr);
            deserializedSourceTickerQuoteInfo.IncrementSize = StreamByteOps.ToDecimal(ref currPtr);
            deserializedSourceTickerQuoteInfo.MinimumQuoteLife = StreamByteOps.ToUShort(ref currPtr);
            deserializedSourceTickerQuoteInfo.LayerFlags = (LayerFlags)StreamByteOps.ToUInt(ref currPtr);
            deserializedSourceTickerQuoteInfo.MaximumPublishedLayers = *currPtr++;
            deserializedSourceTickerQuoteInfo.LastTradedFlags = (LastTradedFlags)StreamByteOps.ToUShort(ref currPtr);
            deserializedSourceTickerQuoteInfo.Source = StreamByteOps.ToStringWithAutoSizeHeader(ref currPtr, fixedBuffer.UnreadBytesRemaining)!;
            deserializedSourceTickerQuoteInfo.Ticker = StreamByteOps.ToStringWithAutoSizeHeader(ref currPtr, fixedBuffer.UnreadBytesRemaining)!;

            messageBufferContext.LastReadLength = (int)messageBufferContext.MessageHeader.MessageSize;
            OnNotify(deserializedSourceTickerQuoteInfo, messageBufferContext);
            return deserializedSourceTickerQuoteInfo;
        }

        throw new ArgumentException("Expected readContext to be of type IMessageBufferContext");
    }

    public override IMessageDeserializer Clone() => new SourceTickerQuoteInfoDeserializer(this);
}
