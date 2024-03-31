#region

using FortitudeCommon.EventProcessing;

#endregion

namespace FortitudeIO.Transports.Network.Config;

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
