#region

using FortitudeCommon.Configuration;
using FortitudeCommon.Types;
using FortitudeIO.Transports.Network.Config;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeMarkets.Configuration.ClientServerConfig.PricingConfig;

public interface IPricingServerConfig : IInterfacesComparable<IPricingServerConfig>, IConnection
{
    INetworkTopicConnectionConfig SnapshotConnectionConfig { get; }
    INetworkTopicConnectionConfig UpdateConnectionConfig { get; }

    int HeartBeatPublishIntervalMs { get; set; }
    int HeartBeatServerToleranceRangeMs { get; set; }
    int MaxMissedHeartBeats { get; set; }
    bool IsLastLook { get; set; }
    bool SupportsIceBergs { get; set; }
    uint SyncRetryIntervalMs { get; set; }
    bool AllowUpdatesCatchup { get; set; }
    IPricingServerConfig ToggleProtocolDirection();
    IPricingServerConfig ShiftPortsBy(ushort deltaPorts);
}

public class PricingServerConfig : ConfigSection, IPricingServerConfig
{
    private object? ignoreSuppressWarnings;

    public PricingServerConfig(IConfigurationRoot root, string path) : base(root, path) { }
    public PricingServerConfig() { }

    public PricingServerConfig(INetworkTopicConnectionConfig snapshotServerNetConnConfig, INetworkTopicConnectionConfig updateServerNetConnConfig,
        int heartBeatPublishIntervalMs = 1_000, int heartBeatServerToleranceRangeMs = 250, int maxMissedHeartBeats = 1,
        bool isLastLook = true, bool supportsIceBergs = false, uint syncRetryIntervalMs = 2_000, bool allowUpdatesCatchup = true)
    {
        IsLastLook = isLastLook;
        SupportsIceBergs = supportsIceBergs;
        SnapshotConnectionConfig = snapshotServerNetConnConfig;
        UpdateConnectionConfig = updateServerNetConnConfig;
        HeartBeatPublishIntervalMs = heartBeatPublishIntervalMs;
        HeartBeatServerToleranceRangeMs = heartBeatServerToleranceRangeMs;
        MaxMissedHeartBeats = maxMissedHeartBeats;
        SyncRetryIntervalMs = syncRetryIntervalMs;
        AllowUpdatesCatchup = allowUpdatesCatchup;
    }

    public PricingServerConfig(IPricingServerConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        SnapshotConnectionConfig = toClone.SnapshotConnectionConfig.Clone();
        UpdateConnectionConfig = toClone.UpdateConnectionConfig.Clone();
        ConnectionName = toClone.ConnectionName;
        HeartBeatPublishIntervalMs = toClone.HeartBeatPublishIntervalMs;
        HeartBeatServerToleranceRangeMs = toClone.HeartBeatServerToleranceRangeMs;
        MaxMissedHeartBeats = toClone.MaxMissedHeartBeats;
        IsLastLook = toClone.IsLastLook;
        SupportsIceBergs = toClone.SupportsIceBergs;
        SyncRetryIntervalMs = toClone.SyncRetryIntervalMs;
        AllowUpdatesCatchup = toClone.AllowUpdatesCatchup;
    }

