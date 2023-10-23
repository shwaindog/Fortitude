#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsApi.Pricing.LayeredBook;

public interface ISourcePriceVolumeLayer : IPriceVolumeLayer, ICloneable<ISourcePriceVolumeLayer>
{
    string? SourceName { get; }
    bool Executable { get; }
    new ISourcePriceVolumeLayer Clone();
}
