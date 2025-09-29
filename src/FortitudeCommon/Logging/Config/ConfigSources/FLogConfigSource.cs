// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.ConfigSources;

public interface IFlogConfigSource : IConfigCloneTo<IFlogConfigSource>, IInterfacesComparable<IFlogConfigSource>
  , IStringBearer, IFLogConfig
{
    static readonly TimeSpanConfig DefaultRecheckConfigTimeSpan = new(days: 1);

    ushort ConfigPriorityOrder { get; } // high Priority overrides lower priority

    FLogConfigSourceType SourceType { get; }

    bool Optional { get; }

    string? ConfigSourceName { get; }

    TimeSpanConfig RecheckConfigIntervalTimeSpan { get; }
}

public interface IMutableFlogConfigSource : IFlogConfigSource, IMutableFLogConfig
{
    new ushort ConfigPriorityOrder { get; set; } // high Priority overrides lower priority

    new FLogConfigSourceType SourceType { get; set; }

    new bool Optional { get; set; }

    new string? ConfigSourceName { get; set; }

    new TimeSpanConfig RecheckConfigIntervalTimeSpan { get; set; }
}

public abstract class FLogConfigSource : FLogConfig, IMutableFlogConfigSource
{
    protected FLogConfigSource(IConfigurationRoot root, string path) : base(root, path) { }

    protected FLogConfigSource() : this(InMemoryConfigRoot, InMemoryPath) { }

    protected FLogConfigSource
    (ushort configPriorityOrder, FLogConfigSourceType sourceType, bool optional = false
      , string configSourceName = "", TimeSpanConfig? recheckConfigIntervalTimeSpan = null)
        : this(InMemoryConfigRoot, InMemoryPath, configPriorityOrder, sourceType, optional, configSourceName, recheckConfigIntervalTimeSpan) { }

    protected FLogConfigSource
    (IConfigurationRoot root, string path, ushort configPriorityOrder, FLogConfigSourceType sourceType, bool optional = false
      , string? configSourceName = null, TimeSpanConfig? recheckConfigIntervalTimeSpan = null) : base(root, path)
    {
        ConfigPriorityOrder = configPriorityOrder;
        SourceType          = sourceType;
        Optional            = optional;
        ConfigSourceName    = configSourceName;
        if (recheckConfigIntervalTimeSpan != null) RecheckConfigIntervalTimeSpan = recheckConfigIntervalTimeSpan;
    }

    protected FLogConfigSource(IFlogConfigSource toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        ConfigPriorityOrder = toClone.ConfigPriorityOrder;
        ConfigSourceName    = toClone.ConfigSourceName;

        RecheckConfigIntervalTimeSpan = toClone.RecheckConfigIntervalTimeSpan;

        Optional   = toClone.Optional;
        SourceType = toClone.SourceType;
    }

    protected FLogConfigSource(IFlogConfigSource toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public ushort ConfigPriorityOrder
    {
        get => ushort.TryParse(this[nameof(ConfigPriorityOrder)], out var priorityOrder) ? priorityOrder : (ushort)0;
        set => this[nameof(ConfigPriorityOrder)] = value.ToString();
    }

    public string? ConfigSourceName
    {
        get => this[nameof(ConfigSourceName)];
        set => this[nameof(ConfigSourceName)] = value;
    }

    public bool Optional
    {
        get => bool.TryParse(this[nameof(Optional)], out var optional) && optional;
        set => this[nameof(Optional)] = value.ToString();
    }

    public TimeSpanConfig RecheckConfigIntervalTimeSpan
    {
        get
        {
            if (GetSection(nameof(RecheckConfigIntervalTimeSpan)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                return new TimeSpanConfig(ConfigRoot, $"{Path}{Split}{nameof(RecheckConfigIntervalTimeSpan)}");
            return IFlogConfigSource.DefaultRecheckConfigTimeSpan;
        }
        set => _ = new TimeSpanConfig(value, ConfigRoot, $"{Path}{Split}{nameof(RecheckConfigIntervalTimeSpan)}");
    }

    public FLogConfigSourceType SourceType
    {
        get =>
            Enum.TryParse<FLogConfigSourceType>(this[nameof(SourceType)], out var poolScope)
                ? poolScope
                : FLogConfigSourceType.Unknown;
        set => this[nameof(SourceType)] = value.ToString();
    }

    public abstract IFlogConfigSource CloneConfigTo(IConfigurationRoot configRoot, string path);

    object ICloneable.Clone() => Clone();

    IFlogConfigSource ICloneable<IFlogConfigSource>.Clone() => Clone();

    public abstract FLogConfigSource Clone();

    public virtual bool AreEquivalent(IFlogConfigSource? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var orderSame           = ConfigPriorityOrder == other.ConfigPriorityOrder;
        var srcNameSame         = ConfigSourceName == other.ConfigSourceName;
        var optionalSame        = Optional == other.Optional;
        var recheckIntervalSame = Equals(RecheckConfigIntervalTimeSpan, other.RecheckConfigIntervalTimeSpan);
        var srcTypeSame         = SourceType == other.SourceType;

        var allAreSame = orderSame && srcNameSame && optionalSame && recheckIntervalSame && srcTypeSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IFlogConfigSource, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = ConfigPriorityOrder.GetHashCode();
            hashCode = (hashCode * 397) ^ (ConfigSourceName?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ Optional.GetHashCode();
            hashCode = (hashCode * 397) ^ RecheckConfigIntervalTimeSpan.GetHashCode();
            hashCode = (hashCode * 397) ^ SourceType.GetHashCode();
            return hashCode;
        }
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(ConfigPriorityOrder), ConfigPriorityOrder)
           .Field.AlwaysAdd(nameof(ConfigSourceName), ConfigSourceName)
           .Field.AlwaysAdd(nameof(Optional), Optional)
           .Field.AlwaysAdd(nameof(SourceType), SourceType.ToString())
           .Field.AlwaysReveal(nameof(RecheckConfigIntervalTimeSpan), RecheckConfigIntervalTimeSpan)
           .Complete();

    public override string ToString() => this.DefaultToString();
}
