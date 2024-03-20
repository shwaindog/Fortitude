#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public interface IPQUpdateClient : IConversationSubscriber
{
    IMessageStreamDecoder MessageStreamDecoder { get; }
}
