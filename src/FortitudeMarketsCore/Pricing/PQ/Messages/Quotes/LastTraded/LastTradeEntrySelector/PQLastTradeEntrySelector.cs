#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.Quotes.LastTraded;
using FortitudeMarketsCore.Pricing.Quotes.LastTraded.EntrySelector;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LastTraded.LastTradeEntrySelector;

public interface IPQLastTradeTypeSelector : ILastTradeEntryFlagsSelector<IPQRecentlyTradedFactory, IPQSourceTickerQuoteInfo>
    , ISupportsPQNameIdLookupGenerator
{
    bool TypeCanWholeyContain(Type copySourceType, Type copyDestinationType);

    IPQLastTrade? SelectLastTradeEntry(IPQLastTrade? original, IPQNameIdLookupGenerator nameIdLookup, ILastTrade? desired
        , bool keepCloneState = false);
}

public class PQLastTradeEntrySelector : LastTradeEntryFlagsSelector<IPQRecentlyTradedFactory, IPQSourceTickerQuoteInfo>,
    IPQLastTradeTypeSelector
{
    public PQLastTradeEntrySelector(IPQNameIdLookupGenerator nameIdLookup) => NameIdLookup = nameIdLookup;

    INameIdLookup? IHasNameIdLookup.NameIdLookup => NameIdLookup;
    public IPQNameIdLookupGenerator NameIdLookup { get; set; }

    public override IMutableLastTrade? ConvertToExpectedImplementation(ILastTrade? checkLastTrade, bool clone = false) =>
        ConvertToExpectedImplementation(checkLastTrade, NameIdLookup, clone);

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

    public IPQLastTrade? SelectLastTradeEntry(IPQLastTrade? original, IPQNameIdLookupGenerator nameIdLookup, ILastTrade? desired
        , bool keepCloneState = false)
    {
        if (desired == null) return original;
        if (original == null)
        {
            var cloneOfSrc = (IPQLastTrade?)ConvertToExpectedImplementation(desired, nameIdLookup, true);
            if (!keepCloneState) cloneOfSrc?.StateReset();
            return cloneOfSrc;
        }

        if (original.GetType() != desired.GetType() &&
            !TypeCanWholeyContain(desired.GetType(), original.GetType()))
            return new PQLastTraderPaidGivenTrade(original, nameIdLookup);
        return original;
    }

    public IMutableLastTrade? ConvertToExpectedImplementation(ILastTrade? checkLastTrade, IPQNameIdLookupGenerator nameIdLookup, bool clone = false)
    {
        switch (checkLastTrade)
        {
            case null:
                return null;
            case PQLastTraderPaidGivenTrade pqlastTradedPaidGiventTrade:
                return new PQLastTraderPaidGivenTrade(pqlastTradedPaidGiventTrade, nameIdLookup);
            case PQLastTrade pqlastTrade:
                return clone ? ((IPQLastTrade)pqlastTrade).Clone() : pqlastTrade;
            case ILastTraderPaidGivenTrade _:
                return new PQLastTraderPaidGivenTrade(checkLastTrade, nameIdLookup);
            case ILastPaidGivenTrade trdrPvLayer:
                return new PQLastPaidGivenTrade(trdrPvLayer);
            default:
                return new PQLastTrade(checkLastTrade);
        }
    }

    protected override IPQRecentlyTradedFactory SelectSimpleLastTradeEntry(
        IPQSourceTickerQuoteInfo sourceTickerQuoteInfo) =>
        new PQLastTradeFactory();

    protected override IPQRecentlyTradedFactory SelectLastPaidGivenTradeEntry(
        IPQSourceTickerQuoteInfo sourceTickerQuoteInfo) =>
        new PQLastPaidGivenTradeFactory();

    protected override IPQRecentlyTradedFactory SelectTraderLastTradeEntry(
        IPQSourceTickerQuoteInfo sourceTickerQuoteInfo) =>
        SelectTraderLastTradeEntry(sourceTickerQuoteInfo, NameIdLookup);

    protected IPQRecentlyTradedFactory SelectTraderLastTradeEntry(
        IPQSourceTickerQuoteInfo sourceTickerQuoteInfo, IPQNameIdLookupGenerator nameIdLookupGenerator) =>
        new PQLastTraderPaidGivenTradeFactory(nameIdLookupGenerator);
}
