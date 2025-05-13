// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarkets.Pricing.FeedEvents;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Deserialization.SyncState;

public class SynchronisingState<T> : SyncStateBase<T> where T : PQPublishableTickInstant, new()
{
    private DateTime lastSyncAttempt;

    protected SynchronisingState(IPQQuotePublishingDeserializer<T> linkedDeserializer, QuoteSyncState state)
        : base(linkedDeserializer, state) { }

    public SynchronisingState(IPQQuotePublishingDeserializer<T> linkedDeserializer)
        : base(linkedDeserializer, QuoteSyncState.Synchronising) { }

    protected override void ProcessNextExpectedUpdate(IMessageBufferContext bufferContext, uint sequenceId)
    {
        base.ProcessNextExpectedUpdate(bufferContext, sequenceId);

        var  prevSeqId = LinkedDeserializer.PublishedQuote.PQSequenceId;
        uint mismatchedSeqId;
        if (!LinkedDeserializer.Synchronize(out mismatchedSeqId))
        {
            Logger.Info("Stream {0} could not recover after sequence anomaly, PrevSeqId={1}, " +
                        "RecvSeqID={2}, MismatchedId={3}",
                        LinkedDeserializer.Identifier, prevSeqId, sequenceId, mismatchedSeqId);
            return;
        }

        LogSyncRecoveryMessage(sequenceId);
        SwitchState(QuoteSyncState.InSync);
        var sockBuffContext = bufferContext as SocketBufferReadContext;
        PublishQuoteRunAction(FeedSyncStatus.Good, sockBuffContext?.DispatchLatencyLogger,
                              LinkedDeserializer.OnSyncOk);
    }

    protected virtual void LogSyncRecoveryMessage(uint sequenceId)
    {
        Logger.Info("Stream {0} recovered after message sequence out of order, RecvSeqID={1}",
                    LinkedDeserializer.Identifier, sequenceId);
    }

    protected override void ProcessUnsyncedUpdateMessage(IMessageBufferContext bufferContext, uint sequenceId)
    {
        base.ProcessUnsyncedUpdateMessage(bufferContext, sequenceId);
        SaveMessageToSyncSlot(bufferContext, sequenceId);
    }

    public override void ProcessSnapshot(IMessageBufferContext bufferContext)
    {
        var prevSeqId = LinkedDeserializer.PublishedQuote.PQSequenceId;
        var currSeqId = bufferContext.ReadCurrentMessageSequenceId();
        LinkedDeserializer.UpdateQuote(bufferContext, LinkedDeserializer.PublishedQuote,
                                       currSeqId);
        uint mismatchedSeqId;
        if (!LinkedDeserializer.Synchronize(out mismatchedSeqId))
        {
            Logger.Info("Stream {0} could not recover after snapshot, " +
                        "PrevSeqId={1}, SnapshotSeqId={2}, MismatchedId={3}",
                        LinkedDeserializer.Identifier, prevSeqId, currSeqId, mismatchedSeqId);
            return;
        }

        Logger.Info(
                    "Stream {0} recovered after snapshot, PrevSeqId={1}, SnapshotSeqId={2}, LastUpdateSeqId={3}",
                    LinkedDeserializer.Identifier, prevSeqId, currSeqId, prevSeqId);

        var sockBuffContext = bufferContext as SocketBufferReadContext;
        PublishQuoteRunAction(FeedSyncStatus.Good, sockBuffContext?.DispatchLatencyLogger,
                              LinkedDeserializer.OnSyncOk);
        SwitchState(QuoteSyncState.InSync);
    }

    public override bool EligibleForResync(DateTime utcNow)
    {
        var waitOver                  = (utcNow - lastSyncAttempt).TotalMilliseconds >= LinkedDeserializer.SyncRetryMs;
        if (waitOver) lastSyncAttempt = utcNow;
        return waitOver;
    }
}
