#region

using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeIO.Transports.Network.Config;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeMarkets.Config.PricingConfig;

public interface IPricingServerConfig : IInterfacesComparable<IPricingServerConfig>, IConnection, IStringBearer
{
    public const int DefaultHeartBeatPublishIntervalMs      = 1_000;
    public const int DefaultHeartBeatServerToleranceRangeMs = 250;
    public const int DefaultMaxMissedHeartBeats             = 2;
    public const int DefaultSyncRetryIntervalMs             = 2_000;

    INetworkTopicConnectionConfig SnapshotConnectionConfig { get; }
    INetworkTopicConnectionConfig UpdateConnectionConfig   { get; }

    bool IsPricePublisher { get; }

    int HeartBeatPublishIntervalMs      { get; set; }
    int HeartBeatServerToleranceRangeMs { get; set; }

    int  MaxMissedHeartBeats { get; set; }
    bool IsLastLook          { get; set; }
    bool SupportsIceBergs    { get; set; }
    uint SyncRetryIntervalMs { get; set; }
    bool AllowUpdatesCatchup { get; set; }

    IPricingServerConfig ToggleProtocolDirection();
    IPricingServerConfig ShiftPortsBy(ushort deltaPorts);
}

public class PricingServerConfig : ConfigSection, IPricingServerConfig
{
    private const string SnapshotConnectionNameSuffix = "Snapshot";
    private const string UpdatesConnectionNameSuffix  = "Updates";

    public PricingServerConfig(IConfigurationRoot root, string path) : base(root, path) { }
    public PricingServerConfig() { }

    public PricingServerConfig
    (INetworkTopicConnectionConfig snapshotServerNetConnConfig, INetworkTopicConnectionConfig updateServerNetConnConfig,
        int heartBeatPublishIntervalMs = 1_000, int heartBeatServerToleranceRangeMs = 250, int maxMissedHeartBeats = 1,
        bool isLastLook = true, bool supportsIceBergs = false, uint syncRetryIntervalMs = 2_000, bool allowUpdatesCatchup = true)
    {
        IsLastLook                      = isLastLook;
        SupportsIceBergs                = supportsIceBergs;
        SnapshotConnectionConfig        = snapshotServerNetConnConfig;
        UpdateConnectionConfig          = updateServerNetConnConfig;
        HeartBeatPublishIntervalMs      = heartBeatPublishIntervalMs;
        HeartBeatServerToleranceRangeMs = heartBeatServerToleranceRangeMs;
        MaxMissedHeartBeats             = maxMissedHeartBeats;
        SyncRetryIntervalMs             = syncRetryIntervalMs;
        AllowUpdatesCatchup             = allowUpdatesCatchup;
    }

    public PricingServerConfig(IPricingServerConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        if (toClone is PricingServerConfig pricingServerConfig)
        {
            ConnectionName       = pricingServerConfig[nameof(ConnectionName)];
            ParentConnectionName = pricingServerConfig.ParentConnectionName;
        }
        else
        {
            ConnectionName = toClone.ConnectionName;
        }
        SnapshotConnectionConfig        = toClone.SnapshotConnectionConfig.Clone();
        UpdateConnectionConfig          = toClone.UpdateConnectionConfig.Clone();
        HeartBeatPublishIntervalMs      = toClone.HeartBeatPublishIntervalMs;
        HeartBeatServerToleranceRangeMs = toClone.HeartBeatServerToleranceRangeMs;
        MaxMissedHeartBeats             = toClone.MaxMissedHeartBeats;
        IsLastLook                      = toClone.IsLastLook;
        SupportsIceBergs                = toClone.SupportsIceBergs;
        SyncRetryIntervalMs             = toClone.SyncRetryIntervalMs;
        AllowUpdatesCatchup             = toClone.AllowUpdatesCatchup;
    }

