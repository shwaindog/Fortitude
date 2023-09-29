using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.Conflation;
using FortitudeMarketsCore.Pricing.Conflation;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Quotes;

namespace FortitudeMarketsCore.Pricing.PQ.Conflation
{
    public class PQPeriodSummary : IPQPeriodSummary
    {
        private TimeFrame timeFrame;
        private PeriodSummaryUpdatedFlags updatedFlags;
        private DateTime startTime = DateTimeConstants.UnixEpoch;
        private DateTime endTime = DateTimeConstants.UnixEpoch;
        private decimal startBidPrice;
        private decimal startAskPrice;
        private decimal highestBidPrice;
        private decimal highestAskPrice;
        private decimal lowestBidPrice;
        private decimal lowestAskPrice;
        private decimal endBidPrice;
        private decimal endAskPrice;
        private uint tickCount;
        private long periodVolume;

        public PQPeriodSummary()
        {
        }

        public PQPeriodSummary(IPeriodSummary toClone)
        {
            TimeFrame = toClone.TimeFrame;
            StartTime = toClone.StartTime;
            EndTime = toClone.EndTime;
            StartBidPrice = toClone.StartBidPrice;
            StartAskPrice = toClone.StartAskPrice;
            HighestBidPrice = toClone.HighestBidPrice;
            HighestAskPrice = toClone.HighestAskPrice;
            LowestBidPrice = toClone.LowestBidPrice;
            LowestAskPrice = toClone.LowestAskPrice;
            EndBidPrice = toClone.EndBidPrice;
            EndAskPrice = toClone.EndAskPrice;
            TickCount = toClone.TickCount;
            PeriodVolume = toClone.PeriodVolume;
        }

        public TimeFrame TimeFrame
        {
            get { if (timeFrame == TimeFrame.Unknown)
                    {
                        timeFrame = this.CalcTimeFrame();
                    }
                return timeFrame;
            }
            set => timeFrame = value;
        }

        public DateTime StartTime
        {
            get => startTime;
            set
            {
                if (startTime == value) return;
                IsStartTimeDateUpdated |= startTime.GetHoursFromUnixEpoch() != value.GetHoursFromUnixEpoch();
                IsStartTimeSubHourUpdated |= startTime.GetSubHourComponent() != value.GetSubHourComponent();
                startTime = value;
            }
        }

        public bool IsStartTimeDateUpdated
        {
            get => (updatedFlags & PeriodSummaryUpdatedFlags.StartTimeDateFlag) > 0;
            set
            {
                if (value)
                {
                    updatedFlags |= PeriodSummaryUpdatedFlags.StartTimeDateFlag;
                }
                else if (IsStartTimeDateUpdated)
                {
                    updatedFlags ^= PeriodSummaryUpdatedFlags.StartTimeDateFlag;
                }
            }
        }

        public bool IsStartTimeSubHourUpdated
        {
            get => (updatedFlags & PeriodSummaryUpdatedFlags.StartTimeSubHourFlag) > 0;
            set
            {
                if (value)
                {
                    updatedFlags |= PeriodSummaryUpdatedFlags.StartTimeSubHourFlag;
                }
                else if (IsStartTimeSubHourUpdated)
                {
                    updatedFlags ^= PeriodSummaryUpdatedFlags.StartTimeSubHourFlag;
                }
            }
        }

        public DateTime EndTime
        {
            get => endTime;
            set
            {
                if (endTime == value) return;
                IsEndTimeDateUpdated |= endTime.GetHoursFromUnixEpoch() != value.GetHoursFromUnixEpoch();
                IsEndTimeSubHourUpdated |= endTime.GetSubHourComponent() != value.GetSubHourComponent();
                endTime = value;
            }
        }

