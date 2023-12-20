namespace FortitudeMarketsApi.Trading.Orders.Server;

[Flags]
public enum OrderUpdateEventType : uint
{
    Unknown = 0x00_0000
    , OrderSent = 0x00_0001
    , OrderUnsent = 0x00_0002
    , OrderAcknowledged = 0x00_0004
    , OrderAccepted = 0x00_0008
    , OrderRejected = 0x00_0010
    , OrderActive = 0x00_0020
    , Execution = 0x00_0040
    , ExecutionAmended = 0x00_0080
    , OrderAmended = 0x00_0100
    , OrderPaused = 0x00_0200
    , OrderResumed = 0x00_0400
    , OrderCancelSent = 0x00_0800
    , OrderCancelNotSent = 0x00_1000
    , OrderCancelRejected = 0x00_2000
    , OrderCancelled = 0x00_4000
    , OrderDead = 0x00_8000
    , OrderReplay = 0x01_0000
    , Venue = 0x02_0000
    , AllExceptError = 0x02_FFFF
    , Error = 0x8000_0000
}
