#region

using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Transports;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.Subscription;
using FortitudeMarketsCore.Pricing.PQ.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public abstract class PQSocketSubscriptionRegistrationFactoryBase<T> : IPQSocketSubscriptionRegistrationFactory<T>
    where T : SocketSubscriber
{
    private readonly IOSNetworkingController networkingController;

    private readonly IDictionary<IConnectionConfig, T> socketSubscriptions =
        new Dictionary<IConnectionConfig, T>();

    private readonly object syncLock = new();

    protected PQSocketSubscriptionRegistrationFactoryBase(IOSNetworkingController networkingController) =>
        this.networkingController = networkingController;

    public T RegisterSocketSubscriber(string socketUseDescription, IConnectionConfig cfg, uint streamId,
        ISocketDispatcher dispatcher, int wholeMessagesPerReceive,
        IPQQuoteSerializerRepository ipqQuoteSerializerRepository, string? multicastInterface = null)
    {
        T? socketClient;
        lock (syncLock)
        {
            if (!socketSubscriptions.TryGetValue(cfg, out socketClient))
            {
                socketClient = CreateNewSocketSubscriptionType(dispatcher, networkingController, cfg,
                    socketUseDescription, 5, wholeMessagesPerReceive, ipqQuoteSerializerRepository, multicastInterface);
                socketSubscriptions[cfg] = socketClient;
            }

            socketClient.RegisterDeserializer<PQLevel0Quote>(streamId, NoOp!);
        }

        return socketClient;
    }

    public T? FindSocketSubscription(IConnectionConfig configKey) =>
        // ReSharper disable once InconsistentlySynchronizedField
        socketSubscriptions.TryGetValue(configKey, out var findValue) ? findValue : null;

    public void UnregisterSocketSubscriber(IConnectionConfig cfg, uint streamId)
    {
        lock (syncLock)
        {
            if (socketSubscriptions.TryGetValue(cfg, out var socketClient))
            {
                socketClient.UnregisterDeserializer<PQLevel0Quote>(streamId, NoOp);
                if (socketClient.RegisteredDeserializersCount == 0)
                {
                    socketSubscriptions.Remove(cfg);
                    socketClient.Disconnect();
                }
            }
        }
    }

    public T RegisterSocketSubscriber(string socketUseDescription, ISocketConnectionConfig cfg, uint streamId,
        ISocketDispatcher dispatcher, int wholeMessagesPerReceive,
        IPQQuoteSerializerRepository ipqQuoteSerializerRepository, string? multicastInterface = null) =>
        RegisterSocketSubscriber(socketUseDescription, cfg.ToConnectionConfig(), streamId, dispatcher
            , wholeMessagesPerReceive, ipqQuoteSerializerRepository, multicastInterface);

    public T? FindSocketSubscription(ISocketConnectionConfig configKey) =>
        // ReSharper disable once InconsistentlySynchronizedField
        socketSubscriptions.TryGetValue(configKey.ToConnectionConfig(), out var findValue) ? findValue : null;

    public void UnregisterSocketSubscriber(ISocketConnectionConfig cfg, uint streamId)
    {
        lock (syncLock)
        {
            if (socketSubscriptions.TryGetValue(cfg.ToConnectionConfig(), out var socketClient))
            {
                socketClient.UnregisterDeserializer<PQLevel0Quote>(streamId, NoOp);
                if (socketClient.RegisteredDeserializersCount == 0)
                {
                    socketSubscriptions.Remove(cfg.ToConnectionConfig());
                    socketClient.Disconnect();
                }
            }
        }
    }

    protected abstract T CreateNewSocketSubscriptionType(ISocketDispatcher dispatcher,
        IOSNetworkingController networkingController,
        IConnectionConfig connectionConfig,
        string socketUseDescription, uint cxTimeoutS, int wholeMessagesPerReceive,
        IPQQuoteSerializerRepository ipqQuoteSerializerRepository, string? multicastInterface);


    private static void NoOp(IPQLevel0Quote ent, object state, ISession cx) { }
}
