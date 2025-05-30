﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols;
using FortitudeMarkets.Trading.Executions;
using FortitudeMarkets.Trading.Orders.Server;
using FortitudeMarkets.Trading.ORX.Session;

#endregion

namespace FortitudeMarkets.Trading.Replay;

public class ReplayMessage : TradingMessage, IReplayMessage
{
    public ReplayMessage() { }

    private ReplayMessage(ReplayMessage toClone)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public override uint MessageId => (uint)TradingMessageIds.Replay;

    public ReplayMessageType ReplayMessageType   { get; set; }
    public IOrderUpdate?     PastOrder           { get; set; }
    public IExecutionUpdate? PastExecutionUpdate { get; set; }

    public override IVersionedMessage CopyFrom
    (IVersionedMessage source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is IReplayMessage replayMessage)
        {
            ReplayMessageType = replayMessage.ReplayMessageType;
            PastOrder         = replayMessage.PastOrder?.SyncOrRecycle(PastOrder as OrderUpdate);
            PastExecutionUpdate
                = replayMessage.PastExecutionUpdate?.SyncOrRecycle(PastExecutionUpdate as ExecutionUpdate);
        }

        return this;
    }

    public IReplayMessage CopyFrom(IReplayMessage source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        (IReplayMessage)CopyFrom((IVersionedMessage)source, copyMergeFlags);


    public override IReplayMessage Clone() => (IReplayMessage?)Recycler?.Borrow<ReplayMessage>().CopyFrom(this) ?? new ReplayMessage(this);
}
