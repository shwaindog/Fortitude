namespace FortitudeMarkets.Trading.Orders.Venues;

[Flags]
public enum VenueSelectionMethod
{
    Default = 0
    , Slippage = 0x01
    , FillRatio = 0x02
    , Brokerage = 0x04
    , NonLastLookOnly = 0x08
    , Internal = 0x10
    , ReferenceMarketsLast = 0x20
    , HighestOutstandingPosition = 0x40
    , OldestOutstandingPosition = 0x80
}
