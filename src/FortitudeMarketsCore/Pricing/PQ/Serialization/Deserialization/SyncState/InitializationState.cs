#region

using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Subscription;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState;

public class InitializationState<T> : SynchronisingState<T> where T : PQLevel0Quote, new()
{
    public InitializationState(IPQQuoteDeserializer<T> linkedDeserializer) : base(linkedDeserializer,
        QuoteSyncState.InitializationState) { }

    protected override void ProcessUnsyncedUpdateMessage(DispatchContext dispatchContext, uint sequenceId)
    {
        base.ProcessUnsyncedUpdateMessage(dispatchContext, sequenceId);
        SwitchState(QuoteSyncState.Synchronising);
    }

    protected override void LogSyncRecoveryMessage(uint sequenceId)
    {
        Logger.Info("Stream {0} started from first message, RecvSeqID={1}",
            LinkedDeserializer.Identifier, sequenceId);
    }

    protected override void ProcessUpdate(DispatchContext dispatchContext)
    {
        var msgHeader = dispatchContext.MessageHeader as PQQuoteTransmissionHeader;
        if (msgHeader == null) return;
        if (msgHeader.SequenceId == 0)
            ProcessNextExpectedUpdate(dispatchContext, msgHeader.SequenceId);
        else
            ProcessUnsyncedUpdateMessage(dispatchContext, msgHeader.SequenceId);
    }
}
