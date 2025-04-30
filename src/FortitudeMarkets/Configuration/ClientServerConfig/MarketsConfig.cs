// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using System.Text.Json.Serialization;
using FortitudeCommon.Configuration;
using FortitudeCommon.Extensions;
using FortitudeIO.Transports.Network.Config;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeMarkets.Configuration.ClientServerConfig;

public interface IMarketsConfig : IConnection, IEnumerable<IMarketConnectionConfig>
{
    IDictionary<string, IMarketConnectionConfig> Markets { get; set; }

    void                     Add(IMarketConnectionConfig toAdd);
    IMarketConnectionConfig? Find(string name);
    IMarketsConfig           ToggleProtocolDirection(string connectionName);
    IMarketsConfig           ShiftPortsBy(ushort deltaPorts);
}

public class MarketsConfig : ConfigSection, IMarketsConfig
{
    private Dictionary<string, MarketConnectionConfig> markets = new();

    public MarketsConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public MarketsConfig(string connectionName, IConfigurationRoot root, string path) : base(root, path) => ConnectionName = connectionName;

    public MarketsConfig(string connectionName) : this(connectionName, InMemoryConfigRoot, InMemoryPath) { }

    public MarketsConfig(string connectionName, IEnumerable<IMarketConnectionConfig> marketConnectionConfigs) : this(connectionName
   , InMemoryConfigRoot, InMemoryPath) =>
        markets = marketConnectionConfigs.ToDictionary(mcc => mcc.SourceName, mcc => new MarketConnectionConfig(mcc));

    public MarketsConfig(string connectionName, params IMarketConnectionConfig[] marketConnectionConfigs) : this(connectionName, InMemoryConfigRoot
   , InMemoryPath) =>
        markets = marketConnectionConfigs.ToDictionary(mcc => mcc.SourceName, mcc => new MarketConnectionConfig(mcc));

    public MarketsConfig(IMarketsConfig toClone, IConfigurationRoot root, string path) : this(root, path)
    {
        ConnectionName = toClone.ConnectionName;
        Markets        = toClone.Markets.ToDictionary(mccKvp => mccKvp.Key, mccKvp => new MarketConnectionConfig(mccKvp.Value));
    }

