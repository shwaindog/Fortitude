// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Chronometry;
using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeMarkets.Trading.Accounts.Config;

public interface IOrderSubmitThroughputLimitConfig : IInterfacesComparable<IOrderSubmitThroughputLimitConfig>, ICloneable<IOrderSubmitThroughputLimitConfig>
{
    const TimeBoundaryPeriod DefaultFixedLimitPeriodSeconds = TimeBoundaryPeriod.TenSeconds;

    const int DefaultAllOrdersFixedBreachLimit     = 20;
    const int DefaultAllOrdersFixedAlertLimit      = 16;
    const int DefaultOpeningOrdersFixedBreachLimit = 10;
    const int DefaultOpeningOrdersFixedAlertLimit  = 8;

    TimeBoundaryPeriod? Period { get; set; }

    IFixedPeriodLimitConfig AllOrdersSubmitCount { get; set; }

    IFixedPeriodLimitConfig OpeningPositionOrdersSubmitCount { get; set; }

    IFixedPeriodLimitConfig? ReducingPositionOrdersSubmitCount { get; set; }
}

public class OrderSubmitThroughputLimitConfig : ConfigSection, IOrderSubmitThroughputLimitConfig
{
    public OrderSubmitThroughputLimitConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public OrderSubmitThroughputLimitConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public OrderSubmitThroughputLimitConfig
    (IFixedPeriodLimitConfig allOrdersSubmitCount, IFixedPeriodLimitConfig openingPositionOrdersSubmitCount
      , IFixedPeriodLimitConfig? reducingPositionOrdersSubmitCount = null, TimeBoundaryPeriod? period = null)
        : this()
    {
        AllOrdersSubmitCount              = allOrdersSubmitCount;
        OpeningPositionOrdersSubmitCount  = openingPositionOrdersSubmitCount;
        ReducingPositionOrdersSubmitCount = reducingPositionOrdersSubmitCount;
        Period                            = period;
    }

    public OrderSubmitThroughputLimitConfig
        (IOrderSubmitThroughputLimitConfig toClone, IConfigurationRoot root, string path) : this(root, path)
    {
        AllOrdersSubmitCount              = toClone.AllOrdersSubmitCount;
        OpeningPositionOrdersSubmitCount  = toClone.OpeningPositionOrdersSubmitCount;
        ReducingPositionOrdersSubmitCount = toClone.ReducingPositionOrdersSubmitCount;
        Period                            = toClone.Period;
    }

