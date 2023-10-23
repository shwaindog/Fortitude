#region

using FortitudeCommon.Types.Mutable;
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

    public override uint MessageId => (uint)TradingMessageIds.GetTradeBook;

    [OrxMandatoryField(10)] public OrxAccountEntry? OrxAccount { get; }
}
