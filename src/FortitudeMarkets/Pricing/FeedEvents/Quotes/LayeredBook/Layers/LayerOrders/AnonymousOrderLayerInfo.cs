// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers.LayerOrders;

public class AnonymousOrderLayerInfo : PublishedOrder, IMutableAnonymousOrderLayerInfo
{
    public AnonymousOrderLayerInfo() { }

    public AnonymousOrderLayerInfo
    (int orderId, DateTime createdTime, decimal orderDisplayVolume, LayerOrderFlags orderLayerFlags = LayerOrderFlags.None
      , OrderType orderType = OrderType.None, OrderFlags typeFlags = OrderFlags.None
      , OrderLifeCycleState lifeCycleState = OrderLifeCycleState.None
      , DateTime? updatedTime = null, decimal? remainingVolume = null, uint trackingId = 0)
        : base(orderId, createdTime, orderDisplayVolume, orderType, typeFlags, lifeCycleState, updatedTime, remainingVolume, trackingId)
    {
        OrderLayerFlags = orderLayerFlags;
    }

    public AnonymousOrderLayerInfo(IAnonymousOrderLayerInfo toClone)
    {
        OrderId              = toClone.OrderId;
        OrderLayerFlags      = toClone.OrderLayerFlags;
        OrderType            = toClone.OrderType;
        TypeFlags            = toClone.TypeFlags;
        OrderLifeCycleState  = toClone.OrderLifeCycleState;
        CreatedTime          = toClone.CreatedTime;
        OrderDisplayVolume   = toClone.OrderDisplayVolume;
        OrderRemainingVolume = toClone.OrderRemainingVolume;
        UpdateTime           = toClone.UpdateTime;
        TrackingId = toClone.TrackingId;
    }

    public LayerOrderFlags OrderLayerFlags { get; set; }

    public override bool IsEmpty
    {
        get => base.IsEmpty && OrderLayerFlags == LayerOrderFlags.None;
        set
        {
            base.IsEmpty = value;
            if (!value) return;
            OrderLayerFlags = LayerOrderFlags.None;
        }
    }

    public override void StateReset()
    {
        OrderLayerFlags = LayerOrderFlags.None;
        base.StateReset();
    }

    public override AnonymousOrderLayerInfo CopyFrom(IPublishedOrder source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is IAnonymousOrderLayerInfo anonymousOrderLayer)
        {
            OrderLayerFlags = anonymousOrderLayer.OrderLayerFlags;
        }
        return this;
    }

    IInternalPassiveOrderLayerInfo? IAnonymousOrderLayerInfo.ToInternalOrder() => ToInternalOrder();

    IMutableInternalPassiveOrderLayerInfo? IMutableAnonymousOrderLayerInfo.ToInternalOrder() => ToInternalOrder();

    public override InternalPassiveOrderLayerInfo? ToInternalOrder() =>
        this is InternalPassiveOrderLayerInfo internalOrder && TypeFlags.IsInternalOrder() && TypeFlags.HasInternalOrderInfo()
            ? internalOrder
            : null;

    IExternalCounterPartyOrderLayerInfo? IAnonymousOrderLayerInfo.ToExternalCounterPartyInfoOrder() => ToExternalCounterPartyInfoOrder();

    IMutableExternalCounterPartyOrderLayerInfo? IMutableAnonymousOrderLayerInfo.ToExternalCounterPartyInfoOrder() =>
        ToExternalCounterPartyInfoOrder();

    public override ExternalCounterPartyOrderLayerInfo? ToExternalCounterPartyInfoOrder() =>
        this is ExternalCounterPartyOrderLayerInfo externalCounterPartyOrder && TypeFlags.IsExternalOrder() && TypeFlags.HasExternalCounterPartyInfo()
            ? externalCounterPartyOrder
            : null;

    object ICloneable.Clone() => Clone();

    IMutableAnonymousOrderLayerInfo IMutableAnonymousOrderLayerInfo.Clone() => Clone();

    IAnonymousOrderLayerInfo IAnonymousOrderLayerInfo.Clone() => Clone();

    public override AnonymousOrderLayerInfo Clone() =>
        Recycler?.Borrow<AnonymousOrderLayerInfo>().CopyFrom(this) ?? new AnonymousOrderLayerInfo(this);

    public bool AreEquivalent(IMutableAnonymousOrderLayerInfo? other, bool exactTypes = false) =>
        AreEquivalent((IAnonymousOrderLayerInfo?)other, exactTypes);

    public bool AreEquivalent(IAnonymousOrderLayerInfo? other, bool exactTypes = false) => AreEquivalent(other as IPublishedOrder, exactTypes);

    public override bool AreEquivalent(IPublishedOrder? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (other is not IAnonymousOrderLayerInfo anonymousOrder) return false;

        var baseSame       = base.AreEquivalent(anonymousOrder, exactTypes);
        var orderFlagsSame = OrderLayerFlags == anonymousOrder.OrderLayerFlags;

        return orderFlagsSame && baseSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IAnonymousOrderLayerInfo?)obj, true);


    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = ((int)OrderLayerFlags * 397) ^ hashCode;
            return hashCode;
        }
    }

    protected string AnonymousOrderLayerInfoToStringMembers => $"{PublishedOrderToStringMembers}, {nameof(OrderLayerFlags)}: {OrderLayerFlags}";

    public override string ToString() => $"{GetType().Name}{{{AnonymousOrderLayerInfoToStringMembers}}}";
}
