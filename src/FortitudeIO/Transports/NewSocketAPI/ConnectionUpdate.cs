#region

using FortitudeCommon.EventProcessing;
using FortitudeIO.Transports.NewSocketAPI.Config;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI;

public interface IConnectionUpdate
{
    ISocketConnectionConfig ConnectionConfig { get; }
    EventType EventType { get; }
}

public class ConnectionUpdate(ISocketConnectionConfig connectionConfig, EventType eventType) : IConnectionUpdate
{
    public ISocketConnectionConfig ConnectionConfig { get; } = connectionConfig;
    public EventType EventType { get; } = eventType;
}
