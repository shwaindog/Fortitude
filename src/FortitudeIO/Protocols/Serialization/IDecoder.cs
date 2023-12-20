namespace FortitudeIO.Protocols.Serialization;

public interface IDecoder
{
    bool AddMessageDecoder(uint msgId, IBinaryDeserializer deserializer);
}

public interface IStreamDecoder : IDecoder
{
    int Process(DispatchContext dispatchContext);
}
