namespace FortitudeMarketsCore.Trading.ORX.Session;

public enum TradingMessageIds : ushort
{
    SubmitRequest = 11
    , SubmitRejectResponse = 12
    , CancelRequest = 13
    , CancelRejectResponse = 14
    , OrderUpdate = 15
    , VenueOrderUpdate = 16
    , OrderReplayRequest = 17
    , OrderReplayComplete = 18
    , ExecutionUpdate = 19
    , Replay = 20
    , ExecutionReplayComplete = 21
    , Heartbeat = 23
    , Ticker = 24
    , Amend = 25
    , AmendReject = 26
    , GetOrderBook = 27
    , GetTradeBook = 28
    , StatusUpdate = 29
    , AccountEntry = 30
}
