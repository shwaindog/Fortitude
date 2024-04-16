#region

using FortitudeCommon.DataStructures.Lists;

#endregion

namespace FortitudeBusRules.MessageBus.Pipelines.Groups;

public interface IEventQueueList : IReusableList<IEventQueue>
{
    IEventQueue SelectEventQueue(QueueSelectionStrategy selectionStrategy);
}

public class EventQueueList : ReusableList<IEventQueue>, IEventQueueList
{
    private readonly List<long> orderedQueueEntryIds = new();
    private readonly List<DateTime> orderedTimes = new();
    private readonly Random random = new();

    public IEventQueue SelectEventQueue(QueueSelectionStrategy selectionStrategy)
    {
        return selectionStrategy switch
        {
            QueueSelectionStrategy.Any or QueueSelectionStrategy.FirstInSet => this[0], QueueSelectionStrategy.LastInSet => this[^1]
            , QueueSelectionStrategy.Random => this[random.Next(Count)], QueueSelectionStrategy.LeastBusy => LeastBusy()
            , QueueSelectionStrategy.EarliestCompleted => EarliestMessageCompletedTime()
            , QueueSelectionStrategy.EarliestStarted => EarliestMessageStartedTime()
            , QueueSelectionStrategy.LatestCompleted => LatestMessageCompletedTime()
            , QueueSelectionStrategy.LatestStarted => LatestMessageStartedTime()
            , QueueSelectionStrategy.LowestQueueEntryPosition => LowestQueueEntryPosition()
            , QueueSelectionStrategy.HighestQueueEntryPosition => HighestQueueEntryPosition()
        };
    }

    private IEventQueue LeastBusy()
    {
        Sort();
        return this[0];
    }

    private IEventQueue EarliestMessageCompletedTime()
    {
        orderedTimes.Clear();
        orderedTimes.AddRange(this.Select(eq => eq.LatestMessageFinishedProcessing.Time));
        var minTime = orderedTimes.Min();
        var indexOfMinTime = orderedTimes.IndexOf(minTime);
        return this[indexOfMinTime];
    }

    private IEventQueue EarliestMessageStartedTime()
    {
        orderedTimes.Clear();
        orderedTimes.AddRange(this.Select(eq => eq.LatestMessageStartProcessing.Time));
        var minTime = orderedTimes.Min();
        var indexOfMinTime = orderedTimes.IndexOf(minTime);
        return this[indexOfMinTime];
    }

    private IEventQueue LatestMessageCompletedTime()
    {
        orderedTimes.Clear();
        orderedTimes.AddRange(this.Select(eq => eq.LatestMessageFinishedProcessing.Time));
        var minTime = orderedTimes.Min();
        var indexOfMinTime = orderedTimes.IndexOf(minTime);
        return this[indexOfMinTime];
    }

    private IEventQueue LatestMessageStartedTime()
    {
        orderedTimes.Clear();
        orderedTimes.AddRange(this.Select(eq => eq.LatestMessageStartProcessing.Time));
        var minTime = orderedTimes.Min();
        var indexOfMinTime = orderedTimes.IndexOf(minTime);
        return this[indexOfMinTime];
    }

    private IEventQueue LowestQueueEntryPosition()
    {
        orderedQueueEntryIds.Clear();
        orderedQueueEntryIds.AddRange(this.Select(eq => eq.LatestMessageFinishedProcessing.MessageNumber));
        var minTime = orderedQueueEntryIds.Min();
        var indexOfMinTime = orderedQueueEntryIds.IndexOf(minTime);
        return this[indexOfMinTime];
    }

    private IEventQueue HighestQueueEntryPosition()
    {
        orderedQueueEntryIds.Clear();
        orderedQueueEntryIds.AddRange(this.Select(eq => eq.LatestMessageFinishedProcessing.MessageNumber));
        var minTime = orderedQueueEntryIds.Min();
        var indexOfMinTime = orderedQueueEntryIds.IndexOf(minTime);
        return this[indexOfMinTime];
    }
}