        public bool IsEndTimeDateUpdated
        {
            get => (updatedFlags & PeriodSummaryUpdatedFlags.EndTimeDateFlag) > 0;
            set
            {
                if (value)
                {
                    updatedFlags |= PeriodSummaryUpdatedFlags.EndTimeDateFlag;
                }
                else if (IsEndTimeDateUpdated)
                {
                    updatedFlags ^= PeriodSummaryUpdatedFlags.EndTimeDateFlag;
                }
            }
        }

        public bool IsEndTimeSubHourUpdated
        {
            get => (updatedFlags & PeriodSummaryUpdatedFlags.EndTimeSubHourFlag) > 0;
            set
            {
                if (value)
                {
                    updatedFlags |= PeriodSummaryUpdatedFlags.EndTimeSubHourFlag;
                }
                else if (IsEndTimeSubHourUpdated)
                {
                    updatedFlags ^= PeriodSummaryUpdatedFlags.EndTimeSubHourFlag;
                }
            }
        }

        public decimal StartBidPrice
        {
            get => startBidPrice;
            set
            {
                if (startBidPrice == value) return;
                IsStartBidPriceUpdated = true;
                startBidPrice = value;
            }
        }

        public bool IsStartBidPriceUpdated
        {
            get => (updatedFlags & PeriodSummaryUpdatedFlags.StartBidPriceFlag) > 0;
            set
            {
                if (value)
                {
                    updatedFlags |= PeriodSummaryUpdatedFlags.StartBidPriceFlag;
                }
                else if (IsStartBidPriceUpdated)
                {
                    updatedFlags ^= PeriodSummaryUpdatedFlags.StartBidPriceFlag;
                }
            }
        }

        public decimal StartAskPrice
        {
            get => startAskPrice;
            set
            {
                if (startAskPrice == value) return;
                IsStartAskPriceUpdated = true;
                startAskPrice = value;
            }
        }

        public bool IsStartAskPriceUpdated
        {
            get => (updatedFlags & PeriodSummaryUpdatedFlags.StartAskPriceFlag) > 0;
            set
            {
                if (value)
                {
                    updatedFlags |= PeriodSummaryUpdatedFlags.StartAskPriceFlag;
                }
                else if (IsStartAskPriceUpdated)
                {
                    updatedFlags ^= PeriodSummaryUpdatedFlags.StartAskPriceFlag;
                }
            }
        }

        public decimal HighestBidPrice
        {
            get => highestBidPrice;
            set
            {
                if (highestBidPrice == value) return;
                IsHighestBidPriceUpdated = true;
                highestBidPrice = value;
            }
        }

        public bool IsHighestBidPriceUpdated
        {
            get => (updatedFlags & PeriodSummaryUpdatedFlags.HighestBidPriceFlag) > 0;
            set
            {
                if (value)
                {
                    updatedFlags |= PeriodSummaryUpdatedFlags.HighestBidPriceFlag;
                }
                else if (IsHighestBidPriceUpdated)
                {
                    updatedFlags ^= PeriodSummaryUpdatedFlags.HighestBidPriceFlag;
                }
            }
        }

        public decimal HighestAskPrice
        {
            get => highestAskPrice;
            set
            {
                if (highestAskPrice == value) return;
                IsHighestAskPriceUpdated = true;
                highestAskPrice = value;
            }
        }

        public bool IsHighestAskPriceUpdated
        {
            get => (updatedFlags & PeriodSummaryUpdatedFlags.HighestAskPriceFlag) > 0;
            set
            {
                if (value)
                {
                    updatedFlags |= PeriodSummaryUpdatedFlags.HighestAskPriceFlag;
                }
                else if (IsHighestAskPriceUpdated)
                {
                    updatedFlags ^= PeriodSummaryUpdatedFlags.HighestAskPriceFlag;
                }
            }
        }

        public decimal LowestBidPrice
        {
            get => lowestBidPrice;
            set
            {
                if (lowestBidPrice == value) return;
                IsLowestBidPriceUpdated = true;
                lowestBidPrice = value;
            }
        }

