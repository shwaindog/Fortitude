#region

using FortitudeMarketsApi.Pricing.Quotes;

#endregion

namespace FortitudeMarketsApi.Pricing;

public interface IQuotePublisher<in T> : IDisposable where T : ILevel0Quote
{
    void PublishReset(string ticker, DateTime exchangeTs, DateTime exchangeSentTs, DateTime adapterRecvTs);
    void PublishQuoteUpdate(T quote);
}
