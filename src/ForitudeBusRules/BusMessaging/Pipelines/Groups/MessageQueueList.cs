#region

using FortitudeCommon.DataStructures.Lists;

#endregion

namespace FortitudeBusRules.BusMessaging.Pipelines.Groups;

public interface IMessageQueueList<T> : IReusableList<T> where T : IMessageQueue
{
    T SelectEventQueue(QueueSelectionStrategy selectionStrategy);
}

public class MessageQueueList<T> : ReusableList<T>, IMessageQueueList<T> where T : IMessageQueue
{
    private readonly List<long> orderedQueueEntryIds = new();
    private readonly List<DateTime> orderedTimes = new();
    private readonly Random random = new();

    public T SelectEventQueue(QueueSelectionStrategy selectionStrategy)
    {
        return selectionStrategy switch
        {
            QueueSelectionStrategy.Any or QueueSelectionStrategy.FirstInSet => this[0]
            , QueueSelectionStrategy.LastInSet => this[^1]
            , QueueSelectionStrategy.Random => this[random.Next(Count)]
            , QueueSelectionStrategy.LeastBusy => LeastBusy()
            , QueueSelectionStrategy.EarliestCompleted => EarliestMessageCompletedTime()
            , QueueSelectionStrategy.EarliestStarted => EarliestMessageStartedTime()
            , QueueSelectionStrategy.LatestCompleted => LatestMessageCompletedTime()
            , QueueSelectionStrategy.LatestStarted => LatestMessageStartedTime()
            , QueueSelectionStrategy.LowestQueueEntryPosition => LowestQueueEntryPosition()
            , QueueSelectionStrategy.HighestQueueEntryPosition => HighestQueueEntryPosition()
            , _ => throw new NotImplementedException()
        };
    }

    private T LeastBusy()
    {
        Sort();
        return this[0];
    }

    private T EarliestMessageCompletedTime()
    {
        orderedTimes.Clear();
        orderedTimes.AddRange(this.Select(eq => eq.LatestMessageFinishedProcessing.Time));
        var minTime = orderedTimes.Min();
        var indexOfMinTime = orderedTimes.IndexOf(minTime);
        return this[indexOfMinTime];
    }

    private T EarliestMessageStartedTime()
    {
        orderedTimes.Clear();
        orderedTimes.AddRange(this.Select(eq => eq.LatestMessageStartProcessing.Time));
        var minTime = orderedTimes.Min();
        var indexOfMinTime = orderedTimes.IndexOf(minTime);
        return this[indexOfMinTime];
    }

    private T LatestMessageCompletedTime()
    {
        orderedTimes.Clear();
        orderedTimes.AddRange(this.Select(eq => eq.LatestMessageFinishedProcessing.Time));
        var minTime = orderedTimes.Min();
        var indexOfMinTime = orderedTimes.IndexOf(minTime);
        return this[indexOfMinTime];
    }

    private T LatestMessageStartedTime()
    {
        orderedTimes.Clear();
        orderedTimes.AddRange(this.Select(eq => eq.LatestMessageStartProcessing.Time));
        var minTime = orderedTimes.Min();
        var indexOfMinTime = orderedTimes.IndexOf(minTime);
        return this[indexOfMinTime];
    }

    private T LowestQueueEntryPosition()
    {
        orderedQueueEntryIds.Clear();
        orderedQueueEntryIds.AddRange(this.Select(eq => eq.LatestMessageFinishedProcessing.MessageNumber));
        var minTime = orderedQueueEntryIds.Min();
        var indexOfMinTime = orderedQueueEntryIds.IndexOf(minTime);
        return this[indexOfMinTime];
    }

    private T HighestQueueEntryPosition()
    {
        orderedQueueEntryIds.Clear();
        orderedQueueEntryIds.AddRange(this.Select(eq => eq.LatestMessageFinishedProcessing.MessageNumber));
        var minTime = orderedQueueEntryIds.Min();
        var indexOfMinTime = orderedQueueEntryIds.IndexOf(minTime);
        return this[indexOfMinTime];
    }
}
