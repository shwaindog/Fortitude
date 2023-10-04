using FortitudeCommon.Configuration;
using FortitudeCommon.EventProcessing;

namespace FortitudeIO.Transports.Sockets
{
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
        public IConnectionConfig ConnectionConfig { get; private set; }
        public EventType EventType { get; private set; }
    }
}