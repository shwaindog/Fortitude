#region

using FortitudeCommon.EventProcessing;
using FortitudeIO.Transports.NewSocketAPI.Config;

#endregion

namespace FortitudeIO.Transports.Sockets;

public interface IConnectionUpdate
{
    INetworkTopicConnectionConfig ConnectionConfig { get; }
    EventType EventType { get; }
}

public class ConnectionUpdate(INetworkTopicConnectionConfig connectionConfig, EventType eventType) : IConnectionUpdate
{
    public INetworkTopicConnectionConfig ConnectionConfig { get; } = connectionConfig;
    public EventType EventType { get; } = eventType;
}
