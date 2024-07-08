// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeMarketsApi.Pricing.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules.BusMessages;

public class FeedSourceTickerInfoUpdate : RecyclableObject
{
    public string FeedName { get; set; } = null!;

    public List<ISourceTickerQuoteInfo> SourceTickerQuoteInfos { get; set; } = new();

    public override void StateReset()
    {
        SourceTickerQuoteInfos.Clear();
        FeedName = null!;
        base.StateReset();
    }

    public override string ToString() =>
        $"{GetType().Name}({nameof(FeedName)}: {FeedName}, {nameof(SourceTickerQuoteInfos)}: \n{string.Join("\n", SourceTickerQuoteInfos)})";
}
