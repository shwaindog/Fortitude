// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using FortitudeCommon.Configuration;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Types;
using FortitudeIO.Topics.Config.ConnectionConfig;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeIO.Transports.Network.Config;

public interface IConnection
{
    string? ConnectionName { get; set; }
}

public enum ConnectionSelectionOrder
{
    ListedOrder
  , Random
}

public interface INetworkTopicConnectionConfig : ITopicConnectionConfig, IConnection, IEnumerable<IEndpointConfig>
  , ICloneable<INetworkTopicConnectionConfig>
{
    SocketConversationProtocol    ConversationProtocol     { get; set; }
    IEnumerable<IEndpointConfig>  AvailableConnections     { get; set; }
    public int                    ReceiveBufferSize        { get; set; }
    public int                    SendBufferSize           { get; set; }
    int                           NumberOfReceivesPerPoll  { get; set; }
    SocketConnectionAttributes    ConnectionAttributes     { get; set; }
    ConnectionSelectionOrder      ConnectionSelectionOrder { get; set; }
    uint                          ConnectionTimeoutMs      { get; set; }
    uint                          ResponseTimeoutMs        { get; set; }
    ISocketReconnectConfig        ReconnectConfig          { get; set; }
    INetworkTopicConnectionConfig ShiftPortsBy(ushort deltaPorts);
    INetworkTopicConnectionConfig ToggleProtocolDirection();
}

public class NetworkTopicConnectionConfig : ConfigSection, INetworkTopicConnectionConfig
{
    private static readonly Dictionary<string, string?> Defaults = new()
    {
        { nameof(ReceiveBufferSize), (1024 * 1024 * 2).ToString() }, { nameof(SendBufferSize), (1024 * 1024 * 2).ToString() }
      , { nameof(NumberOfReceivesPerPoll), "50" }, { nameof(ConnectionAttributes), SocketConnectionAttributes.None.ToString() }
      , { nameof(ConnectionSelectionOrder), ConnectionSelectionOrder.ListedOrder.ToString() }, { nameof(ConnectionTimeoutMs), "2000" }
      , { nameof(ResponseTimeoutMs), "10000" }
    };

    private object? ignoreSuppressWarnings;

    public NetworkTopicConnectionConfig(IConfigurationRoot configRoot, string path) : base(configRoot, path)
    {
        foreach (var checkDefault in Defaults) this[checkDefault.Key] ??= checkDefault.Value;
    }

    public NetworkTopicConnectionConfig
    (string connectionName, string topicName, SocketConversationProtocol conversationProtocol
      , IEnumerable<IEndpointConfig> availableConnections,
        string? topicDescription = null, int receiveBufferSize = 1024 * 1024 * 2, int sendBufferSize = 1024 * 1024 * 2,
        int numberOfReceivesPerPoll = 50
      , SocketConnectionAttributes connectionAttributes = SocketConnectionAttributes.None,
        ConnectionSelectionOrder connectionSelectionOrder = ConnectionSelectionOrder.ListedOrder,
        uint connectionTimeoutMs = 10_000, uint responseTimeoutMs = 10_000
      , ISocketReconnectConfig? reconnectConfig = null)
        : this(topicName, conversationProtocol, availableConnections, topicDescription, receiveBufferSize, sendBufferSize, numberOfReceivesPerPoll,
               connectionAttributes, connectionSelectionOrder, connectionTimeoutMs, responseTimeoutMs, reconnectConfig) =>
        ConnectionName = connectionName;

