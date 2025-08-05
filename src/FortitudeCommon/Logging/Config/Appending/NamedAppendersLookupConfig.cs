// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending;

public interface INamedAppendersLookupConfig<TReadOnly> :IInterfacesComparable<INamedAppendersLookupConfig<TReadOnly>>
  , ICloneable< INamedAppendersLookupConfig<TReadOnly>>, IStyledToStringObject, IFLogConfig
where TReadOnly : class, IAppenderReferenceConfig
{
    bool ContainsKey(string key);
    bool TryGetValue(string key, [MaybeNullWhen(false)] out TReadOnly value);

    int Count { get; }

    TReadOnly this[string key] { get; }
    IEnumerable<string>    Keys   { get; }
    IEnumerable<TReadOnly> Values { get; }

    IEnumerator<KeyValuePair<string, TReadOnly>> GetEnumerator();

    List<TReadOnly> ClearAndCopyTo(List<TReadOnly> list);
}

public interface INamedAppendersLookupConfig : INamedAppendersLookupConfig<IAppenderReferenceConfig>
{
}

public interface IAppendableNamedAppendersLookupConfig<T, TReadOnly> : INamedAppendersLookupConfig<TReadOnly>
  , IAppendableDictionary<string, T>, IMutableFLogConfig
    where T : class, IMutableAppenderReferenceConfig, TReadOnly
    where TReadOnly : class, IAppenderReferenceConfig
{
    new IEnumerable<string> Keys { get; }

    new IEnumerable<T> Values { get; }

    new int Count { get; }

    IAppendableNamedAppendersLookupConfig<T, TReadOnly> Add(TReadOnly value);

    IAppendableNamedAppendersLookupConfig<T, TReadOnly> Add(T value);

    new T this[string loggerName] { get; set; }

    new bool ContainsKey(string appenderName);

    new IEnumerator<KeyValuePair<string, T>> GetEnumerator();
}

public interface IAppendableNamedAppendersLookupConfig : IAppendableNamedAppendersLookupConfig<IMutableAppenderReferenceConfig, IAppenderReferenceConfig>, INamedAppendersLookupConfig
{
}

public abstract class NamedAppendersLookupConfig<T, TReadOnly> : FLogConfig, IAppendableNamedAppendersLookupConfig<T, TReadOnly>
    where T : class, IMutableAppenderReferenceConfig, TReadOnly
    where TReadOnly : class, IAppenderReferenceConfig

