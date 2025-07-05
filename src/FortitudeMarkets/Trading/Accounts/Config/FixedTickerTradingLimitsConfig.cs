using FortitudeCommon.Config;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeMarkets.Trading.Accounts.Config;

public interface IFixedTickerTradingLimitsConfig : IInterfacesComparable<IFixedTickerTradingLimitsConfig>, ICloneable<IFixedTickerTradingLimitsConfig>
{
    decimal? MaxFixedTickerOpenPosition { get; set; }

    decimal? MaxFixedTickerOpeningOrderSize { get; set; }

    decimal? MaxFixedTickerReducingOrderSize { get; set; }
}

public class FixedTickerTradingLimitsConfig: ConfigSection, IFixedTickerTradingLimitsConfig
{
    public FixedTickerTradingLimitsConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public FixedTickerTradingLimitsConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public FixedTickerTradingLimitsConfig
    (decimal? maxFixedTickerOpenPosition = null, decimal? maxFixedTickerOpeningOrderSize = null, decimal? maxFixedTickerReducingOrderSize = null)
        : this(InMemoryConfigRoot, InMemoryPath, maxFixedTickerOpenPosition, maxFixedTickerOpeningOrderSize, maxFixedTickerReducingOrderSize)
    {
    }

    public FixedTickerTradingLimitsConfig
    (IConfigurationRoot root, string path, decimal? maxFixedTickerOpenPosition = null, decimal? maxFixedTickerOpeningOrderSize = null
      , decimal? maxFixedTickerReducingOrderSize = null) : this(root, path)
    {
        MaxFixedTickerOpenPosition      = maxFixedTickerOpenPosition;
        MaxFixedTickerOpeningOrderSize  = maxFixedTickerOpeningOrderSize;
        MaxFixedTickerReducingOrderSize = maxFixedTickerReducingOrderSize;
    }

    public FixedTickerTradingLimitsConfig(IFixedTickerTradingLimitsConfig toClone, IConfigurationRoot root, string path) : this(root, path)
    {
        MaxFixedTickerOpenPosition      = toClone.MaxFixedTickerOpenPosition;
        MaxFixedTickerOpeningOrderSize  = toClone.MaxFixedTickerOpeningOrderSize;
        MaxFixedTickerReducingOrderSize = toClone.MaxFixedTickerReducingOrderSize;
    }

    public FixedTickerTradingLimitsConfig(IFixedTickerTradingLimitsConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public decimal? MaxFixedTickerOpenPosition
    {
        get => decimal.TryParse(this[nameof(MaxFixedTickerOpenPosition)], out var fixedTickerOpenPosition) ? fixedTickerOpenPosition : null;
        set => this[nameof(MaxFixedTickerOpenPosition)] = value.ToString();
    }

    public decimal? MaxFixedTickerOpeningOrderSize
    {
        get => decimal.TryParse(this[nameof(MaxFixedTickerOpeningOrderSize)], out var fixedTickerOpeningOrderSize) ? fixedTickerOpeningOrderSize : null;
        set => this[nameof(MaxFixedTickerOpeningOrderSize)] = value.ToString();
    }

    public decimal? MaxFixedTickerReducingOrderSize
    {
        get => decimal.TryParse(this[nameof(MaxFixedTickerReducingOrderSize)], out var fixedTickerReducingOrderSize) ? fixedTickerReducingOrderSize : null;
        set => this[nameof(MaxFixedTickerReducingOrderSize)] = value.ToString();
    }

    object ICloneable.Clone() => Clone();

    IFixedTickerTradingLimitsConfig ICloneable<IFixedTickerTradingLimitsConfig>.Clone() => Clone();

    public virtual FixedTickerTradingLimitsConfig Clone() => new(this);

    public virtual bool AreEquivalent(IFixedTickerTradingLimitsConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var maxFixedTickerOpenPosSame       = MaxFixedTickerOpenPosition == other.MaxFixedTickerOpenPosition;
        var maxFixedTickerOpenOrderSzSame   = MaxFixedTickerOpeningOrderSize == other.MaxFixedTickerOpeningOrderSize;
        var maxFixedTickerReduceOrderSzSame = MaxFixedTickerReducingOrderSize == other.MaxFixedTickerReducingOrderSize;

        var allAreSame = maxFixedTickerOpenPosSame && maxFixedTickerOpenOrderSzSame && maxFixedTickerReduceOrderSzSame;

        return allAreSame;
    }

    public static void ClearValues(IConfigurationRoot root, string path)
    {
        root[$"{path}{Split}{nameof(MaxFixedTickerOpenPosition)}"]      = null;
        root[$"{path}{Split}{nameof(MaxFixedTickerOpeningOrderSize)}"]  = null;
        root[$"{path}{Split}{nameof(MaxFixedTickerReducingOrderSize)}"] = null;
    }

    protected bool Equals(IFixedTickerTradingLimitsConfig other) => AreEquivalent(other, true);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IFixedTickerTradingLimitsConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = MaxFixedTickerOpenPosition?.GetHashCode() ?? 0;
            hashCode = (hashCode * 397) ^ (MaxFixedTickerOpeningOrderSize?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ (MaxFixedTickerReducingOrderSize?.GetHashCode() ?? 0);
            return hashCode;
        }
    }

    public string FixedTickerTradingLimitsConfigToStringMembers =>
        $"{nameof(MaxFixedTickerOpenPosition)}: {MaxFixedTickerOpenPosition}, {nameof(MaxFixedTickerOpeningOrderSize)}: {MaxFixedTickerOpeningOrderSize}, " +
        $"{nameof(MaxFixedTickerReducingOrderSize)}: {MaxFixedTickerReducingOrderSize}";

    public override string ToString() => $"{nameof(FixedTickerTradingLimitsConfig)}{{{FixedTickerTradingLimitsConfigToStringMembers}}}";
}
