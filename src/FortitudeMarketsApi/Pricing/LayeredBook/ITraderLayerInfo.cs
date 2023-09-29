using FortitudeCommon.Types;

namespace FortitudeMarketsApi.Pricing.LayeredBook
{
    public interface ITraderLayerInfo : ICloneable<ITraderLayerInfo>, IInterfacesComparable<ITraderLayerInfo>
    {
        string TraderName { get; }
        decimal TraderVolume { get; }
        bool IsEmpty { get; }
    }
}