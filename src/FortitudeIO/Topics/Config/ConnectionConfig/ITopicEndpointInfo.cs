using FortitudeIO.Conversations;
using FortitudeIO.Transports;

namespace FortitudeIO.Topics.Config.ConnectionConfig
{
    public interface ITopicEndpointInfo
    {
        TransportType TransportType { get; }
        ConversationType ConversationType { get; }
        ConversationState ConversationState { get; }
        string InstanceName { get; set; }
        bool EquivalentEndpoint(ITopicEndpointInfo test);
    }
}
