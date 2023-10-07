using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsCore.Trading.ORX.Serialization;

namespace FortitudeMarketsCore.Trading.ORX.Executions
{
    public class OrxInactiveTrades
    {
        [OrxMandatoryField(0)]
        public bool GetInactiveOrders { get; set; }

        public OrxInactiveTrades(bool getInactiveOrders)
        {
            GetInactiveOrders = getInactiveOrders;
        }
    }
}