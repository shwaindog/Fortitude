#region

using FortitudeCommon.EventProcessing;

#endregion

namespace FortitudeIO.Transports.Sockets;

public interface IConnectionUpdate
{
    IConnectionConfig ConnectionConfig { get; }
    EventType EventType { get; }
}

public class ConnectionUpdate : IConnectionUpdate
{
    public ConnectionUpdate(IConnectionConfig connectionConfig, EventType eventType)
    {
        ConnectionConfig = connectionConfig;
        EventType = eventType;
    }

    public IConnectionConfig ConnectionConfig { get; }
    public EventType EventType { get; }
}
