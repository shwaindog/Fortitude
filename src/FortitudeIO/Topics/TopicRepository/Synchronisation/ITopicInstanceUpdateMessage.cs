#region

using FortitudeCommon.EventProcessing;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Topics.TopicRepository.Synchronisation.ORX;

#endregion

namespace FortitudeIO.Topics.TopicRepository.Synchronisation;

public interface ITopicInstanceUpdateMessage : IAuthenticatedMessage
{
    TopicUpdateType TopicUpdateType { get; set; }
    EventType UpdateType { get; set; }
    uint PostUpdateRegistryHash { get; set; }
    uint HashingType { get; set; }
    List<OrxTopicClientConnectionDetails>? TopicsAffected { get; set; }
    MutableString? Reason { get; set; }
}
