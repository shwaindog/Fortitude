using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsCore.Trading.ORX.Executions;
using FortitudeMarketsCore.Trading.ORX.Serialization;
using FortitudeMarketsCore.Trading.ORX.Session;

namespace FortitudeMarketsCore.Trading.ORX.Orders.Client
{
    public class OrxGetOrderBookMessage : OrxTradingMessage
    {
        public override uint MessageId => (uint) TradingMessageIds.GetOrderBook;

        [OrxMandatoryField(10)]
        public OrxAccountEntry OrxAccount { get; }

        [OrxOptionalField(12)]
        public OrxInactiveTrades OrxInactiveTrades { get; }

        public OrxGetOrderBookMessage()
        {
        }

        public OrxGetOrderBookMessage(OrxAccountEntry orxAccount)
        {
            OrxAccount = orxAccount;
        }

        public OrxGetOrderBookMessage(string account, bool getInactiveOrders = false)
        {

        }
        public OrxGetOrderBookMessage(MutableString account, bool getInactiveOrders = false)
        {
            OrxAccount = new OrxAccountEntry(account);
            if (getInactiveOrders)
            {
                OrxInactiveTrades = new OrxInactiveTrades(getInactiveOrders);
            }
        }
    }
}
