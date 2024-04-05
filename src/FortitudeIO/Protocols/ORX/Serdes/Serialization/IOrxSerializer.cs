namespace FortitudeIO.Protocols.ORX.Serdes.Serialization;

public interface IOrxSerializer
{
    int Serialize(object message, byte[] buffer, int msgOffset, int headerOffset);
    unsafe int Serialize(object message, byte* ptr, byte* msgStart, byte* endPtr);
}
