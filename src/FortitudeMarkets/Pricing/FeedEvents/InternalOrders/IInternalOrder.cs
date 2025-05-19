// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types;

namespace FortitudeMarkets.Pricing.FeedEvents.InternalOrders;

public interface IAdditionalInternalOrderInfo
{
    int ClientOrderId { get; }

    TimeInForceType   TimeInForce  { get; }
    OrderUpdateReason UpdateReason { get; }

    decimal OrderSubmittedPrice { get; }

    OrderSourceSubmissionStateFlags SourceSubmissionStateType { get; }
    OrderPositionAlterationFlags    PositionAlterationType    { get; }

    decimal OrderSubmittedTakeProfitPrice { get; }
    decimal OrderSubmittedStopLossPrice   { get; }

    decimal InternalTargetPrice           { get; }
    decimal OnCreateTargetTakeProfitPrice { get; }
    decimal OnCreateTargetStopLossPrice   { get; }
    decimal CurrentTargetTakeProfitPrice  { get; }
    decimal CurrentTargetStopLossPrice    { get; }
    uint    TakeProfitOrderId             { get; }
    uint    StopLossOrderId               { get; }
}

public interface IAdditionalMutableInternalOrder : IAdditionalInternalOrderInfo
{
    new int ClientOrderId { get; set; }

    new TimeInForceType   TimeInForce  { get; set; }
    new OrderUpdateReason UpdateReason { get; set; }

    new decimal OrderSubmittedPrice { get; set; }

    new OrderSourceSubmissionStateFlags SourceSubmissionStateType { get; set; }
    new OrderPositionAlterationFlags    PositionAlterationType    { get; set; }

    new decimal OrderSubmittedTakeProfitPrice { get; set; }
    new decimal OrderSubmittedStopLossPrice   { get; set; }

    new decimal InternalTargetPrice           { get; set; }
    new decimal OnCreateTargetTakeProfitPrice { get; set; }
    new decimal OnCreateTargetStopLossPrice   { get; set; }
    new decimal CurrentTargetTakeProfitPrice  { get; set; }
    new decimal CurrentTargetStopLossPrice    { get; set; }

    new uint TakeProfitOrderId { get; set; }
    new uint StopLossOrderId   { get; set; }
}


public interface IInternalOrder : IInternalPassiveOrder, IAdditionalInternalOrderInfo, ICloneable<IInternalOrder>
{
    new IInternalOrder Clone();
}


public interface IMutableInternalOrder : IInternalOrder, IAdditionalMutableInternalOrder, IMutableInternalPassiveOrder, ICloneable<IMutableInternalOrder>
{
    new IMutableInternalOrder Clone();
}