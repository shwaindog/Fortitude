// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Config;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Logging.Config.LoggersHierarchy;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.MatchConditions.Sequences;

public interface IExtractedMessageKeyValuesConfig : IStickyKeyValueDictionary<string, IExtractKeyExpressionConfig>
  , IConfigCloneTo<IExtractedMessageKeyValuesConfig>, IStyledToStringObject, IFLogConfig, IInterfacesComparable<IExtractedMessageKeyValuesConfig>
{
    new IExtractedMessageKeyValuesConfig Clone();
}

public interface IAppendableExtractedMessageKeyValuesConfig : IExtractedMessageKeyValuesConfig
  , IAppendableDictionary<string, IMutableExtractKeyExpressionConfig>, IMutableFLogConfig
{
    new IEnumerable<string> Keys { get; }

    new IEnumerable<IMutableExtractKeyExpressionConfig> Values { get; }

    new int Count { get; }

    new IMutableExtractKeyExpressionConfig this[string appenderName] { get; set; }

    new bool ContainsKey(string keyName);

    new IEnumerator<KeyValuePair<string, IMutableExtractKeyExpressionConfig>> GetEnumerator();
}

public class ExtractedMessageKeyValuesConfig : FLogConfig, IAppendableExtractedMessageKeyValuesConfig
{
    private static TimeSpan recheckTimeSpanInterval;

    private readonly Dictionary<string, IMutableExtractKeyExpressionConfig> extractConfigByKeyName = new();

    private DateTime nextConfigReadTime = DateTime.MinValue;

