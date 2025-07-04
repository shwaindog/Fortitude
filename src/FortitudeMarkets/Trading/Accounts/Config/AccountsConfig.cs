using System.Configuration;
using System.Globalization;
using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeMarkets.Trading.Accounts.Config;

public interface IAccountsConfig : IInterfacesComparable<IAccountsConfig>, ICloneable<IAccountsConfig>
{
    bool TradingEnabled { get; set; }

    decimal UsingTradingAccountSize { get; set; }

    decimal MaxTotalOpenPositionPercentage { get; set; }

    decimal MaxTickerOpenPositionPercentage { get; set; }

    decimal MaxTickerOpeningOrderSizePercentage { get; set; }

    decimal? MaxTickerReducingOrderSizePercentage { get; set; }

    ISourceAccountTradingConfig SourceAccount { get; set; }

    IAccountThroughputLimitsConfig AdapterThroughputLimitsConfig { get; set; }

    IReadOnlyDictionary<string, ITickerAccountConfig> AllAccountsTickers { get; set; }

    IReadOnlyDictionary<uint, IAccountTradingConfig> TradingAccounts { get; set; }

    IDailyAccountLimitsConfig DefaultDailyLimits { get; set; }
}

public class AccountsConfig : ConfigSection, IAccountsConfig
{
    private readonly Dictionary<string, ITickerAccountConfig> allAccountTickers = new();
    private readonly Dictionary<uint, IAccountTradingConfig>  tradingAccounts   = new();

    public AccountsConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public AccountsConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public AccountsConfig
    (bool tradingEnabled, ISourceAccountTradingConfig sourceTradingAccount, decimal usingTradingAccountSize, decimal maxTotalOpenPositionPercentage
      , decimal maxTickerOpenPositionPercentage, decimal maxTickerOpeningOrderSizePercentage
      , IReadOnlyDictionary<uint, IAccountTradingConfig> tradingAccounts
      , IAccountThroughputLimitsConfig adapterThroughputLimits, IDailyAccountLimitsConfig defaultDailyLimits
      , IReadOnlyDictionary<string, ITickerAccountConfig>? allAccountsTickers = null, decimal? maxTickerReducingOrderSizePercentage = null)
        : this(InMemoryConfigRoot, InMemoryPath, tradingEnabled, sourceTradingAccount, usingTradingAccountSize, maxTotalOpenPositionPercentage
             , maxTickerOpenPositionPercentage, maxTickerOpeningOrderSizePercentage, tradingAccounts, adapterThroughputLimits, defaultDailyLimits
             , allAccountsTickers, maxTickerReducingOrderSizePercentage) { }

    public AccountsConfig
    (IConfigurationRoot root, string path, bool tradingEnabled, ISourceAccountTradingConfig sourceTradingAccount, decimal usingTradingAccountSize
      , decimal maxTotalOpenPositionPercentage, decimal maxTickerOpenPositionPercentage, decimal maxTickerOpeningOrderSizePercentage
      , IReadOnlyDictionary<uint, IAccountTradingConfig> tradingAccounts, IAccountThroughputLimitsConfig adapterThroughputLimits
      , IDailyAccountLimitsConfig defaultDailyLimits, IReadOnlyDictionary<string, ITickerAccountConfig>? allAccountsTickers = null
      , decimal? maxTickerReducingOrderSizePercentage = null)
        : base(root, path)
    {
        TradingEnabled                       = tradingEnabled;
        UsingTradingAccountSize              = usingTradingAccountSize;
        MaxTotalOpenPositionPercentage       = maxTotalOpenPositionPercentage;
        MaxTickerOpenPositionPercentage      = maxTickerOpenPositionPercentage;
        MaxTickerOpeningOrderSizePercentage  = maxTickerOpeningOrderSizePercentage;
        SourceAccount                        = sourceTradingAccount;
        TradingAccounts                      = tradingAccounts;
        AdapterThroughputLimitsConfig        = adapterThroughputLimits;
        DefaultDailyLimits                   = defaultDailyLimits;
        AllAccountsTickers                   = allAccountsTickers ?? new Dictionary<string, ITickerAccountConfig>();
        MaxTickerReducingOrderSizePercentage = maxTickerReducingOrderSizePercentage;
    }

    public AccountsConfig(IAccountsConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        TradingEnabled                       = toClone.TradingEnabled;
        UsingTradingAccountSize              = toClone.UsingTradingAccountSize;
        MaxTotalOpenPositionPercentage       = toClone.MaxTotalOpenPositionPercentage;
        MaxTickerOpenPositionPercentage      = toClone.MaxTickerOpenPositionPercentage;
        MaxTickerOpeningOrderSizePercentage  = toClone.MaxTickerOpeningOrderSizePercentage;
        SourceAccount                        = toClone.SourceAccount;
        TradingAccounts                      = toClone.TradingAccounts;
        AdapterThroughputLimitsConfig        = toClone.AdapterThroughputLimitsConfig;
        DefaultDailyLimits                   = toClone.DefaultDailyLimits;
        AllAccountsTickers                   = toClone.AllAccountsTickers;
        MaxTickerReducingOrderSizePercentage = toClone.MaxTickerReducingOrderSizePercentage;
    }

