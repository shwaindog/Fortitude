// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using FortitudeCommon.Config;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Extensions;
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
    const int  DefaultReceiveBufferSize       = 1024 * 1024 * 4;
    const int  DefaultSendBufferSize          = 1024 * 1024 * 4;
    const int  DefaultNumberOfReceivesPerPoll = 50;
    const uint DefaultConnectionTimeoutMs     = 60_000;
    const uint DefaultResponseTimeoutMs       = 60_000;


    const SocketConnectionAttributes DefaultConnectionAttributes     = SocketConnectionAttributes.None;
    const ConnectionSelectionOrder   DefaultConnectionSelectionOrder = ConnectionSelectionOrder.ListedOrder;

    SocketConversationProtocol   ConversationProtocol     { get; set; }
    SocketConnectionAttributes   ConnectionAttributes     { get; set; }
    IEnumerable<IEndpointConfig> AvailableConnections     { get; set; }
    ConnectionSelectionOrder     ConnectionSelectionOrder { get; set; }

    IConnectedEndpoint? ConnectedEndpoint { get; set; }

    int  ReceiveBufferSize       { get; set; }
    int  SendBufferSize          { get; set; }
    int  NumberOfReceivesPerPoll { get; set; }
    uint ConnectionTimeoutMs     { get; set; }
    uint ResponseTimeoutMs       { get; set; }

    ISocketReconnectConfig        ReconnectConfig { get; set; }
    INetworkTopicConnectionConfig ShiftPortsBy(ushort deltaPorts);
    INetworkTopicConnectionConfig ToggleProtocolDirection();
}

public class NetworkTopicConnectionConfig : ConfigSection, INetworkTopicConnectionConfig
{
    private static readonly Dictionary<string, string?> Defaults = new()
    {
        { nameof(ReceiveBufferSize), INetworkTopicConnectionConfig.DefaultReceiveBufferSize.ToString() }
      , { nameof(SendBufferSize), INetworkTopicConnectionConfig.DefaultSendBufferSize.ToString() }
      , { nameof(NumberOfReceivesPerPoll), INetworkTopicConnectionConfig.DefaultNumberOfReceivesPerPoll.ToString() }
      , { nameof(ConnectionAttributes), INetworkTopicConnectionConfig.DefaultConnectionAttributes.ToString() }
      , { nameof(ConnectionSelectionOrder), INetworkTopicConnectionConfig.DefaultConnectionSelectionOrder.ToString() }
      , { nameof(ConnectionTimeoutMs), INetworkTopicConnectionConfig.DefaultConnectionTimeoutMs.ToString() }
      , { nameof(ResponseTimeoutMs), INetworkTopicConnectionConfig.DefaultResponseTimeoutMs.ToString() }
    };

    public NetworkTopicConnectionConfig(IConfigurationRoot configRoot, string path) : base(configRoot, path)
    {
        foreach (var checkDefault in Defaults) this[checkDefault.Key] ??= checkDefault.Value;
    }

    public NetworkTopicConnectionConfig
    (string connectionName, string topicName, SocketConversationProtocol conversationProtocol
      , IEnumerable<IEndpointConfig> availableConnections
      , string? topicDescription = null
      , int receiveBufferSize = INetworkTopicConnectionConfig.DefaultReceiveBufferSize
      , int sendBufferSize = INetworkTopicConnectionConfig.DefaultSendBufferSize
      , int numberOfReceivesPerPoll = INetworkTopicConnectionConfig.DefaultNumberOfReceivesPerPoll
      , SocketConnectionAttributes connectionAttributes = INetworkTopicConnectionConfig.DefaultConnectionAttributes
      , ConnectionSelectionOrder connectionSelectionOrder = INetworkTopicConnectionConfig.DefaultConnectionSelectionOrder
      , uint connectionTimeoutMs = INetworkTopicConnectionConfig.DefaultConnectionTimeoutMs
      , uint responseTimeoutMs = INetworkTopicConnectionConfig.DefaultResponseTimeoutMs
      , ISocketReconnectConfig? reconnectConfig = null)
        : this(topicName, conversationProtocol, availableConnections, topicDescription, receiveBufferSize, sendBufferSize, numberOfReceivesPerPoll,
               connectionAttributes, connectionSelectionOrder, connectionTimeoutMs, responseTimeoutMs, reconnectConfig) =>
        ConnectionName = connectionName;