    public NetworkTopicConnectionConfig
    (string topicName, SocketConversationProtocol conversationProtocol
      , IEnumerable<IEndpointConfig> availableConnections,
        string? topicDescription = null, int receiveBufferSize = 1024 * 1024 * 2, int sendBufferSize = 1024 * 1024 * 2,
        int numberOfReceivesPerPoll = 50
      , SocketConnectionAttributes connectionAttributes = SocketConnectionAttributes.None,
        ConnectionSelectionOrder connectionSelectionOrder = ConnectionSelectionOrder.ListedOrder,
        uint connectionTimeoutMs = 10_000, uint responseTimeoutMs = 10_000
      , ISocketReconnectConfig? reconnectConfig = null)
    {
        TopicName                = topicName;
        ConversationProtocol     = conversationProtocol;
        AvailableConnections     = availableConnections.ToList();
        TopicDescription         = topicDescription ?? topicName;
        ReceiveBufferSize        = receiveBufferSize;
        SendBufferSize           = sendBufferSize;
        ConnectionAttributes     = connectionAttributes;
        ConnectionSelectionOrder = connectionSelectionOrder;
        NumberOfReceivesPerPoll  = numberOfReceivesPerPoll;
        ConnectionTimeoutMs      = connectionTimeoutMs;
        ResponseTimeoutMs        = responseTimeoutMs;
        ReconnectConfig          = reconnectConfig ?? new SocketReconnectConfig();
    }

    public NetworkTopicConnectionConfig(INetworkTopicConnectionConfig toClone, IConfigurationRoot configRoot, string path)
        : base(configRoot, path)
    {
        TopicName                = toClone.TopicName;
        ConversationProtocol     = toClone.ConversationProtocol;
        AvailableConnections     = toClone.AvailableConnections.Select(scc => scc.Clone()).ToList();
        TopicDescription         = toClone.TopicDescription;
        ReceiveBufferSize        = toClone.ReceiveBufferSize;
        SendBufferSize           = toClone.SendBufferSize;
        ConnectionAttributes     = toClone.ConnectionAttributes;
        ConnectionSelectionOrder = toClone.ConnectionSelectionOrder;
        NumberOfReceivesPerPoll  = toClone.NumberOfReceivesPerPoll;
        ConnectionTimeoutMs      = toClone.ConnectionTimeoutMs;
        ResponseTimeoutMs        = toClone.ResponseTimeoutMs;
        ReconnectConfig          = toClone.ReconnectConfig.Clone();
    }