        public bool IsLowestBidPriceUpdated
        {
            get => (updatedFlags & PeriodSummaryUpdatedFlags.LowestBidPriceFlag) > 0;
            set
            {
                if (value)
                {
                    updatedFlags |= PeriodSummaryUpdatedFlags.LowestBidPriceFlag;
                }
                else if (IsLowestBidPriceUpdated)
                {
                    updatedFlags ^= PeriodSummaryUpdatedFlags.LowestBidPriceFlag;
                }
            }
        }

        public decimal LowestAskPrice
        {
            get => lowestAskPrice;
            set
            {
                if (lowestAskPrice == value) return;
                IsLowestAskPriceUpdated = true;
                lowestAskPrice = value;
            }
        }

        public bool IsLowestAskPriceUpdated
        {
            get => (updatedFlags & PeriodSummaryUpdatedFlags.LowestAskPriceFlag) > 0;
            set
            {
                if (value)
                {
                    updatedFlags |= PeriodSummaryUpdatedFlags.LowestAskPriceFlag;
                }
                else if (IsLowestAskPriceUpdated)
                {
                    updatedFlags ^= PeriodSummaryUpdatedFlags.LowestAskPriceFlag;
                }
            }
        }

        public decimal EndBidPrice
        {
            get => endBidPrice;
            set
            {
                if (endBidPrice == value) return;
                IsEndBidPriceUpdated = true;
                endBidPrice = value;
            }
        }

        public bool IsEndBidPriceUpdated
        {
            get => (updatedFlags & PeriodSummaryUpdatedFlags.EndBidPriceFlag) > 0;
            set
            {
                if (value)
                {
                    updatedFlags |= PeriodSummaryUpdatedFlags.EndBidPriceFlag;
                }
                else if (IsEndBidPriceUpdated)
                {
                    updatedFlags ^= PeriodSummaryUpdatedFlags.EndBidPriceFlag;
                }
            }
        }

        public decimal EndAskPrice
        {
            get => endAskPrice;
            set
            {
                if (endAskPrice == value) return;
                IsEndAskPriceUpdated = true;
                endAskPrice = value;
            }
        }

        public bool IsEndAskPriceUpdated
        {
            get => (updatedFlags & PeriodSummaryUpdatedFlags.EndAskPriceFlag) > 0;
            set
            {
                if (value)
                {
                    updatedFlags |= PeriodSummaryUpdatedFlags.EndAskPriceFlag;
                }
                else if (IsEndAskPriceUpdated)
                {
                    updatedFlags ^= PeriodSummaryUpdatedFlags.EndAskPriceFlag;
                }
            }
        }

        public uint TickCount
        {
            get => tickCount;
            set
            {
                if (tickCount == value) return;
                IsTickCountUpdated = true;
                tickCount = value;
            }
        }

        public bool IsTickCountUpdated
        {
            get => (updatedFlags & PeriodSummaryUpdatedFlags.TickCountFlag) > 0;
            set
            {
                if (value)
                {
                    updatedFlags |= PeriodSummaryUpdatedFlags.TickCountFlag;
                }
                else if (IsTickCountUpdated)
                {
                    updatedFlags ^= PeriodSummaryUpdatedFlags.TickCountFlag;
                }
            }
        }

        public long PeriodVolume
        {
            get => periodVolume;
            set
            {
                if (periodVolume == value) return;
                IsPeriodVolumeLowerBytesUpdated |= unchecked((uint)periodVolume != (uint) value);
                IsPeriodVolumeUpperBytesUpdated |= periodVolume >> 32 != value >> 32;
                periodVolume = value;
            }
        }

