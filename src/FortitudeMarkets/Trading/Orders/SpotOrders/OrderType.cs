namespace FortitudeMarkets.Trading.Orders.SpotOrders;

[Flags]
public enum OrderType
{
    Unset = 0x00
    , Market = 0x01
    , Limit = 0x02
    , Stop = 0x04
    , Composite = 0x08
}