    public NetworkTopicConnectionConfig
    (string topicName, SocketConversationProtocol conversationProtocol
      , IEnumerable<IEndpointConfig> availableConnections
      , string? topicDescription = null
      , int receiveBufferSize = INetworkTopicConnectionConfig.DefaultReceiveBufferSize
      , int sendBufferSize = INetworkTopicConnectionConfig.DefaultSendBufferSize
      , int numberOfReceivesPerPoll = INetworkTopicConnectionConfig.DefaultNumberOfReceivesPerPoll
      , SocketConnectionAttributes connectionAttributes = INetworkTopicConnectionConfig.DefaultConnectionAttributes
      , ConnectionSelectionOrder connectionSelectionOrder = INetworkTopicConnectionConfig.DefaultConnectionSelectionOrder
      , uint connectionTimeoutMs = INetworkTopicConnectionConfig.DefaultConnectionTimeoutMs
      , uint responseTimeoutMs = INetworkTopicConnectionConfig.DefaultResponseTimeoutMs
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
        if (toClone is NetworkTopicConnectionConfig networkTopicConnConfig)
        {
            ConnectionName       = networkTopicConnConfig[nameof(ConnectionName)];
            ParentConnectionName = networkTopicConnConfig.ParentConnectionName;
        }
        else
        {
            ConnectionName = toClone.ConnectionName;
        }
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
        var shiftedPorts = new NetworkTopicConnectionConfig(this);

        foreach (var availableConnection in shiftedPorts.AvailableConnections) availableConnection.Port += deltaPorts;
        return shiftedPorts;
    }

    public string? ConnectionName
    {
        get => this[nameof(ConnectionName)] ?? ParentConnectionName;
        set => this[nameof(ConnectionName)] = value;
    }

    public string? ParentConnectionName { get ; set ; }

    public string TopicName
    {
        get => this[nameof(TopicName)]!;
        set => this[nameof(TopicName)] = value;
    }

    public SocketConversationProtocol ConversationProtocol
    {
        get
        {
            var checkValue = this[nameof(ConversationProtocol)];
            return checkValue.IsNotNullOrEmpty() ? Enum.Parse<SocketConversationProtocol>(checkValue!) : SocketConversationProtocol.Unknown;
        }
        set => this[nameof(ConversationProtocol)] = value.ToString();
    }

    public SocketConnectionAttributes ConnectionAttributes
    {
        get
        {
            var checkValue = this[nameof(ConnectionAttributes)];
            return checkValue != null ? Enum.Parse<SocketConnectionAttributes>(checkValue) : SocketConnectionAttributes.Reliable;
        }
        set
        {
            if (value.HasMulticastFlag())
            {
                if (ConversationProtocol == SocketConversationProtocol.Unknown)
                {
                    ConversationProtocol = SocketConversationProtocol.UdpPublisher;
                }
                else if (ConversationProtocol is not (SocketConversationProtocol.UdpPublisher or SocketConversationProtocol.UdpSubscriber))
                {
                    throw new ArgumentException("Expected ConversationProtocol to be UdpPublisher or UdpSubscriber when setting ConnectionAttributes to Multicast");
                }
            }
            this[nameof(ConnectionAttributes)] = value.ToString();
        }
    }

