// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending;

public interface INamedAppendersLookupConfig : IInterfacesComparable<INamedAppendersLookupConfig>
  , ICloneable<INamedAppendersLookupConfig>, IStringBearer, IFLogConfig
{
    int Count { get; }

    IAppenderReferenceConfig this[string key] { get; }
    IEnumerable<string> Keys { get; }
    IEnumerable<IAppenderReferenceConfig> Values { get; }
    bool ContainsKey(string key);
    bool TryGetValue(string key, [MaybeNullWhen(false)] out IAppenderReferenceConfig value);

    IEnumerator<KeyValuePair<string, IAppenderReferenceConfig>> GetEnumerator();

    List<IAppenderReferenceConfig> ClearAndCopyTo(List<IAppenderReferenceConfig> list);
}

public interface IAppendableNamedAppendersLookupConfig : INamedAppendersLookupConfig
  , IAppendableDictionary<string, IMutableAppenderReferenceConfig>, IMutableFLogConfig
{
    new IEnumerable<string> Keys { get; }

    new IEnumerable<IMutableAppenderReferenceConfig> Values { get; }

    new int Count { get; }

    new IMutableAppenderReferenceConfig this[string appenderName] { get; set; }

    IAppendableNamedAppendersLookupConfig Add(IAppenderReferenceConfig value);

    IAppendableNamedAppendersLookupConfig Add(IMutableAppenderReferenceConfig value);

    new bool ContainsKey(string appenderName);

    new IEnumerator<KeyValuePair<string, IMutableAppenderReferenceConfig>> GetEnumerator();
}

public class NamedAppendersLookupConfig : FLogConfig, IAppendableNamedAppendersLookupConfig
{
    private static TimeSpan recheckTimeSpanInterval;

    protected readonly Dictionary<string, IMutableAppenderReferenceConfig> AppendersByName = new();

    private readonly List<IMutableAppenderReferenceConfig> cachedAppenderReferences = new();

    private DateTime nextConfigReadTime = DateTime.MinValue;

    public NamedAppendersLookupConfig(IConfigurationRoot root, string path) : base(root, path) => recheckTimeSpanInterval = TimeSpan.FromMinutes(1);

    public NamedAppendersLookupConfig() : this(InMemoryConfigRoot, InMemoryPath) => recheckTimeSpanInterval = TimeSpan.FromMinutes(1);

