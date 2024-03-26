#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Construction;

#endregion

namespace FortitudeIO.Protocols.ORX.Serialization.Deserialization;

public interface IOrxDeserializationRepository : IStreamDecoderFactory
{
    IOrxDeserializationRepository RegisterDeserializer<TM>(Action<TM, object?, IConversation?>? msgHandler)
        where TM : class, IVersionedMessage, new();

    IOrxDeserializationRepository RegisterDeserializer<TM>(uint msgId
        , Action<TM, object?, IConversation?>? msgHandler)
        where TM : class, IVersionedMessage, new();
}

internal class OrxStreamDecoderFactory : SocketStreamDecoderFactory, IOrxDeserializationRepository
{
    private readonly IMessageIdDeserializationRepository deSerializationRepository;
    private readonly IRecycler recycler;

    public OrxStreamDecoderFactory(
        Func<IMap<uint, IMessageDeserializer>, IMessageStreamDecoder> messageStreamDecoderFactory,
        IMessageIdDeserializationRepository deSerializationRepository, IRecycler recycler) : base(
        messageStreamDecoderFactory)
    {
        this.deSerializationRepository = deSerializationRepository;
        this.recycler = recycler;
    }

    public IOrxDeserializationRepository RegisterDeserializer<T>(Action<T, object?, IConversation?>? msgHandler)
        where T : class, IVersionedMessage, new()
    {
        var instanceOfTypeToDeserialize = recycler.Borrow<T>();
        RegisterDeserializer(instanceOfTypeToDeserialize.MessageId, msgHandler);
        instanceOfTypeToDeserialize.DecrementRefCount();
        return this;
    }

    public IOrxDeserializationRepository RegisterDeserializer<TM>(uint msgId
        , Action<TM, object?, IConversation?>? msgHandler)
        where TM : class, IVersionedMessage, new()
    {
        if (msgHandler == null)
            throw new Exception("Message Handler cannot be null");
        ICallbackMessageDeserializer<TM>? mu;
        if (!DeserializersMap.TryGetValue(msgId, out var u))
            DeserializersMap.Add(msgId, mu = deSerializationRepository.GetDeserializer<TM>(msgId)!);
        else if ((mu = u as ICallbackMessageDeserializer<TM>) == null)
            throw new Exception("Two different message types cannot be registered to the same Id");
        else if (mu.IsRegistered(msgHandler)) throw new Exception("Message Handler already registered");

        mu.Deserialized2 += msgHandler;
        return this;
    }
}
