using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;

namespace FortitudeMarketsCore.Pricing.LastTraded.EntrySelector
{
    public class RecentlyTradedLastTradeEntrySelector : LastTradeEntryFlagsSelector<IMutableLastTrade, ISourceTickerQuoteInfo>
    {
        protected override IMutableLastTrade SelectSimpleLastTradeEntry(ISourceTickerQuoteInfo sourceTickerQuoteInfo)
        {
            return new LastTrade();
        }

        protected override IMutableLastTrade SelectLastPaidGivenTradeEntry(ISourceTickerQuoteInfo sourceTickerQuoteInfo)
        {
            return new LastPaidGivenTrade();
        }

        protected override IMutableLastTrade SelectTraderLastTradeEntry(ISourceTickerQuoteInfo sourceTickerQuoteInfo)
        {
            return new LastTraderPaidGivenTrade();
        }

        public override IMutableLastTrade ConvertToExpectedImplementation(ILastTrade checkLastTrade, bool clone = false)
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
}