        public bool IsPeriodVolumeLowerBytesUpdated
        {
            get => (updatedFlags & PeriodSummaryUpdatedFlags.PeriodVolumeLowerBytesFlag) > 0;
            set
            {
                if (value)
                {
                    updatedFlags |= PeriodSummaryUpdatedFlags.PeriodVolumeLowerBytesFlag;
                }
                else if (IsPeriodVolumeLowerBytesUpdated)
                {
                    updatedFlags ^= PeriodSummaryUpdatedFlags.PeriodVolumeLowerBytesFlag;
                }
            }
        }

        public bool IsPeriodVolumeUpperBytesUpdated
        {
            get => (updatedFlags & PeriodSummaryUpdatedFlags.PeriodVolumeUpperBytesFlag) > 0;
            set
            {
                if (value)
                {
                    updatedFlags |= PeriodSummaryUpdatedFlags.PeriodVolumeUpperBytesFlag;
                }
                else if (IsPeriodVolumeUpperBytesUpdated)
                {
                    updatedFlags ^= PeriodSummaryUpdatedFlags.PeriodVolumeUpperBytesFlag;
                }
            }
        }

        public bool HasUpdates
        {
            get => updatedFlags > 0;
            set => updatedFlags = value ? updatedFlags.AllFlags() : 0;
        }

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, UpdateStyle updateStyle,
            IPQQuotePublicationPrecisionSettings quotePublicationPrecisionSettings = null)
        {
            var updatedOnly = (updateStyle & UpdateStyle.Updates) > 0;
            if (!updatedOnly || IsStartTimeDateUpdated)
            {
                yield return new PQFieldUpdate(PQFieldKeys.PeriodStartDateTime, startTime.GetHoursFromUnixEpoch());
            }
            if (!updatedOnly || IsStartTimeSubHourUpdated)
            {
                byte fifthByte = startTime.GetSubHourComponent().BreakLongToByteAndUint(out var lowerFourBytes);
                yield return new PQFieldUpdate(PQFieldKeys.PeriodStartSubHourTime, lowerFourBytes, fifthByte);
            }
            if (!updatedOnly || IsEndTimeDateUpdated)
            {
                yield return new PQFieldUpdate(PQFieldKeys.PeriodEndDateTime, endTime.GetHoursFromUnixEpoch());
            }
            if (!updatedOnly || IsEndTimeSubHourUpdated)
            {
                byte fifthByte = endTime.GetSubHourComponent().BreakLongToByteAndUint(out var lowerFourBytes);
                yield return new PQFieldUpdate(PQFieldKeys.PeriodEndSubHourTime, lowerFourBytes, fifthByte);
            }
            if (!updatedOnly || IsStartBidPriceUpdated)
            {
                yield return new PQFieldUpdate(PQFieldKeys.PeriodStartPrice, StartBidPrice, 
                    quotePublicationPrecisionSettings.PriceScalingPrecision);
            }
            if (!updatedOnly || IsStartAskPriceUpdated)
            {
                yield return new PQFieldUpdate(PQFieldKeys.PeriodStartPrice, StartAskPrice,
                    (byte)(PQFieldFlags.IsAskSideFlag | quotePublicationPrecisionSettings.PriceScalingPrecision));
            }
            if (!updatedOnly || IsHighestBidPriceUpdated)
            {
                yield return new PQFieldUpdate(PQFieldKeys.PeriodHighestPrice, HighestBidPrice,
                    quotePublicationPrecisionSettings.PriceScalingPrecision);
            }
            if (!updatedOnly || IsHighestAskPriceUpdated)
            {
                yield return new PQFieldUpdate(PQFieldKeys.PeriodHighestPrice, HighestAskPrice,
                    (byte)(PQFieldFlags.IsAskSideFlag | quotePublicationPrecisionSettings.PriceScalingPrecision));
            }
            if (!updatedOnly || IsLowestBidPriceUpdated)
            {
                yield return new PQFieldUpdate(PQFieldKeys.PeriodLowestPrice, LowestBidPrice,
                    quotePublicationPrecisionSettings.PriceScalingPrecision);
            }
            if (!updatedOnly || IsLowestAskPriceUpdated)
            {
                yield return new PQFieldUpdate(PQFieldKeys.PeriodLowestPrice, LowestAskPrice,
                    (byte)(PQFieldFlags.IsAskSideFlag | quotePublicationPrecisionSettings.PriceScalingPrecision));
            }
            if (!updatedOnly || IsEndBidPriceUpdated)
            {
                yield return new PQFieldUpdate(PQFieldKeys.PeriodEndPrice, EndBidPrice,
                    quotePublicationPrecisionSettings.PriceScalingPrecision);
            }
            if (!updatedOnly || IsEndAskPriceUpdated)
            {
                yield return new PQFieldUpdate(PQFieldKeys.PeriodEndPrice, EndAskPrice,
                    (byte)(PQFieldFlags.IsAskSideFlag | quotePublicationPrecisionSettings.PriceScalingPrecision));
            }
            if (!updatedOnly || IsTickCountUpdated)
            {
                yield return new PQFieldUpdate(PQFieldKeys.PeriodTickCount, TickCount);
            }
            if (!updatedOnly || IsPeriodVolumeLowerBytesUpdated)
            {
                uint lowerBytes = (uint)PeriodVolume;
                yield return new PQFieldUpdate(PQFieldKeys.PeriodVolumeLowerBytes, lowerBytes);
            }
            if (!updatedOnly || IsPeriodVolumeUpperBytesUpdated)
            {
                uint upperBytes = (uint)(PeriodVolume >> 32);
                yield return new PQFieldUpdate(PQFieldKeys.PeriodVolumeUpperBytes, upperBytes);
            }
        }

