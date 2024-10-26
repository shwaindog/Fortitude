// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes;

public class PQImplementationFactory : IPQImplementationFactory
{
    private readonly Type lvl1QuoteInterface   = typeof(IPQLevel1Quote);
    private readonly Type lvl2QuoteInterface   = typeof(IPQLevel2Quote);
    private readonly Type lvl3QuoteInterface   = typeof(IPQLevel3Quote);
    private readonly Type tickInstantInterface = typeof(IPQTickInstant);

    public virtual T GetConcreteMapping<T>(ISourceTickerInfo info) where T : IPQTickInstant
    {
        var interfaceType = typeof(T);
        if (interfaceType == tickInstantInterface || interfaceType == typeof(PQTickInstant)) return (T)(object)new PQTickInstant(info);
        if (interfaceType == lvl1QuoteInterface || interfaceType == typeof(PQLevel1Quote)) return (T)(object)new PQLevel1Quote(info);
        if (interfaceType == lvl2QuoteInterface || interfaceType == typeof(PQLevel2Quote)) return (T)(object)new PQLevel2Quote(info);
        if (interfaceType == lvl3QuoteInterface || interfaceType == typeof(PQLevel3Quote)) return (T)(object)new PQLevel3Quote(info);
        throw new ArgumentOutOfRangeException("Expected any of IPQTickInstant to IPQLevel3Quote");
    }
}
