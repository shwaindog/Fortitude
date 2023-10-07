namespace FortitudeMarketsApi.Pricing.LayeredBook;

public interface IMutableTraderPriceVolumeLayer : ITraderPriceVolumeLayer, IMutablePriceVolumeLayer,
    IEnumerable<IMutableTraderLayerInfo>
{
    new IMutableTraderLayerInfo? this[int i] { get; set; }
    void Add(string traderName, decimal size);
    void SetTradersCountOnly(int numberOfTraders);
    bool RemoveAt(int index);
    new IMutableTraderPriceVolumeLayer Clone();
    new IEnumerator<IMutableTraderLayerInfo> GetEnumerator();
}
