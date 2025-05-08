#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeMarkets.Trading.Executions;
using FortitudeMarkets.Trading.Orders.Server;
using FortitudeMarkets.Trading.Replay;
using FortitudeMarkets.Trading.ORX.Executions;
using FortitudeMarkets.Trading.ORX.Orders.Server;
using FortitudeMarkets.Trading.ORX.Session;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarkets.Trading.ORX.Replay;

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

    public override void StateReset()
    {
        PastOrder?.DecrementRefCount();
        PastOrder = null;
        PastExecutionUpdate?.DecrementRefCount();
        PastExecutionUpdate = null;
        ReplayMessageType = ReplayMessageType.PastOrder;
        base.StateReset();
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
