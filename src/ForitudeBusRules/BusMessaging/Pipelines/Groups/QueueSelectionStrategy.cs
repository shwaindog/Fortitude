namespace FortitudeBusRules.BusMessaging.Pipelines.Groups;

public enum QueueSelectionStrategy
{
    Any = 1
    , FirstInSet
    , LastInSet
    , Random
    , EarliestStarted
    , EarliestCompleted
    , LatestStarted
    , LatestCompleted
    , LowestQueueEntryPosition
    , HighestQueueEntryPosition
    , LeastBusy
}
