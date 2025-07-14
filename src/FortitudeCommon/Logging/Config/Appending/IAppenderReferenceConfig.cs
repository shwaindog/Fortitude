// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Config;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending;

public interface IAppenderReferenceConfig : ICloneable<IAppenderReferenceConfig>, IInterfacesComparable<IAppenderReferenceConfig>
  , IStyledToStringObject
{
    string?  AppenderName       { get; }

    string? AppenderConfigRef { get; }

    bool DisableHere { get; }

    IAppenderDefinitionConfig? ResolveAppenderDefinition();
}

public interface IMutableAppenderReferenceConfig : IAppenderReferenceConfig
{
    new string?  AppenderName       { get; set; }

    new string? AppenderConfigRef { get; set; }

    new bool DisableHere { get; set; }

    new IMutableAppenderDefinitionConfig? ResolveAppenderDefinition();
}

public class AppenderReferenceConfig: ConfigSection, IMutableAppenderReferenceConfig
{
    public AppenderReferenceConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public AppenderReferenceConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public AppenderReferenceConfig(string? appenderName = null, string? appenderConfigRef = null, bool disableHere = false)
        : this(InMemoryConfigRoot, InMemoryPath, appenderName, appenderConfigRef, disableHere)
    {
    }

    public AppenderReferenceConfig(IConfigurationRoot root, string path, string? appenderName = null, string? appenderConfigRef = null, bool disableHere = false) : base(root, path)
    {
        AppenderName      = appenderName;
        AppenderConfigRef = appenderConfigRef;
        DisableHere       = disableHere;
    }

    public AppenderReferenceConfig(IAppenderReferenceConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        AppenderName      = toClone.AppenderName;
        AppenderConfigRef = toClone.AppenderConfigRef;
        DisableHere       = toClone.DisableHere;
    }

    public AppenderReferenceConfig(IAppenderReferenceConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }
    
    public string? AppenderConfigRef
    {
        get => this[nameof(AppenderConfigRef)];
        set => this[nameof(AppenderConfigRef)] = value;
    }

    public string? AppenderName
    {
        get => this[nameof(AppenderName)];
        set => this[nameof(AppenderName)] = value;
    }

    public bool DisableHere
    {
        get => bool.TryParse(this[nameof(DisableHere)], out var disabled) && disabled;
        set => this[nameof(DisableHere)] = value.ToString();
    }

    IAppenderDefinitionConfig? IAppenderReferenceConfig.ResolveAppenderDefinition() => ResolveAppenderDefinition();

    public IMutableAppenderDefinitionConfig? ResolveAppenderDefinition()
    {
        return null;
    }

    object ICloneable.     Clone() => Clone();

    IAppenderReferenceConfig ICloneable<IAppenderReferenceConfig>.Clone() => Clone();

    public virtual AppenderReferenceConfig Clone() => new (this);

    public virtual bool AreEquivalent(IAppenderReferenceConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var nameSame  = AppenderName == other.AppenderName;
        var refSame      = AppenderConfigRef == other.AppenderConfigRef;
        var disabledSame = DisableHere == other.DisableHere;

        var allAreSame = nameSame && refSame && disabledSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IAppenderReferenceConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = AppenderName?.GetHashCode() ?? 0;
            hashCode = (hashCode * 397) ^ (AppenderConfigRef?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ DisableHere.GetHashCode();
            return hashCode;
        }
    }

    public void ToString(IStyledTypeStringAppender sbc)
    {
        sbc.AddTypeName(nameof(AppenderReferenceConfig))
           .AddTypeStart()
           .AddNonNullOrEmptyField(nameof(AppenderName), AppenderName)
           .AddNonNullOrEmptyField(nameof(AppenderConfigRef), AppenderConfigRef)
           .AddField(nameof(DisableHere), DisableHere)
           .AddTypeEnd();
    }


    public override string ToString() => this.DefaultToString();
}
