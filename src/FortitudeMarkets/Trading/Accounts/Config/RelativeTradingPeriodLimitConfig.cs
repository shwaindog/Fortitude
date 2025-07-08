// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeMarkets.Trading.Accounts.Config;

public interface IRelativeTradingPeriodLimitConfig : IFixedPeriodLimitConfig, ICloneable<IRelativeTradingPeriodLimitConfig>
{
    decimal? AlertAccountSizePercentageLimit { get; set; }

    decimal? BreachAccountSizePercentageLimit { get; set; }

    new IRelativeTradingPeriodLimitConfig Clone();
}

public class RelativeTradingPeriodLimitConfig : FixedPeriodLimitConfig, IRelativeTradingPeriodLimitConfig
{
    public RelativeTradingPeriodLimitConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public RelativeTradingPeriodLimitConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public RelativeTradingPeriodLimitConfig
    (decimal? breachAccountSizePercentageLimit = null, decimal? breachFixedLimit = null, decimal? alertAccountSizePercentageLimit = null
      , decimal? alertFixedLimit = null, TimeBoundaryPeriod period = TimeBoundaryPeriod.Any)
        : this(InMemoryConfigRoot, InMemoryPath, breachAccountSizePercentageLimit, breachFixedLimit, alertAccountSizePercentageLimit, alertFixedLimit, period)
    {
    }

    public RelativeTradingPeriodLimitConfig
    (IConfigurationRoot root, string path, decimal? breachAccountSizePercentageLimit = null, decimal? breachFixedLimit = null
      , decimal? alertAccountSizePercentageLimit = null, decimal? alertFixedLimit = null, TimeBoundaryPeriod? period = null)
        : base(root, path, breachFixedLimit, alertFixedLimit, period)
    {
        BreachAccountSizePercentageLimit = breachAccountSizePercentageLimit;
        AlertAccountSizePercentageLimit  = alertAccountSizePercentageLimit;
    }

    public RelativeTradingPeriodLimitConfig(IRelativeTradingPeriodLimitConfig toClone, IConfigurationRoot root, string path) 
        : base(toClone, root, path)
    {
        BreachAccountSizePercentageLimit = toClone.BreachAccountSizePercentageLimit;
        AlertAccountSizePercentageLimit  = toClone.AlertAccountSizePercentageLimit;
    }

    public RelativeTradingPeriodLimitConfig(IRelativeTradingPeriodLimitConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public decimal? AlertAccountSizePercentageLimit
    {
        get =>
            decimal.TryParse(this[nameof(AlertAccountSizePercentageLimit)], out var fixedTickerReducingOrderSize)
                ? fixedTickerReducingOrderSize
                : null;
        set => this[nameof(AlertAccountSizePercentageLimit)] = value.ToString();
    }

    public decimal? BreachAccountSizePercentageLimit
    {
        get =>
            decimal.TryParse(this[nameof(BreachAccountSizePercentageLimit)], out var fixedTickerReducingOrderSize)
                ? fixedTickerReducingOrderSize
                : null;
        set => this[nameof(BreachAccountSizePercentageLimit)] = value.ToString();
    }

    object ICloneable.Clone() => Clone();

    IRelativeTradingPeriodLimitConfig ICloneable<IRelativeTradingPeriodLimitConfig>.Clone() => Clone();

    IFixedPeriodLimitConfig ICloneable<IFixedPeriodLimitConfig>.Clone() => Clone();

    IRelativeTradingPeriodLimitConfig IRelativeTradingPeriodLimitConfig.Clone() => Clone();

    public override RelativeTradingPeriodLimitConfig Clone() => new(this);

    public override bool AreEquivalent(IFixedPeriodLimitConfig? other, bool exactTypes = false)
    {
        if (other is not IRelativeTradingPeriodLimitConfig relativeTradingPeriodLimit) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var breachAcctSzPctLimitSame = BreachAccountSizePercentageLimit == relativeTradingPeriodLimit.BreachAccountSizePercentageLimit;
        var alertAcctSzPctLimitSame  = AlertAccountSizePercentageLimit == relativeTradingPeriodLimit.AlertAccountSizePercentageLimit;

        var allAreSame = baseSame && breachAcctSzPctLimitSame && alertAcctSzPctLimitSame;

        return allAreSame;
    }

    public new static void ClearValues(IConfigurationRoot root, string path)
    {
        root[$"{path}{Split}{nameof(BreachAccountSizePercentageLimit)}"] = null;
        root[$"{path}{Split}{nameof(AlertAccountSizePercentageLimit)}"]  = null;
        FixedPeriodLimitConfig.ClearValues(root, path);
    }

    protected bool Equals(IRelativeTradingPeriodLimitConfig other) => AreEquivalent(other, true);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IRelativeTradingPeriodLimitConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ (BreachAccountSizePercentageLimit?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ (AlertAccountSizePercentageLimit?.GetHashCode() ?? 0);
            return hashCode;
        }
    }

    protected string RelativeTradingPeriodLimitConfigToStringMembers =>
        $"{nameof(BreachAccountSizePercentageLimit)}: {BreachAccountSizePercentageLimit}, {nameof(AlertAccountSizePercentageLimit)}: {AlertAccountSizePercentageLimit}, " +
        $"{nameof(FixedPeriodLimitConfigToStringMembers)}";

    public override string ToString() => $"{nameof(RelativeTradingPeriodLimitConfig)}{{{RelativeTradingPeriodLimitConfigToStringMembers}}}";
}