    public MarketsConfig(IMarketsConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    private IEnumerable<IConfigurationSection> NonEmptyConfigs =>
        GetSection(nameof(Markets)).GetChildren().Where(cs => cs[nameof(IMarketConnectionConfig.SourceId)] != null);

    public Dictionary<string, MarketConnectionConfig> Markets
    {
        get
        {
            foreach (var configurationSection in NonEmptyConfigs)
            {
                var marketConnectionConfig = new MarketConnectionConfig(ConfigRoot, configurationSection.Path);
                if (marketConnectionConfig.SourceId != 0 && configurationSection.Key != marketConnectionConfig.SourceName)
                    throw new
                        ArgumentException($"The key name '{configurationSection.Key}' for a ticker config does not match the configured ticker Value {marketConnectionConfig.SourceName}");
                if (marketConnectionConfig.SourceName.IsEmpty()) marketConnectionConfig.SourceName = configurationSection.Key;
                if (!markets.ContainsKey(configurationSection.Key))
                    markets.TryAdd(configurationSection.Key, marketConnectionConfig);
                else
                    markets[configurationSection.Key] = marketConnectionConfig;
            }
            return markets;
        }
        set
        {
            var oldKeys = markets.Keys.ToHashSet();
            foreach (var marketConConfigKvp in value)
            {
                var checkMarketConConfig = new MarketConnectionConfig(marketConConfigKvp.Value, ConfigRoot
                                                                    , Path + ":" + nameof(Markets) + $":{marketConConfigKvp.Key}");
                if (marketConConfigKvp.Value.SourceName.IsNotNullOrEmpty() && marketConConfigKvp.Key != marketConConfigKvp.Value.SourceName)
                    throw new
                        ArgumentException($"The key name '{marketConConfigKvp.Key}' for a ticker config does not match the configured ticker Value {marketConConfigKvp.Value.SourceName}");
                if (marketConConfigKvp.Value.SourceName.IsNullOrEmpty()) checkMarketConConfig.SourceName = marketConConfigKvp.Key;
            }

            var deletedKeys = oldKeys.Except(value.Keys.ToHashSet());
            foreach (var deletedKey in deletedKeys) MarketConnectionConfig.ClearValues(ConfigRoot, Path + ":" + nameof(Markets) + $":{deletedKey}");
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<IMarketConnectionConfig> GetEnumerator() => Markets.Values.GetEnumerator();

    public void Add(IMarketConnectionConfig toAdd)
    {
        markets.Add(toAdd.SourceName, new MarketConnectionConfig(toAdd));
    }

    public IMarketConnectionConfig? Find(string name) =>
        Markets.Values
               .FirstOrDefault(sci => sci.SourceName?.Equals(name, StringComparison.InvariantCultureIgnoreCase) ?? false);

    public IMarketsConfig ToggleProtocolDirection(string connectionName)
    {
        var toggledConfig = new MarketsConfig(connectionName, Markets.Values.Select(mcc => mcc.ToggleProtocolDirection(connectionName)));
        return toggledConfig;
    }

    [JsonIgnore]
    IDictionary<string, IMarketConnectionConfig> IMarketsConfig.Markets
    {
        get
        {
            foreach (var configurationSection in NonEmptyConfigs)
            {
                var marketConnectionConfig = new MarketConnectionConfig(ConfigRoot, configurationSection.Path);
                if (marketConnectionConfig.SourceId != 0 && configurationSection.Key != marketConnectionConfig.SourceName)
                    throw new
                        ArgumentException($"The key name '{configurationSection.Key}' for a ticker config does not match the configured ticker Value {marketConnectionConfig.SourceName}");
                if (marketConnectionConfig.SourceName.IsEmpty()) marketConnectionConfig.SourceName = configurationSection.Key;
                if (!markets.ContainsKey(configurationSection.Key))
                    markets.TryAdd(configurationSection.Key, marketConnectionConfig);
                else
                    markets[configurationSection.Key] = marketConnectionConfig;
            }
            return markets.ToDictionary(tc => tc.Key, tc => (IMarketConnectionConfig)tc.Value);
        }
        set
        {
            var oldKeys = markets.Keys.ToHashSet();
            foreach (var marketConConfigKvp in value)
            {
                var checkMarketConConfig = new MarketConnectionConfig(marketConConfigKvp.Value, ConfigRoot
                                                                    , Path + ":" + nameof(Markets) + $":{marketConConfigKvp.Key}");
                if (marketConConfigKvp.Value.SourceName.IsNotNullOrEmpty() && marketConConfigKvp.Key != marketConConfigKvp.Value.SourceName)
                    throw new
                        ArgumentException($"The key name '{marketConConfigKvp.Key}' for a ticker config does not match the configured ticker Value {marketConConfigKvp.Value.SourceName}");
                if (marketConConfigKvp.Value.SourceName.IsNullOrEmpty()) checkMarketConConfig.SourceName = marketConConfigKvp.Key;
            }

            var deletedKeys = oldKeys.Except(value.Keys.ToHashSet());
            foreach (var deletedKey in deletedKeys) MarketConnectionConfig.ClearValues(ConfigRoot, Path + ":" + nameof(Markets) + $":{deletedKey}");
        }
    }

    public string? ConnectionName
    {
        get => this[nameof(ConnectionName)];
        set
        {
            if (value == ConnectionName) return;
            this[nameof(ConnectionName)] = value;
            foreach (var marketConnectionConfig in Markets.Values) marketConnectionConfig.ConnectionName = value;
        }
    }

    public IMarketsConfig ShiftPortsBy(ushort deltaPorts)
    {
        var shiftedMarketsConfig = new MarketsConfig(this)
        {
            Markets = Markets.ToDictionary(mccKvp => mccKvp.Key, mccKvp => mccKvp.Value.ShiftPortsBy(deltaPorts))
        };
        return shiftedMarketsConfig;
    }

    public void Add(MarketConnectionConfig toAdd)
    {
        markets.Add(toAdd.SourceName, toAdd);
    }

    public void Add(KeyValuePair<string, IMarketConnectionConfig> toAdd)
    {
        markets.Add(toAdd.Key, new MarketConnectionConfig(toAdd.Value));
    }

    public void Add(KeyValuePair<string, MarketConnectionConfig> toAdd)
    {
        markets.Add(toAdd.Key, toAdd.Value);
    }

    public bool AddOrUpdate(IMarketConnectionConfig item)
    {
        if (Markets.ContainsKey(item.SourceName))
            Markets[item.SourceName] = new MarketConnectionConfig(item, ConfigRoot, Path + ":" + nameof(Markets) + $":{item.SourceName}")
            {
                ConnectionName = ConnectionName
            };
        else
            Markets.Add(item.SourceName, new MarketConnectionConfig(item, ConfigRoot, Path + ":" + nameof(Markets) + $":{item.SourceName}")
            {
                ConnectionName = ConnectionName
            });
        return true;
    }
}
