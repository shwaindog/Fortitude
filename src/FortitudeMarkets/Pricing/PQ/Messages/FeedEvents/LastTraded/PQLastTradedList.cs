// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

using System.Collections;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded.LastTradeEntrySelector;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;

public interface IPQLastTradedList : IMutableLastTradedList,
    IPQSupportsNumberPrecisionFieldUpdates<ILastTradedList>, IPQSupportsStringUpdates<ILastTradedList>,
    IRelatedItems<IPQNameIdLookupGenerator>, IRelatedItems<ISourceTickerInfo>
  , ISupportsPQNameIdLookupGenerator, ITrackableReset<IPQLastTradedList>, IMutableCapacityList<IPQLastTrade>
{
    new IPQLastTrade this[int index] { get; set; }

    new int Capacity { get; set; }

    new bool IsReadOnly { get; }

    new int Count { get; set; }

    new void RemoveAt(int index);

    new void Clear();

    new IPQLastTradedList Clone();

    new IEnumerator<IPQLastTrade> GetEnumerator();

    new IPQLastTradedList ResetWithTracking();
}

public class PQLastTradedList : ReusableObject<ILastTradedList>, IPQLastTradedList
{
    protected readonly List<IPQLastTrade> LastTrades;

    private IPQNameIdLookupGenerator nameIdLookupGenerator;

    protected uint NumUpdatesSinceEmpty = uint.MaxValue;

    public PQLastTradedList()
    {
        LastTrades             = [];
        nameIdLookupGenerator  = new PQNameIdLookupGenerator(PQFeedFields.LastTradedStringUpdates);
        LastTradeEntrySelector = new PQLastTradeEntrySelector(nameIdLookupGenerator);

        if (GetType() == typeof(PQLastTradedList)) NumUpdatesSinceEmpty = 0;
    }

    public PQLastTradedList(IPQNameIdLookupGenerator nameIdLookup)
    {
        LastTrades             = [];
        nameIdLookupGenerator  = nameIdLookup;
        LastTradeEntrySelector = new PQLastTradeEntrySelector(nameIdLookupGenerator);

        if (GetType() == typeof(PQLastTradedList)) NumUpdatesSinceEmpty = 0;
    }

    public PQLastTradedList(ISourceTickerInfo sourceTickerInfo)
    {
        LastTradesSupportFlags = sourceTickerInfo.LastTradedFlags;
        nameIdLookupGenerator  = new PQNameIdLookupGenerator(PQFeedFields.LastTradedStringUpdates);
        LastTradeEntrySelector = new PQLastTradeEntrySelector(nameIdLookupGenerator);

        LastTrades = new List<IPQLastTrade>();
        EnsureRelatedItemsAreConfigured(sourceTickerInfo);

        if (GetType() == typeof(PQLastTradedList)) NumUpdatesSinceEmpty = 0;
    }

    public PQLastTradedList(ISourceTickerInfo sourceTickerInfo, IPQNameIdLookupGenerator nameIdLookup)
    {
        LastTradesSupportFlags = sourceTickerInfo.LastTradedFlags;
        nameIdLookupGenerator  = nameIdLookup;
        LastTradeEntrySelector = new PQLastTradeEntrySelector(nameIdLookupGenerator);

        LastTrades = new List<IPQLastTrade>();
        EnsureRelatedItemsAreConfigured(sourceTickerInfo);

        if (GetType() == typeof(PQLastTradedList)) NumUpdatesSinceEmpty = 0;
    }

    public PQLastTradedList(IEnumerable<IPQLastTrade> lastTrades) : this(lastTrades.ToList())
    {
        LastTradesSupportFlags = lastTrades.FirstOrDefault()?.SupportsLastTradedFlags ?? LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags;
        nameIdLookupGenerator  = SourcePopulatedNameIdGeneratorFrom(lastTrades);
        LastTradeEntrySelector = new PQLastTradeEntrySelector(nameIdLookupGenerator);

        this.LastTrades = lastTrades.Select(lt => LastTradeEntrySelector.ConvertToExpectedImplementation(lt, NameIdLookup, true)).ToList();
        if (GetType() == typeof(PQLastTradedList)) NumUpdatesSinceEmpty = 0;
    }

