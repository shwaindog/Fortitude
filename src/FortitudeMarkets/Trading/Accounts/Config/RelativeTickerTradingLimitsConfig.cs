// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeMarkets.Trading.Accounts.Config;

public interface IRelativeTickerTradingLimitsConfig : IFixedTickerTradingLimitsConfig
  , ICloneable<IRelativeTickerTradingLimitsConfig>
{
    decimal? UsingTradingAccountSize { get; set; }

    decimal? AdjustTradingAccountSizePercentage { get; set; }

    decimal? MaxTickerOpenPositionPercentage { get; set; }

    decimal? MaxTickerOpeningOrderSizePercentage { get; set; }

    decimal? MaxTickerReducingOrderSizePercentage { get; set; }

    new IRelativeTickerTradingLimitsConfig Clone();
}



public class RelativeTickerTradingLimitsConfig : FixedTickerTradingLimitsConfig, IRelativeTickerTradingLimitsConfig
{
    public RelativeTickerTradingLimitsConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public RelativeTickerTradingLimitsConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public RelativeTickerTradingLimitsConfig
    (decimal? overrideTradingAccountSize = null, decimal? adjustTradingAccountSizePercentage = null
      , decimal? maxTickerOpenPositionPercentage = null, decimal? maxFixedTickerOpenPosition = null
      , decimal? maxTickerOpeningOrderSizePercentage = null, decimal? maxFixedTickerOpeningOrderSize = null
      , decimal? maxTickerReducingOrderSizePercentage = null, decimal? maxFixedTickerReducingOrderSize = null)
        : this(InMemoryConfigRoot, InMemoryPath, overrideTradingAccountSize, adjustTradingAccountSizePercentage, maxTickerOpenPositionPercentage
             , maxFixedTickerOpenPosition, maxTickerOpeningOrderSizePercentage, maxFixedTickerOpeningOrderSize, maxTickerReducingOrderSizePercentage
             , maxFixedTickerReducingOrderSize)
    {
    }

    public RelativeTickerTradingLimitsConfig
    (IConfigurationRoot root, string path, decimal? overrideTradingAccountSize = null, decimal? adjustTradingAccountSizePercentage = null
      , decimal? maxTickerOpenPositionPercentage = null, decimal? maxFixedTickerOpenPosition = null
      , decimal? maxTickerOpeningOrderSizePercentage = null, decimal? maxFixedTickerOpeningOrderSize = null
      , decimal? maxTickerReducingOrderSizePercentage = null, decimal? maxFixedTickerReducingOrderSize = null)
        : base(root, path, maxFixedTickerOpenPosition, maxFixedTickerOpeningOrderSize, maxFixedTickerReducingOrderSize)
    {
        UsingTradingAccountSize      = overrideTradingAccountSize;
        AdjustTradingAccountSizePercentage   = adjustTradingAccountSizePercentage;
        MaxTickerOpenPositionPercentage      = maxTickerOpenPositionPercentage;
        MaxTickerOpeningOrderSizePercentage  = maxTickerOpeningOrderSizePercentage;
        MaxTickerReducingOrderSizePercentage = maxTickerReducingOrderSizePercentage;
    }

    public RelativeTickerTradingLimitsConfig(IRelativeTickerTradingLimitsConfig toClone, IConfigurationRoot root, string path) : base(toClone, root, path)
    {
        UsingTradingAccountSize      = toClone.UsingTradingAccountSize;
        AdjustTradingAccountSizePercentage   = toClone.AdjustTradingAccountSizePercentage;
        MaxTickerOpenPositionPercentage      = toClone.MaxTickerOpenPositionPercentage;
        MaxTickerOpeningOrderSizePercentage  = toClone.MaxTickerOpeningOrderSizePercentage;
        MaxTickerReducingOrderSizePercentage = toClone.MaxTickerReducingOrderSizePercentage;
    }

