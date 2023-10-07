#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeIO.Transports.Sockets;

public interface IConnectionConfig : ICloneable<IConnectionConfig>
{
    long Id { get; }
    string ConnectionName { get; }
    string Hostname { get; }
    int Port { get; }
    string NetworkSubAddress { get; }
    ConnectionDirectionType ConnectionDirectionType { get; }
    IObservable<IConnectionUpdate> Updates { get; set; }
    uint ReconnectIntervalMs { get; }
    IConnectionConfig? FallBackConnectionConfig { get; }
}
