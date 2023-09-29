using System;
using System.Collections.Generic;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeIO.Transports.Sockets.SessionConnection;

namespace FortitudeIO.Transports.Sockets.Subscription
{
    public interface ISocketSelector
    {
        void Register(ISocketSessionConnection socketSessionConnection);
        void Unregister(ISocketSessionConnection sessionConnection);
        DateTime WakeTs { get; }
        IEnumerable<ISocketSessionConnection> WatchSocketsForRecv(IPerfLogger socketReadTraceLogger = null);
    }
}