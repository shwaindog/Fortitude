#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeIO.Conversations;
using FortitudeIO.Transports.NewSocketAPI.Config;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public abstract class PQConversationRepositoryBase<T> : IPQConversationRepository<T>
    where T : class, IConversation
{
    private readonly IMap<ISocketConnectionConfig, T> socketSubscriptions =
        new ConcurrentMap<ISocketConnectionConfig, T>();

    public T RetrieveOrCreateConversation(ISocketConnectionConfig socketConnectionConfig)
    {
        if (!socketSubscriptions.TryGetValue(socketConnectionConfig, out var socketClient))
        {
            socketClient = CreateNewSocketSubscriptionType(socketConnectionConfig);
            socketSubscriptions[socketConnectionConfig] = socketClient;
        }

        return socketClient!;
    }

    public T? RetrieveConversation(ISocketConnectionConfig socketConnectionConfig) =>
        // ReSharper disable once InconsistentlySynchronizedField
        socketSubscriptions.TryGetValue(socketConnectionConfig, out var foundConversation) ? foundConversation : null;

    public void RemoveConversation(ISocketConnectionConfig socketConnectionConfig)
    {
        if (socketSubscriptions.TryGetValue(socketConnectionConfig, out var foundConversation))
            foundConversation!.Stop();
        socketSubscriptions.Remove(socketConnectionConfig);
    }

    protected abstract T CreateNewSocketSubscriptionType(ISocketConnectionConfig socketConnectionConfig);
}
