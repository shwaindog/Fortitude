// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarkets.Pricing.Quotes.LayeredBook;

public readonly struct VwapResult(int layerDepth, decimal volumeRequested, decimal volumeAchieved, decimal achievedVwap)
{
    private int     LayerDepth      { get; } = layerDepth;
    public  decimal VolumeRequested { get; } = volumeRequested;
    public  decimal VolumeAchieved  { get; } = volumeAchieved;
    public  decimal AchievedVwap    { get; } = achievedVwap;
}
