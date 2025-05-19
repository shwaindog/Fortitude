// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

namespace FortitudeMarkets.Pricing.FeedEvents.InternalOrders;

public interface IPublishedOrder : IReusableObject<IPublishedOrder>, IInterfacesComparable<IPublishedOrder>
{
    int OrderId { get; }

    OrderType OrderType { get; }

    OrderLifeCycleState OrderLifeCycleState { get; }

    OrderFlags TypeFlags { get; }
    uint       TrackingId     { get; }

    DateTime CreatedTime { get; }
    DateTime UpdateTime  { get; }

    decimal OrderDisplayVolume          { get; }
    decimal OrderRemainingVolume { get; }

    IInternalPassiveOrder? ToInternalOrder();

    IExternalCounterPartyInfoOrder? ToExternalCounterPartyInfoOrder();
}

public interface IMutablePublishedOrder : IPublishedOrder
{
    new int OrderId { get; set; }

    new OrderType           OrderType           { get; set; }
    new OrderLifeCycleState OrderLifeCycleState { get; set; }
    new OrderFlags          TypeFlags           { get; set; }

    new uint TrackingId { get; set; }

    new DateTime CreatedTime { get; set; }
    new DateTime UpdateTime  { get; set; }

    new decimal OrderDisplayVolume          { get; set; }
    new decimal OrderRemainingVolume { get; set; }
}
