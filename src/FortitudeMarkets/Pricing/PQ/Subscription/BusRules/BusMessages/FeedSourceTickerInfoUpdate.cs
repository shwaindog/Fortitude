// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Subscription.BusRules.BusMessages;

public class FeedSourceTickerInfoUpdate : RecyclableObject
{
    public string FeedName { get; set; } = null!;

    public List<ISourceTickerInfo> SourceTickerInfos { get; set; } = new();

    public override void StateReset()
    {
        SourceTickerInfos.Clear();
        FeedName = null!;
        base.StateReset();
    }

    public override string ToString() =>
        $"{GetType().Name}({nameof(FeedName)}: {FeedName}, {nameof(SourceTickerInfos)}: \n{string.Join("\n", SourceTickerInfos)})";
}
