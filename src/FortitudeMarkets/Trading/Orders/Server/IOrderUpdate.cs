// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarkets.Trading.Orders.Server;

public interface IOrderUpdate : ITradingMessage, ITransferState<IOrderUpdate>
{
    IOrder? Order { get; set; }

    OrderUpdateEventType OrderUpdateType { get; set; }

    DateTime AdapterUpdateTime  { get; set; }
    DateTime ClientReceivedTime { get; set; }

    new IOrderUpdate Clone();
}
