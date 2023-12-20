namespace FortitudeMarketsApi.Configuration.ClientServerConfig.MarketsConfig;

internal interface IMarketsConfig
{
    IList<IVenueConfig> VenueConfigs { get; set; }
}

public interface IVenueConfig { }
