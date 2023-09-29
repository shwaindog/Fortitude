using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsCore.Trading.ORX.Session;

namespace FortitudeMarketsCore.Trading.ORX.Executions
{
    public class OrxAccountEntry : OrxTradingMessage
    {
        public override uint MessageId => (uint) TradingMessageIds.AccountEntry;

        [OrxMandatoryField(0)]
        public MutableString Account { get; }


        public OrxAccountEntry(string account)
            : this((MutableString)account)
        {
        }
        public OrxAccountEntry(MutableString account)
        {
            Account = account;
        }

        public OrxAccountEntry() {}
    }
}
