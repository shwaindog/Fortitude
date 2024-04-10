#region

using FortitudeCommon.Configuration;
using FortitudeCommon.Types;
using FortitudeIO.Transports.Network.Config;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

public interface IPricingServerConfig : IInterfacesComparable<IPricingServerConfig>
{
    INetworkTopicConnectionConfig? SnapshotConnectionConfig { get; }
    INetworkTopicConnectionConfig? UpdateConnectionConfig { get; }
    bool IsLastLook { get; set; }
    bool SupportsIceBergs { get; set; }
    uint SyncRetryIntervalMs { get; set; }
    bool AllowUpdatesCatchup { get; set; }
    IPricingServerConfig ToggleProtocolDirection();
}

public class PricingServerConfig : ConfigSection, IPricingServerConfig
{
    private INetworkTopicConnectionConfig? snapshotConnectionConfig;
    private INetworkTopicConnectionConfig? updateConnectionConfig;

    public PricingServerConfig(IConfigurationRoot root, string path) : base(root, path) { }
    public PricingServerConfig() { }

    public PricingServerConfig(INetworkTopicConnectionConfig snapshotServerNetConnConfig, INetworkTopicConnectionConfig updateServerNetConnConfig,
        bool isLastLook = true, bool supportsIceBergs = false, uint syncRetryIntervalMs = 2_000, bool allowUpdatesCatchup = true)
    {
        IsLastLook = isLastLook;
        SupportsIceBergs = supportsIceBergs;
        SnapshotConnectionConfig = snapshotServerNetConnConfig;
        UpdateConnectionConfig = updateServerNetConnConfig;
        SyncRetryIntervalMs = syncRetryIntervalMs;
        AllowUpdatesCatchup = allowUpdatesCatchup;
    }

    public PricingServerConfig(IPricingServerConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        IsLastLook = toClone.IsLastLook;
        SupportsIceBergs = toClone.SupportsIceBergs;
        SnapshotConnectionConfig = toClone.SnapshotConnectionConfig?.Clone();
        UpdateConnectionConfig = toClone.UpdateConnectionConfig?.Clone();
        SyncRetryIntervalMs = toClone.SyncRetryIntervalMs;
        AllowUpdatesCatchup = toClone.AllowUpdatesCatchup;
    }

    public PricingServerConfig(IPricingServerConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public INetworkTopicConnectionConfig? SnapshotConnectionConfig
    {
        get =>
            snapshotConnectionConfig
                = new NetworkTopicConnectionConfig(ConfigRoot, Path + ":" + nameof(SnapshotConnectionConfig));
        set =>
            snapshotConnectionConfig
                = new NetworkTopicConnectionConfig(value, ConfigRoot, Path + ":" + nameof(SnapshotConnectionConfig));
    }

    public INetworkTopicConnectionConfig? UpdateConnectionConfig
    {
        get =>
            updateConnectionConfig
                = new NetworkTopicConnectionConfig(ConfigRoot, Path + ":" + nameof(UpdateConnectionConfig));
        set =>
            updateConnectionConfig
                = new NetworkTopicConnectionConfig(value, ConfigRoot, Path + ":" + nameof(UpdateConnectionConfig));
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

    public IPricingServerConfig ToggleProtocolDirection() =>
        new PricingServerConfig(this)
        {
            SnapshotConnectionConfig = SnapshotConnectionConfig?.ToggleProtocolDirection()
            , UpdateConnectionConfig = UpdateConnectionConfig?.ToggleProtocolDirection()
        };


    public bool AreEquivalent(IPricingServerConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;

        var snapshotServerConConfigSame = Equals(snapshotConnectionConfig, other.SnapshotConnectionConfig);
        var updateServerConConfigSame = Equals(updateConnectionConfig, other.UpdateConnectionConfig);
        var isLastLookSame = IsLastLook == other.IsLastLook;
        var supportsIceBergsSame = SupportsIceBergs == other.SupportsIceBergs;
        var syncRetryIntervalSame = SyncRetryIntervalMs == other.SyncRetryIntervalMs;
        var syncAllowUpdatesCatchupSame = AllowUpdatesCatchup == other.AllowUpdatesCatchup;

        return snapshotServerConConfigSame && updateServerConConfigSame && isLastLookSame && supportsIceBergsSame && syncRetryIntervalSame &&
               syncAllowUpdatesCatchupSame;
    }

    public IPricingServerConfig Clone() => new PricingServerConfig(this);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IPricingServerConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = snapshotConnectionConfig?.GetHashCode() ?? 0;
            hashCode = (hashCode * 397) ^ (updateConnectionConfig?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ IsLastLook.GetHashCode();
            hashCode = (hashCode * 397) ^ SupportsIceBergs.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)SyncRetryIntervalMs;
            hashCode = (hashCode * 397) ^ AllowUpdatesCatchup.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() =>
        $"{nameof(PricingServerConfig)}({nameof(SnapshotConnectionConfig)}: {SnapshotConnectionConfig}, " +
        $"{nameof(UpdateConnectionConfig)}: {UpdateConnectionConfig}, {nameof(IsLastLook)}: {IsLastLook}, " +
        $"{nameof(SupportsIceBergs)}: {SupportsIceBergs}, {nameof(SyncRetryIntervalMs)}: {SyncRetryIntervalMs}, " +
        $"{nameof(AllowUpdatesCatchup)}: {AllowUpdatesCatchup}, {nameof(Path)}: {Path}";
}
