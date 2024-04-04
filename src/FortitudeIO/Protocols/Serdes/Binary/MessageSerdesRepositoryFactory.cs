namespace FortitudeIO.Protocols.Serdes.Binary;

public class MessageSerdesRepositoryFactory : IMessageSerdesRepositoryFactory
{
    public MessageSerdesRepositoryFactory(IMessageSerializationRepository messageSerializationRepository
        , IMessageDeserializationRepository messageDeserializationRepository)
    {
        MessageSerializationRepository = messageSerializationRepository;
        MessageDeserializationRepository = messageDeserializationRepository;
    }

    public IMessageSerializationRepository MessageSerializationRepository { get; }
    public IMessageDeserializationRepository MessageDeserializationRepository { get; }
}
