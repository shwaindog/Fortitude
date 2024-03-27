#region

using FortitudeCommon.EventProcessing;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Config;

public interface IConnectionUpdate
{
    ISocketTopicConnectionConfig ConnectionConfig { get; }
    EventType EventType { get; }
}

public class ConnectionUpdate(ISocketTopicConnectionConfig connectionConfig, EventType eventType) : IConnectionUpdate
{
    public ISocketTopicConnectionConfig ConnectionConfig { get; } = connectionConfig;
    public EventType EventType { get; } = eventType;
}
