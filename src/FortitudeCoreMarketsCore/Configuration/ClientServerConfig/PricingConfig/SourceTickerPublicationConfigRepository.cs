#region

using System.Collections;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

#endregion

namespace FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig;

public class SourceTickerPublicationConfigRepository : ISourceTickerPublicationConfigRepository
{
    private readonly Dictionary<string, ISourceTickerPublicationConfig?> defs = new();

    public SourceTickerPublicationConfigRepository(IEnumerable<ISourceTickerPublicationConfig> initialise)
    {
        foreach (var sourceTickerPublicationConfig in initialise)
            defs.Add(sourceTickerPublicationConfig.Ticker, sourceTickerPublicationConfig);
    }


    public ISourceTickerPublicationConfig? this[string symbol] => defs.TryGetValue(symbol, out var def) ? def : null;

    public IEnumerator<ISourceTickerPublicationConfig> GetEnumerator() => defs.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
