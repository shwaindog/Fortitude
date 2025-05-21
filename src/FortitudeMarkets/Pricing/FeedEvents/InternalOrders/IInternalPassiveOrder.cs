// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.InternalOrders;

public interface IAdditionalInternalPassiveOrderInfo : IReusableObject<IAdditionalInternalPassiveOrderInfo>, IShowsEmpty, IInterfacesComparable<IAdditionalInternalPassiveOrderInfo>
{
    uint    OrderSequenceId   { get; set; }
    uint    ParentOrderId     { get; }
    uint    ClosingOrderId    { get; }
    decimal ClosingOrderPrice { get; }

    DateTime DecisionCreatedTime { get; }
    DateTime DecisionAmendTime   { get; }

    uint    DivisionId   { get; }
    string? DivisionName { get; }

    uint    DeskId   { get; }
    string? DeskName { get; }

    uint    StrategyId           { get; }
    string? StrategyName         { get; }
    uint    StrategyDecisionId   { get; }
    string? StrategyDecisionName { get; }

    uint    PortfolioId   { get; }
    string? PortfolioName { get; }

    uint    InternalTraderId   { get; }
    string? InternalTraderName { get; }

    decimal MarginConsumed { get; }
}

public interface IMutableAdditionalInternalPassiveOrderInfo : IAdditionalInternalPassiveOrderInfo, ICloneable<IMutableAdditionalInternalPassiveOrderInfo>, IEmptyable
  , ITransferState<IMutableAdditionalInternalPassiveOrderInfo>
{
    new uint OrderSequenceId { get; set; }
    new uint ParentOrderId   { get; set; }

    new uint    ClosingOrderId    { get; set; }
    new decimal ClosingOrderPrice { get; set; }

    new DateTime DecisionCreatedTime { get; set; }
    new DateTime DecisionAmendTime   { get; set; }

    new uint    DivisionId   { get; set; }
    new string? DivisionName { get; set; }

    new uint    DeskId   { get; set; }
    new string? DeskName { get; set; }

    new uint    StrategyId   { get; set; }
    new string? StrategyName { get; set; }

    new uint    StrategyDecisionId   { get; set; }
    new string? StrategyDecisionName { get; set; }

    new uint    PortfolioId   { get; set; }
    new string? PortfolioName { get; set; }

    new uint    InternalTraderId   { get; set; }
    new string? InternalTraderName { get; set; }

    new decimal MarginConsumed { get; set; }

    new IMutableAdditionalInternalPassiveOrderInfo Clone();
}

public interface IInternalPassiveOrder : IAnonymousOrder, IAdditionalInternalPassiveOrderInfo, ICloneable<IInternalPassiveOrder>
{
    const OrderGenesisFlags HasInternalOrderInfo = OrderGenesisFlags.HasInternalOrderInfo;

    const OrderGenesisFlags AllExceptHasInternalInfoFlag = ~OrderGenesisFlags.HasInternalOrderInfo;

    new IInternalPassiveOrder Clone();
}

public interface IMutableInternalPassiveOrder : IInternalPassiveOrder, IMutableAdditionalInternalPassiveOrderInfo, IMutableAnonymousOrder
  , ICloneable<IMutableInternalPassiveOrder>
{
    new IMutableInternalPassiveOrder Clone();
}