    public PricingServerConfig(IPricingServerConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public INetworkTopicConnectionConfig SnapshotConnectionConfig
    {
        get => new NetworkTopicConnectionConfig(ConfigRoot, Path + ":" + nameof(SnapshotConnectionConfig));
        set =>
            ignoreSuppressWarnings = new NetworkTopicConnectionConfig(value, ConfigRoot, Path + ":" + nameof(SnapshotConnectionConfig))
            {
                ConnectionName = ConnectionName
            };
    }

    public INetworkTopicConnectionConfig UpdateConnectionConfig
    {
        get => new NetworkTopicConnectionConfig(ConfigRoot, Path + ":" + nameof(UpdateConnectionConfig));
        set =>
            ignoreSuppressWarnings = new NetworkTopicConnectionConfig(value, ConfigRoot, Path + ":" + nameof(UpdateConnectionConfig))
            {
                ConnectionName = ConnectionName
            };
    }

    public string? ConnectionName
    {
        get => this[nameof(ConnectionName)];
        set
        {
            if (value == ConnectionName) return;
            this[nameof(ConnectionName)] = value;
            SnapshotConnectionConfig.ConnectionName = value + "Snapshot";
            UpdateConnectionConfig.ConnectionName = value + "Updates";
        }
    }

    public int HeartBeatPublishIntervalMs
    {
        get
        {
            var checkValue = this[nameof(HeartBeatPublishIntervalMs)];
            return checkValue != null ? int.Parse(checkValue) : 1_000;
        }
        set => this[nameof(HeartBeatPublishIntervalMs)] = value.ToString();
    }

    public int HeartBeatServerToleranceRangeMs
    {
        get
        {
            var checkValue = this[nameof(HeartBeatServerToleranceRangeMs)];
            return checkValue != null ? int.Parse(checkValue) : 250;
        }
        set => this[nameof(HeartBeatServerToleranceRangeMs)] = value.ToString();
    }

    public int MaxMissedHeartBeats
    {
        get
        {
            var checkValue = this[nameof(MaxMissedHeartBeats)];
            return checkValue != null ? int.Parse(checkValue) : 2;
        }
        set => this[nameof(MaxMissedHeartBeats)] = value.ToString();
    }

    public bool IsLastLook
    {
        get
        {
            var checkValue = this[nameof(IsLastLook)];
            return checkValue == null || bool.Parse(checkValue);
        }
        set => this[nameof(IsLastLook)] = value.ToString();
    }

    public bool SupportsIceBergs
    {
        get
        {
            var checkValue = this[nameof(SupportsIceBergs)];
            return checkValue != null && bool.Parse(checkValue);
        }
        set => this[nameof(SupportsIceBergs)] = value.ToString();
    }

    public uint SyncRetryIntervalMs
    {
        get
        {
            var checkValue = this[nameof(SyncRetryIntervalMs)];
            return checkValue != null ? uint.Parse(checkValue) : 2_000;
        }
        set => this[nameof(SyncRetryIntervalMs)] = value.ToString();
    }

    public bool AllowUpdatesCatchup
    {
        get
        {
            var checkValue = this[nameof(AllowUpdatesCatchup)];
            return checkValue == null || bool.Parse(checkValue);
        }
        set => this[nameof(AllowUpdatesCatchup)] = value.ToString();
    }

    public IPricingServerConfig ShiftPortsBy(ushort deltaPorts)
    {
        var shiftedPricingServerConfig = new PricingServerConfig(this)
        {
            SnapshotConnectionConfig = SnapshotConnectionConfig.ShiftPortsBy(deltaPorts)
            , UpdateConnectionConfig = UpdateConnectionConfig.ShiftPortsBy(deltaPorts)
        };
        return shiftedPricingServerConfig;
    }

    public IPricingServerConfig ToggleProtocolDirection() =>
        new PricingServerConfig(this)
        {
            SnapshotConnectionConfig = SnapshotConnectionConfig.ToggleProtocolDirection()
            , UpdateConnectionConfig = UpdateConnectionConfig.ToggleProtocolDirection()
        };


    public bool AreEquivalent(IPricingServerConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;

        var connectionNameSame = ConnectionName == other.ConnectionName;
        var snapshotServerConConfigSame = Equals(SnapshotConnectionConfig, other.SnapshotConnectionConfig);
        var updateServerConConfigSame = Equals(UpdateConnectionConfig, other.UpdateConnectionConfig);
        var clientHeartBeatsIntervalSame = HeartBeatPublishIntervalMs == other.HeartBeatPublishIntervalMs;
        var serverHeartBeatsToleranceSame = HeartBeatServerToleranceRangeMs == other.HeartBeatServerToleranceRangeMs;
        var maxMissedHeartbeatSame = MaxMissedHeartBeats == other.MaxMissedHeartBeats;
        var isLastLookSame = IsLastLook == other.IsLastLook;
        var supportsIceBergsSame = SupportsIceBergs == other.SupportsIceBergs;
        var syncRetryIntervalSame = SyncRetryIntervalMs == other.SyncRetryIntervalMs;
        var syncAllowUpdatesCatchupSame = AllowUpdatesCatchup == other.AllowUpdatesCatchup;

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

    public override string ToString() =>
        $"{nameof(PricingServerConfig)}({nameof(ConnectionName)}: {ConnectionName}, {nameof(SnapshotConnectionConfig)}: {SnapshotConnectionConfig}, " +
        $"{nameof(UpdateConnectionConfig)}: {UpdateConnectionConfig}, {nameof(HeartBeatPublishIntervalMs)}: {HeartBeatPublishIntervalMs},  " +
        $"{nameof(HeartBeatServerToleranceRangeMs)}: {HeartBeatServerToleranceRangeMs}, {nameof(MaxMissedHeartBeats)}: {MaxMissedHeartBeats}, " +
        $"{nameof(IsLastLook)}: {IsLastLook}, {nameof(SupportsIceBergs)}: {SupportsIceBergs}, {nameof(SyncRetryIntervalMs)}: {SyncRetryIntervalMs}, " +
        $"{nameof(AllowUpdatesCatchup)}: {AllowUpdatesCatchup}, {nameof(Path)}: {Path}";
}
