#region

using FortitudeMarketsApi.Pricing.LayeredBook;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.LayeredBook;

public interface IPQSourceQuoteRefPriceVolumeLayer : IMutableSourceQuoteRefPriceVolumeLayer,
    IPQSourcePriceVolumeLayer

{
    bool IsSourceQuoteReferenceUpdated { get; set; }
    new IPQSourceQuoteRefPriceVolumeLayer Clone();
}
