#region

using FortitudeCommon.EventProcessing;

#endregion

namespace FortitudeIO.Transports.Sockets;

public interface IConnectionUpdate
{
    IConnectionConfig ConnectionConfig { get; }
    EventType EventType { get; }
}

public class ConnectionUpdate(IConnectionConfig connectionConfig, EventType eventType) : IConnectionUpdate
{
    public IConnectionConfig ConnectionConfig { get; } = connectionConfig;
    public EventType EventType { get; } = eventType;
}
