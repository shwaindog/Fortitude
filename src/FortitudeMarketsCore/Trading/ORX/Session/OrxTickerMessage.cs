#region

using System.Text;
using FortitudeIO.Protocols.ORX.Serialization;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Session;

public sealed class OrxTickerMessage : OrxTradingMessage
{
    public OrxTickerMessage(string exchange, string ticker)
    {
        Exchange = exchange;
        Ticker = ticker;
    }

    public OrxTickerMessage() { }
    public override uint MessageId => (uint)TradingMessageIds.Ticker;

    [OrxMandatoryField(10)] public string? Exchange { get; set; }

    [OrxMandatoryField(11)] public string? Ticker { get; set; }

    [OrxMandatoryField(12)] public long ContractSize { get; set; }

    [OrxMandatoryField(13)] public long MinimumSize { get; set; }

    [OrxMandatoryField(14)] public long SizeIncrement { get; set; }

    [OrxMandatoryField(15)] public long MaximumSize { get; set; }

    [OrxMandatoryField(16)] public decimal PriceIncrement { get; set; }

    [OrxMandatoryField(17)] public uint Mql { get; set; }

    [OrxMandatoryField(18)] public bool Tradeable { get; set; }

    public override bool Equals(object? obj) =>
        obj is OrxTickerMessage m && m.Exchange == Exchange && m.Ticker == Ticker;

    public override int GetHashCode()
    {
        var hash = 13;
        hash = hash * 7 + Exchange?.GetHashCode() ?? 0;
        hash = hash * 7 + Ticker?.GetHashCode() ?? 0;
        return hash;
    }

    internal void UpdateData(long contractSize, long minSize, long sizeInc, long maxSize,
        decimal priceInc,
        uint mql)
    {
        ContractSize = contractSize;
        MinimumSize = minSize;
        SizeIncrement = sizeInc;
        MaximumSize = maxSize;
        PriceIncrement = priceInc;
        Mql = mql;
    }

    internal void UpdateStatus(bool tradeable)
    {
        Tradeable = tradeable;
    }

    public override string ToString() =>
        new StringBuilder(1024)
            .Append(Exchange).Append(',')
            .Append(Ticker).Append(',')
            .Append(ContractSize).Append(',')
            .Append(MinimumSize).Append(',')
            .Append(SizeIncrement).Append(',')
            .Append(MaximumSize).Append(',')
            .Append(PriceIncrement).Append(',')
            .Append(Mql).Append(',')
            .Append(Tradeable).ToString();
}
