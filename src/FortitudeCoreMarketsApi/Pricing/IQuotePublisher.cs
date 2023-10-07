using System;
using FortitudeMarketsApi.Pricing.Quotes;

namespace FortitudeMarketsApi.Pricing
{
    public interface IQuotePublisher<in T> : IDisposable where T : ILevel0Quote
    {
        void PublishReset(string ticker, DateTime exchangeTs, DateTime exchangeSentTs, DateTime adapterRecvTs);
        void PublishQuoteUpdate(T quote);
    }
}