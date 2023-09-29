using System.Collections.Generic;
using FortitudeCommon.EventProcessing;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Topics.TopicRepository.ORX;
using FortitudeIO.Topics.TopicRepository.ORX.Resolver;

namespace FortitudeIO.Topics.TopicRepository.Resolver
{
    public interface ITopicInstanceUpdateMessage : IAuthenticatedMessage
    {
        TopicUpdateType TopicUpdateType { get; set; }
        EventType UpdateType { get; set; }
        uint PostUpdateRegistryHash { get; set; }
        uint HashingType { get; set; }
        List<OrxTopicClientConnectionDetails> TopicsAffected { get; set; }
        MutableString Reason { get; set; }
    }
}