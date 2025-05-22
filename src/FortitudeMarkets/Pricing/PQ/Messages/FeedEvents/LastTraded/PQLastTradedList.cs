// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

using System.Collections;
using FortitudeCommon.DataStructures.Collections;
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
  , ISupportsPQNameIdLookupGenerator, ITrackableReset<IPQLastTradedList>, IList<IPQLastTrade>
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
    private readonly List<IPQLastTrade> lastTrades;

    private IPQNameIdLookupGenerator nameIdLookupGenerator;

    protected uint NumUpdates = uint.MaxValue;

    public PQLastTradedList()
    {
        lastTrades             = [];
        nameIdLookupGenerator  = new PQNameIdLookupGenerator(PQFeedFields.LastTradedStringUpdates);
        LastTradeEntrySelector = new PQLastTradeEntrySelector(nameIdLookupGenerator);

        if (GetType() == typeof(PQLastTradedList)) NumUpdates = 0;
    }

    public PQLastTradedList(ISourceTickerInfo sourceTickerInfo)
    {
        LastTradesSupportFlags = sourceTickerInfo.LastTradedFlags;
        nameIdLookupGenerator  = new PQNameIdLookupGenerator(PQFeedFields.LastTradedStringUpdates);
        LastTradeEntrySelector = new PQLastTradeEntrySelector(nameIdLookupGenerator);

        lastTrades = new List<IPQLastTrade>();
        EnsureRelatedItemsAreConfigured(sourceTickerInfo);

        if (GetType() == typeof(PQLastTradedList)) NumUpdates = 0;
    }

    public PQLastTradedList(IEnumerable<IPQLastTrade> lastTrades) : this(lastTrades.ToList())
    {
        LastTradesSupportFlags = lastTrades.FirstOrDefault()?.SupportsLastTradedFlags ?? LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags;
        nameIdLookupGenerator  = SourcePopulatedNameIdGeneratorFrom(lastTrades);
        LastTradeEntrySelector = new PQLastTradeEntrySelector(nameIdLookupGenerator);

        this.lastTrades = lastTrades.Select(lt => LastTradeEntrySelector.ConvertToExpectedImplementation(lt, NameIdLookup, true)).ToList();
        if (GetType() == typeof(PQLastTradedList)) NumUpdates = 0;
    }

    public PQLastTradedList(List<IPQLastTrade> lastTrades)
    {
        LastTradesSupportFlags = lastTrades.FirstOrDefault()?.SupportsLastTradedFlags  ?? LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags;
        nameIdLookupGenerator  = SourcePopulatedNameIdGeneratorFrom(lastTrades);
        LastTradeEntrySelector = new PQLastTradeEntrySelector(nameIdLookupGenerator);
        this.lastTrades        = lastTrades;
        foreach (var pqLastTrade in lastTrades)
        {
            if (pqLastTrade is ISupportsPQNameIdLookupGenerator setNameGen)
            {
                setNameGen.NameIdLookup = nameIdLookupGenerator;
            }
        }

        if (GetType() == typeof(PQLastTradedList)) NumUpdates = 0;
    }

    public PQLastTradedList(ILastTradedList toClone)
    {
        LastTradesSupportFlags = toClone.LastTradesSupportFlags;
        nameIdLookupGenerator  = SourcePopulatedNameIdGeneratorFrom(toClone);
        LastTradeEntrySelector = new PQLastTradeEntrySelector(nameIdLookupGenerator);

        lastTrades = toClone.Select(lt => LastTradeEntrySelector.ConvertToExpectedImplementation(lt, NameIdLookup, true)).ToList();
        EnsureRelatedItemsAreConfigured(toClone, NameIdLookup);

        if (GetType() == typeof(PQLastTradedList)) NumUpdates = 0;
    }

    public IPQLastTradeTypeSelector LastTradeEntrySelector { get; set; }

    public LastTradeType LastTradesOfType => LastTradesSupportFlags.MostCompactLayerType();

    public LastTradedFlags LastTradesSupportFlags { get; private set; } = LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags;

    public IPQNameIdLookupGenerator NameIdLookup
    {
        get => nameIdLookupGenerator;
        set
        {
            nameIdLookupGenerator  = value;
            LastTradeEntrySelector = new PQLastTradeEntrySelector(nameIdLookupGenerator);
            foreach (var pqLastTrade in lastTrades.OfType<ISupportsPQNameIdLookupGenerator>()) pqLastTrade.NameIdLookup = value;
        }
    }

    public IPQLastTrade this[int i]
    {
        get
        {
            if (i > (ushort)PQDepthKey.DepthMask)
                throw new ArgumentException($"The number of last trades can not be greater than {(ushort)PQDepthKey.DepthMask}");
            while (i >= lastTrades.Count)
            {
                lastTrades.Add(LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags).CreateNewLastTradeEntry());
            }
            return lastTrades[i];
        }
        set
        {
            while (lastTrades.Count <= i)
                lastTrades.Add(LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags).CreateNewLastTradeEntry());
            lastTrades[i] = value;
            if (value is ISupportsPQNameIdLookupGenerator lastTradeWithNameId) lastTradeWithNameId.NameIdLookup = NameIdLookup;
        }
    }

    ILastTrade IReadOnlyList<ILastTrade>.this[int i] => this[i];

    IMutableLastTrade IMutableLastTradedList.this[int i]
    {
        get => this[i];
        set => this[i] = (IPQLastTrade)value;
    }

    public int Capacity
    {
        get => lastTrades.Count;
        set
        {
            if (value > PQQuoteFieldsExtensions.SingleByteFieldIdMaxPossibleLastTrades)
                throw new ArgumentException("Expected PQRecentlyTraded Capacity to be less than or equal to " +
                                            PQQuoteFieldsExtensions.SingleByteFieldIdMaxPossibleLastTrades);
            while (lastTrades.Count < value)
            {
                var firstLastTrade = lastTrades[0].Clone();
                firstLastTrade.StateReset();
                lastTrades.Add(firstLastTrade);
            }
        }
    }

    public int Count
    {
        get
        {
            for (var i = lastTrades.Count - 1; i >= 0; i--)
            {
                var layerAtLevel = lastTrades[i];
                if (!layerAtLevel.IsEmpty) return i + 1;
            }

            return 0;
        }
        set
        {
            for (var i = lastTrades.Count - 1; i >= value; i--)
            {
                var layerAtLevel                                             = lastTrades[i];
                if (layerAtLevel is { IsEmpty: false }) layerAtLevel.IsEmpty = true;
            }
        }
    }

    public bool HasUpdates
    {
        get { return lastTrades.Any(pqLt => pqLt.HasUpdates); }
        set
        {
            foreach (var pqLastTrade in lastTrades) pqLastTrade.HasUpdates = value;
        }
    }

    public bool HasLastTrades => Count > 0;

    INameIdLookup IHasNameIdLookup.NameIdLookup => NameIdLookup;

    public uint UpdateCount => NumUpdates;

    public bool IsReadOnly => false;

    IMutableLastTrade IList<IMutableLastTrade>.this[int index]
    {
        get => this[index];
        set => this[index] = (IPQLastTrade)value;
    }

    protected Func<IPQLastTrade> NewElementFactory =>
        () => LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags).CreateNewLastTradeEntry();

    bool ICollection<IMutableLastTrade>.Contains(IMutableLastTrade item) => Contains((IPQLastTrade)item);

    public bool Contains(IPQLastTrade item) => lastTrades.Contains(item);

    void ICollection<IMutableLastTrade>.CopyTo(IMutableLastTrade[] array, int arrayIndex)
    {
        for (int i = arrayIndex; i < lastTrades.Count; i++)
        {
            array[i] = lastTrades[i];
        }
    }

    public void CopyTo(IPQLastTrade[] array, int arrayIndex) => lastTrades.CopyTo(array, arrayIndex);

    void IList<IMutableLastTrade>.Insert(int index, IMutableLastTrade item) => Insert(index, (IPQLastTrade)item);

    public void Insert(int index, IPQLastTrade item) => lastTrades.Insert(index, item);

    int IList<IMutableLastTrade>.IndexOf(IMutableLastTrade item) => IndexOf((IPQLastTrade)item);

    public int IndexOf(IPQLastTrade item) => lastTrades.IndexOf(item);

    bool ICollection<IMutableLastTrade>.Remove(IMutableLastTrade item) => Remove((IPQLastTrade)item);

    public bool Remove(IPQLastTrade toRemove) => lastTrades.Remove(toRemove);

    public void RemoveAt(int index) => lastTrades.RemoveAt(index);

    public void Clear()
    {
        foreach (var lastTrade in lastTrades)
        {
            lastTrade.ResetWithTracking();
        }
    }

    public void UpdateComplete()
    {
        if (HasUpdates) NumUpdates++;
    }

    public int AppendEntryAtEnd()
    {
        var index = Count;
        lastTrades.Add(LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags).CreateNewLastTradeEntry());
        return index;
    }

    IMutableLastTradedList ITrackableReset<IMutableLastTradedList>.ResetWithTracking() => ResetWithTracking();

    IPQLastTradedList ITrackableReset<IPQLastTradedList>.ResetWithTracking() => ResetWithTracking();

    IPQLastTradedList IPQLastTradedList.ResetWithTracking() => ResetWithTracking();

    public virtual PQLastTradedList ResetWithTracking()
    {
        foreach (var mutableLastTrade in lastTrades)
        {
            mutableLastTrade.ResetWithTracking();
        }
        return this;
    }

    public override void StateReset()
    {
        foreach (var lastTrade in lastTrades) lastTrade.StateReset();
        NameIdLookup.Clear();

        NumUpdates = 0;
        base.StateReset();
    }

    void ICollection<IMutableLastTrade>.Add(IMutableLastTrade item) => Add((IPQLastTrade)item);

    public void Add(IPQLastTrade newLastTrade)
    {
        if (lastTrades.Count == Count)
            lastTrades.Add(newLastTrade);
        else
            lastTrades[Count] = newLastTrade;
    }

    public virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        for (var i = 0; i < lastTrades.Count; i++)
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
        if (pqFieldUpdate.Id is PQFeedFields.LastTradedTickTrades)
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
        LastTradesSupportFlags |= source.LastTradesSupportFlags;
        if (lastTrades.Count < source.Capacity)
            for (var i = Count; i < source.Capacity; i++)
            {
                var newEntry = LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags).CreateNewLastTradeEntry();
                lastTrades.Add(newEntry);
            }

        for (var i = 0; i < source.Capacity; i++)
        {
            var sourceLayer = source[i];
            if (i < lastTrades.Count && sourceLayer.IsEmpty)
            {
                lastTrades[i].StateReset();
                continue;
            }


            IPQLastTrade destinationLayer = lastTrades[i];

            var upgradedDestinationLayer = LastTradeEntrySelector.SelectLastTradeEntry(destinationLayer, NameIdLookup, sourceLayer);
            if (i >= lastTrades.Count)
                lastTrades.Add(upgradedDestinationLayer);
            else if (!ReferenceEquals(upgradedDestinationLayer, destinationLayer)) lastTrades[i] = upgradedDestinationLayer;

            upgradedDestinationLayer.CopyFrom(sourceLayer, copyMergeFlags);
        }

        for (var i = source.Capacity; i < lastTrades.Count; i++)
            if (lastTrades[i] is IMutableLastTrade mutableLastTrade)
                mutableLastTrade.IsEmpty = true;
        return this;
    }

    public void EnsureRelatedItemsAreConfigured(ISourceTickerInfo? referenceInstance)
    {
        NameIdLookup = new PQNameIdLookupGenerator(PQFeedFields.LastTradedStringUpdates);
        var entriesFactory = LastTradeEntrySelector.FindForLastTradeFlags(referenceInstance?.LastTradedFlags ?? LastTradedFlags.LastTradedPrice);
        var maxEntries     = PQQuoteFieldsExtensions.SingleByteFieldIdMaxPossibleLastTrades;

        for (var i = 0; i < maxEntries; i++)
        {
            var currLastTrade = i < lastTrades.Count ? lastTrades[i] : null;
            if (i >= lastTrades.Count || currLastTrade is not null)
                lastTrades.Add(entriesFactory.CreateNewLastTradeEntry());
            else if (i < lastTrades.Count && currLastTrade != null
                                          && !LastTradeEntrySelector.TypeCanWhollyContain
                                                 (entriesFactory.EntryCreationType, currLastTrade.GetType()))
                lastTrades[i] = entriesFactory.UpgradeLayer(currLastTrade);
        }

        for (var i = lastTrades.Count; i < maxEntries; i++) lastTrades[i].StateReset();
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
        var lastTradesSame = exactTypes
            ? lastTrades.Take(Count).SequenceEqual(other.Take(Count))
            : lastTrades.Take(Count).Zip(other.Take(Count), (thisLastTrade, otherLastTrade) => new { thisLastTrade, otherLastTrade })
                        .All(joined => joined.thisLastTrade.AreEquivalent(joined.otherLastTrade, exactTypes));
        return lastTradesSame;
    }

    public IEnumerator<IPQLastTrade> GetEnumerator() => lastTrades.Take(Count).GetEnumerator();

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
        NameIdLookup = nameIdLookup as IPQNameIdLookupGenerator ?? new PQNameIdLookupGenerator(PQFeedFields.LastTradedStringUpdates);
        if (referenceInstance != null)
            CopyFrom(referenceInstance, CopyMergeFlags.FullReplace);
        else
            lastTrades.Clear();
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

    protected string PQLastTradedListToStringMembers => $"LastTrades: [{string.Join(", ", lastTrades.Take(Count))}], {nameof(Count)}: {Count}";

    public override string ToString() => $"{nameof(PQLastTradedList)}{{{PQLastTradedListToStringMembers}}}";

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ILastTradedList, true);

    public override int GetHashCode() => lastTrades.GetHashCode();
}
