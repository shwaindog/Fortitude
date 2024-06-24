// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing;

public static class BidAskExtensions
{
    public static BidAsk ToBidAsk(this IPQLevel1Quote level1Quote, IRecycler? recycler = null)
    {
        var bidAsk = (BidAsk)(recycler?.Borrow<BidAsk>().CopyFrom(level1Quote) ?? new BidAsk(level1Quote));
        bidAsk.SequenceNumber = level1Quote.PQSequenceId;
        return bidAsk;
    }
}
