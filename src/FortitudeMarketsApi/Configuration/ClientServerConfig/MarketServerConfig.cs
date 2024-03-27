#region

using System.Reactive.Linq;
using System.Reactive.Subjects;
using FortitudeCommon.Configuration.Availability;
using FortitudeCommon.EventProcessing;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeIO.Transports.NewSocketAPI.Config;

#endregion

namespace FortitudeMarketsApi.Configuration.ClientServerConfig;

public class MarketServerConfig<T> : IMarketServerConfig<T> where T : class, IMarketServerConfig<T>
{
    private readonly IObservable<IMarketServerConfigUpdate<T>>? repoUpdateStream;
    private ITimeTable? availabilityTimeTable;
    protected ISubject<IConnectionUpdate> ConnectionUpdateStream;
    private MarketServerType marketServerType;
    private string? name;
    private IEnumerable<INetworkTopicConnectionConfig>? serverConnections;
    protected ISubject<IMarketServerConfigUpdate<T>>? UpdateStream;
    private IDisposable? updateStreamSub;

    public MarketServerConfig(string name, MarketServerType marketServerType
        , IEnumerable<INetworkTopicConnectionConfig> serverConnections, ITimeTable? availabilityTimeTable = null,
        IObservable<IMarketServerConfigUpdate<T>>? repoUpdateStream = null)
    {
        Id = IdGenLong.Next();
        ConnectionUpdateStream = new Subject<IConnectionUpdate>();
        this.name = name;
        this.marketServerType = marketServerType;
        this.serverConnections = serverConnections;
        this.availabilityTimeTable = availabilityTimeTable;
        this.repoUpdateStream = repoUpdateStream;
        UpdateStream = new Subject<IMarketServerConfigUpdate<T>>();
        updateStreamSub = repoUpdateStream?.Subscribe(UpdateStream);
        UpdateStream.Subscribe(ListenForUpdates);
    }

    public MarketServerConfig(MarketServerConfig<T> toClone, bool toggleProtocolDirection = false)
    {
        Id = toClone.Id;
        ConnectionUpdateStream = new Subject<IConnectionUpdate>();
        name = toClone.Name;
        marketServerType = toClone.MarketServerType;
        serverConnections = toClone.ServerConnections?
                                .Select(sc => toggleProtocolDirection ? sc.ToggleProtocolDirection() : sc.Clone())
                                .ToArray()
                            ?? Array.Empty<INetworkTopicConnectionConfig>();
        availabilityTimeTable = toClone.AvailabilityTimeTable;
        repoUpdateStream = toClone.repoUpdateStream;
        UpdateStream = new Subject<IMarketServerConfigUpdate<T>>();
        updateStreamSub = repoUpdateStream?.Subscribe(UpdateStream);
        UpdateStream.Subscribe(ListenForUpdates);
    }

    public long Id { get; }

    public string? Name
    {
        get => name;
        protected set
        {
            if (name == value) return;
            name = value;
            UpdateStream?.OnNext(new MarketServerConfigUpdate<T>(this as T, EventType.Updated));
        }
    }

    public MarketServerType MarketServerType
    {
        get => marketServerType;
        protected set
        {
            if (marketServerType == value) return;
            marketServerType = value;
            UpdateStream?.OnNext(new MarketServerConfigUpdate<T>(this as T, EventType.Updated));
        }
    }

    public IEnumerable<INetworkTopicConnectionConfig>? ServerConnections
    {
        get => serverConnections;
        protected set
        {
            if (Equals(serverConnections, value)) return;
            serverConnections = value;
            UpdateStream?.OnNext(new MarketServerConfigUpdate<T>(this as T, EventType.Updated));
        }
    }

    public ITimeTable? AvailabilityTimeTable
    {
        get => availabilityTimeTable;
        protected set
        {
            if (availabilityTimeTable == value) return;
            availabilityTimeTable = value;
            UpdateStream?.OnNext(new MarketServerConfigUpdate<T>(this as T, EventType.Updated));
        }
    }

    public IObservable<IMarketServerConfigUpdate<T>>? Updates
    {
        get => UpdateStream?.AsObservable();
        set
        {
            if (ReferenceEquals(value, UpdateStream)) return;
            updateStreamSub?.Dispose();
            if (value != null) updateStreamSub = value.Subscribe(UpdateStream!);
        }
    }

    object ICloneable.Clone() => Clone();

    public IMarketServerConfig<T> Clone() => new MarketServerConfig<T>(this, false);

    IMarketServerConfig IMarketServerConfig.ToggleProtocolDirection() => ToggleProtocolDirection();

    public virtual T ToggleProtocolDirection() => (T)(object)new MarketServerConfig<T>(this, true);

    private void ListenForUpdates(IMarketServerConfigUpdate<T> scu)
    {
        if (scu.ServerConfig?.Id != Id) return;
        if (scu.EventType == EventType.Updated ||
            scu.EventType == EventType.Retrieved)
        {
            name = scu.ServerConfig.Name;
            marketServerType = scu.ServerConfig.MarketServerType;
            UpdateConnectionsList(scu.ServerConfig.ServerConnections);
            availabilityTimeTable = scu.ServerConfig.AvailabilityTimeTable;
            UpdateFields(scu.ServerConfig);
        }
        else if (scu.EventType == EventType.Deleted)
        {
            updateStreamSub?.Dispose();
        }

        if (ReferenceEquals(this, scu.ServerConfig)) return;
        UpdateStream?.OnNext(new MarketServerConfigUpdate<T>(this as T, scu.EventType));
    }

    protected virtual void UpdateFields(T updatedServerConfig) { }

    private void UpdateConnectionsList(IEnumerable<INetworkTopicConnectionConfig>? newConnectionsList)
    {
        var originalServerConnection = serverConnections;
        serverConnections = newConnectionsList?.Select(scc =>
        {
            var clonedScc = scc.Clone();
            return clonedScc;
        })?.ToList();
        var diffResults = originalServerConnection?.Diff(serverConnections, scc => scc.TopicName)
                          ?? new DiffResults<INetworkTopicConnectionConfig>(Array.Empty<INetworkTopicConnectionConfig>()
                              ,
                              Array.Empty<INetworkTopicConnectionConfig>()
                              , Array.Empty<INetworkTopicConnectionConfig>());
        foreach (var scc in diffResults.NewItems)
            ConnectionUpdateStream.OnNext(new ConnectionUpdate(scc, EventType.Created));
        foreach (var scc in diffResults.UpdatedItems)
            ConnectionUpdateStream.OnNext(new ConnectionUpdate(scc, EventType.Updated));
        foreach (var scc in diffResults.DeletedItems)
            ConnectionUpdateStream.OnNext(new ConnectionUpdate(scc, EventType.Deleted));
    }

    protected bool Equals(MarketServerConfig<T> other) =>
        string.Equals(name, other.name) && marketServerType == other.marketServerType
                                        && serverConnections != null && other.ServerConnections != null &&
                                        serverConnections.SequenceEqual(other.ServerConnections)
                                        && Equals(availabilityTimeTable, other.availabilityTimeTable) && Id == other.Id;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((MarketServerConfig<T>)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = name != null ? name.GetHashCode() : 0;
            hashCode = (hashCode * 397) ^ (int)marketServerType;
            hashCode = (hashCode * 397) ^ (serverConnections != null ? serverConnections.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (availabilityTimeTable != null ? availabilityTimeTable.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ Id.GetHashCode();
            return hashCode;
        }
    }
}
