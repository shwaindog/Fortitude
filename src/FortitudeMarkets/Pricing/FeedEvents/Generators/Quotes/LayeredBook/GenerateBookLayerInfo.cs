// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarkets.Pricing.Generators.Quotes.LayeredBook;

public struct GenerateBookLayerInfo
{
    public GenerateBookLayerInfo() { }

    public int MaxNumberOfSourceNames = 12;

    public int[] CandidateValueDateAddDays = [0, 1, 2, 7, 14, 21, 28, 30, 60, 90];

    public int MaxNumberOfUniqueTraderNames = 100;

    public int MaxNumberOfUniqueCounterPartyNames = 100;

    public int MinOrdersPerLayer = 1;

    public int AverageOrdersPerLayer = 9;

    public int MaxOrdersPerLayer = 20;


    public int OrdersCountPerLayerStandardDeviation = 3;

    public double OrderIsInternalProbability = 0.05;

    public double ExecutableProbability = 0.99;
}
