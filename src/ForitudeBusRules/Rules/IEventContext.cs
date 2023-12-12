#region

using FortitudeBusRules.MessageBus;
using FortitudeBusRules.MessageBus.Pipelines;
using FortitudeBusRules.MessageBus.Pipelines.Timers;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Memory;
using Timer = FortitudeCommon.Chronometry.Timers.Timer;

#endregion

namespace FortitudeBusRules.Rules;

public interface IEventContext
{
    IEventQueue RegisteredOn { get; }
    IEventBus EventBus { get; }
    IRecycler PooledRecycler { get; }
    IActionTimer Timer { get; }
}

public class EventContext : IEventContext
{
    public EventContext(EventQueue registeredOn, IEventBus eventBus, IRecycler? pooledRecycler = null)
    {
        RegisteredOn = registeredOn;
        EventBus = eventBus;
        PooledRecycler = pooledRecycler ?? new Recycler();
        Timer = new QueueTimer(new Timer(), this);
    }

    public IEventQueue RegisteredOn { get; }
    public IEventBus EventBus { get; }
    public IRecycler PooledRecycler { get; }
    public IActionTimer Timer { get; }

    public override string ToString() => $"{GetType().Name}({nameof(RegisteredOn)}: {RegisteredOn})";
}
