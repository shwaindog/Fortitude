#region


#endregion

namespace FortitudeIO.Protocols.Serdes.Binary;

public interface ISerdesFactory
{
    IStreamDecoderFactory StreamDecoderFactory { get; set; }
    IDictionary<uint, IMessageSerializer> StreamSerializers { get; }
}

public class SerdesFactory : ISerdesFactory
{
    public SerdesFactory(IStreamDecoderFactory streamDecoderFactory
        , IDictionary<uint, IMessageSerializer> streamSerializers)
    {
        StreamDecoderFactory = streamDecoderFactory;
        StreamSerializers = streamSerializers;
    }

    public IStreamDecoderFactory StreamDecoderFactory { get; set; }
    public IDictionary<uint, IMessageSerializer> StreamSerializers { get; }
}
