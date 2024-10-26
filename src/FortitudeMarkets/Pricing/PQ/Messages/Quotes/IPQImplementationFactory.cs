﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes;

public interface IPQImplementationFactory
{
    T GetConcreteMapping<T>(ISourceTickerInfo info) where T : IPQTickInstant;
}
