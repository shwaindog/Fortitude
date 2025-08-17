// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Configuration;
using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding;


public interface IForwardingAppenderConfig : IAppenderDefinitionConfig, IConfigCloneTo<IForwardingAppenderConfig>
{
    INamedAppendersLookupConfig ForwardToAppenders { get; }

    new IForwardingAppenderConfig CloneConfigTo(IConfigurationRoot configRoot, string path);

    new IForwardingAppenderConfig Clone();
}

public interface IMutableForwardingAppenderConfig : IForwardingAppenderConfig, IMutableAppenderDefinitionConfig
{
    new IAppendableNamedAppendersLookupConfig ForwardToAppenders { get; set; }
}

public class ForwardingAppenderConfig : AppenderDefinitionConfig, IMutableForwardingAppenderConfig
{
    private const string SyncForwardingAppenderType = $"{nameof(FLoggerBuiltinAppenderType.Forwarding)}";

    private IAppendableNamedAppendersLookupConfig?  appendersConfig;

    public ForwardingAppenderConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public ForwardingAppenderConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public ForwardingAppenderConfig (string appenderName, IAppendableNamedAppendersLookupConfig? forwardToAppendersConfig
       , int runOnAsyncQueueNumber = 0, string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false
      , bool deactivateHere = false)
        : this(InMemoryConfigRoot, InMemoryPath, appenderName, forwardToAppendersConfig, runOnAsyncQueueNumber
             , inheritFromAppenderName, isTemplateOnlyDefinition, deactivateHere) { }

    public ForwardingAppenderConfig(IConfigurationRoot root, string path, string appenderName
      , IAppendableNamedAppendersLookupConfig? forwardToAppendersConfig = null, int runOnAsyncQueueNumber = 0
      , string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false, bool deactivateHere = false) 
        : base(root, path, appenderName, runOnAsyncQueueNumber, inheritFromAppenderName, isTemplateOnlyDefinition, deactivateHere)
    {
        ForwardToAppenders     = forwardToAppendersConfig ?? new NamedAppendersLookupConfig();
    }

    public ForwardingAppenderConfig(IForwardingAppenderConfig toClone, IConfigurationRoot root, string path) : base(toClone, root, path)
    {
        ForwardToAppenders     = (IAppendableNamedAppendersLookupConfig)toClone.ForwardToAppenders;
    }

    public ForwardingAppenderConfig(IForwardingAppenderConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public override string AppenderType
    {
        get => this[nameof(AppenderType)] ?? SyncForwardingAppenderType;
        set => this[nameof(AppenderType)] = value;
    }

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    IForwardingAppenderConfig IForwardingAppenderConfig.Clone() => Clone();

    IForwardingAppenderConfig IForwardingAppenderConfig.CloneConfigTo(IConfigurationRoot configRoot, string path) => 
        CloneConfigTo(configRoot, path);

    public override ForwardingAppenderConfig Clone() => new (this);

    IForwardingAppenderConfig ICloneable<IForwardingAppenderConfig>.Clone() => Clone();

    IForwardingAppenderConfig IConfigCloneTo<IForwardingAppenderConfig>.
        CloneConfigTo(IConfigurationRoot configRoot, string path) => 
        CloneConfigTo(configRoot, path);

    public override ForwardingAppenderConfig CloneConfigTo(IConfigurationRoot configRoot, string path) => 
        new (this, configRoot, path);

    INamedAppendersLookupConfig IForwardingAppenderConfig.ForwardToAppenders => ForwardToAppenders;

    public IAppendableNamedAppendersLookupConfig ForwardToAppenders
    {
        get
        {
            if (appendersConfig == null)
            {
                if (GetSection(nameof(ForwardToAppenders)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                {
                    return appendersConfig = new NamedAppendersLookupConfig(ConfigRoot, $"{Path}{Split}{nameof(ForwardToAppenders)}")
                    {
                        ParentConfig = this
                    };
                }
            }
            return appendersConfig ?? throw new ConfigurationErrorsException($"Expected {nameof(ForwardToAppenders)} to be configured");
        }
        set
        {
            appendersConfig = new NamedAppendersLookupConfig(value, ConfigRoot, $"{Path}{Split}{nameof(ForwardToAppenders)}");

            appendersConfig.ParentConfig = this;
        }
    }

    public override bool AreEquivalent(IAppenderReferenceConfig? other, bool exactTypes = false)
    {
        if (other is not IForwardingAppenderConfig forwardingAppenderConfig) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var appendersSame  = ForwardToAppenders.AreEquivalent(forwardingAppenderConfig.ForwardToAppenders, exactTypes);

        var allAreSame = baseSame && appendersSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IForwardingAppenderConfig, true);

    public override int GetHashCode()
    {
        var hashCode = base.GetHashCode();
        hashCode = (hashCode * 397) ^ ForwardToAppenders.GetHashCode();
        return hashCode;
    }

    public override StyledTypeBuildResult ToString(IStyledTypeStringAppender sbc)
    {
        using var tb = sbc.StartComplexType(nameof(ForwardingAppenderConfig))
           .AddBaseFieldsStart();
        base.ToString(sbc);
        tb.Field.AlwaysAdd(nameof(ForwardToAppenders), ForwardToAppenders);
        return tb;
    }
}
