namespace FortitudeMarketsCore.Trading.ORX.Orders.Server
{
    public enum OrxOrderEvent : byte
    {
        Unknown = 0,
        Unsent = 1,
        Sent = 2,
        Active = 3,
        Rejected = 4,
        CancelSent = 5,
        CancelUnsent = 6,
        CancelRejected = 7,
        Deleted = 8,
        Cancelled = 9,
        NotUsed = 10,
        Error = 42
    }
}