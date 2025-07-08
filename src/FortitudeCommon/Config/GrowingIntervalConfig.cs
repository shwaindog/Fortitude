// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Configuration;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Config;

public enum IntervalExpansionType
{
    None
  , Constant
  , Linear
  , Quadratic
}

public interface IGrowingIntervalConfig : IInterfacesComparable<IGrowingIntervalConfig>, ICloneable<IGrowingIntervalConfig>
{
    ITimeSpanConfig  FirstInterval           { get; set; }
    ITimeSpanConfig  MaxIntervalCap          { get; set; }
    ITimeSpanConfig  IntervalIncrement       { get; set; }
    ITimeSpanConfig? MaxIntervalIncrementCap { get; set; }

    IntervalExpansionType IntervalExpansionType { get; set; }

    TimeSpan GetIntervalForAttempt(int attempt);
}

public class GrowingIntervalConfig: ConfigSection, IGrowingIntervalConfig
{
    public GrowingIntervalConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public GrowingIntervalConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public GrowingIntervalConfig(ITimeSpanConfig  firstInterval, ITimeSpanConfig  intervalIncrement, ITimeSpanConfig  maxIntervalCap
      , IntervalExpansionType intervalExpansionType = IntervalExpansionType.Constant, ITimeSpanConfig? maxIntervalIncrementCap = null)
        : this(InMemoryConfigRoot, InMemoryPath, firstInterval, intervalIncrement, maxIntervalCap, intervalExpansionType, maxIntervalIncrementCap)
    {
    }

    public GrowingIntervalConfig(IConfigurationRoot root, string path, ITimeSpanConfig  firstInterval, ITimeSpanConfig  intervalIncrement
      , ITimeSpanConfig  maxIntervalCap, IntervalExpansionType intervalExpansionType = IntervalExpansionType.Constant
      , ITimeSpanConfig? maxIntervalIncrementCap = null) : base(root, path)
    {
        FirstInterval           = firstInterval;
        IntervalIncrement       = intervalIncrement;
        MaxIntervalCap          = maxIntervalCap;
        MaxIntervalIncrementCap = maxIntervalIncrementCap;
        IntervalExpansionType   = intervalExpansionType;
    }

    public GrowingIntervalConfig(IGrowingIntervalConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        FirstInterval           = toClone.FirstInterval;
        IntervalIncrement       = toClone.IntervalIncrement;
        MaxIntervalCap          = toClone.MaxIntervalCap;
        MaxIntervalIncrementCap = toClone.MaxIntervalIncrementCap;
        IntervalExpansionType   = toClone.IntervalExpansionType;
    }

