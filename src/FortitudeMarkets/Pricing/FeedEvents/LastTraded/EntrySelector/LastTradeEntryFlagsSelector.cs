// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarkets.Pricing.FeedEvents.LastTraded.EntrySelector;

public interface ILastTradeEntryFlagsSelector<out T> where T : class
{
    T                 FindForLastTradeFlags(LastTradedFlags lastTradedFlags);
    IMutableLastTrade ConvertToExpectedImplementation(ILastTrade checkLastTrade, bool clone = false);
}

public abstract class LastTradeEntryFlagsSelector<T> : ILastTradeEntryFlagsSelector<T> where T : class
{
    public T FindForLastTradeFlags(LastTradedFlags lastTradedFlags)
    {
        var lastTradeType = lastTradedFlags.MostCompactLayerType();

        switch (lastTradeType)
        {
            case LastTradeType.None:
            case LastTradeType.Price:
                return SelectSimpleLastTradeEntry();
            case LastTradeType.PricePaidOrGivenVolume:
                return SelectLastPaidGivenTradeEntry();
            case LastTradeType.PriceLastTraderName:
            case LastTradeType.PriceLastTraderPaidOrGivenVolume:
            default:
                return SelectTraderLastTradeEntry();
        }
    }

    public abstract IMutableLastTrade ConvertToExpectedImplementation(ILastTrade checkLastTrade, bool clone = false);

    protected abstract T SelectSimpleLastTradeEntry();
    protected abstract T SelectLastPaidGivenTradeEntry();
    protected abstract T SelectTraderLastTradeEntry();
}
