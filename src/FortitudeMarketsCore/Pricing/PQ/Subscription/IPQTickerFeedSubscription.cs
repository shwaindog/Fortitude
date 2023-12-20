#region

using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public interface IPQTickerFeedSubscription
{
    string Source { get; }
    string Ticker { get; }
    ISnapshotUpdatePricingServerConfig Feed { get; }
    void Unsubscribe();
}
