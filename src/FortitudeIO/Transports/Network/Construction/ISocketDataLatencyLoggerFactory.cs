#region

using FortitudeIO.Transports.Network.Logging;

#endregion

namespace FortitudeIO.Transports.Network.Construction;

public interface ISocketDataLatencyLoggerFactory
{
    ISocketDataLatencyLogger GetSocketDataLatencyLogger(string key);
}
