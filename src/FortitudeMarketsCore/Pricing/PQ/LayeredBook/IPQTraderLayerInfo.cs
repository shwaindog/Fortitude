#region

using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.DictionaryCompression;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.LayeredBook;

public interface IPQTraderLayerInfo : IMutableTraderLayerInfo
{
    bool HasUpdates { get; set; }
    int TraderNameId { get; set; }
    bool IsTraderNameUpdated { get; set; }
    bool IsTraderVolumeUpdated { get; set; }
    IPQNameIdLookupGenerator TraderNameIdLookup { get; set; }
    new IPQTraderLayerInfo Clone();
}
