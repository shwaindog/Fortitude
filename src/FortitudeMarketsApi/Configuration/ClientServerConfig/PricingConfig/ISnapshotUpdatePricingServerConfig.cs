using System;
using System.Collections.Generic;
using FortitudeCommon.Types;
using FortitudeIO.Sockets;
using FortitudeIO.Transports.Sockets;

namespace FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig
{

    public interface ISnapshotUpdatePricingServerConfig :  IMarketServerConfig<ISnapshotUpdatePricingServerConfig>, 
        ICloneable<ISnapshotUpdatePricingServerConfig>, IInterfacesComparable<ISnapshotUpdatePricingServerConfig>
    {
        ushort PublicationId { get; }
        IConnectionConfig SnapshotConnectionConfig { get; }
        IConnectionConfig UpdateConnectionConfig { get; }
        IList<ISourceTickerPublicationConfig> SourceTickerPublicationConfigs { get; }
        bool IsLastLook { get; }
        bool SupportsIceBergs { get; }
        new ISnapshotUpdatePricingServerConfig Clone();
    }
}
