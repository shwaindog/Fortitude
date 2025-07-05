// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeMarkets.Trading.Accounts.Config;

public interface IDailyAccountLimitsConfig : IAccountTradingLimitsConfig, ICloneable<IDailyAccountLimitsConfig>
{
    IReadOnlyDictionary<string, ITickerAccountConfig>? Tickers { get; set; }

    new IDailyAccountLimitsConfig Clone();
}

public class DailyAccountLimitsConfig : AccountTradingLimitsConfig, IDailyAccountLimitsConfig
{
    private readonly Dictionary<string, ITickerAccountConfig> tickers = new ();

    public DailyAccountLimitsConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public DailyAccountLimitsConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public DailyAccountLimitsConfig
    (decimal? overrideTradingAccountSize = null, decimal? adjustTradingAccountSizePercentage = null
      , decimal? maxTotalOpenPositionPercentage = null, decimal? maxFixedTotalOpenPosition = null
      , IReadOnlyDictionary<string, ITickerAccountConfig>? dailyTickers = null
      , decimal? maxTickerOpenPositionPercentage = null, decimal? maxFixedTickerOpenPosition = null
      , decimal? maxTickerOpeningOrderSizePercentage = null, decimal? maxFixedTickerOpeningOrderSize = null
      , decimal? maxTickerReducingOrderSizePercentage = null, decimal? maxFixedTickerReducingOrderSize = null)
        : this(InMemoryConfigRoot, InMemoryPath, overrideTradingAccountSize, adjustTradingAccountSizePercentage
             , maxTotalOpenPositionPercentage, maxFixedTotalOpenPosition, dailyTickers, maxTickerOpenPositionPercentage, maxFixedTickerOpenPosition
             , maxTickerOpeningOrderSizePercentage, maxFixedTickerOpeningOrderSize, maxTickerReducingOrderSizePercentage
             , maxFixedTickerReducingOrderSize) { }

    public DailyAccountLimitsConfig
    (IConfigurationRoot root, string path, decimal? overrideTradingAccountSize = null, decimal? adjustTradingAccountSizePercentage = null
      , decimal? maxTotalOpenPositionPercentage = null, decimal? maxFixedTotalOpenPosition = null
      , IReadOnlyDictionary<string, ITickerAccountConfig>? dailyTickers = null
      , decimal? maxTickerOpenPositionPercentage = null, decimal? maxFixedTickerOpenPosition = null
      , decimal? maxTickerOpeningOrderSizePercentage = null, decimal? maxFixedTickerOpeningOrderSize = null
      , decimal? maxTickerReducingOrderSizePercentage = null, decimal? maxFixedTickerReducingOrderSize = null)
        : base(root, path, overrideTradingAccountSize, adjustTradingAccountSizePercentage, maxTotalOpenPositionPercentage, maxFixedTotalOpenPosition
             , maxTickerOpenPositionPercentage, maxFixedTickerOpenPosition, maxTickerOpeningOrderSizePercentage, maxFixedTickerOpeningOrderSize
             , maxTickerReducingOrderSizePercentage, maxFixedTickerReducingOrderSize)
    {
        Tickers = dailyTickers;
    }

    public DailyAccountLimitsConfig(IAccountTradingLimitsConfig toClone, IConfigurationRoot root, string path) : base(toClone, root, path)
    {
        MaxTotalOpenPositionPercentage = toClone.MaxTotalOpenPositionPercentage;
        MaxFixedTotalOpenPosition      = toClone.MaxFixedTotalOpenPosition;
    }

    public DailyAccountLimitsConfig(IAccountTradingLimitsConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public IReadOnlyDictionary<string, ITickerAccountConfig>? Tickers
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
            return tickers.Any() ? tickers : null;
        }
        set
        {
            var oldKeys = tickers.Keys.ToHashSet();
            tickers.Clear();
            if (value != null)
            {
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
            }

            var deletedKeys = oldKeys.Except(value?.Keys.ToHashSet() ?? []);
            foreach (var deletedKey in deletedKeys) TickerAccountConfig.ClearValues(ConfigRoot, $"{Path}{Split}{nameof(Tickers)}{Split}{deletedKey}");
        }
    }

    object ICloneable.Clone() => Clone();

    IDailyAccountLimitsConfig ICloneable<IDailyAccountLimitsConfig>.Clone() => Clone();

    IDailyAccountLimitsConfig IDailyAccountLimitsConfig.Clone() => Clone();

    public override DailyAccountLimitsConfig Clone() => new(this);

    public override bool AreEquivalent(IFixedTickerTradingLimitsConfig? other, bool exactTypes = false)
    {
        if (other is not IDailyAccountLimitsConfig dailyLimits) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);
        
        var tickerConfigsSame        = Tickers?.Values.SequenceEqual(dailyLimits.Tickers?.Values ?? Array.Empty<ITickerAccountConfig>()) ?? dailyLimits.Tickers.IsNullOrNone();

        var allAreSame = baseSame && tickerConfigsSame;

        return allAreSame;
    }

    public new static void ClearValues(IConfigurationRoot root, string path)
    {
        root[$"{path}{Split}{nameof(Tickers)}"]  = null;
        TickerThroughputLimitsConfig.ClearValues(root, path);
    }

    protected bool Equals(IDailyAccountLimitsConfig other) => AreEquivalent(other, true);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IDailyAccountLimitsConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ (Tickers?.GetHashCode() ?? 0);
            return hashCode;
        }
    }

    protected string DailyAccountLimitsConfigToStringMembers =>
        $"{nameof(Tickers)}: {Tickers}, {AccountTradingLimitsConfigToStringMembers}";

    public override string ToString() => $"{nameof(DailyAccountLimitsConfig)}{{{DailyAccountLimitsConfigToStringMembers}}}";
}
