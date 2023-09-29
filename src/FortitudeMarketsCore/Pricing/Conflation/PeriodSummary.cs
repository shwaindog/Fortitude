using System;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.Conflation;

namespace FortitudeMarketsCore.Pricing.Conflation
{
    public class PeriodSummary : IMutablePeriodSummary
    {
        private TimeFrame timeFrame;

        public PeriodSummary(TimeFrame timeFrame = TimeFrame.Unknown, DateTime? startTime = null, DateTime? endTime = null, 
            decimal startBidPrice = 0m, decimal startAskPrice = 0m, decimal highestBidPrice = 0m, 
            decimal highestAskPrice = 0m, decimal lowestBidPrice = 0m, decimal lowestAskPrice = 0m, 
            decimal endBidPrice = 0m, decimal endAskPrice = 0m, uint tickCount = 0u, long periodVolume = 0L)
        {
            TimeFrame = timeFrame;
            StartTime = startTime ?? DateTimeConstants.UnixEpoch;
            EndTime = endTime ?? DateTimeConstants.UnixEpoch;
            StartBidPrice = startBidPrice;
            StartAskPrice = startAskPrice;
            HighestBidPrice = highestBidPrice;
            HighestAskPrice = highestAskPrice;
            LowestBidPrice = lowestBidPrice;
            LowestAskPrice = lowestAskPrice;
            EndBidPrice = endBidPrice;
            EndAskPrice = endAskPrice;
            TickCount = tickCount;
            PeriodVolume = periodVolume;
        }

        public PeriodSummary(IPeriodSummary toClone)
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
            get
            {
                if (timeFrame == TimeFrame.Unknown)
                {
                    timeFrame = this.CalcTimeFrame();
                }
                return timeFrame;
            }
            set => timeFrame = value;
        }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal StartBidPrice { get; set; }
        public decimal StartAskPrice { get; set; }
        public decimal HighestBidPrice { get; set; }
        public decimal HighestAskPrice { get; set; }
        public decimal LowestBidPrice { get; set; }
        public decimal LowestAskPrice { get; set; }
        public decimal EndBidPrice { get; set; }
        public decimal EndAskPrice { get; set; }
        public uint TickCount { get; set; }
        public long PeriodVolume { get; set; }

        public void CopyFrom(IPeriodSummary source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
        {
            TimeFrame = source.TimeFrame;
            StartTime = source.StartTime;
            EndTime = source.EndTime;
            StartBidPrice = source.StartBidPrice;
            StartAskPrice = source.StartAskPrice;
            HighestBidPrice = source.HighestBidPrice;
            HighestAskPrice = source.HighestAskPrice;
            LowestBidPrice = source.LowestBidPrice;
            LowestAskPrice = source.LowestAskPrice;
            EndBidPrice = source.EndBidPrice;
            EndAskPrice = source.EndAskPrice;
            TickCount = source.TickCount;
            PeriodVolume = source.PeriodVolume;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        IPeriodSummary ICloneable<IPeriodSummary>.Clone()
        {
            return Clone();
        }

        public IMutablePeriodSummary Clone()
        {
            return new PeriodSummary(this);
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
            var tickCountSame =  TickCount == other.TickCount;
            var periodVolumeSame = PeriodVolume == other.PeriodVolume;

            return timeFrameSame && startTimeSame && endTimeSame && startBidPriceSame && startAskPriceSame
                   && highestBidPriceSame && highestAskPriceSame && lowestBidPriceSame && lowestAskPriceSame
                   && endBidPriceSame && endAskPriceSame && tickCountSame && periodVolumeSame;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || AreEquivalent((IPeriodSummary) obj, true);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) TimeFrame;
                hashCode = (hashCode * 397) ^ StartTime.GetHashCode();
                hashCode = (hashCode * 397) ^ EndTime.GetHashCode();
                hashCode = (hashCode * 397) ^ StartBidPrice.GetHashCode();
                hashCode = (hashCode * 397) ^ StartAskPrice.GetHashCode();
                hashCode = (hashCode * 397) ^ HighestBidPrice.GetHashCode();
                hashCode = (hashCode * 397) ^ HighestAskPrice.GetHashCode();
                hashCode = (hashCode * 397) ^ EndBidPrice.GetHashCode();
                hashCode = (hashCode * 397) ^ EndAskPrice.GetHashCode();
                hashCode = (hashCode * 397) ^ (int)TickCount;
                hashCode = (hashCode * 397) ^ PeriodVolume.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"PeriodSummary {{ {nameof(TimeFrame)}: {TimeFrame}, {nameof(StartTime)}: {StartTime:O}, " +
                   $"{nameof(EndTime)}: {EndTime:O}, {nameof(StartBidPrice)}: {StartBidPrice:N5}, " +
                   $"{nameof(StartAskPrice)}: {StartAskPrice:N5}, {nameof(HighestBidPrice)}: {HighestBidPrice:N5}, " +
                   $"{nameof(HighestAskPrice)}: {HighestAskPrice:N5}, {nameof(LowestBidPrice)}: {LowestBidPrice:N5}, " +
                   $"{nameof(LowestAskPrice)}: {LowestAskPrice:N5}, {nameof(EndBidPrice)}: {EndBidPrice:N5}, " +
                   $"{nameof(EndAskPrice)}: {EndAskPrice:N5}, {nameof(TickCount)}: {TickCount}, {nameof(PeriodVolume)}: " +
                   $"{PeriodVolume:N2} }}";
        }
    }
}
