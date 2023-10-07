#region

using FortitudeCommon.EventProcessing;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Authentication;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeIO.Topics.TopicRepository.Resolver;

#endregion

namespace FortitudeIO.Topics.TopicRepository.ORX.Resolver;

public class OrxTopicInstanceUpdateMessage : OrxAuthenticatedMessage, ITopicInstanceUpdateMessage
{
    public override uint MessageId => 987123;

    [OrxMandatoryField(10)] public TopicUpdateType TopicUpdateType { get; set; }

    [OrxMandatoryField(11)] public EventType UpdateType { get; set; }

    [OrxMandatoryField(12)] public uint PostUpdateRegistryHash { get; set; }

    [OrxMandatoryField(13)] public uint HashingType { get; set; }

    [OrxMandatoryField(14)] public List<OrxTopicClientConnectionDetails>? TopicsAffected { get; set; }

    [OrxOptionalField(15)] public MutableString? Reason { get; set; }
}

public enum TopicUpdateType
{
    Unknown = 0
    , ServerNotification = 1
    , NotifyServer = 2
    , FullServerUpdate = 4
    , RequestFullUpdate = 8
}