    public ExtractedMessageKeyValuesConfig(IConfigurationRoot root, string path) : base(root, path) =>
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);

    public ExtractedMessageKeyValuesConfig() : this(InMemoryConfigRoot, InMemoryPath) => recheckTimeSpanInterval = TimeSpan.FromMinutes(1);

    public ExtractedMessageKeyValuesConfig(params IMutableExtractKeyExpressionConfig[] toAdd)
        : this(InMemoryConfigRoot, InMemoryPath, toAdd) =>
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);

    public ExtractedMessageKeyValuesConfig
        (IConfigurationRoot root, string path, params IMutableExtractKeyExpressionConfig[] toAdd) : base(root, path)
    {
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);
        for (var i = 0; i < toAdd.Length; i++) extractConfigByKeyName.Add(toAdd[i].KeyName, toAdd[i]);
    }

    public ExtractedMessageKeyValuesConfig(IExtractedMessageKeyValuesConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);
        foreach (var kvp in toClone) extractConfigByKeyName.Add(kvp.Key, (IMutableExtractKeyExpressionConfig)kvp.Value);
    }

    public ExtractedMessageKeyValuesConfig(IExtractedMessageKeyValuesConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) =>
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);

    [JsonIgnore]
    protected Dictionary<string, IMutableExtractKeyExpressionConfig> CheckConfigGetLoggersDict
    {
        get
        {
            if (!extractConfigByKeyName.Any() || nextConfigReadTime < TimeContext.UtcNow)
            {
                recheckTimeSpanInterval =
                    FLogContext.NullOnUnstartedContext?.ConfigRegistry?.ExpireConfigCacheIntervalTimeSpan ?? TimeSpan.FromMinutes(1);
                extractConfigByKeyName.Clear();
                foreach (var configurationSection in GetSection(Path).GetChildren())
                {
                    var descendantConfig = new ExtractKeyExpressionConfig(ConfigRoot, $"{configurationSection.Path}{Split}{configurationSection.Key}")
                    {
                        ParentConfig = this
                    };

                    extractConfigByKeyName.TryAdd(configurationSection.Key, descendantConfig);
                }
                nextConfigReadTime = TimeContext.UtcNow.Add(recheckTimeSpanInterval);
            }
            return extractConfigByKeyName;
        }
    }

    public void Add(KeyValuePair<string, IMutableExtractKeyExpressionConfig> item)
    {
        extractConfigByKeyName.Add(item.Key, item.Value);
        PushToConfigStorage(item.Value);
    }

    public void Add(string key, IMutableExtractKeyExpressionConfig value)
    {
        extractConfigByKeyName.Add(key, value);
        PushToConfigStorage(value);
    }

    public bool ContainsKey(string loggerName) => CheckConfigGetLoggersDict.ContainsKey(loggerName);

    public int Count => CheckConfigGetLoggersDict.Count;

    public new IMutableExtractKeyExpressionConfig this[string appenderName]
    {
        get => CheckConfigGetLoggersDict[appenderName];
        set => CheckConfigGetLoggersDict[appenderName] = value;
    }

    IExtractKeyExpressionConfig IReadOnlyDictionary<string, IExtractKeyExpressionConfig>.this[string appenderName] => this[appenderName];

    IEnumerable<string> IReadOnlyDictionary<string, IMutableExtractKeyExpressionConfig>.Keys => CheckConfigGetLoggersDict.Keys;

    IEnumerable<string> IReadOnlyDictionary<string, IExtractKeyExpressionConfig>.Keys => CheckConfigGetLoggersDict.Keys;

    public ICollection<string> Keys => CheckConfigGetLoggersDict.Keys;

    IEnumerable<string> IAppendableExtractedMessageKeyValuesConfig.Keys => CheckConfigGetLoggersDict.Keys;

    public bool TryGetValue(string appenderName, [MaybeNullWhen(false)] out IMutableExtractKeyExpressionConfig value) =>
        CheckConfigGetLoggersDict.TryGetValue(appenderName, out value);

    public bool TryGetValue(string appenderName, [MaybeNullWhen(false)] out IExtractKeyExpressionConfig value)
    {
        value = null;
        if (CheckConfigGetLoggersDict.TryGetValue(appenderName, out var config))
        {
            value = config;
            return true;
        }
        return false;
    }

    IEnumerable<IMutableExtractKeyExpressionConfig> IAppendableExtractedMessageKeyValuesConfig.Values => CheckConfigGetLoggersDict.Values;

    IEnumerable<IMutableExtractKeyExpressionConfig> IReadOnlyDictionary<string, IMutableExtractKeyExpressionConfig>.Values =>
        CheckConfigGetLoggersDict.Values;

    IEnumerable<IExtractKeyExpressionConfig> IReadOnlyDictionary<string, IExtractKeyExpressionConfig>.Values => CheckConfigGetLoggersDict.Values;

    ICollection<IMutableExtractKeyExpressionConfig> IAppendableDictionary<string, IMutableExtractKeyExpressionConfig>.Values =>
        CheckConfigGetLoggersDict.Values;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<KeyValuePair<string, IExtractKeyExpressionConfig>>
        IEnumerable<KeyValuePair<string, IExtractKeyExpressionConfig>>.GetEnumerator() =>
        CheckConfigGetLoggersDict
            .Select(arcKvp =>
                        new KeyValuePair<string, IExtractKeyExpressionConfig>(arcKvp.Key, arcKvp.Value))
            .GetEnumerator();

    public IEnumerator<KeyValuePair<string, IMutableExtractKeyExpressionConfig>> GetEnumerator() => CheckConfigGetLoggersDict.GetEnumerator();

    public override T Accept<T>(T visitor) => visitor.Visit(this);

    public IExtractedMessageKeyValuesConfig CloneConfigTo(IConfigurationRoot configRoot, string path) =>
        new ExtractedMessageKeyValuesConfig(configRoot, path);

    protected void PushToConfigStorage(IMutableExtractKeyExpressionConfig value)
    {
        value.CloneConfigTo(ConfigRoot, $"{Path}{Split}{value.KeyName}");
    }

    IExtractedMessageKeyValuesConfig ICloneable<IExtractedMessageKeyValuesConfig>.Clone() => Clone();

    object ICloneable.Clone() => Clone();

    IExtractedMessageKeyValuesConfig IExtractedMessageKeyValuesConfig.Clone() => Clone();

    public virtual ExtractedMessageKeyValuesConfig Clone() => new(this);

    public virtual bool AreEquivalent(IExtractedMessageKeyValuesConfig? other, bool exactTypes = false)
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

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IExtractedMessageKeyValuesConfig, true);

    public override int GetHashCode()
    {
        var hashCode = extractConfigByKeyName.GetHashCode();
        return hashCode;
    }

    public StyledTypeBuildResult ToString(IStyledTypeStringAppender stsa) =>
        stsa.StartKeyedCollectionType(this)
           .AddAll(extractConfigByKeyName)
           .Complete();

    public override string ToString() => this.DefaultToString();
}
