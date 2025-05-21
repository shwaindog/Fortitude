// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.InternalOrders;

public class InternalPassiveOrder : ReusableObject<IAnonymousOrder>, IMutableInternalPassiveOrder
{
    private readonly IMutableAnonymousOrder                     owner;

    private          IMutableAdditionalInternalPassiveOrderInfo addInternalOrderInfo;

    public InternalPassiveOrder() : this(new AnonymousOrder(IInternalPassiveOrder.HasInternalOrderInfo), new AdditionalInternalPassiveOrderInfo())
    {
    }

    public InternalPassiveOrder(IMutableAnonymousOrder owner, IMutableAdditionalInternalPassiveOrderInfo? addInternalOrderInfo)
    {
        this.owner                         =  owner;
        this.addInternalOrderInfo          =  this.owner.InternalOrderInfo ?? addInternalOrderInfo ?? new AdditionalInternalPassiveOrderInfo();
        this.owner.EmptyIgnoreGenesisFlags |= IInternalPassiveOrder.HasInternalOrderInfo;
        
        if (!ReferenceEquals(this.owner.InternalOrderInfo, this.addInternalOrderInfo))
        {
            if (this.owner.InternalOrderInfo == null)
            {
                this.owner.InternalOrderInfo = this.addInternalOrderInfo;
            }
            else if(addInternalOrderInfo != null)
            {
                this.owner.InternalOrderInfo.CopyFrom(addInternalOrderInfo, CopyMergeFlags.FullReplace);
            }
        }
    }

