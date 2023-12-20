namespace FortitudeIO.Protocols.ORX.Serialization;

public interface IOlderVersionMessagePart<T> where T : class
{
    T ToLatestVersion();
    void CopyFrom(T source);
}
