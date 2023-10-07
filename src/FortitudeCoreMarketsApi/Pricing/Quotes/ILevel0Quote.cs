using System;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;

namespace FortitudeMarketsApi.Pricing.Quotes
{
    public interface ILevel0Quote : ICloneable<ILevel0Quote>, IInterfacesComparable<ILevel0Quote>
    {
        bool IsReplay { get; }
        DateTime SourceTime { get; }
        DateTime ClientReceivedTime { get; }
        ISourceTickerQuoteInfo SourceTickerQuoteInfo { get; }
        decimal SinglePrice { get; }
        new ILevel0Quote Clone();
    }
}