    public NetworkTopicConnectionConfig(INetworkTopicConnectionConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public INetworkTopicConnectionConfig ShiftPortsBy(ushort deltaPorts)
    {
        var shiftedPorts                                                                                = new NetworkTopicConnectionConfig(this);
        foreach (var availableConnection in shiftedPorts.AvailableConnections) availableConnection.Port += deltaPorts;
        return shiftedPorts;
    }

    public string? ConnectionName
    {
        get => this[nameof(ConnectionName)];
        set => this[nameof(ConnectionName)] = value;
    }

    public string TopicName
    {
        get => this[nameof(TopicName)]!;
        set => this[nameof(TopicName)] = value;
    }

    public SocketConversationProtocol ConversationProtocol
    {
        get => Enum.Parse<SocketConversationProtocol>(this[nameof(ConversationProtocol)]!);
        set => this[nameof(ConversationProtocol)] = value.ToString();
    }

    public IEnumerable<IEndpointConfig> AvailableConnections
    {
        get
        {
            var autoRecycleList = Recycler.Borrow<AutoRecycledEnumerable<IEndpointConfig>>();
            foreach (var configurationSection in GetSection(nameof(AvailableConnections)).GetChildren())
                if (configurationSection["HostName"] != null)
                    autoRecycleList.Add(new EndpointConfig(ConfigRoot, configurationSection.Path));
            return autoRecycleList;
        }
        set
        {
            var oldCount = AvailableConnections.Count();
            var i        = 0;
            foreach (var remoteServiceConfig in value)
            {
                ignoreSuppressWarnings = new EndpointConfig(remoteServiceConfig, ConfigRoot
                                                          , Path + ":" + nameof(AvailableConnections) + $":{i}");
                i++;
            }

            for (var j = i; j < oldCount; j++) EndpointConfig.ClearValues(ConfigRoot, Path + ":" + nameof(AvailableConnections) + $":{i}");
        }
    }

    public string? TopicDescription
    {
        get => this[nameof(TopicDescription)];
        set => this[nameof(TopicDescription)] = value;
    }

    public int ReceiveBufferSize
    {
        get
        {
            var checkValue = this[nameof(ReceiveBufferSize)];
            return checkValue != null ? int.Parse(checkValue) : 2 * 1024 * 1024;
        }
        set => this[nameof(ReceiveBufferSize)] = value.ToString();
    }

    public int SendBufferSize
    {
        get
        {
            var checkValue = this[nameof(SendBufferSize)];
            return checkValue != null ? int.Parse(checkValue) : 2 * 1024 * 1024;
        }
        set => this[nameof(SendBufferSize)] = value.ToString();
    }

    public int NumberOfReceivesPerPoll
    {
        get
        {
            var checkValue = this[nameof(NumberOfReceivesPerPoll)];
            return checkValue != null ? int.Parse(checkValue) : 10;
        }
        set => this[nameof(NumberOfReceivesPerPoll)] = value.ToString();
    }

    public SocketConnectionAttributes ConnectionAttributes
    {
        get
        {
            var checkValue = this[nameof(ConnectionAttributes)];
            return checkValue != null ? Enum.Parse<SocketConnectionAttributes>(checkValue) : SocketConnectionAttributes.Reliable;
        }
        set => this[nameof(ConnectionAttributes)] = value.ToString();
    }

    public ConnectionSelectionOrder ConnectionSelectionOrder
    {
        get
        {
            var checkValue = this[nameof(ConnectionSelectionOrder)];
            return checkValue != null ? Enum.Parse<ConnectionSelectionOrder>(checkValue) : ConnectionSelectionOrder.ListedOrder;
        }
        set => this[nameof(ConnectionSelectionOrder)] = value.ToString();
    }

    public uint ConnectionTimeoutMs
    {
        get
        {
            var checkValue = this[nameof(ConnectionTimeoutMs)];
            return checkValue != null ? uint.Parse(checkValue) : 2_000_000u;
        }
        set => this[nameof(ConnectionTimeoutMs)] = value.ToString();
    }

    public uint ResponseTimeoutMs
    {
        get
        {
            var checkValue = this[nameof(ResponseTimeoutMs)];
            return checkValue != null ? uint.Parse(checkValue) : 2_000_000u;
        }
        set => this[nameof(ResponseTimeoutMs)] = value.ToString();
    }

    public ISocketReconnectConfig ReconnectConfig
    {
        get => new SocketReconnectConfig(ConfigRoot, Path + $":{nameof(ReconnectConfig)}");
        set => ignoreSuppressWarnings = new SocketReconnectConfig(value, ConfigRoot, Path + $":{nameof(ReconnectConfig)}");
    }

    public TransportType TransportType => TransportType.Sockets;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<IEndpointConfig> GetEnumerator()
    {
        var autoRecycleList = Recycler.Borrow<AutoRecycledEnumerable<IEndpointConfig>>();
        foreach (var endpointConfig in AvailableConnections) autoRecycleList.Add(endpointConfig);
        if (ConnectionSelectionOrder == ConnectionSelectionOrder.Random) autoRecycleList.ReusableBackingList.Shuffle();

        return autoRecycleList.GetEnumerator();
    }

    object ICloneable.Clone() => Clone();

    public INetworkTopicConnectionConfig ToggleProtocolDirection()
    {
        var oppositeConnectionConfig = new NetworkTopicConnectionConfig(this);
        oppositeConnectionConfig.ConversationProtocol = oppositeConnectionConfig.ConversationProtocol switch
                                                        {
                                                            SocketConversationProtocol.TcpAcceptor   => SocketConversationProtocol.TcpClient
                                                          , SocketConversationProtocol.TcpClient     => SocketConversationProtocol.TcpAcceptor
                                                          , SocketConversationProtocol.UdpPublisher  => SocketConversationProtocol.UdpSubscriber
                                                          , SocketConversationProtocol.UdpSubscriber => SocketConversationProtocol.UdpPublisher
                                                          , _ => throw new
                                                                NotSupportedException($"Did not expect to receive SocketConversationProtocol unknown")
                                                        };
        return oppositeConnectionConfig;
    }

    public INetworkTopicConnectionConfig Clone() =>
        new NetworkTopicConnectionConfig(TopicName, ConversationProtocol,
                                         AvailableConnections.ToList(), TopicDescription, ReceiveBufferSize, SendBufferSize, NumberOfReceivesPerPoll,
                                         ConnectionAttributes, ConnectionSelectionOrder, ConnectionTimeoutMs, ResponseTimeoutMs, ReconnectConfig);


    public static void ClearValues(IConfigurationRoot root, string path)
    {
        root[path + ":" + nameof(ConnectionName)]          = null;
        root[path + ":" + nameof(TopicName)]               = null;
        root[path + ":" + nameof(ConversationProtocol)]    = null;
        root[path + ":" + nameof(AvailableConnections)]    = null;
        root[path + ":" + nameof(TopicDescription)]        = null;
        root[path + ":" + nameof(ReceiveBufferSize)]       = null;
        root[path + ":" + nameof(SendBufferSize)]          = null;
        root[path + ":" + nameof(ConnectionAttributes)]    = null;
        root[path + ":" + nameof(NumberOfReceivesPerPoll)] = null;
        root[path + ":" + nameof(ConnectionTimeoutMs)]     = null;
        root[path + ":" + nameof(ResponseTimeoutMs)]       = null;
        root[path + ":" + nameof(ReconnectConfig)]         = null;
    }

    protected bool Equals(INetworkTopicConnectionConfig other)
    {
        var connectionNameSame        = ConnectionName == other.ConnectionName;
        var topicNameSame             = TopicName == other.TopicName;
        var conversationProtocolSame  = ConversationProtocol == other.ConversationProtocol;
        var availableConnectionsSame  = AvailableConnections.SequenceEqual(other.AvailableConnections);
        var topicDescriptionSame      = TopicDescription == other.TopicDescription;
        var receivedBufferSizeSame    = ReceiveBufferSize == other.ReceiveBufferSize;
        var sendBufferSizeSame        = SendBufferSize == other.SendBufferSize;
        var numReceivesPerPollSame    = NumberOfReceivesPerPoll == other.NumberOfReceivesPerPoll;
        var connectionAttsSame        = ConnectionAttributes == other.ConnectionAttributes;
        var connectionSelectOrderSame = ConnectionSelectionOrder == other.ConnectionSelectionOrder;
        var connTimeoutMsSame         = ConnectionTimeoutMs == other.ConnectionTimeoutMs;
        var responseTimeoutMsSame     = ResponseTimeoutMs == other.ResponseTimeoutMs;
        var reconnectConfigSame       = ReconnectConfig.Equals(other.ReconnectConfig);

        return connectionNameSame && topicNameSame && conversationProtocolSame && availableConnectionsSame
            && topicDescriptionSame && receivedBufferSizeSame
            && sendBufferSizeSame && numReceivesPerPollSame && connectionAttsSame && connectionSelectOrderSame
            && connTimeoutMsSame && responseTimeoutMsSame && reconnectConfigSame;
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
        return hashCode.ToHashCode();
    }

    public override string ToString() =>
        $"SocketTopicConnectionConfig({nameof(ConnectionName)}: {ConnectionName}, {nameof(TopicName)}: {TopicName}, " +
        $"{nameof(ConversationProtocol)}: {ConversationProtocol}, {nameof(AvailableConnections)}: {AvailableConnections}, " +
        $"{nameof(TopicDescription)}: {TopicDescription}, {nameof(ReceiveBufferSize)}: {ReceiveBufferSize}, {nameof(SendBufferSize)}: {SendBufferSize}, " +
        $"{nameof(NumberOfReceivesPerPoll)}: {NumberOfReceivesPerPoll}, {nameof(ConnectionAttributes)}: {ConnectionAttributes}, " +
        $"{nameof(ConnectionSelectionOrder)}: {ConnectionSelectionOrder}, {nameof(ConnectionTimeoutMs)}: {ConnectionTimeoutMs}, " +
        $"{nameof(ResponseTimeoutMs)}: {ResponseTimeoutMs}, {nameof(ReconnectConfig)}: {ReconnectConfig})";
}
