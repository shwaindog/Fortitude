#region

#endregion

namespace FortitudeIO.Protocols.Serdes.Binary;

public interface ISerdesFactory
{
    IStreamDecoderFactory? StreamDecoderFactory { get; set; }

    IStreamEncoderFactory? StreamEncoderFactory { get; set; }
}

public class SerdesFactory : ISerdesFactory
{
    public SerdesFactory(IStreamDecoderFactory? streamDecoderFactory = null
        , IStreamEncoderFactory? streamEncoderFactory = null)
    {
        StreamDecoderFactory = streamDecoderFactory;
        StreamEncoderFactory = streamEncoderFactory;
    }

    public IStreamDecoderFactory? StreamDecoderFactory { get; set; }

    public IStreamEncoderFactory? StreamEncoderFactory { get; set; }
}
