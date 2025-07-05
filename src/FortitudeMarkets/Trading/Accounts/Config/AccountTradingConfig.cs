// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Configuration;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeMarkets.Trading.Accounts.Config;

public interface IAccountTradingConfig : IAccountTradingLimitsConfig, ICloneable<IAccountTradingConfig>
{
    uint AccountId { get; set; }

    string AccountName { get; set; }

    PositionsDirectionFlags AllowedTradingDirection { get; set; }

    IDictionary<string, ITickerAccountConfig> Tickers { get; set; }

    IReadOnlyList<uint> AllowedOnBehalfOfAccountIds { get; set; }

    IReadOnlyDictionary<uint, IBookingAccountDistributionConfig> DefaultBookingAccounts { get; set; }

    IReadOnlyDictionary<uint, IBookingAccountDistributionConfig> AvailableBookingAccounts { get; set; }

    IAccountThroughputLimitsConfig DefaultAccountThroughputLimitsConfig { get; set; }

    IDailyAccountLimitsConfig? DailyLimits { get; set; }

    new IAccountTradingConfig Clone();
}

public class AccountTradingConfig : AccountTradingLimitsConfig, IAccountTradingConfig
{
    private readonly Dictionary<string, ITickerAccountConfig>            tickers                  = new ();
    private readonly Dictionary<uint, IBookingAccountDistributionConfig> availableBookingAccounts = new();
    private readonly Dictionary<uint, IBookingAccountDistributionConfig> defaultBookingAccounts   = new();

    private readonly List<uint> allowedOnBehalfOfAccountIds = new();

    public AccountTradingConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public AccountTradingConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public AccountTradingConfig
    (decimal? overrideTradingAccountSize = null, decimal? adjustTradingAccountSizePercentage = null
      , decimal? maxTotalOpenPositionPercentage = null, decimal? maxFixedTotalOpenPosition = null
      , decimal? maxTickerOpenPositionPercentage = null, decimal? maxFixedTickerOpenPosition = null
      , decimal? maxTickerOpeningOrderSizePercentage = null, decimal? maxFixedTickerOpeningOrderSize = null
      , decimal? maxTickerReducingOrderSizePercentage = null, decimal? maxFixedTickerReducingOrderSize = null)
        : this(InMemoryConfigRoot, InMemoryPath, overrideTradingAccountSize, adjustTradingAccountSizePercentage
             , maxTotalOpenPositionPercentage, maxFixedTotalOpenPosition, maxTickerOpenPositionPercentage
             , maxFixedTickerOpenPosition, maxTickerOpeningOrderSizePercentage, maxFixedTickerOpeningOrderSize, maxTickerReducingOrderSizePercentage
             , maxFixedTickerReducingOrderSize) { }

    public AccountTradingConfig
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

    public AccountTradingConfig(IAccountTradingConfig toClone, IConfigurationRoot root, string path) : base(toClone, root, path)
    {
        MaxTotalOpenPositionPercentage = toClone.MaxTotalOpenPositionPercentage;
        MaxFixedTotalOpenPosition      = toClone.MaxFixedTotalOpenPosition;
    }

