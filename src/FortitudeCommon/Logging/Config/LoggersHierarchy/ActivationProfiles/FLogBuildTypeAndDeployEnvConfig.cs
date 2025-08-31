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

public interface IFLogBuildTypeAndDeployEnvConfig : IInterfacesComparable<IFLogBuildTypeAndDeployEnvConfig>
  , IConfigCloneTo<IFLogBuildTypeAndDeployEnvConfig>
  , IFLogConfig, IStyledToStringObject
{
    FLogBuildTypeProfile BuildType { get; }

    FlogDeploymentEnvironmentProfileType DeploymentEnvironment { get; }

    LoggerActivationFlags AsLoggerActionFlags { get; }
}

public interface IMutableFLogBuildTypeAndDeployEnvConfig : IFLogBuildTypeAndDeployEnvConfig, IMutableFLogConfig
{
    new FLogBuildTypeProfile BuildType { get; set; }

    new FlogDeploymentEnvironmentProfileType DeploymentEnvironment { get; set; }
}

public class FLogBuildTypeAndDeployEnvConfig : FLogConfig, IMutableFLogBuildTypeAndDeployEnvConfig
{
    protected readonly IFLogBuildTypeAndDeployEnvConfig? ParentLoggerEnvConfig;
    public FLogBuildTypeAndDeployEnvConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public FLogBuildTypeAndDeployEnvConfig(IConfigurationRoot root, string path, IFLogBuildTypeAndDeployEnvConfig? parentLoggerEnvConfig) : base(root, path)
    {
        ParentLoggerEnvConfig = parentLoggerEnvConfig;
    }

    public FLogBuildTypeAndDeployEnvConfig() : this(InMemoryConfigRoot, InMemoryPath) { }


    public FLogBuildTypeAndDeployEnvConfig(IFLogBuildTypeAndDeployEnvConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        BuildType = toClone.BuildType;
        DeploymentEnvironment = toClone.DeploymentEnvironment;
    }

    public FLogBuildTypeAndDeployEnvConfig(IFLogBuildTypeAndDeployEnvConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public FLogBuildTypeProfile BuildType
    {
        get
        {
            if (Enum.TryParse<FLogBuildTypeProfile>(this[nameof(BuildType)], out var buildType))
            {
                return buildType;
            }
            return ParentLoggerEnvConfig?.BuildType ?? FLogBuildTypeProfile.AllBuildTypes;
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
            return ParentLoggerEnvConfig?.DeploymentEnvironment ?? FlogDeploymentEnvironmentProfileType.AnyEnv;
        }

        set => this[nameof(BuildType)] = value.ToString();
    }

    public virtual LoggerActivationFlags AsLoggerActionFlags => (LoggerActivationFlags)((uint)DeploymentEnvironment | (uint)BuildType);

    public override T Visit<T>(T visitor) => throw new NotImplementedException();

    object ICloneable.Clone() => Clone();

    IFLogBuildTypeAndDeployEnvConfig ICloneable<IFLogBuildTypeAndDeployEnvConfig>.Clone() => Clone();

    public virtual FLogBuildTypeAndDeployEnvConfig Clone() => new (this);

    IFLogBuildTypeAndDeployEnvConfig IConfigCloneTo<IFLogBuildTypeAndDeployEnvConfig>.CloneConfigTo(IConfigurationRoot configRoot, string path) =>
        CloneConfigTo(configRoot, path);

    public virtual FLogBuildTypeAndDeployEnvConfig CloneConfigTo(IConfigurationRoot configRoot, string path) => new(this, configRoot, path);

    public virtual bool AreEquivalent(IFLogBuildTypeAndDeployEnvConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var buildTypeSame     = BuildType == other.BuildType;
        var deploymentEnvSame = DeploymentEnvironment == other.DeploymentEnvironment;

        var allAreSame = buildTypeSame && deploymentEnvSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IFLogBuildTypeAndDeployEnvConfig, true);

    public override int GetHashCode()
    {
        var hashCode = (int)BuildType;
        hashCode = (hashCode * 397) ^ (int)DeploymentEnvironment;
        return hashCode;
    }

    public virtual StyledTypeBuildResult ToString(IStyledTypeStringAppender sbc) =>
        sbc.StartComplexType(nameof(FLogBuildTypeAndDeployEnvConfig))
           .Field.AlwaysAdd(nameof(BuildType), BuildType)
           .Field.AlwaysAdd(nameof(DeploymentEnvironment), DeploymentEnvironment)
           .Complete();

    public override string ToString() => this.DefaultToString();
}