    public PricingServerConfig(IPricingServerConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public INetworkTopicConnectionConfig SnapshotConnectionConfig
    {
        get
        {
            var snapshotConn = new NetworkTopicConnectionConfig(ConfigRoot, $"{Path}{Split}{nameof(SnapshotConnectionConfig)}")
            {
                ParentConnectionName = ConnectionName + SnapshotConnectionNameSuffix
            };
            return snapshotConn;
        }
        set
        {
            if (value.ConversationProtocol == SocketConversationProtocol.Unknown)
            {
                value.ConversationProtocol = SocketConversationProtocol.TcpAcceptor;
            }
            if (value is NetworkTopicConnectionConfig networkTopicConnectionConfig)
            {
                networkTopicConnectionConfig.ParentConnectionName = ConnectionName + SnapshotConnectionNameSuffix;
            }
            else if (value.ConnectionName.IsNullOrEmpty())
            {
                value.ConnectionName = ConnectionName + SnapshotConnectionNameSuffix;
            }
            _ = new NetworkTopicConnectionConfig(value, ConfigRoot, $"{Path}{Split}{nameof(SnapshotConnectionConfig)}");
        }
    }

    public INetworkTopicConnectionConfig UpdateConnectionConfig
    {
        get
        {
            var updatesConn = new NetworkTopicConnectionConfig(ConfigRoot, $"{Path}{Split}{nameof(UpdateConnectionConfig)}")
            {
                ParentConnectionName = ConnectionName + UpdatesConnectionNameSuffix
            };
            return updatesConn;
        }
        set
        {
            if (value.ConversationProtocol == SocketConversationProtocol.Unknown)
            {
                value.ConversationProtocol = SocketConversationProtocol.UdpPublisher;
            }
            if (value is NetworkTopicConnectionConfig networkTopicConnectionConfig)
            {
                networkTopicConnectionConfig.ParentConnectionName = ConnectionName + UpdatesConnectionNameSuffix;
            }
            else if (value.ConnectionName.IsNullOrEmpty())
            {
                value.ConnectionName = ConnectionName + UpdatesConnectionNameSuffix;
            }
            _ = new NetworkTopicConnectionConfig(value, ConfigRoot, $"{Path}{Split}{nameof(UpdateConnectionConfig)}");
        }
    }

    public bool IsPricePublisher =>
        SnapshotConnectionConfig.ConversationProtocol == SocketConversationProtocol.TcpAcceptor
     || UpdateConnectionConfig.ConversationProtocol == SocketConversationProtocol.UdpPublisher;

    public string? ConnectionName
    {
        get => this[nameof(ConnectionName)] ?? ParentConnectionName;
        set
        {
            if (value == ConnectionName) return;
            this[nameof(ConnectionName)] = value;
            var snapshotConnectionConfig = SnapshotConnectionConfig;
            if (snapshotConnectionConfig is NetworkTopicConnectionConfig snapNetworkConn)
            {
                snapNetworkConn.ParentConnectionName = value + SnapshotConnectionNameSuffix;
            }
            else if (snapshotConnectionConfig.ConnectionName.IsNullOrEmpty())
            {
                snapshotConnectionConfig.ConnectionName = value + SnapshotConnectionNameSuffix;
            }
            var updateConnectionConfig = UpdateConnectionConfig;
            if (updateConnectionConfig is NetworkTopicConnectionConfig updateNetworkConn)
            {
                updateNetworkConn.ParentConnectionName = value + UpdatesConnectionNameSuffix;
            }
            else if (updateConnectionConfig.ConnectionName.IsNullOrEmpty())
            {
                updateConnectionConfig.ConnectionName = value + UpdatesConnectionNameSuffix;
            }
        }
    }
    public string? ParentConnectionName { get; set; }

    public int HeartBeatPublishIntervalMs
    {
        get
        {
            var checkValue = this[nameof(HeartBeatPublishIntervalMs)];
            return checkValue.IsNotNullOrEmpty() ? int.Parse(checkValue) : IPricingServerConfig.DefaultHeartBeatPublishIntervalMs;
        }
        set => this[nameof(HeartBeatPublishIntervalMs)] = value.ToString();
    }

    public int HeartBeatServerToleranceRangeMs
    {
        get
        {
            var checkValue = this[nameof(HeartBeatServerToleranceRangeMs)];
            return checkValue.IsNotNullOrEmpty() ? int.Parse(checkValue) : IPricingServerConfig.DefaultHeartBeatServerToleranceRangeMs;
        }
        set => this[nameof(HeartBeatServerToleranceRangeMs)] = value.ToString();
    }

    public int MaxMissedHeartBeats
    {
        get
        {
            var checkValue = this[nameof(MaxMissedHeartBeats)];
            return checkValue.IsNotNullOrEmpty() ? int.Parse(checkValue) : IPricingServerConfig.DefaultMaxMissedHeartBeats;
        }
        set => this[nameof(MaxMissedHeartBeats)] = value.ToString();
    }

    public bool IsLastLook
    {
        get
        {
            var checkValue = this[nameof(IsLastLook)];
            return checkValue.IsNotNullOrEmpty() || bool.Parse(checkValue!);
        }
        set => this[nameof(IsLastLook)] = value.ToString();
    }

    public bool SupportsIceBergs
    {
        get
        {
            var checkValue = this[nameof(SupportsIceBergs)];
            return checkValue.IsNotNullOrEmpty() && bool.Parse(checkValue);
        }
        set => this[nameof(SupportsIceBergs)] = value.ToString();
    }

    public uint SyncRetryIntervalMs
    {
        get
        {
            var checkValue = this[nameof(SyncRetryIntervalMs)];
            return checkValue.IsNotNullOrEmpty() ? uint.Parse(checkValue) : IPricingServerConfig.DefaultSyncRetryIntervalMs;
        }
        set => this[nameof(SyncRetryIntervalMs)] = value.ToString();
    }

    public bool AllowUpdatesCatchup
    {
        get
        {
            var checkValue = this[nameof(AllowUpdatesCatchup)];
            return checkValue.IsNotNullOrEmpty() && bool.Parse(checkValue);
        }
        set => this[nameof(AllowUpdatesCatchup)] = value.ToString();
    }

    public IPricingServerConfig ShiftPortsBy(ushort deltaPorts)
    {
        var shiftedPricingServerConfig = new PricingServerConfig(this)
        {
            SnapshotConnectionConfig = SnapshotConnectionConfig.ShiftPortsBy(deltaPorts)
          , UpdateConnectionConfig   = UpdateConnectionConfig.ShiftPortsBy(deltaPorts)
        };
        return shiftedPricingServerConfig;
    }

    public IPricingServerConfig ToggleProtocolDirection() =>
        new PricingServerConfig(this)
        {
            SnapshotConnectionConfig = SnapshotConnectionConfig.ToggleProtocolDirection()
          , UpdateConnectionConfig   = UpdateConnectionConfig.ToggleProtocolDirection()
        };


    public bool AreEquivalent(IPricingServerConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;

        var connectionNameSame            = ConnectionName == other.ConnectionName;
        var snapshotServerConConfigSame   = Equals(SnapshotConnectionConfig, other.SnapshotConnectionConfig);
        var updateServerConConfigSame     = Equals(UpdateConnectionConfig, other.UpdateConnectionConfig);
        var clientHeartBeatsIntervalSame  = HeartBeatPublishIntervalMs == other.HeartBeatPublishIntervalMs;
        var serverHeartBeatsToleranceSame = HeartBeatServerToleranceRangeMs == other.HeartBeatServerToleranceRangeMs;
        var maxMissedHeartbeatSame        = MaxMissedHeartBeats == other.MaxMissedHeartBeats;
        var isLastLookSame                = IsLastLook == other.IsLastLook;
        var supportsIceBergsSame          = SupportsIceBergs == other.SupportsIceBergs;
        var syncRetryIntervalSame         = SyncRetryIntervalMs == other.SyncRetryIntervalMs;
        var syncAllowUpdatesCatchupSame   = AllowUpdatesCatchup == other.AllowUpdatesCatchup;

        return connectionNameSame && snapshotServerConConfigSame && updateServerConConfigSame && clientHeartBeatsIntervalSame
            && serverHeartBeatsToleranceSame && maxMissedHeartbeatSame && isLastLookSame && supportsIceBergsSame && syncRetryIntervalSame
            && syncAllowUpdatesCatchupSame;
    }

    public IPricingServerConfig Clone() => new PricingServerConfig(this);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IPricingServerConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = SnapshotConnectionConfig.GetHashCode();
            hashCode = (hashCode * 397) ^ UpdateConnectionConfig.GetHashCode();
            hashCode = (hashCode * 397) ^ ConnectionName?.GetHashCode() ?? 0;
            hashCode = (hashCode * 397) ^ HeartBeatPublishIntervalMs.GetHashCode();
            hashCode = (hashCode * 397) ^ HeartBeatServerToleranceRangeMs.GetHashCode();
            hashCode = (hashCode * 397) ^ MaxMissedHeartBeats.GetHashCode();
            hashCode = (hashCode * 397) ^ IsLastLook.GetHashCode();
            hashCode = (hashCode * 397) ^ SupportsIceBergs.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)SyncRetryIntervalMs;
            hashCode = (hashCode * 397) ^ AllowUpdatesCatchup.GetHashCode();
            return hashCode;
        }
    }

    public virtual AppendSummary RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
            .Field.AlwaysAdd(nameof(ConnectionName), ConnectionName)
            .Field.AlwaysReveal(nameof(SnapshotConnectionConfig), SnapshotConnectionConfig)
            .Field.AlwaysReveal(nameof(UpdateConnectionConfig), UpdateConnectionConfig)
            .Field.AlwaysAdd(nameof(HeartBeatPublishIntervalMs), HeartBeatPublishIntervalMs)
            .Field.AlwaysAdd(nameof(HeartBeatServerToleranceRangeMs), HeartBeatServerToleranceRangeMs)
            .Field.AlwaysAdd(nameof(MaxMissedHeartBeats), MaxMissedHeartBeats)
            .Field.AlwaysAdd(nameof(IsLastLook), IsLastLook)
            .Field.AlwaysAdd(nameof(SupportsIceBergs), SupportsIceBergs)
            .Field.AlwaysAdd(nameof(SyncRetryIntervalMs), SyncRetryIntervalMs)
            .Field.AlwaysAdd(nameof(AllowUpdatesCatchup), AllowUpdatesCatchup)
            .Complete();

    public override string ToString() =>
        $"{nameof(PricingServerConfig)}({nameof(ConnectionName)}: {ConnectionName}, {nameof(SnapshotConnectionConfig)}: {SnapshotConnectionConfig}, " +
        $"{nameof(UpdateConnectionConfig)}: {UpdateConnectionConfig}, {nameof(HeartBeatPublishIntervalMs)}: {HeartBeatPublishIntervalMs},  " +
        $"{nameof(HeartBeatServerToleranceRangeMs)}: {HeartBeatServerToleranceRangeMs}, {nameof(MaxMissedHeartBeats)}: {MaxMissedHeartBeats}, " +
        $"{nameof(IsLastLook)}: {IsLastLook}, {nameof(SupportsIceBergs)}: {SupportsIceBergs}, {nameof(SyncRetryIntervalMs)}: {SyncRetryIntervalMs}, " +
        $"{nameof(AllowUpdatesCatchup)}: {AllowUpdatesCatchup}, {nameof(Path)}: {Path}";
}
