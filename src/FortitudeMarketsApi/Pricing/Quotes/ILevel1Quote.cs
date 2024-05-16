#region

using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing.TimeSeries;

#endregion

namespace FortitudeMarketsApi.Pricing.Quotes;

public interface ILevel1Quote : ILevel0Quote, ICloneable<ILevel1Quote>
{
    DateTime AdapterReceivedTime { get; }
    DateTime AdapterSentTime { get; }
    DateTime SourceBidTime { get; }
    decimal BidPriceTop { get; }
    bool IsBidPriceTopUpdated { get; }
    DateTime SourceAskTime { get; }
    decimal AskPriceTop { get; }
    bool IsAskPriceTopUpdated { get; }
    bool Executable { get; }
    IQuotePeriodSummary? SummaryPeriod { get; }
    new ILevel1Quote Clone();
}

// last level of QuoteStructs as Level2+ has variable length data.
public struct Level1QuoteStruct : ITimeSeriesEntry<Level1QuoteStruct>
{
    public Level1QuoteStruct(DateTime sourceTime, decimal singlePrice, DateTime clientReceivedTime,
        decimal bidPriceTop, decimal askPriceTop, bool executable, bool isReplay = false,
        DateTime? adapterReceiveTime = null, DateTime? adapterSentTime = null, DateTime? sourceBidTime = null,
        DateTime? sourceAskTime = null)
    {
        IsReplay = isReplay;
        SourceTime = sourceTime;
        ClientReceivedTime = clientReceivedTime;
        SinglePrice = singlePrice;
        BidPriceTop = bidPriceTop;
        AskPriceTop = askPriceTop;
        Executable = executable;
        AdapterReceivedTime = adapterReceiveTime ?? DateTime.UnixEpoch;
        AdapterSentTime = adapterSentTime ?? DateTime.UnixEpoch;
        SourceBidTime = sourceBidTime ?? DateTime.UnixEpoch;
        SourceAskTime = sourceAskTime ?? DateTime.UnixEpoch;
    }

    public DateTime SourceTime;
    public decimal SinglePrice;
    public bool IsReplay;
    public DateTime ClientReceivedTime;

    public DateTime AdapterReceivedTime;
    public DateTime AdapterSentTime;
    public DateTime SourceBidTime;
    public DateTime SourceAskTime;
    public decimal BidPriceTop;
    public decimal AskPriceTop;
    public bool Executable;
    public DateTime StorageTime(IStorageTimeResolver<Level1QuoteStruct>? resolver = null) => SourceTime;

    public override string ToString() =>
        $"{nameof(Level1QuoteStruct)}({nameof(SourceTime)}: {SourceTime}, {nameof(SinglePrice)}: {SinglePrice}, " +
        $"{nameof(IsReplay)}: {IsReplay}, {nameof(ClientReceivedTime)}: {ClientReceivedTime}, " +
        $"{nameof(AdapterReceivedTime)}: {AdapterReceivedTime}, {nameof(AdapterSentTime)}: {AdapterSentTime}, " +
        $"{nameof(SourceBidTime)}: {SourceBidTime}, {nameof(SourceAskTime)}: {SourceAskTime}, " +
        $"{nameof(BidPriceTop)}: {BidPriceTop}, {nameof(AskPriceTop)}: {AskPriceTop}, {nameof(Executable)}: {Executable})";
}
