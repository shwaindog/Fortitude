// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

public class PQImplementationFactory : IPQImplementationFactory
{
    private readonly Type lvl1QuoteInterface   = typeof(IPQPublishableLevel1Quote);
    private readonly Type lvl2QuoteInterface   = typeof(IPQPublishableLevel2Quote);
    private readonly Type lvl3QuoteInterface   = typeof(IPQPublishableLevel3Quote);
    private readonly Type tickInstantInterface = typeof(IPQPublishableTickInstant);

    public virtual T GetConcreteMapping<T>(ISourceTickerInfo info) where T : IPQPublishableTickInstant
    {
        var interfaceType = typeof(T);
        if (interfaceType == tickInstantInterface || interfaceType == typeof(PQPublishableTickInstant)) return (T)(object)new PQPublishableTickInstant(info);
        if (interfaceType == lvl1QuoteInterface || interfaceType == typeof(PQPublishableLevel1Quote)) return (T)(object)new PQPublishableLevel1Quote(info);
        if (interfaceType == lvl2QuoteInterface || interfaceType == typeof(PQPublishableLevel2Quote)) return (T)(object)new PQPublishableLevel2Quote(info);
        if (interfaceType == lvl3QuoteInterface || interfaceType == typeof(PQPublishableLevel3Quote)) return (T)(object)new PQPublishableLevel3Quote(info);
        throw new ArgumentOutOfRangeException("Expected any of IPQTickInstant to IPQLevel3Quote");
    }
}
