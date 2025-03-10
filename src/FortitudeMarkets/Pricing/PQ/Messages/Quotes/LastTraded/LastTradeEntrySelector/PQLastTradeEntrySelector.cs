// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LastTraded.EntrySelector;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.LastTraded.LastTradeEntrySelector;

public interface IPQLastTradeTypeSelector : ILastTradeEntryFlagsSelector<IPQRecentlyTradedFactory>
  , ISupportsPQNameIdLookupGenerator
{
    bool TypeCanWholeyContain(Type copySourceType, Type copyDestinationType);

    IPQLastTrade? SelectLastTradeEntry
    (IPQLastTrade? original, IPQNameIdLookupGenerator nameIdLookup, ILastTrade? desired
      , bool keepCloneState = false);
}

public class PQLastTradeEntrySelector : LastTradeEntryFlagsSelector<IPQRecentlyTradedFactory>,
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

    public IPQLastTrade? SelectLastTradeEntry
    (IPQLastTrade? original, IPQNameIdLookupGenerator nameIdLookup, ILastTrade? desired
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
            case null: return null;
            case PQLastTraderPaidGivenTrade pqlastTradedPaidGiventTrade:
                return new PQLastTraderPaidGivenTrade(pqlastTradedPaidGiventTrade, nameIdLookup);
            case PQLastTrade pqlastTrade:         return clone ? ((IPQLastTrade)pqlastTrade).Clone() : pqlastTrade;
            case ILastTraderPaidGivenTrade _:     return new PQLastTraderPaidGivenTrade(checkLastTrade, nameIdLookup);
            case ILastPaidGivenTrade trdrPvLayer: return new PQLastPaidGivenTrade(trdrPvLayer);
            default:                              return new PQLastTrade(checkLastTrade);
        }
    }

    protected override IPQRecentlyTradedFactory SelectSimpleLastTradeEntry() => new PQLastTradeFactory();

    protected override IPQRecentlyTradedFactory SelectLastPaidGivenTradeEntry() => new PQLastPaidGivenTradeFactory();

    protected override IPQRecentlyTradedFactory SelectTraderLastTradeEntry() => SelectTraderLastTradeEntry(NameIdLookup);

    protected IPQRecentlyTradedFactory SelectTraderLastTradeEntry(IPQNameIdLookupGenerator nameIdLookupGenerator) =>
        new PQLastTraderPaidGivenTradeFactory(nameIdLookupGenerator);
}
