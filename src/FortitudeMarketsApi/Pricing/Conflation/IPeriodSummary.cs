using System;
using FortitudeCommon.Types;

namespace FortitudeMarketsApi.Pricing.Conflation
{
    public interface IPeriodSummary : ICloneable<IPeriodSummary>, IInterfacesComparable<IPeriodSummary>
    {
        TimeFrame TimeFrame { get; }
        DateTime StartTime { get; }
        DateTime EndTime { get; }
        decimal StartBidPrice { get; }
        decimal StartAskPrice { get; }
        decimal HighestBidPrice { get; }
        decimal HighestAskPrice { get; }
        decimal LowestBidPrice { get; }
        decimal LowestAskPrice { get; }
        decimal EndBidPrice { get; }
        decimal EndAskPrice { get; }
        uint TickCount { get; }
        long PeriodVolume { get; }
    }
}
