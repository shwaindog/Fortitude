#region

using FortitudeCommon.Config;
using FortitudeCommon.DataStructures.Lists;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeBusRules.Config;

public interface ILocalServiceConfig
{
    string Name { get; set; }
    string StreamType { get; set; }
    ActivationState ActivationState { get; set; }
    string ServiceInitiatorFullClassName { get; set; }
    IEnumerable<IServiceEndpoint> Endpoints { get; set; }
    IServiceCustomConfig? ServiceCustomConfig { get; set; }
}

public class LocalServiceConfig : ConfigSection, ILocalServiceConfig
{
    private object? ignoreSuppressWarnings;
    private IServiceCustomConfig? serviceCustomConfig;

    public LocalServiceConfig(IConfigurationRoot configRoot, string path) : base(configRoot, path) { }

    public LocalServiceConfig(ILocalServiceConfig toClone, IConfigurationRoot configRoot, string path) : this(configRoot, path)
    {
        Name = toClone.Name;
        StreamType = toClone.StreamType;
        ServiceInitiatorFullClassName = toClone.ServiceInitiatorFullClassName;
        Endpoints = toClone.Endpoints;
        ServiceCustomConfig = toClone.ServiceCustomConfig;
    }

    public LocalServiceConfig(ILocalServiceConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }


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

    public IEnumerable<IServiceEndpoint> Endpoints
    {
        get
        {
            var autoRecycleList = Recycler.Borrow<AutoRecycledEnumerable<IServiceEndpoint>>();
            foreach (var configurationSection in GetSection(nameof(Endpoints)).GetChildren())
                if (configurationSection.GetChildren().Any(cs => cs.Key == "ServiceStartConnectionConfig"))
                    if (configurationSection.GetChildren()
                        .SelectMany(cs => cs.GetChildren())
                        .Any(cs => cs is { Key: "TopicName", Value: not null }))
                        autoRecycleList.Add(new ServiceEndpoint(ConfigRoot, configurationSection.Path));
            return autoRecycleList;
        }
        set
        {
            var oldCount = Endpoints.Count();
            var i = 0;
            foreach (var serviceEndpoint in value)
            {
                ignoreSuppressWarnings = new ServiceEndpoint(serviceEndpoint, ConfigRoot, $"{Path}{Split}{nameof(Endpoints)}{Split}{i}");
                i++;
            }

            for (var j = i; j < oldCount; j++) ServiceEndpoint.ClearValues(ConfigRoot, $"{Path}{Split}{nameof(Endpoints)}{Split}{i}");
        }
    }

    public IServiceCustomConfig? ServiceCustomConfig
    {
        get
        {
            if (GetSection(nameof(ServiceCustomConfig)).GetChildren().Any())
                return serviceCustomConfig ??= new ServiceCustomConfig(ConfigRoot, $"{Path}{Split}{nameof(ServiceCustomConfig)}");
            return serviceCustomConfig;
        }
        set => serviceCustomConfig = value != null ? new ServiceCustomConfig(value, ConfigRoot, $"{Path}{Split}{nameof(ServiceCustomConfig)}") : null;
    }
}
