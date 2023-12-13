#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.Authentication;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading;
using FortitudeMarketsCore.Trading.ORX.Executions;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Session;

public class OrxOrdersReceivedComplete : OrxTradingMessage
{
    public OrxOrdersReceivedComplete() { }
    public OrxOrdersReceivedComplete(ITradingMessage toClone) : base(toClone) { }

    public OrxOrdersReceivedComplete(byte versionNumber, int sequenceNumber,
        bool isReplay, DateTime sendTime, string machineName, string senderName,
        DateTime originalSendTime, string info, OrxUserData userData) :
        base(versionNumber, sequenceNumber, isReplay, sendTime, machineName,
            senderName, originalSendTime, info, userData) { }

    public OrxOrdersReceivedComplete(byte versionNumber, int sequenceNumber, bool isReplay,
        DateTime sendTime, MutableString machineName, MutableString senderName,
        DateTime originalSendTime, MutableString info, OrxUserData userData) :
        base(versionNumber, sequenceNumber, isReplay, sendTime, machineName,
            senderName, originalSendTime, info, userData) { }

    public override uint MessageId => (uint)TradingMessageIds.OrderReplayComplete;

    [OrxOptionalField(9)] public OrxAccountEntry? OrxAccount { get; set; }

    public override IVersionedMessage CopyFrom(IVersionedMessage source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is OrxOrdersReceivedComplete ordersReceivedComplete)
            OrxAccount = ordersReceivedComplete.OrxAccount.SyncOrRecycle(OrxAccount);

        return this;
    }

    public override void StateReset()
    {
        OrxAccount?.DecrementRefCount();
        OrxAccount = null;
        base.StateReset();
    }

    public override IAuthenticatedMessage Clone() =>
        (IAuthenticatedMessage?)Recycler?.Borrow<OrxOrdersReceivedComplete>().CopyFrom(this) ??
        new OrxOrdersReceivedComplete(this);
}
