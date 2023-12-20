#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeIO.Topics.Config.ConnectionConfig;

#endregion

namespace FortitudeIO.Topics.TopicRepository.Synchronisation.ORX;

public class OrxTopicClientConnectionDetails : ITopicInstanceConnectionDetails
{
    [OrxMandatoryField(1)] public string? TopicName { get; set; }

    [OrxMandatoryField(2)] public string? Hostname { get; set; }

    [OrxMandatoryField(3)] public string? InstanceName { get; set; }

    [OrxMandatoryField(4)] public ConversationType ConversationType { get; set; }

    [OrxMandatoryField(5)] public List<ITopicEndpointInfo>? Connections { get; set; }

    public bool TopicHostSubscriptionTypeSameForInstance(ITopicInstanceConnectionDetails other) =>
        string.Equals(TopicName, other.TopicName) && string.Equals(Hostname, other.Hostname) &&
        string.Equals(InstanceName, other.InstanceName) &&
        ConversationType == other.ConversationType;

    protected bool Equals(OrxTopicClientConnectionDetails other) => TopicHostSubscriptionTypeSameForInstance(other);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((OrxTopicClientConnectionDetails)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = TopicName != null ? TopicName.GetHashCode() : 0;
            hashCode = (hashCode * 397) ^ (Hostname != null ? Hostname.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (InstanceName != null ? InstanceName.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (int)ConversationType;
            return hashCode;
        }
    }

    public override string ToString() =>
        $"{nameof(TopicName)}: {TopicName}, {nameof(Hostname)}: {Hostname}, " +
        $"{nameof(InstanceName)}: {InstanceName}, {nameof(ConversationType)}: " +
        $"{ConversationType}";
}
