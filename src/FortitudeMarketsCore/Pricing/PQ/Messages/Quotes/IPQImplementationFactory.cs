#region

using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

public interface IPQImplementationFactory
{
    T GetConcreteMapping<T>(ISourceTickerQuoteInfo quoteInfo) where T : IPQLevel0Quote;
}
