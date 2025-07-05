using FortitudeIO.Storage.Metrics.Config;
using FortitudeIO.Transports.Network.Config;
using FortitudeMarkets.Config;
using FortitudeMarkets.Config.Availability;

namespace FortitudeMarketsConnectivity.Adapters.Config;

public interface IAdapterCommonConfig
{
    string Name { get; set; }

    CountryCityCodes MyLocation { get; set; }

    ITimeTableConfig? VenueOperatingTimeTableConfig { get; set; }

    IMarketConnectionConfig MarketConfig { get; set; }

    IMetricsConfig Metrics { get; set; }
}