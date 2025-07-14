// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Config;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending;

public interface INamedAppendersLookupConfig : IStickyKeyValueDictionary<string, IAppenderReferenceConfig>
  , IInterfacesComparable<INamedAppendersLookupConfig>, ICloneable<INamedAppendersLookupConfig>
  , IStyledToStringObject
{
    IEnumerable<string> NameKeys      { get; }
    IEnumerable<string> ConfigRefKeys { get; }

    bool ContainsConfigRef(string configRef);

    bool TryGetValueByConfigRef(string configRefKey, out IAppenderReferenceConfig? configRef);
}

public interface IAppendableNamedAppendersLookupConfig : INamedAppendersLookupConfig, IAppendableDictionary<string, IMutableAppenderReferenceConfig>
{
    new IEnumerable<string> Keys { get; }

    new IEnumerable<IMutableAppenderReferenceConfig> Values { get; }

    new int Count { get; }

    new IMutableAppenderReferenceConfig this[string loggerName] { get; set; }

    new bool ContainsKey(string appenderName);

    new IEnumerator<KeyValuePair<string, IMutableAppenderReferenceConfig>> GetEnumerator();
}

public class NamedAppendersLookupConfig : ConfigSection, IAppendableNamedAppendersLookupConfig
{
    private Dictionary<string, IMutableAppenderReferenceConfig> appendersByName = new();

    public NamedAppendersLookupConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public NamedAppendersLookupConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public NamedAppendersLookupConfig(params IMutableAppenderReferenceConfig[] toAdd)
        : this(InMemoryConfigRoot, InMemoryPath, toAdd) { }

    public NamedAppendersLookupConfig
    (IConfigurationRoot root, string path, params IMutableAppenderReferenceConfig[] toAdd) : base(root, path)
    {
        for (int i = 0; i < toAdd.Length; i++)
        {
            appendersByName.Add(toAdd[i].AppenderName!, toAdd[i]);
        }
    }

    public NamedAppendersLookupConfig(INamedAppendersLookupConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        foreach (var kvp in toClone)
        {
            appendersByName.Add(kvp.Key, (IMutableAppenderReferenceConfig)kvp.Value);
        }
    }

    public NamedAppendersLookupConfig(INamedAppendersLookupConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public void Add(KeyValuePair<string, IMutableAppenderReferenceConfig> item)
    {
        appendersByName.Add(item.Key, item.Value);
    }

    public void Add(string key, IMutableAppenderReferenceConfig value)
    {
        appendersByName.Add(key, value);
    }

    public IEnumerable<string> ConfigRefKeys =>
        appendersByName
            .Values
            .Where(arc => arc.AppenderConfigRef != null)
            .Select(arc => arc.AppenderConfigRef)
            .OfType<string>();

    bool IReadOnlyDictionary<string, IAppenderReferenceConfig>.ContainsKey(string appenderName) =>
        appendersByName.ContainsKey(appenderName);

    bool IAppendableNamedAppendersLookupConfig.ContainsKey(string appenderName) => appendersByName.ContainsKey(appenderName);

    bool IReadOnlyDictionary<string, IMutableAppenderReferenceConfig>.ContainsKey(string appenderName) =>
        appendersByName.ContainsKey(appenderName);

    public bool ContainsConfigRef(string configRef) =>
        appendersByName
            .Values
            .Any(arc => arc.AppenderConfigRef != configRef);

    public int Count => appendersByName.Count;

    IEnumerator IEnumerable.GetEnumerator() => appendersByName.GetEnumerator();

    IEnumerator<KeyValuePair<string, IMutableAppenderReferenceConfig>> IAppendableNamedAppendersLookupConfig.GetEnumerator() =>
        appendersByName.GetEnumerator();

    IEnumerator<KeyValuePair<string, IAppenderReferenceConfig>> IEnumerable<KeyValuePair<string, IAppenderReferenceConfig>>.GetEnumerator() =>
        appendersByName
            .Select(arcKvp => new KeyValuePair<string, IAppenderReferenceConfig>(arcKvp.Key, (IAppenderDefinitionConfig)arcKvp.Value))
            .GetEnumerator();

    public IEnumerator<KeyValuePair<string, IMutableAppenderReferenceConfig>> GetEnumerator() =>
        appendersByName.GetEnumerator();

    public new IMutableAppenderReferenceConfig this [string loggerName]
    {
        get => appendersByName[loggerName];
        set => appendersByName[loggerName] = value;
    }

    IAppenderReferenceConfig IReadOnlyDictionary<string, IAppenderReferenceConfig>.this[string loggerName] => this[loggerName];

    IEnumerable<string> IReadOnlyDictionary<string, IMutableAppenderReferenceConfig>.Keys => appendersByName.Keys;

    IEnumerable<string> IReadOnlyDictionary<string, IAppenderReferenceConfig>.Keys => appendersByName.Keys;

    ICollection<string> IAppendableDictionary<string, IMutableAppenderReferenceConfig>.Keys => appendersByName.Keys;

    IEnumerable<string> IAppendableNamedAppendersLookupConfig.Keys => appendersByName.Keys;

    public IEnumerable<string> NameKeys => appendersByName.Keys;

    public bool TryGetValue(string appenderName, [MaybeNullWhen(false)] out IMutableAppenderReferenceConfig value) =>
        appendersByName.TryGetValue(appenderName, out value);

    public bool TryGetValue(string appenderName, [MaybeNullWhen(false)] out IAppenderReferenceConfig value)
    {
        value = null;
        if (appendersByName.TryGetValue(appenderName, out var config))
        {
            value = config;
            return true;
        }
        return false;
    }

    public bool TryGetValueByConfigRef(string configRefKey, out IAppenderReferenceConfig? configRef)
    {
        configRef = null;
        if (ContainsConfigRef(configRefKey))
        {
            configRef = appendersByName.Values.FirstOrDefault(arc => arc.AppenderConfigRef == configRefKey);
            return true;
        }
        return false;
    }

    IEnumerable<IMutableAppenderReferenceConfig> IAppendableNamedAppendersLookupConfig.Values => 
        appendersByName.Values;

    IEnumerable<IMutableAppenderReferenceConfig> IReadOnlyDictionary<string, IMutableAppenderReferenceConfig>.Values =>
        appendersByName.Values;
    IEnumerable<IAppenderReferenceConfig> IReadOnlyDictionary<string, IAppenderReferenceConfig>.Values => 
        appendersByName.Values;
    ICollection<IMutableAppenderReferenceConfig> IAppendableDictionary<string, IMutableAppenderReferenceConfig>.Values =>
        appendersByName.Values;

    object ICloneable.Clone() => Clone();

    INamedAppendersLookupConfig ICloneable<INamedAppendersLookupConfig>.Clone() => Clone();

    public virtual NamedAppendersLookupConfig Clone() => new(this);

    public virtual bool AreEquivalent(INamedAppendersLookupConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var countSame       = Count == other.Count;
        if (countSame)
        {
            foreach (var nameKey in NameKeys)
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

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as INamedAppendersLookupConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = appendersByName?.GetHashCode() ?? 0;
            return hashCode;
        }
    }

    public void ToString(IStyledTypeStringAppender sbc)
    {
        sbc.AddTypeName(nameof(AppenderReferenceConfig))
           .AddTypeStart()
           .AddCollectionField(nameof(appendersByName), appendersByName)
           .AddTypeEnd();
    }


    public override string ToString() => this.DefaultToString();
}
