namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;

public interface IPQQuotePublicationPrecisionSettings
{
    byte PriceScalingPrecision { get; }
    byte VolumeScalingPrecision { get; }
}
