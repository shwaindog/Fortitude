// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarkets.Pricing.Generators.Quotes.LayeredBook;

public struct GenerateBookLayerInfo
{
    public GenerateBookLayerInfo() { }

    public int MaxNumberOfSourceNames = 12;

    public int[] CandidateValueDateAddDays = [0, 1, 2, 7, 14, 21, 28, 30, 60, 90];

    public bool IsTraderCountOnly = false;

    public int MaxNumberOfUniqueTraderName = 100;

    public int AverageTradersPerLayer = 3;

    public int TradersPerLayerStandardDeviation = 15;

    public double ExecutableProbability = 0.99;
}
