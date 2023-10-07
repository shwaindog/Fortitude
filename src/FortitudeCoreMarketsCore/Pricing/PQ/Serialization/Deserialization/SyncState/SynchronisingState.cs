#region

using FortitudeIO.Protocols.Serialization;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Subscription;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState;

public class SynchronisingState<T> : SyncStateBase<T> where T : PQLevel0Quote, new()
{
    private DateTime lastSyncAttempt;

    protected SynchronisingState(IPQQuoteDeserializer<T> linkedDeserializer, QuoteSyncState state)
        : base(linkedDeserializer, state) { }

    public SynchronisingState(IPQQuoteDeserializer<T> linkedDeserializer)
        : base(linkedDeserializer, QuoteSyncState.Synchronising) { }

    protected override void ProcessNextExpectedUpdate(DispatchContext dispatchContext, uint sequenceId)
    {
        base.ProcessNextExpectedUpdate(dispatchContext, sequenceId);

        var prevSeqId = LinkedDeserializer.PublishedQuote.PQSequenceId;
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
        PublishQuoteRunAction(PQSyncStatus.Good, dispatchContext.DispatchLatencyLogger,
            LinkedDeserializer.OnSyncOk);
    }

    protected virtual void LogSyncRecoveryMessage(uint sequenceId)
    {
        Logger.Info("Stream {0} recovered after message sequence out of order, RecvSeqID={1}",
            LinkedDeserializer.Identifier, sequenceId);
    }

    protected override void ProcessUnsyncedUpdateMessage(DispatchContext dispatchContext, uint sequenceId)
    {
        base.ProcessUnsyncedUpdateMessage(dispatchContext, sequenceId);
        SaveMessageToSyncSlot(dispatchContext, sequenceId);
    }

    public override void ProcessSnapshot(DispatchContext dispatchContext)
    {
        var msgHeader = dispatchContext.MessageHeader as PQQuoteTransmissionHeader;
        var prevSeqId = LinkedDeserializer.PublishedQuote.PQSequenceId;
        if (msgHeader != null)
        {
            LinkedDeserializer.UpdateQuote(dispatchContext, LinkedDeserializer.PublishedQuote,
                msgHeader.SequenceId);
            uint mismatchedSeqId;
            if (!LinkedDeserializer.Synchronize(out mismatchedSeqId))
            {
                Logger.Info("Stream {0} could not recover after snapshot, " +
                            "PrevSeqId={1}, SnapshotSeqId={2}, MismatchedId={3}",
                    LinkedDeserializer.Identifier, prevSeqId, msgHeader.SequenceId, mismatchedSeqId);
                return;
            }

            Logger.Info(
                "Stream {0} recovered after snapshot, PrevSeqId={1}, SnapshotSeqId={2}, LastUpdateSeqId={3}",
                LinkedDeserializer.Identifier, prevSeqId, msgHeader.SequenceId, prevSeqId);
        }

        PublishQuoteRunAction(PQSyncStatus.Good, dispatchContext.DispatchLatencyLogger,
            LinkedDeserializer.OnSyncOk);
        SwitchState(QuoteSyncState.InSync);
    }

    public override bool EligibleForResync(DateTime utcNow)
    {
        var waitOver = (utcNow - lastSyncAttempt).TotalMilliseconds >= LinkedDeserializer.SyncRetryMs;
        if (waitOver) lastSyncAttempt = utcNow;
        return waitOver;
    }
}
