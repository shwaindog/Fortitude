#region

using FortitudeBusRules.BusMessaging;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.BusMessaging.Pipelines.Timers;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeBusRules.Rules;

public interface IQueueContext
{
    IMessageQueue RegisteredOn { get; }
    IMessageBus MessageBus { get; }
    IRecycler PooledRecycler { get; }
    IActionTimer Timer { get; }

    IMessageQueueList<IMessageQueue> GetEventQueues(MessageQueueType selector);
}

public class QueueContext : IQueueContext
{
    private readonly IConfigureMessageBus configureMessageBus;

    public QueueContext(MessageQueue registeredOn, IConfigureMessageBus configureMessageBus, IRecycler? pooledRecycler = null)
    {
        RegisteredOn = registeredOn;
        this.configureMessageBus = configureMessageBus;
        PooledRecycler = pooledRecycler ?? new Recycler();
        Timer = new QueueTimer(new UpdateableTimer(), this);
    }

    public IMessageQueue RegisteredOn { get; }
    public IMessageBus MessageBus => configureMessageBus;
    public IRecycler PooledRecycler { get; }
    public IActionTimer Timer { get; }

    public IMessageQueueList<IMessageQueue> GetEventQueues(MessageQueueType selector) =>
        configureMessageBus.AllMessageQueues.SelectEventQueues(selector);

    public override string ToString() => $"{GetType().Name}({nameof(RegisteredOn)}: {RegisteredOn})";
}
