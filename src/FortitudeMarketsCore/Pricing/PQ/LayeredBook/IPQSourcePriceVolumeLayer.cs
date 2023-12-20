#region

using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.DictionaryCompression;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.LayeredBook;

public interface IPQSourcePriceVolumeLayer : IMutableSourcePriceVolumeLayer, IPQPriceVolumeLayer,
    IPQSupportsStringUpdates<IPriceVolumeLayer>
{
    ushort SourceId { get; set; }
    bool IsSourceNameUpdated { get; set; }
    bool IsExecutableUpdated { get; set; }
    IPQNameIdLookupGenerator SourceNameIdLookup { get; set; }

    new IPQSourcePriceVolumeLayer Clone();
}
