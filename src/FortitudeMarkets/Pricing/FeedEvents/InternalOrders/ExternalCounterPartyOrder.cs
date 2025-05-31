// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.InternalOrders;

public class ExternalCounterPartyOrder : ReusableObject<IAnonymousOrder>, IMutableExternalCounterPartyOrder
{
    private const    OrderGenesisFlags      HasCpFlags = IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags;
    private readonly IMutableAnonymousOrder owner;

    private IMutableAdditionalExternalCounterPartyOrderInfo addExternalCpOrderInfo;

    public ExternalCounterPartyOrder() : this(new AnonymousOrder(HasCpFlags), new AdditionalExternalCounterPartyInfo()) { }

    public ExternalCounterPartyOrder(IMutableAnonymousOrder owner, IMutableAdditionalExternalCounterPartyOrderInfo? addExternalCpOrderInfo = null)
    {
        this.owner = owner;

        this.addExternalCpOrderInfo = this.owner.ExternalCounterPartyOrderInfo ?? addExternalCpOrderInfo ?? new AdditionalExternalCounterPartyInfo();
        this.owner.GenesisFlags |= HasCpFlags;
        this.owner.EmptyIgnoreGenesisFlags |= HasCpFlags;

        if (!ReferenceEquals(this.owner.ExternalCounterPartyOrderInfo, this.addExternalCpOrderInfo))
        {
            if (this.owner.ExternalCounterPartyOrderInfo == null)
            {
                this.owner.ExternalCounterPartyOrderInfo = this.addExternalCpOrderInfo;
            }
            else if (addExternalCpOrderInfo != null)
            {
                this.addExternalCpOrderInfo = this.owner.ExternalCounterPartyOrderInfo.CopyFrom(addExternalCpOrderInfo, CopyMergeFlags.FullReplace);
            }
        }
    }

    public ExternalCounterPartyOrder(IAnonymousOrder toClone)
    {
        if (toClone is ExternalCounterPartyOrder pqExternalCounterPartyOrder)
        {
            owner = pqExternalCounterPartyOrder.owner.Clone();

            owner.GenesisFlags            |= HasCpFlags;
            owner.EmptyIgnoreGenesisFlags |= HasCpFlags;
            addExternalCpOrderInfo = owner.ExternalCounterPartyOrderInfo ??
                                     new AdditionalExternalCounterPartyInfo(toClone.ExternalCounterPartyOrderInfo);
        }
        else
        {
            owner = new AnonymousOrder(toClone);

            owner.GenesisFlags            |= HasCpFlags;
            owner.EmptyIgnoreGenesisFlags |= HasCpFlags;
            addExternalCpOrderInfo = owner.ExternalCounterPartyOrderInfo ??
                                     new AdditionalExternalCounterPartyInfo(toClone.ExternalCounterPartyOrderInfo);
        }

        if (!ReferenceEquals(owner.ExternalCounterPartyOrderInfo, addExternalCpOrderInfo))
        {
            if (owner.ExternalCounterPartyOrderInfo == null)
            {
                owner.ExternalCounterPartyOrderInfo = addExternalCpOrderInfo;
            }
            else if (toClone.ExternalCounterPartyOrderInfo != null)
            {
                owner.ExternalCounterPartyOrderInfo.CopyFrom(toClone.ExternalCounterPartyOrderInfo, CopyMergeFlags.FullReplace);
            }
        }
    }

    public int OrderId
    {
        get => owner.OrderId;
        set => owner.OrderId = value;
    }

    public OrderType OrderType
    {
        get => owner.OrderType;
        set => owner.OrderType = value;
    }

    public OrderLifeCycleState OrderLifeCycleState
    {
        get => owner.OrderLifeCycleState;
        set => owner.OrderLifeCycleState = value;
    }

    public OrderGenesisFlags GenesisFlags
    {
        get => owner.GenesisFlags;
        set => owner.GenesisFlags = value;
    }

    public OrderGenesisFlags EmptyIgnoreGenesisFlags
    {
        get => owner.EmptyIgnoreGenesisFlags;
        set => owner.EmptyIgnoreGenesisFlags = value;
    }

    public uint TrackingId
    {
        get => owner.TrackingId;
        set => owner.TrackingId = value;
    }

    public DateTime CreatedTime
    {
        get => owner.CreatedTime;
        set => owner.CreatedTime = value;
    }

    public DateTime UpdateTime
    {
        get => owner.UpdateTime;
        set => owner.UpdateTime = value;
    }

    public decimal OrderDisplayVolume
    {
        get => owner.OrderDisplayVolume;
        set => owner.OrderDisplayVolume = value;
    }
    public decimal OrderRemainingVolume
    {
        get => owner.OrderRemainingVolume;
        set => owner.OrderRemainingVolume = value;
    }

    public int ExternalCounterPartyId
    {
        get => addExternalCpOrderInfo.ExternalCounterPartyId;
        set => addExternalCpOrderInfo.ExternalCounterPartyId = value;
    }

