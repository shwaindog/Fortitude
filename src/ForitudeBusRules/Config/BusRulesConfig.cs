#region

using FortitudeBusRules.Injection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;

#endregion

namespace FortitudeBusRules.Config;

public class BusRulesConfig : ConfigurationSection
{
    public const string DefaultBusRulesConfigPath = "BusRulesConfig";
    private readonly IConfigurationRoot configRoot;
    private ClusterConfig? clusterConfig;

    private QueuesConfig? queuesConfig;

    public BusRulesConfig(IConfigurationRoot configRoot, string path) : base(configRoot, path) => this.configRoot = configRoot;

    public BusRulesConfig() : this(new ConfigurationBuilder().Add(new MemoryConfigurationSource()).Build(), DefaultBusRulesConfigPath) { }

    public string? Name
    {
        get => this[nameof(Name)];
        set => this[nameof(Name)] = value;
    }

    public string? Description
    {
        get => this[nameof(Description)];
        set => this[nameof(Description)] = value;
    }

    public QueuesConfig QueuesConfig
    {
        get => queuesConfig ??= new QueuesConfig(configRoot, Path + ":" + nameof(QueuesConfig));
        set => queuesConfig = new QueuesConfig(value, configRoot, Path + ":" + nameof(QueuesConfig));
    }

    public ClusterConfig? ClusterConfig
    {
        get
        {
            if (GetSection(nameof(ClusterConfig)).GetChildren().Any())
                return clusterConfig ??= new ClusterConfig(configRoot, Path + ":" + nameof(ClusterConfig));
            return null;
        }
        set => clusterConfig = value != null ? new ClusterConfig(value, configRoot, Path + ":" + nameof(ClusterConfig)) : null;
    }

    public IDependencyResolver? Resolver { get; set; }
}
