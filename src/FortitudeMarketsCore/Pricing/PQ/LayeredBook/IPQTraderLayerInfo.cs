using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.DictionaryCompression;

namespace FortitudeMarketsCore.Pricing.PQ.LayeredBook
{
    public interface IPQTraderLayerInfo : IMutableTraderLayerInfo, IStoreState<ITraderLayerInfo>
    {
        bool HasUpdates { get; set; }
        int TraderNameId { get; set; }
        bool IsTraderNameUpdated { get; set; }
        bool IsTraderVolumeUpdated { get; set; }
        IPQNameIdLookupGenerator TraderNameIdLookup { get; set; }
        new IPQTraderLayerInfo Clone();
    }
}