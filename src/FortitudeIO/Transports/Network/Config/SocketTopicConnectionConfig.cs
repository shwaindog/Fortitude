#region

using System.Collections;
using FortitudeCommon.Types;
using FortitudeIO.Topics.Config.ConnectionConfig;

#endregion

namespace FortitudeIO.Transports.Network.Config;

public enum ConnectionSelectionOrder
{
    ListedOrder
    , Random
}

public interface INetworkTopicConnectionConfig : ITopicConnectionConfig, IEnumerable<IEndpointConfig>
    , IEnumerator<IEndpointConfig>, ICloneable<INetworkTopicConnectionConfig>
{
    SocketConversationProtocol ConversationProtocol { get; set; }
    List<IEndpointConfig> AvailableConnections { get; set; }
    public int ReceiveBufferSize { get; set; }
    public int SendBufferSize { get; set; }
    int NumberOfReceivesPerPoll { get; set; }
    SocketConnectionAttributes ConnectionAttributes { get; set; }
    ConnectionSelectionOrder ConnectionSelectionOrder { get; set; }
    uint ConnectionTimeoutMs { get; set; }
    uint ResponseTimeoutMs { get; set; }
    ISocketReconnectConfig ReconnectConfig { get; set; }
    INetworkTopicConnectionConfig ToggleProtocolDirection();
}

public class NetworkTopicConnectionConfig : INetworkTopicConnectionConfig
{
    private readonly List<IEndpointConfig> returnedItems = new();

    public NetworkTopicConnectionConfig(string topicName, SocketConversationProtocol conversationProtocol
        , IEnumerable<IEndpointConfig> availableConnections,
        string? topicDescription = null, int receiveBufferSize = 1024 * 1024 * 2, int sendBufferSize = 1024 * 1024 * 2,
        int numberOfReceivesPerPoll = 50
        , SocketConnectionAttributes connectionAttributes = SocketConnectionAttributes.None,
        ConnectionSelectionOrder connectionSelectionOrder = ConnectionSelectionOrder.ListedOrder,
        uint connectionTimeoutMs = 2_000, uint responseTimeoutMs = 10_000
        , ISocketReconnectConfig? reconnectConfig = null)
    {
        TopicName = topicName;
        ConversationProtocol = conversationProtocol;
        AvailableConnections = availableConnections.ToList();
        TopicDescription = topicDescription ?? topicName;
        ReceiveBufferSize = receiveBufferSize;
        SendBufferSize = sendBufferSize;
        ConnectionAttributes = connectionAttributes;
        ConnectionSelectionOrder = connectionSelectionOrder;
        NumberOfReceivesPerPoll = numberOfReceivesPerPoll;
        ConnectionTimeoutMs = connectionTimeoutMs;
        ResponseTimeoutMs = responseTimeoutMs;
        ReconnectConfig = reconnectConfig ?? new SocketReconnectConfig();
        Current = AvailableConnections.First();
    }

    public NetworkTopicConnectionConfig(INetworkTopicConnectionConfig toClone)
    {
        TopicName = toClone.TopicName;
        ConversationProtocol = toClone.ConversationProtocol;
        AvailableConnections = toClone.AvailableConnections.Select(scc => scc.Clone()).ToList();
        TopicDescription = toClone.TopicDescription;
        ReceiveBufferSize = toClone.ReceiveBufferSize;
        SendBufferSize = toClone.SendBufferSize;
        ConnectionAttributes = toClone.ConnectionAttributes;
        ConnectionSelectionOrder = toClone.ConnectionSelectionOrder;
        NumberOfReceivesPerPoll = toClone.NumberOfReceivesPerPoll;
        ConnectionTimeoutMs = toClone.ConnectionTimeoutMs;
        ResponseTimeoutMs = toClone.ResponseTimeoutMs;
        ReconnectConfig = toClone.ReconnectConfig.Clone();
        Current = AvailableConnections.First();
    }

    public string TopicName { get; set; }
    public SocketConversationProtocol ConversationProtocol { get; set; }
    public List<IEndpointConfig> AvailableConnections { get; set; }
    public string? TopicDescription { get; set; }
    public int ReceiveBufferSize { get; set; }
    public int SendBufferSize { get; set; }
    public int NumberOfReceivesPerPoll { get; set; }
    public SocketConnectionAttributes ConnectionAttributes { get; set; }
    public ConnectionSelectionOrder ConnectionSelectionOrder { get; set; }
    public uint ConnectionTimeoutMs { get; set; }
    public uint ResponseTimeoutMs { get; set; }
    public ISocketReconnectConfig ReconnectConfig { get; set; }
    public TransportType TransportType => TransportType.Sockets;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<IEndpointConfig> GetEnumerator()
    {
        returnedItems.Clear();
        return this;
    }

    public bool MoveNext()
    {
        if (returnedItems.Count == AvailableConnections.Count) return false;
        if (ConnectionSelectionOrder == ConnectionSelectionOrder.Random)
        {
            var remaining = AvailableConnections.Except(returnedItems).ToList();
            var selectedItem = Random.Shared.Next(0, remaining.Count);
            Current = remaining[selectedItem];
            returnedItems.Add(Current);
        }
        else
        {
            var indexOfCurrent = AvailableConnections.IndexOf(Current);
            if (returnedItems.Count > 0) indexOfCurrent++;
            Current = AvailableConnections[indexOfCurrent];
            returnedItems.Add(Current);
        }

        return true;
    }

