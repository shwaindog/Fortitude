#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsApi.Trading.Orders.Server;
using FortitudeMarketsApi.Trading.Replay;
using FortitudeMarketsCore.Trading.ORX.Executions;
using FortitudeMarketsCore.Trading.ORX.Orders.Server;
using FortitudeMarketsCore.Trading.ORX.Session;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Replay;

public class OrxReplayMessage : OrxTradingMessage, IReplayMessage
{
    public OrxReplayMessage() { }

    private OrxReplayMessage(OrxReplayMessage toClone)
    {
        CopyFrom(toClone);
    }

    [OrxOptionalField(11)] public OrxOrderUpdate? PastOrder { get; set; }

    [OrxOptionalField(12)] public OrxExecutionUpdate? PastExecutionUpdate { get; set; }

    public override uint MessageId => (uint)TradingMessageIds.Replay;

    [OrxMandatoryField(10)] public ReplayMessageType ReplayMessageType { get; set; }

    IOrderUpdate? IReplayMessage.PastOrder
    {
        get => PastOrder;
        set => PastOrder = value as OrxOrderUpdate;
    }

    IExecutionUpdate? IReplayMessage.PastExecutionUpdate
    {
        get => PastExecutionUpdate;
        set => PastExecutionUpdate = value as OrxExecutionUpdate;
    }

    public override IVersionedMessage CopyFrom(IVersionedMessage source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        CopyFrom((IReplayMessage)source, copyMergeFlags);

    public override void Reset()
    {
        PastOrder?.DecrementRefCount();
        PastOrder = null;
        PastExecutionUpdate?.DecrementRefCount();
        PastExecutionUpdate = null;
        ReplayMessageType = ReplayMessageType.PastOrder;
        base.Reset();
    }

    IReplayMessage IReplayMessage.Clone() => (IReplayMessage)Clone();

    public IReplayMessage CopyFrom(IReplayMessage source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        PastOrder = source.PastOrder.SyncOrRecycle(PastOrder);
        if (source is OrxReplayMessage replayMessage)
        {
            PastOrder = replayMessage.PastOrder.SyncOrRecycle(PastOrder);
            PastExecutionUpdate = replayMessage.PastExecutionUpdate.SyncOrRecycle(PastExecutionUpdate);
        }

        return this;
    }


    public override IAuthenticatedMessage Clone() =>
        (IAuthenticatedMessage?)Recycler?.Borrow<OrxReplayMessage>().CopyFrom(this) ?? new OrxReplayMessage(this);
}
