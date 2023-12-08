#region

using System.Collections;
using Fortitude.EventProcessing.BusRules.MessageBus.Pipelines;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace Fortitude.EventProcessing.BusRules.MessageBus;

public class EventQueueGroup : IEnumerable<IEventQueue>, IList<IEventQueue>
{
    private readonly EventQueueType groupType;
    private readonly IRecycler recycler;
    private List<IEventQueue> eventQueues = new();

    public EventQueueGroup(EventQueueType groupType)
    {
        recycler = new Recycler();
        this.groupType = groupType;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<IEventQueue> GetEnumerator()
    {
        var freshEnumerator = recycler.Borrow<ReusableEnumerator<IEventQueue>>();
        freshEnumerator.BackingList = eventQueues;
        return freshEnumerator;
    }

    public void Add(IEventQueue item)
    {
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
    public int IndexOf(IEventQueue item) => eventQueues.IndexOf(item);

    public void Insert(int index, IEventQueue item)
    {
        eventQueues.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        eventQueues.RemoveAt(index);
    }

    public IEventQueue this[int index]
    {
        get => eventQueues[index];
        set => eventQueues[index] = value;
    }
}