    public OrderSubmitThroughputLimitConfig(IOrderSubmitThroughputLimitConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public TimeBoundaryPeriod? Period
    {
        get => Enum.TryParse<TimeBoundaryPeriod>(this[nameof(Period)], out var fixedTickerOpenPosition) ? fixedTickerOpenPosition : null;
        set => this[nameof(Period)] = value?.ToString() ?? null;
    }

    public IFixedPeriodLimitConfig AllOrdersSubmitCount
    {
        get
        {
            if (GetSection(nameof(AllOrdersSubmitCount)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new FixedPeriodLimitConfig(ConfigRoot, $"{Path}{Split}{nameof(AllOrdersSubmitCount)}");
            }
            return new FixedPeriodLimitConfig
                (ConfigRoot, $"{Path}{Split}{nameof(AllOrdersSubmitCount)}"
               , IOrderSubmitThroughputLimitConfig.DefaultAllOrdersFixedBreachLimit
               , IOrderSubmitThroughputLimitConfig.DefaultAllOrdersFixedAlertLimit
               , IOrderSubmitThroughputLimitConfig.DefaultFixedLimitPeriodSeconds);
        }
        set => _ = new FixedPeriodLimitConfig(value, ConfigRoot, $"{Path}{Split}{nameof(AllOrdersSubmitCount)}");
    }

    public IFixedPeriodLimitConfig OpeningPositionOrdersSubmitCount
    {
        get
        {
            if (GetSection(nameof(OpeningPositionOrdersSubmitCount)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new FixedPeriodLimitConfig(ConfigRoot, $"{Path}{Split}{nameof(OpeningPositionOrdersSubmitCount)}");
            }
            return new FixedPeriodLimitConfig
                (ConfigRoot, $"{Path}{Split}{nameof(OpeningPositionOrdersSubmitCount)}"
               , IOrderSubmitThroughputLimitConfig.DefaultOpeningOrdersFixedBreachLimit
               , IOrderSubmitThroughputLimitConfig.DefaultOpeningOrdersFixedAlertLimit
               , IOrderSubmitThroughputLimitConfig.DefaultFixedLimitPeriodSeconds);
        }
        set => _ = new FixedPeriodLimitConfig(value, ConfigRoot, $"{Path}{Split}{nameof(OpeningPositionOrdersSubmitCount)}");
    }

    public IFixedPeriodLimitConfig? ReducingPositionOrdersSubmitCount
    {
        get
        {
            if (GetSection(nameof(ReducingPositionOrdersSubmitCount)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new FixedPeriodLimitConfig(ConfigRoot, $"{Path}{Split}{nameof(ReducingPositionOrdersSubmitCount)}");
            }
            return null;
        }
        set => _ = value != null ? new FixedPeriodLimitConfig(value, ConfigRoot, $"{Path}{Split}{nameof(ReducingPositionOrdersSubmitCount)}") : null;
    }

    object ICloneable.Clone() => Clone();

    IOrderSubmitThroughputLimitConfig ICloneable<IOrderSubmitThroughputLimitConfig>.Clone() => Clone();

    public virtual OrderSubmitThroughputLimitConfig Clone() => new(this);

    public virtual bool AreEquivalent(IOrderSubmitThroughputLimitConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var allOrdersSubmitCntSame        = AllOrdersSubmitCount.AreEquivalent(other.AllOrdersSubmitCount, exactTypes);
        var openingPosOrdersSubmitCntSame = OpeningPositionOrdersSubmitCount.AreEquivalent(other.OpeningPositionOrdersSubmitCount, exactTypes);
        var reducingPosOrdersSubmitCntSame = ReducingPositionOrdersSubmitCount?.AreEquivalent(other.ReducingPositionOrdersSubmitCount, exactTypes)
                                          ?? other.ReducingPositionOrdersSubmitCount == null;
        var periodSame = Period == other.Period;

        var allAreSame = allOrdersSubmitCntSame && openingPosOrdersSubmitCntSame && reducingPosOrdersSubmitCntSame && periodSame;

        return allAreSame;
    }

    public static void ClearValues(IConfigurationRoot root, string path)
    {
        root[$"{path}{Split}{nameof(AllOrdersSubmitCount)}"]              = null;
        root[$"{path}{Split}{nameof(OpeningPositionOrdersSubmitCount)}"]  = null;
        root[$"{path}{Split}{nameof(ReducingPositionOrdersSubmitCount)}"] = null;
        root[$"{path}{Split}{nameof(Period)}"]                            = null;
    }

    protected bool Equals(IOrderSubmitThroughputLimitConfig other) => AreEquivalent(other, true);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IOrderSubmitThroughputLimitConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = AllOrdersSubmitCount.GetHashCode();
            hashCode = (hashCode * 397) ^ OpeningPositionOrdersSubmitCount.GetHashCode();
            hashCode = (hashCode * 397) ^ (ReducingPositionOrdersSubmitCount?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ (Period?.GetHashCode() ?? 0);
            return hashCode;
        }
    }

    protected string OpeningReducingSubmitCountLimitConfigToStringMembers =>
        $"{nameof(AllOrdersSubmitCount)}: {AllOrdersSubmitCount}, {nameof(OpeningPositionOrdersSubmitCount)}: {OpeningPositionOrdersSubmitCount}, " +
        $"{nameof(ReducingPositionOrdersSubmitCount)}: {ReducingPositionOrdersSubmitCount}, {nameof(Period)}: {Period}, " +
        $"{nameof(Period)}: {Period}, ";

    public override string ToString() => $"{nameof(OrderSubmitThroughputLimitConfig)}{{{OpeningReducingSubmitCountLimitConfigToStringMembers}}}";
}
