// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Config;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.ConfigSources;

public interface IOrderedConfigSourcesLookupConfig : IFLogConfig, IStickyKeyValueDictionary<ushort, IFlogConfigSource>
  , IInterfacesComparable<IOrderedConfigSourcesLookupConfig>, ICloneable<IOrderedConfigSourcesLookupConfig>
  , IStringBearer
{
    TimeSpanConfig ExpireConfigCacheIntervalTimeSpan { get; }
}

public interface IAppendableOrderedConfigSourcesLookupConfig : IMutableFLogConfig, IOrderedConfigSourcesLookupConfig
  , IAppendableDictionary<ushort, IMutableFlogConfigSource>
{
    new TimeSpanConfig ExpireConfigCacheIntervalTimeSpan { get; set; }

    new IEnumerable<ushort> Keys { get; }

    new IEnumerable<IMutableFlogConfigSource> Values { get; }

    new int Count { get; }

    new IMutableFlogConfigSource this[ushort appenderName] { get; set; }

    new bool ContainsKey(ushort priority);

    new IEnumerator<KeyValuePair<ushort, IMutableFlogConfigSource>> GetEnumerator();
}

public class OrderedConfigSourcesLookupConfig : FLogConfig, IAppendableOrderedConfigSourcesLookupConfig
{
    private static TimeSpan recheckTimeSpanInterval;

    private readonly List<KeyValuePair<ushort, IMutableFlogConfigSource>> orderedConfigSources = new();

    private readonly Dictionary<ushort, IMutableFlogConfigSource> priorityConfigSources = new();

    private DateTime nextConfigReadTime = DateTime.MinValue;

