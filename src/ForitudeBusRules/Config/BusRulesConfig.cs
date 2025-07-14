// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Injection;
using FortitudeCommon.Config;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeBusRules.Config;

public class BusRulesConfig : AlternativeConfigLocationLookup<BusRulesConfig>
{
    protected BusRulesConfig? LookupBusRulesConfigConfig;

    private ClusterConfig? clusterConfig;

    private IQueuesConfig? queuesConfig;

    public BusRulesConfig(IConfigurationRoot configRoot, string path) : base(configRoot, path) { }

    public BusRulesConfig(IQueuesConfig queuesConfig, IClusterConfig? clusterConfig = null)
    {
        QueuesConfig  = queuesConfig;
        ClusterConfig = clusterConfig;
    }

    public BusRulesConfig() : this(InMemoryConfigRoot, InMemoryPath) { }
    
    public override BusRulesConfig? LookupValue
    {
        get
        {
            if (HasFoundConfigLookup && LookupBusRulesConfigConfig == null)
            {
                LookupBusRulesConfigConfig = new BusRulesConfig(ConfigRoot, ConfigLookupReferencePath!);
            }
            return LookupBusRulesConfigConfig;
        }
    }

    public string Name
    {
        get => this[nameof(Name)] ?? LookupBusRulesConfigConfig?.Name ?? throw new ArgumentException("Expected BusRulesConfig to have a name");
        set => this[nameof(Name)] = value;
    }

    public string? Description
    {
        get => this[nameof(Description)] ?? LookupBusRulesConfigConfig?.Description;
        set => this[nameof(Description)] = value;
    }

    public IQueuesConfig QueuesConfig
    {
        get
        {
            if (queuesConfig != null) return queuesConfig;

            if (GetSection($"{Path}{Split}{nameof(QueuesConfig)}").GetChildren().Any())
            {
                queuesConfig = new QueuesConfig(ConfigRoot, $"{Path}{Split}{nameof(QueuesConfig)}");
                return queuesConfig;
            }
            queuesConfig = LookupBusRulesConfigConfig?.QueuesConfig;
            return queuesConfig ?? new QueuesConfig(ConfigRoot, $"{Path}{Split}{nameof(QueuesConfig)}");
        }
        set => queuesConfig = new QueuesConfig(value, ConfigRoot, $"{Path}{Split}{nameof(QueuesConfig)}");
    }

    public IClusterConfig? ClusterConfig
    {
        get
        {
            if (GetSection(nameof(ClusterConfig)).GetChildren().Any())
                return clusterConfig ??= new ClusterConfig(ConfigRoot, $"{Path}{Split}{nameof(ClusterConfig)}");
            return LookupBusRulesConfigConfig?.ClusterConfig;
        }
        set => clusterConfig = value != null ? new ClusterConfig(value, ConfigRoot, $"{Path}{Split}{nameof(ClusterConfig)}") : null;
    }

    public IDependencyResolver? Resolver { get; set; }
}
