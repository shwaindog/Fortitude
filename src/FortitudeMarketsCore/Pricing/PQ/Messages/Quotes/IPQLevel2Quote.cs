#region

using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

public interface IPQLevel2Quote : IPQLevel1Quote, IMutableLevel2Quote
{
    new IPQOrderBook BidBook { get; set; }
    new IPQOrderBook AskBook { get; set; }
    new IPQLevel2Quote Clone();
}
