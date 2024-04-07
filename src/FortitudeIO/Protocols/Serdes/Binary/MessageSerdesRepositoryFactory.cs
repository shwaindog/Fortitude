namespace FortitudeIO.Protocols.Serdes.Binary;

public class MessageSerdesRepositoryFactory : IMessageSerdesRepositoryFactory
{
    public MessageSerdesRepositoryFactory(IMessageSerializationRepository messageSerializationRepository
        , IMessageDeserializationRepository messageDeserializationRepository, IMessageStreamDecoderFactory messageStreamDecoderFactory)
    {
        MessageSerializationRepository = messageSerializationRepository;
        MessageDeserializationRepository = messageDeserializationRepository;
        MessageStreamDecoderFactory = messageStreamDecoderFactory;
    }

    public IMessageStreamDecoderFactory MessageStreamDecoderFactory { get; }
    public IMessageSerializationRepository MessageSerializationRepository { get; }
    public IMessageDeserializationRepository MessageDeserializationRepository { get; }
}
