// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeMarkets.Trading.ORX.Session;

#endregion

namespace FortitudeMarkets.Trading.ORX.Executions;

public class OrxGetTradeBookMessage : OrxTradingMessage
{
    public OrxGetTradeBookMessage() { }

    public OrxGetTradeBookMessage(OrxAccountEntry orxAccount) => OrxAccount = orxAccount;

    public OrxGetTradeBookMessage(uint account) => OrxAccount = new OrxAccountEntry(account);

    private OrxGetTradeBookMessage(OrxGetTradeBookMessage toClone)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public override uint MessageId => (uint)TradingMessageIds.GetTradeBook;

    [OrxMandatoryField(10)] public OrxAccountEntry? OrxAccount { get; set; }

    public override IVersionedMessage CopyFrom
        (IVersionedMessage source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is OrxGetTradeBookMessage tradeBookMessage) OrxAccount = tradeBookMessage.OrxAccount.SyncOrRecycle(OrxAccount);

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
