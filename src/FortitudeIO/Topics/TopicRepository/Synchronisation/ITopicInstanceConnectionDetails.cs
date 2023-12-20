#region

using FortitudeIO.Conversations;
using FortitudeIO.Topics.Config.ConnectionConfig;

#endregion

namespace FortitudeIO.Topics.TopicRepository.Synchronisation;

public interface ITopicInstanceConnectionDetails
{
    string? TopicName { get; set; }
    string? Hostname { get; set; }
    string? InstanceName { get; set; }
    ConversationType ConversationType { get; set; }
    List<ITopicEndpointInfo>? Connections { get; set; }
}
