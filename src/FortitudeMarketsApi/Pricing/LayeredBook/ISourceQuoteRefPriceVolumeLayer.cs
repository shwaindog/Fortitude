using FortitudeCommon.Types;

namespace FortitudeMarketsApi.Pricing.LayeredBook
{
    public interface ISourceQuoteRefPriceVolumeLayer : ISourcePriceVolumeLayer, 
        ICloneable<ISourceQuoteRefPriceVolumeLayer>
    {
        uint SourceQuoteReference { get; }
        new ISourceQuoteRefPriceVolumeLayer Clone();
    }
}