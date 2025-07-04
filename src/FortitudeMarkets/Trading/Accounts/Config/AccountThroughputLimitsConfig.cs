// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Configuration;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeMarkets.Trading.Accounts.Config;

public interface IAccountThroughputLimitsConfig : ITickerThroughputLimitsConfig, ICloneable<IAccountThroughputLimitsConfig>
{
    IOrderSubmitThroughputLimitConfig TotalAllOrdersOrderSubmitThroughput { get; set; }

    IOrdersSubmitVolumeThroughputLimitConfig TotalAllOrdersSubmitVolumeThroughput { get; set; }

    new IAccountThroughputLimitsConfig Clone();
}

public class AccountThroughputLimitsConfig : TickerThroughputLimitsConfig, IAccountThroughputLimitsConfig
{
    public AccountThroughputLimitsConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public AccountThroughputLimitsConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public AccountThroughputLimitsConfig(TimeBoundaryPeriod defaultPeriod, IOrderSubmitThroughputLimitConfig totalAllOrdersSubmitThroughputLimits
      , IOrdersSubmitVolumeThroughputLimitConfig totalAllOrdersSubmitVolumeThroughputLimits
      , IOrderSubmitThroughputLimitConfig tickerOrderSubmitThroughputLimits
      , IOrdersSubmitVolumeThroughputLimitConfig tickerSubmitVolumeThroughputLimits) 
        : this(InMemoryConfigRoot, InMemoryPath, defaultPeriod, totalAllOrdersSubmitThroughputLimits, totalAllOrdersSubmitVolumeThroughputLimits
             , tickerOrderSubmitThroughputLimits, tickerSubmitVolumeThroughputLimits)
    {
    }

    public AccountThroughputLimitsConfig(IConfigurationRoot root, string path, TimeBoundaryPeriod defaultPeriod
      , IOrderSubmitThroughputLimitConfig totalAllOrdersSubmitThroughputLimits
      , IOrdersSubmitVolumeThroughputLimitConfig totalAllOrdersSubmitVolumeThroughputLimits
      , IOrderSubmitThroughputLimitConfig tickerOrderSubmitThroughputLimits
      , IOrdersSubmitVolumeThroughputLimitConfig tickerSubmitVolumeThroughputLimits) 
        : base(root, path, defaultPeriod, tickerOrderSubmitThroughputLimits, tickerSubmitVolumeThroughputLimits)
    {
        TotalAllOrdersOrderSubmitThroughput  = totalAllOrdersSubmitThroughputLimits;
        TotalAllOrdersSubmitVolumeThroughput = totalAllOrdersSubmitVolumeThroughputLimits;
    }

    public AccountThroughputLimitsConfig (IAccountThroughputLimitsConfig toClone, IConfigurationRoot root, string path) : base(toClone, root, path)
    {
        TotalAllOrdersOrderSubmitThroughput  = toClone.TotalAllOrdersOrderSubmitThroughput;
        TotalAllOrdersSubmitVolumeThroughput = toClone.TotalAllOrdersSubmitVolumeThroughput;
    }

    public AccountThroughputLimitsConfig(IAccountThroughputLimitsConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public IOrderSubmitThroughputLimitConfig TotalAllOrdersOrderSubmitThroughput
    {
        get
        {
            if (GetSection(nameof(TotalAllOrdersOrderSubmitThroughput)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new OrderSubmitThroughputLimitConfig(ConfigRoot, $"{Path}{Split}{nameof(TotalAllOrdersOrderSubmitThroughput)}");
            }
            throw new ConfigurationErrorsException($"Expected {nameof(TotalAllOrdersOrderSubmitThroughput)} to be configured");
        }
        set => _ = new OrderSubmitThroughputLimitConfig(value, ConfigRoot, $"{Path}{Split}{nameof(TotalAllOrdersOrderSubmitThroughput)}");
    }

    public IOrdersSubmitVolumeThroughputLimitConfig TotalAllOrdersSubmitVolumeThroughput
    {
        get
        {
            if (GetSection(nameof(TotalAllOrdersSubmitVolumeThroughput)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new OrdersSubmitVolumeThroughputLimitConfig(ConfigRoot, $"{Path}{Split}{nameof(TotalAllOrdersSubmitVolumeThroughput)}");
            }
            throw new ConfigurationErrorsException($"Expected {nameof(TotalAllOrdersSubmitVolumeThroughput)} to be configured");
        }
        set => _ = new OrdersSubmitVolumeThroughputLimitConfig(value, ConfigRoot, $"{Path}{Split}{nameof(TotalAllOrdersSubmitVolumeThroughput)}");
    }

    object ICloneable.Clone() => Clone();

    IAccountThroughputLimitsConfig ICloneable<IAccountThroughputLimitsConfig>.Clone() => Clone();

    IAccountThroughputLimitsConfig IAccountThroughputLimitsConfig.Clone() => Clone();

    public override AccountThroughputLimitsConfig Clone() => new(this);

    public override bool AreEquivalent(ITickerThroughputLimitsConfig? other, bool exactTypes = false)
    {
        if (other is not IAccountThroughputLimitsConfig accountThroughputLimits) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var orderSubmitCountThroughputSame        = TotalAllOrdersOrderSubmitThroughput.AreEquivalent(accountThroughputLimits.TotalAllOrdersOrderSubmitThroughput, exactTypes);
        var orderSubmitVolumeThroughputSame = TotalAllOrdersSubmitVolumeThroughput.AreEquivalent(accountThroughputLimits.TotalAllOrdersSubmitVolumeThroughput, exactTypes);

        var allAreSame = baseSame && orderSubmitCountThroughputSame && orderSubmitVolumeThroughputSame;

        return allAreSame;
    }

    public new static void ClearValues(IConfigurationRoot root, string path)
    {
        root[$"{path}{Split}{nameof(TotalAllOrdersOrderSubmitThroughput)}"]  = null;
        root[$"{path}{Split}{nameof(TotalAllOrdersSubmitVolumeThroughput)}"] = null;
        TickerThroughputLimitsConfig.ClearValues(root, path);
    }

    protected bool Equals(IAccountThroughputLimitsConfig other) => AreEquivalent(other, true);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IAccountThroughputLimitsConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = DefaultPeriod.GetHashCode();
            hashCode = (hashCode * 397) ^ TotalAllOrdersOrderSubmitThroughput.GetHashCode();
            hashCode = (hashCode * 397) ^ TotalAllOrdersSubmitVolumeThroughput.GetHashCode();
            return hashCode;
        }
    }

    protected string AccountThroughputLimitsConfigToStringMembers =>
        $"{nameof(TotalAllOrdersOrderSubmitThroughput)}: {TotalAllOrdersOrderSubmitThroughput}, " +
        $"{nameof(TotalAllOrdersSubmitVolumeThroughput)}: {TotalAllOrdersSubmitVolumeThroughput}, " +
        $"{TickerThroughputLimitsConfigToStringMembers}";

    public override string ToString() => $"{nameof(AccountThroughputLimitsConfig)}{{{AccountThroughputLimitsConfigToStringMembers}}}";
}
