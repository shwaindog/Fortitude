// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeMarkets.Pricing.PQ.Summaries;
using static FortitudeMarkets.Pricing.PQ.Summaries.PQPriceStorageSummaryFlags;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

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

    public void Serialize(IPQPriceStoragePeriodSummary psps, ISerdeContext writeContext)
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

    public unsafe int Serialize(IBuffer buffer, IPQPriceStoragePeriodSummary pqPriceStoragePeriodSummary)
    {
        var resolvedFlags = serializationFlags;
        var publishAll = (resolvedFlags & StorageFlags.Complete) > 0
                      || pqPriceStoragePeriodSummary.SummaryStorageFlags.HasSnapshotFlag();

        using var fixedBuffer = buffer;

        var writeStart = fixedBuffer.WriteBuffer + fixedBuffer.BufferRelativeWriteCursor;
        var ptr        = writeStart;

        PQPriceStorageSummaryFlags serializeFlags;
        if (publishAll)
        {
            if (buffer.RemainingStorage < 74) return -1;

            pqPriceStoragePeriodSummary.CalculateAllFromNewDeltas();
            serializeFlags = pqPriceStoragePeriodSummary.SummaryStorageFlags;

            var priceScale   = (byte)((uint)pqPriceStoragePeriodSummary.VolumePricePrecisionScale & 0x0F);
            var volumeScale  = (byte)(((uint)pqPriceStoragePeriodSummary.VolumePricePrecisionScale >> 4) & 0x0F);
            var shiftedScale = (uint)((volumeScale << 4) | priceScale) << 24;
            serializeFlags &= ~PriceVolumeScaleMask;
            serializeFlags |= (PQPriceStorageSummaryFlags)((uint)Snapshot | shiftedScale);
            StreamByteOps.ToBytes(ref ptr, (uint)serializeFlags);
            var serializeShort = (ushort)((ushort)pqPriceStoragePeriodSummary.TimeBoundaryPeriod & 0xFFFF);
            StreamByteOps.ToBytes(ref ptr, serializeShort);
            var summaryStartTimeTicks = pqPriceStoragePeriodSummary.PeriodStartTime.Ticks;
            StreamByteOps.ToBytes(ref ptr, summaryStartTimeTicks);
            var summaryFlags = (uint)pqPriceStoragePeriodSummary.PeriodSummaryFlags;
            StreamByteOps.ToBytes(ref ptr, summaryFlags);
        }
        else
        {
            pqPriceStoragePeriodSummary.CalculatedScaledDeltas();
            serializeFlags = pqPriceStoragePeriodSummary.SummaryStorageFlags;
            if (buffer.RemainingStorage < 28) return -1;
            StreamByteOps.ToBytes(ref ptr, (uint)serializeFlags);
            if (!serializeFlags.HasFlag(IsNextTimePeriod)) Serialize7BitDeltaUint(ref ptr, pqPriceStoragePeriodSummary.DeltaPeriodsFromPrevious);
            if (serializeFlags.HasFlag(HasSummaryFlagsChanges))
            {
                Serialize7BitDeltaUint(ref ptr, pqPriceStoragePeriodSummary.DeltaSummaryFlagsUpperBytes);
                Serialize7BitDeltaUint(ref ptr, pqPriceStoragePeriodSummary.DeltaSummaryFlagsLowerBytes);
            }
        }
        if (!serializeFlags.HasFlag(PricesStartSameAsLastEndPrices) || publishAll)
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
