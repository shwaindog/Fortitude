#region

using FortitudeCommon.Configuration.Availability;
using FortitudeCommon.EventProcessing;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

#endregion

namespace FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig;

public class SnapshotUpdatePricingServerConfig : MarketServerConfig<ISnapshotUpdatePricingServerConfig>,
    ISnapshotUpdatePricingServerConfig
{
    private bool isLastLook;
    private ushort publicationId;
    private INetworkTopicConnectionConfig? snapshotConnectionConfig;
    private IList<ISourceTickerPublicationConfig>? sourceTickerPublicationConfigs;
    private bool supportsIceBergs;
    private INetworkTopicConnectionConfig? updateConnectionConfig;

    public SnapshotUpdatePricingServerConfig(string name, MarketServerType marketServerType,
        IEnumerable<INetworkTopicConnectionConfig> serverConnections, ITimeTable? availabilityTimeTable,
        ushort publicationId, IEnumerable<ISourceTickerPublicationConfig> sourceTickerPublicationConfigs,
        bool isLastLook, bool supportsIceBergs,
        IObservable<IMarketServerConfigUpdate<ISnapshotUpdatePricingServerConfig>>? repoUpdateStream = null)
        : base(name, marketServerType, serverConnections, availabilityTimeTable, repoUpdateStream)
    {
        this.publicationId = publicationId;
        this.isLastLook = isLastLook;
        this.supportsIceBergs = supportsIceBergs;
        this.sourceTickerPublicationConfigs
            = sourceTickerPublicationConfigs?.ToList() ?? new List<ISourceTickerPublicationConfig>();
        snapshotConnectionConfig =
            serverConnections.FirstOrDefault(
                stcc => stcc.ConversationProtocol == SocketConversationProtocol.TcpAcceptor);
        updateConnectionConfig =
            serverConnections.FirstOrDefault(stcc =>
                stcc.ConversationProtocol == SocketConversationProtocol.UdpPublisher);
    }

    public SnapshotUpdatePricingServerConfig(SnapshotUpdatePricingServerConfig toClone, bool toggleProtocolDirection)
        : base(toClone, toggleProtocolDirection)
    {
        publicationId = toClone.publicationId;
        isLastLook = toClone.isLastLook;
        supportsIceBergs = toClone.supportsIceBergs;
        sourceTickerPublicationConfigs = toClone.sourceTickerPublicationConfigs?.ToList();
        snapshotConnectionConfig = toggleProtocolDirection ?
            toClone.SnapshotConnectionConfig?.ToggleProtocolDirection() :
            toClone.SnapshotConnectionConfig?.Clone();
        updateConnectionConfig = toggleProtocolDirection ?
            toClone.UpdateConnectionConfig?.ToggleProtocolDirection() :
            toClone.UpdateConnectionConfig?.Clone();
    }

    object ICloneable.Clone() => Clone();

    public new ISnapshotUpdatePricingServerConfig Clone() => new SnapshotUpdatePricingServerConfig(this, false);

    public override ISnapshotUpdatePricingServerConfig ToggleProtocolDirection() =>
        new SnapshotUpdatePricingServerConfig(this, true);

    public ushort PublicationId
    {
        get => publicationId;
        protected set
        {
            if (publicationId == value) return;
            publicationId = value;
            UpdateStream?.OnNext(new MarketServerConfigUpdate<ISnapshotUpdatePricingServerConfig>(this,
                EventType.Updated));
        }
    }

    public INetworkTopicConnectionConfig? SnapshotConnectionConfig
    {
        get => snapshotConnectionConfig;
        protected set
        {
            if (ReferenceEquals(snapshotConnectionConfig, value)) return;
            snapshotConnectionConfig = value;
            UpdateStream?.OnNext(new MarketServerConfigUpdate<ISnapshotUpdatePricingServerConfig>(this,
                EventType.Updated));
        }
    }

    public INetworkTopicConnectionConfig? UpdateConnectionConfig
    {
        get => updateConnectionConfig;
        protected set
        {
            if (ReferenceEquals(updateConnectionConfig, value)) return;
            updateConnectionConfig = value;
            UpdateStream?.OnNext(new MarketServerConfigUpdate<ISnapshotUpdatePricingServerConfig>(this,
                EventType.Updated));
        }
    }

    public IList<ISourceTickerPublicationConfig>? SourceTickerPublicationConfigs
    {
        get => sourceTickerPublicationConfigs;
        protected set
        {
            if (ReferenceEquals(sourceTickerPublicationConfigs, value)) return;
            sourceTickerPublicationConfigs = value;
            UpdateStream?.OnNext(new MarketServerConfigUpdate<ISnapshotUpdatePricingServerConfig>(this,
                EventType.Updated));
        }
    }

    public bool IsLastLook
    {
        get => isLastLook;
        protected set
        {
            if (isLastLook == value) return;
            isLastLook = value;
            UpdateStream?.OnNext(new MarketServerConfigUpdate<ISnapshotUpdatePricingServerConfig>(this,
                EventType.Updated));
        }
    }

    public bool SupportsIceBergs
    {
        get => supportsIceBergs;
        protected set
        {
            if (supportsIceBergs == value) return;
            supportsIceBergs = value;
            UpdateStream?.OnNext(new MarketServerConfigUpdate<ISnapshotUpdatePricingServerConfig>(this,
                EventType.Updated));
        }
    }

    public bool AreEquivalent(ISnapshotUpdatePricingServerConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;

        var pubIdSame = publicationId == other.PublicationId;
        var snapshotServerConConfigSame = Equals(snapshotConnectionConfig, other.SnapshotConnectionConfig);
        var updateServerConConfigSame = Equals(updateConnectionConfig, other.UpdateConnectionConfig);
        var itemsCountSame = sourceTickerPublicationConfigs?.Count == other.SourceTickerPublicationConfigs?.Count;
        var srcTkrPubConfigsSame = itemsCountSame &&
                                   ((other.SourceTickerPublicationConfigs != null &&
                                     sourceTickerPublicationConfigs != null)
                                    || (other.SourceTickerPublicationConfigs == null &&
                                        sourceTickerPublicationConfigs == null));
        if (srcTkrPubConfigsSame && other.SourceTickerPublicationConfigs != null)
            srcTkrPubConfigsSame = sourceTickerPublicationConfigs?
                .Zip(other.SourceTickerPublicationConfigs, (x, y) => x.AreEquivalent(y)).All(x => x) ?? false;
        var isLastLookSame = isLastLook == other.IsLastLook;
        var supportsIceBergsSame = supportsIceBergs == other.SupportsIceBergs;

        return pubIdSame && snapshotServerConConfigSame && updateServerConConfigSame && itemsCountSame &&
               srcTkrPubConfigsSame && isLastLookSame && supportsIceBergsSame;
    }

    protected override void UpdateFields(ISnapshotUpdatePricingServerConfig updatedMarketsServerConfig)
    {
        publicationId = updatedMarketsServerConfig.PublicationId;
        // previously have already updated ServerConnections from the updatedServerConfig;
        snapshotConnectionConfig =
            ServerConnections!.FirstOrDefault(stcc =>
                stcc.ConversationProtocol == SocketConversationProtocol.TcpAcceptor);
        updateConnectionConfig =
            ServerConnections!.FirstOrDefault(stcc =>
                stcc.ConversationProtocol == SocketConversationProtocol.UdpPublisher);
        sourceTickerPublicationConfigs =
            updatedMarketsServerConfig.SourceTickerPublicationConfigs?.Select(stpc => stpc.Clone())?.ToList();
        isLastLook = updatedMarketsServerConfig.IsLastLook;
        supportsIceBergs = updatedMarketsServerConfig.SupportsIceBergs;
    }

    public override bool Equals(object? obj) =>
        ReferenceEquals(this, obj) || AreEquivalent(obj as ISnapshotUpdatePricingServerConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = publicationId.GetHashCode();
            hashCode = (hashCode * 397) ^ (snapshotConnectionConfig?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ (updateConnectionConfig?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ (sourceTickerPublicationConfigs?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ isLastLook.GetHashCode();
            hashCode = (hashCode * 397) ^ supportsIceBergs.GetHashCode();
            return hashCode;
        }
    }
}
