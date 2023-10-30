using System;
using FortitudeCommon.Types;

namespace FortitudeMarketsApi.Pricing.LayeredBook
{
    public interface IMutableValueDatePriceVolumeLayer : IMutablePriceVolumeLayer, IValueDatePriceVolumeLayer,
        ICloneable<IMutableValueDatePriceVolumeLayer>
    {
        new DateTime ValueDate { get; set; }
        new IMutableValueDatePriceVolumeLayer Clone();
    }
}