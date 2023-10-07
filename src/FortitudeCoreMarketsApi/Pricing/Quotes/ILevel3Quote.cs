using System;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LastTraded;

namespace FortitudeMarketsApi.Pricing.Quotes
{
    public interface ILevel3Quote : ILevel2Quote, ICloneable<ILevel3Quote>
    {
        IRecentlyTraded RecentlyTraded { get; }
        uint BatchId { get; }
        uint SourceQuoteReference { get; }
        DateTime ValueDate { get; }
        new ILevel3Quote Clone();
    }
}