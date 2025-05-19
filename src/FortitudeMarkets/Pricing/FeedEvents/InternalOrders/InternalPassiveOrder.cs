// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.InternalOrders;

public class InternalPassiveOrder : PublishedOrder, IMutableInternalPassiveOrder
{
    public InternalPassiveOrder() { }

    public InternalPassiveOrder
    (int orderId, DateTime createdTime, decimal orderDisplayVolume
      , OrderType orderType = OrderType.None, OrderFlags typeFlags = OrderFlags.None
      , OrderLifeCycleState lifeCycleState = OrderLifeCycleState.None
      , uint orderSequenceId = 0, DateTime? updatedTime = null, decimal? remainingVolume = null, uint trackingId = 0)
        : base(orderId, createdTime, orderDisplayVolume, orderType, typeFlags, lifeCycleState, updatedTime, remainingVolume, trackingId)
    {
        OrderSequenceId = orderSequenceId;
    }

    public InternalPassiveOrder(IPublishedOrder toClone) : base(toClone)
    {
        if (toClone is IInternalPassiveOrder internalPassiveOrder)
        {
            OrderSequenceId      = internalPassiveOrder.OrderSequenceId;
            ParentOrderId        = internalPassiveOrder.ParentOrderId;
            ClosingOrderId       = internalPassiveOrder.ClosingOrderId;
            ClosingOrderPrice    = internalPassiveOrder.ClosingOrderPrice;
            DecisionCreatedTime  = internalPassiveOrder.DecisionCreatedTime;
            DecisionAmendTime    = internalPassiveOrder.DecisionAmendTime;
            DivisionId           = internalPassiveOrder.DivisionId;
            DivisionName         = internalPassiveOrder.DivisionName;
            DeskId               = internalPassiveOrder.DeskId;
            DeskName             = internalPassiveOrder.DeskName;
            StrategyId           = internalPassiveOrder.StrategyId;
            StrategyName         = internalPassiveOrder.StrategyName;
            StrategyDecisionId   = internalPassiveOrder.StrategyDecisionId;
            StrategyDecisionName = internalPassiveOrder.StrategyDecisionName;
            PortfolioId          = internalPassiveOrder.PortfolioId;
            PortfolioName        = internalPassiveOrder.PortfolioName;
            InternalTraderId     = internalPassiveOrder.InternalTraderId;
            InternalTraderName   = internalPassiveOrder.InternalTraderName;
            MarginConsumed       = internalPassiveOrder.MarginConsumed;
        }
    }

    public uint OrderSequenceId { get; set; }

    public uint     ParentOrderId        { get; set; }
    public uint     ClosingOrderId       { get; set; }
    public decimal  ClosingOrderPrice    { get; set; }
    public DateTime DecisionCreatedTime  { get; set; }
    public DateTime DecisionAmendTime    { get; set; }
    public uint     DivisionId           { get; set; }
    public string?  DivisionName         { get; set; }
    public uint     DeskId               { get; set; }
    public string?  DeskName             { get; set; }
    public uint     StrategyId           { get; set; }
    public string?  StrategyName         { get; set; }
    public uint     StrategyDecisionId   { get; set; }
    public string?  StrategyDecisionName { get; set; }
    public uint     PortfolioId          { get; set; }
    public string?  PortfolioName        { get; set; }
    public uint     InternalTraderId     { get; set; }
    public string?  InternalTraderName   { get; set; }
    public decimal  MarginConsumed       { get; set; }

    public override bool IsEmpty
    {
        get =>
            base.IsEmpty
         && OrderSequenceId == 0
         && ParentOrderId == 0
         && ClosingOrderId == 0
         && ClosingOrderPrice == 0
         && DecisionCreatedTime == DateTime.MinValue
         && DecisionAmendTime == DateTime.MinValue
         && DivisionId == 0
         && DivisionName == null
         && DeskId == 0
         && DeskName == null
         && StrategyId == 0
         && StrategyName == null
         && StrategyDecisionId == 0
         && StrategyDecisionName == null
         && PortfolioId == 0
         && PortfolioName == null
         && InternalTraderId == 0
         && InternalTraderName == null
         && MarginConsumed == 0;
        set
        {
            base.IsEmpty = value;
            if (!value) return;
            OrderSequenceId = 0;
            ParentOrderId = 0;
            ClosingOrderId = 0;
            ClosingOrderPrice = 0;
            DecisionCreatedTime = DateTime.MinValue;
            DecisionAmendTime = DateTime.MinValue;
            DivisionId = 0;
            DivisionName = null;
            DeskId = 0;
            DeskName = null;
            StrategyId = 0;
            StrategyName = null;
            StrategyDecisionId = 0;
            StrategyDecisionName = null;
            PortfolioId = 0;
            PortfolioName = null;
            InternalTraderId = 0;
            InternalTraderName = null;
            MarginConsumed = 0;
        }
    }

