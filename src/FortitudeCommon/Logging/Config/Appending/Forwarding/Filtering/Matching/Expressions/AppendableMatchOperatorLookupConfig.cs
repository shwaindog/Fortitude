// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.Expressions;

public interface IMatchOperatorCollectionConfig : IFLogConfig, IStickyKeyValueDictionary<ushort, IMatchOperatorExpressionConfig>
  , ICloneable<IMatchOperatorCollectionConfig>, IStyledToStringObject, IInterfacesComparable<IMatchOperatorCollectionConfig> { }

public interface IAppendableMatchOperatorLookupConfig : IMutableFLogConfig, IMatchOperatorCollectionConfig
  , IAppendableDictionary<ushort, IMutableMatchOperatorExpressionConfig>
{
    new IEnumerable<ushort> Keys { get; }

    new IEnumerable<IMutableMatchOperatorExpressionConfig> Values { get; }

    new int Count { get; }

    new IMutableMatchOperatorExpressionConfig this[ushort appenderName] { get; set; }

    new bool ContainsKey(ushort evaluateOrder);

    new IEnumerator<KeyValuePair<ushort, IMutableMatchOperatorExpressionConfig>> GetEnumerator();
}

public class AppendableMatchOperatorLookupConfig : FLogConfig, IAppendableMatchOperatorLookupConfig
{
    private static TimeSpan recheckTimeSpanInterval;

    private readonly Dictionary<ushort, IMutableMatchOperatorExpressionConfig> evalOrderKeyedExpressions = new();

    private readonly List<KeyValuePair<ushort, IMutableMatchOperatorExpressionConfig>> orderedConfigSources = new();

    private DateTime nextConfigReadTime = DateTime.MinValue;

