using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;

namespace FortitudeMarketsCore.Pricing.PQ.Subscription
{
    public abstract class PQTickerFeedSubscription : IPQTickerFeedSubscription
    {
        private readonly ISourceTickerQuoteInfo sourceTickerQuoteInfo;

        protected PQTickerFeedSubscription(ISnapshotUpdatePricingServerConfig feedServerConfig, 
            ISourceTickerQuoteInfo sourceTickerQuoteInfo)
        {
            Feed = feedServerConfig;
            this.sourceTickerQuoteInfo = sourceTickerQuoteInfo;
        }

        public string Source => sourceTickerQuoteInfo.Source;
        public string Ticker => sourceTickerQuoteInfo.Ticker;
        public ISnapshotUpdatePricingServerConfig Feed { get; }

        public abstract void Unsubscribe();

        public override int GetHashCode()
        {
            var hash = 13;
            hash = hash * 7 + Source.GetHashCode();
            hash = hash * 7 + Ticker.GetHashCode();
            return hash;
        }

        public override bool Equals(object obj)
        {
            return obj is PQTickerFeedSubscription sub && Source == sub.Source && Ticker == sub.Ticker;
        }

        public override string ToString()
        {
            return $"PQTickerFeedSubscription {Source}-{Ticker}";
        }
    }
}