    public IEnumerable<IEndpointConfig> AvailableConnections
    {
        get
        {
            var autoRecycleList = Recycler.Borrow<AutoRecycledEnumerable<IEndpointConfig>>();
            int i      = 0;
            foreach (var configurationSection in GetSection(nameof(AvailableConnections)).GetChildren())
            {
                if (configurationSection["HostName"] != null)
                {
                    var endpointConfig = new EndpointConfig(ConfigRoot, configurationSection.Path);
                    UpdateEndpointConnectionName(endpointConfig, i);
                    autoRecycleList.Add(endpointConfig);
                }
                i++;
            }
            return autoRecycleList;
        }
        set
        {
            var oldCount = AvailableConnections.Count();
            var i        = 0;
            foreach (var remoteServiceConfig in value)
            {
                var endpointConfig = new EndpointConfig(remoteServiceConfig, ConfigRoot, $"{Path}{Split}{nameof(AvailableConnections)}{Split}{i}");
                UpdateEndpointConnectionName(endpointConfig, i);
                i++;
            }

            for (var j = i; j < oldCount; j++) EndpointConfig.ClearValues(ConfigRoot, $"{Path}{Split}{nameof(AvailableConnections)}{Split}{i}");
        }
    }

    private void UpdateEndpointConnectionName(EndpointConfig endpointConfig, int i)
    {
        if (ConnectionName.IsNotNullOrEmpty() && (endpointConfig.InstanceName.IsNullOrEmpty() || !endpointConfig.InstanceName.Contains(ConnectionName!)))
        {
            if (endpointConfig.InstanceName.IsNotNullOrEmpty())
            {
                var lastInstanceNamePart = endpointConfig.InstanceName.Split("_").Last();
                endpointConfig.InstanceName = ConnectionName! + "_" + lastInstanceNamePart;
            }
            else
            {
                endpointConfig.InstanceName = ConnectionName! + "_" + i;
            }
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
            return checkValue != null ? int.Parse(checkValue) : INetworkTopicConnectionConfig.DefaultReceiveBufferSize;
        }
        set => this[nameof(ReceiveBufferSize)] = value.ToString();
    }

    public int SendBufferSize
    {
        get
        {
            var checkValue = this[nameof(SendBufferSize)];
            return checkValue != null ? int.Parse(checkValue) : INetworkTopicConnectionConfig.DefaultSendBufferSize;
        }
        set => this[nameof(SendBufferSize)] = value.ToString();
    }

    public int NumberOfReceivesPerPoll
    {
        get
        {
            var checkValue = this[nameof(NumberOfReceivesPerPoll)];
            return checkValue != null ? int.Parse(checkValue) : INetworkTopicConnectionConfig.DefaultNumberOfReceivesPerPoll;
        }
        set => this[nameof(NumberOfReceivesPerPoll)] = value.ToString();
    }

    public ConnectionSelectionOrder ConnectionSelectionOrder
    {
        get
        {
            var checkValue = this[nameof(ConnectionSelectionOrder)];
            return checkValue != null
                ? Enum.Parse<ConnectionSelectionOrder>(checkValue)
                : INetworkTopicConnectionConfig.DefaultConnectionSelectionOrder;
        }
        set => this[nameof(ConnectionSelectionOrder)] = value.ToString();
    }

    public uint ConnectionTimeoutMs
    {
        get
        {
            var checkValue = this[nameof(ConnectionTimeoutMs)];
            return checkValue != null ? uint.Parse(checkValue) : INetworkTopicConnectionConfig.DefaultConnectionTimeoutMs;
        }
        set => this[nameof(ConnectionTimeoutMs)] = value.ToString();
    }

    public uint ResponseTimeoutMs
    {
        get
        {
            var checkValue = this[nameof(ResponseTimeoutMs)];
            return checkValue != null ? uint.Parse(checkValue) : INetworkTopicConnectionConfig.DefaultResponseTimeoutMs;
        }
        set => this[nameof(ResponseTimeoutMs)] = value.ToString();
    }

    public ISocketReconnectConfig ReconnectConfig
    {
        get => new SocketReconnectConfig(ConfigRoot, $"{Path}{Split}{nameof(ReconnectConfig)}");
        set => _ = new SocketReconnectConfig(value, ConfigRoot, $"{Path}{Split}{nameof(ReconnectConfig)}");
    }

