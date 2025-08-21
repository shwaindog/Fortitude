// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Maps;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

#endregion

namespace FortitudeMarkets.Config.PricingConfig;

public interface ISourceTickerInfoRegistry
{
    string Name { get; }

    IEnumerable<ISourceTickerInfo> AllSourceTickerInfos { get; }

    event Action<IEnumerable<ISourceTickerInfo>> UpdatedSourceTickerInfos;

    IEnumerable<ISourceTickerInfo> GetAllSourceTickerInfoForTicker(string ticker);
    IEnumerable<ISourceTickerInfo> GetAllSourceTickerInfoForSource(string source);

    ISourceTickerInfo? GetSourceTickerInfo(string sourceName, string ticker);

    void AppendReplace(IEnumerable<ISourceTickerInfo> toAmendAdd);
}

public class SourceTickerInfoRegistry : ISourceTickerInfoRegistry
{
    private readonly IMap<string, List<ISourceTickerInfo>> sourceTickerMap = new ConcurrentMap<string, List<ISourceTickerInfo>>();

    public SourceTickerInfoRegistry(string name, IEnumerable<ISourceTickerInfo>? initialization = null)
    {
        Name = name;
        if (initialization != null) AppendReplace(initialization);
    }

    public string Name { get; }


    public event Action<IEnumerable<ISourceTickerInfo>>? UpdatedSourceTickerInfos;

    public IEnumerable<ISourceTickerInfo> AllSourceTickerInfos => sourceTickerMap.SelectMany(kvp => kvp.Value);

    public IEnumerable<ISourceTickerInfo> GetAllSourceTickerInfoForTicker(string ticker) =>
        sourceTickerMap.SelectMany(kvp => kvp.Value).Where(stqi => stqi.InstrumentName == ticker);

    public IEnumerable<ISourceTickerInfo> GetAllSourceTickerInfoForSource(string source) =>
        sourceTickerMap.SelectMany(kvp => kvp.Value).Where(stqi => stqi.SourceName == source);

    public ISourceTickerInfo? GetSourceTickerInfo(string sourceName, string ticker) =>
        sourceTickerMap.TryGetValue(sourceName, out List<ISourceTickerInfo>? sourceTickerList)
            ? sourceTickerList!.FirstOrDefault(stqi => stqi.InstrumentName == ticker)
            : null;

    public void AppendReplace(IEnumerable<ISourceTickerInfo> toAmendAdd)
    {
        foreach (var sourceTickerInfo in toAmendAdd)
        {
            List<ISourceTickerInfo>? sourceTickerList;
            if (!sourceTickerMap.TryGetValue(sourceTickerInfo.SourceName, out sourceTickerList))
            {
                sourceTickerList = new List<ISourceTickerInfo> { sourceTickerInfo };
                sourceTickerMap.TryAdd(sourceTickerInfo.SourceName, sourceTickerList);
            }
            else
            {
                var foundExisting = sourceTickerList!.FirstOrDefault(stqi => stqi.InstrumentName == sourceTickerInfo.InstrumentName);
                if (foundExisting != null) sourceTickerList!.Remove(foundExisting);
                sourceTickerList!.Add(sourceTickerInfo);
            }
        }

        UpdatedSourceTickerInfos?.Invoke(toAmendAdd);
    }
}
