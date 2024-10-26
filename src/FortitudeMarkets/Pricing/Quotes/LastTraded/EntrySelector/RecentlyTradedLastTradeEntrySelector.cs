// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.Quotes.LastTraded;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LastTraded.EntrySelector;

public class RecentlyTradedLastTradeEntrySelector : LastTradeEntryFlagsSelector<IMutableLastTrade>
{
    protected override IMutableLastTrade SelectSimpleLastTradeEntry() => new LastTrade();

    protected override IMutableLastTrade SelectLastPaidGivenTradeEntry() => new LastPaidGivenTrade();

    protected override IMutableLastTrade SelectTraderLastTradeEntry() => new LastTraderPaidGivenTrade();

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
