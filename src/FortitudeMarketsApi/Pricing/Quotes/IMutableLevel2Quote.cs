using FortitudeMarketsApi.Pricing.LayeredBook;

namespace FortitudeMarketsApi.Pricing.Quotes
{
    public interface IMutableLevel2Quote : ILevel2Quote, IMutableLevel1Quote
    {
        new IMutableOrderBook BidBook { get; set; }
        new IMutableOrderBook AskBook { get; set; }
        new bool IsBidBookChanged { get; set; }
        new bool IsAskBookChanged { get; set; }
        new IMutableLevel2Quote Clone();
    }
}