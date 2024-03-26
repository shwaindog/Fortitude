#region

using FortitudeIO.Transports.NewSocketAPI.Logging;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Construction;

public interface ISocketDataLatencyLoggerFactory
{
    ISocketDataLatencyLogger GetSocketDataLatencyLogger(string key);
}
