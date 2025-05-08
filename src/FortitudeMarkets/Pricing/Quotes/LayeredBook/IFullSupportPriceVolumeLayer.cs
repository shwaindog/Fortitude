// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LayeredBook;

public interface IFullSupportPriceVolumeLayer : ISourceQuoteRefPriceVolumeLayer,
    IValueDatePriceVolumeLayer, IOrdersPriceVolumeLayer, ICloneable<IFullSupportPriceVolumeLayer>
{
    new IFullSupportPriceVolumeLayer Clone();
}

public interface IMutableFullSupportPriceVolumeLayer : IFullSupportPriceVolumeLayer,
    IMutableSourceQuoteRefPriceVolumeLayer, IMutableValueDatePriceVolumeLayer, IMutableOrdersPriceVolumeLayer,
    ICloneable<IMutableFullSupportPriceVolumeLayer>
{
    new IMutableFullSupportPriceVolumeLayer Clone();
}
