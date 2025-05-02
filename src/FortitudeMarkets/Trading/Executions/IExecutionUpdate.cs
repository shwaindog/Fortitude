// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarkets.Trading.Executions;

public interface IExecutionUpdate : ITradingMessage, ITransferState<IExecutionUpdate>
{
    IExecution? Execution { get; set; }

    ExecutionUpdateType ExecutionUpdateType { get; set; }

    DateTime SocketReceivedTime   { get; set; }
    DateTime AdapterProcessedTime { get; set; }
    DateTime ClientReceivedTime   { get; set; }
}
