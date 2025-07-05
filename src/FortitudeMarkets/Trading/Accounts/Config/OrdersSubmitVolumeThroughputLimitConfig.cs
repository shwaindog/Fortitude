// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Chronometry;
using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeMarkets.Trading.Accounts.Config;

public interface IOrdersSubmitVolumeThroughputLimitConfig : IInterfacesComparable<IOrdersSubmitVolumeThroughputLimitConfig>, ICloneable<IOrdersSubmitVolumeThroughputLimitConfig>
{
    const TimeBoundaryPeriod DefaultVolumeLimitPeriod = TimeBoundaryPeriod.TenMinutes;

    const decimal DefaultAllOrdersBreachLimitPercentage     = 1.0m;
    const decimal DefaultAllOrdersAlertLimitPercentage      = 0.98m;
    const decimal DefaultOpeningOrdersBreachLimitPercentage = 0.5m;
    const decimal DefaultOpeningOrdersAlertLimitPercentage  = 1.0m;

    TimeBoundaryPeriod? Period { get; set; }

    IRelativeTradingPeriodLimitConfig AllOrdersVolume { get; set; }

    IRelativeTradingPeriodLimitConfig OpeningPositionVolume { get; set; }

    IRelativeTradingPeriodLimitConfig? ReducingPositionVolume { get; set; }
}

public class OrdersSubmitVolumeThroughputLimitConfig : ConfigSection, IOrdersSubmitVolumeThroughputLimitConfig
{
    public OrdersSubmitVolumeThroughputLimitConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public OrdersSubmitVolumeThroughputLimitConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public OrdersSubmitVolumeThroughputLimitConfig
    (IRelativeTradingPeriodLimitConfig allOrdersVolume, IRelativeTradingPeriodLimitConfig openingPositionOrdersVolume
      , IRelativeTradingPeriodLimitConfig? reducingPositionOrdersVolume = null, TimeBoundaryPeriod? period = null)
        : this()
    {
        AllOrdersVolume        = allOrdersVolume;
        OpeningPositionVolume  = openingPositionOrdersVolume;
        ReducingPositionVolume = reducingPositionOrdersVolume;
        Period                 = period;
    }

    public OrdersSubmitVolumeThroughputLimitConfig
        (IOrdersSubmitVolumeThroughputLimitConfig toClone, IConfigurationRoot root, string path) : this(root, path)
    {
        AllOrdersVolume        = toClone.AllOrdersVolume;
        OpeningPositionVolume  = toClone.OpeningPositionVolume;
        ReducingPositionVolume = toClone.ReducingPositionVolume;
        Period                 = toClone.Period;
    }