    public void Reset()
    {
        returnedItems.Clear();
    }

    object IEnumerator.Current => Current;

    public void Dispose()
    {
        returnedItems.Clear();
    }

    public IEndpointConfig Current { get; private set; }

    object ICloneable.Clone() => Clone();

    public INetworkTopicConnectionConfig ToggleProtocolDirection()
    {
        var oppositeConnectionConfig = new NetworkTopicConnectionConfig(this);
        oppositeConnectionConfig.ConversationProtocol = oppositeConnectionConfig.ConversationProtocol switch
        {
            SocketConversationProtocol.TcpAcceptor => SocketConversationProtocol.TcpClient
            , SocketConversationProtocol.TcpClient => SocketConversationProtocol.TcpAcceptor
            , SocketConversationProtocol.UdpPublisher => SocketConversationProtocol.UdpSubscriber
            , SocketConversationProtocol.UdpSubscriber => SocketConversationProtocol.UdpPublisher
            , _ => throw new NotSupportedException($"Did not expect to receive SocketConversationProtocol unknown")
        };
        return oppositeConnectionConfig;
    }

    public INetworkTopicConnectionConfig Clone() =>
        new NetworkTopicConnectionConfig(TopicName, ConversationProtocol,
            AvailableConnections.ToList(), TopicDescription, ReceiveBufferSize, SendBufferSize, NumberOfReceivesPerPoll,
            ConnectionAttributes, ConnectionSelectionOrder, ConnectionTimeoutMs, ResponseTimeoutMs, ReconnectConfig);

    protected bool Equals(INetworkTopicConnectionConfig other)
    {
        var topicNameSame = TopicName == other.TopicName;
        var conversationProtocolSame = ConversationProtocol == other.ConversationProtocol;
        var availableConnectionsSame = AvailableConnections.SequenceEqual(other.AvailableConnections);
        var topicDescriptionSame = TopicDescription == other.TopicDescription;
        var receivedBufferSizeSame = ReceiveBufferSize == other.ReceiveBufferSize;
        var sendBufferSizeSame = SendBufferSize == other.SendBufferSize;
        var numReceivesPerPollSame = NumberOfReceivesPerPoll == other.NumberOfReceivesPerPoll;
        var connectionAttsSame = ConnectionAttributes == other.ConnectionAttributes;
        var connectionSelectOrderSame = ConnectionSelectionOrder == other.ConnectionSelectionOrder;
        var connTimeoutMsSame = ConnectionTimeoutMs == other.ConnectionTimeoutMs;
        var responseTimeoutMsSame = ResponseTimeoutMs == other.ResponseTimeoutMs;
        var reconnectConfigSame = ReconnectConfig.Equals(other.ReconnectConfig);

        return topicNameSame && conversationProtocolSame && availableConnectionsSame && topicDescriptionSame &&
               receivedBufferSizeSame
               && sendBufferSizeSame && numReceivesPerPollSame && connectionAttsSame && connectionSelectOrderSame &&
               connTimeoutMsSame &&
               responseTimeoutMsSame && reconnectConfigSame;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((INetworkTopicConnectionConfig)obj);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(TopicName);
        hashCode.Add((int)ConversationProtocol);
        hashCode.Add(AvailableConnections);
        hashCode.Add(TopicDescription);
        hashCode.Add(ReceiveBufferSize);
        hashCode.Add(SendBufferSize);
        hashCode.Add(NumberOfReceivesPerPoll);
        hashCode.Add((int)ConnectionAttributes);
        hashCode.Add((int)ConnectionSelectionOrder);
        hashCode.Add(ConnectionTimeoutMs);
        hashCode.Add(ResponseTimeoutMs);
        hashCode.Add(ReconnectConfig);
        return hashCode.ToHashCode();
    }

    public override string ToString() =>
        $"SocketTopicConnectionConfig({nameof(TopicName)}: {TopicName}, {nameof(ConversationProtocol)}: {ConversationProtocol}, " +
        $"{nameof(AvailableConnections)}: {AvailableConnections}, {nameof(TopicDescription)}: {TopicDescription}, {nameof(ReceiveBufferSize)}: " +
        $"{ReceiveBufferSize}, {nameof(SendBufferSize)}: {SendBufferSize}, {nameof(NumberOfReceivesPerPoll)}: {NumberOfReceivesPerPoll}, " +
        $"{nameof(ConnectionAttributes)}: {ConnectionAttributes}, {nameof(ConnectionSelectionOrder)}: {ConnectionSelectionOrder}, " +
        $"{nameof(ConnectionTimeoutMs)}: {ConnectionTimeoutMs}, {nameof(ResponseTimeoutMs)}: {ResponseTimeoutMs}, {nameof(ReconnectConfig)}: " +
        $"{ReconnectConfig}, {nameof(Current)}: {Current})";
}
