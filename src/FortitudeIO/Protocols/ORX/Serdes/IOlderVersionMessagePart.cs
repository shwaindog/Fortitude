namespace FortitudeIO.Protocols.ORX.Serdes;

public interface IOlderVersionMessagePart<T> where T : class
{
    T ToLatestVersion();
    void CopyFrom(T source);
}
