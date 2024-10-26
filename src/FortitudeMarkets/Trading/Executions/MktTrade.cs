#region

using System.Text;

#endregion

namespace FortitudeMarkets.Trading.Executions;

public enum TradedSide
{
    Given
    , Paid
}

public class MktTrade
{
    public readonly decimal Price;
    public readonly TradedSide Side;
    public readonly string Source;
    public readonly string Ticker;
    public readonly DateTime Timestamp;

    public MktTrade(string source, DateTime timestamp, string ticker, decimal price, TradedSide side)
    {
        Source = source;
        Timestamp = timestamp;
        Ticker = ticker;
        Price = price;
        Side = side;
    }

    public override string ToString() =>
        new StringBuilder(128)
            .Append(Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff")).Append(';')
            .Append(Ticker).Append(";")
            .Append(Price).Append(';')
            .Append(Side)
            .ToString();
}
