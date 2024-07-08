// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsApi.Pricing.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

public interface IPQImplementationFactory
{
    T GetConcreteMapping<T>(ISourceTickerQuoteInfo quoteInfo) where T : IPQLevel0Quote;
}
