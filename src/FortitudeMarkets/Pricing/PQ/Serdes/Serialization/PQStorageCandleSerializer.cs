// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Candles;
using static FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Candles.PQStorageCandleFlags;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

public class PQStorageCandleSerializer : ISerializer<IPQStorageCandle>
{
    private const byte NextByteBitShift      = 7;
    private const byte Lowest7BitsInByteMask = 0x7F;
    private const byte HighestBitInByteMask  = 0x80;

    private readonly StorageFlags serializationFlags;

    // ReSharper disable once UnusedMember.Local
    private IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQStorageCandleSerializer));

    public PQStorageCandleSerializer(StorageFlags serializationFlags) => this.serializationFlags = serializationFlags;

    public MarshalType MarshalType => MarshalType.Binary;

    public void Serialize(IPQStorageCandle psps, ISerdeContext writeContext)
    {
        if ((writeContext.Direction & ContextDirection.Write) == 0) throw new ArgumentException("Expected writeContext to support writing");
        if (writeContext is IBufferContext bufferContext)
        {
            var writeLength = Serialize(bufferContext.EncodedBuffer!, psps);

            if (writeLength > 0) bufferContext.EncodedBuffer!.WriteCursor += writeLength;
            bufferContext.LastWriteLength = writeLength;

            psps.HasUpdates = false;
        }
        else
        {
            throw new ArgumentException("Expected writeContext to be IBufferContext");
        }
    }

    public unsafe int Serialize(IBuffer buffer, IPQStorageCandle pqStorageCandle)
    {
        var resolvedFlags = serializationFlags;
        var publishAll = (resolvedFlags & StorageFlags.Complete) > 0
                      || pqStorageCandle.CandleStorageFlags.HasSnapshotFlag();

        using var fixedBuffer = buffer;

        var writeStart = fixedBuffer.WriteBuffer + fixedBuffer.BufferRelativeWriteCursor;
        var ptr        = writeStart;

        PQStorageCandleFlags serializeFlags;
        if (publishAll)
        {
            if (buffer.RemainingStorage < 74) return -1;

            pqStorageCandle.CalculateAllFromNewDeltas();
            serializeFlags = pqStorageCandle.CandleStorageFlags;

            var priceScale   = (byte)((uint)pqStorageCandle.VolumePricePrecisionScale & 0x0F);
            var volumeScale  = (byte)(((uint)pqStorageCandle.VolumePricePrecisionScale >> 4) & 0x0F);
            var shiftedScale = (uint)((volumeScale << 4) | priceScale) << 24;
            serializeFlags &= ~PriceVolumeScaleMask;
            serializeFlags |= (PQStorageCandleFlags)((uint)Snapshot | shiftedScale);
            StreamByteOps.ToBytes(ref ptr, (uint)serializeFlags);
            var serializeShort = (ushort)((ushort)pqStorageCandle.TimeBoundaryPeriod & 0xFFFF);
            StreamByteOps.ToBytes(ref ptr, serializeShort);
            var summaryStartTimeTicks = pqStorageCandle.PeriodStartTime.Ticks;
            StreamByteOps.ToBytes(ref ptr, summaryStartTimeTicks);
            var summaryFlags = (uint)pqStorageCandle.CandleFlags;
            StreamByteOps.ToBytes(ref ptr, summaryFlags);
        }
        else
        {
            pqStorageCandle.CalculatedScaledDeltas();
            serializeFlags = pqStorageCandle.CandleStorageFlags;
            if (buffer.RemainingStorage < 28) return -1;
            StreamByteOps.ToBytes(ref ptr, (uint)serializeFlags);
            if (!serializeFlags.HasFlag(IsNextTimePeriod)) Serialize7BitDeltaUint(ref ptr, pqStorageCandle.DeltaPeriodsFromPrevious);
            if (serializeFlags.HasFlag(HasSummaryFlagsChanges))
            {
                Serialize7BitDeltaUint(ref ptr, pqStorageCandle.DeltaCandleFlagsUpperBytes);
                Serialize7BitDeltaUint(ref ptr, pqStorageCandle.DeltaCandleFlagsLowerBytes);
            }
        }
        if (!serializeFlags.HasFlag(PricesStartSameAsLastEndPrices) || publishAll)
        {
            Serialize7BitDeltaUint(ref ptr, pqStorageCandle.DeltaStartBidPrice);
            Serialize7BitDeltaUint(ref ptr, pqStorageCandle.DeltaStartAskPrice);
        }
        Serialize7BitDeltaUint(ref ptr, pqStorageCandle.DeltaHighestBidPrice);
        Serialize7BitDeltaUint(ref ptr, pqStorageCandle.DeltaHighestAskPrice);
        Serialize7BitDeltaUint(ref ptr, pqStorageCandle.DeltaLowestBidPrice);
        Serialize7BitDeltaUint(ref ptr, pqStorageCandle.DeltaLowestAskPrice);
        Serialize7BitDeltaUint(ref ptr, pqStorageCandle.DeltaEndBidPrice);
        Serialize7BitDeltaUint(ref ptr, pqStorageCandle.DeltaEndAskPrice);
        Serialize7BitDeltaUint(ref ptr, pqStorageCandle.DeltaAverageBidPrice);
        Serialize7BitDeltaUint(ref ptr, pqStorageCandle.DeltaAverageAskPrice);
        Serialize7BitDeltaUint(ref ptr, pqStorageCandle.DeltaPeriodVolume);
        Serialize7BitDeltaUint(ref ptr, pqStorageCandle.DeltaTickCount);

        var written = (int)(ptr - writeStart);
        return written;
    }

    public unsafe void Serialize7BitDeltaUint(ref byte* ptr, uint field)
    {
        var remainingBits = field;
        var isLastByte    = false;
        while (!isLastByte)
        {
            var curr7Bits = (byte)(remainingBits & Lowest7BitsInByteMask);
            isLastByte = (remainingBits & 0xFFFF_FF80) == 0;
            var serializeByte = (byte)(curr7Bits | (isLastByte ? HighestBitInByteMask : 0));
            *ptr++        =   serializeByte;
            remainingBits >>= NextByteBitShift;
        }
    }
}
