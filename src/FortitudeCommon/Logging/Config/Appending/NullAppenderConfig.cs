// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config.Appending.Forwarding;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending;


public interface INullAppenderConfig : IAppenderDefinitionConfig
{
    const string NullAppenderName = "NullAppender";
    const string NullAppenderType = $"{nameof(FLoggerBuiltinAppenderType.Null)}";
}

public class NullAppenderConfig : AppenderDefinitionConfig, INullAppenderConfig
{
    private const string SyncForwardingAppenderType = $"{nameof(FLoggerBuiltinAppenderType.SyncForwarding)}";

    private IAppendableForwardingAppendersLookupConfig?  appendersConfig;

    public NullAppenderConfig(IConfigurationRoot root, string path) 
        : base(root, path, INullAppenderConfig.NullAppenderName, INullAppenderConfig.NullAppenderType ) { }

    public NullAppenderConfig() : this(InMemoryConfigRoot, InMemoryPath) { }
    
    public NullAppenderConfig(INullAppenderConfig toClone, IConfigurationRoot root, string path) : base(toClone, root, path)
    {
    }

    public NullAppenderConfig(INullAppenderConfig toClone) : this(toClone, InMemoryConfigRoot, toClone.ConfigSubPath) { }

    public override string AppenderType
    {
        get => this[nameof(AppenderType)] ?? SyncForwardingAppenderType;
        set => this[nameof(AppenderType)] = value;
    }

    public override T Visit<T>(T visitor) => visitor.Accept(this);

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

    public override IStyledTypeStringAppender ToString(IStyledTypeStringAppender sbc)
    {
        sbc.AddTypeName(nameof(NullAppenderConfig))
           .AddTypeStart()
           .AddBaseFieldsStart();
        base.ToString(sbc)
            .AddBaseFieldsEnd()
            .AddTypeEnd();
        return sbc;
    }
}