    public override void StateReset()
    {
        OrderSequenceId      = 0;
        ParentOrderId        = 0;
        ClosingOrderId       = 0;
        ClosingOrderPrice    = 0;
        DecisionCreatedTime  = DateTime.MinValue;
        DecisionAmendTime    = DateTime.MinValue;
        DivisionId           = 0;
        DivisionName         = null;
        DeskId               = 0;
        DeskName             = null;
        StrategyId           = 0;
        StrategyName         = null;
        StrategyDecisionId   = 0;
        StrategyDecisionName = null;
        PortfolioId          = 0;
        PortfolioName        = null;
        InternalTraderId     = 0;
        InternalTraderName   = null;
        MarginConsumed       = 0;
        base.StateReset();
    }

    IInternalPassiveOrder ICloneable<IInternalPassiveOrder>.Clone() => Clone();

    IInternalPassiveOrder IInternalPassiveOrder.Clone() => Clone();

    IMutableInternalPassiveOrder ICloneable<IMutableInternalPassiveOrder>.Clone() => Clone();

    IMutableInternalPassiveOrder IMutableInternalPassiveOrder.Clone() => Clone();

    public override InternalPassiveOrder Clone() =>
        Recycler?.Borrow<InternalPassiveOrder>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new InternalPassiveOrder(this);