    public string? ExternalCounterPartyName
    {
        get => addExternalCpOrderInfo.ExternalCounterPartyName;
        set => addExternalCpOrderInfo.ExternalCounterPartyName = value;
    }

    public int ExternalTraderId
    {
        get => addExternalCpOrderInfo.ExternalTraderId;
        set => addExternalCpOrderInfo.ExternalTraderId = value;
    }

    public string? ExternalTraderName
    {
        get => addExternalCpOrderInfo.ExternalTraderName;
        set => addExternalCpOrderInfo.ExternalTraderName = value;
    }

    public IInternalPassiveOrder? ToInternalOrder() => owner.ToInternalOrder();

    public IExternalCounterPartyOrder ToExternalCounterPartyInfoOrder() => this;

    IAdditionalInternalPassiveOrderInfo? IAnonymousOrder.InternalOrderInfo => owner.InternalOrderInfo;

    IAdditionalExternalCounterPartyOrderInfo IAnonymousOrder.ExternalCounterPartyOrderInfo => this;

    public IMutableAdditionalInternalPassiveOrderInfo? InternalOrderInfo
    {
        get => owner.InternalOrderInfo;
        set => owner.InternalOrderInfo = value;
    }
    public IMutableAdditionalExternalCounterPartyOrderInfo? ExternalCounterPartyOrderInfo
    {
        get => owner.ExternalCounterPartyOrderInfo;
        set
        {
            if (ReferenceEquals(value, addExternalCpOrderInfo)) return;
            if (value != null)
            {
                owner.ExternalCounterPartyOrderInfo = value;
                addExternalCpOrderInfo              = value;
            }
            else
            {
                addExternalCpOrderInfo.IsEmpty = true;
            }
        }
    }

    public bool IsEmpty
    {
        get => owner.IsEmpty && addExternalCpOrderInfo.IsEmpty;
        set
        {
            if (!value) return;
            ResetWithTracking();
        }
    }

    IMutableAdditionalExternalCounterPartyOrderInfo ITrackableReset<IMutableAdditionalExternalCounterPartyOrderInfo>.ResetWithTracking() =>
        ResetWithTracking();

    IMutableAnonymousOrder ITrackableReset<IMutableAnonymousOrder>.ResetWithTracking() => ResetWithTracking();

    IMutableExternalCounterPartyOrder ITrackableReset<IMutableExternalCounterPartyOrder>.ResetWithTracking() => ResetWithTracking();

    IMutableExternalCounterPartyOrder IMutableExternalCounterPartyOrder.ResetWithTracking() => ResetWithTracking();

    public ExternalCounterPartyOrder ResetWithTracking()
    {
        owner.ResetWithTracking();

        return this;
    }

    public override void StateReset()
    {
        owner.StateReset();
    }

    IAnonymousOrder ICloneable<IAnonymousOrder>.Clone() => Clone();

    IAdditionalExternalCounterPartyOrderInfo ICloneable<IAdditionalExternalCounterPartyOrderInfo>.Clone() => Clone();

    IMutableAdditionalExternalCounterPartyOrderInfo ICloneable<IMutableAdditionalExternalCounterPartyOrderInfo>.Clone() => Clone();

    IMutableAdditionalExternalCounterPartyOrderInfo IMutableAdditionalExternalCounterPartyOrderInfo.Clone() => Clone();

    IMutableAnonymousOrder ICloneable<IMutableAnonymousOrder>.Clone() => Clone();

    IMutableAnonymousOrder IMutableAnonymousOrder.Clone() => Clone();

    IMutableExternalCounterPartyOrder ICloneable<IMutableExternalCounterPartyOrder>.Clone() => Clone();

    IMutableExternalCounterPartyOrder IMutableExternalCounterPartyOrder.Clone() => Clone();

    IExternalCounterPartyOrder ICloneable<IExternalCounterPartyOrder>.Clone() => Clone();

    IExternalCounterPartyOrder IExternalCounterPartyOrder.Clone() => Clone();

    public override ExternalCounterPartyOrder Clone() =>
        Recycler?.Borrow<ExternalCounterPartyOrder>().CopyFrom(this, CopyMergeFlags.FullReplace) ??
        new ExternalCounterPartyOrder(owner.Clone(), addExternalCpOrderInfo.Clone());

    IReusableObject<IAdditionalExternalCounterPartyOrderInfo> ITransferState<IReusableObject<IAdditionalExternalCounterPartyOrderInfo>>.CopyFrom
        (IReusableObject<IAdditionalExternalCounterPartyOrderInfo> source, CopyMergeFlags copyMergeFlags) =>
        TryCopyAdditionalCounterPartyOrderInfo(source as IAdditionalExternalCounterPartyOrderInfo, copyMergeFlags);


    IAdditionalExternalCounterPartyOrderInfo ITransferState<IAdditionalExternalCounterPartyOrderInfo>.CopyFrom
        (IAdditionalExternalCounterPartyOrderInfo source, CopyMergeFlags copyMergeFlags) =>
        TryCopyAdditionalCounterPartyOrderInfo(source, copyMergeFlags);

