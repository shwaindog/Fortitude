#region

using FortitudeIO.Conversations;
using FortitudeIO.Transports.Network.Config;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription.Standalone;

public interface IPQConversationRepository<out T> where T : class, IConversation
{
    T RetrieveOrCreateConversation(INetworkTopicConnectionConfig networkConnectionConfig);

    void RemoveConversation(INetworkTopicConnectionConfig networkConnectionConfig);
    T? RetrieveConversation(INetworkTopicConnectionConfig networkConnectionConfig);
}
