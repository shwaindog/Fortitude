#region

using FortitudeCommon.EventProcessing;
using FortitudeIO.Transports.NewSocketAPI.Config;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI;

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
