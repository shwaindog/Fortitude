namespace FortitudeMarketsCore.Trading.ORX.Session;

public class OrxExecutionsReceivedComplete : OrxOrdersReceivedComplete
{
    public override uint MessageId => (uint)TradingMessageIds.ExecutionReplayComplete;
}
