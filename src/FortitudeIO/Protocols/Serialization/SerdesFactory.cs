using System.Collections.Generic;

namespace FortitudeIO.Protocols.Serialization
{
    public interface ISerdesFactory
    {
        IStreamDecoderFactory StreamDecoderFactory { get; set; }
        IDictionary<uint, IBinarySerializer> StreamSerializers { get; }
    }

    public class SerdesFactory : ISerdesFactory
    {
        public SerdesFactory(IStreamDecoderFactory streamDecoderFactory, IDictionary<uint, IBinarySerializer> streamSerializers)
        {
            StreamDecoderFactory = streamDecoderFactory;
            StreamSerializers = streamSerializers;
        }

        public IStreamDecoderFactory StreamDecoderFactory { get; set; }
        public IDictionary<uint, IBinarySerializer> StreamSerializers { get; }
    }
}
