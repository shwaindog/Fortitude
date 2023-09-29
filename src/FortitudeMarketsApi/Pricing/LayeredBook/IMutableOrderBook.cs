using FortitudeCommon.Types;

namespace FortitudeMarketsApi.Pricing.LayeredBook
{
    public interface IMutableOrderBook : IOrderBook, ICloneable<IMutableOrderBook>, IStoreState<IOrderBook>
    {
        new int Capacity { get; set; }
        new IMutablePriceVolumeLayer this[int level] { get; set; }
        void Reset();
        new IMutableOrderBook Clone();
    }
}