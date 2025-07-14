// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Config;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config;

public interface IFLoggerConfigSourceDefinitions : IStickyKeyValueDictionary<uint, IFloggerConfigSource>
  , ICloneable<IFLoggerConfigSourceDefinitions>, IInterfacesComparable<IFLoggerConfigSourceDefinitions>
  , IStyledToStringObject { }

public interface IMutableFLoggerConfigSourceDefinitions : IFLoggerConfigSourceDefinitions, IAppendableDictionary<uint, IFloggerConfigSource> { }

public class FLoggerConfigSourceDefinitions : ConfigSection, IMutableFLoggerConfigSourceDefinitions
{
    private Dictionary<uint, IFloggerConfigSource> configSourceByPriority = new();

    public FLoggerConfigSourceDefinitions(IConfigurationRoot root, string path) : base(root, path) { }

    public FLoggerConfigSourceDefinitions() : this(InMemoryConfigRoot, InMemoryPath) { }

    public FLoggerConfigSourceDefinitions(params IFloggerConfigSource[] toAdd)
        : this(InMemoryConfigRoot, InMemoryPath, toAdd) { }


    public FLoggerConfigSourceDefinitions
        (IConfigurationRoot root, string path, params IFloggerConfigSource[] toAdd) : base(root, path)
    {
        for (int i = 0; i < toAdd.Length; i++)
        {
            configSourceByPriority.Add(toAdd[i].ConfigPriorityOrder, toAdd[i]);
        }
    }

    public FLoggerConfigSourceDefinitions(IFLoggerConfigSourceDefinitions toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        foreach (var kvp in toClone)
        {
            configSourceByPriority.Add(kvp.Key, kvp.Value);
        }
    }

    public FLoggerConfigSourceDefinitions(IFLoggerConfigSourceDefinitions toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public void Add(KeyValuePair<uint, IFloggerConfigSource> item)
    {
        configSourceByPriority.Add(item.Key, item.Value);
    }

    public void Add(uint key, IFloggerConfigSource value)
    {
        configSourceByPriority.Add(key, value);
    }

    public bool ContainsKey(uint priority) => configSourceByPriority.ContainsKey(priority);


    public int Count => configSourceByPriority.Count;

    IEnumerator IEnumerable.GetEnumerator() => configSourceByPriority.GetEnumerator();

    public IEnumerator<KeyValuePair<uint, IFloggerConfigSource>> GetEnumerator() => configSourceByPriority.GetEnumerator();

    public new IFloggerConfigSource this[uint loggerName]
    {
        get => configSourceByPriority[loggerName];
        set => configSourceByPriority[loggerName] = value;
    }

    public ICollection<uint> Keys => configSourceByPriority.Keys;

    IEnumerable<uint> IReadOnlyDictionary<uint, IFloggerConfigSource>.Keys => configSourceByPriority.Keys;

    public ICollection<IFloggerConfigSource> Values => configSourceByPriority.Values;

    IEnumerable<IFloggerConfigSource> IReadOnlyDictionary<uint, IFloggerConfigSource>.Values => configSourceByPriority.Values;

    public bool TryGetValue(uint priority, [MaybeNullWhen(false)] out IFloggerConfigSource value) =>
        configSourceByPriority.TryGetValue(priority, out value);


    object ICloneable.Clone() => Clone();

    IFLoggerConfigSourceDefinitions ICloneable<IFLoggerConfigSourceDefinitions>.Clone() => Clone();

    public virtual FLoggerConfigSourceDefinitions Clone() => new(this);

    public virtual bool AreEquivalent(IFLoggerConfigSourceDefinitions? other, bool exactTypes = false)
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

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IFLoggerConfigSourceDefinitions, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = configSourceByPriority?.GetHashCode() ?? 0;
            return hashCode;
        }
    }

    public void ToString(IStyledTypeStringAppender sbc)
    {
        sbc.AddTypeName(nameof(AppenderReferenceConfig))
           .AddTypeStart()
           .AddCollectionField(nameof(configSourceByPriority), configSourceByPriority)
           .AddTypeEnd();
    }


    public override string ToString() => this.DefaultToString();
}
