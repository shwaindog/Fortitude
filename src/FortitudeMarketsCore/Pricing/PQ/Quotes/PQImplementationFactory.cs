using System;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;

namespace FortitudeMarketsCore.Pricing.PQ.Quotes
{
    public class PQImplementationFactory : IPQImplementationFactory
    {
        private readonly Type lvl0QuoteInterface = typeof(IPQLevel0Quote);
        private readonly Type lvl1QuoteInterface = typeof(IPQLevel1Quote);
        private readonly Type lvl2QuoteInterface = typeof(IPQLevel2Quote);
        private readonly Type lvl3QuoteInterface = typeof(IPQLevel3Quote);
        public virtual T GetConcreteMapping<T>(ISourceTickerQuoteInfo quoteInfo) where T : IPQLevel0Quote
        {
            var interfaceType = typeof(T);
            if (interfaceType == lvl0QuoteInterface)
            {
                return (T)(object) new PQLevel0Quote(quoteInfo);
            }
            if (interfaceType == lvl1QuoteInterface)
            {
                return (T)(object)new PQLevel1Quote(quoteInfo);
            }
            if (interfaceType == lvl2QuoteInterface)
            {
                return (T)(object)new PQLevel2Quote(quoteInfo);
            }
            if (interfaceType == lvl3QuoteInterface)
            {
                return (T)(object)new PQLevel3Quote(quoteInfo);
            }
            throw new ArgumentOutOfRangeException("Expected a IPQLevel0Quote - IPQLevel3Quote");
        }
    }
}