    public GrowingIntervalConfig(IGrowingIntervalConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public ITimeSpanConfig FirstInterval
    {
        get
        {
            if (GetSection(nameof(FirstInterval)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                var check         = new TimeSpanConfig(ConfigRoot, $"{Path}{Split}{nameof(FirstInterval)}");
                var checkTimeSpan = check.ToTimeSpan();
                if (checkTimeSpan < TimeSpan.Zero)
                {
                    throw new ConfigurationErrorsException($"Expected {nameof(FirstInterval)}: {check} to be a positive TimeSpan");
                }
                var maxTimeSpanConfig = MaxIntervalCap;
                if (checkTimeSpan > maxTimeSpanConfig.ToTimeSpan())
                {
                    throw new ConfigurationErrorsException($"Expected {nameof(FirstInterval)}: {check} to be less than " +
                                                           $"{nameof(MaxIntervalCap)} : {MaxIntervalCap}");
                }
                return check;
            }
            throw new ConfigurationErrorsException($"Expected {nameof(FirstInterval)} to be configured");
        }
        set => _ = new TimeSpanConfig(value, ConfigRoot, $"{Path}{Split}{nameof(FirstInterval)}");
    }

    public ITimeSpanConfig MaxIntervalCap
    {
        get
        {
            if (GetSection(nameof(MaxIntervalCap)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                var check = new TimeSpanConfig(ConfigRoot, $"{Path}{Split}{nameof(MaxIntervalCap)}");

                if (check.ToTimeSpan() < TimeSpan.Zero)
                {
                    throw new ConfigurationErrorsException($"Expected {nameof(MaxIntervalCap)} to be a positive TimeSpan");
                }
                return check;
            }
            throw new ConfigurationErrorsException($"Expected {nameof(MaxIntervalCap)} to be configured");
        }
        set => _ = new TimeSpanConfig(value, ConfigRoot, $"{Path}{Split}{nameof(MaxIntervalCap)}");
    }

    public ITimeSpanConfig IntervalIncrement
    {
        get
        {
            if (GetSection(nameof(IntervalIncrement)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                var check = new TimeSpanConfig(ConfigRoot, $"{Path}{Split}{nameof(IntervalIncrement)}");
                
                var checkTimeSpan = check.ToTimeSpan();
                if (checkTimeSpan < TimeSpan.Zero)
                {
                    throw new ConfigurationErrorsException($"Expected {nameof(IntervalIncrement)} to be a positive TimeSpan");
                }
                if (MaxIntervalIncrementCap is { } intervalMaxCap&& checkTimeSpan > intervalMaxCap.ToTimeSpan())
                {
                    throw new ConfigurationErrorsException($"Expected {nameof(IntervalIncrement)}: {check} to be less than " +
                                                           $"{nameof(MaxIntervalIncrementCap)} : {MaxIntervalIncrementCap}");
                }
                return check;
            }
            throw new ConfigurationErrorsException($"Expected {nameof(IntervalIncrement)} to be configured");
        }
        set => _ = new TimeSpanConfig(value, ConfigRoot, $"{Path}{Split}{nameof(IntervalIncrement)}");
    }

    public ITimeSpanConfig? MaxIntervalIncrementCap
    {
        get
        {
            if (GetSection(nameof(MaxIntervalIncrementCap)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                var check = new TimeSpanConfig(ConfigRoot, $"{Path}{Split}{nameof(MaxIntervalIncrementCap)}");

                if (check.ToTimeSpan() < TimeSpan.Zero)
                {
                    throw new ConfigurationErrorsException($"Expected {nameof(MaxIntervalIncrementCap)} to be a positive TimeSpan");
                }
                return check;
            }
            return null;
        }
        set => _ = value != null ? new TimeSpanConfig(value, ConfigRoot, $"{Path}{Split}{nameof(MaxIntervalIncrementCap)}") : null;
    }

    public IntervalExpansionType IntervalExpansionType
    {
        get =>
            Enum.TryParse<IntervalExpansionType>(this[nameof(IntervalExpansionType)], out var allowedTradingDirection)
                ? allowedTradingDirection
                : IntervalExpansionType.Constant;
        set => this[nameof(IntervalExpansionType)] = value.ToString();
    }

    public virtual TimeSpan GetIntervalForAttempt(int attemptCount)
    {
        var calcInterval   = FirstInterval.ToTimeSpan();
        if (IntervalExpansionType == IntervalExpansionType.None) return calcInterval;
        var nextIncrement  = IntervalIncrement.ToTimeSpan(); 
        var maxIntervalCap = MaxIntervalCap.ToTimeSpan();
        for (int i = 0; i < attemptCount; i++)
        {
            calcInterval = calcInterval.Add(nextIncrement).Max(maxIntervalCap);
            if (calcInterval >= maxIntervalCap) return calcInterval;
            switch (IntervalExpansionType)
            {
                case IntervalExpansionType.Linear:    nextIncrement += IntervalIncrement.ToTimeSpan(); break;
                case IntervalExpansionType.Quadratic: nextIncrement += nextIncrement; break;

                case IntervalExpansionType.Constant:
                default:
                    break;
            }
            if (MaxIntervalIncrementCap != null)
            {
                nextIncrement = nextIncrement.Max(MaxIntervalIncrementCap!.ToTimeSpan());
            }
        }
        return calcInterval;
    }

    object ICloneable.     Clone() => Clone();

    IGrowingIntervalConfig ICloneable<IGrowingIntervalConfig>.Clone() => Clone();

    public virtual GrowingIntervalConfig Clone() => new (this);


    public virtual bool AreEquivalent(IGrowingIntervalConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var firstIntervalSame  = FirstInterval.AreEquivalent(other.FirstInterval, exactTypes);
        var incrementSame      = IntervalIncrement.AreEquivalent(other.IntervalIncrement, exactTypes);
        var maxIntervalCapSame = MaxIntervalCap.AreEquivalent(other.MaxIntervalCap, exactTypes);
        var expansionTypeSame  = IntervalExpansionType == other.IntervalExpansionType;
        var maxIncrementCapSame = MaxIntervalIncrementCap?.AreEquivalent(other.MaxIntervalIncrementCap, exactTypes) ?? other.MaxIntervalIncrementCap == null;

        var allAreSame = firstIntervalSame && incrementSame && maxIntervalCapSame && expansionTypeSame && maxIncrementCapSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IGrowingIntervalConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = FirstInterval.GetHashCode();
            hashCode = (hashCode * 397) ^ IntervalIncrement.GetHashCode();
            hashCode = (hashCode * 397) ^ MaxIntervalCap.GetHashCode();
            hashCode = (hashCode * 397) ^ IntervalExpansionType.GetHashCode();
            hashCode = (hashCode * 397) ^ (MaxIntervalIncrementCap?.GetHashCode() ?? 0);
            return hashCode;
        }
    }

    public virtual string BuildToString()
    {
        var sb = new StringBuilder();
        sb.Append(nameof(FirstInterval)).Append(": ").Append(FirstInterval).Append(", ");
        sb.Append(nameof(IntervalIncrement)).Append(": ").Append(IntervalIncrement).Append(", ");
        sb.Append(nameof(MaxIntervalCap)).Append(": ").Append(MaxIntervalCap).Append(", ");
        sb.Append(nameof(IntervalExpansionType)).Append(": ").Append(IntervalExpansionType).Append(", ");
        if(MaxIntervalIncrementCap != null) sb.Append(nameof(MaxIntervalIncrementCap)).Append(": ").Append(MaxIntervalIncrementCap).Append(", ");
        if (sb.Length > 3)
        {
            sb.Length -= 2;
        }
        return sb.ToString();
    }

    public override string ToString() => $"{nameof(GrowingIntervalConfig)}{{{BuildToString()}}}";
}