    public OrderedConfigSourcesLookupConfig(IConfigurationRoot root, string path) : base(root, path) =>
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);

    public OrderedConfigSourcesLookupConfig() : this(InMemoryConfigRoot, InMemoryPath) => recheckTimeSpanInterval = TimeSpan.FromMinutes(1);

    public OrderedConfigSourcesLookupConfig(params IMutableFlogConfigSource[] toAdd)
        : this(InMemoryConfigRoot, InMemoryPath, toAdd) =>
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);

    public OrderedConfigSourcesLookupConfig
        (IConfigurationRoot root, string path, params IMutableFlogConfigSource[] toAdd) : base(root, path)
    {
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);
        for (var i = 0; i < toAdd.Length; i++) priorityConfigSources.Add(toAdd[i].ConfigPriorityOrder, toAdd[i]);
    }

    public OrderedConfigSourcesLookupConfig(IOrderedConfigSourcesLookupConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);
        foreach (var kvp in toClone) priorityConfigSources.Add(kvp.Key, (IMutableFlogConfigSource)kvp.Value);
    }

    public OrderedConfigSourcesLookupConfig(IOrderedConfigSourcesLookupConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) =>
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);

    [JsonIgnore]
    protected Dictionary<ushort, IMutableFlogConfigSource> CheckConfigGetConfigSourcesDict
    {
        get
        {
            if (!priorityConfigSources.Any() || nextConfigReadTime < TimeContext.UtcNow)
            {
                recheckTimeSpanInterval = FLogContext.NullOnUnstartedContext?.ConfigRegistry.ExpireConfigCacheIntervalTimeSpan ??
                                          TimeSpan.FromMinutes(1);
                priorityConfigSources.Clear();
                foreach (var configurationSection in GetSection(Path).GetChildren())
                {
                    var configSource = FLogContext.Context.ConfigRegistry.ConfigPathToConfigSourceConfig
                        (ConfigRoot, $"{configurationSection.Path}{Split}{configurationSection.Key}");
                    if (configSource != null && ushort.TryParse(configurationSection.Key, out var key))
                    {
                        configSource.ParentConfig = this;
                        priorityConfigSources.TryAdd(key, configSource);
                    }
                }
                nextConfigReadTime = TimeContext.UtcNow.Add(recheckTimeSpanInterval);
            }
            return priorityConfigSources;
        }
    }

    protected IReadOnlyList<KeyValuePair<ushort, IMutableFlogConfigSource>> OrderedConfigSources
    {
        get
        {
            if (nextConfigReadTime < TimeContext.UtcNow)
            {
                ClearAndCopyEitherTo(orderedConfigSources);
                orderedConfigSources.Sort((lhs, rhs) => lhs.Key - rhs.Key);
                nextConfigReadTime = TimeContext.UtcNow.Add(recheckTimeSpanInterval);
            }
            return orderedConfigSources;
        }
    }

    public TimeSpanConfig ExpireConfigCacheIntervalTimeSpan
    {
        get
        {
            if (GetSection(nameof(ExpireConfigCacheIntervalTimeSpan)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
                return new TimeSpanConfig(ConfigRoot, $"{Path}{Split}{nameof(ExpireConfigCacheIntervalTimeSpan)}");
            return IFlogConfigSource.DefaultRecheckConfigTimeSpan;
        }
        set => _ = new TimeSpanConfig(value, ConfigRoot, $"{Path}{Split}{nameof(ExpireConfigCacheIntervalTimeSpan)}");
    }

    public void Add(KeyValuePair<ushort, IMutableFlogConfigSource> item)
    {
        priorityConfigSources.Add(item.Key, item.Value);
        PushToConfigStorage(item.Value);
    }

    public void Add(ushort key, IMutableFlogConfigSource value)
    {
        priorityConfigSources.Add(key, value);
        PushToConfigStorage(value);
    }

    bool IReadOnlyDictionary<ushort, IFlogConfigSource>.ContainsKey
        (ushort priorityOrder) =>
        CheckConfigGetConfigSourcesDict.ContainsKey(priorityOrder);

    bool IAppendableOrderedConfigSourcesLookupConfig.ContainsKey(ushort priorityOrder) => CheckConfigGetConfigSourcesDict.ContainsKey(priorityOrder);

    public bool ContainsKey(ushort priorityOrder) => CheckConfigGetConfigSourcesDict.ContainsKey(priorityOrder);

    public int Count => CheckConfigGetConfigSourcesDict.Count;

    IEnumerator IEnumerable.GetEnumerator() => OrderedConfigSources.GetEnumerator();

    IEnumerator<KeyValuePair<ushort, IMutableFlogConfigSource>> IAppendableOrderedConfigSourcesLookupConfig.GetEnumerator() =>
        OrderedConfigSources.GetEnumerator();

    IEnumerator<KeyValuePair<ushort, IFlogConfigSource>> IEnumerable<KeyValuePair<ushort, IFlogConfigSource>>.GetEnumerator() =>
        OrderedConfigSources
            .Select(arcKvp => new KeyValuePair<ushort, IFlogConfigSource>(arcKvp.Key, arcKvp.Value))
            .GetEnumerator();

    public IEnumerator<KeyValuePair<ushort, IMutableFlogConfigSource>> GetEnumerator() => OrderedConfigSources.GetEnumerator();

    public IMutableFlogConfigSource this[ushort appenderName]
    {
        get => CheckConfigGetConfigSourcesDict[appenderName];
        set => CheckConfigGetConfigSourcesDict[appenderName] = value;
    }

    IFlogConfigSource IReadOnlyDictionary<ushort, IFlogConfigSource>.this[ushort appenderName] => this[appenderName];

    public IEnumerable<ushort> Keys => OrderedConfigSources.Select(kvp => kvp.Key);

    ICollection<ushort> IAppendableDictionary<ushort, IMutableFlogConfigSource>.Keys => OrderedConfigSources.Select(kvp => kvp.Key).ToList();

    public bool TryGetValue(ushort priorityOrder, [MaybeNullWhen(false)] out IMutableFlogConfigSource value) =>
        CheckConfigGetConfigSourcesDict.TryGetValue(priorityOrder, out value);

    public bool TryGetValue(ushort priorityOrder, [MaybeNullWhen(false)] out IFlogConfigSource value)
    {
        value = null;
        if (CheckConfigGetConfigSourcesDict.TryGetValue(priorityOrder, out var config))
        {
            value = config;
            return true;
        }
        return false;
    }

    public IEnumerable<IMutableFlogConfigSource> Values => OrderedConfigSources.Select(kvp => kvp.Value);

    IEnumerable<IFlogConfigSource> IReadOnlyDictionary<ushort, IFlogConfigSource>.Values => OrderedConfigSources.Select(kvp => kvp.Value);

    ICollection<IMutableFlogConfigSource> IAppendableDictionary<ushort, IMutableFlogConfigSource>.Values =>
        OrderedConfigSources.Select(kvp => kvp.Value).ToList();

    public override T Accept<T>(T visitor) => visitor.Visit(this);

    protected void PushToConfigStorage(IMutableFlogConfigSource value)
    {
        value.CloneConfigTo(ConfigRoot, $"{Path}{Split}{value.ConfigPriorityOrder}");
    }

    protected List<KeyValuePair<ushort, IMutableFlogConfigSource>> ClearAndCopyEitherTo(List<KeyValuePair<ushort, IMutableFlogConfigSource>> list)
    {
        static IMutableFlogConfigSource? LoadNewConfig
        (
            List<KeyValuePair<ushort, IMutableFlogConfigSource>> unorderedConfigSources
          , Dictionary<ushort, IMutableFlogConfigSource> priorityConfigSources
          , IConfigurationRoot configRoot, string path)
        {
            if (FLogContext.Context.ConfigRegistry.ConfigPathToConfigSourceConfig(configRoot, path) is { } configSource)
            {
                priorityConfigSources.Add(configSource.ConfigPriorityOrder, configSource);
                unorderedConfigSources.Add(new KeyValuePair<ushort, IMutableFlogConfigSource>(configSource.ConfigPriorityOrder, configSource));
                return configSource;
            }
            return null;
        }

        list.Clear();

        nextConfigReadTime = TimeContext.UtcNow;
        foreach (var appenderRefs in GetChildren())
        {
            var configSourcePath = $"{Path}{Split}{appenderRefs.Key}";
            if (ushort.TryParse(appenderRefs.Key, out var key))
            {
                if (priorityConfigSources.TryGetValue(key, out var configSource))
                {
                    var appenderType = ConfigRoot.GetConfigSourceType(configSourcePath);
                    if (appenderType == configSource.SourceType)
                    {
                        list.Add(new KeyValuePair<ushort, IMutableFlogConfigSource>(key, configSource));
                    }
                    else
                    {
                        priorityConfigSources.Remove(key);
                        var appenderConfig = LoadNewConfig(list, priorityConfigSources, ConfigRoot, configSourcePath);
                        if (appenderConfig != null) appenderConfig.ParentConfig = this;
                    }
                }
                else
                {
                    var appenderConfig = LoadNewConfig(list, priorityConfigSources, ConfigRoot, configSourcePath);
                    if (appenderConfig != null) appenderConfig.ParentConfig = this;
                }
            }
        }
        nextConfigReadTime = TimeContext.UtcNow.Add(recheckTimeSpanInterval);
        return list;
    }

    object ICloneable.Clone() => Clone();

    IOrderedConfigSourcesLookupConfig ICloneable<IOrderedConfigSourcesLookupConfig>.Clone() => Clone();

    public virtual OrderedConfigSourcesLookupConfig Clone() => new(this);

    public virtual bool AreEquivalent(IOrderedConfigSourcesLookupConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var countSame = Count == other.Count;
        if (countSame)
            foreach (var nameKey in Keys)
            {
                var myConfig    = this[nameKey];
                var otherConfig = other[nameKey];
                if (!myConfig.AreEquivalent(otherConfig, exactTypes)) return false;
            }

        return countSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IOrderedConfigSourcesLookupConfig, true);

    public override int GetHashCode()
    {
        var hashCode = priorityConfigSources.GetHashCode();
        return hashCode;
    }

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll(priorityConfigSources)
           .Complete();

    public override string ToString() => this.DefaultToString();
}
