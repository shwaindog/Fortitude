#region

using System.Collections;
using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeBusRules.MessageBus.Pipelines.Groups;

public interface IEventQueueGroup : IList<IEventQueue>
{
    bool HasStarted { get; }
    int AddNewQueues(DeploymentOptions deploymentOptions);
    ValueTask<IDispatchResult> LaunchRule(IRule sender, IRule rule, DeploymentOptions deployment);
}

public class SpecificEventQueueGroup : IEventQueueGroup
{
    private readonly int defaultNewQueueSize;
    private readonly EventQueueType groupType;
    private readonly IEventBus owningEventBus;
    private List<IEventQueue> eventQueues = new();

    public SpecificEventQueueGroup(IEventBus owningEventBus, EventQueueType groupType, IRecycler recycler
        , int defaultNewQueueSize = 10_000)
    {
        Recycler = recycler;
        this.owningEventBus = owningEventBus;
        this.groupType = groupType;
        this.defaultNewQueueSize = defaultNewQueueSize;
    }

    public IRecycler Recycler { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<IEventQueue> GetEnumerator()
    {
        var freshEnumerator = Recycler.Borrow<AutoRecycleEnumerator<IEventQueue>>();
        freshEnumerator.AddRange(eventQueues);
        return freshEnumerator;
    }

    public void Add(IEventQueue item)
    {
        if (item.QueueType != groupType)
            throw new InvalidOperationException(
                $"Can not add EventQueue of type {item.QueueType} to EventQueueGroup of type {groupType}");
        eventQueues.Add(item);
    }

    public void Clear()
    {
        eventQueues.Clear();
    }

    public bool Contains(IEventQueue item) => eventQueues.Contains(item);

    public void CopyTo(IEventQueue[] array, int arrayIndex)
    {
        eventQueues.CopyTo(array, arrayIndex);
    }

    public bool Remove(IEventQueue item) => eventQueues.Remove(item);

    public int Count => eventQueues.Count;
    public bool IsReadOnly => false;

    public bool HasStarted => eventQueues.Any(eq => eq.IsRunning);
    public int IndexOf(IEventQueue item) => eventQueues.IndexOf(item);

    public void Insert(int index, IEventQueue item)
    {
        if (item.QueueType != groupType)
            throw new InvalidOperationException(
                $"Can not add EventQueue of type {item.QueueType} to EventQueueGroup of type {groupType}");
        eventQueues.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        eventQueues.RemoveAt(index);
    }

    public IEventQueue this[int index]
    {
        get => eventQueues[index];
        set
        {
            if (value.QueueType != groupType)
                throw new InvalidOperationException(
                    $"Can not add EventQueue of type {value.QueueType} to EventQueueGroup of type {groupType}");
            eventQueues[index] = value;
        }
    }

    public int AddNewQueues(DeploymentOptions deploymentOptions)
    {
        var existingCount = eventQueues.Count;
        for (var i = existingCount; i < existingCount + deploymentOptions.Instances; i++)
            eventQueues.Add(new EventQueue(owningEventBus, groupType, i, defaultNewQueueSize));
        return eventQueues.Count;
    }

    public ValueTask<IDispatchResult> LaunchRule(IRule sender, IRule rule, DeploymentOptions deploymentOptions)
    {
        var leastBusy = ResolveLeastBusy();
        return eventQueues[leastBusy].LaunchRule(sender, rule);
    }

    public int ResolveLeastBusy()
    {
        var minIndex = 0;
        var currentMinMessageCount = uint.MaxValue;


        for (var i = 0; i < eventQueues.Count; i++)
        {
            var checkQueue = eventQueues[i];
            var checkQueueMessageCount = checkQueue.NumOfMessagesReceivedRecently();
            if (checkQueueMessageCount < currentMinMessageCount)
            {
                minIndex = i;
                currentMinMessageCount = checkQueueMessageCount;
            }
        }

        return minIndex;
    }
}
