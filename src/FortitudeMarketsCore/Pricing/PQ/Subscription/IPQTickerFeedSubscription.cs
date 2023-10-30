using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

namespace FortitudeMarketsCore.Pricing.PQ.Subscription
{
    public interface IPQTickerFeedSubscription
    {
        string Source { get; }
        string Ticker { get; }
        ISnapshotUpdatePricingServerConfig Feed { get; }
        void Unsubscribe();
    }
}