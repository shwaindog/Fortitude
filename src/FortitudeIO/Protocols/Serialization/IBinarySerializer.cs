
namespace FortitudeIO.Protocols.Serialization
{
    public interface IBinarySerializer
    {
        int Serialize(byte[] buffer, int writeOffset, IVersionedMessage message);
    }

    public interface IBinarySerializer<Tm> : IBinarySerializer
    {
    }
}
