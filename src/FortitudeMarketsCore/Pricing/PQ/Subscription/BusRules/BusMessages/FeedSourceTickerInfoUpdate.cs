#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

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
