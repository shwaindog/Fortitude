// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;

public interface IOrdersPriceVolumeLayer : IOrdersCountPriceVolumeLayer, ITracksShiftsList<IAnonymousOrder, IAnonymousOrder>,
    ICloneable<IOrdersPriceVolumeLayer>
{
    IReadOnlyList<IAnonymousOrder> Orders { get; }

    QuoteLayerInstantBehaviorFlags LayerBehavior { get; }

    new IOrdersPriceVolumeLayer    Clone();
}

public interface IMutableOrdersPriceVolumeLayer : IOrdersPriceVolumeLayer, IMutableOrdersCountPriceVolumeLayer
  , ITrackableReset<IMutableOrdersPriceVolumeLayer>, IMutableTracksReorderingList<IMutableAnonymousOrder, IAnonymousOrder>
{
    new IMutableAnonymousOrder this[int i] { get; set; }

    new QuoteLayerInstantBehaviorFlags LayerBehavior { get; set; }

    new int Count { get; set; }

    new int Capacity { get; set; }

    new IReadOnlyList<ListShiftCommand> ShiftCommands { get; set; }

    new int? ClearRemainingElementsFromIndex { get; set; }

    new bool HasUnreliableListTracking { get; set; }

    new ushort MaxAllowedSize { get; set; }

    new uint    OrdersCount    { get; }

    new decimal InternalVolume { get; }

    new IReadOnlyList<IMutableAnonymousOrder> Orders { get; }

    new bool CalculateShift(DateTime asAtTime, IReadOnlyList<IAnonymousOrder> updatedCollection);

    new IEnumerator<IMutableAnonymousOrder> GetEnumerator();

    string EachOrderByIndexOnNewLines();

    new IMutableOrdersPriceVolumeLayer Clone();
    new IMutableOrdersPriceVolumeLayer ResetWithTracking();
}