    public InternalPassiveOrder(IAnonymousOrder toClone)
    {
        if (toClone is InternalPassiveOrder pqExternalCounterPartyOrder)
        {
            owner                  = pqExternalCounterPartyOrder.owner.Clone();
            addInternalOrderInfo = owner.InternalOrderInfo?.Clone() ?? new AdditionalInternalPassiveOrderInfo(toClone.InternalOrderInfo);
        }
        else
        {
            owner                = new AnonymousOrder(toClone);
            addInternalOrderInfo = new AdditionalInternalPassiveOrderInfo(toClone.InternalOrderInfo);
        }

        if (!ReferenceEquals(owner.InternalOrderInfo, addInternalOrderInfo))
        {
            if (owner.InternalOrderInfo == null)
            {
                owner.InternalOrderInfo = addInternalOrderInfo;
            }
            else if (toClone.InternalOrderInfo != null)
            {
                owner.InternalOrderInfo.CopyFrom(toClone.InternalOrderInfo, CopyMergeFlags.FullReplace);
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
    public uint OrderSequenceId
    {
        get => addInternalOrderInfo.OrderSequenceId;
        set => addInternalOrderInfo.OrderSequenceId = value;
    }
    public uint ParentOrderId
    {
        get => addInternalOrderInfo.ParentOrderId;
        set => addInternalOrderInfo.ParentOrderId = value;
    }
    public uint ClosingOrderId
    {
        get => addInternalOrderInfo.ClosingOrderId;
        set => addInternalOrderInfo.ClosingOrderId = value;
    }
    public decimal ClosingOrderPrice
    {
        get => addInternalOrderInfo.ClosingOrderPrice;
        set => addInternalOrderInfo.ClosingOrderPrice = value;
    }
    public DateTime DecisionCreatedTime
    {
        get => addInternalOrderInfo.DecisionCreatedTime;
        set => addInternalOrderInfo.DecisionCreatedTime = value;
    }
    public DateTime DecisionAmendTime
    {
        get => addInternalOrderInfo.DecisionAmendTime;
        set => addInternalOrderInfo.DecisionAmendTime = value;
    }
    public uint DivisionId
    {
        get => addInternalOrderInfo.DivisionId;
        set => addInternalOrderInfo.DivisionId = value;
    }
    public string? DivisionName
    {
        get => addInternalOrderInfo.DivisionName;
        set => addInternalOrderInfo.DivisionName = value;
    }
    public uint DeskId
    {
        get => addInternalOrderInfo.DeskId;
        set => addInternalOrderInfo.DeskId = value;
    }
    public string? DeskName
    {
        get => addInternalOrderInfo.DeskName;
        set => addInternalOrderInfo.DeskName = value;
    }
    public uint StrategyId
    {
        get => addInternalOrderInfo.StrategyId;
        set => addInternalOrderInfo.StrategyId = value;
    }
    public string? StrategyName
    {
        get => addInternalOrderInfo.StrategyName;
        set => addInternalOrderInfo.StrategyName = value;
    }
    public uint StrategyDecisionId
    {
        get => addInternalOrderInfo.StrategyDecisionId;
        set => addInternalOrderInfo.StrategyDecisionId = value;
    }
    public string? StrategyDecisionName
    {
        get => addInternalOrderInfo.StrategyDecisionName;
        set => addInternalOrderInfo.StrategyDecisionName = value;
    }
    public uint PortfolioId
    {
        get => addInternalOrderInfo.PortfolioId;
        set => addInternalOrderInfo.PortfolioId = value;
    }
    public string? PortfolioName
    {
        get => addInternalOrderInfo.PortfolioName;
        set => addInternalOrderInfo.PortfolioName = value;
    }
    public uint InternalTraderId
    {
        get => addInternalOrderInfo.InternalTraderId;
        set => addInternalOrderInfo.InternalTraderId = value;
    }
    public string? InternalTraderName
    {
        get => addInternalOrderInfo.InternalTraderName;
        set => addInternalOrderInfo.InternalTraderName = value;
    }
    public decimal MarginConsumed
    {
        get => addInternalOrderInfo.MarginConsumed;
        set => addInternalOrderInfo.MarginConsumed = value;
    }

    IAdditionalInternalPassiveOrderInfo? IAnonymousOrder.InternalOrderInfo => this;

    IAdditionalExternalCounterPartyOrderInfo? IAnonymousOrder.ExternalCounterPartyOrderInfo => owner.ExternalCounterPartyOrderInfo;

    public IMutableAdditionalInternalPassiveOrderInfo? InternalOrderInfo
    {
        get => owner.InternalOrderInfo;
        set
        {
            if (ReferenceEquals(value, addInternalOrderInfo)) return;
            if (value != null)
            {
                owner.InternalOrderInfo       = value;
                addInternalOrderInfo = value;
            }
            else
            {
                addInternalOrderInfo.IsEmpty = true;
            }
        }
    }
    public IMutableAdditionalExternalCounterPartyOrderInfo? ExternalCounterPartyOrderInfo
    {
        get => owner.ExternalCounterPartyOrderInfo;
        set => owner.ExternalCounterPartyOrderInfo = value;
    }


    public bool IsEmpty
    {
        get => owner.IsEmpty && addInternalOrderInfo.IsEmpty;
        set
        {
            owner.IsEmpty                         = value;
            addInternalOrderInfo.IsEmpty = value;
        }
    }

    public IInternalPassiveOrder? ToInternalOrder() => this;

    public IExternalCounterPartyOrder? ToExternalCounterPartyInfoOrder() => owner.ToExternalCounterPartyInfoOrder();


    IInternalPassiveOrder ICloneable<IInternalPassiveOrder>.Clone() => Clone();

    IInternalPassiveOrder IInternalPassiveOrder.Clone() => Clone();

    IMutableAdditionalInternalPassiveOrderInfo ICloneable<IMutableAdditionalInternalPassiveOrderInfo>.Clone() => Clone();

    IMutableAdditionalInternalPassiveOrderInfo IMutableAdditionalInternalPassiveOrderInfo.Clone() => Clone();

    IMutableInternalPassiveOrder ICloneable<IMutableInternalPassiveOrder>.Clone() => Clone();

    IMutableInternalPassiveOrder IMutableInternalPassiveOrder.Clone() => Clone();

    IAdditionalInternalPassiveOrderInfo ICloneable<IAdditionalInternalPassiveOrderInfo>.Clone() => Clone();

    IMutableAnonymousOrder ICloneable<IMutableAnonymousOrder>.Clone() => Clone();

    IMutableAnonymousOrder IMutableAnonymousOrder.Clone() => Clone();

    public override InternalPassiveOrder Clone() =>
        Recycler?.Borrow<InternalPassiveOrder>().CopyFrom(this, CopyMergeFlags.FullReplace) ??
        new InternalPassiveOrder(owner.Clone(), addInternalOrderInfo.Clone());


    IReusableObject<IAdditionalInternalPassiveOrderInfo> ITransferState<IReusableObject<IAdditionalInternalPassiveOrderInfo>>.CopyFrom
        (IReusableObject<IAdditionalInternalPassiveOrderInfo> source, CopyMergeFlags copyMergeFlags) =>
        TryCopyAdditionalInternalPassiveOrderInfo(source as IAdditionalInternalPassiveOrderInfo, copyMergeFlags);


    IAdditionalInternalPassiveOrderInfo ITransferState<IAdditionalInternalPassiveOrderInfo>.CopyFrom
        (IAdditionalInternalPassiveOrderInfo source, CopyMergeFlags copyMergeFlags) =>
        TryCopyAdditionalInternalPassiveOrderInfo(source, copyMergeFlags);

    IMutableAdditionalInternalPassiveOrderInfo ITransferState<IMutableAdditionalInternalPassiveOrderInfo>.CopyFrom
        (IMutableAdditionalInternalPassiveOrderInfo source, CopyMergeFlags copyMergeFlags) =>
        TryCopyAdditionalInternalPassiveOrderInfo(source, copyMergeFlags);
    
    protected virtual InternalPassiveOrder TryCopyAdditionalInternalPassiveOrderInfo
        (IAdditionalInternalPassiveOrderInfo? source, CopyMergeFlags copyMergeFlags)
    {
        if (source is IAnonymousOrder anonymousOrder)
        {
            return CopyFrom(anonymousOrder, copyMergeFlags);
        }
        if(source != null) addInternalOrderInfo.CopyFrom(source, copyMergeFlags);
        return this;
    }

    IMutableAnonymousOrder ITransferState<IMutableAnonymousOrder>.CopyFrom(IMutableAnonymousOrder source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    public override InternalPassiveOrder CopyFrom(IAnonymousOrder source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        owner.CopyFrom(source, copyMergeFlags);
        if (source is IAdditionalInternalPassiveOrderInfo addPassiveOrder)
        {
            addInternalOrderInfo.CopyFrom(addPassiveOrder, copyMergeFlags);
        }
        return this;
    }

    bool IInterfacesComparable<IAdditionalInternalPassiveOrderInfo>.AreEquivalent(IAdditionalInternalPassiveOrderInfo? other, bool exactTypes) => 
        AreEquivalent(other as IAnonymousOrder, exactTypes);

    public bool AreEquivalent(IAnonymousOrder? other, bool exactTypes = false)
    {
        if (other is not IInternalPassiveOrder intPassiveOrder) return false;
        var anonOrderSame       = true;
        var addPassiveOrderSame = true;
        if (intPassiveOrder is InternalPassiveOrder internalPassive)
        {
            anonOrderSame       = owner.AreEquivalent(internalPassive.owner, exactTypes);
            addPassiveOrderSame = addInternalOrderInfo.AreEquivalent(internalPassive.addInternalOrderInfo, exactTypes);
        }
        else
        {
            if (exactTypes) return false;
            addPassiveOrderSame = addInternalOrderInfo.AreEquivalent(intPassiveOrder, false);
            anonOrderSame       = owner.AreEquivalent(other, false);
        }

        var allAreSame = anonOrderSame && addPassiveOrderSame;

        return allAreSame;
    }

    
    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IAnonymousOrder, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = owner.GetHashCode();
            hashCode = (addInternalOrderInfo.GetHashCode() * 397) ^ hashCode;
            return hashCode;
        }
    }

    protected string InternalPassiveOrderToStringMembers => 
        $"{nameof(OrderId)}: {OrderId}, {nameof(OrderType)}: {OrderType}, {nameof(GenesisFlags)}: {GenesisFlags}, " +
        $"{nameof(EmptyIgnoreGenesisFlags)}: {EmptyIgnoreGenesisFlags}, {nameof(CreatedTime)}: {CreatedTime}, " +
        $"{nameof(OrderLifeCycleState)}: {OrderLifeCycleState}, {nameof(UpdateTime)}: {UpdateTime}, {nameof(OrderDisplayVolume)}: {OrderDisplayVolume:N2}, " +
        $"{nameof(OrderRemainingVolume)}: {OrderRemainingVolume:N2}, {nameof(TrackingId)}: {TrackingId}, "  + 
        $"{nameof(OrderSequenceId)}: {OrderSequenceId}, {nameof(ParentOrderId)}: {ParentOrderId}, {nameof(ClosingOrderId)}: {ClosingOrderId}, " +
        $"{nameof(ClosingOrderPrice)}: {ClosingOrderPrice}, {nameof(DecisionCreatedTime)}: {DecisionCreatedTime}, " +
        $"{nameof(DecisionAmendTime)}: {DecisionAmendTime}, {nameof(DivisionId)}: {DivisionId}, {nameof(DivisionName)}: {DivisionName}, " +
        $"{nameof(DeskId)}: {DeskId}, {nameof(DeskName)}: {DeskName}, {nameof(StrategyId)}: {StrategyId}, {nameof(StrategyName)}: {StrategyName}, " +
        $"{nameof(StrategyDecisionId)}: {StrategyDecisionId}, {nameof(StrategyDecisionName)}: {StrategyDecisionName}, {nameof(PortfolioId)}: {PortfolioId}, " +
        $"{nameof(PortfolioName)}: {PortfolioName}, {nameof(InternalTraderId)}: {InternalTraderId}, {nameof(InternalTraderName)}: {InternalTraderName}, " +
        $"{nameof(MarginConsumed)}: {MarginConsumed}";

    public override string ToString() => $"{GetType().Name}{{{InternalPassiveOrderToStringMembers}}}";
}
