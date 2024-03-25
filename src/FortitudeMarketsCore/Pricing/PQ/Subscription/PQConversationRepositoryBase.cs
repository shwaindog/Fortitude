#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeIO.Conversations;
using FortitudeIO.Transports.NewSocketAPI.Config;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public abstract class PQConversationRepositoryBase<T> : IPQConversationRepository<T>
    where T : class, IConversation
{
    private readonly IMap<ISocketTopicConnectionConfig, T> socketSubscriptions =
        new ConcurrentMap<ISocketTopicConnectionConfig, T>();

    public T RetrieveOrCreateConversation(ISocketTopicConnectionConfig socketConnectionConfig)
    {
        if (!socketSubscriptions.TryGetValue(socketConnectionConfig, out var socketClient))
        {
            socketClient = CreateNewSocketSubscriptionType(socketConnectionConfig);
            socketSubscriptions[socketConnectionConfig] = socketClient;
        }

        return socketClient!;
    }

    public T? RetrieveConversation(ISocketTopicConnectionConfig socketConnectionConfig) =>
        // ReSharper disable once InconsistentlySynchronizedField
        socketSubscriptions.TryGetValue(socketConnectionConfig, out var foundConversation) ? foundConversation : null;

    public void RemoveConversation(ISocketTopicConnectionConfig socketConnectionConfig)
    {
        if (socketSubscriptions.TryGetValue(socketConnectionConfig, out var foundConversation))
            foundConversation!.Stop();
        socketSubscriptions.Remove(socketConnectionConfig);
    }

    protected abstract T CreateNewSocketSubscriptionType(ISocketTopicConnectionConfig socketConnectionConfig);
}
