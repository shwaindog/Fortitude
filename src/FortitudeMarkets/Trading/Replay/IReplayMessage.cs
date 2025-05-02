// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Trading.Executions;
using FortitudeMarkets.Trading.Orders.Server;

#endregion

namespace FortitudeMarkets.Trading.Replay;

public interface IReplayMessage : ITradingMessage, ITransferState<IReplayMessage>
{
    ReplayMessageType ReplayMessageType { get; set; }
    IOrderUpdate?     PastOrder         { get; set; }

    IExecutionUpdate?  PastExecutionUpdate { get; set; }
    new IReplayMessage Clone();
}
