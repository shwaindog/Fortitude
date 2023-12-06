namespace FortitudeMarketsApi.Pricing.LayeredBook;

public interface IMutableTraderLayerInfo : ITraderLayerInfo
{
    new string? TraderName { get; set; }
    new decimal TraderVolume { get; set; }
    new IMutableTraderLayerInfo Clone();
}
