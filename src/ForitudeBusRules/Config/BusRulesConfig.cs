// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Injection;
using FortitudeCommon.Configuration;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeBusRules.Config;

public class BusRulesConfig : ConfigSection
{
    private ClusterConfig? clusterConfig;

    private QueuesConfig? queuesConfig;

    public BusRulesConfig(IConfigurationRoot configRoot, string path) : base(configRoot, path) { }

    public BusRulesConfig(IQueuesConfig queuesConfig, IClusterConfig? clusterConfig = null)
    {
        QueuesConfig  = queuesConfig;
        ClusterConfig = clusterConfig;
    }

    public BusRulesConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

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

    public IQueuesConfig QueuesConfig
    {
        get => queuesConfig ??= new QueuesConfig(ConfigRoot, Path + ":" + nameof(QueuesConfig));
        set => queuesConfig = new QueuesConfig(value, ConfigRoot, Path + ":" + nameof(QueuesConfig));
    }

    public IClusterConfig? ClusterConfig
    {
        get
        {
            if (GetSection(nameof(ClusterConfig)).GetChildren().Any())
                return clusterConfig ??= new ClusterConfig(ConfigRoot, Path + ":" + nameof(ClusterConfig));
            return null;
        }
        set => clusterConfig = value != null ? new ClusterConfig(value, ConfigRoot, Path + ":" + nameof(ClusterConfig)) : null;
    }

    public IDependencyResolver? Resolver { get; set; }
}
