#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeMarketsCore.Trading.ORX.Executions;
using FortitudeMarketsCore.Trading.ORX.Session;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Orders.Client;

public class OrxGetOrderBookMessage : OrxTradingMessage
{
    public OrxGetOrderBookMessage() { }

    public OrxGetOrderBookMessage(OrxAccountEntry orxAccount) => OrxAccount = orxAccount;

    public OrxGetOrderBookMessage(string account, bool getInactiveOrders = false) { }

    public OrxGetOrderBookMessage(MutableString account, bool getInactiveOrders = false)
    {
        OrxAccount = new OrxAccountEntry(account);
        if (getInactiveOrders) OrxInactiveTrades = new OrxInactiveTrades(getInactiveOrders);
    }

    private OrxGetOrderBookMessage(OrxGetOrderBookMessage toClone)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public override uint MessageId => (uint)TradingMessageIds.GetOrderBook;

    [OrxMandatoryField(10)] public OrxAccountEntry? OrxAccount { get; set; }

    [OrxOptionalField(12)] public OrxInactiveTrades? OrxInactiveTrades { get; set; }

    public override IVersionedMessage CopyFrom(IVersionedMessage source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is OrxGetOrderBookMessage orderBookMessage)
        {
            OrxAccount = orderBookMessage.OrxAccount.SyncOrRecycle(OrxAccount);
            OrxInactiveTrades = orderBookMessage.OrxInactiveTrades.SyncOrRecycle(OrxInactiveTrades);
        }

        return this;
    }

    public override void StateReset()
    {
        OrxAccount?.DecrementRefCount();
        OrxAccount = null;
        OrxInactiveTrades?.DecrementRefCount();
        OrxInactiveTrades = null;
        base.StateReset();
    }

    public override IAuthenticatedMessage Clone() =>
        (IAuthenticatedMessage?)Recycler?.Borrow<OrxGetOrderBookMessage>().CopyFrom(this) ??
        new OrxGetOrderBookMessage(this);
}
