// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Config;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.LoggersHierarchy.ActivationProfiles;


public interface IFLoggerActivationConfig : IFLogBuildTypeAndDeployEnvConfig, IConfigCloneTo<IFLoggerActivationConfig>
{
    LoggerActivationFlags LoggerActivationFlags { get; }
    
    new IFLoggerActivationConfig Clone();

    new IFLoggerActivationConfig CloneConfigTo(IConfigurationRoot configRoot, string path);
}

public interface IMutableFLoggerActivationConfig : IFLoggerActivationConfig, IMutableFLogBuildTypeAndDeployEnvConfig
{
    new LoggerActivationFlags LoggerActivationFlags { get; set; }
}

public class FLoggerActivationConfig : FLogBuildTypeAndDeployEnvConfig, IMutableFLoggerActivationConfig
{
    public FLoggerActivationConfig(IConfigurationRoot root, string path) : base(root, path) { }
    
    public FLoggerActivationConfig(IConfigurationRoot root, string path, IFLoggerActivationConfig? parentLoggerEnvConfig) : base(root, path, parentLoggerEnvConfig) { }
    
    public FLoggerActivationConfig() { }
    public FLoggerActivationConfig(IFLoggerActivationConfig toClone, IConfigurationRoot root, string path) : base(toClone, root, path) { }
    public FLoggerActivationConfig(IFLoggerActivationConfig toClone) : base(toClone) { }


    public LoggerActivationFlags LoggerActivationFlags
    {
        get
        {
            if (Enum.TryParse<LoggerActivationFlags>(this[nameof(LoggerActivationFlags)], out var env))
            {
                return env;
            }
            return ParentLoggerActivationConfig?.LoggerActivationFlags ?? LoggerActivationFlags.Disabled;
        }

        set => this[nameof(LoggerActivationFlags)] = value.ToString();
    }

    public override LoggerActivationFlags AsLoggerActionFlags => base.AsLoggerActionFlags | LoggerActivationFlags;

    protected IFLoggerActivationConfig? ParentLoggerActivationConfig => ParentLoggerEnvConfig as IFLoggerActivationConfig;

    IFLoggerActivationConfig ICloneable<IFLoggerActivationConfig>.    Clone() => new FLoggerActivationConfig(this);

    IFLoggerActivationConfig IFLoggerActivationConfig.Clone() => Clone();

    public override FLoggerActivationConfig Clone() => new (this);

    IFLoggerActivationConfig IConfigCloneTo<IFLoggerActivationConfig>.CloneConfigTo(IConfigurationRoot configRoot, string path) => CloneConfigTo(configRoot, path);

    IFLoggerActivationConfig IFLoggerActivationConfig.CloneConfigTo(IConfigurationRoot configRoot, string path) => CloneConfigTo(configRoot, path);

    public override FLoggerActivationConfig CloneConfigTo(IConfigurationRoot configRoot, string path) => 
        new (this, configRoot, path);
    
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(LoggerActivationFlags), LoggerActivationFlags)
           .AddBaseRevealStateFields(this)
           .Complete();
}
