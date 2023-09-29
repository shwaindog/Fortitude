using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serialization;

namespace FortitudeMarketsCore.Trading.ORX.Session
{
    public sealed class OrxStatusMessage : OrxTradingMessage
    {
        public override uint MessageId => (uint)TradingMessageIds.StatusUpdate;

        [OrxMandatoryField(10)]
        public OrxExchangeStatus ExchangeStatus { get; set; }
        [OrxMandatoryField(11)]
        public MutableString ExchangeName { get; set; }

        public OrxStatusMessage() { }

        public OrxStatusMessage(OrxExchangeStatus orxExchangeStatus, MutableString exchangeName)
        {
            ExchangeStatus = orxExchangeStatus;
            ExchangeName = exchangeName;
        }
        
        public void Configure(OrxExchangeStatus orxExchangeStatus, string exchangeName,
            IRecycler orxRecyclingFactory)
        {
            Configure();
            ExchangeStatus = orxExchangeStatus;
            var mutableExchangeNameString = orxRecyclingFactory.Borrow<MutableString>();
            mutableExchangeNameString.Clear().Append(exchangeName);
            ExchangeName = mutableExchangeNameString;
        }

    }
}
