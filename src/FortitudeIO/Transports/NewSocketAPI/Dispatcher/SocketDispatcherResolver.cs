#region

using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Receiving;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Dispatcher;

public interface ISocketDispatcherResolver
{
    ISocketDispatcher Resolve(INetworkTopicConnectionConfig networkSessionContext);
}

public class SimpleSocketDispatcherResolver : ISocketDispatcherResolver
{
    private ISocketDispatcher dispatcher;

    public SimpleSocketDispatcherResolver(ISocketDispatcher dispatcher) => this.dispatcher = dispatcher;

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

    public ISocketDispatcher Resolve(INetworkTopicConnectionConfig networkSessionContext) =>
        new SocketDispatcher(listenerSelector(pooledListeners),
            senderSelector(pooledSenders));

    public static ISocketDispatcherResolver BuildLeastUsedPooledDispatcherResolver(string name, int numListeners
        , int numSenders)
    {
        var listeners = new List<ISocketDispatcherListener>();
        for (var i = 0; i < numListeners; i++)
            listeners.Add(new SimpleSocketRingPollerListener($"{name}_{i}", 5
                , new SocketSelector(100, new OSNetworkingController())));
        var senders = new List<ISocketDispatcherSender>();
        for (var i = 0; i < numListeners; i++) senders.Add(new SimpleSocketRingPollerSender($"{name}_{i}", 5));

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

    public ISocketDispatcher Resolve(INetworkTopicConnectionConfig networkSessionContext) =>
        singletonDispatcher ??= new SocketDispatcher(
            new SimpleSocketRingPollerListener($"Singleton", 5
                , new SocketSelector(100, new OSNetworkingController())),
            new SimpleSocketRingPollerSender("Singleton", 5));
}
