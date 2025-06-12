using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeIO.Transports.Network.Config;

public interface IConnectedEndpoint
{
    IEndpointConfig ConnectedEndpointConfig { get; }

    DateTime ConnectedAt { get;  }
}

public class ConnectedEndpoint(DateTime connectedAt, IEndpointConfig connectedEndpointConfig) : IConnectedEndpoint
{
    public IEndpointConfig ConnectedEndpointConfig { get; } = connectedEndpointConfig;
    public DateTime        ConnectedAt             { get; } = connectedAt;

    public override string ToString() => 
        $"{nameof(ConnectedEndpoint)}{{{nameof(ConnectedEndpointConfig)}: {ConnectedEndpointConfig}, {nameof(ConnectedAt)}: {ConnectedAt}}}";
}