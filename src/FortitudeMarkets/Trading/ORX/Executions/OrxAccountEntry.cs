// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeMarkets.Trading.ORX.Session;

#endregion

namespace FortitudeMarkets.Trading.ORX.Executions;

public class OrxAccountEntry : OrxTradingMessage, ITransferState<OrxAccountEntry>
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

    public OrxAccountEntry CopyFrom(OrxAccountEntry source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        (OrxAccountEntry)CopyFrom((IVersionedMessage)source, copyMergeFlags);

    public override IVersionedMessage CopyFrom
    (IVersionedMessage source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        if (source is OrxAccountEntry accountEntry) Account = accountEntry.Account.SyncOrRecycle(Account);

        return this;
    }

    public override void StateReset()
    {
        Account?.DecrementRefCount();
        Account = null;

        base.StateReset();
    }

    public override IAuthenticatedMessage Clone() =>
        (IAuthenticatedMessage?)Recycler?.Borrow<OrxAccountEntry>().CopyFrom(this) ?? new OrxAccountEntry(this);
}
