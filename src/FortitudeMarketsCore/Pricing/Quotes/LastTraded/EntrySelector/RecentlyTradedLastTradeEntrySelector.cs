#region

using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;

#endregion

namespace FortitudeMarketsCore.Pricing.Quotes.LastTraded.EntrySelector;

public class RecentlyTradedLastTradeEntrySelector : LastTradeEntryFlagsSelector<IMutableLastTrade, ISourceTickerQuoteInfo>
{
    protected override IMutableLastTrade SelectSimpleLastTradeEntry(ISourceTickerQuoteInfo sourceTickerQuoteInfo) => new LastTrade();

    protected override IMutableLastTrade SelectLastPaidGivenTradeEntry(ISourceTickerQuoteInfo sourceTickerQuoteInfo) => new LastPaidGivenTrade();

    protected override IMutableLastTrade SelectTraderLastTradeEntry(ISourceTickerQuoteInfo sourceTickerQuoteInfo) => new LastTraderPaidGivenTrade();

    public override IMutableLastTrade? ConvertToExpectedImplementation(ILastTrade? checkLastTrade, bool clone = false)
    {
        switch (checkLastTrade)
        {
            case null:
                return null;
            case LastTrade pqlastTrade:
                return clone ? ((IMutableLastTrade)pqlastTrade).Clone() : pqlastTrade;
            case ILastTraderPaidGivenTrade _:
                return new LastTraderPaidGivenTrade(checkLastTrade);
            case ILastPaidGivenTrade trdrPvLayer:
                return new LastPaidGivenTrade(trdrPvLayer);
            default:
                return new LastTrade(checkLastTrade);
        }
    }
}
