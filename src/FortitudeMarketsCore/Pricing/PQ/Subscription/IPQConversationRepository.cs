#region

using FortitudeIO.Conversations;
using FortitudeIO.Transports.NewSocketAPI.Config;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public interface IPQConversationRepository<out T> where T : class, IConversation
{
    T RetrieveOrCreateConversation(ISocketConnectionConfig socketConnectionConfig);

    void RemoveConversation(ISocketConnectionConfig socketConnectionConfig);
    T? RetrieveConversation(ISocketConnectionConfig socketConnectionConfig);
}
