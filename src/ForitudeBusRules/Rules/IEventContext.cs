#region

using Fortitude.EventProcessing.BusRules.MessageBus;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace Fortitude.EventProcessing.BusRules.Rules;

public interface IEventContext
{
    EventQueue RegisteredOn { get; }

    IEventBus EventBus { get; }
    IRecycler PooledRecycler { get; }
}

public class EventContext : IEventContext
{
    private long executionTimeTicks;
    private long messageCount;

    public EventContext(EventQueue registeredOn, IEventBus eventBus, IRecycler? pooledRecycler = null)
    {
        RegisteredOn = registeredOn;
        EventBus = eventBus;
        PooledRecycler = pooledRecycler ?? new Recycler();
    }

    public EventQueue RegisteredOn { get; }
    public IEventBus EventBus { get; }
    public IRecycler PooledRecycler { get; }
    public long IncrementMessageCount() => Interlocked.Increment(ref messageCount);
    public long AddProcessingTicks(long addThis) => Interlocked.Add(ref executionTimeTicks, addThis);
}
