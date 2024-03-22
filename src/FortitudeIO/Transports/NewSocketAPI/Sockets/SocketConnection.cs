#region

using System.Net;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Conversations;
using FortitudeIO.Topics.Config.ConnectionConfig;
using FortitudeIO.Transports.NewSocketAPI.Config;
using TransportType = FortitudeIO.Topics.Config.ConnectionConfig.TransportType;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Sockets;

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
        ConnectedIPAddress = remoteIPAddress;
        ConnectedPort = remotePort;
    }

    public TransportType TransportType => TransportType.Sockets;
    public ConversationType ConversationType { get; }
    public string InstanceName { get; set; }
    public IOSSocket OSSocket { get; }
    public IPAddress ConnectedIPAddress { get; set; }
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
               ConnectedIPAddress == socketEndpoint.ConnectedIPAddress &&
               ConnectedPort == socketEndpoint.ConnectedPort;
    }
}
