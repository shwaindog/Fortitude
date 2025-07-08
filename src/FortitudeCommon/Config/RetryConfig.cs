using System.Text;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Config;


public interface IRetryConfig : IGrowingIntervalConfig, ICloneable<IRetryConfig>
{
    uint RetryAttempts { get; set; }
    uint? BackgroundRetryAttempts        { get; set; }
    bool EnableBackgroundRetry { get; set; }

    new IRetryConfig Clone();
}

public class RetryConfig: GrowingIntervalConfig, IRetryConfig
{
    public RetryConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public RetryConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public RetryConfig(uint retryAttempts, ITimeSpanConfig  firstInterval, ITimeSpanConfig  intervalIncrement, ITimeSpanConfig  maxIntervalCap
      , IntervalExpansionType intervalExpansionType = IntervalExpansionType.Constant, uint? backgroundRetries = null, bool enableBackgroundRetry = false
      , ITimeSpanConfig? maxIntervalIncrementCap = null)
        : this(InMemoryConfigRoot, InMemoryPath, retryAttempts, firstInterval, intervalIncrement, maxIntervalCap, intervalExpansionType
             , backgroundRetries, enableBackgroundRetry, maxIntervalIncrementCap)
    {
    }

    public RetryConfig(IConfigurationRoot root, string path, uint retryAttempts, ITimeSpanConfig  firstInterval, ITimeSpanConfig  intervalIncrement
      , ITimeSpanConfig  maxIntervalCap, IntervalExpansionType intervalExpansionType = IntervalExpansionType.Constant, uint? backgroundRetries = null
      , bool enableBackgroundRetry = false, ITimeSpanConfig? maxIntervalIncrementCap = null) 
        : base(root, path, firstInterval, intervalIncrement, maxIntervalCap, intervalExpansionType, maxIntervalIncrementCap)
    {
        RetryAttempts         = retryAttempts;
        BackgroundRetryAttempts     = backgroundRetries;
        EnableBackgroundRetry = enableBackgroundRetry;
    }

    public RetryConfig(IRetryConfig toClone, IConfigurationRoot root, string path) : base(toClone, root, path)
    {
        RetryAttempts         = toClone.RetryAttempts;
        BackgroundRetryAttempts     = toClone.BackgroundRetryAttempts;
        EnableBackgroundRetry = toClone.EnableBackgroundRetry;
    }

    public RetryConfig(IRetryConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }
    
    public uint RetryAttempts
    {
        get => uint.TryParse(this[nameof(RetryAttempts)], out var timePart) ? timePart : 0;
        set => this[nameof(RetryAttempts)] = value.ToString();
    }

    public uint? BackgroundRetryAttempts
    {
        get => uint.TryParse(this[nameof(BackgroundRetryAttempts)], out var timePart) ? timePart : null;
        set => this[nameof(BackgroundRetryAttempts)] = value.ToString();
    }

    public bool EnableBackgroundRetry
    {
        get => bool.TryParse(this[nameof(EnableBackgroundRetry)], out var alwaysSubmitWithStopLoss) && alwaysSubmitWithStopLoss;
        set => this[nameof(EnableBackgroundRetry)] = value.ToString();
    }

    object ICloneable.     Clone() => Clone();

    IRetryConfig ICloneable<IRetryConfig>.Clone() => Clone();

    IRetryConfig IRetryConfig.Clone() => Clone();

    public override RetryConfig Clone() => new (this);


    public override bool AreEquivalent(IGrowingIntervalConfig? other, bool exactTypes = false)
    {
        if (other is not IRetryConfig retryConfig) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var retryAttemptsSame  = RetryAttempts == retryConfig.RetryAttempts;
        var backgroundRetriesSame  = BackgroundRetryAttempts == retryConfig.BackgroundRetryAttempts;
        var enableBackgroundRetrySame  = EnableBackgroundRetry == retryConfig.EnableBackgroundRetry;

        var allAreSame = baseSame && retryAttemptsSame && backgroundRetriesSame && enableBackgroundRetrySame;

        return allAreSame;
    }

    
    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IRetryConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ RetryAttempts.GetHashCode();
            hashCode = (hashCode * 397) ^ EnableBackgroundRetry.GetHashCode();
            hashCode = (hashCode * 397) ^ (BackgroundRetryAttempts?.GetHashCode() ?? 0);
            return hashCode;
        }
    }

    public override string BuildToString()
    {
        var sb = new StringBuilder();
        sb.Append(nameof(RetryAttempts)).Append(": ").Append(RetryAttempts).Append(", ");
        sb.Append(base.BuildToString()).Append(", ");
        sb.Append(nameof(BackgroundRetryAttempts)).Append(": ").Append(BackgroundRetryAttempts).Append(", ");
        sb.Append(nameof(EnableBackgroundRetry)).Append(": ").Append(EnableBackgroundRetry);
        return sb.ToString();
    }

    public override string ToString() => $"{nameof(RetryConfig)}{{{BuildToString()}}}";
}

