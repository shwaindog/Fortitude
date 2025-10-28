// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.DataStructures.MemoryPools.Buffers.ByteBuffers;
using FortitudeCommon.Extensions;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Candles;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using static FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Candles.PQStorageCandleFlags;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;

public interface IPQStorageCandleDeserializer : IDeserializer<ICandle>
{
    IPQStorageCandle DeserializedCandle { get; }
}

public class PQStorageCandleDeserializer : IPQStorageCandleDeserializer
{
    private const byte NextByteBitShift      = 7;
    private const byte Lowest7BitsInByteMask = 0x7F;
    private const byte HighestBitInByteMask  = 0x80;

    public PQStorageCandleDeserializer() => DeserializedCandle = new PQStorageCandle();

    public PQStorageCandleDeserializer
        (IPQPriceVolumePublicationPrecisionSettings precisionSettings) =>
        DeserializedCandle = new PQStorageCandle();

    public IPQStorageCandle DeserializedCandle { get; }

    public MarshalType MarshalType => MarshalType.Binary;

    public ICandle? Deserialize(ISerdeContext readContext)
    {
        if ((readContext.Direction & ContextDirection.Read) == 0) throw new ArgumentException("Expected readContext to allow reading");
        if ((readContext.MarshalType & MarshalType.Binary) == 0) throw new ArgumentException("Expected readContext to be a binary buffer context");
        if (readContext is IBufferContext bufferContext)
        {
            using var fixedBuffer = bufferContext.EncodedBuffer!;

            var bytesRead = Deserialize(fixedBuffer, DeserializedCandle);
            bufferContext.LastReadLength = bytesRead;
            if (bytesRead > 0)
            {
                fixedBuffer.ReadCursor += bytesRead;

                DeserializedCandle.HasUpdates = false;
                return DeserializedCandle;
            }
            return null;
        }

        throw new ArgumentException("Expected readContext to be of type IBufferContext");
    }

