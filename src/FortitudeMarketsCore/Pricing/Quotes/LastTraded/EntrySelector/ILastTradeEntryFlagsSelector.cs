#region

using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;

#endregion

namespace FortitudeMarketsCore.Pricing.Quotes.LastTraded.EntrySelector;

public interface ILastTradeEntryFlagsSelector<T, Tu> where T : class where Tu : ISourceTickerQuoteInfo
{
    T? FindForLastTradeFlags(Tu? sourceTickerQuoteInfo);
    IMutableLastTrade? ConvertToExpectedImplementation(ILastTrade? checkLastTrade, bool clone = false);
}
