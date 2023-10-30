using System;

namespace FortitudeMarketsCore.Trading.Orders
{
    public class MarketPriceQuoteId
    {
        public MarketPriceQuoteId(ushort source, ushort ticker, DateTime timeStamp)
        {
            MarketId = source;
            TickerId = ticker;
            TimeStamp = timeStamp;
        }

        public ushort MarketId { get; }
        public ushort TickerId { get; }
        public uint SourceRefId { get; }
        public DateTime TimeStamp { get; }
    }
}