    public AccountTradingConfig(IAccountTradingConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

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

    public IReadOnlyList<uint> AllowedOnBehalfOfAccountIds
    {
        get
        {
            if (GetSection(nameof(AllowedOnBehalfOfAccountIds)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                foreach (var configurationSection in GetSection(nameof(AllowedOnBehalfOfAccountIds)).GetChildren())
                {
                    if (uint.TryParse(configurationSection.Value, out var accountId) && accountId > 0) allowedOnBehalfOfAccountIds.Add(accountId);
                }
            }
            return allowedOnBehalfOfAccountIds;
        }
        set
        {
            var oldCount = allowedOnBehalfOfAccountIds.Count;
            allowedOnBehalfOfAccountIds.Clear();

            for (var i = 0; i < value.Count; i++)
            {
                var onBehalfOfAccountId = value[i];
                this[$"{nameof(AllowedOnBehalfOfAccountIds)}{Split}{i}"] = onBehalfOfAccountId.ToString();
            }

            for (var i = value.Count; i < oldCount; i++)
            {
                this[$"{nameof(AllowedOnBehalfOfAccountIds)}{Split}{i}"] = null;
            }
        }
    }

    public PositionsDirectionFlags AllowedTradingDirection
    {
        get =>
            Enum.TryParse<PositionsDirectionFlags>(this[nameof(AllowedTradingDirection)], out var allowedTradingDirection)
                ? allowedTradingDirection
                : PositionsDirectionFlags.Default;
        set => this[nameof(AllowedTradingDirection)] = value.ToString();
    }

    public IReadOnlyDictionary<uint, IBookingAccountDistributionConfig> AvailableBookingAccounts
    {
        get
        {
            if (!availableBookingAccounts.Any())
            {
                if (GetSection(nameof(AvailableBookingAccounts)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                {
                    foreach (var configurationSection in GetSection(nameof(AvailableBookingAccounts)).GetChildren())
                    {
                        var tickerConfig = new BookingAccountDistributionConfig(ConfigRoot, configurationSection.Path);

                        if (uint.TryParse(configurationSection.Key, out var key))
                        {
                            if (!availableBookingAccounts.ContainsKey(key))
                                availableBookingAccounts.TryAdd(key, tickerConfig);
                            else
                                availableBookingAccounts[key] = tickerConfig;
                        }
                    }
                }
            }
            return availableBookingAccounts;
        }
        set
        {
            var oldKeys = availableBookingAccounts.Keys.ToHashSet();
            availableBookingAccounts.Clear();
            foreach (var tickerConfigKvp in value)
            {
                var checkTickerConfig = new BookingAccountDistributionConfig(tickerConfigKvp.Value, ConfigRoot
                                                                           , $"{Path}{Split}{nameof(AvailableBookingAccounts)}:{tickerConfigKvp.Key}");
                availableBookingAccounts.Add(tickerConfigKvp.Key, checkTickerConfig);
            }

            var deletedKeys = oldKeys.Except(value.Keys.ToHashSet());
            foreach (var deletedKey in deletedKeys) BookingAccountDistributionConfig.ClearValues(ConfigRoot, $"{Path}{Split}{nameof(AvailableBookingAccounts)}{Split}{deletedKey}");
        }
    }

    public IDailyAccountLimitsConfig? DailyLimits
    {
        get
        {
            if (GetSection(nameof(DailyLimits)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new DailyAccountLimitsConfig(ConfigRoot, $"{Path}{Split}{nameof(DailyLimits)}");
            }
            throw new ConfigurationErrorsException($"Expected {nameof(DailyLimits)} to be configured");
        }
        set => _ = value != null ? new DailyAccountLimitsConfig(value, ConfigRoot, $"{Path}{Split}{nameof(DailyLimits)}") : null;
    }

    public IAccountThroughputLimitsConfig DefaultAccountThroughputLimitsConfig
    {
        get
        {
            if (GetSection(nameof(DefaultAccountThroughputLimitsConfig)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new AccountThroughputLimitsConfig(ConfigRoot, $"{Path}{Split}{nameof(DefaultAccountThroughputLimitsConfig)}");
            }
            throw new ConfigurationErrorsException($"Expected {nameof(DefaultAccountThroughputLimitsConfig)} to be configured");
        }
        set => _ = new AccountThroughputLimitsConfig(value, ConfigRoot, $"{Path}{Split}{nameof(DefaultAccountThroughputLimitsConfig)}");
    }

    public IReadOnlyDictionary<uint, IBookingAccountDistributionConfig> DefaultBookingAccounts
    {
        get
        {
            if (!defaultBookingAccounts.Any())
            {
                if (GetSection(nameof(DefaultBookingAccounts)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                {
                    foreach (var configurationSection in GetSection(nameof(DefaultBookingAccounts)).GetChildren())
                    {
                        var tickerConfig = new BookingAccountDistributionConfig(ConfigRoot, configurationSection.Path);

                        if (uint.TryParse(configurationSection.Key, out var key))
                        {
                            if (!defaultBookingAccounts.ContainsKey(key))
                                defaultBookingAccounts.TryAdd(key, tickerConfig);
                            else
                                defaultBookingAccounts[key] = tickerConfig;
                        }
                    }
                }
            }
            return defaultBookingAccounts;
        }
        set
        {
            var oldKeys = defaultBookingAccounts.Keys.ToHashSet();
            defaultBookingAccounts.Clear();
            foreach (var tickerConfigKvp in value)
            {
                var checkTickerConfig = new BookingAccountDistributionConfig(tickerConfigKvp.Value, ConfigRoot
                                                                           , $"{Path}{Split}{nameof(DefaultBookingAccounts)}:{tickerConfigKvp.Key}");
                defaultBookingAccounts.Add(tickerConfigKvp.Key, checkTickerConfig);
            }

            var deletedKeys = oldKeys.Except(value.Keys.ToHashSet());
            foreach (var deletedKey in deletedKeys) BookingAccountDistributionConfig.ClearValues(ConfigRoot, $"{Path}{Split}{nameof(DefaultBookingAccounts)}{Split}{deletedKey}");
        }
    }

    public IDictionary<string, ITickerAccountConfig> Tickers
    {
        get
        {
            if (!tickers.Any())
            {
                if (GetSection(nameof(Tickers)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                {
                    foreach (var configurationSection in GetSection(nameof(Tickers)).GetChildren())
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
                                                              , $"{Path}{Split}{nameof(Tickers)}:{tickerConfigKvp.Key}");
                if (tickerConfigKvp.Value.InstrumentName.IsNotNullOrEmpty() && tickerConfigKvp.Key != tickerConfigKvp.Value.InstrumentName)
                    throw new
                        ArgumentException($"The key name '{tickerConfigKvp.Key}' for a ticker config does not match the configured ticker Value {tickerConfigKvp.Value.InstrumentName}");
                if (tickerConfigKvp.Value.InstrumentName.IsNullOrEmpty()) checkTickerConfig.InstrumentName = tickerConfigKvp.Key;
                tickers.Add(tickerConfigKvp.Key, checkTickerConfig);
            }

            var deletedKeys = oldKeys.Except(value.Keys.ToHashSet());
            foreach (var deletedKey in deletedKeys) TickerAccountConfig.ClearValues(ConfigRoot, $"{Path}{Split}{nameof(Tickers)}{Split}{deletedKey}");
        }
    }

    IAccountTradingConfig ICloneable<IAccountTradingConfig>.Clone() => Clone();

    IAccountTradingConfig IAccountTradingConfig.Clone() => Clone();

    IRelativeTickerTradingLimitsConfig ICloneable<IRelativeTickerTradingLimitsConfig>.Clone() => Clone();

    IRelativeTickerTradingLimitsConfig IRelativeTickerTradingLimitsConfig.Clone() => Clone();

    public override AccountTradingConfig Clone() => new(this);

    public override bool AreEquivalent(IFixedTickerTradingLimitsConfig? other, bool exactTypes = false)
    {
        if (other is not IAccountTradingConfig accountTradingConfig) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var acctIdSame              = AccountId == accountTradingConfig.AccountId;
        var acctNameSame            = AccountName == accountTradingConfig.AccountName;
        var onBehalfOfAllowedSame   = AllowedOnBehalfOfAccountIds.SequenceEqual(accountTradingConfig.AllowedOnBehalfOfAccountIds);
        var tradingDirectionSame = AllowedTradingDirection == accountTradingConfig.AllowedTradingDirection;
        var availableActsSame      = AvailableBookingAccounts.Values.SequenceEqual(accountTradingConfig.AvailableBookingAccounts.Values);
        var defaultBookingActsSame      = DefaultBookingAccounts.Values.SequenceEqual(accountTradingConfig.DefaultBookingAccounts.Values);
        var dailyLimitsSame      = DailyLimits?.AreEquivalent(accountTradingConfig.DailyLimits) ?? accountTradingConfig.DailyLimits == null;
        var throughputLimitsSame      = DefaultAccountThroughputLimitsConfig.AreEquivalent(accountTradingConfig.DefaultAccountThroughputLimitsConfig);
        var tickerConfigsSame        = Tickers?.Values.SequenceEqual(accountTradingConfig.Tickers?.Values ?? Array.Empty<ITickerAccountConfig>()) 
                                    ?? accountTradingConfig.Tickers.IsNullOrNone();

        var allAreSame = baseSame && acctIdSame && acctNameSame && onBehalfOfAllowedSame && tradingDirectionSame && availableActsSame && defaultBookingActsSame 
                      && dailyLimitsSame && throughputLimitsSame && tickerConfigsSame;

        return allAreSame;
    }

    public new static void ClearValues(IConfigurationRoot root, string path)
    {
        root[$"{path}{Split}{nameof(AccountId)}"]                            = null;
        root[$"{path}{Split}{nameof(AccountName)}"]                          = null;
        root[$"{path}{Split}{nameof(AllowedOnBehalfOfAccountIds)}"]          = null;
        root[$"{path}{Split}{nameof(AllowedTradingDirection)}"]              = null;
        root[$"{path}{Split}{nameof(AvailableBookingAccounts)}"]             = null;
        root[$"{path}{Split}{nameof(DefaultBookingAccounts)}"]               = null;
        root[$"{path}{Split}{nameof(DailyLimits)}"]                          = null;
        root[$"{path}{Split}{nameof(DefaultAccountThroughputLimitsConfig)}"] = null;
        root[$"{path}{Split}{nameof(Tickers)}"]                              = null;
        AccountTradingLimitsConfig.ClearValues(root, path);
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IAccountTradingLimitsConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)AccountId;
            hashCode = (hashCode * 397) ^ AccountName.GetHashCode();
            hashCode = (hashCode * 397) ^ AllowedOnBehalfOfAccountIds.GetHashCode();
            hashCode = (hashCode * 397) ^ AllowedTradingDirection.GetHashCode();
            hashCode = (hashCode * 397) ^ AvailableBookingAccounts.GetHashCode();
            hashCode = (hashCode * 397) ^ DefaultBookingAccounts.GetHashCode();
            hashCode = (hashCode * 397) ^ (DailyLimits?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ DefaultAccountThroughputLimitsConfig.GetHashCode();
            hashCode = (hashCode * 397) ^ Tickers.GetHashCode();
            return hashCode;
        }
    }

    public string AccountTradingConfigToStringMembers =>
        $"{nameof(AccountId)}: {AccountId}, {nameof(AccountName)}: {AccountName}, {nameof(AllowedOnBehalfOfAccountIds)}: {AllowedOnBehalfOfAccountIds}, " +
        $"{nameof(AllowedTradingDirection)}: {AllowedTradingDirection}, {nameof(AvailableBookingAccounts)}: {AvailableBookingAccounts}, " +
        $"{nameof(DailyLimits)}: {DailyLimits}, {nameof(DefaultAccountThroughputLimitsConfig)}: {DefaultAccountThroughputLimitsConfig}, " +
        $"{nameof(Tickers)}: {Tickers}, {AccountTradingLimitsConfigToStringMembers}";

    public override string ToString() => $"{nameof(AccountTradingConfig)}{{{AccountTradingConfigToStringMembers}}}";
}
