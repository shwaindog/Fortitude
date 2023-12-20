#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsApi.Pricing.LayeredBook;

public interface ISourceQuoteRefPriceVolumeLayer : ISourcePriceVolumeLayer,
    ICloneable<ISourceQuoteRefPriceVolumeLayer>
{
    uint SourceQuoteReference { get; }
    new ISourceQuoteRefPriceVolumeLayer Clone();
}