    public override InternalPassiveOrder CopyFrom(IPublishedOrder source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is IInternalPassiveOrder internalPassiveOrder)
        {
            OrderSequenceId      = internalPassiveOrder.OrderSequenceId;
            ParentOrderId        = internalPassiveOrder.ParentOrderId;
            ClosingOrderId       = internalPassiveOrder.ClosingOrderId;
            ClosingOrderPrice    = internalPassiveOrder.ClosingOrderPrice;
            DecisionCreatedTime  = internalPassiveOrder.DecisionCreatedTime;
            DecisionAmendTime    = internalPassiveOrder.DecisionAmendTime;
            DivisionId           = internalPassiveOrder.DivisionId;
            DivisionName         = internalPassiveOrder.DivisionName;
            DeskId               = internalPassiveOrder.DeskId;
            DeskName             = internalPassiveOrder.DeskName;
            StrategyId           = internalPassiveOrder.StrategyId;
            StrategyName         = internalPassiveOrder.StrategyName;
            StrategyDecisionId   = internalPassiveOrder.StrategyDecisionId;
            StrategyDecisionName = internalPassiveOrder.StrategyDecisionName;
            PortfolioId          = internalPassiveOrder.PortfolioId;
            PortfolioName        = internalPassiveOrder.PortfolioName;
            InternalTraderId     = internalPassiveOrder.InternalTraderId;
            InternalTraderName   = internalPassiveOrder.InternalTraderName;
            MarginConsumed       = internalPassiveOrder.MarginConsumed;
        }
        return this;
    }


    public override bool AreEquivalent(IPublishedOrder? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (other is not IInternalPassiveOrder internalPassiveOrder) return false;

        var baseSame                 = base.AreEquivalent(other, exactTypes);
        var orderSequenceIdSame      = OrderSequenceId == internalPassiveOrder.OrderSequenceId;
        var parentOrderIdSame        = ParentOrderId == internalPassiveOrder.ParentOrderId;
        var closingOrderIdSame       = ClosingOrderId == internalPassiveOrder.ClosingOrderId;
        var closingOrderPriceSame    = ClosingOrderPrice == internalPassiveOrder.ClosingOrderPrice;
        var decisionCreatedTimeSame  = DecisionCreatedTime == internalPassiveOrder.DecisionCreatedTime;
        var decisionAmendTimeSame    = DecisionAmendTime == internalPassiveOrder.DecisionAmendTime;
        var divisionIdSame           = DivisionId == internalPassiveOrder.DivisionId;
        var divisionNameSame         = DivisionName == internalPassiveOrder.DivisionName;
        var deskIdSame               = DeskId == internalPassiveOrder.DeskId;
        var deskNameSame             = DeskName == internalPassiveOrder.DeskName;
        var strategyIdSame           = StrategyId == internalPassiveOrder.StrategyId;
        var strategyNameSame         = StrategyName == internalPassiveOrder.StrategyName;
        var strategyDecisionIdSame   = StrategyDecisionId == internalPassiveOrder.StrategyDecisionId;
        var strategyDecisionNameSame = StrategyDecisionName == internalPassiveOrder.StrategyDecisionName;
        var portfolioIdSame          = PortfolioId == internalPassiveOrder.PortfolioId;
        var portfolioNameSame        = PortfolioName == internalPassiveOrder.PortfolioName;
        var internalTraderIdSame     = InternalTraderId == internalPassiveOrder.InternalTraderId;
        var internalTraderNameSame   = InternalTraderName == internalPassiveOrder.InternalTraderName;
        var marginConsumedSame       = MarginConsumed == internalPassiveOrder.MarginConsumed;

        var allAreSame = orderSequenceIdSame && parentOrderIdSame && closingOrderIdSame && closingOrderPriceSame && decisionCreatedTimeSame 
            && decisionAmendTimeSame && divisionIdSame && divisionNameSame && deskIdSame && deskNameSame && strategyIdSame
               && strategyNameSame && strategyDecisionNameSame && strategyDecisionIdSame && portfolioIdSame && portfolioNameSame && internalTraderIdSame &&
               internalTraderNameSame && marginConsumedSame && baseSame;

        return allAreSame;
    }
    
    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IInternalPassiveOrder, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = ((int)OrderSequenceId * 397) ^ hashCode;
            hashCode = ((int)ParentOrderId * 397) ^ hashCode;
            hashCode = ((int)ClosingOrderId * 397) ^ hashCode;
            hashCode = (ClosingOrderPrice.GetHashCode() * 397) ^ hashCode;
            hashCode = (DecisionCreatedTime.GetHashCode() * 397) ^ hashCode;
            hashCode = (DecisionAmendTime.GetHashCode() * 397) ^ hashCode;
            hashCode = ((int)DivisionId * 397) ^ hashCode;
            hashCode = ((DivisionName?.GetHashCode() ?? 0) * 397) ^ hashCode;
            hashCode = ((int)DeskId * 397) ^ hashCode;
            hashCode = ((DeskName?.GetHashCode() ?? 0) * 397) ^ hashCode;
            hashCode = ((int)StrategyId * 397) ^ hashCode;
            hashCode = ((StrategyName?.GetHashCode() ?? 0) * 397) ^ hashCode;
            hashCode = ((int)StrategyDecisionId * 397) ^ hashCode;
            hashCode = ((StrategyDecisionName?.GetHashCode() ?? 0) * 397) ^ hashCode;
            hashCode = ((int)PortfolioId * 397) ^ hashCode;
            hashCode = ((PortfolioName?.GetHashCode() ?? 0) * 397) ^ hashCode;
            hashCode = ((int)InternalTraderId * 397) ^ hashCode;
            hashCode = ((InternalTraderName?.GetHashCode() ?? 0) * 397) ^ hashCode;
            hashCode = (MarginConsumed.GetHashCode() * 397) ^ hashCode;
            return hashCode;
        }
    }

    protected string InternalPassiveOrderToStringMembers => 
        $"{PublishedOrderToStringMembers}, {nameof(OrderSequenceId)}: {OrderSequenceId}, {nameof(ParentOrderId)}: {ParentOrderId}, " +
        $"{nameof(ClosingOrderId)}: {ClosingOrderId}, {nameof(ClosingOrderPrice)}: {ClosingOrderPrice}, {nameof(DecisionCreatedTime)}: {DecisionCreatedTime}, " +
        $"{nameof(DecisionAmendTime)}: {DecisionAmendTime}, {nameof(DivisionId)}: {DivisionId}, {nameof(DivisionName)}: {DivisionName}, " +
        $"{nameof(DeskId)}: {DeskId}, {nameof(DeskName)}: {DeskName}, {nameof(StrategyId)}: {StrategyId}, {nameof(StrategyName)}: {StrategyName}, " +
        $"{nameof(StrategyDecisionId)}: {StrategyDecisionId}, {nameof(StrategyDecisionName)}: {StrategyDecisionName}, {nameof(PortfolioId)}: {PortfolioId}, " +
        $"{nameof(PortfolioName)}: {PortfolioName}, {nameof(InternalTraderId)}: {InternalTraderId}, {nameof(InternalTraderName)}: {InternalTraderName}, " +
        $"{nameof(MarginConsumed)}: {MarginConsumed}, {nameof(PublishedOrderToStringMembers)}: {PublishedOrderToStringMembers}";

    public override string ToString() => $"{GetType().Name}{{{InternalPassiveOrderToStringMembers}}}";
}
