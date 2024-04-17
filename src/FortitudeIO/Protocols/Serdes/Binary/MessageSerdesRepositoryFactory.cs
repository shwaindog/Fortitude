namespace FortitudeIO.Protocols.Serdes.Binary;

public class MessageSerdesRepositoryFactory : IMessageSerdesRepositoryFactory
{
    private readonly IMessageDeserializationRepository messageDeserializationRepository;
    private readonly IMessageStreamDecoderFactory messageStreamDecoderFactory;

    public MessageSerdesRepositoryFactory(IMessageSerializationRepository messageSerializationRepository
        , IMessageDeserializationRepository messageDeserializationRepository, IMessageStreamDecoderFactory messageStreamDecoderFactory)
    {
        this.messageDeserializationRepository = messageDeserializationRepository;
        this.messageStreamDecoderFactory = messageStreamDecoderFactory;
        MessageSerializationRepository = messageSerializationRepository;
    }

    public IMessageStreamDecoderFactory MessageStreamDecoderFactory(string _) => messageStreamDecoderFactory;
    public IMessageSerializationRepository MessageSerializationRepository { get; }
    public IMessageDeserializationRepository MessageDeserializationRepository(string _) => messageDeserializationRepository;
}