    public PQLastTradedList(List<IPQLastTrade> lastTrades)
    {
        LastTradesSupportFlags = lastTrades.FirstOrDefault()?.SupportsLastTradedFlags  ?? LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags;
        nameIdLookupGenerator  = SourcePopulatedNameIdGeneratorFrom(lastTrades);
        LastTradeEntrySelector = new PQLastTradeEntrySelector(nameIdLookupGenerator);
        this.LastTrades        = lastTrades;
        foreach (var pqLastTrade in lastTrades)
        {
            if (pqLastTrade is ISupportsPQNameIdLookupGenerator setNameGen)
            {
                setNameGen.NameIdLookup = nameIdLookupGenerator;
            }
        }

        if (GetType() == typeof(PQLastTradedList)) NumUpdatesSinceEmpty = 0;
    }

    public PQLastTradedList(ILastTradedList toClone, IPQNameIdLookupGenerator? nameIdLookup = null)
    {
        LastTradesSupportFlags = toClone.LastTradesSupportFlags;
        nameIdLookupGenerator  = nameIdLookup ?? SourcePopulatedNameIdGeneratorFrom(toClone);
        LastTradeEntrySelector = new PQLastTradeEntrySelector(nameIdLookupGenerator);

        LastTrades = toClone.Select(lt => LastTradeEntrySelector.ConvertToExpectedImplementation(lt, NameIdLookup, true)).ToList();
        EnsureRelatedItemsAreConfigured(toClone, NameIdLookup);

        if (GetType() == typeof(PQLastTradedList)) NumUpdatesSinceEmpty = 0;
    }

    public IPQLastTradeTypeSelector LastTradeEntrySelector { get; set; }

    public LastTradeType LastTradesOfType => LastTradesSupportFlags.MostCompactLayerType();

    public LastTradedFlags LastTradesSupportFlags { get; private set; } = LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags;

    INameIdLookup IHasNameIdLookup.NameIdLookup => NameIdLookup;

    public IPQNameIdLookupGenerator NameIdLookup
    {
        get => nameIdLookupGenerator;
        set
        {
            nameIdLookupGenerator  = value;
            LastTradeEntrySelector = new PQLastTradeEntrySelector(nameIdLookupGenerator);
            foreach (var pqLastTrade in LastTrades.OfType<ISupportsPQNameIdLookupGenerator>()) pqLastTrade.NameIdLookup = value;
        }
    }

    ILastTrade IReadOnlyList<ILastTrade>.this[int i] => this[i];

    IMutableLastTrade IMutableLastTradedList.this[int i]
    {
        get => this[i];
        set => this[i] = (IPQLastTrade)value;
    }

    IMutableLastTrade IReadOnlyList<IMutableLastTrade>.this[int index] => this[index];

    IMutableLastTrade IMutableCapacityList<IMutableLastTrade>.this[int i]
    {
        get => this[i];
        set => this[i] = (IPQLastTrade)value;
    }

