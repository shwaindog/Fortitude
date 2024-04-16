#region

using FortitudeBusRules.MessageBus;
using FortitudeBusRules.MessageBus.Pipelines;
using FortitudeBusRules.MessageBus.Pipelines.Groups;
using FortitudeBusRules.MessageBus.Pipelines.Timers;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeBusRules.Rules;

public interface IEventContext
{
    IEventQueue RegisteredOn { get; }
    IEventBus EventBus { get; }
    IRecycler PooledRecycler { get; }
    IActionTimer Timer { get; }

    IEventQueueList GetEventQueues(EventQueueType selector);
}

public class EventContext : IEventContext
{
    private readonly IConfigureEventBus configureEventBus;

    public EventContext(EventQueue registeredOn, IConfigureEventBus configureEventBus, IRecycler? pooledRecycler = null)
    {
        RegisteredOn = registeredOn;
        this.configureEventBus = configureEventBus;
        PooledRecycler = pooledRecycler ?? new Recycler();
        Timer = new QueueTimer(new UpdateableTimer(), this);
    }

    public IEventQueue RegisteredOn { get; }
    public IEventBus EventBus => configureEventBus;
    public IRecycler PooledRecycler { get; }
    public IActionTimer Timer { get; }
    public IEventQueueList GetEventQueues(EventQueueType selector) => configureEventBus.AllEventQueues.SelectEventQueues(selector);
    public override string ToString() => $"{GetType().Name}({nameof(RegisteredOn)}: {RegisteredOn})";
}
