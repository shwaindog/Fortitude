// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.LoggersHierarchy;

public interface INamedChildLoggersLookupConfig : IStickyKeyValueDictionary<string, IFLoggerDescendantConfig>
  , IInterfacesComparable<INamedChildLoggersLookupConfig>, ICloneable<INamedChildLoggersLookupConfig>, IFLogConfig
  , IStyledToStringObject { }

public interface IMutableNamedChildLoggersLookupConfig : INamedChildLoggersLookupConfig
  , IAppendableDictionary<string, IMutableFLoggerDescendantConfig>, IMutableFLogConfig
{
    new IEnumerable<string> Keys { get; }

    new IEnumerable<IMutableFLoggerDescendantConfig> Values { get; }

    new int Count { get; }

    new IMutableFLoggerDescendantConfig this[string appenderName] { get; set; }

    new bool ContainsKey(string key);

    void Add(IMutableFLoggerDescendantConfig toAdd);

    new bool TryGetValue(string appenderName, [MaybeNullWhen(false)] out IMutableFLoggerDescendantConfig value);

    new IEnumerator<KeyValuePair<string, IMutableFLoggerDescendantConfig>> GetEnumerator();
}

public class NamedChildLoggersLookupConfig : FLogConfig, IMutableNamedChildLoggersLookupConfig
{
    private static TimeSpan recheckTimeSpanInterval;

    private readonly Dictionary<string, IMutableFLoggerDescendantConfig> loggersByName = new();

    private DateTime nextConfigReadTime = DateTime.MinValue;

