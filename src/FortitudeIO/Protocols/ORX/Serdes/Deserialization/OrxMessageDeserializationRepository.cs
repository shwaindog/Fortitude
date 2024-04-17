#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.Protocols.ORX.Serdes.Deserialization;

internal class OrxMessageRepository : ConversationRepository, IConversationDeserializationRepository
{
    public OrxMessageRepository(string name, IRecycler recycler, IMessageDeserializationRepository? cascadingFallbackDeserializationRepo = null) :
        base(name, recycler, cascadingFallbackDeserializationRepo) { }

    public override IMessageStreamDecoder Supply(string name) => new OrxMessageStreamDecoder(new OrxMessageRepository(name, Recycler, this));

    public override INotifyingMessageDeserializer<TM>? SourceNotifyingMessageDeserializerFromMessageId<TM>(uint msgId) =>
        new OrxDeserializer<TM>(Recycler, msgId);

    public override IMessageDeserializer? SourceDeserializerFromMessageId(uint msgId, Type messageType)
    {
        if (messageType.GetInterfaces().All(t => t != typeof(IVersionedMessage)))
            return CascadingFallbackDeserializationFactoryRepo?.SourceDeserializerFromMessageId(msgId, messageType);
        if (messageType.GetConstructor(Type.EmptyTypes) == null)
            return CascadingFallbackDeserializationFactoryRepo?.SourceDeserializerFromMessageId(msgId, messageType);
        if (!OrxMandatoryField.FindAll(messageType).Any() && !OrxOptionalField.FindAll(messageType).Any())
            return CascadingFallbackDeserializationFactoryRepo?.SourceDeserializerFromMessageId(msgId, messageType);
        var genericOrxDeserializer = typeof(OrxDeserializer<>);
        var orxDeserializerMessageType = genericOrxDeserializer.MakeGenericType(messageType);
        var dynamicOrxDeserializer = (IMessageDeserializer)Activator.CreateInstance(orxDeserializerMessageType, Recycler, msgId)!;

        return dynamicOrxDeserializer;
    }

    public override INotifyingMessageDeserializer<TM>? SourceTypedMessageDeserializerFromMessageId<TM>(uint msgId) =>
        new OrxDeserializer<TM>(Recycler, msgId);
}