    public AppendableMatchOperatorLookupConfig(IConfigurationRoot root, string path) : base(root, path) =>
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);

    public AppendableMatchOperatorLookupConfig() : this(InMemoryConfigRoot, InMemoryPath) => recheckTimeSpanInterval = TimeSpan.FromMinutes(1);

    public AppendableMatchOperatorLookupConfig(params IMutableMatchOperatorExpressionConfig[] toAdd)
        : this(InMemoryConfigRoot, InMemoryPath, toAdd) =>
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);

    public AppendableMatchOperatorLookupConfig
        (IConfigurationRoot root, string path, params IMutableMatchOperatorExpressionConfig[] toAdd) : base(root, path)
    {
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);
        for (var i = 0; i < toAdd.Length; i++) evalOrderKeyedExpressions.Add(toAdd[i].EvaluateOrder, toAdd[i]);
    }

    public AppendableMatchOperatorLookupConfig(IMatchOperatorCollectionConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);
        foreach (var kvp in toClone) evalOrderKeyedExpressions.Add(kvp.Key, (IMutableMatchOperatorExpressionConfig)kvp.Value);
    }

    public AppendableMatchOperatorLookupConfig(IMatchOperatorCollectionConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) =>
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);

    [JsonIgnore]
    protected Dictionary<ushort, IMutableMatchOperatorExpressionConfig> CheckConfigGetConfigSourcesDict
    {
        get
        {
            if (!evalOrderKeyedExpressions.Any() || nextConfigReadTime < TimeContext.UtcNow)
            {
                recheckTimeSpanInterval = FLogContext.NullOnUnstartedContext?.ConfigRegistry?.ExpireConfigCacheIntervalTimeSpan ??
                                          TimeSpan.FromMinutes(1);
                evalOrderKeyedExpressions.Clear();
                foreach (var configurationSection in GetSection(Path).GetChildren())
                {
                    IMutableMatchOperatorExpressionConfig configSource =
                        new MatchOperatorExpressionConfig(ConfigRoot, $"{Path}{Split}{configurationSection.Key}")
                        {
                            ParentConfig = this
                        };
                    if (ushort.TryParse(configurationSection.Key, out var key)) evalOrderKeyedExpressions.TryAdd(key, configSource);
                }
                nextConfigReadTime = TimeContext.UtcNow.Add(recheckTimeSpanInterval);
            }
            return evalOrderKeyedExpressions;
        }
    }

    protected IReadOnlyList<KeyValuePair<ushort, IMutableMatchOperatorExpressionConfig>> OrderedConfigSources
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

    public void Add(KeyValuePair<ushort, IMutableMatchOperatorExpressionConfig> item)
    {
        evalOrderKeyedExpressions.Add(item.Key, item.Value);
        PushToConfigStorage(item.Value);
    }

    public void Add(ushort key, IMutableMatchOperatorExpressionConfig value)
    {
        evalOrderKeyedExpressions.Add(key, value);
        PushToConfigStorage(value);
    }

    bool IReadOnlyDictionary<ushort, IMutableMatchOperatorExpressionConfig>.ContainsKey(ushort priorityOrder) =>
        CheckConfigGetConfigSourcesDict.ContainsKey(priorityOrder);

    bool IAppendableMatchOperatorLookupConfig.ContainsKey(ushort priorityOrder) => CheckConfigGetConfigSourcesDict.ContainsKey(priorityOrder);

    public bool ContainsKey(ushort priorityOrder) => CheckConfigGetConfigSourcesDict.ContainsKey(priorityOrder);

    public int Count => CheckConfigGetConfigSourcesDict.Count;

    IEnumerator IEnumerable.GetEnumerator() => OrderedConfigSources.GetEnumerator();

    IEnumerator<KeyValuePair<ushort, IMutableMatchOperatorExpressionConfig>> IAppendableMatchOperatorLookupConfig.GetEnumerator() =>
        OrderedConfigSources.GetEnumerator();

    IEnumerator<KeyValuePair<ushort, IMatchOperatorExpressionConfig>> IEnumerable<KeyValuePair<ushort, IMatchOperatorExpressionConfig>>.
        GetEnumerator() =>
        OrderedConfigSources
            .Select(arcKvp => new KeyValuePair<ushort, IMatchOperatorExpressionConfig>(arcKvp.Key, arcKvp.Value))
            .GetEnumerator();

    public IEnumerator<KeyValuePair<ushort, IMutableMatchOperatorExpressionConfig>> GetEnumerator() => OrderedConfigSources.GetEnumerator();

    public IMutableMatchOperatorExpressionConfig this[ushort appenderName]
    {
        get => CheckConfigGetConfigSourcesDict[appenderName];
        set => CheckConfigGetConfigSourcesDict[appenderName] = value;
    }

    IMatchOperatorExpressionConfig IReadOnlyDictionary<ushort, IMatchOperatorExpressionConfig>.this[ushort appenderName] => this[appenderName];

    public IEnumerable<ushort> Keys => OrderedConfigSources.Select(kvp => kvp.Key);

    ICollection<ushort> IAppendableDictionary<ushort, IMutableMatchOperatorExpressionConfig>.Keys =>
        OrderedConfigSources.Select(kvp => kvp.Key).ToList();

    public bool TryGetValue(ushort priorityOrder, [MaybeNullWhen(false)] out IMutableMatchOperatorExpressionConfig value) =>
        CheckConfigGetConfigSourcesDict.TryGetValue(priorityOrder, out value);

    public bool TryGetValue(ushort priorityOrder, [MaybeNullWhen(false)] out IMatchOperatorExpressionConfig value)
    {
        value = null;
        if (CheckConfigGetConfigSourcesDict.TryGetValue(priorityOrder, out var evalExpression))
        {
            value = evalExpression;
            return true;
        }
        return false;
    }

    public IEnumerable<IMutableMatchOperatorExpressionConfig> Values => OrderedConfigSources.Select(kvp => kvp.Value);

    IEnumerable<IMatchOperatorExpressionConfig> IReadOnlyDictionary<ushort, IMatchOperatorExpressionConfig>.Values =>
        OrderedConfigSources.Select(kvp => kvp.Value);

    ICollection<IMutableMatchOperatorExpressionConfig> IAppendableDictionary<ushort, IMutableMatchOperatorExpressionConfig>.Values =>
        OrderedConfigSources.Select(kvp => kvp.Value).ToList();

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    object ICloneable.Clone() => Clone();

    IMatchOperatorCollectionConfig ICloneable<IMatchOperatorCollectionConfig>.Clone() => Clone();

    public virtual bool AreEquivalent(IMatchOperatorCollectionConfig? other, bool exactTypes = false)
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

    protected void PushToConfigStorage(IMutableMatchOperatorExpressionConfig value)
    {
        value.CloneConfigTo(ConfigRoot, $"{Path}{Split}{value.EvaluateOrder}");
    }

    protected List<KeyValuePair<ushort, IMutableMatchOperatorExpressionConfig>> ClearAndCopyEitherTo
        (List<KeyValuePair<ushort, IMutableMatchOperatorExpressionConfig>> list)
    {
        static IMutableMatchOperatorExpressionConfig LoadNewConfig
        (
            List<KeyValuePair<ushort, IMutableMatchOperatorExpressionConfig>> unorderedConfigSources
          , Dictionary<ushort, IMutableMatchOperatorExpressionConfig> priorityConfigSources
          , IConfigurationRoot configRoot, string path)
        {
            var compareExpression = new MatchOperatorExpressionConfig(configRoot, path);
            priorityConfigSources.Add(compareExpression.EvaluateOrder, compareExpression);
            unorderedConfigSources.Add(new KeyValuePair<ushort, IMutableMatchOperatorExpressionConfig>(compareExpression.EvaluateOrder
                                      , compareExpression));
            return compareExpression;
        }

        list.Clear();

        nextConfigReadTime = TimeContext.UtcNow;
        foreach (var appenderRefs in GetChildren())
        {
            var configSourcePath = $"{Path}{Split}{appenderRefs.Key}";
            if (ushort.TryParse(appenderRefs.Key, out var key))
            {
                if (evalOrderKeyedExpressions.TryGetValue(key, out var configSource))
                {
                    list.Add(new KeyValuePair<ushort, IMutableMatchOperatorExpressionConfig>(key, configSource));
                }
                else
                {
                    var appenderConfig = LoadNewConfig(list, evalOrderKeyedExpressions, ConfigRoot, configSourcePath);
                    appenderConfig.ParentConfig = this;
                }
            }
        }
        nextConfigReadTime = TimeContext.UtcNow.Add(recheckTimeSpanInterval);
        return list;
    }

    public virtual AppendableMatchOperatorLookupConfig Clone() => new(this);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IMatchOperatorCollectionConfig, true);

    public override int GetHashCode()
    {
        var hashCode = evalOrderKeyedExpressions.GetHashCode();
        return hashCode;
    }

    public StyledTypeBuildResult ToString(IStyledTypeStringAppender sbc) =>
        sbc.StartKeyedCollectionType(nameof(AppendableMatchOperatorLookupConfig))
           .AddAll(evalOrderKeyedExpressions)
           .Complete();

    public override string ToString() => this.DefaultToString();
}