    public NamedChildLoggersLookupConfig(IConfigurationRoot root, string path) : base(root, path) =>
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);

    public NamedChildLoggersLookupConfig() : this(InMemoryConfigRoot, InMemoryPath) => recheckTimeSpanInterval = TimeSpan.FromMinutes(1);

    public NamedChildLoggersLookupConfig(params IMutableFLoggerDescendantConfig[] toAdd)
        : this(InMemoryConfigRoot, InMemoryPath, toAdd) =>
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);

    public NamedChildLoggersLookupConfig
        (IConfigurationRoot root, string path, params IMutableFLoggerDescendantConfig[] toAdd) : base(root, path)
    {
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);
        for (var i = 0; i < toAdd.Length; i++) loggersByName.Add(toAdd[i].Name, toAdd[i]);
    }

    public NamedChildLoggersLookupConfig(INamedChildLoggersLookupConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);
        foreach (var kvp in toClone)
        {
            var childCloneLogger = (IMutableFLoggerDescendantConfig)kvp.Value.Clone();
            loggersByName.Add(kvp.Key, childCloneLogger);
            childCloneLogger.ParentConfig = this;
            PushToConfigStorage(childCloneLogger);
        }
    }

    public NamedChildLoggersLookupConfig(INamedChildLoggersLookupConfig toClone) : this(toClone, InMemoryConfigRoot, toClone.ConfigSubPath) =>
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);


    protected Dictionary<string, IMutableFLoggerDescendantConfig> CheckConfigGetLoggersDict
    {
        get
        {
            if (!loggersByName.Any() || nextConfigReadTime < TimeContext.UtcNow)
            {
                recheckTimeSpanInterval = FLogContext.NullOnUnstartedContext?.ConfigRegistry.ExpireConfigCacheIntervalTimeSpan ??
                                          TimeSpan.FromMinutes(1);
                loggersByName.Clear();
                foreach (var configurationSection in GetSection(Path).GetChildren())
                {
                    var descendantConfig =
                        FLogCreate.MakeDescendantLoggerConfig(ConfigRoot, $"{configurationSection.Path}{Split}{configurationSection.Key}");
                    descendantConfig.ParentConfig = this;

                    loggersByName.TryAdd(configurationSection.Key, descendantConfig);
                }
                nextConfigReadTime = TimeContext.UtcNow.Add(recheckTimeSpanInterval);
            }
            return loggersByName;
        }
    }

    public void Add(KeyValuePair<string, IMutableFLoggerDescendantConfig> item)
    {
        loggersByName.Add(item.Key, item.Value);
        item.Value.ParentConfig = this;
        PushToConfigStorage(item.Value);
    }

    public void Add(string key, IMutableFLoggerDescendantConfig value)
    {
        loggersByName.Add(key, value);
        value.ParentConfig = this;
        PushToConfigStorage(value);
    }

    public void Add(IMutableFLoggerDescendantConfig toAdd)
    {
        loggersByName.Add(toAdd.Name, toAdd);
        toAdd.ParentConfig = this;
        PushToConfigStorage(toAdd);
    }

    public bool ContainsKey(string loggerName) => CheckConfigGetLoggersDict.ContainsKey(loggerName);

    public int Count => CheckConfigGetLoggersDict.Count;

    public new IMutableFLoggerDescendantConfig this[string appenderName]
    {
        get => CheckConfigGetLoggersDict[appenderName];
        set => CheckConfigGetLoggersDict[appenderName] = value;
    }

    IFLoggerDescendantConfig IReadOnlyDictionary<string, IFLoggerDescendantConfig>.this[string appenderName] => this[appenderName];

    IEnumerable<string> IReadOnlyDictionary<string, IMutableFLoggerDescendantConfig>.Keys => CheckConfigGetLoggersDict.Keys;

    IEnumerable<string> IReadOnlyDictionary<string, IFLoggerDescendantConfig>.Keys => CheckConfigGetLoggersDict.Keys;

    public ICollection<string> Keys => CheckConfigGetLoggersDict.Keys;

    IEnumerable<string> IMutableNamedChildLoggersLookupConfig.Keys => CheckConfigGetLoggersDict.Keys;

    public bool TryGetValue(string appenderName, [MaybeNullWhen(false)] out IMutableFLoggerDescendantConfig value) =>
        CheckConfigGetLoggersDict.TryGetValue(appenderName, out value);

    bool IReadOnlyDictionary<string, IFLoggerDescendantConfig>.TryGetValue
        (string appenderName, [MaybeNullWhen(false)] out IFLoggerDescendantConfig value)
    {
        value = null;
        if (CheckConfigGetLoggersDict.TryGetValue(appenderName, out var config))
        {
            value = config;
            return true;
        }
        return false;
    }

    IEnumerable<IMutableFLoggerDescendantConfig> IMutableNamedChildLoggersLookupConfig.Values => CheckConfigGetLoggersDict.Values;

    IEnumerable<IMutableFLoggerDescendantConfig> IReadOnlyDictionary<string, IMutableFLoggerDescendantConfig>.Values =>
        CheckConfigGetLoggersDict.Values;

    IEnumerable<IFLoggerDescendantConfig> IReadOnlyDictionary<string, IFLoggerDescendantConfig>.Values => CheckConfigGetLoggersDict.Values;

    ICollection<IMutableFLoggerDescendantConfig> IAppendableDictionary<string, IMutableFLoggerDescendantConfig>.Values =>
        CheckConfigGetLoggersDict.Values;

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    protected void PushToConfigStorage(IMutableFLoggerDescendantConfig value)
    {
        value.CloneConfigTo(ConfigRoot, $"{Path}{Split}{value.Name}");
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<KeyValuePair<string, IFLoggerDescendantConfig>> IEnumerable<KeyValuePair<string, IFLoggerDescendantConfig>>.GetEnumerator() =>
        CheckConfigGetLoggersDict
            .Select(arcKvp =>
                        new KeyValuePair<string, IFLoggerDescendantConfig>(arcKvp.Key, arcKvp.Value))
            .GetEnumerator();

    public IEnumerator<KeyValuePair<string, IMutableFLoggerDescendantConfig>> GetEnumerator() => CheckConfigGetLoggersDict.GetEnumerator();

    object ICloneable.Clone() => Clone();

    INamedChildLoggersLookupConfig ICloneable<INamedChildLoggersLookupConfig>.Clone() => Clone();

    public virtual NamedChildLoggersLookupConfig Clone() => new(this);

    public virtual bool AreEquivalent(INamedChildLoggersLookupConfig? other, bool exactTypes = false)
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

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as INamedChildLoggersLookupConfig, true);

    public override int GetHashCode()
    {
        var hashCode = loggersByName.GetHashCode();
        return hashCode;
    }

    public StyledTypeBuildResult ToString(IStyledTypeStringAppender sbc) =>
        sbc.StartKeyedCollectionType(nameof(NamedChildLoggersLookupConfig))
           .AddAll(loggersByName)
           .Complete();

    public override string ToString() => this.DefaultToString();
}
