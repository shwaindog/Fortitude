#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeBusRules.Connectivity.Network.Serdes.Deserialization.Rules;

public class MessageDeserializerResolveRun : RecyclableObject
{
    public string SubscribeFullAddress { get; set; } = null!;
    public string SubscribePostFix { get; set; } = null!;
    public INotifyingMessageDeserializer? MessageDeserializer { get; set; }
    public uint? MessageId { get; set; }
    public RemoteNotificationRegistration RemoteNotificationRegistration { get; set; } = null!;
    public Type PublishType { get; set; } = null!;

    public IMessageDeserializationRepository RootMessageDeserializationRepository { get; set; } = null!;

    public bool RootMessageDeserializationRepositoryIsFactoryRepository =>
        RootMessageDeserializationRepository is IMessageDeserializerFactoryRepository;

    public IMessageDeserializerFactoryRepository? RootMessageDeserializationFactoryRepository =>
        RootMessageDeserializationRepository as IMessageDeserializerFactoryRepository;

    public bool HaveBothMessageDeserializerAndMessageId => MessageDeserializer != null && MessageId != null;

    public Type? DeserializedType => RemoteNotificationRegistration.DeserializedType;
    public IConverter? Converter => RemoteNotificationRegistration.Converter;

    public bool ContinueSearching => FailureMessage == null && !HaveBothMessageDeserializerAndMessageId;

    public string? FailureMessage { get; set; }

    public override void StateReset()
    {
        MessageId = null;
        MessageDeserializer = null;
        RemoteNotificationRegistration = null!;
        PublishType = null!;
        FailureMessage = null;
        base.StateReset();
    }

    public override string ToString() =>
        $"{nameof(MessageDeserializerResolveRun)}({nameof(SubscribeFullAddress)}: {SubscribeFullAddress}, " +
        $"{nameof(SubscribePostFix)}: {SubscribePostFix}, {nameof(MessageDeserializer)}: {MessageDeserializer}, " +
        $"{nameof(MessageId)}: {MessageId}, {nameof(RemoteNotificationRegistration)}: {RemoteNotificationRegistration}, " +
        $"{nameof(PublishType)}: {PublishType}, {nameof(ContinueSearching)}: {ContinueSearching}, " +
        $"{nameof(FailureMessage)}: {FailureMessage})";
}
