namespace FortitudeIO.Transports.Sockets.Logging;

public interface ISocketDataLatencyLoggerFactory
{
    ISocketDataLatencyLogger GetSocketDataLatencyLogger(string key);
}
