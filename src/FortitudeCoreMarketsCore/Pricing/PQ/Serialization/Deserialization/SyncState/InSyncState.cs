#region

using FortitudeIO.Protocols.Serialization;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Subscription;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState;

public class InSyncState<T> : SyncStateBase<T> where T : PQLevel0Quote, new()
{
    internal const uint PQTimeoutMs = 2000;

    protected long LastSuccessfulUpdateSequienceId = -1;

    public InSyncState(IPQQuoteDeserializer<T> linkedDeserializer)
        : base(linkedDeserializer, QuoteSyncState.InSync) { }

    protected InSyncState(IPQQuoteDeserializer<T> linkedDeserializer, QuoteSyncState state)
        : base(linkedDeserializer, state) { }

    protected override void ProcessUpdate(DispatchContext dispatchContext)
    {
        var msgHeader = dispatchContext.MessageHeader as PQQuoteTransmissionHeader;
        if (msgHeader == null) return;
        if (msgHeader.SequenceId == LinkedDeserializer.PublishedQuote.PQSequenceId + 1 ||
            msgHeader.SequenceId == LinkedDeserializer.PublishedQuote.PQSequenceId)
            ProcessNextExpectedUpdate(dispatchContext, msgHeader.SequenceId);
        else
            ProcessUnsyncedUpdateMessage(dispatchContext, msgHeader.SequenceId);
    }

    protected override void ProcessNextExpectedUpdate(DispatchContext dispatchContext, uint sequenceId)
    {
        base.ProcessNextExpectedUpdate(dispatchContext, sequenceId);
        LastSuccessfulUpdateSequienceId = sequenceId;
        PublishQuoteRunAction(PQSyncStatus.Good, dispatchContext.DispatchLatencyLogger,
            LinkedDeserializer.OnReceivedUpdate);
    }

    protected override void ProcessUnsyncedUpdateMessage(DispatchContext dispatchContext, uint sequenceId)
    {
        if (LinkedDeserializer.AllowUpdatesCatchup && sequenceId < LinkedDeserializer.PublishedQuote.PQSequenceId
                                                   && sequenceId > LastSuccessfulUpdateSequienceId &&
                                                   LastSuccessfulUpdateSequienceId > 0)
        {
            if (LogCounter % 100 == 0)
                Logger.Info("Sequence anomaly ignored on stream {0}, PrevSeqID={1}, RecvSeqID={2}",
                    LinkedDeserializer.Identifier,
                    LinkedDeserializer.PublishedQuote.PQSequenceId, sequenceId);
            LogCounter++;
            return;
        }

        base.ProcessUnsyncedUpdateMessage(dispatchContext, sequenceId);
        LinkedDeserializer.ClearSyncRing();
        SaveMessageToSyncSlot(dispatchContext, sequenceId);
        Logger.Info("Sequence anomaly detected on stream {0}, PrevSeqID={1}, RecvSeqID={2}, WakeUpTs={3}, " +
                    "DeserializeTs={4}, ReceivingTimestamp={5}",
            LinkedDeserializer.Identifier, LinkedDeserializer.PublishedQuote.PQSequenceId, sequenceId,
            dispatchContext.DetectTimestamp.ToString(DateTimeFormat),
            dispatchContext.DeserializerTimestamp.ToString(DateTimeFormat),
            dispatchContext.ReceivingTimestamp.ToString(DateTimeFormat));
        SwitchState(QuoteSyncState.Synchronising);
        PublishQuoteRunAction(PQSyncStatus.OutOfSync, dispatchContext.DispatchLatencyLogger,
            LinkedDeserializer.OnOutOfSync);
    }

    public override bool HasJustGoneStale(DateTime utcNow)
    {
        int elapsed;
        if ((elapsed = (int)(utcNow - LinkedDeserializer.PublishedQuote.ClientReceivedTime).TotalMilliseconds) <=
            PQTimeoutMs) return false;
        Logger.Info("Stale detected on stream {0}, {1}ms elapsed with no update",
            LinkedDeserializer.Identifier, elapsed);
        SwitchState(QuoteSyncState.Stale);
        LinkedDeserializer.PushQuoteToSubscribers(PQSyncStatus.Stale);
        return true;
    }
}
