namespace FortitudeMarkets.Trading.Orders;

[Flags]
public enum TimeInForce
{
    None = 0x00
    , GoodTillCancelled = 0x01
    , GoodTillTime = 0x02
    , GoodTillEndOfDay = 0x04
    , ImmediateOrCancel = 0x08
    , FillOrKill = 0x10
}
