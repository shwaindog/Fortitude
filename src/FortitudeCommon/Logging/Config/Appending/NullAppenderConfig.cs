// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config.Appending.Forwarding;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;
using static FortitudeCommon.Logging.Config.Appending.FLoggerBuiltinAppenderType;

namespace FortitudeCommon.Logging.Config.Appending;


public interface INullAppenderConfig : IAppenderDefinitionConfig
{
    const string NullAppenderName = "NullAppender";
    const string NullAppenderType = $"{nameof(NullEntry)}";
}

public class NullAppenderConfig : AppenderDefinitionConfig, INullAppenderConfig
{
    private const string NullAppenderType = $"{nameof(NullEntry)}";

    public NullAppenderConfig(IConfigurationRoot root, string path) 
        : base(root, path, INullAppenderConfig.NullAppenderName, INullAppenderConfig.NullAppenderType ) { }

    public NullAppenderConfig() : this(InMemoryConfigRoot, InMemoryPath) { }
    
    public NullAppenderConfig(INullAppenderConfig toClone, IConfigurationRoot root, string path) : base(toClone, root, path)
    {
    }

    public NullAppenderConfig(INullAppenderConfig toClone) : this(toClone, InMemoryConfigRoot, toClone.ConfigSubPath) { }

    public static NullAppenderConfig SingletonInstance = new ();

    public override string AppenderType
    {
        get => this[nameof(AppenderType)] ?? NullAppenderType;
        set => this[nameof(AppenderType)] = value;
    }

    public override T Accept<T>(T visitor) => visitor.Visit(this);

    public override NullAppenderConfig Clone() => new (this);

    public override bool AreEquivalent(IAppenderReferenceConfig? other, bool exactTypes = false)
    {
        if (other is not INullAppenderConfig) return false;
        return true;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IForwardingAppenderConfig, true);

    public override int GetHashCode()
    {
        var hashCode = base.GetHashCode();
        return hashCode;
    }

    public override StyledTypeBuildResult ToString(IStyledTypeStringAppender stsa) =>
        stsa.StartComplexType(this)
           .AddBaseStyledToStringFields(this).Complete();
}
