// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries;
using static FortitudeMarketsCore.Pricing.PQ.TimeSeries.PQPriceStorageSummaryFlags;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

public class PQPriceStoragePeriodSummarySerializer : ISerializer<IPQPriceStoragePeriodSummary>
{
    private const byte NextByteBitShift      = 7;
    private const byte Lowest7BitsInByteMask = 0x7F;
    private const byte HighestBitInByteMask  = 0x80;

    private readonly StorageFlags serializationFlags;

    // ReSharper disable once UnusedMember.Local
    private IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQPriceStoragePeriodSummarySerializer));

    public PQPriceStoragePeriodSummarySerializer(StorageFlags serializationFlags) => this.serializationFlags = serializationFlags;

    public MarshalType MarshalType => MarshalType.Binary;

    public void Serialize(IPQPriceStoragePeriodSummary priceSummaryPeriod, ISerdeContext writeContext)
    {
        if ((writeContext.Direction & ContextDirection.Write) == 0)
            throw new ArgumentException("Expected writeContext to support writing");
        if (writeContext is IBufferContext bufferContext)
        {
            var writeLength = Serialize(bufferContext.EncodedBuffer!, priceSummaryPeriod);

            if (writeLength > 0) bufferContext.EncodedBuffer!.WriteCursor += writeLength;
            bufferContext.LastWriteLength = writeLength;
        }
        else
        {
            throw new ArgumentException("Expected writeContext to be IBufferContext");
        }
    }

    public unsafe int Serialize(IBuffer buffer, IPQPriceStoragePeriodSummary pqPriceStoragePeriodSummary)
    {
        var resolvedFlags = serializationFlags;
        var publishAll = (resolvedFlags & StorageFlags.Complete) > 0
                      || pqPriceStoragePeriodSummary.SummaryStorageFlags.HasSnapshotFlag();

        using var fixedBuffer = buffer;

        var writeStart     = fixedBuffer.WriteBuffer + fixedBuffer.BufferRelativeWriteCursor;
        var ptr            = writeStart;
        var priceScale     = (byte)((uint)pqPriceStoragePeriodSummary.VolumePricePrecisionScale & 0x0F);
        var volumeScale    = (byte)(((uint)pqPriceStoragePeriodSummary.VolumePricePrecisionScale >> 4) & 0x0F);
        var serializeFlags = (PQPriceStorageSummaryFlags)((uint)pqPriceStoragePeriodSummary.SummaryStorageFlags & 0xFFFF);
        if (publishAll)
        {
            if (buffer.RemainingStorage < 74) return -1;
            serializeFlags = (PQPriceStorageSummaryFlags)(((ushort)serializeFlags & (uint)SnapshotClearMask)
                                                        | (uint)Snapshot);
            StreamByteOps.ToBytes(ref ptr, (ushort)serializeFlags);
            var serializeShort = (ushort)(((uint)pqPriceStoragePeriodSummary.SummaryStorageFlags >> 16) & 0xFFFF);
            StreamByteOps.ToBytes(ref ptr, serializeShort);
            serializeShort = (ushort)((ushort)pqPriceStoragePeriodSummary.SummaryPeriod & 0xFFFF);
            StreamByteOps.ToBytes(ref ptr, serializeShort);
            var summaryStartTimeTicks = pqPriceStoragePeriodSummary.SummaryStartTime.Ticks;
            StreamByteOps.ToBytes(ref ptr, summaryStartTimeTicks);
            pqPriceStoragePeriodSummary.DeltaEndBidPrice     = PQScaling.Scale(pqPriceStoragePeriodSummary.EndBidPrice, priceScale);
            pqPriceStoragePeriodSummary.DeltaEndAskPrice     = PQScaling.Scale(pqPriceStoragePeriodSummary.EndAskPrice, priceScale);
            pqPriceStoragePeriodSummary.DeltaStartBidPrice   = PQScaling.Scale(pqPriceStoragePeriodSummary.StartBidPrice, priceScale);
            pqPriceStoragePeriodSummary.DeltaStartAskPrice   = PQScaling.Scale(pqPriceStoragePeriodSummary.StartAskPrice, priceScale);
            pqPriceStoragePeriodSummary.DeltaHighestBidPrice = PQScaling.Scale(pqPriceStoragePeriodSummary.HighestBidPrice, priceScale);
            pqPriceStoragePeriodSummary.DeltaHighestAskPrice = PQScaling.Scale(pqPriceStoragePeriodSummary.HighestAskPrice, priceScale);
            pqPriceStoragePeriodSummary.DeltaLowestBidPrice  = PQScaling.Scale(pqPriceStoragePeriodSummary.LowestBidPrice, priceScale);
            pqPriceStoragePeriodSummary.DeltaLowestAskPrice  = PQScaling.Scale(pqPriceStoragePeriodSummary.LowestAskPrice, priceScale);
            pqPriceStoragePeriodSummary.DeltaEndBidPrice     = PQScaling.Scale(pqPriceStoragePeriodSummary.EndBidPrice, priceScale);
            pqPriceStoragePeriodSummary.DeltaEndAskPrice     = PQScaling.Scale(pqPriceStoragePeriodSummary.EndAskPrice, priceScale);
            pqPriceStoragePeriodSummary.DeltaAverageBidPrice = PQScaling.Scale(pqPriceStoragePeriodSummary.AverageBidPrice, priceScale);
            pqPriceStoragePeriodSummary.DeltaAverageAskPrice = PQScaling.Scale(pqPriceStoragePeriodSummary.AverageAskPrice, priceScale);
            pqPriceStoragePeriodSummary.DeltaPeriodVolume    = PQScaling.Scale(pqPriceStoragePeriodSummary.PeriodVolume, volumeScale);
            pqPriceStoragePeriodSummary.DeltaTickCount       = pqPriceStoragePeriodSummary.TickCount;
        }
        else
        {
            if (buffer.RemainingStorage < 28) return -1;
            StreamByteOps.ToBytes(ref ptr, (ushort)serializeFlags);
            if (!serializeFlags.HasFlag(IsNextTimePeriod)) Serialize7BitDeltaUint(ref ptr, pqPriceStoragePeriodSummary.DeltaPeriodsFromPrevious);
        }
        if (!serializeFlags.HasFlag(PricesStartSameAsLastEndPrices))
        {
            Serialize7BitDeltaUint(ref ptr, pqPriceStoragePeriodSummary.DeltaStartBidPrice);
            Serialize7BitDeltaUint(ref ptr, pqPriceStoragePeriodSummary.DeltaStartAskPrice);
        }
        Serialize7BitDeltaUint(ref ptr, pqPriceStoragePeriodSummary.DeltaHighestBidPrice);
        Serialize7BitDeltaUint(ref ptr, pqPriceStoragePeriodSummary.DeltaHighestAskPrice);
        Serialize7BitDeltaUint(ref ptr, pqPriceStoragePeriodSummary.DeltaLowestBidPrice);
        Serialize7BitDeltaUint(ref ptr, pqPriceStoragePeriodSummary.DeltaLowestAskPrice);
        Serialize7BitDeltaUint(ref ptr, pqPriceStoragePeriodSummary.DeltaEndBidPrice);
        Serialize7BitDeltaUint(ref ptr, pqPriceStoragePeriodSummary.DeltaEndAskPrice);
        Serialize7BitDeltaUint(ref ptr, pqPriceStoragePeriodSummary.DeltaAverageBidPrice);
        Serialize7BitDeltaUint(ref ptr, pqPriceStoragePeriodSummary.DeltaAverageAskPrice);
        Serialize7BitDeltaUint(ref ptr, pqPriceStoragePeriodSummary.DeltaPeriodVolume);
        Serialize7BitDeltaUint(ref ptr, pqPriceStoragePeriodSummary.DeltaTickCount);

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
