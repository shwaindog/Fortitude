// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Chronometry;
using FortitudeCommon.Config;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeMarkets.Trading.Accounts.Config;

public interface IFixedPeriodLimitConfig : IInterfacesComparable<IFixedPeriodLimitConfig>, ICloneable<IFixedPeriodLimitConfig>
{
    TimeBoundaryPeriod? Period { get; set; }

    decimal? AlertFixedLimit { get; set; }

    decimal? BreachFixedLimit { get; set; }
}

public class FixedPeriodLimitConfig : ConfigSection, IFixedPeriodLimitConfig
{
    public FixedPeriodLimitConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public FixedPeriodLimitConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public FixedPeriodLimitConfig
        (decimal? breachFixedLimit = null, decimal? alertFixedLimit = null, TimeBoundaryPeriod period = TimeBoundaryPeriod.Any)
        : this(InMemoryConfigRoot, InMemoryPath, breachFixedLimit, alertFixedLimit, period)
    {
    }

    public FixedPeriodLimitConfig
        (IConfigurationRoot root, string path, decimal? breachFixedLimit = null, decimal? alertFixedLimit = null, TimeBoundaryPeriod? period = null)
        : base(root, path)
    {
        BreachFixedLimit = breachFixedLimit;
        AlertFixedLimit  = alertFixedLimit;
        Period           = period;
    }

    public FixedPeriodLimitConfig(IFixedPeriodLimitConfig toClone, IConfigurationRoot root, string path) : this(root, path)
    {
        BreachFixedLimit = toClone.BreachFixedLimit;
        AlertFixedLimit  = toClone.AlertFixedLimit;
        Period           = toClone.Period;
    }

    public FixedPeriodLimitConfig(IFixedPeriodLimitConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public TimeBoundaryPeriod? Period
    {
        get => Enum.TryParse<TimeBoundaryPeriod>(this[nameof(Period)], out var fixedTickerOpenPosition) ? fixedTickerOpenPosition : null;
        set => this[nameof(Period)] = value?.ToString() ?? null;
    }

    public decimal? AlertFixedLimit
    {
        get => decimal.TryParse(this[nameof(AlertFixedLimit)], out var fixedTickerOpeningOrderSize) ? fixedTickerOpeningOrderSize : null;
        set => this[nameof(AlertFixedLimit)] = value.ToString();
    }

    public decimal? BreachFixedLimit
    {
        get => decimal.TryParse(this[nameof(BreachFixedLimit)], out var fixedTickerOpeningOrderSize) ? fixedTickerOpeningOrderSize : null;
        set => this[nameof(BreachFixedLimit)] = value.ToString();
    }

    object ICloneable.Clone() => Clone();

    IFixedPeriodLimitConfig ICloneable<IFixedPeriodLimitConfig>.Clone() => Clone();

    public virtual FixedPeriodLimitConfig Clone() => new(this);

    public virtual bool AreEquivalent(IFixedPeriodLimitConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var breachFixedLimitSame = BreachFixedLimit == other.BreachFixedLimit;
        var alertFixedLimitSame  = AlertFixedLimit == other.AlertFixedLimit;
        var periodSame           = Period == other.Period;

        var allAreSame = breachFixedLimitSame && alertFixedLimitSame && periodSame;

        return allAreSame;
    }

    public static void ClearValues(IConfigurationRoot root, string path)
    {
        root[$"{path}{Split}{nameof(BreachFixedLimit)}"] = null;
        root[$"{path}{Split}{nameof(AlertFixedLimit)}"]  = null;
        root[$"{path}{Split}{nameof(Period)}"]           = null;
    }

    protected bool Equals(IFixedPeriodLimitConfig other) => AreEquivalent(other, true);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IFixedPeriodLimitConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = BreachFixedLimit?.GetHashCode() ?? 0;
            hashCode = (hashCode * 397) ^ (AlertFixedLimit?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ (Period?.GetHashCode() ?? 0);
            return hashCode;
        }
    }

    protected string FixedPeriodLimitConfigToStringMembers =>
        $"{nameof(BreachFixedLimit)}: {BreachFixedLimit}, {nameof(AlertFixedLimit)}: {AlertFixedLimit}, {nameof(Period)}: {Period}, ";

    public override string ToString() => $"{nameof(FixedPeriodLimitConfig)}{{{FixedPeriodLimitConfigToStringMembers}}}";
}
