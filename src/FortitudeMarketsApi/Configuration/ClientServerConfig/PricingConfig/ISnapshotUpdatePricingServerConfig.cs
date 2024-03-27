#region

using FortitudeCommon.Types;
using FortitudeIO.Transports.NewSocketAPI.Config;

#endregion

namespace FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

public interface ISnapshotUpdatePricingServerConfig : IMarketServerConfig<ISnapshotUpdatePricingServerConfig>,
    ICloneable<ISnapshotUpdatePricingServerConfig>, IInterfacesComparable<ISnapshotUpdatePricingServerConfig>
{
    ushort PublicationId { get; }
    INetworkTopicConnectionConfig? SnapshotConnectionConfig { get; }
    INetworkTopicConnectionConfig? UpdateConnectionConfig { get; }
    IList<ISourceTickerPublicationConfig>? SourceTickerPublicationConfigs { get; }
    bool IsLastLook { get; }
    bool SupportsIceBergs { get; }
    new ISnapshotUpdatePricingServerConfig Clone();
}