    private unsafe int Deserialize(IBuffer buffer, IPQStorageCandle ent)
    {
        using var fixedBuffer = buffer;

        var readStart   = fixedBuffer.ReadBuffer + buffer.BufferRelativeReadCursor;
        var ptr         = readStart;
        var flags       = (PQStorageCandleFlags)StreamByteOps.ToUInt(ref ptr);
        var priceScale  = (PQFieldFlags)(((uint)(flags & PriceScaleMask) >> 24) & 0x0F);
        var volumeScale = (PQFieldFlags)(((uint)(flags & VolumeScaleMask) >> 28) & 0x0F);

        var isSnapshot = flags.HasSnapshotFlag();
        ent.HasUpdates = false;
        if (isSnapshot)
        {
            ent.StartBidPrice   = 0m;
            ent.StartAskPrice   = 0m;
            ent.HighestBidPrice = 0m;
            ent.HighestAskPrice = 0m;
            ent.LowestBidPrice  = 0m;
            ent.LowestAskPrice  = 0m;
            ent.EndBidPrice     = 0m;
            ent.EndAskPrice     = 0m;
            ent.AverageBidPrice = 0m;
            ent.AverageAskPrice = 0m;
            ent.PeriodVolume    = 0;
            ent.TickCount       = 0;
            ent.PeriodEndTime   = DateTime.MinValue;

            ent.TimeBoundaryPeriod = (TimeBoundaryPeriod)StreamByteOps.ToUShort(ref ptr);
            ent.PeriodStartTime    = StreamByteOps.ToLong(ref ptr).CappedTicksToDateTime();
            ent.CandleFlags = (CandleFlags)StreamByteOps.ToUInt(ref ptr);
        }
        else
        {
            if (!flags.HasFlag(IsNextTimePeriod))
            {
                var numberOfPeriods                            = Deserialize7BitDeltaUint(ref ptr);
                var currentStartTime                           = ent.PeriodStartTime;
                while (numberOfPeriods-- > 0) currentStartTime = ent.TimeBoundaryPeriod.PeriodEnd(currentStartTime);
                ent.PeriodStartTime = currentStartTime;
            }
            else
            {
                ent.PeriodStartTime = ent.PeriodEndTime;
            }
            if (flags.HasFlag(HasSummaryFlagsChanges))
            {
                var existingSummaryFlagsUpperBytes = (uint)ent.CandleFlags >> 16;
                var deltaUpperBytes = Deserialize7BitDeltaUint(ref ptr);
                var newUpper = (flags.HasFlag(NegateDeltaSummaryFlagsUpperByte) ? -1 : 1) * deltaUpperBytes + existingSummaryFlagsUpperBytes;
                var existingSummaryFlagsLowerBytes = (ushort)ent.CandleFlags;
                var deltaLowerBytes = Deserialize7BitDeltaUint(ref ptr);
                var newLower = (flags.HasFlag(NegateDeltaSummaryFlagsLowerByte) ? -1 : 1) * deltaLowerBytes + existingSummaryFlagsLowerBytes;
                var currentFlags = (CandleFlags)((newUpper << 16) | newLower);
                ent.CandleFlags = currentFlags;
            }
        }
        if (isSnapshot)
        {
            ent.StartBidPrice = flags.SignMultiplier(NegateDeltaStartBidPrice)
                              * PQScaling.Unscale(Deserialize7BitDeltaUint(ref ptr), priceScale);
            ent.StartAskPrice = flags.SignMultiplier(NegateDeltaStartAskPrice)
                              * PQScaling.Unscale(Deserialize7BitDeltaUint(ref ptr), priceScale);
        }
        else if (!flags.HasFlag(PricesStartSameAsLastEndPrices))
        {
            ent.StartBidPrice = ent.EndBidPrice + flags.SignMultiplier(NegateDeltaStartBidPrice)
              * PQScaling.Unscale(Deserialize7BitDeltaUint(ref ptr), priceScale);
            ent.StartAskPrice = ent.EndAskPrice + flags.SignMultiplier(NegateDeltaStartAskPrice)
              * PQScaling.Unscale(Deserialize7BitDeltaUint(ref ptr), priceScale);
        }
        else
        {
            ent.StartBidPrice = ent.EndBidPrice;
            ent.StartAskPrice = ent.EndAskPrice;
        }
        ent.HighestBidPrice += flags.SignMultiplier(NegateDeltaHighestBidPrice)
                             * PQScaling.Unscale(Deserialize7BitDeltaUint(ref ptr), priceScale);
        ent.HighestAskPrice += flags.SignMultiplier(NegateDeltaHighestAskPrice)
                             * PQScaling.Unscale(Deserialize7BitDeltaUint(ref ptr), priceScale);
        ent.LowestBidPrice += flags.SignMultiplier(NegateDeltaLowestBidPrice)
                            * PQScaling.Unscale(Deserialize7BitDeltaUint(ref ptr), priceScale);
        ent.LowestAskPrice += flags.SignMultiplier(NegateDeltaLowestAskPrice)
                            * PQScaling.Unscale(Deserialize7BitDeltaUint(ref ptr), priceScale);
        ent.EndBidPrice += flags.SignMultiplier(NegateDeltaEndBidPrice)
                         * PQScaling.Unscale(Deserialize7BitDeltaUint(ref ptr), priceScale);
        ent.EndAskPrice += flags.SignMultiplier(NegateDeltaEndAskPrice)
                         * PQScaling.Unscale(Deserialize7BitDeltaUint(ref ptr), priceScale);
        ent.AverageBidPrice += flags.SignMultiplier(NegateDeltaAverageBidPrice)
                             * PQScaling.Unscale(Deserialize7BitDeltaUint(ref ptr), priceScale);
        ent.AverageAskPrice += flags.SignMultiplier(NegateDeltaAverageAskPrice)
                             * PQScaling.Unscale(Deserialize7BitDeltaUint(ref ptr), priceScale);
        ent.PeriodVolume += flags.SignMultiplier(NegateDeltaPeriodVolume)
                          * (long)PQScaling.Unscale(Deserialize7BitDeltaUint(ref ptr), volumeScale);
        ent.TickCount = (uint)(ent.TickCount + flags.SignMultiplier(NegateDeltaTickCount) * Deserialize7BitDeltaUint(ref ptr));

        var read = (int)(ptr - readStart);
        return read;
    }

    public unsafe uint Deserialize7BitDeltaUint(ref byte* ptr)
    {
        var currByte   = *ptr++;
        var isLastByte = (currByte & HighestBitInByteMask) > 0;
        var result     = (uint)(currByte & Lowest7BitsInByteMask);
        var count      = 0;
        var bitShift   = NextByteBitShift;
        while (!isLastByte && count < 4)
        {
            count++;
            currByte   = *ptr++;
            isLastByte = (currByte & HighestBitInByteMask) > 0;

            result   |= (uint)((currByte & Lowest7BitsInByteMask) << bitShift);
            bitShift += NextByteBitShift;
        }
        return result;
    }
}
