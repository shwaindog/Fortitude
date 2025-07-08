// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeMarkets.Trading.Accounts.Config;

public interface ISourceAccountTradingConfig : IAccountTradingLimitsConfig, ICloneable<ISourceAccountTradingConfig>
{
    uint AccountId { get; set; }

    string AccountName { get; set; }

    decimal? SourceMarginCollateral { get; set; }

    string? SourceMarginCollateralCurrency { get; set; }

    decimal? MarginReserveMultiple { get; set; }

    decimal? NewOrderConsumePips { get; set; }

    IReadOnlyDictionary<string, ITickerAccountConfig> OverrideTickers { get; set; }

    IDailyAccountLimitsConfig? DailyLimits { get; set; }

    new ISourceAccountTradingConfig Clone();
}

public class SourceAccountTradingConfig : AccountTradingLimitsConfig, ISourceAccountTradingConfig
{
    private readonly Dictionary<string, ITickerAccountConfig> tickers = new();

    public SourceAccountTradingConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public SourceAccountTradingConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public SourceAccountTradingConfig
    (uint accountId, string accountName, IReadOnlyDictionary<string, ITickerAccountConfig> sourceTradeableTickers
        , IDailyAccountLimitsConfig? sourceDailyLimits, decimal? sourceMarginCollateral = null, string? sourceMarginCollateralCurrency = null
        , decimal? marginReserveMultiple = null, decimal? newOrderConsumePips = null
     ,  decimal? overrideTradingAccountSize = null, decimal? adjustTradingAccountSizePercentage = null
      , decimal? maxTotalOpenPositionPercentage = null, decimal? maxFixedTotalOpenPosition = null
      , decimal? maxTickerOpenPositionPercentage = null, decimal? maxFixedTickerOpenPosition = null
      , decimal? maxTickerOpeningOrderSizePercentage = null, decimal? maxFixedTickerOpeningOrderSize = null
      , decimal? maxTickerReducingOrderSizePercentage = null, decimal? maxFixedTickerReducingOrderSize = null)
        : this(InMemoryConfigRoot, InMemoryPath, accountId, accountName, sourceTradeableTickers, sourceDailyLimits, sourceMarginCollateral
              , sourceMarginCollateralCurrency, marginReserveMultiple, newOrderConsumePips, overrideTradingAccountSize, adjustTradingAccountSizePercentage 
             , maxTotalOpenPositionPercentage, maxFixedTotalOpenPosition, maxTickerOpenPositionPercentage
             , maxFixedTickerOpenPosition, maxTickerOpeningOrderSizePercentage, maxFixedTickerOpeningOrderSize, maxTickerReducingOrderSizePercentage
             , maxFixedTickerReducingOrderSize) { }

    public SourceAccountTradingConfig
    (IConfigurationRoot root, string path, uint accountId, string accountName, IReadOnlyDictionary<string, ITickerAccountConfig> sourceTradeableTickers
      , IDailyAccountLimitsConfig? sourceDailyLimits, decimal? sourceMarginCollateral = null, string? sourceMarginCollateralCurrency = null
      , decimal? marginReserveMultiple = null, decimal? newOrderConsumePips = null
      , decimal? overrideTradingAccountSize = null, decimal? adjustTradingAccountSizePercentage = null
      , decimal? maxTotalOpenPositionPercentage = null, decimal? maxFixedTotalOpenPosition = null
      , decimal? maxTickerOpenPositionPercentage = null, decimal? maxFixedTickerOpenPosition = null
      , decimal? maxTickerOpeningOrderSizePercentage = null, decimal? maxFixedTickerOpeningOrderSize = null
      , decimal? maxTickerReducingOrderSizePercentage = null, decimal? maxFixedTickerReducingOrderSize = null)
        : base(root, path, overrideTradingAccountSize, adjustTradingAccountSizePercentage, maxTotalOpenPositionPercentage, maxFixedTotalOpenPosition
            ,  maxTickerOpenPositionPercentage, maxFixedTickerOpenPosition, maxTickerOpeningOrderSizePercentage
             , maxFixedTickerOpeningOrderSize, maxTickerReducingOrderSizePercentage, maxFixedTickerReducingOrderSize)
    {
        AccountId                      = accountId;
        AccountName                    = accountName;
        OverrideTickers         = sourceTradeableTickers;
        DailyLimits                    = sourceDailyLimits;
        SourceMarginCollateral         = sourceMarginCollateral;
        SourceMarginCollateralCurrency = sourceMarginCollateralCurrency;
        MarginReserveMultiple          = marginReserveMultiple;
        NewOrderConsumePips            = newOrderConsumePips;
    }

