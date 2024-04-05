#region

using FortitudeCommon.Serdes.Binary;
using FortitudeMarketsCore.Pricing.PQ.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization.SyncState;

public class InitializationState<T> : SynchronisingState<T> where T : PQLevel0Quote, new()
{
    public InitializationState(IPQQuoteDeserializer<T> linkedDeserializer) : base(linkedDeserializer,
        QuoteSyncState.InitializationState) { }

    protected override void ProcessUnsyncedUpdateMessage(IBufferContext bufferContext, uint sequenceId)
    {
        base.ProcessUnsyncedUpdateMessage(bufferContext, sequenceId);
        SwitchState(QuoteSyncState.Synchronising);
    }

    protected override void LogSyncRecoveryMessage(uint sequenceId)
    {
        Logger.Info("Stream {0} started from first message, RecvSeqID={1}",
            LinkedDeserializer.Identifier, sequenceId);
    }

    protected override void ProcessUpdate(IBufferContext bufferContext)
    {
        var sequenceId = bufferContext.ReadCurrentMessageSequenceId();
        if (sequenceId == 0)
            ProcessNextExpectedUpdate(bufferContext, sequenceId);
        else
            ProcessUnsyncedUpdateMessage(bufferContext, sequenceId);
    }
}
