using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using FortitudeIO.Config;
using FortitudeIO.Transports.Network.Config;
using Microsoft.Extensions.Configuration;

namespace FortitudeIO.Storage.Metrics.Config;

public interface IMetricsConfig
{
    bool EnableMetrics { get; set; }

    bool EnableMetricsCollection { get; set; }

    ILocalEndpointConfig? LocalMetricsPollingEndpoint { get; set; }

    IEndpointConfig? MetricsPublishEndpoint { get; set; }
}

public class MetricsConfig : ConfigSection, IMetricsConfig
{
    public MetricsConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public MetricsConfig() { }

    public MetricsConfig(bool enableMetrics, bool enableMetricsCollection, 
        ILocalEndpointConfig? localEndpointConfig = null, IEndpointConfig? publishMetricsEndpoint = null ) 
    {
        EnableMetrics               = enableMetrics;
        EnableMetricsCollection     = enableMetricsCollection;
        LocalMetricsPollingEndpoint = localEndpointConfig;
        MetricsPublishEndpoint      = publishMetricsEndpoint;
    }

    public MetricsConfig(IMetricsConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        EnableMetrics               = toClone.EnableMetrics;
        EnableMetricsCollection     = toClone.EnableMetricsCollection;
        LocalMetricsPollingEndpoint = toClone.LocalMetricsPollingEndpoint;
        MetricsPublishEndpoint      = toClone.MetricsPublishEndpoint;
    }

    public MetricsConfig(IMetricsConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public bool EnableMetrics
    {
        get
        {
            var checkValue = this[nameof(EnableMetrics)]!;
            return checkValue.IsNullOrEmpty() || bool.Parse(checkValue);
        }

        set => this[nameof(EnableMetrics)] = value.ToString();
    }

    public bool EnableMetricsCollection
    {
        get
        {
            var checkValue = this[nameof(EnableMetricsCollection)]!;
            return checkValue.IsNullOrEmpty() || bool.Parse(checkValue);
        }

        set => this[nameof(EnableMetricsCollection)] = value.ToString();
    }

    public ILocalEndpointConfig? LocalMetricsPollingEndpoint
    {
        get
        {
            if (GetSection(nameof(MetricsPublishEndpoint)).GetChildren().Any())
            {
                var endpointConfig = new LocalEndpointConfig(ConfigRoot, $"{Path}{Split}{nameof(LocalMetricsPollingEndpoint)}");
                return endpointConfig;
            }
            return null;
        }
        set => _ = value != null ? new LocalEndpointConfig(value, ConfigRoot, $"{Path}{Split}{nameof(LocalMetricsPollingEndpoint)}") : null;
    }

    public IEndpointConfig? MetricsPublishEndpoint
    {
        get
        {
            if (GetSection(nameof(MetricsPublishEndpoint)).GetChildren().Any())
            {
                var endpointConfig = new EndpointConfig(ConfigRoot, $"{Path}{Split}{nameof(MetricsPublishEndpoint)}");
                return endpointConfig;
            }
            return null;
        }
        set => _ = value != null ? new EndpointConfig(value, ConfigRoot, $"{Path}{Split}{nameof(MetricsPublishEndpoint)}") : null;
    }
}
