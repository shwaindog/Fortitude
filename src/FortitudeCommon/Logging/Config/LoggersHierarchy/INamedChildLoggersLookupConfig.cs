// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Config;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.LoggersHierarchy;

public interface INamedChildLoggersLookupConfig : IStickyKeyValueDictionary<string, IFLoggerDescendantConfig>
  , IInterfacesComparable<INamedChildLoggersLookupConfig>, ICloneable<INamedChildLoggersLookupConfig>
  , IStyledToStringObject
{
}

public interface IMutableNamedChildLoggersLookupConfig : INamedChildLoggersLookupConfig
  , IAppendableDictionary<string, IMutableFLoggerDescendantConfig>
{
    new IEnumerable<string> Keys { get; }

    new IEnumerable<IMutableFLoggerDescendantConfig> Values { get; }

    new int Count { get; }

    new IMutableFLoggerDescendantConfig this[string loggerName] { get; set; }

    new bool ContainsKey(string key);

    new IEnumerator<KeyValuePair<string, IMutableFLoggerDescendantConfig>> GetEnumerator();
}

public class NamedChildLoggersLookupConfig : ConfigSection, IMutableNamedChildLoggersLookupConfig
{
    private Dictionary<string, IMutableFLoggerDescendantConfig> loggersByName = new();

    public NamedChildLoggersLookupConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public NamedChildLoggersLookupConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public NamedChildLoggersLookupConfig(params IMutableFLoggerDescendantConfig[] toAdd)
        : this(InMemoryConfigRoot, InMemoryPath, toAdd) { }

    public NamedChildLoggersLookupConfig
        (IConfigurationRoot root, string path, params IMutableFLoggerDescendantConfig[] toAdd) : base(root, path)
    {
        for (int i = 0; i < toAdd.Length; i++)
        {
            loggersByName.Add(toAdd[i].Name, toAdd[i]);
        }
    }

    public NamedChildLoggersLookupConfig(INamedChildLoggersLookupConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        foreach (var kvp in toClone)
        {
            loggersByName.Add(kvp.Key, (IMutableFLoggerDescendantConfig)kvp.Value);
        }
    }

    public NamedChildLoggersLookupConfig(INamedChildLoggersLookupConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public void Add(KeyValuePair<string, IMutableFLoggerDescendantConfig> item)
    {
        loggersByName.Add(item.Key, item.Value);
    }

    public void Add(string key, IMutableFLoggerDescendantConfig value)
    {
        loggersByName.Add(key, value);
    }


    public bool ContainsKey(string loggerName) => loggersByName.ContainsKey(loggerName);

    public int Count => loggersByName.Count;

    public new IMutableFLoggerDescendantConfig this[string loggerName]
    {
        get => loggersByName[loggerName];
        set => loggersByName[loggerName] = value;
    }

    IFLoggerDescendantConfig IReadOnlyDictionary<string, IFLoggerDescendantConfig>.this[string loggerName] => this[loggerName];

    IEnumerable<string> IReadOnlyDictionary<string, IMutableFLoggerDescendantConfig>.Keys => loggersByName.Keys;

    IEnumerable<string> IReadOnlyDictionary<string, IFLoggerDescendantConfig>.Keys => loggersByName.Keys;

    public ICollection<string> Keys => loggersByName.Keys;

    IEnumerable<string> IMutableNamedChildLoggersLookupConfig.Keys => loggersByName.Keys;

    public bool TryGetValue(string appenderName, [MaybeNullWhen(false)] out IMutableFLoggerDescendantConfig value) =>
        loggersByName.TryGetValue(appenderName, out value);

    public bool TryGetValue(string appenderName, [MaybeNullWhen(false)] out IFLoggerDescendantConfig value)
    {
        value = null;
        if (loggersByName.TryGetValue(appenderName, out var config))
        {
            value = config;
            return true;
        }
        return false;
    }

    IEnumerable<IMutableFLoggerDescendantConfig> IMutableNamedChildLoggersLookupConfig.Values => loggersByName.Values;

    IEnumerable<IMutableFLoggerDescendantConfig> IReadOnlyDictionary<string, IMutableFLoggerDescendantConfig>.  Values => loggersByName.Values;
    IEnumerable<IFLoggerDescendantConfig> IReadOnlyDictionary<string, IFLoggerDescendantConfig>.                Values => loggersByName.Values;
    ICollection<IMutableFLoggerDescendantConfig> IAppendableDictionary<string, IMutableFLoggerDescendantConfig>.Values => loggersByName.Values;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<KeyValuePair<string, IFLoggerDescendantConfig>> IEnumerable<KeyValuePair<string, IFLoggerDescendantConfig>>.GetEnumerator() =>
        loggersByName
            .Select(arcKvp => 
                        new KeyValuePair<string, IFLoggerDescendantConfig>(arcKvp.Key, arcKvp.Value))
            .GetEnumerator();

    public IEnumerator<KeyValuePair<string, IMutableFLoggerDescendantConfig>> GetEnumerator() => loggersByName.GetEnumerator();

    object ICloneable.Clone() => Clone();

    INamedChildLoggersLookupConfig ICloneable<INamedChildLoggersLookupConfig>.Clone() => Clone();

    public virtual NamedChildLoggersLookupConfig Clone() => new(this);

    public virtual bool AreEquivalent(INamedChildLoggersLookupConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var countSame = Count == other.Count;
        if (countSame)
        {
            foreach (var nameKey in Keys)
            {
                var myConfig    = this[nameKey];
                var otherConfig = other[nameKey];
                if (!myConfig.AreEquivalent(otherConfig, exactTypes))
                {
                    return false;
                }
            }
        }

        return countSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as INamedChildLoggersLookupConfig, true);

    public override int GetHashCode()
    {
        var hashCode = loggersByName.GetHashCode();
        return hashCode;
    }

    public void ToString(IStyledTypeStringAppender sbc)
    {
        sbc.AddTypeName(nameof(NamedChildLoggersLookupConfig))
           .AddTypeStart()
           .AddCollectionField(nameof(loggersByName), loggersByName)
           .AddTypeEnd();
    }


    public override string ToString() => this.DefaultToString();
}