    public RelativeTickerTradingLimitsConfig(IRelativeTickerTradingLimitsConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public IAccountTradingLimitsConfig? ParentAccountSizingConfig { get; set; }

    public decimal? UsingTradingAccountSize
    {
        get => decimal.TryParse(this[nameof(UsingTradingAccountSize)], out var fixedTradingAccountSize) ? fixedTradingAccountSize : null;
        set => this[nameof(UsingTradingAccountSize)] = value.ToString();
    }

    public decimal? AdjustTradingAccountSizePercentage
    {
        get => decimal.TryParse(this[nameof(AdjustTradingAccountSizePercentage)], out var adjustTradingAccountSizePercentage) ? adjustTradingAccountSizePercentage : null;
        set => this[nameof(AdjustTradingAccountSizePercentage)] = value.ToString();
    }

    public decimal? MaxTickerOpenPositionPercentage
    {
        get => decimal.TryParse(this[nameof(MaxTickerOpenPositionPercentage)], out var maxTickerOpenPositionPercentage) ? maxTickerOpenPositionPercentage : null;
        set => this[nameof(MaxTickerOpenPositionPercentage)] = value.ToString();
    }

    public decimal? MaxTickerOpeningOrderSizePercentage
    {
        get => decimal.TryParse(this[nameof(MaxTickerOpeningOrderSizePercentage)], out var maxTickerOpeningOrderSizePercentage) ? maxTickerOpeningOrderSizePercentage : null;
        set => this[nameof(MaxTickerOpeningOrderSizePercentage)] = value.ToString();
    }

    public decimal? MaxTickerReducingOrderSizePercentage
    {
        get => decimal.TryParse(this[nameof(MaxTickerReducingOrderSizePercentage)], out var maxTickerReducingOrderSizePercentage) ? maxTickerReducingOrderSizePercentage : null;
        set => this[nameof(MaxTickerReducingOrderSizePercentage)] = value.ToString();
    }

    IRelativeTickerTradingLimitsConfig ICloneable<IRelativeTickerTradingLimitsConfig>.Clone() => Clone();

    IRelativeTickerTradingLimitsConfig IRelativeTickerTradingLimitsConfig.            Clone() => Clone();

    public override RelativeTickerTradingLimitsConfig Clone() => new (this);

    public override bool AreEquivalent(IFixedTickerTradingLimitsConfig? other, bool exactTypes = false)
    {
        if (other is not IRelativeTickerTradingLimitsConfig relativeLimitsConfig) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var tradingAcctSizeSame           = UsingTradingAccountSize == relativeLimitsConfig.UsingTradingAccountSize;
        var adjustTradingAcctSizePctSame  = AdjustTradingAccountSizePercentage == relativeLimitsConfig.AdjustTradingAccountSizePercentage;
        var maxTickerOpenPosPctSame       = MaxTickerOpenPositionPercentage == relativeLimitsConfig.MaxTickerOpenPositionPercentage;
        var maxTickerOpenOrderSzPctSame   = MaxTickerOpeningOrderSizePercentage == relativeLimitsConfig.MaxTickerOpeningOrderSizePercentage;
        var maxTickerReduceOrderSzPctSame = MaxTickerReducingOrderSizePercentage == relativeLimitsConfig.MaxTickerReducingOrderSizePercentage;

        var allAreSame = baseSame && tradingAcctSizeSame && adjustTradingAcctSizePctSame && maxTickerOpenPosPctSame
                      && maxTickerOpenOrderSzPctSame && maxTickerReduceOrderSzPctSame;

        return allAreSame;
    }

    public new static void ClearValues(IConfigurationRoot root, string path)
    {
        root[$"{path}{Split}{nameof(UsingTradingAccountSize)}"]      = null;
        root[$"{path}{Split}{nameof(AdjustTradingAccountSizePercentage)}"]   = null;
        root[$"{path}{Split}{nameof(MaxTickerOpenPositionPercentage)}"]      = null;
        root[$"{path}{Split}{nameof(MaxTickerOpeningOrderSizePercentage)}"]  = null;
        root[$"{path}{Split}{nameof(MaxTickerReducingOrderSizePercentage)}"] = null;
        FixedTickerTradingLimitsConfig.ClearValues(root, path);
    }

    protected bool Equals(IAccountTradingLimitsConfig other) => AreEquivalent(other, true);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IAccountTradingLimitsConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ (UsingTradingAccountSize?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ (AdjustTradingAccountSizePercentage?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ (MaxTickerOpenPositionPercentage?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ (MaxTickerOpeningOrderSizePercentage?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ (MaxTickerReducingOrderSizePercentage?.GetHashCode() ?? 0);
            return hashCode;
        }
    }

    public string RelativeTickerLimitsToStringMembers =>
        $"{nameof(UsingTradingAccountSize)}: {UsingTradingAccountSize}, {nameof(AdjustTradingAccountSizePercentage)}: {AdjustTradingAccountSizePercentage}, " +
        $"{nameof(MaxTickerOpenPositionPercentage)}: {MaxTickerOpenPositionPercentage}, {nameof(MaxTickerOpeningOrderSizePercentage)}: {MaxTickerOpeningOrderSizePercentage}, " +
        $"{nameof(MaxTickerReducingOrderSizePercentage)}: {MaxTickerReducingOrderSizePercentage}, {FixedTickerTradingLimitsConfigToStringMembers}";

    public override string ToString() => $"{nameof(RelativeTickerTradingLimitsConfig)}{{{RelativeTickerLimitsToStringMembers}}}";
}
