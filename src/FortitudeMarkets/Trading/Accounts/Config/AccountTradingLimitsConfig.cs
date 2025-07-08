// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeMarkets.Trading.Accounts.Config;

public interface IAccountTradingLimitsConfig : IRelativeTickerTradingLimitsConfig
  , ICloneable<IAccountTradingLimitsConfig>
{
    decimal? MaxFixedTotalOpenPosition { get; set; }

    decimal? MaxTotalOpenPositionPercentage { get; set; }

    new IAccountTradingLimitsConfig Clone();
}

public class AccountTradingLimitsConfig : RelativeTickerTradingLimitsConfig, IAccountTradingLimitsConfig
{
    public AccountTradingLimitsConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public AccountTradingLimitsConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public AccountTradingLimitsConfig
    (decimal? overrideTradingAccountSize = null, decimal? adjustTradingAccountSizePercentage = null
      , decimal? maxTotalOpenPositionPercentage = null, decimal? maxFixedTotalOpenPosition = null
      , decimal? maxTickerOpenPositionPercentage = null, decimal? maxFixedTickerOpenPosition = null
      , decimal? maxTickerOpeningOrderSizePercentage = null, decimal? maxFixedTickerOpeningOrderSize = null
      , decimal? maxTickerReducingOrderSizePercentage = null, decimal? maxFixedTickerReducingOrderSize = null)
        : this(InMemoryConfigRoot, InMemoryPath, overrideTradingAccountSize, adjustTradingAccountSizePercentage
             , maxTotalOpenPositionPercentage, maxFixedTotalOpenPosition, maxTickerOpenPositionPercentage
             , maxFixedTickerOpenPosition, maxTickerOpeningOrderSizePercentage, maxFixedTickerOpeningOrderSize, maxTickerReducingOrderSizePercentage
             , maxFixedTickerReducingOrderSize)
    {
    }

    public AccountTradingLimitsConfig
    (IConfigurationRoot root, string path, decimal? overrideTradingAccountSize = null, decimal? adjustTradingAccountSizePercentage = null
      , decimal? maxTotalOpenPositionPercentage = null, decimal? maxFixedTotalOpenPosition = null
      , decimal? maxTickerOpenPositionPercentage = null, decimal? maxFixedTickerOpenPosition = null
      , decimal? maxTickerOpeningOrderSizePercentage = null, decimal? maxFixedTickerOpeningOrderSize = null
      , decimal? maxTickerReducingOrderSizePercentage = null, decimal? maxFixedTickerReducingOrderSize = null)
        : base(root, path, overrideTradingAccountSize, adjustTradingAccountSizePercentage, maxTickerOpenPositionPercentage, maxFixedTickerOpenPosition
             , maxTickerOpeningOrderSizePercentage
             , maxFixedTickerOpeningOrderSize, maxTickerReducingOrderSizePercentage, maxFixedTickerReducingOrderSize)
    {
        MaxTotalOpenPositionPercentage = maxTotalOpenPositionPercentage;
        MaxFixedTotalOpenPosition      = maxFixedTotalOpenPosition;
    }

    public AccountTradingLimitsConfig(IAccountTradingLimitsConfig toClone, IConfigurationRoot root, string path) : base(toClone, root, path)
    {
        MaxTotalOpenPositionPercentage = toClone.MaxTotalOpenPositionPercentage;
        MaxFixedTotalOpenPosition      = toClone.MaxFixedTotalOpenPosition;
    }

    public AccountTradingLimitsConfig(IAccountTradingLimitsConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public IAccountTradingLimitsConfig? ParentAccountTradingLimitsConfig { get; set; }

    public decimal? MaxTotalOpenPositionPercentage
    {
        get =>
            decimal.TryParse(this[nameof(MaxTotalOpenPositionPercentage)], out var maxTotalOpenPositionPercentage)
                ? maxTotalOpenPositionPercentage
                : null;
        set => this[nameof(MaxTotalOpenPositionPercentage)] = value.ToString();
    }

    public decimal? MaxFixedTotalOpenPosition
    {
        get => decimal.TryParse(this[nameof(MaxFixedTotalOpenPosition)], out var fixedTotalOpenPosition) ? fixedTotalOpenPosition : null;
        set => this[nameof(MaxFixedTotalOpenPosition)] = value.ToString();
    }

    IAccountTradingLimitsConfig ICloneable<IAccountTradingLimitsConfig>.Clone() => Clone();

    IAccountTradingLimitsConfig IAccountTradingLimitsConfig.Clone() => Clone();

    IRelativeTickerTradingLimitsConfig ICloneable<IRelativeTickerTradingLimitsConfig>.Clone() => Clone();

    IRelativeTickerTradingLimitsConfig IRelativeTickerTradingLimitsConfig.Clone() => Clone();

    public override AccountTradingLimitsConfig Clone() => new(this);

    public override bool AreEquivalent(IFixedTickerTradingLimitsConfig? other, bool exactTypes = false)
    {
        if (other is not IAccountTradingLimitsConfig relativeLimitsConfig) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var maxTotalOpenPosPctSame   = MaxTotalOpenPositionPercentage == relativeLimitsConfig.MaxTotalOpenPositionPercentage;
        var maxTotalFixedOpenPosSame = MaxFixedTotalOpenPosition == relativeLimitsConfig.MaxFixedTotalOpenPosition;

        var allAreSame = baseSame && maxTotalOpenPosPctSame && maxTotalFixedOpenPosSame;

        return allAreSame;
    }

    public new static void ClearValues(IConfigurationRoot root, string path)
    {
        root[$"{path}{Split}{nameof(MaxTotalOpenPositionPercentage)}"] = null;
        root[$"{path}{Split}{nameof(MaxFixedTotalOpenPosition)}"]      = null;
        RelativeTickerTradingLimitsConfig.ClearValues(root, path);
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IAccountTradingLimitsConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ (MaxTotalOpenPositionPercentage?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ (MaxFixedTotalOpenPosition?.GetHashCode() ?? 0);
            return hashCode;
        }
    }

    public string AccountTradingLimitsConfigToStringMembers =>
        $"{nameof(MaxTotalOpenPositionPercentage)}: {MaxTotalOpenPositionPercentage}, {nameof(MaxFixedTotalOpenPosition)}: {MaxFixedTotalOpenPosition}, " +
        $"{RelativeTickerLimitsToStringMembers}";

    public override string ToString() => $"{nameof(AccountTradingLimitsConfig)}{{{AccountTradingLimitsConfigToStringMembers}}}";
}