{
    private DateTime nextConfigReadTime = DateTime.MinValue;

    private static TimeSpan recheckTimeSpanInterval;

    private readonly List<T> cachedAppenderReferences = new();

    protected readonly Dictionary<string, T> AppendersByName = new();

    protected NamedAppendersLookupConfig(IConfigurationRoot root, string path) : base(root, path)
    {
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);
    }

    protected NamedAppendersLookupConfig() : this(InMemoryConfigRoot, InMemoryPath)
    {
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);
    }

    protected NamedAppendersLookupConfig(params T[] toAdd)
        : this(InMemoryConfigRoot, InMemoryPath, toAdd)
    {
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);
    }

    protected NamedAppendersLookupConfig
        (IConfigurationRoot root, string path, params T[] toAdd) : base(root, path)
    {
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);
        for (int i = 0; i < toAdd.Length; i++)
        {
            AppendersByName.Add(toAdd[i].AppenderName, toAdd[i]);
            PushToConfigStorage(toAdd[i]);
        }
    }

    protected NamedAppendersLookupConfig(INamedAppendersLookupConfig<TReadOnly> toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);
        foreach (var kvp in toClone)
        {
            AppendersByName.Add(kvp.Key, (T)kvp.Value);
            PushToConfigStorage(kvp.Value);
        }
    }

    protected NamedAppendersLookupConfig(INamedAppendersLookupConfig<TReadOnly> toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath)
    {
    }

    public virtual void Add(KeyValuePair<string, T> item)
    {
        AppendersByName.Add(item.Key, item.Value);
        PushToConfigStorage(item.Value);
    }

    public IAppendableNamedAppendersLookupConfig<T, TReadOnly> Add(TReadOnly value)
    {
        AppendersByName.Add(value.AppenderName, (T)value);
        PushToConfigStorage(value);

        return this;
    }

    public IAppendableNamedAppendersLookupConfig<T, TReadOnly> Add(T value)
    {
        AppendersByName.Add(value.AppenderName, value);
        PushToConfigStorage(value);

        return this;
    }

    public virtual void Add(string key, T value)
    {
        AppendersByName.Add(key, value);
        PushToConfigStorage(value);
    }

    protected void PushToConfigStorage(TReadOnly value)
    {
        value.CloneConfigTo(ConfigRoot, $"{Path}{Split}{value.AppenderName}");
    }

    [JsonIgnore]
    protected Dictionary<string, T> CheckConfigGetAppendersDict
    {
        get
        {
            if (!AppendersByName.Any() || nextConfigReadTime < TimeContext.UtcNow)
            {
                recheckTimeSpanInterval = FLogContext.NullOnUnstartedContext?.ConfigRegistry?.ExpireConfigCacheIntervalTimeSpan ?? TimeSpan.FromMinutes(1);
                AppendersByName.Clear();
                foreach (var configurationSection in GetChildren())
                {
                    if (FLogCreate.MakeAppenderConfig(ConfigRoot, $"{configurationSection.Path}") is T appenderConfig)
                    {
                        appenderConfig.ParentConfig = this;
                        AppendersByName.TryAdd(configurationSection.Key, appenderConfig);
                    }
                }
                nextConfigReadTime = TimeContext.UtcNow.Add(recheckTimeSpanInterval);
            }
            return AppendersByName;
        }
    }

    public bool ContainsKey(string appenderName) => CheckConfigGetAppendersDict.ContainsKey(appenderName);

    bool INamedAppendersLookupConfig<TReadOnly>.ContainsKey(string appenderName) => CheckConfigGetAppendersDict.ContainsKey(appenderName);

    bool IReadOnlyDictionary<string, T>.ContainsKey(string appenderName) => CheckConfigGetAppendersDict.ContainsKey(appenderName);

    public int Count => CheckConfigGetAppendersDict.Count;

    public List<TReadOnly> ClearAndCopyTo(List<TReadOnly> list)
    {
        return ClearAndCopyEitherTo(list);
    }

    protected List<TEither> ClearAndCopyEitherTo<TEither>(List<TEither> list) 
    where TEither : TReadOnly
    {
        static T? LoadNewConfig
        (
            List<TEither> cachedAppenderReferences
          , Dictionary<string, T> appendersByName
          , IConfigurationRoot configRoot, string path)
        {
            if (FLogCreate.MakeAppenderConfig(configRoot, path) is T appenderConfig)
            {
                appendersByName.Add(appenderConfig.AppenderName, appenderConfig);
                if (appenderConfig is TEither appenderEitherConfig)
                {
                    cachedAppenderReferences.Add(appenderEitherConfig);
                }
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
                    list.Add((TEither)(TReadOnly)appenderRef);
                }
                else
                {
                    AppendersByName.Remove(appenderRefs.Key);
                    var appenderConfig = LoadNewConfig(list, AppendersByName, ConfigRoot, appenderRefPath);
                    if (appenderConfig != null)
                    {
                        appenderConfig.ParentConfig = this;
                    }
                }
            }
            else
            {
                var appenderConfig = LoadNewConfig(list, AppendersByName, ConfigRoot, appenderRefPath);
                if (appenderConfig != null)
                {
                    appenderConfig.ParentConfig = this;
                }
            }
        }
        nextConfigReadTime = TimeContext.UtcNow.Add(recheckTimeSpanInterval);
        return list;
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

    IEnumerator IEnumerable.GetEnumerator() => CheckConfigGetAppendersDict.GetEnumerator();

    IEnumerator<KeyValuePair<string, T>> IAppendableNamedAppendersLookupConfig<T, TReadOnly>.GetEnumerator() =>
        CheckConfigGetAppendersDict.GetEnumerator();

    IEnumerator<KeyValuePair<string, TReadOnly>> INamedAppendersLookupConfig<TReadOnly>.GetEnumerator() =>
        CheckConfigGetAppendersDict
            .Select(abnKvp => 
                        new KeyValuePair<string, TReadOnly>(abnKvp.Key, abnKvp.Value))
            .GetEnumerator();

    public IEnumerator<KeyValuePair<string, T>> GetEnumerator() => CheckConfigGetAppendersDict.GetEnumerator();

    public new T this[string loggerName]
    {
        get => CheckConfigGetAppendersDict[loggerName];
        set => CheckConfigGetAppendersDict[loggerName] = value;
    }

    public IAppenderReferenceConfig this[int toCountIndex] => OrderAppenders[toCountIndex];

    TReadOnly INamedAppendersLookupConfig<TReadOnly>.this[string loggerName] => this[loggerName];

    IEnumerable<string> IReadOnlyDictionary<string, T>.Keys => CheckConfigGetAppendersDict.Keys;

    IEnumerable<string> INamedAppendersLookupConfig<TReadOnly>.Keys => CheckConfigGetAppendersDict.Keys;

    ICollection<string> IAppendableDictionary<string, T>.Keys => CheckConfigGetAppendersDict.Keys;

    IEnumerable<string> IAppendableNamedAppendersLookupConfig<T, TReadOnly>.Keys => CheckConfigGetAppendersDict.Keys;

    public IEnumerable<string> NameKeys => CheckConfigGetAppendersDict.Keys;

    public bool TryGetValue(string appenderName, [MaybeNullWhen(false)] out T value) =>
        CheckConfigGetAppendersDict.TryGetValue(appenderName, out value);

    public bool TryGetValue(string appenderName, [MaybeNullWhen(false)] out TReadOnly value)
    {
        value = null;
        if (CheckConfigGetAppendersDict.TryGetValue(appenderName, out var config))
        {
            value = config;
            return true;
        }
        return false;
    }

    IEnumerable<T> IAppendableNamedAppendersLookupConfig<T, TReadOnly>.Values => CheckConfigGetAppendersDict.Values;

    IEnumerable<T> IReadOnlyDictionary<string, T>.Values => CheckConfigGetAppendersDict.Values;

    IEnumerable<TReadOnly> INamedAppendersLookupConfig<TReadOnly>.Values => CheckConfigGetAppendersDict.Values;

    ICollection<T> IAppendableDictionary<string, T>.Values => CheckConfigGetAppendersDict.Values;

    object ICloneable.Clone() => Clone();

    INamedAppendersLookupConfig<TReadOnly> ICloneable<INamedAppendersLookupConfig<TReadOnly>>.Clone() => Clone();

    public abstract NamedAppendersLookupConfig<T, TReadOnly> Clone();

    public virtual bool AreEquivalent(INamedAppendersLookupConfig<TReadOnly>? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var countSame = Count == other.Count;
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

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as INamedAppendersLookupConfig<TReadOnly>, true);

    public override int GetHashCode()
    {
        var hashCode = AppendersByName.GetHashCode();
        return hashCode;
    }

    public abstract StyledTypeBuildResult ToString(IStyledTypeStringAppender sbc);

    public override string ToString() => this.DefaultToString();
}

public class NamedAppendersLookupConfig : NamedAppendersLookupConfig<IMutableAppenderReferenceConfig, IAppenderReferenceConfig>, IAppendableNamedAppendersLookupConfig
{
    public NamedAppendersLookupConfig(IConfigurationRoot root, string path) : base(root, path) { }
    public NamedAppendersLookupConfig() { }
    public NamedAppendersLookupConfig(params IMutableAppenderReferenceConfig[] toAdd) : base(toAdd) { }
    public NamedAppendersLookupConfig(IConfigurationRoot root, string path, params IMutableAppenderReferenceConfig[] toAdd) : base(root, path, toAdd) { }
    public NamedAppendersLookupConfig(INamedAppendersLookupConfig toClone, IConfigurationRoot root, string path) : base(toClone, root, path) { }
    public NamedAppendersLookupConfig(INamedAppendersLookupConfig toClone) : base(toClone) { }

    public override NamedAppendersLookupConfig Clone() => new (this);

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    public override StyledTypeBuildResult ToString(IStyledTypeStringAppender sbc)
    {
        return
            sbc.StartKeyedCollectionType(nameof(NamedAppendersLookupConfig))
               .AddAll(AppendersByName)
               .Complete();
    }
}