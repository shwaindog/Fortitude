#region

using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsCore.Pricing.LastTraded;
using FortitudeMarketsCore.Pricing.LastTraded.EntrySelector;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.LastTraded.LastTradeEntrySelector;

public class PQLastTradeEntrySelector : LastTradeEntryFlagsSelector<IPQRecentlyTradedFactory, IPQSourceTickerQuoteInfo>,
    IPQLastTradeTypeSelector
{
    public override IMutableLastTrade? ConvertToExpectedImplementation(ILastTrade? checkLastTrade, bool clone = false)
    {
        switch (checkLastTrade)
        {
            case null:
                return null;
            case PQLastTrade pqlastTrade:
                return clone ? ((IPQLastTrade)pqlastTrade).Clone() : pqlastTrade;
            case ILastTraderPaidGivenTrade _:
                return new PQLastTraderPaidGivenTrade(checkLastTrade);
            case ILastPaidGivenTrade trdrPvLayer:
                return new PQLastPaidGivenTrade(trdrPvLayer);
            default:
                return new PQLastTrade(checkLastTrade);
        }
    }

    public bool TypeCanWholeyContain(Type copySourceType, Type copyDestinationType)
    {
        if (copySourceType == typeof(PQLastTrade) || copySourceType == typeof(LastTrade)) return true;
        if (copySourceType == typeof(LastPaidGivenTrade) ||
            copySourceType == typeof(PQLastPaidGivenTrade))
            return copyDestinationType == typeof(PQLastPaidGivenTrade) ||
                   copyDestinationType == typeof(PQLastTraderPaidGivenTrade);
        if (copySourceType == typeof(LastTraderPaidGivenTrade) ||
            copySourceType == typeof(PQLastTraderPaidGivenTrade))
            return copyDestinationType == typeof(PQLastTraderPaidGivenTrade);
        return false;
    }

    public IPQLastTrade? SelectLastTradeEntry(IPQLastTrade? original, ILastTrade? desired)
    {
        if (desired == null) return original;
        if (original == null)
        {
            var cloneOfSrc = (IPQLastTrade?)ConvertToExpectedImplementation(desired)?.Clone();
            cloneOfSrc?.Reset();
            return cloneOfSrc;
        }

        if (original.GetType() != desired.GetType() &&
            !TypeCanWholeyContain(desired.GetType(), original.GetType()))
            return new PQLastTraderPaidGivenTrade(original);
        return original;
    }

    protected override IPQRecentlyTradedFactory SelectSimpleLastTradeEntry(
        IPQSourceTickerQuoteInfo sourceTickerQuoteInfo) =>
        new PQLastTradeFactory();

    protected override IPQRecentlyTradedFactory SelectLastPaidGivenTradeEntry(
        IPQSourceTickerQuoteInfo sourceTickerQuoteInfo) =>
        new PQLastPaidGivenTradeFactory();

    protected override IPQRecentlyTradedFactory SelectTraderLastTradeEntry(
        IPQSourceTickerQuoteInfo sourceTickerQuoteInfo)
    {
        var traderNameIdLookup = sourceTickerQuoteInfo.TraderNameIdLookup ??
                                 new PQNameIdLookupGenerator(PQFieldKeys.LastTraderDictionaryUpsertCommand,
                                     PQFieldFlags.TraderNameIdLookupSubDictionaryKey);
        return new PQLastTraderPaidGivenTradeFactory(traderNameIdLookup);
    }
}
