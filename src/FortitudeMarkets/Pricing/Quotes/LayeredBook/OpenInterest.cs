// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarkets.Pricing.Quotes.LayeredBook;

public enum MarketDataSource
{
    Venue
  , Adapter
  , Published
}

public readonly struct OpenInterest(MarketDataSource dataSource, decimal volume, decimal vwap)
{
    public MarketDataSource DataSource { get; } = dataSource;

    public decimal Volume { get; } = volume;
    public decimal Vwap   { get; } = vwap;
}
