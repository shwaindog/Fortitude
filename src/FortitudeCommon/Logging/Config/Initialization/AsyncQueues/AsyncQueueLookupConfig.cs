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

namespace FortitudeCommon.Logging.Config.Initialization.AsyncQueues;

public interface IAsyncQueueLookupConfig : IFLogConfig, IStickyKeyValueDictionary<byte, IAsyncQueueConfig>
  , IInterfacesComparable<IAsyncQueueLookupConfig>, ICloneable<IAsyncQueueLookupConfig>
  , IStringBearer
{
    IAsyncQueuesInitConfig? ParentDefaultQueuesInitConfig { get; }
}

public interface IAppendableAsyncQueueLookupConfig : IMutableFLogConfig, IAsyncQueueLookupConfig
  , IAppendableDictionary<byte, IMutableAsyncQueueConfig>
{
    new IEnumerable<byte> Keys { get; }

    new IEnumerable<IMutableAsyncQueueConfig> Values { get; }

    new int Count { get; }

    new IMutableAsyncQueueConfig this[byte appenderName] { get; set; }

    new IMutableAsyncQueuesInitConfig? ParentDefaultQueuesInitConfig { get; }

    new bool ContainsKey(byte queueNumber);

    new IEnumerator<KeyValuePair<byte, IMutableAsyncQueueConfig>> GetEnumerator();
}

public class AsyncQueueLookupConfig : FLogConfig, IAppendableAsyncQueueLookupConfig
{
    private static TimeSpan recheckTimeSpanInterval;

    private readonly List<KeyValuePair<byte, IMutableAsyncQueueConfig>> asyncQueueConfigList = new();

    private readonly Dictionary<byte, IMutableAsyncQueueConfig> queueConfigByQueueNumber = new();

    private DateTime nextConfigReadTime = DateTime.MinValue;

    public AsyncQueueLookupConfig(IConfigurationRoot root, string path) : base(root, path) => recheckTimeSpanInterval = TimeSpan.FromMinutes(1);

    public AsyncQueueLookupConfig() : this(InMemoryConfigRoot, InMemoryPath) => recheckTimeSpanInterval = TimeSpan.FromMinutes(1);

