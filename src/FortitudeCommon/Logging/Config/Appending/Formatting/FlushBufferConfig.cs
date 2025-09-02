// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Globalization;
using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending.Formatting;

public interface IFlushBufferConfig : IFLogConfig, IStyledToStringObject, IConfigCloneTo<IFlushBufferConfig>
  , IInterfacesComparable<IFlushBufferConfig>
{
    const decimal DefaultBufferSizeTriggerPercentage = 0.90m;

    static readonly ITimeSpanConfig DefaultWriteTriggeredFlushTimeSpan = new TimeSpanConfig(500);
    static readonly ITimeSpanConfig DefaultAutoTriggeredFlushTimeSpan  = new TimeSpanConfig(seconds: 2);

    decimal WriteTriggeredAtBufferPercentage { get; }

    ITimeSpanConfig WriteTriggeredAfterTimeSpan { get; }

    ITimeSpanConfig AutoTriggeredAfterTimeSpan { get; }

    new IFlushBufferConfig Clone();
}

public interface IMutableFlushBufferConfig : IFlushBufferConfig
{
    new decimal WriteTriggeredAtBufferPercentage { get; set; }

    new ITimeSpanConfig WriteTriggeredAfterTimeSpan { get; set; }

    new ITimeSpanConfig AutoTriggeredAfterTimeSpan { get; set; }

    new IMutableFlushBufferConfig Clone();
}

public class FlushBufferConfig : FLogConfig, IMutableFlushBufferConfig
{
    public FlushBufferConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public FlushBufferConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public FlushBufferConfig
    (decimal writeTriggeredAtBufferPercentage = IFlushBufferConfig.DefaultBufferSizeTriggerPercentage
      , ITimeSpanConfig? writeTriggeredAfterTimeSpan = null
      , ITimeSpanConfig? autoTriggeredAfterTimeSpan = null)
        : this(InMemoryConfigRoot, InMemoryPath, writeTriggeredAtBufferPercentage, writeTriggeredAfterTimeSpan, autoTriggeredAfterTimeSpan) { }

    public FlushBufferConfig
    (IConfigurationRoot root, string path
      , decimal writeTriggeredAtBufferPercentage = IFlushBufferConfig.DefaultBufferSizeTriggerPercentage
      , ITimeSpanConfig? writeTriggeredAfterTimeSpan = null
      , ITimeSpanConfig? autoTriggeredAfterTimeSpan = null)
        : base(root, path)
    {
        WriteTriggeredAtBufferPercentage = writeTriggeredAtBufferPercentage;

        WriteTriggeredAfterTimeSpan = writeTriggeredAfterTimeSpan ?? IFlushBufferConfig.DefaultWriteTriggeredFlushTimeSpan;
        AutoTriggeredAfterTimeSpan  = autoTriggeredAfterTimeSpan ?? IFlushBufferConfig.DefaultAutoTriggeredFlushTimeSpan;
    }

    public FlushBufferConfig(IFlushBufferConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        WriteTriggeredAtBufferPercentage = toClone.WriteTriggeredAtBufferPercentage;

        WriteTriggeredAfterTimeSpan = toClone.WriteTriggeredAfterTimeSpan;
        AutoTriggeredAfterTimeSpan  = toClone.AutoTriggeredAfterTimeSpan;
    }

    public FlushBufferConfig(IFlushBufferConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }


    public decimal WriteTriggeredAtBufferPercentage
    {
        get => decimal.TryParse(this[nameof(WriteTriggeredAtBufferPercentage)], out var timePart) ? timePart : 0m;
        set => this[nameof(WriteTriggeredAtBufferPercentage)] = value.ToString(CultureInfo.InvariantCulture);
    }

    public ITimeSpanConfig WriteTriggeredAfterTimeSpan
    {
        get
        {
            if (GetSection(nameof(WriteTriggeredAfterTimeSpan)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                return new TimeSpanConfig(ConfigRoot, $"{Path}{Split}{nameof(WriteTriggeredAfterTimeSpan)}");
            return new TimeSpanConfig(IFlushBufferConfig.DefaultWriteTriggeredFlushTimeSpan
                                    , ConfigRoot, $"{Path}{Split}{nameof(WriteTriggeredAfterTimeSpan)}");
        }
        set => _ = new TimeSpanConfig(value, ConfigRoot, $"{Path}{Split}{nameof(WriteTriggeredAfterTimeSpan)}");
    }

    public ITimeSpanConfig AutoTriggeredAfterTimeSpan
    {
        get
        {
            if (GetSection(nameof(AutoTriggeredAfterTimeSpan)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                return new TimeSpanConfig(ConfigRoot, $"{Path}{Split}{nameof(AutoTriggeredAfterTimeSpan)}");
            return new TimeSpanConfig(IFlushBufferConfig.DefaultAutoTriggeredFlushTimeSpan
                                    , ConfigRoot, $"{Path}{Split}{nameof(WriteTriggeredAfterTimeSpan)}");
        }
        set => _ = new TimeSpanConfig(value, ConfigRoot, $"{Path}{Split}{nameof(AutoTriggeredAfterTimeSpan)}");
    }

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    IFlushBufferConfig IConfigCloneTo<IFlushBufferConfig>.CloneConfigTo(IConfigurationRoot configRoot, string path) =>
        CloneConfigTo(configRoot, path);

    public FlushBufferConfig CloneConfigTo(IConfigurationRoot configRoot, string path) => new(this, configRoot, path);

    object ICloneable.Clone() => Clone();

    IFlushBufferConfig ICloneable<IFlushBufferConfig>.Clone() => Clone();

    IFlushBufferConfig IFlushBufferConfig.Clone() => Clone();

    IMutableFlushBufferConfig IMutableFlushBufferConfig.Clone() => Clone();

    public virtual FlushBufferConfig Clone() => new(this);

    public virtual bool AreEquivalent(IFlushBufferConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var bufferPercentageSame = WriteTriggeredAtBufferPercentage == other.WriteTriggeredAtBufferPercentage;
        var triggerAfterSame     = WriteTriggeredAfterTimeSpan.AreEquivalent(other.WriteTriggeredAfterTimeSpan, exactTypes);
        var autoAfterSame        = AutoTriggeredAfterTimeSpan.AreEquivalent(other.AutoTriggeredAfterTimeSpan, exactTypes);

        var allAreSame = bufferPercentageSame && triggerAfterSame && autoAfterSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IFlushBufferConfig, true);

    public override int GetHashCode()
    {
        var hashCode = WriteTriggeredAtBufferPercentage.GetHashCode();
        hashCode = (hashCode * 397) ^ WriteTriggeredAfterTimeSpan.GetHashCode();
        hashCode = (hashCode * 397) ^ AutoTriggeredAfterTimeSpan.GetHashCode();
        return hashCode;
    }

    public virtual StyledTypeBuildResult ToString(IStyledTypeStringAppender stsa) =>
        stsa.StartComplexType(this)
           .Field.AlwaysAdd(nameof(WriteTriggeredAtBufferPercentage), WriteTriggeredAtBufferPercentage)
           .Field.AlwaysAdd(nameof(WriteTriggeredAfterTimeSpan), WriteTriggeredAfterTimeSpan)
           .Field.AlwaysAdd(nameof(AutoTriggeredAfterTimeSpan), AutoTriggeredAfterTimeSpan)
           .Complete();
}
