namespace FortitudeIO.Transports.NewSocketAPI.Logging;

public interface ISocketDataLatencyLoggerFactory
{
    ISocketDataLatencyLogger GetSocketDataLatencyLogger(string key);
}
