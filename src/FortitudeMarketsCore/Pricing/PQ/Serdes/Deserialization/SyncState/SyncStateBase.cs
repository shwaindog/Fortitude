#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization.SyncState;

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

    public virtual void ProcessInState(IMessageBufferContext bufferContext)
    {
        var msgFlags = (PQMessageFlags)bufferContext.MessageHeader.Flags;
        if ((msgFlags & PQMessageFlags.Snapshot) > 0)
            ProcessSnapshot(bufferContext);
        else
            ProcessUpdate(bufferContext);
    }

    protected virtual void ProcessUpdate(IMessageBufferContext bufferContext)
    {
        var sequenceId = bufferContext.ReadCurrentMessageSequenceId();
        if (sequenceId == LinkedDeserializer.PublishedQuote.PQSequenceId + 1 ||
            (sequenceId == 0 && LinkedDeserializer.PublishedQuote.PQSequenceId == 0))
            ProcessNextExpectedUpdate(bufferContext, sequenceId);
        else
            ProcessUnsyncedUpdateMessage(bufferContext, sequenceId);
    }

    public virtual void ProcessSnapshot(IMessageBufferContext bufferContext)
    {
        var sequenceId = bufferContext.ReadCurrentMessageSequenceId();
        if (sequenceId > LinkedDeserializer.PublishedQuote.PQSequenceId + 1)
        {
            Logger.Info("Received snapshot for {0} with sequence id {1} that is infront of update stream {2}",
                LinkedDeserializer.Identifier, sequenceId, LinkedDeserializer.PublishedQuote.PQSequenceId);
            LinkedDeserializer.UpdateQuote(bufferContext, LinkedDeserializer.PublishedQuote, sequenceId);
        }
        else
        {
            Logger.Info("Received unexpected or no longer required snapshot for stream {0}",
                LinkedDeserializer.Identifier);
        }
    }

    protected virtual void ProcessNextExpectedUpdate(IMessageBufferContext bufferContext, uint sequenceId)
    {
        LinkedDeserializer.UpdateQuote(bufferContext, LinkedDeserializer.PublishedQuote, sequenceId);
    }

    protected virtual void ProcessUnsyncedUpdateMessage(IMessageBufferContext bufferContext, uint sequenceId)
    {
        if (sequenceId < LinkedDeserializer.PublishedQuote.PQSequenceId)
        {
            if (LogCounter % 100 == 0)
            {
                if (bufferContext is SocketBufferReadContext sockBuffContext)
                    Logger.Info("Unexpected sequence Id (#{0}) on stream {1}, PrevSeqID={2}, RecvSeqID={3}, " +
                                "WakeUpTs={4}, DeserializeTs={5}, ReceivingTimestamp={6}",
                        LogCounter, LinkedDeserializer.Identifier, LinkedDeserializer.PublishedQuote.PQSequenceId,
                        sequenceId, sockBuffContext.DetectTimestamp.ToString(DateTimeFormat),
                        sockBuffContext.DeserializerTime.ToString(DateTimeFormat),
                        sockBuffContext.ReceivingTimestamp.ToString(DateTimeFormat));
                else
                    Logger.Info("Unexpected sequence Id (#{0}) on stream {1}, PrevSeqID={2}, RecvSeqID={3} ",
                        LogCounter, LinkedDeserializer.Identifier, LinkedDeserializer.PublishedQuote.PQSequenceId,
                        sequenceId);
            }

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

    protected void SaveMessageToSyncSlot(IMessageBufferContext bufferContext, uint sequenceId)
    {
        var ent = LinkedDeserializer.ClaimSyncSlotEntry();
        ent.HasUpdates = false;
        LinkedDeserializer.UpdateQuote(bufferContext, ent, sequenceId);
    }
}
