#region

using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeBusRules.Config;

public interface ILocalServiceConfig
{
    string Name { get; set; }
    string StreamType { get; set; }
    ActivationState ActivationState { get; set; }
    string ServiceInitiatorFullClassName { get; set; }
    List<IServiceEndpoint> Endpoints { get; set; }
    IServiceCustomConfig? ServiceCustomConfig { get; set; }
}

public class LocalServiceConfig : ConfigurationSection, ILocalServiceConfig
{
    public const string DefaultLocalServiceConfigPath = ClusterConfig.DefaultClusterConfigPath + ":" + "LocalServiceConfig";
    private readonly IConfigurationRoot configRoot;

    private readonly List<IServiceEndpoint> lastAccessedServiceEndpoints = new();
    private IServiceCustomConfig? serviceCustomConfig;

    public LocalServiceConfig(IConfigurationRoot configRoot, string path) : base(configRoot, path) => this.configRoot = configRoot;

    public LocalServiceConfig(ILocalServiceConfig toClone, IConfigurationRoot configRoot, string path) : this(configRoot, path)
    {
        Name = toClone.Name;
        StreamType = toClone.StreamType;
        ServiceInitiatorFullClassName = toClone.ServiceInitiatorFullClassName;
        Endpoints = toClone.Endpoints;
        ServiceCustomConfig = toClone.ServiceCustomConfig;
    }


    public string Name
    {
        get => this[nameof(Name)]!;
        set => this[nameof(Name)] = value;
    }

    public string StreamType
    {
        get => this[nameof(StreamType)]!;
        set => this[nameof(StreamType)] = value;
    }

    public ActivationState ActivationState
    {
        get => Enum.TryParse<ActivationState>(this[nameof(ActivationState)]!, out var activationState) ? activationState : ActivationState.Disabled;
        set => this[nameof(ActivationState)] = value.ToString();
    }

    public string ServiceInitiatorFullClassName
    {
        get => this[nameof(ServiceInitiatorFullClassName)]!;
        set => this[nameof(ServiceInitiatorFullClassName)] = value;
    }

    public List<IServiceEndpoint> Endpoints
    {
        get
        {
            lastAccessedServiceEndpoints.Clear();
            foreach (var configurationSection in GetSection(nameof(Endpoints)).GetChildren())
                if (configurationSection.GetChildren().Any(cs => cs.Key == "ServiceStartConnectionConfig"))
                    lastAccessedServiceEndpoints.Add(new ServiceEndpoint(configRoot, configurationSection.Path));
            return lastAccessedServiceEndpoints;
        }
        set
        {
            lastAccessedServiceEndpoints.Clear();
            for (var i = 0; i < value.Count; i++)
                lastAccessedServiceEndpoints.Add(new ServiceEndpoint(value[i], configRoot
                    , Path + ":" + nameof(Endpoints) + $":{i}"));
        }
    }

    public IServiceCustomConfig? ServiceCustomConfig
    {
        get
        {
            if (GetSection(nameof(ServiceCustomConfig)).GetChildren().Any())
                return serviceCustomConfig ??= new ServiceCustomConfig(configRoot, Path + ":" + nameof(ServiceCustomConfig));
            return null;
        }
        set => serviceCustomConfig = value != null ? new ServiceCustomConfig(value, configRoot, Path + ":" + nameof(ServiceCustomConfig)) : null;
    }
}
