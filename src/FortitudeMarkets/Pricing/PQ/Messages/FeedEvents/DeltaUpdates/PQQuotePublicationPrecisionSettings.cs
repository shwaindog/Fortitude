// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

public interface IPQPriceVolumePublicationPrecisionSettings
{
    PQFieldFlags PriceScalingPrecision  { get; }
    PQFieldFlags VolumeScalingPrecision { get; }
}

public class PQPriceVolumePublicationPrecisionSettings : IPQPriceVolumePublicationPrecisionSettings
{
    public PQPriceVolumePublicationPrecisionSettings() { }

    public PQPriceVolumePublicationPrecisionSettings(PQFieldFlags priceScaling, PQFieldFlags volumeScaling)
    {
        PriceScalingPrecision  = priceScaling;
        VolumeScalingPrecision = volumeScaling;
    }

    public PQFieldFlags PriceScalingPrecision  { get; set; }
    public PQFieldFlags VolumeScalingPrecision { get; set; }
}
