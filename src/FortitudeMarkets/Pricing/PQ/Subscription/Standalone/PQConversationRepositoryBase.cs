#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeIO.Conversations;
using FortitudeIO.Transports.Network.Config;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Subscription.Standalone;

public abstract class PQConversationRepositoryBase<T> : IPQConversationRepository<T>
    where T : class, IConversation
{
    private readonly IMap<INetworkTopicConnectionConfig, T> socketSubscriptions =
        new ConcurrentMap<INetworkTopicConnectionConfig, T>();

    public T RetrieveOrCreateConversation(INetworkTopicConnectionConfig networkConnectionConfig)
    {
        if (!socketSubscriptions.TryGetValue(networkConnectionConfig, out var socketClient))
        {
            socketClient = CreateNewSocketSubscriptionType(networkConnectionConfig);
            socketSubscriptions[networkConnectionConfig] = socketClient;
        }

        return socketClient!;
    }

    public T? RetrieveConversation(INetworkTopicConnectionConfig networkConnectionConfig) =>
        // ReSharper disable once InconsistentlySynchronizedField
        socketSubscriptions.TryGetValue(networkConnectionConfig, out var foundConversation) ? foundConversation : null;

    public void RemoveConversation(INetworkTopicConnectionConfig networkConnectionConfig)
    {
        if (socketSubscriptions.TryGetValue(networkConnectionConfig, out var foundConversation))
            foundConversation!.Stop();
        socketSubscriptions.Remove(networkConnectionConfig);
    }

    protected abstract T CreateNewSocketSubscriptionType(INetworkTopicConnectionConfig networkConnectionConfig);
}
