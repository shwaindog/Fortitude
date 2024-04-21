#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules.BusMessages;

public class FeedSourceTickerInfoUpdate : RecyclableObject
{
    public string FeedName { get; set; } = null!;
    public IList<ISourceTickerQuoteInfo> FeedSourceTickerQuoteInfos { get; set; } = null!;

    public override void StateReset()
    {
        FeedSourceTickerQuoteInfos = null!;
        FeedName = null!;
        base.StateReset();
    }

    public override string ToString() =>
        $"{GetType().Name}({nameof(FeedName)}: {FeedName}, {nameof(FeedSourceTickerQuoteInfos)}: {FeedSourceTickerQuoteInfos})";
}