    IMutableAdditionalExternalCounterPartyOrderInfo ITransferState<IMutableAdditionalExternalCounterPartyOrderInfo>.CopyFrom
        (IMutableAdditionalExternalCounterPartyOrderInfo source, CopyMergeFlags copyMergeFlags) =>
        TryCopyAdditionalCounterPartyOrderInfo(source, copyMergeFlags);

    protected virtual ExternalCounterPartyOrder TryCopyAdditionalCounterPartyOrderInfo
        (IAdditionalExternalCounterPartyOrderInfo? source, CopyMergeFlags copyMergeFlags)
    {
        if (source is IAnonymousOrder anonymousOrder)
        {
            return CopyFrom(anonymousOrder, copyMergeFlags);
        }
        if (source != null) addExternalCpOrderInfo.CopyFrom(source, copyMergeFlags);
        return this;
    }

    IMutableAnonymousOrder ITransferState<IMutableAnonymousOrder>.CopyFrom(IMutableAnonymousOrder source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    IMutableExternalCounterPartyOrder ITransferState<IMutableExternalCounterPartyOrder>.CopyFrom
        (IMutableExternalCounterPartyOrder source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    IReusableObject<IMutableAnonymousOrder> ITransferState<IReusableObject<IMutableAnonymousOrder>>.CopyFrom
        (IReusableObject<IMutableAnonymousOrder> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IAnonymousOrder)source, copyMergeFlags);

    public override ExternalCounterPartyOrder CopyFrom(IAnonymousOrder source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        owner.CopyFrom(source, copyMergeFlags);
        if (source.ExternalCounterPartyOrderInfo is { } addPassiveOrder)
        {
            addExternalCpOrderInfo.CopyFrom(addPassiveOrder, copyMergeFlags);
        }
        return this;
    }

    bool IInterfacesComparable<IAdditionalExternalCounterPartyOrderInfo>.AreEquivalent
        (IAdditionalExternalCounterPartyOrderInfo? other, bool exactTypes) =>
        AreEquivalent(other as IAnonymousOrder, exactTypes);

    bool IInterfacesComparable<IExternalCounterPartyOrder>.AreEquivalent(IExternalCounterPartyOrder? other, bool exactTypes) =>
        AreEquivalent(other, exactTypes);

    bool IInterfacesComparable<IMutableExternalCounterPartyOrder>.AreEquivalent(IMutableExternalCounterPartyOrder? other, bool exactTypes) =>
        AreEquivalent(other, exactTypes);

    public bool AreEquivalent(IAnonymousOrder? other, bool exactTypes = false)
    {
        if (other is not IExternalCounterPartyOrder extCpOrder) return false;
        bool anonOrderSame;
        bool addExtCpOrderSame;
        if (extCpOrder is ExternalCounterPartyOrder externalCounterPartyOrder)
        {
            anonOrderSame     = owner.AreEquivalent(externalCounterPartyOrder.owner, exactTypes);
            addExtCpOrderSame = addExternalCpOrderInfo.AreEquivalent(externalCounterPartyOrder.addExternalCpOrderInfo, exactTypes);
        }
        else
        {
            if (exactTypes) return false;
            addExtCpOrderSame = addExternalCpOrderInfo.AreEquivalent(extCpOrder);
            anonOrderSame     = owner.AreEquivalent(other);
        }

        var allAreSame = anonOrderSame && addExtCpOrderSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IAnonymousOrder, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = owner.GetHashCode();
            hashCode = (addExternalCpOrderInfo.GetHashCode() * 397) ^ hashCode;
            return hashCode;
        }
    }

    protected string ExternalCounterPartyOrderToStringMembers =>
        $"{nameof(OrderId)}: {OrderId}, {nameof(OrderType)}: {OrderType}, {nameof(GenesisFlags)}: {GenesisFlags}, " +
        $"{nameof(EmptyIgnoreGenesisFlags)}: {EmptyIgnoreGenesisFlags}, {nameof(CreatedTime)}: {CreatedTime}, " +
        $"{nameof(OrderLifeCycleState)}: {OrderLifeCycleState}, {nameof(UpdateTime)}: {UpdateTime}, {nameof(OrderDisplayVolume)}: {OrderDisplayVolume:N2}, " +
        $"{nameof(OrderRemainingVolume)}: {OrderRemainingVolume:N2}, {nameof(TrackingId)}: {TrackingId}, " +
        $"{nameof(ExternalCounterPartyId)}: {ExternalCounterPartyId}, {nameof(ExternalCounterPartyName)}: {ExternalCounterPartyName}, " +
        $"{nameof(ExternalTraderId)}: {ExternalTraderId}, {nameof(ExternalTraderName)}: {ExternalTraderName}";

    public override string ToString() => $"{GetType().Name}{{{ExternalCounterPartyOrderToStringMembers}}}";
}
