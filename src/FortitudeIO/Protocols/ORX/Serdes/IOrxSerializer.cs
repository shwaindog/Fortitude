namespace FortitudeIO.Protocols.ORX.Serdes;

public interface IOrxSerializer
{
    int Serialize(object message, byte[] buffer, int msgOffset, int headerOffset);
    unsafe int Serialize(object message, byte* ptr, byte* msgStart, byte* endPtr);
}
