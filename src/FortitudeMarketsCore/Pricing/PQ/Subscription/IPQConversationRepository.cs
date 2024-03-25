#region

using FortitudeIO.Conversations;
using FortitudeIO.Transports.NewSocketAPI.Config;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public interface IPQConversationRepository<out T> where T : class, IConversation
{
    T RetrieveOrCreateConversation(ISocketTopicConnectionConfig socketConnectionConfig);

    void RemoveConversation(ISocketTopicConnectionConfig socketConnectionConfig);
    T? RetrieveConversation(ISocketTopicConnectionConfig socketConnectionConfig);
}
