// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Config;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending;

public interface IAppenderReferenceConfig : IInterfacesComparable<IAppenderReferenceConfig>
  , IStyledToStringObject, IConfigCloneTo<IAppenderReferenceConfig>, IFLogConfig
{
    string AppenderName { get; }

    // can have the values FLoggerBuiltinAppenderType or a .net fully qualified assembly and type namespace
    string AppenderType { get; }

    bool DeactivateHere { get; }

    IAppenderDefinitionConfig? ResolveAppenderDefinition();

    new IAppenderReferenceConfig Clone();
}

public interface IMutableAppenderReferenceConfig : IAppenderReferenceConfig, IMutableFLogConfig
{
    new string AppenderName { get; set; }

    // can have the values FLoggerBuiltinAppenderType or a .net fully qualified assembly and type namespace
    new string AppenderType { get; set; }

    new bool DeactivateHere { get; set; }

    new IMutableAppenderDefinitionConfig? ResolveAppenderDefinition();
}

public class AppenderReferenceConfig : FLogConfig, IMutableAppenderReferenceConfig
{
    public AppenderReferenceConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public AppenderReferenceConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public AppenderReferenceConfig(string appenderName, string appenderType, bool deactivateHere = false)
        : this(InMemoryConfigRoot, InMemoryPath, appenderName, appenderType, deactivateHere) { }

    public AppenderReferenceConfig(string appenderName, bool deactivateHere = false)
        : this(InMemoryConfigRoot, InMemoryPath, appenderName, deactivateHere) { }

    public AppenderReferenceConfig
        (IConfigurationRoot root, string path, string appenderName, string appenderType, bool deactivateHere = false) : base(root, path)
    {
        AppenderName = appenderName;
        AppenderType = appenderType;
        DeactivateHere  = deactivateHere;
    }

    public AppenderReferenceConfig
        (IConfigurationRoot root, string path, string appenderName, bool deactivateHere = false) : base(root, path)
    {
        AppenderName = appenderName;
        DeactivateHere  = deactivateHere;
    }

    public AppenderReferenceConfig(IAppenderReferenceConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        AppenderName = toClone.AppenderName;
        AppenderType = toClone.AppenderType;
        DeactivateHere  = toClone.DeactivateHere;
    }

    public AppenderReferenceConfig(IAppenderReferenceConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public string AppenderName
    {
        get => this[nameof(AppenderName)]!;
        set => this[nameof(AppenderName)] = value;
    }

    public virtual string AppenderType
    {
        get => this[nameof(AppenderType)] ?? $"{nameof(FLoggerBuiltinAppenderType.Ref)}";
        set => this[nameof(AppenderType)] = value;
    }

    public bool DeactivateHere
    {
        get => bool.TryParse(this[nameof(DeactivateHere)], out var disabled) && disabled;
        set => this[nameof(DeactivateHere)] = value.ToString();
    }

    IAppenderDefinitionConfig? IAppenderReferenceConfig.ResolveAppenderDefinition() => ResolveAppenderDefinition();

    public virtual IMutableAppenderDefinitionConfig? ResolveAppenderDefinition()
    {
        return null;
    }

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    object ICloneable.Clone() => Clone();

    IAppenderReferenceConfig ICloneable<IAppenderReferenceConfig>.Clone() => Clone();

    IAppenderReferenceConfig IAppenderReferenceConfig.Clone() => Clone();

    public virtual AppenderReferenceConfig Clone() => new(this);

    public virtual IAppenderReferenceConfig CloneConfigTo(IConfigurationRoot configRoot, string path)
    {
        return new AppenderReferenceConfig(this, configRoot, path);
    }

    public virtual bool AreEquivalent(IAppenderReferenceConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var nameSame     = AppenderName == other.AppenderName;
        var typeSame     = AppenderType == other.AppenderType;
        var disabledSame = DeactivateHere == other.DeactivateHere;

        var allAreSame = nameSame && typeSame  && disabledSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IAppenderReferenceConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = AppenderName.GetHashCode();
            hashCode = (hashCode * 397) ^ AppenderType.GetHashCode();
            hashCode = (hashCode * 397) ^ DeactivateHere.GetHashCode();
            return hashCode;
        }
    }

    public virtual StyledTypeBuildResult ToString(IStyledTypeStringAppender sbc)
    {
        return
            sbc.StartComplexType(nameof(AppenderReferenceConfig))
               .Field.AddWhenNonNullOrDefault(nameof(AppenderName), AppenderName)
               .Field.AddWhenNonNullOrDefault(nameof(AppenderType), AppenderType)
               .Field.AddWhenNonDefault(nameof(DeactivateHere), DeactivateHere)
               .Complete();
    }

    public override string ToString() => this.DefaultToString();
}
