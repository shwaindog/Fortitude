// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarkets.Pricing.FeedEvents.LastTraded.EntrySelector;

public class LastTradedLastTradeEntrySelector : LastTradeEntryFlagsSelector<IMutableLastTrade>
{
    protected override IMutableLastTrade SelectSimpleLastTradeEntry() => new LastTrade();

    protected override IMutableLastTrade SelectLastPaidGivenTradeEntry() => new LastPaidGivenTrade();

    protected override IMutableLastTrade SelectTraderLastTradeEntry() => new LastExternalCounterPartyTrade();

    public override IMutableLastTrade ConvertToExpectedImplementation(ILastTrade checkLastTrade, bool clone = false)
    {
        switch (checkLastTrade)
        {
            case LastTrade pqlastTrade:
                return clone ? ((IMutableLastTrade)pqlastTrade).Clone() : pqlastTrade;
            case ILastExternalCounterPartyTrade _:
                return new LastExternalCounterPartyTrade(checkLastTrade);
            case ILastPaidGivenTrade trdrPvLayer:
                return new LastPaidGivenTrade(trdrPvLayer);
            default:
                return new LastTrade(checkLastTrade);
        }
    }
}
