#region

using FortitudeCommon.Configuration;
using FortitudeIO.Transports.Network.Config;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeBusRules.Config;

public interface IRemoteServiceConfig
{
    List<INetworkTopicConnectionConfig> RemoteServiceConnectionConfigs { get; set; }
    string Name { get; set; }
    string StreamType { get; set; }
    ActivationState ActivationState { get; set; }
    string Description { get; set; }
    string ClientInitiatorFullClassName { get; set; }
    IServiceCustomConfig? ServiceCustomConfig { get; set; }
}

public class RemoteServiceConfig : ConfigSection, IRemoteServiceConfig
{
    private List<string>? connectRemoteServiceNames;
    private List<INetworkTopicConnectionConfig>? remoteServiceConnectionConfigs;
    private IServiceCustomConfig? serviceCustomConfig;

    public RemoteServiceConfig(IConfigurationRoot configRoot, string path) : base(configRoot, path) { }

    public RemoteServiceConfig(IRemoteServiceConfig toClone, IConfigurationRoot root, string path) : this(root, path)
    {
        Name = toClone.Name;
        StreamType = toClone.StreamType;
        Description = toClone.Description;
        ClientInitiatorFullClassName = toClone.ClientInitiatorFullClassName;
        RemoteServiceConnectionConfigs = toClone.RemoteServiceConnectionConfigs;
        ServiceCustomConfig = toClone.ServiceCustomConfig;
    }

    public RemoteServiceConfig(IRemoteServiceConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public List<string> ConnectToRemoteServiceNames
    {
        get
        {
            if (connectRemoteServiceNames != null) return connectRemoteServiceNames;
            connectRemoteServiceNames = new List<string>();
            foreach (var serviceName in GetSection(nameof(ConnectToRemoteServiceNames)).GetChildren())
                connectRemoteServiceNames.Add(serviceName!.Value!);
            return connectRemoteServiceNames;
        }
        set
        {
            connectRemoteServiceNames = new List<string>();
            for (var i = 0; i < value.Count; i++)
            {
                this[nameof(ConnectToRemoteServiceNames) + $":{i}"] = value[i];
                connectRemoteServiceNames.Add(value[i]);
            }
        }
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

    public string Description
    {
        get => this[nameof(Description)]!;
        set => this[nameof(Description)] = value;
    }

    public string ClientInitiatorFullClassName
    {
        get => this[nameof(ClientInitiatorFullClassName)]!;
        set => this[nameof(ClientInitiatorFullClassName)] = value;
    }

    public List<INetworkTopicConnectionConfig> RemoteServiceConnectionConfigs
    {
        get
        {
            if (remoteServiceConnectionConfigs != null) return remoteServiceConnectionConfigs;
            remoteServiceConnectionConfigs = new List<INetworkTopicConnectionConfig>();
            foreach (var configurationSection in GetSection(nameof(RemoteServiceConnectionConfigs)).GetChildren())
                if (configurationSection["TopicName"] != null)
                    remoteServiceConnectionConfigs.Add(new NetworkTopicConnectionConfig(ConfigRoot, configurationSection.Path));
            return remoteServiceConnectionConfigs;
        }
        set
        {
            remoteServiceConnectionConfigs = new List<INetworkTopicConnectionConfig>();
            for (var i = 0; i < value.Count; i++)
                remoteServiceConnectionConfigs.Add(new NetworkTopicConnectionConfig(value[i], ConfigRoot
                    , Path + ":" + nameof(RemoteServiceConnectionConfigs) + $":{i}"));
        }
    }

    public IServiceCustomConfig? ServiceCustomConfig
    {
        get
        {
            if (GetSection(nameof(ServiceCustomConfig)).GetChildren().Any())
                return serviceCustomConfig ??= new ServiceCustomConfig(ConfigRoot, Path + ":" + nameof(ServiceCustomConfig));
            return null;
        }
        set => serviceCustomConfig = value != null ? new ServiceCustomConfig(value, ConfigRoot, Path + ":" + nameof(ServiceCustomConfig)) : null;
    }
}
