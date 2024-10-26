namespace FortitudeMarkets.Trading.Orders;

public enum OrderStatus
{
    New = 'N'
    , PendingNew = 'P'
    , Active = 'A'
    , Cancelling = 'C'
    , Dead = 'D'
    , Frozen = 'F'
    , Unknown = 'U'
}
