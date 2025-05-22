// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded.EntrySelector;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded.LastTradeEntrySelector;

public interface IPQLastTradeTypeSelector : ILastTradeEntryFlagsSelector<IPQRecentlyTradedFactory>
  , ISupportsPQNameIdLookupGenerator
{
    bool TypeCanWhollyContain(Type copySourceType, Type copyDestinationType);

    IPQLastTrade SelectLastTradeEntry
    (IPQLastTrade original, IPQNameIdLookupGenerator nameIdLookup, ILastTrade? desired
      , bool keepCloneState = false);

    IPQLastTrade ConvertToExpectedImplementation(ILastTrade checkLastTrade, IPQNameIdLookupGenerator nameIdLookup, bool clone = false);
}

public class PQLastTradeEntrySelector(IPQNameIdLookupGenerator nameIdLookup) : LastTradeEntryFlagsSelector<IPQRecentlyTradedFactory>,
    IPQLastTradeTypeSelector
{
    INameIdLookup IHasNameIdLookup. NameIdLookup => NameIdLookup;
    public IPQNameIdLookupGenerator NameIdLookup { get; set; } = nameIdLookup;

    public override IMutableLastTrade ConvertToExpectedImplementation(ILastTrade checkLastTrade, bool clone = false) =>
        ConvertToExpectedImplementation(checkLastTrade, NameIdLookup, clone);

    public bool TypeCanWhollyContain(Type copySourceType, Type copyDestinationType)
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

    public IPQLastTrade SelectLastTradeEntry
    (IPQLastTrade original, IPQNameIdLookupGenerator nameIdLookup, ILastTrade? desired
      , bool keepCloneState = false)
    {
        if (desired == null) return original;

        if (original.GetType() != desired.GetType() &&
            !TypeCanWhollyContain(desired.GetType(), original.GetType()))
            return new PQLastTraderPaidGivenTrade(original, nameIdLookup);
        return original;
    }

    public IPQLastTrade ConvertToExpectedImplementation(ILastTrade checkLastTrade, IPQNameIdLookupGenerator nameIdLookup, bool clone = false)
    {
        switch (checkLastTrade)
        {
            case PQLastTraderPaidGivenTrade pqLastTradedPaidGivenTrade:
                return new PQLastTraderPaidGivenTrade(pqLastTradedPaidGivenTrade, nameIdLookup);
            case PQLastTrade pqLastTrade:                return clone ? ((IPQLastTrade)pqLastTrade).Clone() : pqLastTrade;
            case ILastTraderPaidGivenTrade:              return new PQLastTraderPaidGivenTrade(checkLastTrade, nameIdLookup);
            case ILastPaidGivenTrade lastPaidGivenTrade: return new PQLastPaidGivenTrade(lastPaidGivenTrade);
            default:                                     return new PQLastTrade(checkLastTrade);
        }
    }

    protected override IPQRecentlyTradedFactory SelectSimpleLastTradeEntry() => new PQLastTradeFactory();

    protected override IPQRecentlyTradedFactory SelectLastPaidGivenTradeEntry() => new PQLastPaidGivenTradeFactory();

    protected override IPQRecentlyTradedFactory SelectTraderLastTradeEntry() => SelectTraderLastTradeEntry(NameIdLookup);

    protected IPQRecentlyTradedFactory SelectTraderLastTradeEntry(IPQNameIdLookupGenerator nameIdLookupGenerator) =>
        new PQLastTraderPaidGivenTradeFactory(nameIdLookupGenerator);
}
