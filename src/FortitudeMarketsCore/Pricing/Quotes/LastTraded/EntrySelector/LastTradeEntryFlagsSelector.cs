#region

using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;

#endregion

namespace FortitudeMarketsCore.Pricing.Quotes.LastTraded.EntrySelector;

public abstract class LastTradeEntryFlagsSelector<T, Tu> :
    ILastTradeEntryFlagsSelector<T, Tu> where T : class where Tu : ISourceTickerQuoteInfo
{
    public T? FindForLastTradeFlags(Tu? pqSourceTickerQuoteInfo)
    {
        var flags = pqSourceTickerQuoteInfo!.LastTradedFlags;
        const LastTradedFlags lastTradeEntryFlags = LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime;
        var testCondition = lastTradeEntryFlags;

        var onlyPriceVolume = (flags & testCondition) == flags;
        if (onlyPriceVolume) return SelectSimpleLastTradeEntry(pqSourceTickerQuoteInfo);

        var notTraderName = (flags & ~LastTradedFlags.TraderName) == flags;
        if (notTraderName) return SelectLastPaidGivenTradeEntry(pqSourceTickerQuoteInfo);

        return SelectTraderLastTradeEntry(pqSourceTickerQuoteInfo);
    }

    public abstract IMutableLastTrade? ConvertToExpectedImplementation(ILastTrade? checkLastTrade, bool clone = false);

    protected abstract T SelectSimpleLastTradeEntry(Tu sourceTickerQuoteInfo);
    protected abstract T SelectLastPaidGivenTradeEntry(Tu sourceTickerQuoteInfo);
    protected abstract T SelectTraderLastTradeEntry(Tu sourceTickerQuoteInfo);
}
