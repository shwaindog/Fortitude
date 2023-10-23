namespace FortitudeMarketsApi.Pricing.LayeredBook;

public interface IMutableSourcePriceVolumeLayer : ISourcePriceVolumeLayer, IMutablePriceVolumeLayer
{
    new string? SourceName { get; set; }
    new bool Executable { get; set; }
    new IMutableSourcePriceVolumeLayer Clone();
}
