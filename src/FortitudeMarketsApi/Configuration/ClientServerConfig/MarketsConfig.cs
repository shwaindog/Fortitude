#region

using FortitudeCommon.Configuration;
using FortitudeCommon.DataStructures.Lists;
using FortitudeIO.Transports.Network.Config;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeMarketsApi.Configuration.ClientServerConfig;

public interface IMarketsConfig : IConnection
{
    IEnumerable<IMarketConnectionConfig> Markets { get; }
    IMarketConnectionConfig? Find(string name);
    IMarketsConfig ToggleProtocolDirection(string connectionName);
    IMarketsConfig ShiftPortsBy(ushort deltaPorts);
}

public class MarketsConfig : ConfigSection, IMarketsConfig
{
    private object? ignoreSuppressWarnings;

    public MarketsConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public MarketsConfig(string connectionName, IConfigurationRoot root, string path) : base(root, path) => ConnectionName = connectionName;

    public MarketsConfig(string connectionName) : this(connectionName, InMemoryConfigRoot, InMemoryPath) { }

    public MarketsConfig(string connectionName, IEnumerable<IMarketConnectionConfig> marketConnectionConfigs) : this(connectionName
        , InMemoryConfigRoot, InMemoryPath) =>
        Markets = marketConnectionConfigs;

    public MarketsConfig(string connectionName, params IMarketConnectionConfig[] marketConnectionConfigs) : this(connectionName, InMemoryConfigRoot
        , InMemoryPath) =>
        Markets = marketConnectionConfigs;

    public MarketsConfig(IMarketsConfig toClone, IConfigurationRoot root, string path) : this(root, path)
    {
        ConnectionName = toClone.ConnectionName;
        Markets = toClone.Markets;
    }

    public MarketsConfig(IMarketsConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public IMarketConnectionConfig? Find(string name) =>
        Markets
            .FirstOrDefault(sci => sci.Name?.Equals(name, StringComparison.InvariantCultureIgnoreCase) ?? false);

    public IMarketsConfig ToggleProtocolDirection(string connectionName)
    {
        var toggledConfig = new MarketsConfig(connectionName, Markets.Select(mcc => mcc.ToggleProtocolDirection(connectionName)));
        return toggledConfig;
    }

    public IEnumerable<IMarketConnectionConfig> Markets
    {
        get
        {
            var autoRecycleList = Recycler.Borrow<AutoRecycledEnumerable<IMarketConnectionConfig>>();
            foreach (var configurationSection in GetSection(nameof(Markets)).GetChildren())
                if (configurationSection["Name"] != null)
                    autoRecycleList.Add(new MarketConnectionConfig(ConfigRoot, configurationSection.Path));
            return autoRecycleList;
        }
        set
        {
            var oldCount = Markets.Count();
            var i = 0;
            foreach (var marketConnectionConfig in value)
            {
                ignoreSuppressWarnings
                    = new MarketConnectionConfig(marketConnectionConfig, ConfigRoot, Path + ":" + nameof(Markets) + $":{i}")
                    {
                        ConnectionName = ConnectionName
                    };
                i++;
            }

            for (var j = i; j < oldCount; j++) MarketConnectionConfig.ClearValues(ConfigRoot, Path + ":" + nameof(Markets) + $":{i}");
        }
    }

    public string? ConnectionName
    {
        get => this[nameof(ConnectionName)];
        set
        {
            if (value == ConnectionName) return;
            this[nameof(ConnectionName)] = value;
            foreach (var marketConnectionConfig in Markets) marketConnectionConfig.ConnectionName = value;
        }
    }

    public IMarketsConfig ShiftPortsBy(ushort deltaPorts)
    {
        var shiftedMarketsConfig = new MarketsConfig(this)
        {
            Markets = Markets.Select(mcc => mcc.ShiftPortsBy(deltaPorts))
        };
        return shiftedMarketsConfig;
    }

    public bool AddOrUpdate(IMarketConnectionConfig item)
    {
        var i = 0;
        foreach (var existingConfig in Markets)
        {
            if (existingConfig.Name == item.Name)
            {
                ignoreSuppressWarnings
                    = new MarketConnectionConfig(existingConfig, ConfigRoot, Path + ":" + nameof(Markets) + $":{i}")
                    {
                        ConnectionName = ConnectionName
                    };
                return true;
            }

            i++;
        }

        Markets = Markets.Concat(new[] { item });
        return true;
    }
}
