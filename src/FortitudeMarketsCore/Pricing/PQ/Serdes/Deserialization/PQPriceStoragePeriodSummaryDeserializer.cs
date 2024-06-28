// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing.Summaries;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Summaries;
using static FortitudeMarketsCore.Pricing.PQ.Summaries.PQPriceStorageSummaryFlags;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

public interface IPQPriceStoragePeriodSummaryDeserializer : IDeserializer<IPricePeriodSummary>
{
    IPQPriceStoragePeriodSummary DeserializedPriceSummary { get; }
}

public class PQPriceStoragePeriodSummaryDeserializer : IPQPriceStoragePeriodSummaryDeserializer
{
    private const byte NextByteBitShift      = 7;
    private const byte Lowest7BitsInByteMask = 0x7F;
    private const byte HighestBitInByteMask  = 0x80;

    public PQPriceStoragePeriodSummaryDeserializer() => DeserializedPriceSummary = new PQPriceStoragePeriodSummary();

    public PQPriceStoragePeriodSummaryDeserializer
        (IPQPriceVolumePublicationPrecisionSettings precisionSettings) =>
        DeserializedPriceSummary = new PQPriceStoragePeriodSummary(precisionSettings);

    public IPQPriceStoragePeriodSummary DeserializedPriceSummary { get; }

    public MarshalType MarshalType => MarshalType.Binary;

    public IPricePeriodSummary? Deserialize(ISerdeContext readContext)
    {
        if ((readContext.Direction & ContextDirection.Read) == 0)
            throw new ArgumentException("Expected readContext to allow reading");
        if ((readContext.MarshalType & MarshalType.Binary) == 0)
            throw new ArgumentException("Expected readContext to be a binary buffer context");
        if (readContext is IBufferContext bufferContext)
        {
            using var fixedBuffer = bufferContext.EncodedBuffer!;

            var bytesRead = Deserialize(fixedBuffer, DeserializedPriceSummary);
            bufferContext.LastReadLength = bytesRead;
            if (bytesRead > 0)
            {
                fixedBuffer.ReadCursor += bytesRead;
                return DeserializedPriceSummary;
            }
            return null;
        }

        throw new ArgumentException("Expected readContext to be of type IBufferContext");
    }

    private unsafe int Deserialize(IBuffer buffer, IPQPriceStoragePeriodSummary ent)
    {
        using var fixedBuffer = buffer;

        var readStart        = fixedBuffer.ReadBuffer + buffer.BufferRelativeReadCursor;
        var ptr              = readStart;
        var flags            = (PQPriceStorageSummaryFlags)StreamByteOps.ToUInt(ref ptr);
        var priceScale       = (byte)(((uint)(flags & PriceScaleMask) >> 24) & 0x0F);
        var volumeScale      = (byte)(((uint)(flags & VolumeScaleMask) >> 28) & 0x0F);
        var precisionSetting = ent.PrecisionSettings;
        if (precisionSetting == null || (precisionSetting.PriceScalingPrecision & 0x0F) != priceScale
                                     || (precisionSetting.VolumeScalingPrecision & 0x0F) != volumeScale)
            ent.PrecisionSettings = new PQPriceVolumePublicationPrecisionSettings(priceScale, volumeScale);

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
            ent.PeriodEndTime   = DateTimeConstants.UnixEpoch;

            ent.TimeSeriesPeriod   = (TimeSeriesPeriod)StreamByteOps.ToUShort(ref ptr);
            ent.PeriodStartTime    = StreamByteOps.ToLong(ref ptr).CappedTicksToDateTime();
            ent.PeriodSummaryFlags = (PricePeriodSummaryFlags)StreamByteOps.ToUInt(ref ptr);
        }
        else
        {
            if (!flags.HasFlag(IsNextTimePeriod))
            {
                var numberOfPeriods                            = Deserialize7BitDeltaUint(ref ptr);
                var currentStartTime                           = ent.PeriodStartTime;
                while (numberOfPeriods-- > 0) currentStartTime = ent.TimeSeriesPeriod.PeriodEnd(currentStartTime);
                ent.PeriodStartTime = currentStartTime;
            }
            else
            {
                ent.PeriodStartTime = ent.PeriodEndTime;
            }
            if (flags.HasFlag(HasSummaryFlagsChanges))
            {
                var existingSummaryFlagsUpperBytes = (uint)ent.PeriodSummaryFlags >> 16;
                var deltaUpperBytes = Deserialize7BitDeltaUint(ref ptr);
                var newUpper = (flags.HasFlag(NegateDeltaSummaryFlagsUpperByte) ? -1 : 1) * deltaUpperBytes + existingSummaryFlagsUpperBytes;
                var existingSummaryFlagsLowerBytes = (ushort)ent.PeriodSummaryFlags;
                var deltaLowerBytes = Deserialize7BitDeltaUint(ref ptr);
                var newLower = (flags.HasFlag(NegateDeltaSummaryFlagsLowerByte) ? -1 : 1) * deltaLowerBytes + existingSummaryFlagsLowerBytes;
                var currentFlags = (PricePeriodSummaryFlags)((newUpper << 16) | newLower);
                ent.PeriodSummaryFlags = currentFlags;
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