    public IConnectedEndpoint? ConnectedEndpoint { get; set; }

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
        oppositeConnectionConfig.ConversationProtocol =
            oppositeConnectionConfig.ConversationProtocol switch
            {
                SocketConversationProtocol.TcpAcceptor   => SocketConversationProtocol.TcpClient
              , SocketConversationProtocol.TcpClient     => SocketConversationProtocol.TcpAcceptor
              , SocketConversationProtocol.UdpPublisher  => SocketConversationProtocol.UdpSubscriber
              , SocketConversationProtocol.UdpSubscriber => SocketConversationProtocol.UdpPublisher
              , _ => throw new
                    NotSupportedException("Did not expect to receive SocketConversationProtocol unknown")
            };
        return oppositeConnectionConfig;
    }

    public INetworkTopicConnectionConfig Clone() => new NetworkTopicConnectionConfig(this);


    public static void ClearValues(IConfigurationRoot root, string path)
    {
        root[$"{path}{Split}{nameof(ConnectionName)}"]          = null;
        root[$"{path}{Split}{nameof(TopicName)}"]               = null;
        root[$"{path}{Split}{nameof(ConversationProtocol)}"]    = null;
        root[$"{path}{Split}{nameof(AvailableConnections)}"]    = null;
        root[$"{path}{Split}{nameof(TopicDescription)}"]        = null;
        root[$"{path}{Split}{nameof(ReceiveBufferSize)}"]       = null;
        root[$"{path}{Split}{nameof(SendBufferSize)}"]          = null;
        root[$"{path}{Split}{nameof(ConnectionAttributes)}"]    = null;
        root[$"{path}{Split}{nameof(NumberOfReceivesPerPoll)}"] = null;
        root[$"{path}{Split}{nameof(ConnectionTimeoutMs)}"]     = null;
        root[$"{path}{Split}{nameof(ResponseTimeoutMs)}"]       = null;
        root[$"{path}{Split}{nameof(ReconnectConfig)}"]         = null;
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
        var connectionAttribsSame        = ConnectionAttributes == other.ConnectionAttributes;
        var connectionSelectOrderSame = ConnectionSelectionOrder == other.ConnectionSelectionOrder;
        var connTimeoutMsSame         = ConnectionTimeoutMs == other.ConnectionTimeoutMs;
        var responseTimeoutMsSame     = ResponseTimeoutMs == other.ResponseTimeoutMs;
        var reconnectConfigSame       = ReconnectConfig.Equals(other.ReconnectConfig);

        return connectionNameSame && topicNameSame && conversationProtocolSame && availableConnectionsSame
            && topicDescriptionSame && receivedBufferSizeSame
            && sendBufferSizeSame && numReceivesPerPollSame && connectionAttribsSame && connectionSelectOrderSame
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
        $"{nameof(NetworkTopicConnectionConfig)}({nameof(ConnectionName)}: {ConnectionName}, {nameof(TopicName)}: {TopicName}, " +
        $"{nameof(ConversationProtocol)}: {ConversationProtocol}, {nameof(AvailableConnections)}: {AvailableConnections}, " +
        $"{nameof(TopicDescription)}: {TopicDescription}, {nameof(ReceiveBufferSize)}: {ReceiveBufferSize}, {nameof(SendBufferSize)}: {SendBufferSize}, " +
        $"{nameof(NumberOfReceivesPerPoll)}: {NumberOfReceivesPerPoll}, {nameof(ConnectionAttributes)}: {ConnectionAttributes}, " +
        $"{nameof(ConnectionSelectionOrder)}: {ConnectionSelectionOrder}, {nameof(ConnectionTimeoutMs)}: {ConnectionTimeoutMs}, " +
        $"{nameof(ResponseTimeoutMs)}: {ResponseTimeoutMs}, {nameof(ReconnectConfig)}: {ReconnectConfig})";
}
