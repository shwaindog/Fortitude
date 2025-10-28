// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

#endregion

namespace FortitudeMarkets.Pricing;

public static class BidAskExtensions
{
    public static BidAskInstant ToBidAsk(this IPQPublishableLevel1Quote level1Quote, IRecycler? recycler = null)
    {
        var bidAsk = (BidAskInstant)(recycler?.Borrow<BidAskInstant>().CopyFrom(level1Quote) ?? new BidAskInstant(level1Quote));
        return bidAsk;
    }

    public static ValidRangeBidAskPeriod ToValidRangeBidAskPeriod(this IPQPublishableLevel1Quote level1Quote, IRecycler? recycler = null)
    {
        var bidAsk = (ValidRangeBidAskPeriod)(recycler?.Borrow<ValidRangeBidAskPeriod>().CopyFrom(level1Quote) ??
                                              new ValidRangeBidAskPeriod(level1Quote));
        return bidAsk;
    }
}
