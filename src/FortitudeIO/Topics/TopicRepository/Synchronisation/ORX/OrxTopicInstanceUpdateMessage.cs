#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.EventProcessing;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.Authentication;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeIO.Topics.TopicRepository.Resolver;

#endregion

namespace FortitudeIO.Topics.TopicRepository.ORX.Resolver;

public class OrxTopicInstanceUpdateMessage : OrxAuthenticatedMessage, ITopicInstanceUpdateMessage
{
    public OrxTopicInstanceUpdateMessage() { }

    private OrxTopicInstanceUpdateMessage(OrxTopicInstanceUpdateMessage toClone)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public override uint MessageId => 987123;

    [OrxMandatoryField(10)] public TopicUpdateType TopicUpdateType { get; set; }

    [OrxMandatoryField(11)] public EventType UpdateType { get; set; }

    [OrxMandatoryField(12)] public uint PostUpdateRegistryHash { get; set; }

    [OrxMandatoryField(13)] public uint HashingType { get; set; }

    [OrxMandatoryField(14)] public List<OrxTopicClientConnectionDetails>? TopicsAffected { get; set; }

    [OrxOptionalField(15)] public MutableString? Reason { get; set; }

    public override void StateReset()
    {
        TopicUpdateType = TopicUpdateType.Unknown;
        UpdateType = EventType.Unknown;
        PostUpdateRegistryHash = 0;
        HashingType = 0;
        TopicsAffected = TopicsAffected.RecycleNonNull(Recycler);
        Reason?.DecrementRefCount();
        Reason = null;
        base.StateReset();
    }

    public override IVersionedMessage CopyFrom(IVersionedMessage tradingMessage
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(tradingMessage, copyMergeFlags);
        if (tradingMessage is ITopicInstanceUpdateMessage instanceUpdateMessage)
        {
            TopicUpdateType = instanceUpdateMessage.TopicUpdateType;
            UpdateType = instanceUpdateMessage.UpdateType;
            PostUpdateRegistryHash = instanceUpdateMessage.PostUpdateRegistryHash;
            HashingType = instanceUpdateMessage.HashingType;
            if (instanceUpdateMessage.TopicsAffected != null)
            {
                TopicsAffected ??= Recycler?.Borrow<List<OrxTopicClientConnectionDetails>>() ??
                                   new List<OrxTopicClientConnectionDetails>();
                TopicsAffected.Clear();
                TopicsAffected.AddRange(instanceUpdateMessage.TopicsAffected ??
                                        Enumerable.Empty<OrxTopicClientConnectionDetails>());
            }
            else if (TopicsAffected != null)
            {
                Recycler?.Recycle(TopicsAffected);
                TopicsAffected = null;
            }

            Reason = instanceUpdateMessage.Reason.SyncOrRecycle(Reason);
        }

        return this;
    }

    public override IAuthenticatedMessage Clone() =>
        (IAuthenticatedMessage?)Recycler?.Borrow<OrxTopicInstanceUpdateMessage>().CopyFrom(this) ??
        new OrxTopicInstanceUpdateMessage(this);
}

public enum TopicUpdateType
{
    Unknown = 0
    , ServerNotification = 1
    , NotifyServer = 2
    , FullServerUpdate = 4
    , RequestFullUpdate = 8
}
