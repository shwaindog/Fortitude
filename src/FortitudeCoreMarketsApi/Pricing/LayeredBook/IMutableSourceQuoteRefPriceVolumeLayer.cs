using FortitudeCommon.Types;

namespace FortitudeMarketsApi.Pricing.LayeredBook
{
    public interface IMutableSourceQuoteRefPriceVolumeLayer : IMutableSourcePriceVolumeLayer, 
        ISourceQuoteRefPriceVolumeLayer, ICloneable<IMutableSourceQuoteRefPriceVolumeLayer>
    {
        new uint SourceQuoteReference { get; set; }
        new IMutableSourceQuoteRefPriceVolumeLayer Clone();
    }
}