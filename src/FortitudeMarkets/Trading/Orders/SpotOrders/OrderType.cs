namespace FortitudeMarkets.Trading.Orders.SpotOrders;

[Flags]
public enum OrderType
{
    Unknown        = 0x00
  , IsParent       = 0x01
  , IsInternalOnly = 0x02
  , Market         = 0x04
  , Stop           = 0x08
  , AggressiveLimit = 0x10
  , PassiveLimit    = 0x20
  , Composite       = 0x40
}
