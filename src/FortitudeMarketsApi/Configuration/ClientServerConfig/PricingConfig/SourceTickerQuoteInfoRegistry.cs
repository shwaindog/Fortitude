// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeMarketsApi.Pricing.Quotes;

#endregion

namespace FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

public interface ISourceTickerQuoteInfoRegistry
{
    string Name { get; }

    IEnumerable<ISourceTickerQuoteInfo> AllSourceTickerInfos { get; }

    event Action<IEnumerable<ISourceTickerQuoteInfo>> UpdatedSourceTickerInfos;

    IEnumerable<ISourceTickerQuoteInfo> GetAllSourceTickerInfoForTicker(string ticker);
    IEnumerable<ISourceTickerQuoteInfo> GetAllSourceTickerInfoForSource(string source);

    ISourceTickerQuoteInfo? GetSourceTickerInfo(string sourceName, string ticker);

    void AppendReplace(IEnumerable<ISourceTickerQuoteInfo> toAmendAdd);
}

public class SourceTickerQuoteInfoRegistry : ISourceTickerQuoteInfoRegistry
{
    private readonly IMap<string, List<ISourceTickerQuoteInfo>> sourceTickerMap = new ConcurrentMap<string, List<ISourceTickerQuoteInfo>>();

    public SourceTickerQuoteInfoRegistry(string name, IEnumerable<ISourceTickerQuoteInfo>? initialization = null)
    {
        Name = name;
        if (initialization != null) AppendReplace(initialization);
    }

    public string Name { get; }


    public event Action<IEnumerable<ISourceTickerQuoteInfo>>? UpdatedSourceTickerInfos;

    public IEnumerable<ISourceTickerQuoteInfo> AllSourceTickerInfos => sourceTickerMap.SelectMany(kvp => kvp.Value);

    public IEnumerable<ISourceTickerQuoteInfo> GetAllSourceTickerInfoForTicker(string ticker) =>
        sourceTickerMap.SelectMany(kvp => kvp.Value).Where(stqi => stqi.Ticker == ticker);

    public IEnumerable<ISourceTickerQuoteInfo> GetAllSourceTickerInfoForSource(string source) =>
        sourceTickerMap.SelectMany(kvp => kvp.Value).Where(stqi => stqi.Source == source);

    public ISourceTickerQuoteInfo? GetSourceTickerInfo(string sourceName, string ticker) =>
        sourceTickerMap.TryGetValue(sourceName, out List<ISourceTickerQuoteInfo>? sourceTickerList)
            ? sourceTickerList!.FirstOrDefault(stqi => stqi.Ticker == ticker)
            : null;

    public void AppendReplace(IEnumerable<ISourceTickerQuoteInfo> toAmendAdd)
    {
        foreach (var sourceTickerQuoteInfo in toAmendAdd)
        {
            List<ISourceTickerQuoteInfo>? sourceTickerList;
            if (!sourceTickerMap.TryGetValue(sourceTickerQuoteInfo.Source, out sourceTickerList))
            {
                sourceTickerList = new List<ISourceTickerQuoteInfo> { sourceTickerQuoteInfo };
                sourceTickerMap.Add(sourceTickerQuoteInfo.Source, sourceTickerList);
            }
            else
            {
                var foundExisting = sourceTickerList!.FirstOrDefault(stqi => stqi.Ticker == sourceTickerQuoteInfo.Ticker);
                if (foundExisting != null) sourceTickerList!.Remove(foundExisting);
                sourceTickerList!.Add(sourceTickerQuoteInfo);
            }
        }

        UpdatedSourceTickerInfos?.Invoke(toAmendAdd);
    }
}
