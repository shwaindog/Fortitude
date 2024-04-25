#region

using System.Net;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Conversations;
using FortitudeIO.Topics.Config.ConnectionConfig;

#endregion

namespace FortitudeIO.Transports.Network.Sockets;

public interface ISocketSessionInfo : ITopicEndpointInfo
{
    ushort ConnectedPort { get; set; }
    IPAddress ConnectedIpAddress { get; set; }
}

public interface ISocketConnection : ISocketSessionInfo
{
    IOSSocket OSSocket { get; }
    bool IsConnected { get; }

    bool IsAuthenticated { get; set; }
}

public class SocketConnection : ISocketConnection
{
    private ConversationState? connectionState;

    public SocketConnection(string instanceName, ConversationType conversationType, IOSSocket osSocket
        , IPAddress remoteIPAddress, ushort remotePort)
    {
        InstanceName = instanceName;
        ConversationType = conversationType;
        OSSocket = osSocket;
        ConnectedIpAddress = remoteIPAddress;
        ConnectedPort = remotePort;
    }

    public TransportType TransportType => TransportType.Sockets;
    public ConversationType ConversationType { get; }
    public string InstanceName { get; set; }
    public IOSSocket OSSocket { get; }
    public IPAddress ConnectedIpAddress { get; set; }
    public ushort ConnectedPort { get; set; }

    public bool IsAuthenticated { get; set; }

    public ConversationState ConversationState
    {
        get
        {
            if (connectionState != null && connectionState != ConversationState.New) return connectionState.Value;
            return IsConnected ? ConversationState.Started : ConversationState.Stopped;
        }
        set => connectionState = value;
    }

    public bool IsConnected => OSSocket != null && (OSSocket.Connected || OSSocket.IsBound);

    public bool EquivalentEndpoint(ITopicEndpointInfo test)
    {
        var socketEndpoint = test as ISocketSessionInfo;
        return socketEndpoint != null && TransportType == test.TransportType &&
               InstanceName == socketEndpoint.InstanceName &&
               ConnectedIpAddress == socketEndpoint.ConnectedIpAddress &&
               ConnectedPort == socketEndpoint.ConnectedPort;
    }
}
