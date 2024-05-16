#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing.TimeSeries;

#endregion

namespace FortitudeMarketsCore.Pricing.TimeSeries;

public struct Candle
{
    public DateTime SummaryStartTime;
    public TimeSeriesPeriod SummaryPeriod;
    public DateTime SummaryEndTime;
    public decimal StartBidPrice;
    public decimal StartAskPrice;
    public decimal HighestBidPrice;
    public decimal HighestAskPrice;
    public decimal LowestBidPrice;
    public decimal LowestAskPrice;
    public decimal EndBidPrice;
    public decimal EndAskPrice;
    public uint TickCount;
    public long PeriodVolume;
    public long AverageMidPrice;
}

public class QuotePeriodSummary : IMutableQuotePeriodSummary
{
    private TimeSeriesPeriod timeSeriesPeriod;

    public QuotePeriodSummary(TimeSeriesPeriod timeSeriesPeriod = TimeSeriesPeriod.None, DateTime? startTime = null, DateTime? endTime = null,
        decimal startBidPrice = 0m, decimal startAskPrice = 0m, decimal highestBidPrice = 0m,
        decimal highestAskPrice = 0m, decimal lowestBidPrice = 0m, decimal lowestAskPrice = 0m,
        decimal endBidPrice = 0m, decimal endAskPrice = 0m, uint tickCount = 0u, long periodVolume = 0L)
    {
        SummaryPeriod = timeSeriesPeriod;
        SummaryStartTime = startTime ?? DateTimeConstants.UnixEpoch;
        SummaryEndTime = endTime ?? DateTimeConstants.UnixEpoch;
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

    public QuotePeriodSummary(IQuotePeriodSummary toClone)
    {
        SummaryPeriod = toClone.SummaryPeriod;
        SummaryStartTime = toClone.SummaryStartTime;
        SummaryEndTime = toClone.SummaryEndTime;
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

    public TimeSeriesPeriod SummaryPeriod
    {
        get
        {
            if (timeSeriesPeriod == TimeSeriesPeriod.None) timeSeriesPeriod = this.CalcTimeFrame();
            return timeSeriesPeriod;
        }
        set => timeSeriesPeriod = value;
    }

    public DateTime SummaryStartTime { get; set; }
    public DateTime SummaryEndTime { get; set; }
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

    public decimal AverageMidPrice { get; set; }

    public IQuotePeriodSummary CopyFrom(IQuotePeriodSummary source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        SummaryPeriod = source.SummaryPeriod;
        SummaryStartTime = source.SummaryStartTime;
        SummaryEndTime = source.SummaryEndTime;
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
        AverageMidPrice = source.AverageMidPrice;
        return this;
    }

    public IStoreState CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags) => CopyFrom((IQuotePeriodSummary)source, copyMergeFlags);

    object ICloneable.Clone() => Clone();

    IQuotePeriodSummary ICloneable<IQuotePeriodSummary>.Clone() => Clone();

    public IMutableQuotePeriodSummary Clone() => new QuotePeriodSummary(this);

    public bool AreEquivalent(IQuotePeriodSummary? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var timeFrameSame = SummaryPeriod == other.SummaryPeriod;
        var startTimeSame = SummaryStartTime.Equals(other.SummaryStartTime);
        var endTimeSame = SummaryEndTime.Equals(other.SummaryEndTime);
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
        var averageMidPriceSame = AverageMidPrice == other.AverageMidPrice;

        return timeFrameSame && startTimeSame && endTimeSame && startBidPriceSame && startAskPriceSame
               && highestBidPriceSame && highestAskPriceSame && lowestBidPriceSame && lowestAskPriceSame
               && endBidPriceSame && endAskPriceSame && tickCountSame && periodVolumeSame && averageMidPriceSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IQuotePeriodSummary?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)SummaryPeriod;
            hashCode = (hashCode * 397) ^ SummaryStartTime.GetHashCode();
            hashCode = (hashCode * 397) ^ SummaryEndTime.GetHashCode();
            hashCode = (hashCode * 397) ^ StartBidPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ StartAskPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ HighestBidPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ HighestAskPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ EndBidPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ EndAskPrice.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)TickCount;
            hashCode = (hashCode * 397) ^ PeriodVolume.GetHashCode();
            hashCode = (hashCode * 397) ^ AverageMidPrice.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() =>
        $"{nameof(QuotePeriodSummary)} ({nameof(SummaryPeriod)}: {SummaryPeriod}, {nameof(SummaryStartTime)}: {SummaryStartTime:O}, " +
        $"{nameof(SummaryEndTime)}: {SummaryEndTime:O}, {nameof(StartBidPrice)}: {StartBidPrice:N5}, " +
        $"{nameof(StartAskPrice)}: {StartAskPrice:N5}, {nameof(HighestBidPrice)}: {HighestBidPrice:N5}, " +
        $"{nameof(HighestAskPrice)}: {HighestAskPrice:N5}, {nameof(LowestBidPrice)}: {LowestBidPrice:N5}, " +
        $"{nameof(LowestAskPrice)}: {LowestAskPrice:N5}, {nameof(EndBidPrice)}: {EndBidPrice:N5}, " +
        $"{nameof(EndAskPrice)}: {EndAskPrice:N5}, {nameof(TickCount)}: {TickCount}, " +
        $"{nameof(PeriodVolume)}: {PeriodVolume:N2}, {nameof(AverageMidPrice)}: {AverageMidPrice})";
}
