#region

using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeIO.Transports.Sockets.SessionConnection;

#endregion

namespace FortitudeIO.Transports.Sockets.Subscription;

public interface ISocketSelector
{
    DateTime WakeTs { get; }
    void Register(ISocketSessionConnection socketSessionConnection);
    void Unregister(ISocketSessionConnection sessionConnection);
    IEnumerable<ISocketSessionConnection> WatchSocketsForRecv(IPerfLogger? socketReadTraceLogger = null);
}
