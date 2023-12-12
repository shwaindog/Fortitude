#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsCore.Trading.ORX.Session;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Executions;

public class OrxGetTradeBookMessage : OrxTradingMessage
{
    public OrxGetTradeBookMessage() { }

    public OrxGetTradeBookMessage(OrxAccountEntry orxAccount) => OrxAccount = orxAccount;

    public OrxGetTradeBookMessage(string account)
        : this((MutableString)account) { }

    public OrxGetTradeBookMessage(MutableString account) => OrxAccount = new OrxAccountEntry(account);

    private OrxGetTradeBookMessage(OrxGetTradeBookMessage toClone)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public override uint MessageId => (uint)TradingMessageIds.GetTradeBook;

    [OrxMandatoryField(10)] public OrxAccountEntry? OrxAccount { get; set; }

    public override IVersionedMessage CopyFrom(IVersionedMessage source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is OrxGetTradeBookMessage tradeBookMessage)
            OrxAccount = tradeBookMessage.OrxAccount.SyncOrRecycle(OrxAccount);

        return this;
    }

    public override void StateReset()
    {
        OrxAccount?.DecrementRefCount();
        OrxAccount = null;
        base.StateReset();
    }

    public override IAuthenticatedMessage Clone() =>
        (IAuthenticatedMessage?)Recycler?.Borrow<OrxGetTradeBookMessage>().CopyFrom(this) ??
        new OrxGetTradeBookMessage(this);
}
