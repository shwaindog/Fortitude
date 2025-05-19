// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;

public interface IFullSupportPriceVolumeLayer : ISourceQuoteRefPriceVolumeLayer,
    IValueDatePriceVolumeLayer, IOrdersPriceVolumeLayer, ICloneable<IFullSupportPriceVolumeLayer>
{
    new IFullSupportPriceVolumeLayer Clone();
}

public interface IMutableFullSupportPriceVolumeLayer : IFullSupportPriceVolumeLayer,
    IMutableSourceQuoteRefPriceVolumeLayer, IMutableValueDatePriceVolumeLayer, IMutableOrdersPriceVolumeLayer,
    ICloneable<IMutableFullSupportPriceVolumeLayer>, ITrackableReset<IMutableFullSupportPriceVolumeLayer>
{
    new IMutableFullSupportPriceVolumeLayer Clone();
    new IMutableFullSupportPriceVolumeLayer ResetWithTracking();
}