    public AsyncQueueLookupConfig(params IMutableAsyncQueueConfig[] toAdd)
        : this(InMemoryConfigRoot, InMemoryPath, toAdd) =>
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);

    public AsyncQueueLookupConfig
        (IConfigurationRoot root, string path, params IMutableAsyncQueueConfig[] toAdd) : base(root, path)
    {
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);
        for (var i = 0; i < toAdd.Length; i++) queueConfigByQueueNumber.Add(toAdd[i].QueueNumber, toAdd[i]);
    }

    public AsyncQueueLookupConfig(IAsyncQueueLookupConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);
        foreach (var kvp in toClone) queueConfigByQueueNumber.Add(kvp.Key, (IMutableAsyncQueueConfig)kvp.Value);
    }

    public AsyncQueueLookupConfig(IAsyncQueueLookupConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) =>
        recheckTimeSpanInterval = TimeSpan.FromMinutes(1);

    [JsonIgnore]
    protected Dictionary<byte, IMutableAsyncQueueConfig> CheckConfigGetConfigByQueueNumberDict
    {
        get
        {
            if (!queueConfigByQueueNumber.Any() || nextConfigReadTime < TimeContext.UtcNow)
            {
                recheckTimeSpanInterval = FLogContext.NullOnUnstartedContext?.ConfigRegistry?.ExpireConfigCacheIntervalTimeSpan ??
                                          TimeSpan.FromMinutes(1);
                queueConfigByQueueNumber.Clear();
                foreach (var configurationSection in GetSection(Path).GetChildren())
                {
                    var asyncQueueConfig = new AsyncQueueConfig(ConfigRoot, $"{configurationSection.Path}{Split}{configurationSection.Key}");
                    var checkQueueNumber = byte.TryParse(configurationSection.Key, out var keyQueueNumber);
                    if (checkQueueNumber)
                    {
                        if (asyncQueueConfig.QueueNumber != keyQueueNumber) asyncQueueConfig.QueueNumber = keyQueueNumber;
                        asyncQueueConfig.ParentConfig = this;
                        queueConfigByQueueNumber.TryAdd(keyQueueNumber, asyncQueueConfig);
                    }
                }
                nextConfigReadTime = TimeContext.UtcNow.Add(recheckTimeSpanInterval);
            }
            return queueConfigByQueueNumber;
        }
    }

    protected IReadOnlyList<KeyValuePair<byte, IMutableAsyncQueueConfig>> AsyncQueuesConfig
    {
        get
        {
            if (nextConfigReadTime < TimeContext.UtcNow)
            {
                ClearAndCopyEitherTo(asyncQueueConfigList);
                asyncQueueConfigList.Sort((lhs, rhs) => lhs.Key - rhs.Key);
                nextConfigReadTime = TimeContext.UtcNow.Add(recheckTimeSpanInterval);
            }
            return asyncQueueConfigList;
        }
    }

    IAsyncQueuesInitConfig? IAsyncQueueLookupConfig.ParentDefaultQueuesInitConfig => ParentDefaultQueuesInitConfig;

    public IMutableAsyncQueuesInitConfig? ParentDefaultQueuesInitConfig =>
        ParentConfig as IMutableAsyncQueuesInitConfig ?? FLogContext.Context.AsyncRegistry.AsyncBufferingConfig as IMutableAsyncQueuesInitConfig;

    public void Add(KeyValuePair<byte, IMutableAsyncQueueConfig> item)
    {
        queueConfigByQueueNumber.Add(item.Key, item.Value);
        PushToConfigStorage(item.Value);
    }

    public void Add(byte key, IMutableAsyncQueueConfig value)
    {
        queueConfigByQueueNumber.Add(key, value);
        PushToConfigStorage(value);
    }

    bool IReadOnlyDictionary<byte, IAsyncQueueConfig>.ContainsKey(byte priorityOrder) =>
        CheckConfigGetConfigByQueueNumberDict.ContainsKey(priorityOrder);

    bool IAppendableAsyncQueueLookupConfig.ContainsKey(byte priorityOrder) => CheckConfigGetConfigByQueueNumberDict.ContainsKey(priorityOrder);

    public bool ContainsKey(byte priorityOrder) => CheckConfigGetConfigByQueueNumberDict.ContainsKey(priorityOrder);

    public int Count => CheckConfigGetConfigByQueueNumberDict.Count;

    IEnumerator IEnumerable.GetEnumerator() => AsyncQueuesConfig.GetEnumerator();

    IEnumerator<KeyValuePair<byte, IMutableAsyncQueueConfig>> IAppendableAsyncQueueLookupConfig.GetEnumerator() => AsyncQueuesConfig.GetEnumerator();

    IEnumerator<KeyValuePair<byte, IAsyncQueueConfig>> IEnumerable<KeyValuePair<byte, IAsyncQueueConfig>>.GetEnumerator() =>
        AsyncQueuesConfig
            .Select(arcKvp => new KeyValuePair<byte, IAsyncQueueConfig>(arcKvp.Key, arcKvp.Value))
            .GetEnumerator();

    public IEnumerator<KeyValuePair<byte, IMutableAsyncQueueConfig>> GetEnumerator() => AsyncQueuesConfig.GetEnumerator();

    public IMutableAsyncQueueConfig this[byte appenderName]
    {
        get => CheckConfigGetConfigByQueueNumberDict[appenderName];
        set => CheckConfigGetConfigByQueueNumberDict[appenderName] = value;
    }

    IAsyncQueueConfig IReadOnlyDictionary<byte, IAsyncQueueConfig>.this[byte appenderName] => this[appenderName];

    public IEnumerable<byte> Keys => AsyncQueuesConfig.Select(kvp => kvp.Key);

    ICollection<byte> IAppendableDictionary<byte, IMutableAsyncQueueConfig>.Keys => AsyncQueuesConfig.Select(kvp => kvp.Key).ToList();

    public bool TryGetValue(byte queueNumber, [MaybeNullWhen(false)] out IMutableAsyncQueueConfig value) =>
        CheckConfigGetConfigByQueueNumberDict.TryGetValue(queueNumber, out value);

    public bool TryGetValue(byte queueNumber, [MaybeNullWhen(false)] out IAsyncQueueConfig value)
    {
        value = null;
        if (CheckConfigGetConfigByQueueNumberDict.TryGetValue(queueNumber, out var config))
        {
            value = config;
            return true;
        }
        return false;
    }

    public IEnumerable<IMutableAsyncQueueConfig> Values => AsyncQueuesConfig.Select(kvp => kvp.Value);

    IEnumerable<IAsyncQueueConfig> IReadOnlyDictionary<byte, IAsyncQueueConfig>.Values => AsyncQueuesConfig.Select(kvp => kvp.Value);

    ICollection<IMutableAsyncQueueConfig> IAppendableDictionary<byte, IMutableAsyncQueueConfig>.Values =>
        AsyncQueuesConfig.Select(kvp => kvp.Value).ToList();

    public override T Accept<T>(T visitor) => visitor.Visit(this);

    protected void PushToConfigStorage(IMutableAsyncQueueConfig value)
    {
        value.CloneConfigTo(ConfigRoot, $"{Path}{Split}{value.QueueNumber}");
    }

    protected List<KeyValuePair<byte, IMutableAsyncQueueConfig>> ClearAndCopyEitherTo(List<KeyValuePair<byte, IMutableAsyncQueueConfig>> list)
    {
        static IMutableAsyncQueueConfig LoadNewAsyncQueueConfig
        (
            List<KeyValuePair<byte, IMutableAsyncQueueConfig>> asyncQueuesConfig
          , Dictionary<byte, IMutableAsyncQueueConfig> asyncQueuesByQueueNumDict
          , byte keyQueueNumber, IConfigurationRoot configRoot, string path)
        {
            var asyncQueue                                                       = new AsyncQueueConfig(configRoot, path);
            if (asyncQueue.QueueNumber != keyQueueNumber) asyncQueue.QueueNumber = keyQueueNumber;
            asyncQueuesByQueueNumDict.Add(keyQueueNumber, asyncQueue);
            asyncQueuesConfig.Add(new KeyValuePair<byte, IMutableAsyncQueueConfig>(asyncQueue.QueueNumber, asyncQueue));
            return asyncQueue;
        }

        list.Clear();

        nextConfigReadTime = TimeContext.UtcNow;
        foreach (var appenderRefs in GetChildren())
        {
            var configSourcePath = $"{Path}{Split}{appenderRefs.Key}";
            if (byte.TryParse(appenderRefs.Key, out var keyQueueNumber))
            {
                if (queueConfigByQueueNumber.TryGetValue(keyQueueNumber, out var asyncQueueConfig))
                {
                    list.Add(new KeyValuePair<byte, IMutableAsyncQueueConfig>(keyQueueNumber, asyncQueueConfig));
                }
                else
                {
                    var appenderConfig = LoadNewAsyncQueueConfig(list, queueConfigByQueueNumber, keyQueueNumber, ConfigRoot, configSourcePath);
                    appenderConfig.ParentConfig = this;
                }
            }
        }
        nextConfigReadTime = TimeContext.UtcNow.Add(recheckTimeSpanInterval);
        return list;
    }

    object ICloneable.Clone() => Clone();

    IAsyncQueueLookupConfig ICloneable<IAsyncQueueLookupConfig>.Clone() => Clone();

    public virtual AsyncQueueLookupConfig Clone() => new(this);

    public virtual bool AreEquivalent(IAsyncQueueLookupConfig? other, bool exactTypes = false)
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

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IAsyncQueueLookupConfig, true);

    public override int GetHashCode()
    {
        var hashCode = queueConfigByQueueNumber.GetHashCode();
        return hashCode;
    }

    public AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll(queueConfigByQueueNumber)
           .Complete();

    public override string ToString() => this.DefaultToString();
}
