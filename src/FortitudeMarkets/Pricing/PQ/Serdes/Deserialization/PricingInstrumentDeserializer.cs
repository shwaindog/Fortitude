// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.TimeSeries;
using FortitudeMarkets.Configuration.ClientServerConfig;
using FortitudeMarkets.Pricing;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;

internal class PricingInstrumentDeserializer : IDeserializer<IPricingInstrumentId>
{
    private readonly IRecycler recycler;

    public PricingInstrumentDeserializer(IRecycler recycler) => this.recycler = recycler;

    public PricingInstrumentDeserializer(PricingInstrumentDeserializer toClone) => recycler = toClone.recycler;

    public MarshalType MarshalType => MarshalType.Binary;

    public unsafe IPricingInstrumentId Deserialize(ISerdeContext readContext)
    {
        if ((readContext.Direction & ContextDirection.Read) == 0) throw new ArgumentException("Expected readContext to allow reading");
        if ((readContext.MarshalType & MarshalType.Binary) == 0) throw new ArgumentException("Expected readContext to be a binary buffer context");
        if (readContext is IBufferContext messageBufferContext)
        {
            var pricingInstrument = recycler.Borrow<PricingInstrument>();

            using var fixedBuffer = messageBufferContext.EncodedBuffer!;

            fixedBuffer.LimitNextDeserialize = byte.MaxValue;

            var ptr         = fixedBuffer.ReadBuffer + fixedBuffer.BufferRelativeReadCursor;
            var messageSize = StreamByteOps.ToUShort(ref ptr);

            DeserializePricingInstrument(pricingInstrument, ref ptr, fixedBuffer);
            messageBufferContext.LastReadLength = messageSize;
            return pricingInstrument;
        }

        throw new ArgumentException("Expected readContext to be of type IMessageBufferContext");
    }

    public static unsafe void DeserializePricingInstrument(IPricingInstrumentId pricingInstrument, ref byte* ptr, IBuffer fixedBuffer)
    {
        pricingInstrument.SourceId = StreamByteOps.ToUShort(ref ptr);
        pricingInstrument.TickerId = StreamByteOps.ToUShort(ref ptr);
        pricingInstrument.Source   = StreamByteOps.ToStringWithAutoSizeHeader(ref ptr, Math.Min(fixedBuffer.UnreadBytesRemaining, 255))!;
        pricingInstrument.Ticker   = StreamByteOps.ToStringWithAutoSizeHeader(ref ptr, Math.Min(fixedBuffer.UnreadBytesRemaining, 255))!;

        pricingInstrument.MarketClassification = new MarketClassification(StreamByteOps.ToUInt(ref ptr));
        pricingInstrument.InstrumentType       = (InstrumentType)(*ptr++);
        pricingInstrument.CoveringPeriod       = new DiscreetTimePeriod((TimeBoundaryPeriod)StreamByteOps.ToUInt(ref ptr));
        var numberOfAttributes = *ptr++;
        for (var i = 0; i < numberOfAttributes; i++)
        {
            var key   = StreamByteOps.ToStringWithAutoSizeHeader(ref ptr, Math.Min(fixedBuffer.UnreadBytesRemaining, 255))!;
            var value = StreamByteOps.ToStringWithAutoSizeHeader(ref ptr, Math.Min(fixedBuffer.UnreadBytesRemaining, 255))!;
            pricingInstrument[key] = value;
        }
    }

    public IDeserializer<IPricingInstrumentId> Clone() => new PricingInstrumentDeserializer(this);
}
