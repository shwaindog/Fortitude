// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Configuration;
using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending;

public interface IFLoggerMatchedAppenders
{
    INamedAppendersLookupConfig Appenders { get; }
}

public interface IMutableFLoggerMatchedAppenders : IFLoggerMatchedAppenders
{
    new IAppendableNamedAppendersLookupConfig Appenders { get; set; }
}

public abstract class FLoggerMatchedAppenders : FLogConfig, IMutableFLoggerMatchedAppenders
{
    private IAppendableNamedAppendersLookupConfig? appendersConfig;
    protected FLoggerMatchedAppenders(IConfigurationRoot root, string path) : base(root, path) { }

    protected FLoggerMatchedAppenders() : this(InMemoryConfigRoot, InMemoryPath) { }

    protected FLoggerMatchedAppenders
        (IAppendableNamedAppendersLookupConfig? appendersCfg)
        : this(InMemoryConfigRoot, InMemoryPath, appendersCfg) { }

    protected FLoggerMatchedAppenders
        (IConfigurationRoot root, string path, IAppendableNamedAppendersLookupConfig? appendersCfg = null) : base(root, path)
    {
        Appenders = appendersCfg ?? new NamedAppendersLookupConfig();
    }

    protected FLoggerMatchedAppenders(IFLoggerMatchedAppenders toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        Appenders = (IAppendableNamedAppendersLookupConfig)toClone.Appenders;
    }

    protected FLoggerMatchedAppenders(IFLoggerMatchedAppenders toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    INamedAppendersLookupConfig IFLoggerMatchedAppenders.Appenders => Appenders;

    public IAppendableNamedAppendersLookupConfig Appenders
    {
        get
        {
            if (appendersConfig == null)
            {
                if (GetSection(nameof(Appenders)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                {
                    return appendersConfig ??= new NamedAppendersLookupConfig(ConfigRoot, $"{Path}{Split}{nameof(Appenders)}")
                    {
                        ParentConfig = this
                    };
                }
            }
            return appendersConfig ??= new NamedAppendersLookupConfig(ConfigRoot, $"{Path}{Split}{nameof(Appenders)}")
            {
                ParentConfig = this
            };
        }
        set
        {
            appendersConfig = new NamedAppendersLookupConfig(value, ConfigRoot, $"{Path}{Split}{nameof(Appenders)}");

            value.ParentConfig = this;
        }
    }

    public virtual bool AreEquivalent(IFLoggerMatchedAppenders? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var appendersSame = Appenders.AreEquivalent(other.Appenders, exactTypes);

        var allAreSame = appendersSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IFLoggerMatchedAppenders, true);

    public override int GetHashCode()
    {
        var hashCode = Appenders.GetHashCode();
        return hashCode;
    }
}
