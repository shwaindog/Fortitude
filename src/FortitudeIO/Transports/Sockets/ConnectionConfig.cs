#region

using System.Reactive.Linq;
using System.Reactive.Subjects;
using FortitudeCommon.EventProcessing;
using FortitudeCommon.Types;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Transports.Sockets;

public interface IConnectionConfig : ICloneable<IConnectionConfig>
{
    long Id { get; }
    string ConnectionName { get; }
    string Hostname { get; }
    int Port { get; }
    string? NetworkSubAddress { get; }
    ConnectionDirectionType ConnectionDirectionType { get; }
    IObservable<IConnectionUpdate> Updates { get; set; }
    uint ReconnectIntervalMs { get; }
    IConnectionConfig? FallBackConnectionConfig { get; }
    ISocketTopicConnectionConfig ToSocketTopicConnectionConfig();
}

public class ConnectionConfig : IConnectionConfig
{
    private readonly IObservable<IConnectionUpdate>? repoUpdateStream;
    private readonly Subject<IConnectionUpdate> updateSubject;
    private ConnectionDirectionType connectionDirectionType;
    private string connectionName;
    private IConnectionConfig? fallbackConnectionConfig;
    private string hostname;
    private IDisposable? isSubscribedExternally;
    private string? networkSubAddress;
    private int port;
    private uint reconnectIntervalMs;

    public ConnectionConfig(string connectionName, string hostname, int port
        , ConnectionDirectionType connectionDirectionType,
        string? networkSubAddress, uint reconnectIntervalMs, IObservable<IConnectionUpdate>? repoUpdateStream = null)
    {
        this.connectionName = connectionName;
        this.hostname = hostname;
        this.port = port;
        this.connectionDirectionType = connectionDirectionType;
        this.networkSubAddress = networkSubAddress;
        this.reconnectIntervalMs = reconnectIntervalMs;
        this.repoUpdateStream = repoUpdateStream;
        updateSubject = new Subject<IConnectionUpdate>();
        updateSubject.Subscribe(ListenToConnectionConfigUpdates);
        isSubscribedExternally = repoUpdateStream?.Subscribe(updateSubject);
        Id = connectionName.GetHashCode() << (32 + GetHashCode());
    }

    public ConnectionConfig(ConnectionConfig toClone)
    {
        Id = toClone.Id;
        connectionName = toClone.ConnectionName;
        hostname = toClone.Hostname;
        port = toClone.Port;
        connectionDirectionType = toClone.ConnectionDirectionType;
        networkSubAddress = toClone.NetworkSubAddress;
        reconnectIntervalMs = toClone.ReconnectIntervalMs;
        repoUpdateStream = toClone.repoUpdateStream;
        updateSubject = new Subject<IConnectionUpdate>();
        updateSubject.Subscribe(ListenToConnectionConfigUpdates);
        isSubscribedExternally = repoUpdateStream?.Subscribe(updateSubject);
    }

    public ISocketTopicConnectionConfig ToSocketTopicConnectionConfig() =>
        new SocketTopicConnectionConfig(connectionName, SocketConversationProtocol.TcpClient
            , new[] { new SocketConnectionConfig(hostname, (ushort)port) }, connectionName, 2_000_000
            , 2_000_000);

    public long Id { get; protected set; }

    public string ConnectionName
    {
        get => connectionName;
        protected set
        {
            if (connectionName == value) return;
            connectionName = value;
            updateSubject.OnNext(new ConnectionUpdate(ToSocketTopicConnectionConfig(), EventType.Updated));
        }
    }

    public string Hostname
    {
        get => hostname;
        protected set
        {
            if (hostname == value) return;
            hostname = value;
            updateSubject.OnNext(new ConnectionUpdate(ToSocketTopicConnectionConfig(), EventType.Updated));
        }
    }

    public int Port
    {
        get => port;
        protected set
        {
            if (port == value) return;
            port = value;
            updateSubject.OnNext(new ConnectionUpdate(ToSocketTopicConnectionConfig(), EventType.Updated));
        }
    }

    public string? NetworkSubAddress
    {
        get => networkSubAddress;
        protected set
        {
            if (networkSubAddress == value) return;
            networkSubAddress = value;
            updateSubject.OnNext(new ConnectionUpdate(ToSocketTopicConnectionConfig(), EventType.Updated));
        }
    }

    public ConnectionDirectionType ConnectionDirectionType
    {
        get => connectionDirectionType;
        protected set
        {
            if (connectionDirectionType == value) return;
            connectionDirectionType = value;
            updateSubject.OnNext(new ConnectionUpdate(ToSocketTopicConnectionConfig(), EventType.Updated));
        }
    }

    public uint ReconnectIntervalMs
    {
        get => reconnectIntervalMs;
        protected set
        {
            if (reconnectIntervalMs == value) return;
            reconnectIntervalMs = value;
            updateSubject.OnNext(new ConnectionUpdate(ToSocketTopicConnectionConfig(), EventType.Updated));
        }
    }

    public IConnectionConfig? FallBackConnectionConfig
    {
        get => fallbackConnectionConfig;
        set
        {
            if (fallbackConnectionConfig == value) return;
            fallbackConnectionConfig = value;
            updateSubject.OnNext(new ConnectionUpdate(ToSocketTopicConnectionConfig(), EventType.Updated));
        }
    }

    public IObservable<IConnectionUpdate> Updates
    {
        get => updateSubject.AsObservable();
        set
        {
            if (ReferenceEquals(value, updateSubject)) return;
            if (isSubscribedExternally != null)
            {
                isSubscribedExternally.Dispose();
                isSubscribedExternally = null;
            }

            isSubscribedExternally = value.Subscribe(updateSubject);
        }
    }

    object ICloneable.Clone() => Clone();

    public IConnectionConfig Clone() => new ConnectionConfig(this);

    private void ListenToConnectionConfigUpdates(IConnectionUpdate scu)
    {
        if (scu.EventType == EventType.Created
            || scu.EventType == EventType.Retrieved
            || scu.EventType == EventType.Updated)
            if (scu.ConnectionConfig.TopicName == ConnectionName)
            {
                connectionName = scu.ConnectionConfig.TopicName;
                hostname = scu.ConnectionConfig.Current.Hostname;
                port = scu.ConnectionConfig.Current.Port;
                connectionDirectionType = ConnectionDirectionType.Both;
                networkSubAddress = scu.ConnectionConfig.Current.SubnetMask;
                reconnectIntervalMs = 1_000;
            }
    }

    protected bool Equals(ConnectionConfig other) =>
        string.Equals(connectionName, other.connectionName)
        && string.Equals(hostname, other.hostname) && port == other.port
        && string.Equals(networkSubAddress, other.networkSubAddress)
        && connectionDirectionType == other.connectionDirectionType
        && reconnectIntervalMs == other.reconnectIntervalMs
        && Equals(fallbackConnectionConfig, other.fallbackConnectionConfig)
        && Id == other.Id;

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((ConnectionConfig)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = connectionName.GetHashCode();
            hashCode = (hashCode * 397) ^ hostname.GetHashCode();
            hashCode = (hashCode * 397) ^ port;
            hashCode = (hashCode * 397) ^ networkSubAddress?.GetHashCode() ?? 0;
            hashCode = (hashCode * 397) ^ (int)connectionDirectionType;
            hashCode = (hashCode * 397) ^ (int)reconnectIntervalMs;
            hashCode = (hashCode * 397) ^
                       (fallbackConnectionConfig != null ? fallbackConnectionConfig.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ Id.GetHashCode();
            return hashCode;
        }
    }
}
