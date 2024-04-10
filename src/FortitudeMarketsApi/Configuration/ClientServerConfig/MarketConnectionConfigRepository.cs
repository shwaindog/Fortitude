#region

using System.Reactive.Linq;
using System.Reactive.Subjects;
using FortitudeCommon.Configuration;
using FortitudeCommon.EventProcessing;

#endregion

namespace FortitudeMarketsApi.Configuration.ClientServerConfig;

public interface IMarketConnectionConfigRepository
{
    IEnumerable<IMarketConnectionConfig> CurrentConfigs { get; }
    IMarketConnectionConfig? Find(string name);
    IMarketConnectionConfigRepository ToggleProtocolDirection();
}

public class MarketConnectionConfigRepository : IMarketConnectionConfigRepository
{
    private readonly Dictionary<ushort, IMarketConnectionConfig> currentConfigs = new ();

    public MarketConnectionConfigRepository() => currentConfigs = [];

    public MarketConnectionConfigRepository(IEnumerable<IMarketConnectionConfig> serverConfigs)
    {
        CurrentConfigs = serverConfigs;
    }
    public MarketConnectionConfigRepository(params IMarketConnectionConfig[] serverConfigs) : this((IEnumerable<IMarketConnectionConfig>)serverConfigs)
    {
    }

    public MarketConnectionConfigRepository(IMarketConnectionConfigRepository toClone)
    {
        CurrentConfigs = toClone.CurrentConfigs.Select( mcc => mcc.Clone());
    }

    public IEnumerable<IMarketConnectionConfig> CurrentConfigs
    {
        get => currentConfigs.Values;
        private init => currentConfigs = value.ToDictionary(item => item.SourceId);
    }

    public IMarketConnectionConfig? Find(string name)
    {
        return currentConfigs.Select(kvp => kvp.Value)
            .FirstOrDefault(sci => sci.Name?.Equals(name, StringComparison.InvariantCultureIgnoreCase) ?? false);
    }

    public bool ContainsItem(IMarketConnectionConfig checkItem) => currentConfigs.ContainsKey(checkItem.SourceId);

    public bool Update(IMarketConnectionConfig item)
    {
        var containsItem = ContainsItem(item);
        if (!containsItem) return false; // doesn't exist
        currentConfigs[item.SourceId] = item;
        return true;
    }

    public bool Add(IMarketConnectionConfig item)
    {
        var containsItem = ContainsItem(item);
        if (containsItem) return false;
        currentConfigs.Add(item.SourceId, item);
        return true;
    }

    public bool AddOrUpdate(IMarketConnectionConfig item) => Update(item) || Add(item);

    public bool Delete(IMarketConnectionConfig item)
    {
        var itemIndex = currentConfigs.ContainsKey(item.SourceId);
        if (!itemIndex) return false;
        currentConfigs.Remove(item.SourceId);
        return true;
    }

    public IMarketConnectionConfigRepository ToggleProtocolDirection() => new MarketConnectionConfigRepository(this)
    {
        CurrentConfigs = CurrentConfigs.Select( mcc => mcc.ToggleProtocolDirection())
    };
}
