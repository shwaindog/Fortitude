namespace FortitudeMarketsApi.Pricing.Conflation;

public interface IMutablePeriodSummary : IPeriodSummary
{
    new TimeFrame TimeFrame { get; set; }
    new DateTime StartTime { get; set; }
    new DateTime EndTime { get; set; }
    new decimal StartBidPrice { get; set; }
    new decimal StartAskPrice { get; set; }
    new decimal HighestBidPrice { get; set; }
    new decimal HighestAskPrice { get; set; }
    new decimal LowestBidPrice { get; set; }
    new decimal LowestAskPrice { get; set; }
    new decimal EndBidPrice { get; set; }
    new decimal EndAskPrice { get; set; }
    new uint TickCount { get; set; }
    new long PeriodVolume { get; set; }
    new IMutablePeriodSummary Clone();
}
