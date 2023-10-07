using System;
using System.Text;

namespace FortitudeMarketsApi.Trading.Executions
{
    public enum TradedSide
    {
        Given,
        Paid
    }

    public class MktTrade
    {
        public readonly string Source;
        public readonly DateTime Timestamp;
        public readonly string Ticker;
        public readonly decimal Price;
        public readonly TradedSide Side;

        public MktTrade(string source, DateTime timestamp, string ticker, decimal price, TradedSide side)
        {
            Source = source;
            Timestamp = timestamp;
            Ticker = ticker;
            Price = price;
            Side = side;
        }

        public override string ToString()
        {
            return new StringBuilder(128)
                .Append(Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff")).Append(';')
                .Append(Ticker).Append(";")
                .Append(Price).Append(';')
                .Append(Side)
                .ToString();
        }
    }
}
