// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;

#endregion

namespace FortitudeMarkets.Pricing.Generators.Quotes.LayeredBook;

public struct BookGenerationInfo
{
    public BookGenerationInfo() { }

    public LayerType LayerType = LayerType.PriceVolume;

    public decimal Pip = 0.0001m;

    public int VolumeRoundingDp = 2;

    public int PriceRoundingDp = 5;

    public int NumberOfBookLayers = 20;

    public decimal AverageSpreadPips = 0.0001m;

    public decimal AverageLayerPips = 0.1m;

    public int HighestVolumeLayer = 4;

    public double ChangeOnSameProbability = 0.3;

    public long AverageTopOfBookVolume = 300_000;

    public long HighestLayerAverageVolume = 5_000_000;

    public long AverageDeltaVolumePerLayer = 500_000;

    public long MaxVolumeVariance = 4_000_000;

    public decimal MaxPriceLayerPips = 2m;

    public decimal SmallestPriceLayerPips = 0.05m;

    public decimal SpreadStandardDeviation = 0.0005m;

    public decimal TightestSpreadPips = 0.1m;

    public bool AllowEmptySlotGaps = true;

    public GenerateBookLayerInfo GenerateBookLayerInfo = new();
}
