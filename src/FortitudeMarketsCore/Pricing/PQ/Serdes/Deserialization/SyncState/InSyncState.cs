// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization.SyncState;

public class InSyncState<T> : SyncStateBase<T> where T : PQLevel0Quote, new()
{
    internal const uint PQTimeoutMs = 2000;

    protected long LastSuccessfulUpdateSequienceId = -1;

    public InSyncState(IPQQuotePublishingDeserializer<T> linkedDeserializer)
        : base(linkedDeserializer, QuoteSyncState.InSync) { }

    protected InSyncState(IPQQuotePublishingDeserializer<T> linkedDeserializer, QuoteSyncState state)
        : base(linkedDeserializer, state) { }

    protected override void ProcessUpdate(IMessageBufferContext bufferContext)
    {
        var sequenceId = bufferContext.ReadCurrentMessageSequenceId();
        if (sequenceId == LinkedDeserializer.PublishedQuote.PQSequenceId + 1 ||
            sequenceId == LinkedDeserializer.PublishedQuote.PQSequenceId)
            ProcessNextExpectedUpdate(bufferContext, sequenceId);
        else
            ProcessUnsyncedUpdateMessage(bufferContext, sequenceId);
    }

    protected override void ProcessNextExpectedUpdate(IMessageBufferContext bufferContext, uint sequenceId)
    {
        base.ProcessNextExpectedUpdate(bufferContext, sequenceId);
        LastSuccessfulUpdateSequienceId = sequenceId;
        var sockBuffContext = bufferContext as SocketBufferReadContext;
        PublishQuoteRunAction(PQSyncStatus.Good, sockBuffContext?.DispatchLatencyLogger,
                              LinkedDeserializer.OnReceivedUpdate);
    }

    protected override void ProcessUnsyncedUpdateMessage(IMessageBufferContext bufferContext, uint sequenceId)
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

        base.ProcessUnsyncedUpdateMessage(bufferContext, sequenceId);
        LinkedDeserializer.ClearSyncRing();
        SaveMessageToSyncSlot(bufferContext, sequenceId);
        var sockBuffContext = bufferContext as SocketBufferReadContext;
        if (sockBuffContext != null)
            Logger.Info("Sequence anomaly detected on stream {0}, PrevSeqID={1}, RecvSeqID={2}, WakeUpTs={3}, " +
                        "DeserializeTs={4}, ReceivingTimestamp={5}",
                        LinkedDeserializer.Identifier, LinkedDeserializer.PublishedQuote.PQSequenceId, sequenceId,
                        sockBuffContext.DetectTimestamp.ToString(DateTimeFormat),
                        sockBuffContext.DeserializerTime.ToString(DateTimeFormat),
                        sockBuffContext.ReceivingTimestamp.ToString(DateTimeFormat));
        else
            Logger.Info("Sequence anomaly detected on stream {0}, PrevSeqID={1}, RecvSeqID={2}",
                        LinkedDeserializer.Identifier, LinkedDeserializer.PublishedQuote.PQSequenceId, sequenceId);

        SwitchState(QuoteSyncState.Synchronising);
        PublishQuoteRunAction(PQSyncStatus.OutOfSync, sockBuffContext?.DispatchLatencyLogger,
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