    public SourceAccountTradingConfig(ISourceAccountTradingConfig toClone, IConfigurationRoot root, string path) : base(toClone, root, path)
    {
        MaxTotalOpenPositionPercentage = toClone.MaxTotalOpenPositionPercentage;
        MaxFixedTotalOpenPosition      = toClone.MaxFixedTotalOpenPosition;
    }

    public SourceAccountTradingConfig(ISourceAccountTradingConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public uint AccountId
    {
        get => uint.TryParse(this[nameof(AccountId)], out var accountId) ? accountId : 0u;
        set => this[nameof(AccountId)] = value.ToString();
    }

    public string AccountName
    {
        get => this[nameof(AccountName)]!;
        set => this[nameof(AccountName)] = value;
    }

    public decimal? SourceMarginCollateral
    {
        get => decimal.TryParse(this[nameof(SourceMarginCollateral)], out var fixedTickerOpeningOrderSize) ? fixedTickerOpeningOrderSize : null;
        set => this[nameof(SourceMarginCollateral)] = value.ToString();
    }

    public string? SourceMarginCollateralCurrency
    {
        get => this[nameof(SourceMarginCollateralCurrency)];
        set => this[nameof(SourceMarginCollateralCurrency)] = value;
    }

    public decimal? MarginReserveMultiple
    {
        get => decimal.TryParse(this[nameof(MarginReserveMultiple)], out var fixedTickerOpeningOrderSize) ? fixedTickerOpeningOrderSize : null;
        set => this[nameof(MarginReserveMultiple)] = value.ToString();
    }

    public decimal? NewOrderConsumePips
    {
        get => decimal.TryParse(this[nameof(NewOrderConsumePips)], out var fixedTickerOpeningOrderSize) ? fixedTickerOpeningOrderSize : null;
        set => this[nameof(NewOrderConsumePips)] = value.ToString();
    }

    public IDailyAccountLimitsConfig? DailyLimits
    {
        get
        {
            if (GetSection(nameof(DailyLimits)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new DailyAccountLimitsConfig(ConfigRoot, $"{Path}{Split}{nameof(DailyLimits)}");
            }
            return null;
        }
        set => _ = value != null ? new DailyAccountLimitsConfig(value, ConfigRoot, $"{Path}{Split}{nameof(DailyLimits)}") : null;
    }

    public IReadOnlyDictionary<string, ITickerAccountConfig> OverrideTickers
    {
        get
        {
            if (!tickers.Any())
            {
                if (GetSection(nameof(OverrideTickers)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                {
                    foreach (var configurationSection in GetSection(nameof(OverrideTickers)).GetChildren())
                    {
                        var tickerConfig = new TickerAccountConfig(ConfigRoot, configurationSection.Path);
                        if (tickerConfig.InstrumentName.IsNotNullOrEmpty() && configurationSection.Key != tickerConfig.InstrumentName)
                            throw new
                                ArgumentException($"The key name '{configurationSection.Key}' for a ticker config does not match the configured ticker Value {tickerConfig.InstrumentName}");
                        if (tickerConfig.InstrumentName.IsNullOrEmpty()) tickerConfig.InstrumentName = configurationSection.Key;
                        if (!tickers.ContainsKey(configurationSection.Key))
                            tickers.TryAdd(configurationSection.Key, tickerConfig);
                        else
                            tickers[configurationSection.Key] = tickerConfig;
                    }
                }
            }
            return tickers;
        }
        set
        {
            var oldKeys = tickers.Keys.ToHashSet();
            tickers.Clear();
            foreach (var tickerConfigKvp in value)
            {
                var checkTickerConfig = new TickerAccountConfig(tickerConfigKvp.Value, ConfigRoot
                                                              , $"{Path}{Split}{nameof(OverrideTickers)}:{tickerConfigKvp.Key}");
                if (tickerConfigKvp.Value.InstrumentName.IsNotNullOrEmpty() && tickerConfigKvp.Key != tickerConfigKvp.Value.InstrumentName)
                    throw new
                        ArgumentException($"The key name '{tickerConfigKvp.Key}' for a ticker config does not match the configured ticker Value {tickerConfigKvp.Value.InstrumentName}");
                if (tickerConfigKvp.Value.InstrumentName.IsNullOrEmpty()) checkTickerConfig.InstrumentName = tickerConfigKvp.Key;
                tickers.Add(tickerConfigKvp.Key, checkTickerConfig);
            }

            var deletedKeys = oldKeys.Except(value.Keys.ToHashSet());
            foreach (var deletedKey in deletedKeys)
                TickerAccountConfig.ClearValues(ConfigRoot, $"{Path}{Split}{nameof(OverrideTickers)}{Split}{deletedKey}");
        }
    }

    ISourceAccountTradingConfig ICloneable<ISourceAccountTradingConfig>.Clone() => Clone();

    ISourceAccountTradingConfig ISourceAccountTradingConfig.Clone() => Clone();

    IRelativeTickerTradingLimitsConfig ICloneable<IRelativeTickerTradingLimitsConfig>.Clone() => Clone();

    IRelativeTickerTradingLimitsConfig IRelativeTickerTradingLimitsConfig.Clone() => Clone();

    public override SourceAccountTradingConfig Clone() => new(this);

    public override bool AreEquivalent(IFixedTickerTradingLimitsConfig? other, bool exactTypes = false)
    {
        if (other is not ISourceAccountTradingConfig srcAcctTradingConfig) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var acctIdSame              = AccountId == srcAcctTradingConfig.AccountId;
        var acctNameSame            = AccountName == srcAcctTradingConfig.AccountName;
        var srcMarginColSame        = SourceMarginCollateral == srcAcctTradingConfig.SourceMarginCollateral;
        var srcMarginColCcySame     = SourceMarginCollateralCurrency == srcAcctTradingConfig.SourceMarginCollateralCurrency;
        var marginReserveSame       = MarginReserveMultiple == srcAcctTradingConfig.MarginReserveMultiple;
        var newOrderConsumePipsSame = NewOrderConsumePips == srcAcctTradingConfig.NewOrderConsumePips;
        var dailyLimitsSame         = DailyLimits?.AreEquivalent(srcAcctTradingConfig.DailyLimits) ?? srcAcctTradingConfig.DailyLimits == null;
        var tickerConfigsSame       = OverrideTickers.Values.SequenceEqual(srcAcctTradingConfig.OverrideTickers.Values);

        var allAreSame = baseSame && acctIdSame && acctNameSame && srcMarginColSame && srcMarginColCcySame && marginReserveSame &&
                         newOrderConsumePipsSame
                      && dailyLimitsSame && tickerConfigsSame;

        return allAreSame;
    }

    public new static void ClearValues(IConfigurationRoot root, string path)
    {
        root[$"{path}{Split}{nameof(AccountId)}"]                      = null;
        root[$"{path}{Split}{nameof(AccountName)}"]                    = null;
        root[$"{path}{Split}{nameof(SourceMarginCollateral)}"]         = null;
        root[$"{path}{Split}{nameof(SourceMarginCollateralCurrency)}"] = null;
        root[$"{path}{Split}{nameof(MarginReserveMultiple)}"]          = null;
        root[$"{path}{Split}{nameof(NewOrderConsumePips)}"]            = null;
        root[$"{path}{Split}{nameof(DailyLimits)}"]                    = null;
        root[$"{path}{Split}{nameof(OverrideTickers)}"]         = null;
        RelativeTickerTradingLimitsConfig.ClearValues(root, path);
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IAccountTradingLimitsConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)AccountId;
            hashCode = (hashCode * 397) ^ AccountName.GetHashCode();
            hashCode = (hashCode * 397) ^ (SourceMarginCollateral?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ (SourceMarginCollateralCurrency?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ (MarginReserveMultiple?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ (NewOrderConsumePips?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ (DailyLimits?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ OverrideTickers.GetHashCode();
            return hashCode;
        }
    }

    public string AccountTradingConfigToStringMembers =>
        $"{nameof(AccountId)}: {AccountId}, {nameof(AccountName)}: {AccountName}, {nameof(SourceMarginCollateral)}: {SourceMarginCollateral}, " +
        $"{nameof(SourceMarginCollateralCurrency)}: {SourceMarginCollateralCurrency}, {nameof(MarginReserveMultiple)}: {MarginReserveMultiple}, " +
        $"{nameof(NewOrderConsumePips)}: {NewOrderConsumePips},{nameof(DailyLimits)}: {DailyLimits}, " +
        $"{nameof(OverrideTickers)}: {OverrideTickers}, {AccountTradingLimitsConfigToStringMembers}";

    public override string ToString() => $"{nameof(SourceAccountTradingConfig)}{{{AccountTradingConfigToStringMembers}}}";
}
