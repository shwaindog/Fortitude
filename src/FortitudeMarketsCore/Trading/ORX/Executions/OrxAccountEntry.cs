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

public class OrxAccountEntry : OrxTradingMessage
{
    public OrxAccountEntry() { }

    public OrxAccountEntry(string account)
        : this((MutableString)account) { }

    public OrxAccountEntry(MutableString account) => Account = account;

    private OrxAccountEntry(OrxAccountEntry toClone)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public override uint MessageId => (uint)TradingMessageIds.AccountEntry;

    [OrxMandatoryField(10)] public MutableString? Account { get; set; }

    public override IVersionedMessage CopyFrom(IVersionedMessage source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        if (source is OrxAccountEntry accountEntry) Account = accountEntry.Account.SyncOrRecycle(Account);

        return this;
    }

    public override void Reset()
    {
        Account?.DecrementRefCount();
        Account = null;

        base.Reset();
    }

    public override IAuthenticatedMessage Clone() =>
        (IAuthenticatedMessage?)Recycler?.Borrow<OrxAccountEntry>().CopyFrom(this) ?? new OrxAccountEntry(this);
}
