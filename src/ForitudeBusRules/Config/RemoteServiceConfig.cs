#region

using FortitudeCommon.Configuration;
using FortitudeCommon.DataStructures.Lists;
using FortitudeIO.Transports.Network.Config;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeBusRules.Config;

public interface IRemoteServiceConfig
{
    IEnumerable<INetworkTopicConnectionConfig> RemoteServiceConnectionConfigs { get; set; }
    string Name { get; set; }
    string StreamType { get; set; }
    ActivationState ActivationState { get; set; }
    string Description { get; set; }
    string ClientInitiatorFullClassName { get; set; }
    IServiceCustomConfig? ServiceCustomConfig { get; set; }
}

public class RemoteServiceConfig : ConfigSection, IRemoteServiceConfig
{
    private object? ignoreSuppressWarnings;
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

    public IEnumerable<INetworkTopicConnectionConfig> RemoteServiceConnectionConfigs
    {
        get
        {
            var autoRecycleList = Recycler.Borrow<AutoRecycledEnumerable<INetworkTopicConnectionConfig>>();
            foreach (var configurationSection in GetSection(nameof(RemoteServiceConnectionConfigs)).GetChildren())
                if (configurationSection["TopicName"] != null)
                    autoRecycleList.Add(new NetworkTopicConnectionConfig(ConfigRoot, configurationSection.Path));
            return autoRecycleList;
        }
        set
        {
            var oldCount = RemoteServiceConnectionConfigs.Count();
            var i = 0;
            foreach (var remoteServiceConfig in value)
            {
                ignoreSuppressWarnings = new NetworkTopicConnectionConfig(remoteServiceConfig, ConfigRoot
                    , Path + ":" + nameof(RemoteServiceConnectionConfigs) + $":{i}");
                i++;
            }

            for (var j = i; j < oldCount; j++)
                NetworkTopicConnectionConfig.ClearValues(ConfigRoot, Path + ":" + nameof(RemoteServiceConnectionConfigs) + $":{i}");
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

    public static void ClearValues(IConfigurationRoot root, string path)
    {
        root[path + ":" + nameof(Name)] = null;
        root[path + ":" + nameof(StreamType)] = null;
        root[path + ":" + nameof(Description)] = null;
        root[path + ":" + nameof(ClientInitiatorFullClassName)] = null;
        root[path + ":" + nameof(RemoteServiceConnectionConfigs)] = null;
        root[path + ":" + nameof(ServiceCustomConfig)] = null;
    }
}
