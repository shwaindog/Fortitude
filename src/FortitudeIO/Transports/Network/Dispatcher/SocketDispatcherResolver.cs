#region

using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Receiving;

#endregion

namespace FortitudeIO.Transports.Network.Dispatcher;

public interface ISocketDispatcherResolver
{
    IUpdateableTimer RealTimer { get; set; }
    ISocketDispatcher Resolve(INetworkTopicConnectionConfig networkSessionContext);
}

public class SimpleSocketDispatcherResolver : ISocketDispatcherResolver
{
    private ISocketDispatcher dispatcher;

    public SimpleSocketDispatcherResolver(ISocketDispatcher dispatcher) => this.dispatcher = dispatcher;
    public IUpdateableTimer RealTimer { get; set; } = new UpdateableTimer("SimpleSocketDispatcher");

    public ISocketDispatcher Resolve(INetworkTopicConnectionConfig networkSessionContext) => dispatcher;
}

public class PoolSocketDispatcherResolver : ISocketDispatcherResolver
{
    private readonly Func<IList<ISocketDispatcherListener>, ISocketDispatcherListener> listenerSelector;
    private readonly Func<IList<ISocketDispatcherSender>, ISocketDispatcherSender> senderSelector;
    private List<ISocketDispatcherListener> pooledListeners;
    private List<ISocketDispatcherSender> pooledSenders;

    public PoolSocketDispatcherResolver(List<ISocketDispatcherListener> pooledListeners
        , List<ISocketDispatcherSender> pooledSenders,
        Func<IList<ISocketDispatcherListener>, ISocketDispatcherListener> listenerSelector,
        Func<IList<ISocketDispatcherSender>, ISocketDispatcherSender> senderSelector)
    {
        this.pooledListeners = pooledListeners;
        this.pooledSenders = pooledSenders;
        this.listenerSelector = listenerSelector;
        this.senderSelector = senderSelector;
    }

    public IUpdateableTimer RealTimer { get; set; } = new UpdateableTimer("PoolSocketDispatcher");

    public ISocketDispatcher Resolve(INetworkTopicConnectionConfig networkSessionContext) =>
        new SocketDispatcher(listenerSelector(pooledListeners),
            senderSelector(pooledSenders));

    public ISocketDispatcherResolver BuildLeastUsedPooledDispatcherResolver(string name, int numListeners
        , int numSenders)
    {
        var listeners = new List<ISocketDispatcherListener>();
        for (var i = 0; i < numListeners; i++)
            listeners.Add(new SimpleSocketAsyncValueTaskRingPollerListener($"{name}_{i}", 1
                , new SocketSelector(1, new OSNetworkingController()), RealTimer));
        var senders = new List<ISocketDispatcherSender>();
        for (var i = 0; i < numListeners; i++) senders.Add(new SimpleAsyncValueTaskSocketRingPollerSender($"{name}_{i}", 1));

        Func<IList<ISocketDispatcherListener>, ISocketDispatcherListener> minUsageListeners = (availableListeners) =>
        {
            var maxCount = availableListeners.Min(d => d.UsageCount);
            return availableListeners.FirstOrDefault(d => d.UsageCount == maxCount) ?? availableListeners.First();
        };
        Func<IList<ISocketDispatcherSender>, ISocketDispatcherSender> minUsageSenders = (availableSenders) =>
        {
            var maxCount = availableSenders.Min(d => d.UsageCount);
            return availableSenders.FirstOrDefault(d => d.UsageCount == maxCount) ?? availableSenders.First();
        };
        return new PoolSocketDispatcherResolver(listeners, senders, minUsageListeners, minUsageSenders);
    }
}

public class SingletonSocketDispatcherResolver : ISocketDispatcherResolver
{
    private static volatile ISocketDispatcherResolver? singletonInstance;
    private static readonly object syncLock = new();
    private ISocketDispatcher? singletonDispatcher;

    public static ISocketDispatcherResolver Instance
    {
        get
        {
            if (singletonInstance == null)
                lock (syncLock)
                {
                    singletonInstance ??= new SingletonSocketDispatcherResolver();
                }

            return singletonInstance;
        }
    }

    public IUpdateableTimer RealTimer { get; set; } = new UpdateableTimer("SingletonSocketDispatcher");

    public ISocketDispatcher Resolve(INetworkTopicConnectionConfig networkSessionContext) =>
        singletonDispatcher ??= new SocketDispatcher(
            new SimpleSocketAsyncValueTaskRingPollerListener($"Singleton", 1
                , new SocketSelector(1, new OSNetworkingController()), RealTimer),
            new SimpleAsyncValueTaskSocketRingPollerSender("Singleton", 1));
}
