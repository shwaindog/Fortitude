namespace FortitudeMarketsCore.Pricing.PQ.DeltaUpdates
{
    public interface IPQQuotePublicationPrecisionSettings
    {
        byte PriceScalingPrecision { get; }
        byte VolumeScalingPrecision { get; }
    }
}