    public IPQLastTrade this[int i]
    {
        get
        {
            if (i > (ushort)PQDepthKey.DepthMask)
                throw new ArgumentException($"The number of last trades can not be greater than {(ushort)PQDepthKey.DepthMask}");
            while (i >= LastTrades.Count)
            {
                LastTrades.Add(LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags).CreateNewLastTradeEntry());
            }
            return LastTrades[i];
        }
        set
        {
            while (LastTrades.Count <= i)
                LastTrades.Add(LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags).CreateNewLastTradeEntry());
            LastTrades[i] = value;
            if (value is ISupportsPQNameIdLookupGenerator lastTradeWithNameId) lastTradeWithNameId.NameIdLookup = NameIdLookup;
        }
    }

    ILastTrade IList<ILastTrade>.this[int index]
    {
        get => this[index];
        set => this[index] = (IPQLastTrade)value;
    }

    ILastTrade IMutableCapacityList<ILastTrade>.this[int i]
    {
        get => this[i];
        set => this[i] = (IPQLastTrade)value;
    }

    public int Capacity
    {
        get => LastTrades.Count;
        set
        {
            if (value > PQFeedFieldsExtensions.SingleByteFieldIdMaxPossibleLastTrades)
                throw new ArgumentException("Expected PQRecentlyTraded Capacity to be less than or equal to " +
                                            PQFeedFieldsExtensions.SingleByteFieldIdMaxPossibleLastTrades);
            while (LastTrades.Count < value)
            {
                var firstLastTrade = LastTrades[0].Clone();
                firstLastTrade.StateReset();
                LastTrades.Add(firstLastTrade);
            }
        }
    }

    public int Count
    {
        get
        {
            for (var i = LastTrades.Count - 1; i >= 0; i--)
            {
                var layerAtLevel = LastTrades[i];
                if (!layerAtLevel.IsEmpty) return i + 1;
            }

            return 0;
        }
        set
        {
            for (var i = LastTrades.Count - 1; i >= value; i--)
            {
                var layerAtLevel                                             = LastTrades[i];
                if (layerAtLevel is { IsEmpty: false }) layerAtLevel.IsEmpty = true;
            }
        }
    }

    public bool HasUpdates
    {
        get { return LastTrades.Any(pqLt => pqLt.HasUpdates); }
        set
        {
            foreach (var pqLastTrade in LastTrades) pqLastTrade.HasUpdates = value;
        }
    }

    public bool HasLastTrades => Count > 0;

    public uint UpdateCount => NumUpdatesSinceEmpty;

    public bool IsReadOnly => false;

    IMutableLastTrade IList<IMutableLastTrade>.this[int index]
    {
        get => this[index];
        set => this[index] = (IPQLastTrade)value;
    }

    protected Func<IPQLastTrade> NewElementFactory =>
        () => LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags).CreateNewLastTradeEntry();

    bool ICollection<ILastTrade>.Contains(ILastTrade item) => Contains((IPQLastTrade)item);

    bool ICollection<IMutableLastTrade>.Contains(IMutableLastTrade item) => Contains((IPQLastTrade)item);

    public bool Contains(IPQLastTrade item) => LastTrades.Contains(item);

    void ICollection<ILastTrade>.CopyTo(ILastTrade[] array, int arrayIndex)
    {
        for (int i = 0; i < LastTrades.Count && i + arrayIndex < array.Length; i++)
        {
            array[i + arrayIndex] = LastTrades[i];
        }
    }

    void ICollection<IMutableLastTrade>.CopyTo(IMutableLastTrade[] array, int arrayIndex)
    {
        for (int i = 0; i < LastTrades.Count && i + arrayIndex < array.Length; i++)
        {
            array[i + arrayIndex] = LastTrades[i];
        }
    }

    public void CopyTo(IPQLastTrade[] array, int arrayIndex) => LastTrades.CopyTo(array, arrayIndex);

    void IList<IMutableLastTrade>.Insert(int index, IMutableLastTrade item) => Insert(index, (IPQLastTrade)item);

    void IList<ILastTrade>.Insert(int index, ILastTrade item)=> Insert(index, (IPQLastTrade)item);

    public void Insert(int index, IPQLastTrade item) => LastTrades.Insert(index, item);

    int IList<IMutableLastTrade>.IndexOf(IMutableLastTrade item) => IndexOf((IPQLastTrade)item);

    int IList<ILastTrade>.IndexOf(ILastTrade item) => IndexOf((IPQLastTrade)item);

    public int IndexOf(IPQLastTrade item) => LastTrades.IndexOf(item);

    bool ICollection<IMutableLastTrade>.Remove(IMutableLastTrade item) => Remove((IPQLastTrade)item);

    bool ICollection<ILastTrade>.Remove(ILastTrade item)        => Remove((IPQLastTrade)item);

    public bool Remove(IPQLastTrade toRemove) => LastTrades.Remove(toRemove);

    public void RemoveAt(int index) => LastTrades.RemoveAt(index);

    public void Clear()
    {
        foreach (var lastTrade in LastTrades)
        {
            lastTrade.ResetWithTracking();
        }
    }

    public void UpdateComplete()
    {
        if (HasUpdates) NumUpdatesSinceEmpty++;
    }

    public int AppendEntryAtEnd()
    {
        var index = Count;
        LastTrades.Add(LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags).CreateNewLastTradeEntry());
        return index;
    }

    IMutableLastTradedList ITrackableReset<IMutableLastTradedList>.ResetWithTracking() => ResetWithTracking();

    IPQLastTradedList ITrackableReset<IPQLastTradedList>.ResetWithTracking() => ResetWithTracking();

    IPQLastTradedList IPQLastTradedList.ResetWithTracking() => ResetWithTracking();

    public virtual PQLastTradedList ResetWithTracking()
    {
        foreach (var mutableLastTrade in LastTrades)
        {
            mutableLastTrade.ResetWithTracking();
        }
        return this;
    }

    public override void StateReset()
    {
        foreach (var lastTrade in LastTrades) lastTrade.StateReset();
        NameIdLookup.Clear();

        NumUpdatesSinceEmpty = 0;
        base.StateReset();
    }

    void ICollection<ILastTrade>.Add(ILastTrade item) => Add((IPQLastTrade)item);

    void ICollection<IMutableLastTrade>.Add(IMutableLastTrade item) => Add((IPQLastTrade)item);

    public void Add(IPQLastTrade newLastTrade)
    {
        if (LastTrades.Count == Count)
            LastTrades.Add(newLastTrade);
        else
            LastTrades[Count] = newLastTrade;
    }

    public virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        for (var i = 0; i < LastTrades.Count; i++)
            if (this[i] is PQLastTrade lastTrade)
                foreach (var layerFields in lastTrade.GetDeltaUpdateFields(snapShotTime, messageFlags,
                                                                           quotePublicationPrecisionSetting))
                {
                    var atDepthLayer = layerFields.AtDepth((ushort)i);
                    yield return atDepthLayer;
                }
    }

    public virtual int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        if (pqFieldUpdate.Id is >= PQFeedFields.LastTradedTickTrades and <= PQFeedFields.LastTradedAlertTrades)
        {
            var index       = pqFieldUpdate.DepthIndex();
            var pqLastTrade = this[index] as PQLastTrade;
            var result      = pqLastTrade?.UpdateField(pqFieldUpdate) ?? -1;
            return result;
        }

        return -1;
    }

    public virtual IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags)
    {
        return NameIdLookup.GetStringUpdates(snapShotTime, messageFlags);
    }

    public virtual bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        if (stringUpdate.Field.Id != PQFeedFields.LastTradedStringUpdates) return false;
        return NameIdLookup.UpdateFieldString(stringUpdate);
    }

    public override PQLastTradedList CopyFrom(ILastTradedList source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is PQLastTradedList sourceRecentlyTraded) NameIdLookup.CopyFrom(sourceRecentlyTraded.NameIdLookup, copyMergeFlags);
        var requiresUpgrade = LastTradesSupportFlags != source.LastTradesSupportFlags;
        LastTradesSupportFlags |= source.LastTradesSupportFlags;
        if (LastTrades.Count < source.Capacity)
            for (var i = Count; i < source.Capacity; i++)
            {
                var newEntry = LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags).CreateNewLastTradeEntry();
                LastTrades.Add(newEntry);
            }

        for (var i = 0; i < source.Capacity; i++)
        {
            var sourceLayer = source[i];

            IPQLastTrade destinationLayer = LastTrades[i];

            var upgradedDestinationLayer = LastTradeEntrySelector.SelectLastTradeEntry(destinationLayer, NameIdLookup, sourceLayer);
            if (i >= LastTrades.Count)
                LastTrades.Add(upgradedDestinationLayer);
            else if (!ReferenceEquals(upgradedDestinationLayer, destinationLayer)) LastTrades[i] = upgradedDestinationLayer;

            upgradedDestinationLayer.CopyFrom(sourceLayer, copyMergeFlags);
        }

        for (var i = Capacity - 1; i >= source.Capacity; i--)
            if (LastTrades[i] is IMutableLastTrade mutableLastTrade)
            {
                if (requiresUpgrade)
                {
                    var upgrade = LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags).CreateNewLastTradeEntry();
                    upgrade.CopyFrom(LastTrades[i], copyMergeFlags);
                    LastTrades[i] = upgrade;
                }
                mutableLastTrade.IsEmpty = true;
            }
        return this;
    }

    public void EnsureRelatedItemsAreConfigured(ISourceTickerInfo? referenceInstance)
    {
        LastTradesSupportFlags |= referenceInstance?.LastTradedFlags ?? LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags;
        var entriesFactory = LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags);
        var maxEntries     = PQFeedFieldsExtensions.SingleByteFieldIdMaxPossibleLastTrades;

        for (var i = 0; i < maxEntries; i++)
        {
            var currLastTrade = i < LastTrades.Count ? LastTrades[i] : null;
            if (i >= LastTrades.Count || currLastTrade is not null)
                LastTrades.Add(entriesFactory.CreateNewLastTradeEntry());
            else if (i < LastTrades.Count && currLastTrade != null
                                          && !LastTradeEntrySelector.TypeCanWhollyContain
                                                 (entriesFactory.EntryCreationType, currLastTrade.GetType()))
                LastTrades[i] = entriesFactory.UpgradeLayer(currLastTrade);
        }

        for (var i = LastTrades.Count; i < maxEntries; i++) LastTrades[i].StateReset();
    }

    object ICloneable.Clone() => Clone();

    ILastTradedList ICloneable<ILastTradedList>.Clone() => Clone();

    IPQLastTradedList IPQLastTradedList.Clone() => Clone();

    IMutableLastTradedList IMutableLastTradedList.Clone() => Clone();

    IEnumerator<IMutableLastTrade> IEnumerable<IMutableLastTrade>.GetEnumerator() => GetEnumerator();

    public bool AreEquivalent(ILastTradedList? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var countSame = Count == other.Count;
        if (!countSame) return false;
        var allLastTradesSame = true;
        for (int i = 0; i < Count && allLastTradesSame; i++)
        {
            var localLastTrade = this[i];
            var otherLastTrade = other[i];
            allLastTradesSame &= localLastTrade.AreEquivalent(otherLastTrade, exactTypes);
        }
        var allAreSame = countSame && allLastTradesSame;

        return allAreSame;
    }

    public IEnumerator<IPQLastTrade> GetEnumerator() => LastTrades.Take(Count).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<ILastTrade> IEnumerable<ILastTrade>.GetEnumerator() => GetEnumerator();

    IEnumerator<IMutableLastTrade> IMutableLastTradedList.GetEnumerator() => GetEnumerator();

    public void EnsureRelatedItemsAreConfigured(IPQNameIdLookupGenerator? otherNameIdLookupGenerator)
    {
        if (otherNameIdLookupGenerator != null)
        {
            NameIdLookup.CopyFrom(otherNameIdLookupGenerator);
            if (NameIdLookup.Count != otherNameIdLookupGenerator.Count) NameIdLookup.CopyFrom(otherNameIdLookupGenerator, CopyMergeFlags.FullReplace);
        }
    }

    public void EnsureRelatedItemsAreConfigured(ILastTradedList? referenceInstance, INameIdLookup? nameIdLookup)
    {
        NameIdLookup = nameIdLookup as IPQNameIdLookupGenerator ?? NameIdLookup;
        if (referenceInstance != null)
        {
            LastTradesSupportFlags |= referenceInstance?.LastTradesSupportFlags ?? LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags;
            var entriesFactory = LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags);
            var maxEntries     = PQFeedFieldsExtensions.SingleByteFieldIdMaxPossibleLastTrades;

            for (var i = 0; i < maxEntries; i++)
            {
                var currLastTrade = i < LastTrades.Count ? LastTrades[i] : null;
                if (i >= LastTrades.Count || currLastTrade is not null)
                    LastTrades.Add(entriesFactory.CreateNewLastTradeEntry());
                else if (i < LastTrades.Count && currLastTrade != null
                                              && !LastTradeEntrySelector.TypeCanWhollyContain
                                                     (entriesFactory.EntryCreationType, currLastTrade.GetType()))
                    LastTrades[i] = entriesFactory.UpgradeLayer(currLastTrade);
            }
        }
        else
            LastTrades.Clear();
    }

    private IPQNameIdLookupGenerator SourcePopulatedNameIdGeneratorFrom(IEnumerable<ILastTrade?> existing)
    {
        if (existing is IPQLastTradedList pqLastTradedList)
            return new PQNameIdLookupGenerator(pqLastTradedList.NameIdLookup, PQFeedFields.LastTradedStringUpdates);
        return existing.OfType<ISupportsPQNameIdLookupGenerator>().Any()
            ? new PQNameIdLookupGenerator(existing.OfType<ISupportsPQNameIdLookupGenerator>().Select(snilg => snilg.NameIdLookup).First())
            : new PQNameIdLookupGenerator(PQFeedFields.LastTradedStringUpdates);
    }

    public override PQLastTradedList Clone() =>
        Recycler?.Borrow<PQLastTradedList>().CopyFrom(this) ??
        new PQLastTradedList((ILastTradedList)this);

    protected string PQLastTradedListToStringMembers => $"LastTrades: [{string.Join(", ", LastTrades.Take(Count))}], {nameof(Count)}: {Count}";

    public override string ToString() => $"{nameof(PQLastTradedList)}{{{PQLastTradedListToStringMembers}}}";

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ILastTradedList, true);

    public override int GetHashCode() => LastTrades.GetHashCode();
}