    public AccountsConfig(IAccountsConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public bool TradingEnabled
    {
        get => bool.TryParse(this[nameof(TradingEnabled)], out var alwaysSubmitWithStopLoss) && alwaysSubmitWithStopLoss;
        set => this[nameof(TradingEnabled)] = value.ToString();
    }

    public decimal UsingTradingAccountSize
    {
        get =>
            decimal.TryParse(this[nameof(UsingTradingAccountSize)], out var maxTotalOpenPositionPercentage)
                ? maxTotalOpenPositionPercentage
                : 0m;
        set => this[nameof(UsingTradingAccountSize)] = value.ToString(CultureInfo.InvariantCulture);
    }

    public decimal MaxTotalOpenPositionPercentage
    {
        get =>
            decimal.TryParse(this[nameof(MaxTotalOpenPositionPercentage)], out var maxTotalOpenPositionPercentage)
                ? maxTotalOpenPositionPercentage
                : 0m;
        set => this[nameof(MaxTotalOpenPositionPercentage)] = value.ToString(CultureInfo.InvariantCulture);
    }

    public decimal MaxTickerOpenPositionPercentage
    {
        get =>
            decimal.TryParse(this[nameof(MaxTickerOpenPositionPercentage)], out var maxTickerOpenPositionPercentage)
                ? maxTickerOpenPositionPercentage
                : 0m;
        set => this[nameof(MaxTickerOpenPositionPercentage)] = value.ToString(CultureInfo.InvariantCulture);
    }

    public decimal MaxTickerOpeningOrderSizePercentage
    {
        get =>
            decimal.TryParse(this[nameof(MaxTickerOpeningOrderSizePercentage)], out var maxTickerOpeningOrderSizePercentage)
                ? maxTickerOpeningOrderSizePercentage
                : 0m;
        set => this[nameof(MaxTickerOpeningOrderSizePercentage)] = value.ToString(CultureInfo.InvariantCulture);
    }

    public decimal? MaxTickerReducingOrderSizePercentage
    {
        get =>
            decimal.TryParse(this[nameof(MaxTickerReducingOrderSizePercentage)], out var maxTickerReducingOrderSizePercentage)
                ? maxTickerReducingOrderSizePercentage
                : null;
        set => this[nameof(MaxTickerReducingOrderSizePercentage)] = value.ToString();
    }

    public IAccountThroughputLimitsConfig AdapterThroughputLimitsConfig
    {
        get
        {
            if (GetSection(nameof(AdapterThroughputLimitsConfig)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new AccountThroughputLimitsConfig(ConfigRoot, $"{Path}{Split}{nameof(AdapterThroughputLimitsConfig)}");
            }
            throw new ConfigurationErrorsException($"Expected {nameof(AdapterThroughputLimitsConfig)} to be configured");
        }
        set => _ = new AccountThroughputLimitsConfig(value, ConfigRoot, $"{Path}{Split}{nameof(AdapterThroughputLimitsConfig)}");
    }

    public IReadOnlyDictionary<string, ITickerAccountConfig> AllAccountsTickers
    {
        get
        {
            if (!allAccountTickers.Any())
            {
                if (GetSection(nameof(AllAccountsTickers)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                {
                    foreach (var configurationSection in GetSection(nameof(AllAccountsTickers)).GetChildren())
                    {
                        var tickerConfig = new TickerAccountConfig(ConfigRoot, configurationSection.Path);
                        if (tickerConfig.InstrumentName.IsNotNullOrEmpty() && configurationSection.Key != tickerConfig.InstrumentName)
                            throw new
                                ArgumentException($"The key name '{configurationSection.Key}' for a ticker config does not match the configured ticker Value {tickerConfig.InstrumentName}");
                        if (tickerConfig.InstrumentName.IsNullOrEmpty()) tickerConfig.InstrumentName = configurationSection.Key;
                        if (!allAccountTickers.ContainsKey(configurationSection.Key))
                            allAccountTickers.TryAdd(configurationSection.Key, tickerConfig);
                        else
                            allAccountTickers[configurationSection.Key] = tickerConfig;
                    }
                }
            }
            return allAccountTickers;
        }
        set
        {
            var oldKeys = allAccountTickers.Keys.ToHashSet();
            allAccountTickers.Clear();
            foreach (var tickerConfigKvp in value)
            {
                var checkTickerConfig = new TickerAccountConfig(tickerConfigKvp.Value, ConfigRoot
                                                              , $"{Path}{Split}{nameof(AllAccountsTickers)}:{tickerConfigKvp.Key}");
                if (tickerConfigKvp.Value.InstrumentName.IsNotNullOrEmpty() && tickerConfigKvp.Key != tickerConfigKvp.Value.InstrumentName)
                    throw new
                        ArgumentException($"The key name '{tickerConfigKvp.Key}' for a ticker config does not match the configured ticker Value {tickerConfigKvp.Value.InstrumentName}");
                if (tickerConfigKvp.Value.InstrumentName.IsNullOrEmpty()) checkTickerConfig.InstrumentName = tickerConfigKvp.Key;
                allAccountTickers.Add(tickerConfigKvp.Key, checkTickerConfig);
            }

            var deletedKeys = oldKeys.Except(value.Keys.ToHashSet());
            foreach (var deletedKey in deletedKeys)
                TickerAccountConfig.ClearValues(ConfigRoot, $"{Path}{Split}{nameof(AllAccountsTickers)}{Split}{deletedKey}");
        }
    }

    public ISourceAccountTradingConfig SourceAccount
    {
        get
        {
            if (GetSection(nameof(SourceAccount)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new SourceAccountTradingConfig(ConfigRoot, $"{Path}{Split}{nameof(SourceAccount)}");
            }
            throw new ConfigurationErrorsException($"Expected {nameof(SourceAccount)} to be configured");
        }
        set => _ = new SourceAccountTradingConfig(value, ConfigRoot, $"{Path}{Split}{nameof(SourceAccount)}");
    }

    public IReadOnlyDictionary<uint, IAccountTradingConfig> TradingAccounts
    {
        get
        {
            if (!tradingAccounts.Any())
            {
                if (GetSection(nameof(TradingAccounts)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                {
                    foreach (var configurationSection in GetSection(nameof(TradingAccounts)).GetChildren())
                    {
                        var accountTradingConfig = new AccountTradingConfig(ConfigRoot, configurationSection.Path);

                        if (uint.TryParse(configurationSection.Key, out var key))
                        {
                            if (!tradingAccounts.ContainsKey(key))
                                tradingAccounts.TryAdd(key, accountTradingConfig);
                            else
                                tradingAccounts[key] = accountTradingConfig;
                        }
                    }
                }
            }
            return tradingAccounts;
        }
        set
        {
            var oldKeys = tradingAccounts.Keys.ToHashSet();
            tradingAccounts.Clear();
            foreach (var tickerConfigKvp in value)
            {
                var checkTickerConfig = new AccountTradingConfig(tickerConfigKvp.Value, ConfigRoot
                                                               , $"{Path}{Split}{nameof(TradingAccounts)}:{tickerConfigKvp.Key}");
                tradingAccounts.Add(tickerConfigKvp.Key, checkTickerConfig);
            }

            var deletedKeys = oldKeys.Except(value.Keys.ToHashSet());
            foreach (var deletedKey in deletedKeys)
                TickerAccountConfig.ClearValues(ConfigRoot, $"{Path}{Split}{nameof(TradingAccounts)}{Split}{deletedKey}");
        }
    }

    public IDailyAccountLimitsConfig DefaultDailyLimits
    {
        get
        {
            if (GetSection(nameof(DefaultDailyLimits)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new DailyAccountLimitsConfig(ConfigRoot, $"{Path}{Split}{nameof(DefaultDailyLimits)}");
            }
            throw new ConfigurationErrorsException($"Expected {nameof(DefaultDailyLimits)} to be configured");
        }
        set => _ = new DailyAccountLimitsConfig(value, ConfigRoot, $"{Path}{Split}{nameof(DefaultDailyLimits)}");
    }


    IAccountsConfig ICloneable<IAccountsConfig>.Clone() => Clone();

    object ICloneable.Clone() => Clone();

    public virtual AccountsConfig Clone() => new(this);

    public virtual bool AreEquivalent(IAccountsConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var tradingEnabledSame            = TradingEnabled == other.TradingEnabled;
        var tradingAcctSizeSame           = UsingTradingAccountSize == other.UsingTradingAccountSize;
        var maxTotalOpenPosPctSame        = MaxTotalOpenPositionPercentage == other.MaxTotalOpenPositionPercentage;
        var maxTickerOpenPosPctSame       = MaxTickerOpenPositionPercentage == other.MaxTickerOpenPositionPercentage;
        var maxTickerOpenOrderSzPctSame   = MaxTickerOpeningOrderSizePercentage == other.MaxTickerOpeningOrderSizePercentage;
        var maxTickerReduceOrderSzPctSame = MaxTickerReducingOrderSizePercentage == other.MaxTickerReducingOrderSizePercentage;
        var adapterThroughputLimitsSame   = AdapterThroughputLimitsConfig.AreEquivalent(other.AdapterThroughputLimitsConfig);
        var tickerConfigsSame             = AllAccountsTickers.Values.SequenceEqual(other.AllAccountsTickers.Values);
        var sourceAcctSame                = SourceAccount.AreEquivalent(other.SourceAccount);
        var tradingAcctSame               = TradingAccounts.Values.SequenceEqual(other.TradingAccounts.Values);
        var defaultDailyLimitsSame               = DefaultDailyLimits.AreEquivalent(other.DefaultDailyLimits);


        var allAreSame = tradingEnabledSame && tradingAcctSizeSame && maxTotalOpenPosPctSame && maxTickerOpenPosPctSame && maxTickerOpenOrderSzPctSame 
                      && maxTickerReduceOrderSzPctSame && adapterThroughputLimitsSame && tickerConfigsSame && sourceAcctSame && tradingAcctSame && defaultDailyLimitsSame;

        return allAreSame;
    }


    public static void ClearValues(IConfigurationRoot root, string path)
    {
        root[$"{path}{Split}{nameof(TradingEnabled)}"]                       = null;
        root[$"{path}{Split}{nameof(UsingTradingAccountSize)}"]              = null;
        root[$"{path}{Split}{nameof(MaxTotalOpenPositionPercentage)}"]       = null;
        root[$"{path}{Split}{nameof(MaxTickerOpenPositionPercentage)}"]      = null;
        root[$"{path}{Split}{nameof(MaxTickerOpeningOrderSizePercentage)}"]  = null;
        root[$"{path}{Split}{nameof(MaxTickerReducingOrderSizePercentage)}"] = null;
        root[$"{path}{Split}{nameof(AdapterThroughputLimitsConfig)}"]        = null;
        root[$"{path}{Split}{nameof(AllAccountsTickers)}"]                   = null;
        root[$"{path}{Split}{nameof(SourceAccount)}"]                        = null;
        root[$"{path}{Split}{nameof(TradingAccounts)}"]                      = null;
        root[$"{path}{Split}{nameof(DefaultDailyLimits)}"]                   = null;
        RelativeTickerTradingLimitsConfig.ClearValues(root, path);
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IAccountsConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = TradingEnabled.GetHashCode();
            hashCode = (hashCode * 397) ^ UsingTradingAccountSize.GetHashCode();
            hashCode = (hashCode * 397) ^ MaxTotalOpenPositionPercentage.GetHashCode();
            hashCode = (hashCode * 397) ^ MaxTickerOpenPositionPercentage.GetHashCode();
            hashCode = (hashCode * 397) ^ MaxTickerOpeningOrderSizePercentage.GetHashCode();
            hashCode = (hashCode * 397) ^ MaxTickerReducingOrderSizePercentage.GetHashCode();
            hashCode = (hashCode * 397) ^ AdapterThroughputLimitsConfig.GetHashCode();
            hashCode = (hashCode * 397) ^ AllAccountsTickers.GetHashCode();
            hashCode = (hashCode * 397) ^ SourceAccount.GetHashCode();
            hashCode = (hashCode * 397) ^ TradingAccounts.GetHashCode();
            hashCode = (hashCode * 397) ^ DefaultDailyLimits.GetHashCode();
            return hashCode;
        }
    }

    public string AccountTradingLimitsConfigToStringMembers =>
        $"{nameof(TradingEnabled)}: {TradingEnabled}, {nameof(SourceAccount)}: {SourceAccount}, {nameof(UsingTradingAccountSize)}: {UsingTradingAccountSize}, " +
        $"{nameof(MaxTotalOpenPositionPercentage)}: {MaxTotalOpenPositionPercentage}, {nameof(MaxTickerOpenPositionPercentage)}: {MaxTickerOpenPositionPercentage}, " +
        $"{nameof(MaxTickerOpeningOrderSizePercentage)}: {MaxTickerOpeningOrderSizePercentage}, " +
        $"{nameof(TradingAccounts)}: {TradingAccounts}, {nameof(AdapterThroughputLimitsConfig)}: {AdapterThroughputLimitsConfig}, " +
        $"{nameof(DefaultDailyLimits)}: {DefaultDailyLimits}, {nameof(AllAccountsTickers)}: {AllAccountsTickers}, " +
        $"{nameof(MaxTickerReducingOrderSizePercentage)}: {MaxTickerReducingOrderSizePercentage}";

    public override string ToString() => $"{nameof(AccountTradingLimitsConfig)}{{{AccountTradingLimitsConfigToStringMembers}}}";
}
