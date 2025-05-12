// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

public class PQPriceVolumePublicationPrecisionSettingsTests
{
    public static IPQPriceVolumePublicationPrecisionSettings CreatePriceVolumeSettings
        (PQFieldFlags priceScale = (PQFieldFlags)1, PQFieldFlags volumeScale = (PQFieldFlags)6) =>
        new DummyPriceVolumePrecisionSettings(priceScale, volumeScale);

    private class DummyPriceVolumePrecisionSettings
        (PQFieldFlags priceScale = (PQFieldFlags)6, PQFieldFlags volumeScale = (PQFieldFlags)8) : IPQPriceVolumePublicationPrecisionSettings
    {
        public PQFieldFlags PriceScalingPrecision  => priceScale;
        public PQFieldFlags VolumeScalingPrecision => volumeScale;
    }
}
