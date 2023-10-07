using System;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.Conflation;

namespace FortitudeMarketsApi.Pricing.Quotes
{
    public interface ILevel1Quote: ILevel0Quote, ICloneable<ILevel1Quote>
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
        IPeriodSummary PeriodSummary { get; }
        new ILevel1Quote Clone();
    }
}
