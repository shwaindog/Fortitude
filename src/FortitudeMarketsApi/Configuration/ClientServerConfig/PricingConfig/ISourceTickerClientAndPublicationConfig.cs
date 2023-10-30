using FortitudeCommon.Types;

namespace FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig
{
    public interface ISourceTickerClientAndPublicationConfig : ISourceTickerPublicationConfig
    {
        uint SyncRetryIntervalMs { get; }

        bool AllowUpdatesCatchup { get; }

        new ISourceTickerClientAndPublicationConfig Clone();
    }
}