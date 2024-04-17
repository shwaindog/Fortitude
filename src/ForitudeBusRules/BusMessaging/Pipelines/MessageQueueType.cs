namespace FortitudeBusRules.BusMessaging.Pipelines;

[Flags]
public enum MessageQueueType
{
    None = 0
    , IOOutbound = 1 // socket sending
    , IOInbound = 2 // socket listening
    , AllIO = 3 // IOOutbound and IOInbound
    , Event = 4 // fast executing non I/O rules and events, actions (monitored by watch dog to report long running operations)
    , Worker = 8 // un-monitored (via watchdog) for long running I/O rules, database, disk actions
    , Custom = 16 // user custom pool separate to I/O for custom, instance specific work
    , All = 31
}

public static class MessageQueueTypeExtensions
{
    public static bool IsEvent(this MessageQueueType messageQueueType) => (messageQueueType & MessageQueueType.Event) != 0;
    public static bool IsWorker(this MessageQueueType messageQueueType) => (messageQueueType & MessageQueueType.Worker) != 0;

    public static bool IsIOOutbound(this MessageQueueType messageQueueType) => (messageQueueType & MessageQueueType.IOOutbound) != 0;

    public static bool IsIOInbound(this MessageQueueType messageQueueType) => (messageQueueType & MessageQueueType.IOInbound) != 0;

    public static bool IsCustom(this MessageQueueType messageQueueType) => (messageQueueType & MessageQueueType.Custom) != 0;
    public static bool IsAll(this MessageQueueType messageQueueType) => messageQueueType == MessageQueueType.All;
    public static bool IsNone(this MessageQueueType messageQueueType) => messageQueueType == MessageQueueType.None;
}
