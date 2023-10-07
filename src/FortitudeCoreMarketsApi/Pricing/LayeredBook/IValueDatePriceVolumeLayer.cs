using System;
using FortitudeCommon.Types;

namespace FortitudeMarketsApi.Pricing.LayeredBook
{
    public interface IValueDatePriceVolumeLayer : IPriceVolumeLayer, ICloneable<IValueDatePriceVolumeLayer>
    {
        DateTime ValueDate { get; }
        new IValueDatePriceVolumeLayer Clone();
    }
}