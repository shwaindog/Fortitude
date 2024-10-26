// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LayeredBook;

public interface ISourceQuoteRefTraderValueDatePriceVolumeLayer : ISourceQuoteRefPriceVolumeLayer,
    IValueDatePriceVolumeLayer, ITraderPriceVolumeLayer, ICloneable<ISourceQuoteRefTraderValueDatePriceVolumeLayer>
{
    new ISourceQuoteRefTraderValueDatePriceVolumeLayer Clone();
}

public interface IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer : ISourceQuoteRefTraderValueDatePriceVolumeLayer,
    IMutableSourceQuoteRefPriceVolumeLayer, IMutableValueDatePriceVolumeLayer, IMutableTraderPriceVolumeLayer,
    ICloneable<IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer>
{
    new IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer Clone();
}
