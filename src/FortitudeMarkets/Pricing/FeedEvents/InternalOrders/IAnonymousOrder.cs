// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.InternalOrders;

namespace FortitudeMarkets.Pricing.FeedEvents.InternalOrders;

[JsonDerivedType(typeof(AnonymousOrder))]
[JsonDerivedType(typeof(ExternalCounterPartyOrder))]
[JsonDerivedType(typeof(PQAnonymousOrder))]
[JsonDerivedType(typeof(PQAdditionalExternalCounterPartyInfo))]
public interface IAnonymousOrder : IReusableObject<IAnonymousOrder>, IInterfacesComparable<IAnonymousOrder>, IShowsEmpty
{
    public const OrderGenesisFlags AllAdditionalInfoFlags = 
        IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags | IInternalPassiveOrder.HasInternalOrderInfo;

    public const OrderGenesisFlags AllExceptExtraInfoFlags = ~AllAdditionalInfoFlags;

    int OrderId { get; }

    OrderType OrderType { get; }

    OrderLifeCycleState OrderLifeCycleState { get; }

    OrderGenesisFlags GenesisFlags { get; }
    uint       TrackingId     { get; }

    DateTime CreatedTime { get; }
    DateTime UpdateTime  { get; }

    decimal OrderDisplayVolume          { get; }
    decimal OrderRemainingVolume { get; }

    IAdditionalInternalPassiveOrderInfo?      InternalOrderInfo             { get; }
    IAdditionalExternalCounterPartyOrderInfo? ExternalCounterPartyOrderInfo { get; }

    OrderGenesisFlags EmptyIgnoreGenesisFlags { get; }

    IInternalPassiveOrder? ToInternalOrder();

    IExternalCounterPartyOrder? ToExternalCounterPartyInfoOrder();
}

public interface IMutableAnonymousOrder : IReusableObject<IMutableAnonymousOrder>, IAnonymousOrder, IEmptyable, ITrackableReset<IMutableAnonymousOrder>
{
    new int OrderId { get; set; }

    new OrderType           OrderType           { get; set; }
    new OrderLifeCycleState OrderLifeCycleState { get; set; }
    new OrderGenesisFlags          GenesisFlags           { get; set; }

    new uint TrackingId { get; set; }

    new DateTime CreatedTime { get; set; }
    new DateTime UpdateTime  { get; set; }

    new decimal OrderDisplayVolume          { get; set; }
    new decimal OrderRemainingVolume { get; set; }

    new OrderGenesisFlags EmptyIgnoreGenesisFlags { get; set; }

    new IMutableAdditionalInternalPassiveOrderInfo?      InternalOrderInfo             { get; set; }
    new IMutableAdditionalExternalCounterPartyOrderInfo? ExternalCounterPartyOrderInfo { get; set; }

    new IMutableAnonymousOrder Clone();


}