        public int UpdateField(PQFieldUpdate pqFieldUpdate)
        {
            switch (pqFieldUpdate.Id)
            {
                case PQFieldKeys.PeriodStartDateTime:
                    IsStartTimeDateUpdated = true;
                    PQFieldConverters.UpdateHoursFromUnixEpoch(ref startTime, pqFieldUpdate.Value);
                    return 0;
                case PQFieldKeys.PeriodStartSubHourTime:
                    IsStartTimeSubHourUpdated = true;
                    PQFieldConverters.UpdateSubHourComponent(ref startTime,
                        pqFieldUpdate.Flag.AppendUintToMakeLong(pqFieldUpdate.Value));
                    return 0;
                case PQFieldKeys.PeriodEndDateTime:
                    IsEndTimeDateUpdated = true;
                    PQFieldConverters.UpdateHoursFromUnixEpoch(ref endTime, pqFieldUpdate.Value);
                    return 0;
                case PQFieldKeys.PeriodEndSubHourTime:
                    IsEndTimeSubHourUpdated = true;
                    PQFieldConverters.UpdateSubHourComponent(ref endTime,
                        pqFieldUpdate.Flag.AppendUintToMakeLong(pqFieldUpdate.Value));
                    return 0;
                case PQFieldKeys.PeriodStartPrice:
                    if (pqFieldUpdate.IsBid())
                    {
                        StartBidPrice = PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
                    }
                    else
                    {
                        StartAskPrice = PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
                    }
                    return 0;
                case PQFieldKeys.PeriodHighestPrice:
                    if (pqFieldUpdate.IsBid())
                    {
                        HighestBidPrice = PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
                    }
                    else
                    {
                        HighestAskPrice = PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
                    }
                    return 0;
                case PQFieldKeys.PeriodLowestPrice:
                    if (pqFieldUpdate.IsBid())
                    {
                        LowestBidPrice = PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
                    }
                    else
                    {
                        LowestAskPrice = PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
                    }
                    return 0;
                case PQFieldKeys.PeriodEndPrice:
                    if (pqFieldUpdate.IsBid())
                    {
                        EndBidPrice = PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
                    }
                    else
                    {
                        EndAskPrice = PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
                    }
                    return 0;
                case PQFieldKeys.PeriodTickCount:
                    TickCount = pqFieldUpdate.Value;
                    return 0;
                case PQFieldKeys.PeriodVolumeLowerBytes:
                    PeriodVolume = PeriodVolume & unchecked ((long)0xFFFF_FFFF_0000_0000) | pqFieldUpdate.Value;
                    return 0;
                case PQFieldKeys.PeriodVolumeUpperBytes:
                    PeriodVolume = PeriodVolume & 0x0000_0000_FFFF_FFFF | ((long)pqFieldUpdate.Value << 32);
                    return 0;
                default:
                    return -1;
            }
        }

