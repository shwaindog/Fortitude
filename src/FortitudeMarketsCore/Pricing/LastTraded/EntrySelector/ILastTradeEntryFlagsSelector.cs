using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;

namespace FortitudeMarketsCore.Pricing.LastTraded.EntrySelector
{
    public interface ILastTradeEntryFlagsSelector<T, Tu> where T : class where Tu : ISourceTickerQuoteInfo
    {
        T FindForLastTradeFlags(Tu sourceTickerQuoteInfo);
        IMutableLastTrade ConvertToExpectedImplementation(ILastTrade checkLastTrade, bool clone = false);
    }
}