    public NamedAppendersLookupConfig(params IMutableAppenderReferenceConfig[] toAdd)
        : this(InMemoryConfigRoot, InMemoryPath, toAdd) =>
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);

    public NamedAppendersLookupConfig
        (IConfigurationRoot root, string path, params IMutableAppenderReferenceConfig[] toAdd) : base(root, path)
    {
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);
        for (var i = 0; i < toAdd.Length; i++)
        {
            AppendersByName.Add(toAdd[i].AppenderName, toAdd[i]);
            PushToConfigStorage(toAdd[i]);
        }
    }

    public NamedAppendersLookupConfig(INamedAppendersLookupConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);
        foreach (var kvp in toClone)
        {
            AppendersByName.Add(kvp.Key, (IMutableAppenderReferenceConfig)kvp.Value);
            PushToConfigStorage(kvp.Value);
        }
    }

    public NamedAppendersLookupConfig(INamedAppendersLookupConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    [JsonIgnore]
    protected Dictionary<string, IMutableAppenderReferenceConfig> CheckConfigGetAppendersDict
    {
        get
        {
            if (!AppendersByName.Any() || nextConfigReadTime < TimeContext.UtcNow)
            {
                recheckTimeSpanInterval = FLogContext.NullOnUnstartedContext?.ConfigRegistry.ExpireConfigCacheIntervalTimeSpan ??
                                          TimeSpan.FromMinutes(1);
                AppendersByName.Clear();
                foreach (var configurationSection in GetChildren())
                    if (FLogCreate.MakeAppenderConfig(ConfigRoot, $"{configurationSection.Path}") is { } appenderConfig)
                    {
                        appenderConfig.ParentConfig = this;
                        if (appenderConfig.AppenderName != configurationSection.Key) appenderConfig.AppenderName = configurationSection.Key;
                        AppendersByName.TryAdd(configurationSection.Key, appenderConfig);
                    }
                nextConfigReadTime = TimeContext.UtcNow.Add(recheckTimeSpanInterval);
            }
            return AppendersByName;
        }
    }

    protected IReadOnlyList<IAppenderReferenceConfig> OrderAppenders
    {
        get
        {
            if (nextConfigReadTime < TimeContext.UtcNow)
            {
                ClearAndCopyEitherTo(cachedAppenderReferences);
                nextConfigReadTime = TimeContext.UtcNow.Add(recheckTimeSpanInterval);
            }
            return cachedAppenderReferences;
        }
    }

    public IAppenderReferenceConfig this[int toCountIndex] => OrderAppenders[toCountIndex];

    public IEnumerable<string> NameKeys => CheckConfigGetAppendersDict.Keys;

    public virtual void Add(KeyValuePair<string, IMutableAppenderReferenceConfig> item)
    {
        AppendersByName.Add(item.Key, item.Value);
        PushToConfigStorage(item.Value);
    }

    public IAppendableNamedAppendersLookupConfig Add(IAppenderReferenceConfig value)
    {
        AppendersByName.Add(value.AppenderName, (IMutableAppenderReferenceConfig)value);
        PushToConfigStorage(value);

        return this;
    }

    public IAppendableNamedAppendersLookupConfig Add(IMutableAppenderReferenceConfig value)
    {
        AppendersByName.Add(value.AppenderName, value);
        PushToConfigStorage(value);

        return this;
    }

    public virtual void Add(string key, IMutableAppenderReferenceConfig value)
    {
        AppendersByName.Add(key, value);
        PushToConfigStorage(value);
    }

    public bool ContainsKey(string appenderName) => CheckConfigGetAppendersDict.ContainsKey(appenderName);

    bool INamedAppendersLookupConfig.ContainsKey(string appenderName) => CheckConfigGetAppendersDict.ContainsKey(appenderName);

    bool IReadOnlyDictionary<string, IMutableAppenderReferenceConfig>.ContainsKey(string appenderName) =>
        CheckConfigGetAppendersDict.ContainsKey(appenderName);

    public int Count => CheckConfigGetAppendersDict.Count;

    public List<IAppenderReferenceConfig> ClearAndCopyTo(List<IAppenderReferenceConfig> list) => ClearAndCopyEitherTo(list);

    public override T Accept<T>(T visitor) => visitor.Visit(this);

    IEnumerator IEnumerable.GetEnumerator() => CheckConfigGetAppendersDict.GetEnumerator();

    IEnumerator<KeyValuePair<string, IMutableAppenderReferenceConfig>> IAppendableNamedAppendersLookupConfig.GetEnumerator() =>
        CheckConfigGetAppendersDict.GetEnumerator();

    IEnumerator<KeyValuePair<string, IAppenderReferenceConfig>> INamedAppendersLookupConfig.GetEnumerator() =>
        CheckConfigGetAppendersDict
            .Select(abnKvp =>
                        new KeyValuePair<string, IAppenderReferenceConfig>(abnKvp.Key, abnKvp.Value))
            .GetEnumerator();

    public IEnumerator<KeyValuePair<string, IMutableAppenderReferenceConfig>> GetEnumerator() => CheckConfigGetAppendersDict.GetEnumerator();

    public new IMutableAppenderReferenceConfig this[string appenderName]
    {
        get => CheckConfigGetAppendersDict[appenderName];
        set => CheckConfigGetAppendersDict[appenderName] = value;
    }

    IAppenderReferenceConfig INamedAppendersLookupConfig.this[string appenderName] => this[appenderName];

    IEnumerable<string> IReadOnlyDictionary<string, IMutableAppenderReferenceConfig>.Keys => CheckConfigGetAppendersDict.Keys;

    IEnumerable<string> INamedAppendersLookupConfig.Keys => CheckConfigGetAppendersDict.Keys;

    ICollection<string> IAppendableDictionary<string, IMutableAppenderReferenceConfig>.Keys => CheckConfigGetAppendersDict.Keys;

    IEnumerable<string> IAppendableNamedAppendersLookupConfig.Keys => CheckConfigGetAppendersDict.Keys;

    public bool TryGetValue(string appenderName, [MaybeNullWhen(false)] out IMutableAppenderReferenceConfig value) =>
        CheckConfigGetAppendersDict.TryGetValue(appenderName, out value);

    public bool TryGetValue(string appenderName, [MaybeNullWhen(false)] out IAppenderReferenceConfig value)
    {
        value = null;
        if (CheckConfigGetAppendersDict.TryGetValue(appenderName, out var config))
        {
            value = config;
            return true;
        }
        return false;
    }

    IEnumerable<IMutableAppenderReferenceConfig> IAppendableNamedAppendersLookupConfig.Values => CheckConfigGetAppendersDict.Values;

    IEnumerable<IMutableAppenderReferenceConfig> IReadOnlyDictionary<string, IMutableAppenderReferenceConfig>.Values =>
        CheckConfigGetAppendersDict.Values;

    IEnumerable<IAppenderReferenceConfig> INamedAppendersLookupConfig.Values => CheckConfigGetAppendersDict.Values;

    ICollection<IMutableAppenderReferenceConfig> IAppendableDictionary<string, IMutableAppenderReferenceConfig>.Values =>
        CheckConfigGetAppendersDict.Values;

    protected void PushToConfigStorage(IAppenderReferenceConfig value)
    {
        value.CloneConfigTo(ConfigRoot, $"{Path}{Split}{value.AppenderName}");
    }

    protected List<TEither> ClearAndCopyEitherTo<TEither>(List<TEither> list)
        where TEither : IAppenderReferenceConfig
    {
        static IMutableAppenderReferenceConfig? LoadNewConfig
        (
            List<TEither> cachedAppenderReferences
          , Dictionary<string, IMutableAppenderReferenceConfig> appendersByName
          , IConfigurationRoot configRoot, string path, string appenderName)
        {
            if (FLogCreate.MakeAppenderConfig(configRoot, path) is { } appenderConfig)
            {
                if (appenderConfig.AppenderName != appenderName) appenderConfig.AppenderName = appenderName;
                appendersByName.Add(appenderConfig.AppenderName, appenderConfig);
                if (appenderConfig is TEither appenderEitherConfig) cachedAppenderReferences.Add(appenderEitherConfig);
                return appenderConfig;
            }
            return null;
        }

        list.Clear();

        nextConfigReadTime = TimeContext.UtcNow;
        foreach (var appenderRefs in GetChildren())
        {
            var appenderRefPath = $"{Path}{Split}{appenderRefs.Key}";
            if (AppendersByName.TryGetValue(appenderRefs.Key, out var appenderRef))
            {
                var appenderType = ConfigRoot.GetAppenderType(appenderRefPath);
                if (appenderType == appenderRef.AppenderType)
                {
                    list.Add((TEither)appenderRef);
                }
                else
                {
                    AppendersByName.Remove(appenderRefs.Key);
                    var appenderConfig = LoadNewConfig(list, AppendersByName, ConfigRoot, appenderRefPath, appenderRefs.Key);
                    if (appenderConfig != null) appenderConfig.ParentConfig = this;
                }
            }
            else
            {
                var appenderConfig = LoadNewConfig(list, AppendersByName, ConfigRoot, appenderRefPath, appenderRefs.Key);
                if (appenderConfig != null) appenderConfig.ParentConfig = this;
            }
        }
        nextConfigReadTime = TimeContext.UtcNow.Add(recheckTimeSpanInterval);
        return list;
    }

    object ICloneable.Clone() => Clone();

    INamedAppendersLookupConfig ICloneable<INamedAppendersLookupConfig>.Clone() => Clone();

    public virtual NamedAppendersLookupConfig Clone() => new(this);

    public virtual bool AreEquivalent(INamedAppendersLookupConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var countSame = Count == other.Count;
        if (countSame)
        {
            var nameKeys = NameKeys.ToList();
            foreach (var nameKey in nameKeys)
            {
                var myConfig = this[nameKey];
                if (other.TryGetValue(nameKey, out var otherConfig))
                {
                    if (!myConfig.AreEquivalent(otherConfig, exactTypes)) return false;
                }
                else
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
        var hashCode = AppendersByName.GetHashCode();
        return hashCode;
    }

    public virtual AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll(AppendersByName)
           .Complete();

    public override string ToString() => this.DefaultToString();
}
