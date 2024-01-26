#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Subscription;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState;

public abstract class SyncStateBase<T> where T : PQLevel0Quote, new()
{
    protected const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.ffffff";

    protected static IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQQuoteDeserializer<T>));
    protected int LogCounter;

    internal SyncStateBase(IPQQuoteDeserializer<T> linkedDeserializer, QuoteSyncState state)
    {
        LinkedDeserializer = linkedDeserializer;
        State = state;
    }

    public IPQQuoteDeserializer<T> LinkedDeserializer { get; }

    public QuoteSyncState State { get; protected set; }

    public virtual bool EligibleForResync(DateTime utcNow) => false;

    public virtual bool HasJustGoneStale(DateTime utcNow) => false;

    public virtual void ProcessInState(DispatchContext dispatchContext)
    {
        var msgHeader = dispatchContext.MessageHeader as PQQuoteTransmissionHeader;
        if (msgHeader != null && msgHeader.Origin == PQFeedType.Update)
            ProcessUpdate(dispatchContext);
        else
            ProcessSnapshot(dispatchContext);
    }

    protected virtual void ProcessUpdate(DispatchContext dispatchContext)
    {
        var msgHeader = dispatchContext.MessageHeader as PQQuoteTransmissionHeader;
        if (msgHeader == null) return;
        if (msgHeader.SequenceId == LinkedDeserializer.PublishedQuote.PQSequenceId + 1)
            ProcessNextExpectedUpdate(dispatchContext, msgHeader.SequenceId);
        else
            ProcessUnsyncedUpdateMessage(dispatchContext, msgHeader.SequenceId);
    }

    public virtual void ProcessSnapshot(DispatchContext dispatchContext)
    {
        Logger.Info("Received unexpected or no longer required snapshot for stream {0}",
            LinkedDeserializer.Identifier);
    }

    protected virtual void ProcessNextExpectedUpdate(DispatchContext dispatchContext, uint sequenceId)
    {
        LinkedDeserializer.UpdateQuote(dispatchContext, LinkedDeserializer.PublishedQuote, sequenceId);
    }

    protected virtual void ProcessUnsyncedUpdateMessage(DispatchContext dispatchContext, uint sequenceId)
    {
        if (sequenceId < LinkedDeserializer.PublishedQuote.PQSequenceId)
        {
            if (LogCounter % 100 == 0)
                Logger.Info("Unexpected sequence Id (#{0}) on stream {1}, PrevSeqID={2}, RecvSeqID={3}, " +
                            "WakeUpTs={4}, DeserializeTs={5}, ReceivingTimestamp={6}",
                    LogCounter, LinkedDeserializer.Identifier, LinkedDeserializer.PublishedQuote.PQSequenceId,
                    sequenceId, dispatchContext.DetectTimestamp.ToString(DateTimeFormat),
                    dispatchContext.DeserializerTimestamp.ToString(DateTimeFormat),
                    dispatchContext.ReceivingTimestamp.ToString(DateTimeFormat));
            LogCounter++;
        }
    }

    protected void PublishQuoteRunAction(PQSyncStatus syncStatus, IPerfLogger? dispatchLatencyLogger,
        Action<IPQDeserializer> syncStateAction)
    {
        LinkedDeserializer.PushQuoteToSubscribers(syncStatus, dispatchLatencyLogger);
        syncStateAction?.Invoke(LinkedDeserializer);
    }

    protected void SwitchState(QuoteSyncState newState)
    {
        LinkedDeserializer.SwitchSyncState(newState);
    }

    protected void SaveMessageToSyncSlot(DispatchContext dispatchContext, uint sequenceId)
    {
        var ent = LinkedDeserializer.ClaimSyncSlotEntry();
        ent.HasUpdates = false;
        LinkedDeserializer.UpdateQuote(dispatchContext, ent, sequenceId);
    }
}
