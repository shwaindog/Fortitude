#region

using System.Net;
using FortitudeIO.Topics.Config.ConnectionConfig;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Config;

public interface ISocketSessionInfo : ITopicEndpointInfo
{
    ushort ConnectedPort { get; set; }
    IPAddress ConnectedIPAddress { get; set; }
}
