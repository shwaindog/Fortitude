// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types;

namespace FortitudeMarkets.Pricing.FeedEvents.InternalOrders;


public class InternalOrder : InternalPassiveOrder, IMutableInternalOrder
{
    public int ClientOrderId { get; set; }

    public TimeInForceType   TimeInForce  { get; set; }
    public OrderUpdateReason UpdateReason { get; set; }

    public decimal OrderSubmittedPrice { get; set; }

    public OrderSourceSubmissionStateFlags SourceSubmissionStateType { get; set; }
    public OrderPositionAlterationFlags    PositionAlterationType    { get; set; }

    public decimal OrderSubmittedTakeProfitPrice { get; set; }
    public decimal OrderSubmittedStopLossPrice   { get; set; }
    public decimal InternalTargetPrice           { get; set; }
    public decimal OnCreateTargetTakeProfitPrice { get; set; }
    public decimal OnCreateTargetStopLossPrice   { get; set; }
    public decimal CurrentTargetTakeProfitPrice  { get; set; }
    public decimal CurrentTargetStopLossPrice    { get; set; }
    public uint    TakeProfitOrderId             { get; set; }
    public uint    StopLossOrderId               { get; set; }


    IInternalOrder ICloneable<IInternalOrder>.Clone() => throw new NotImplementedException();

    IInternalOrder IInternalOrder.Clone() => throw new NotImplementedException();

    IMutableInternalOrder ICloneable<IMutableInternalOrder>.Clone() => throw new NotImplementedException();
    IMutableInternalOrder IMutableInternalOrder.            Clone() => throw new NotImplementedException();
}
