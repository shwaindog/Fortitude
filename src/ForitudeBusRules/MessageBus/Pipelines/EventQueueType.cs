namespace FortitudeBusRules.MessageBus.Pipelines;

[Flags]
public enum EventQueueType
{
    None = 0
    , IOOutbound = 1
    , IOInbound = 2
    , Event = 4
    , Worker = 8
    , Custom = 16
    , All = 31
}

public static class EventQueueTypeExtensions
{
    public static bool IsEvent(this EventQueueType eventQueueType) => (eventQueueType & EventQueueType.Event) != 0;
    public static bool IsWorker(this EventQueueType eventQueueType) => (eventQueueType & EventQueueType.Worker) != 0;

    public static bool IsIOOutbound(this EventQueueType eventQueueType) =>
        (eventQueueType & EventQueueType.IOOutbound) != 0;

    public static bool IsIOInbound(this EventQueueType eventQueueType) =>
        (eventQueueType & EventQueueType.IOInbound) != 0;

    public static bool IsCustom(this EventQueueType eventQueueType) => (eventQueueType & EventQueueType.Custom) != 0;
    public static bool IsAll(this EventQueueType eventQueueType) => eventQueueType == EventQueueType.All;
    public static bool IsNone(this EventQueueType eventQueueType) => eventQueueType == EventQueueType.None;
}
