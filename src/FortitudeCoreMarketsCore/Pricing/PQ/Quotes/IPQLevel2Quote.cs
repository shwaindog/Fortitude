using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.LayeredBook;

namespace FortitudeMarketsCore.Pricing.PQ.Quotes
{
    public interface IPQLevel2Quote : IPQLevel1Quote, IMutableLevel2Quote
    {
        new IPQOrderBook BidBook { get; set; }
        new IPQOrderBook AskBook { get; set; }
        new IPQLevel2Quote Clone();
    }
}