    public OrdersSubmitVolumeThroughputLimitConfig
        (IOrdersSubmitVolumeThroughputLimitConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public TimeBoundaryPeriod? Period
    {
        get => Enum.TryParse<TimeBoundaryPeriod>(this[nameof(Period)], out var fixedTickerOpenPosition) ? fixedTickerOpenPosition : null;
        set => this[nameof(Period)] = value?.ToString() ?? null;
    }

    public IRelativeTradingPeriodLimitConfig AllOrdersVolume
    {
        get
        {
            if (GetSection(nameof(AllOrdersVolume)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new RelativeTradingPeriodLimitConfig(ConfigRoot, $"{Path}{Split}{nameof(AllOrdersVolume)}");
            }
            return new RelativeTradingPeriodLimitConfig
                (ConfigRoot, $"{Path}{Split}{nameof(AllOrdersVolume)}"
               , IOrdersSubmitVolumeThroughputLimitConfig.DefaultAllOrdersBreachLimitPercentage
               , alertAccountSizePercentageLimit: IOrdersSubmitVolumeThroughputLimitConfig.DefaultAllOrdersAlertLimitPercentage
               , period: IOrdersSubmitVolumeThroughputLimitConfig.DefaultVolumeLimitPeriod);
        }
        set => _ = new RelativeTradingPeriodLimitConfig(value, ConfigRoot, $"{Path}{Split}{nameof(AllOrdersVolume)}");
    }

    public IRelativeTradingPeriodLimitConfig OpeningPositionVolume
    {
        get
        {
            if (GetSection(nameof(OpeningPositionVolume)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new RelativeTradingPeriodLimitConfig(ConfigRoot, $"{Path}{Split}{nameof(OpeningPositionVolume)}");
            }
            return new RelativeTradingPeriodLimitConfig
                (ConfigRoot, $"{Path}{Split}{nameof(AllOrdersVolume)}"
               , IOrdersSubmitVolumeThroughputLimitConfig.DefaultOpeningOrdersBreachLimitPercentage
               , alertAccountSizePercentageLimit: IOrdersSubmitVolumeThroughputLimitConfig.DefaultOpeningOrdersAlertLimitPercentage
               , period: IOrdersSubmitVolumeThroughputLimitConfig.DefaultVolumeLimitPeriod);
        }
        set => _ = new RelativeTradingPeriodLimitConfig(value, ConfigRoot, $"{Path}{Split}{nameof(OpeningPositionVolume)}");
    }

    public IRelativeTradingPeriodLimitConfig? ReducingPositionVolume
    {
        get
        {
            if (GetSection(nameof(ReducingPositionVolume)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new RelativeTradingPeriodLimitConfig(ConfigRoot, $"{Path}{Split}{nameof(ReducingPositionVolume)}");
            }
            return null;
        }
        set => _ = value != null ? new RelativeTradingPeriodLimitConfig(value, ConfigRoot, $"{Path}{Split}{nameof(ReducingPositionVolume)}") : null;
    }

    object ICloneable.Clone() => Clone();

    IOrdersSubmitVolumeThroughputLimitConfig ICloneable<IOrdersSubmitVolumeThroughputLimitConfig>.Clone() => Clone();

    public virtual OrdersSubmitVolumeThroughputLimitConfig Clone() => new(this);

    public virtual bool AreEquivalent(IOrdersSubmitVolumeThroughputLimitConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var allOrdersVolumeSame           = AllOrdersVolume.AreEquivalent(other.AllOrdersVolume, exactTypes);
        var openingPosOrdersVolumeSame = OpeningPositionVolume.AreEquivalent(other.OpeningPositionVolume, exactTypes);
        var reducingPosOrdersVolumeSame = ReducingPositionVolume?.AreEquivalent(other.ReducingPositionVolume, exactTypes)
                                          ?? other.ReducingPositionVolume == null;
        var periodSame = Period == other.Period;

        var allAreSame = allOrdersVolumeSame && openingPosOrdersVolumeSame && reducingPosOrdersVolumeSame && periodSame;

        return allAreSame;
    }

    public static void ClearValues(IConfigurationRoot root, string path)
    {
        root[$"{path}{Split}{nameof(AllOrdersVolume)}"]        = null;
        root[$"{path}{Split}{nameof(OpeningPositionVolume)}"]  = null;
        root[$"{path}{Split}{nameof(ReducingPositionVolume)}"] = null;
        root[$"{path}{Split}{nameof(Period)}"]                 = null;
    }

    protected bool Equals(IOrdersSubmitVolumeThroughputLimitConfig other) => AreEquivalent(other, true);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IOrdersSubmitVolumeThroughputLimitConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = AllOrdersVolume.GetHashCode();
            hashCode = (hashCode * 397) ^ OpeningPositionVolume.GetHashCode();
            hashCode = (hashCode * 397) ^ (ReducingPositionVolume?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ (Period?.GetHashCode() ?? 0);
            return hashCode;
        }
    }

    protected string OpeningReducingSubmitCountLimitConfigToStringMembers =>
        $"{nameof(AllOrdersVolume)}: {AllOrdersVolume}, {nameof(OpeningPositionVolume)}: {OpeningPositionVolume}, " +
        $"{nameof(ReducingPositionVolume)}: {ReducingPositionVolume}, {nameof(Period)}: {Period}, " +
        $"{nameof(Period)}: {Period}, ";

    public override string ToString() => $"{nameof(OrdersSubmitVolumeThroughputLimitConfig)}{{{OpeningReducingSubmitCountLimitConfigToStringMembers}}}";
}
