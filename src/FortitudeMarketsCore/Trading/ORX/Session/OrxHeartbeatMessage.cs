#region

using FortitudeIO.Protocols.Authentication;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Session;

public sealed class OrxHeartbeatMessage : OrxTradingMessage
{
    public OrxHeartbeatMessage() { }

    private OrxHeartbeatMessage(OrxHeartbeatMessage toClone)
    {
        CopyFrom(toClone);
    }

    public override uint MessageId => (uint)TradingMessageIds.Heartbeat;

    public override IAuthenticatedMessage Clone() =>
        (IAuthenticatedMessage?)Recycler?.Borrow<OrxHeartbeatMessage>().CopyFrom(this) ?? new OrxHeartbeatMessage(this);
}