        public void CopyFrom(IPeriodSummary ps, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
        {
            if (!(ps is IPQPeriodSummary pqPs))
            {
                StartTime = ps.StartTime;
                EndTime = ps.EndTime;
                StartBidPrice = ps.StartBidPrice;
                StartAskPrice = ps.StartAskPrice;
                HighestBidPrice = ps.HighestBidPrice;
                HighestAskPrice = ps.HighestAskPrice;
                LowestBidPrice = ps.LowestBidPrice;
                LowestAskPrice = ps.LowestAskPrice;
                EndBidPrice = ps.EndBidPrice;
                EndAskPrice = ps.EndAskPrice;
                TickCount = ps.TickCount;
                PeriodVolume = ps.PeriodVolume;
            }
            else
            {
                // between types only copy the changed parts not everything.
                if (pqPs.IsStartTimeDateUpdated)
                {
                    PQFieldConverters.UpdateHoursFromUnixEpoch(ref startTime,
                        pqPs.StartTime.GetHoursFromUnixEpoch());
                }
                if (pqPs.IsStartTimeSubHourUpdated)
                {
                    PQFieldConverters.UpdateSubHourComponent(ref startTime,
                        pqPs.StartTime.GetSubHourComponent());
                }
                if (pqPs.IsEndTimeDateUpdated)
                {
                    PQFieldConverters.UpdateHoursFromUnixEpoch(ref endTime,
                        pqPs.EndTime.GetHoursFromUnixEpoch());
                }
                if (pqPs.IsEndTimeSubHourUpdated)
                {
                    PQFieldConverters.UpdateSubHourComponent(ref endTime,
                        pqPs.EndTime.GetSubHourComponent());
                }
                if (pqPs.IsStartBidPriceUpdated)
                {
                    StartBidPrice = pqPs.StartBidPrice;
                }
                if (pqPs.IsStartAskPriceUpdated)
                {
                    StartAskPrice = pqPs.StartAskPrice;
                }
                if (pqPs.IsHighestBidPriceUpdated)
                {
                    HighestBidPrice = pqPs.HighestBidPrice;
                }
                if (pqPs.IsHighestAskPriceUpdated)
                {
                    HighestAskPrice = pqPs.HighestAskPrice;
                }
                if (pqPs.IsLowestBidPriceUpdated)
                {
                    LowestBidPrice = pqPs.LowestBidPrice;
                }
                if (pqPs.IsLowestAskPriceUpdated)
                {
                    LowestAskPrice = pqPs.LowestAskPrice;
                }
                if (pqPs.IsEndBidPriceUpdated)
                {
                    EndBidPrice = pqPs.EndBidPrice;
                }
                if (pqPs.IsEndAskPriceUpdated)
                {
                    EndAskPrice = pqPs.EndAskPrice;
                }
                if (pqPs.IsTickCountUpdated)
                {
                    TickCount = pqPs.TickCount;
                }
                if (pqPs.IsPeriodVolumeLowerBytesUpdated || pqPs.IsPeriodVolumeUpperBytesUpdated)
                {
                    PeriodVolume = pqPs.PeriodVolume;
                }

                if (pqPs is PQPeriodSummary pqPeriodSummary)
                {
                    updatedFlags = pqPeriodSummary.updatedFlags;
                }
            }
        }

        public IMutablePeriodSummary Clone()
        {
            return new PQPeriodSummary(this);
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        IPeriodSummary ICloneable<IPeriodSummary>.Clone()
        {
            return Clone();
        }

        public bool AreEquivalent(IPeriodSummary other, bool exactTypes = false)
        {
            if (other == null) return false;
            if (exactTypes && other.GetType() != GetType()) return false;
            var timeFrameSame = TimeFrame == other.TimeFrame;
            var startTimeSame = StartTime.Equals(other.StartTime);
            var endTimeSame = EndTime.Equals(other.EndTime);
            var startBidPriceSame = StartBidPrice == other.StartBidPrice;
            var startAskPriceSame = StartAskPrice == other.StartAskPrice;
            var highestBidPriceSame = HighestBidPrice == other.HighestBidPrice;
            var highestAskPriceSame = HighestAskPrice == other.HighestAskPrice;
            var lowestBidPriceSame = LowestBidPrice == other.LowestBidPrice;
            var lowestAskPriceSame = LowestAskPrice == other.LowestAskPrice;
            var endBidPriceSame = EndBidPrice == other.EndBidPrice;
            var endAskPriceSame = EndAskPrice == other.EndAskPrice;
            var tickCountSame = TickCount == other.TickCount;
            var periodVolumeSame = PeriodVolume == other.PeriodVolume;

            var updateFlagsSame = true;
            if (exactTypes)
            {
                var otherPQ = other as PQPeriodSummary;
                updateFlagsSame = updatedFlags == otherPQ.updatedFlags;
            }

            return timeFrameSame && startTimeSame && endTimeSame && startBidPriceSame && startAskPriceSame
                   && highestBidPriceSame && highestAskPriceSame && lowestBidPriceSame && lowestAskPriceSame
                   && endBidPriceSame && endAskPriceSame && tickCountSame && periodVolumeSame && updateFlagsSame;
        }
        
        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || AreEquivalent((IPeriodSummary)obj, true);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = startTime.GetHashCode();
                hashCode = (hashCode * 397) ^ endTime.GetHashCode();
                hashCode = (hashCode * 397) ^ startBidPrice.GetHashCode();
                hashCode = (hashCode * 397) ^ startAskPrice.GetHashCode();
                hashCode = (hashCode * 397) ^ highestBidPrice.GetHashCode();
                hashCode = (hashCode * 397) ^ highestAskPrice.GetHashCode();
                hashCode = (hashCode * 397) ^ lowestBidPrice.GetHashCode();
                hashCode = (hashCode * 397) ^ lowestAskPrice.GetHashCode();
                hashCode = (hashCode * 397) ^ endBidPrice.GetHashCode();
                hashCode = (hashCode * 397) ^ endAskPrice.GetHashCode();
                hashCode = (hashCode * 397) ^ (int)tickCount;
                hashCode = (hashCode * 397) ^ periodVolume.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"PQPeriodSummary {{ {nameof(TimeFrame)}: {TimeFrame}, {nameof(StartTime)}: {StartTime}, " +
                   $"{nameof(EndTime)}: {EndTime}, {nameof(StartBidPrice)}: {StartBidPrice}, {nameof(StartAskPrice)}:" +
                   $" {StartAskPrice}, {nameof(HighestBidPrice)}: {HighestBidPrice}, {nameof(HighestAskPrice)}: " +
                   $"{HighestAskPrice}, {nameof(LowestBidPrice)}: {LowestBidPrice}, {nameof(LowestAskPrice)}: " +
                   $"{LowestAskPrice}, {nameof(EndBidPrice)}: {EndBidPrice}, {nameof(EndAskPrice)}: {EndAskPrice}, " +
                   $"{nameof(TickCount)}: {TickCount}, {nameof(PeriodVolume)}: {PeriodVolume} }}";
        }
    }
}