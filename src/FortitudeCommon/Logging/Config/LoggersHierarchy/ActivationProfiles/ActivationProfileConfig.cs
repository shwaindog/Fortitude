// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Config;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.LoggersHierarchy.ActivationProfiles;

public interface IActivationProfileConfig : IInterfacesComparable<IActivationProfileConfig>, IConfigCloneTo<IActivationProfileConfig>
  , IFLogConfig, IStyledToStringObject
{
    FLogBuildTypeProfile BuildType { get; }
    
    FlogDeploymentEnvironmentProfileType DeploymentEnvironment { get; }

    LoggerActivationFlags AsLoggerActionFlags { get; }
}

public interface IMutableActivationProfileConfig : IActivationProfileConfig, IMutableFLogConfig
{
    new FLogBuildTypeProfile BuildType { get; set; }
    
    new FlogDeploymentEnvironmentProfileType DeploymentEnvironment { get; set; }
}


public class ActivationProfileConfig: FLogConfig, IMutableActivationProfileConfig
{
    private readonly IActivationProfileConfig?              parentConfig;
    
    private          IAppendableNamedAppendersLookupConfig? appendersConfig;
    public ActivationProfileConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public ActivationProfileConfig(IConfigurationRoot root, string path, IActivationProfileConfig? parentConfig) : base(root, path)
    {
        this.parentConfig = parentConfig;
    }

    public ActivationProfileConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public ActivationProfileConfig(IAppendableNamedAppendersLookupConfig? appendersCfg)
        : this(InMemoryConfigRoot, InMemoryPath, appendersCfg) { }

    public ActivationProfileConfig(IConfigurationRoot root, string path, IAppendableNamedAppendersLookupConfig? appendersCfg = null) 
        : base(root, path)
    {
    }

    public ActivationProfileConfig(IActivationProfileConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
    }

    public ActivationProfileConfig(IActivationProfileConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }
    
    public FLogBuildTypeProfile BuildType
    {
        get
        {
            if (Enum.TryParse<FLogBuildTypeProfile>(this[nameof(BuildType)], out var buildType))
            {
                return buildType; 
            }
            return parentConfig?.BuildType ?? FLogBuildTypeProfile.AllBuildTypes;
        }
        
        set => this[nameof(BuildType)] = value.ToString();
    }

    public FlogDeploymentEnvironmentProfileType DeploymentEnvironment
    {
        get
        {
            if (Enum.TryParse<FlogDeploymentEnvironmentProfileType>(this[nameof(DeploymentEnvironment)], out var env))
            {
                return env; 
            }
            return parentConfig?.DeploymentEnvironment ?? FlogDeploymentEnvironmentProfileType.AnyEnv;
        }
        
        set => this[nameof(BuildType)] = value.ToString();
    }

    public LoggerActivationFlags AsLoggerActionFlags => (LoggerActivationFlags)((uint)DeploymentEnvironment | (uint)BuildType);

    public override T               Visit<T>(T visitor) => throw new NotImplementedException();

    object ICloneable.              Clone() => Clone();

    public IActivationProfileConfig Clone() => new ActivationProfileConfig(this);

    IActivationProfileConfig IConfigCloneTo<IActivationProfileConfig>.CloneConfigTo(IConfigurationRoot configRoot, string path) => 
        CloneConfigTo(configRoot, path);

    public ActivationProfileConfig CloneConfigTo(IConfigurationRoot configRoot, string path) => 
        new (this, configRoot, path);

    public virtual bool AreEquivalent(IActivationProfileConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var buildTypeSame = BuildType == other.BuildType;
        var deploymentEnvSame = DeploymentEnvironment == other.DeploymentEnvironment;

        var allAreSame = buildTypeSame && deploymentEnvSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IActivationProfileConfig, true);

    public override int GetHashCode()
    {
        var hashCode = (int)BuildType;
        hashCode = (hashCode * 397) ^ (int)DeploymentEnvironment;
        return hashCode;
    }

    public StyledTypeBuildResult ToString(IStyledTypeStringAppender sbc) => 
        sbc.StartComplexType(nameof(ActivationProfileConfig))
           .Field.AlwaysAdd(nameof(BuildType), BuildType)
           .Field.AlwaysAdd(nameof(DeploymentEnvironment), DeploymentEnvironment)
           .Complete();

    public override string ToString() => this.DefaultToString();
}

