// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers.LayerOrders;

public class ExternalCounterPartyOrderLayerInfo : AnonymousOrderLayerInfo, IMutableExternalCounterPartyOrderLayerInfo
{
    public ExternalCounterPartyOrderLayerInfo() { }

    public ExternalCounterPartyOrderLayerInfo
    (int orderId, DateTime createdTime, decimal orderDisplayVolume, LayerOrderFlags orderLayerFlags = LayerOrderFlags.None
      , OrderType orderType = OrderType.None, OrderFlags typeFlags = OrderFlags.None
      , OrderLifeCycleState lifeCycleState = OrderLifeCycleState.None
      , string? counterPartyName = null, string? traderName = null, int externalCounterPartyId = 0, int externalTraderId = 0
      , DateTime? updatedTime = null, decimal? remainingVolume = null, uint trackingId = 0)
        : base(orderId, createdTime, orderDisplayVolume, orderLayerFlags, orderType, typeFlags, lifeCycleState, updatedTime, remainingVolume, trackingId)
    {
        ExternalCounterPartyId   = externalCounterPartyId;
        ExternalCounterPartyName = counterPartyName;
        ExternalTraderId         = externalTraderId;
        ExternalTraderName       = traderName;
    }

    public ExternalCounterPartyOrderLayerInfo(IAnonymousOrderLayerInfo toClone) : base(toClone)
    {
        if (toClone is IExternalCounterPartyOrderLayerInfo counterPartyOrderLayer)
        {
            ExternalCounterPartyId   = counterPartyOrderLayer.ExternalCounterPartyId;
            ExternalCounterPartyName = counterPartyOrderLayer.ExternalCounterPartyName;
            ExternalTraderId         = counterPartyOrderLayer.ExternalTraderId;
            ExternalTraderName       = counterPartyOrderLayer.ExternalTraderName;
        }
    }

    public override bool IsEmpty
    {
        get =>
            base.IsEmpty
         && ExternalCounterPartyId == 0
         && ExternalCounterPartyName == null
         && ExternalTraderId == 0
         && ExternalTraderName == null;
        set
        {
            base.IsEmpty = value;
            if (!value) return;
            ExternalCounterPartyId   = 0;
            ExternalCounterPartyName = null;
            ExternalTraderId         = 0;
            ExternalTraderName       = null;
        }
    }

    public int ExternalCounterPartyId { get; set; }

    public string? ExternalCounterPartyName { get; set; }

    public int ExternalTraderId { get; set; }

    public string? ExternalTraderName { get; set; }

    public override void StateReset()
    {
        ExternalCounterPartyId   = 0;
        ExternalCounterPartyName = null;
        ExternalTraderId         = 0;
        ExternalTraderName       = null;
        base.StateReset();
    }


    public override ExternalCounterPartyOrderLayerInfo Clone() =>
        Recycler?.Borrow<ExternalCounterPartyOrderLayerInfo>().CopyFrom(this) ?? new ExternalCounterPartyOrderLayerInfo(this);

    IExternalCounterPartyOrderLayerInfo IExternalCounterPartyOrderLayerInfo.Clone() => Clone();

    IExternalCounterPartyOrderLayerInfo ICloneable<IExternalCounterPartyOrderLayerInfo>.Clone() => Clone();

    IMutableExternalCounterPartyOrderLayerInfo IMutableExternalCounterPartyOrderLayerInfo.Clone() => Clone();

    IMutableExternalCounterPartyOrderLayerInfo ICloneable<IMutableExternalCounterPartyOrderLayerInfo>.Clone() => Clone();

    public override ExternalCounterPartyOrderLayerInfo CopyFrom(IPublishedOrder source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is IExternalCounterPartyOrderLayerInfo counterPartyOrderLayer)
        {
            ExternalCounterPartyId   = counterPartyOrderLayer.ExternalCounterPartyId;
            ExternalCounterPartyName = counterPartyOrderLayer.ExternalCounterPartyName;
            ExternalTraderId         = counterPartyOrderLayer.ExternalTraderId;
            ExternalTraderName       = counterPartyOrderLayer.ExternalTraderName;
        }
        return this;
    }

    public bool AreEquivalent(IExternalCounterPartyOrderLayerInfo? other, bool exactTypes = false) =>
        AreEquivalent((IAnonymousOrderLayerInfo?)other, exactTypes);

    public override bool AreEquivalent(IPublishedOrder? other, bool exactTypes = false)
    {
        var baseSame = base.AreEquivalent(other, exactTypes);

        if (other is not IExternalCounterPartyOrderLayerInfo counterPartyOrderLayer) return baseSame && !exactTypes;

        var counterPartyNameSame = string.Equals(ExternalCounterPartyName, counterPartyOrderLayer.ExternalCounterPartyName);
        var traderNameSame       = string.Equals(ExternalTraderName, counterPartyOrderLayer.ExternalTraderName);

        return baseSame && counterPartyNameSame && traderNameSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IAnonymousOrderLayerInfo?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = ((ExternalCounterPartyName?.GetHashCode() ?? 0) * 397) ^ hashCode;
            hashCode = ((ExternalTraderName?.GetHashCode() ?? 0) * 397) ^ hashCode;
            return hashCode;
        }
    }

    protected string ExternalCounterPartyOrderLayerToStringMembers =>
        $"{AnonymousOrderLayerInfoToStringMembers}, {nameof(ExternalCounterPartyId)}: {ExternalCounterPartyId}," +
        $"{nameof(ExternalCounterPartyName)}: {ExternalCounterPartyName}, {nameof(ExternalTraderId)}: {ExternalTraderId}, " +
        $"{nameof(ExternalTraderName)}: {ExternalTraderName}";

    public override string ToString() => $"{GetType().Name}{{{ExternalCounterPartyOrderLayerToStringMembers}}}";
}
