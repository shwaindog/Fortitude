#region

using FortitudeCommon.Types;
using FortitudeIO.Transports.NewSocketAPI.Config;

#endregion

namespace FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

public interface ISnapshotUpdatePricingServerConfig : IMarketServerConfig<ISnapshotUpdatePricingServerConfig>,
    ICloneable<ISnapshotUpdatePricingServerConfig>, IInterfacesComparable<ISnapshotUpdatePricingServerConfig>
{
    ushort PublicationId { get; }
    ISocketTopicConnectionConfig? SnapshotConnectionConfig { get; }
    ISocketTopicConnectionConfig? UpdateConnectionConfig { get; }
    IList<ISourceTickerPublicationConfig>? SourceTickerPublicationConfigs { get; }
    bool IsLastLook { get; }
    bool SupportsIceBergs { get; }
    new ISnapshotUpdatePricingServerConfig Clone();
}
