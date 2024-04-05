#region

using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeBusRules.Config;

public interface IClusterInstance
{
    ActivationState ActivationState { get; set; }

    IServiceEndpoint? ClusterServiceEndpoint { get; set; }
}

public class ClusterInstance : ConfigurationSection, IClusterInstance
{
    private readonly IConfigurationRoot configRoot;

    private IServiceEndpoint? clusterServiceEndpoint;

    public ClusterInstance(IConfigurationRoot configRoot, string path) : base(configRoot, path) => this.configRoot = configRoot;

    public ClusterInstance(IClusterInstance toClone, IConfigurationRoot root, string path) : this(root, path)
    {
        ActivationState = toClone.ActivationState;
        ClusterServiceEndpoint = toClone.ClusterServiceEndpoint;
    }

    public ActivationState ActivationState
    {
        get => Enum.TryParse<ActivationState>(this[nameof(ActivationState)]!, out var activationState) ? activationState : ActivationState.Disabled;
        set => this[nameof(ActivationState)] = value.ToString();
    }

    public IServiceEndpoint? ClusterServiceEndpoint
    {
        get => clusterServiceEndpoint ??= new ServiceEndpoint(configRoot, Path + ":" + nameof(ClusterServiceEndpoint));
        set => clusterServiceEndpoint = value != null ? new ServiceEndpoint(value, configRoot, Path + ":" + nameof(ClusterServiceEndpoint)) : null;
    }
}
