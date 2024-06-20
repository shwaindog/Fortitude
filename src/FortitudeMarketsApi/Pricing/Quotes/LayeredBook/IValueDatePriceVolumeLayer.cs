// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsApi.Pricing.Quotes.LayeredBook;

public interface IValueDatePriceVolumeLayer : IPriceVolumeLayer, ICloneable<IValueDatePriceVolumeLayer>
{
    DateTime                       ValueDate { get; }
    new IValueDatePriceVolumeLayer Clone();
}

public interface IMutableValueDatePriceVolumeLayer : IMutablePriceVolumeLayer, IValueDatePriceVolumeLayer,
    ICloneable<IMutableValueDatePriceVolumeLayer>
{
    new DateTime ValueDate { get; set; }

    new IMutableValueDatePriceVolumeLayer Clone();
}
