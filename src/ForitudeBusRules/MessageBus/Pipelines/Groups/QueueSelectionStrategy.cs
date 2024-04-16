namespace FortitudeBusRules.MessageBus.Pipelines.Groups;

public enum QueueSelectionStrategy
{
    Any
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
