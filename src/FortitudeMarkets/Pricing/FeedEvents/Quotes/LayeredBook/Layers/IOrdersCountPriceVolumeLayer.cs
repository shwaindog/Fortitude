// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;

public interface IOrdersCountPriceVolumeLayer : IPriceVolumeLayer,
    ICloneable<IOrdersCountPriceVolumeLayer>
{
    uint    OrdersCount    { get; }
    decimal InternalVolume { get; }
    decimal ExternalVolume { get; }

    new IOrdersCountPriceVolumeLayer Clone();
}

public interface IMutableOrdersCountPriceVolumeLayer : IOrdersCountPriceVolumeLayer, IMutablePriceVolumeLayer, ITrackableReset<IMutableOrdersCountPriceVolumeLayer>
{
    new uint    OrdersCount    { get; set; }
    new decimal InternalVolume { get; set; }

    new IMutableOrdersCountPriceVolumeLayer Clone();

    new IMutableOrdersCountPriceVolumeLayer ResetWithTracking();
}