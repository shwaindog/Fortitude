// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Configuration;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeMarkets.Trading.Accounts.Config;

public interface ITickerThroughputLimitsConfig : IInterfacesComparable<ITickerThroughputLimitsConfig>, ICloneable<ITickerThroughputLimitsConfig>
{
    const TimeBoundaryPeriod DefaultTimeBoundaryPeriod = TimeBoundaryPeriod.TenSeconds;

    TimeBoundaryPeriod DefaultPeriod { get; set; }

    IOrderSubmitThroughputLimitConfig TickerOrderSubmitThroughputLimits { get; set; }

    IOrdersSubmitVolumeThroughputLimitConfig TickerOrdersSubmitVolumeThroughputLimits { get; set; }
}


public class TickerThroughputLimitsConfig : ConfigSection, ITickerThroughputLimitsConfig
{
    public TickerThroughputLimitsConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public TickerThroughputLimitsConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public TickerThroughputLimitsConfig(TimeBoundaryPeriod defaultPeriod, IOrderSubmitThroughputLimitConfig tickerOrderSubmitThroughputLimits
      , IOrdersSubmitVolumeThroughputLimitConfig tickerSubmitVolumeThroughputLimits) 
        : this(InMemoryConfigRoot, InMemoryPath, defaultPeriod, tickerOrderSubmitThroughputLimits, tickerSubmitVolumeThroughputLimits)
    {
    }

    public TickerThroughputLimitsConfig(IConfigurationRoot root, string path, TimeBoundaryPeriod defaultPeriod
      , IOrderSubmitThroughputLimitConfig tickerOrderSubmitThroughputLimits, IOrdersSubmitVolumeThroughputLimitConfig tickerSubmitVolumeThroughputLimits) : base(root, path)
    {
        TickerOrderSubmitThroughputLimits        = tickerOrderSubmitThroughputLimits;
        TickerOrdersSubmitVolumeThroughputLimits = tickerSubmitVolumeThroughputLimits;
        DefaultPeriod                            = defaultPeriod;
    }

    public TickerThroughputLimitsConfig (ITickerThroughputLimitsConfig toClone, IConfigurationRoot root, string path) : this(root, path)
    {
        TickerOrderSubmitThroughputLimits        = toClone.TickerOrderSubmitThroughputLimits;
        TickerOrdersSubmitVolumeThroughputLimits = toClone.TickerOrdersSubmitVolumeThroughputLimits;
        DefaultPeriod                            = toClone.DefaultPeriod;
    }

    public TickerThroughputLimitsConfig(ITickerThroughputLimitsConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public TimeBoundaryPeriod DefaultPeriod
    {
        get => Enum.TryParse<TimeBoundaryPeriod>(this[nameof(DefaultPeriod)], out var fixedTickerOpenPosition) 
            ? fixedTickerOpenPosition 
            : ITickerThroughputLimitsConfig.DefaultTimeBoundaryPeriod;
        set => this[nameof(DefaultPeriod)] = value.ToString();
    }

    public IOrderSubmitThroughputLimitConfig TickerOrderSubmitThroughputLimits
    {
        get
        {
            if (GetSection(nameof(TickerOrderSubmitThroughputLimits)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new OrderSubmitThroughputLimitConfig(ConfigRoot, $"{Path}{Split}{nameof(TickerOrderSubmitThroughputLimits)}");
            }
            throw new ConfigurationErrorsException($"Expected {nameof(TickerOrderSubmitThroughputLimits)} to be configured");
        }
        set => _ = new OrderSubmitThroughputLimitConfig(value, ConfigRoot, $"{Path}{Split}{nameof(TickerOrderSubmitThroughputLimits)}");
    }

    public IOrdersSubmitVolumeThroughputLimitConfig TickerOrdersSubmitVolumeThroughputLimits
    {
        get
        {
            if (GetSection(nameof(TickerOrdersSubmitVolumeThroughputLimits)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new OrdersSubmitVolumeThroughputLimitConfig(ConfigRoot, $"{Path}{Split}{nameof(TickerOrdersSubmitVolumeThroughputLimits)}");
            }
            throw new ConfigurationErrorsException($"Expected {nameof(TickerOrdersSubmitVolumeThroughputLimits)} to be configured");
        }
        set => _ = new OrdersSubmitVolumeThroughputLimitConfig(value, ConfigRoot, $"{Path}{Split}{nameof(TickerOrdersSubmitVolumeThroughputLimits)}");
    }


    object ICloneable.Clone() => Clone();

    ITickerThroughputLimitsConfig ICloneable<ITickerThroughputLimitsConfig>.Clone() => Clone();

    public virtual TickerThroughputLimitsConfig Clone() => new(this);

    public virtual bool AreEquivalent(ITickerThroughputLimitsConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var orderSubmitCountThroughputSame        = TickerOrderSubmitThroughputLimits.AreEquivalent(other.TickerOrderSubmitThroughputLimits, exactTypes);
        var orderSubmitVolumeThroughputSame = TickerOrdersSubmitVolumeThroughputLimits.AreEquivalent(other.TickerOrdersSubmitVolumeThroughputLimits, exactTypes);
        var defaultPeriodSame = DefaultPeriod == other.DefaultPeriod;

        var allAreSame = orderSubmitCountThroughputSame && orderSubmitVolumeThroughputSame && defaultPeriodSame;

        return allAreSame;
    }

    public static void ClearValues(IConfigurationRoot root, string path)
    {
        root[$"{path}{Split}{nameof(TickerOrderSubmitThroughputLimits)}"]        = null;
        root[$"{path}{Split}{nameof(TickerOrdersSubmitVolumeThroughputLimits)}"] = null;
        root[$"{path}{Split}{nameof(DefaultPeriod)}"]                            = null;
    }

    protected bool Equals(ITickerThroughputLimitsConfig other) => AreEquivalent(other, true);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ITickerThroughputLimitsConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = DefaultPeriod.GetHashCode();
            hashCode = (hashCode * 397) ^ TickerOrderSubmitThroughputLimits.GetHashCode();
            hashCode = (hashCode * 397) ^ TickerOrdersSubmitVolumeThroughputLimits.GetHashCode();
            return hashCode;
        }
    }

    protected string TickerThroughputLimitsConfigToStringMembers =>
        $"{nameof(TickerOrderSubmitThroughputLimits)}: {TickerOrderSubmitThroughputLimits}, " +
        $"{nameof(TickerOrdersSubmitVolumeThroughputLimits)}: {TickerOrdersSubmitVolumeThroughputLimits}, " +
        $"{nameof(DefaultPeriod)}: {DefaultPeriod}";

    public override string ToString() => $"{nameof(TickerThroughputLimitsConfig)}{{{TickerThroughputLimitsConfigToStringMembers}}}";
}
