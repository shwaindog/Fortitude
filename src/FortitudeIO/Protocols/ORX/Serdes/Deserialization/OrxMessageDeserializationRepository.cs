#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.Protocols.ORX.Serdes.Deserialization;

public interface IOrxDeserializationRepository : IMessageDeserializationRepository
{
    IOrxDeserializationRepository RegisterDeserializer<TM>(Action<TM, object?, IConversation?>? msgHandler)
        where TM : class, IVersionedMessage, new();
}

internal class OrxMessageDeserializationRepository : FactoryDeserializationRepository, IOrxDeserializationRepository
{
    public OrxMessageDeserializationRepository(IRecycler recycler, IMessageDeserializationRepository? cascadingFallbackDeserializationRepo = null) :
        base(recycler, cascadingFallbackDeserializationRepo) { }

    public IOrxDeserializationRepository RegisterDeserializer<T>(Action<T, object?, IConversation?>? msgHandler)
        where T : class, IVersionedMessage, new()
    {
        var instanceOfTypeToDeserialize = Recycler.Borrow<T>();
        RegisterDeserializer(instanceOfTypeToDeserialize.MessageId, msgHandler);
        instanceOfTypeToDeserialize.DecrementRefCount();
        return this;
    }

    public override IMessageStreamDecoder Supply() => new OrxMessageStreamDecoder(this);

    public bool RegisterDeserializer<TM>(uint msgId
        , Action<TM, object?, IConversation?>? msgHandler)
        where TM : class, IVersionedMessage, new()
    {
        if (msgHandler == null)
            throw new Exception("Message Handler cannot be null");
        if (!RegisteredDeserializers.TryGetValue(msgId, out var existingMessageDeserializer))
        {
            var sourceNewMessageDeserializer
                = existingMessageDeserializer as INotifyingMessageDeserializer<TM> ?? SourceMessageDeserializer<TM>(msgId);
            if (sourceNewMessageDeserializer != null)
            {
                RegisterDeserializer(sourceNewMessageDeserializer);
                existingMessageDeserializer = sourceNewMessageDeserializer;
            }
            else
            {
                throw new Exception($"Could not source MessageDeserializer for {nameof(TM)}");
            }
        }

        INotifyingMessageDeserializer<TM>? resolvedDeserializer = existingMessageDeserializer as INotifyingMessageDeserializer<TM>;
        if (resolvedDeserializer == null)
            throw new Exception("Two different message types cannot be registered to the same Id");
        if (resolvedDeserializer.IsRegistered(msgHandler)) throw new Exception("Message Handler already registered");

        resolvedDeserializer.ConversationMessageDeserialized += msgHandler;
        return true;
    }

    protected override INotifyingMessageDeserializer<TM>? SourceMessageDeserializer<TM>(uint msgId) => new OrxDeserializer<TM>(Recycler, msgId);
}
