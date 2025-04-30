// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LayeredBook;

public interface ISourceQuoteRefOrdersValueDatePriceVolumeLayer : ISourceQuoteRefPriceVolumeLayer,
    IValueDatePriceVolumeLayer, IOrdersPriceVolumeLayer, ICloneable<ISourceQuoteRefOrdersValueDatePriceVolumeLayer>
{
    new ISourceQuoteRefOrdersValueDatePriceVolumeLayer Clone();
}

public interface IMutableSourceQuoteRefOrdersValueDatePriceVolumeLayer : ISourceQuoteRefOrdersValueDatePriceVolumeLayer,
    IMutableSourceQuoteRefPriceVolumeLayer, IMutableValueDatePriceVolumeLayer, IMutableOrdersPriceVolumeLayer,
    ICloneable<IMutableSourceQuoteRefOrdersValueDatePriceVolumeLayer>
{
    new IMutableSourceQuoteRefOrdersValueDatePriceVolumeLayer Clone();
}
