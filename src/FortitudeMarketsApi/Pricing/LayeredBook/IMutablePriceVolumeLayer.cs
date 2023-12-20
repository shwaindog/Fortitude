namespace FortitudeMarketsApi.Pricing.LayeredBook;

public interface IMutablePriceVolumeLayer : IPriceVolumeLayer
{
    new decimal Price { get; set; }
    new decimal Volume { get; set; }
}
