// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Serdes.Binary;
using FortitudeMarkets.Pricing.PQ.Messages;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Deserialization.SyncState;

public class InitializationState<T> : SynchronisingState<T> where T : IPQMutableMessage
{
    public InitializationState(IPQMessagePublishingDeserializer<T> linkedDeserializer)
        : base(linkedDeserializer, QuoteSyncState.InitializationState) { }

    protected override void ProcessUnsyncedUpdateMessage(IMessageBufferContext bufferContext, uint sequenceId)
    {
        base.ProcessUnsyncedUpdateMessage(bufferContext, sequenceId);
        SwitchState(QuoteSyncState.Synchronising);
    }

    protected override void LogSyncRecoveryMessage(uint sequenceId)
    {
        Logger.Info("Stream {0} started from first message, RecvSeqID={1}",
                    LinkedDeserializer.Identifier, sequenceId);
    }

    protected override void ProcessUpdate(IMessageBufferContext bufferContext)
    {
        var sequenceId = bufferContext.ReadCurrentMessageSequenceId();
        if (sequenceId == 0)
            ProcessNextExpectedUpdate(bufferContext, sequenceId);
        else
            ProcessUnsyncedUpdateMessage(bufferContext, sequenceId);
    }
}
