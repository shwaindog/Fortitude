// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;

public interface IPQPriceVolumePublicationPrecisionSettings
{
    byte PriceScalingPrecision  { get; }
    byte VolumeScalingPrecision { get; }
}

public class PQPriceVolumePublicationPrecisionSettings : IPQPriceVolumePublicationPrecisionSettings
{
    public PQPriceVolumePublicationPrecisionSettings() { }

    public PQPriceVolumePublicationPrecisionSettings(byte priceScaling, byte volumeScaling)
    {
        PriceScalingPrecision  = priceScaling;
        VolumeScalingPrecision = volumeScaling;
    }

    public byte PriceScalingPrecision  { get; set; }
    public byte VolumeScalingPrecision { get; set; }
}
