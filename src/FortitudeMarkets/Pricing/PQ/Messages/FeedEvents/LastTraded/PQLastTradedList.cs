// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

using System.Collections;
using System.Text;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded.LastTradeEntrySelector;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;

public interface IPQLastTradedList : IMutableLastTradedList, IPQSupportsNumberPrecisionFieldUpdates, IPQSupportsStringUpdates, 
    IRelatedItems<IPQNameIdLookupGenerator>, IRelatedItems<ISourceTickerInfo>, ISupportsPQNameIdLookupGenerator, 
    ITrackableReset<IPQLastTradedList>, IMutableCapacityList<IPQLastTrade>
{
    new IPQLastTrade this[int index] { get; set; }

    new IReadOnlyList<IPQLastTrade> LastTrades { get; }

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
    protected readonly List<IPQLastTrade> TradesList;

    private IPQNameIdLookupGenerator nameIdLookupGenerator;

    protected uint SequenceId = uint.MaxValue;

    public PQLastTradedList()
    {
        TradesList             = [];
        nameIdLookupGenerator  = new PQNameIdLookupGenerator(PQFeedFields.LastTradedStringUpdates);
        LastTradeEntrySelector = new PQLastTradeEntrySelector(nameIdLookupGenerator);

        if (GetType() == typeof(PQLastTradedList)) SequenceId = 0;
    }

    public PQLastTradedList(IPQNameIdLookupGenerator nameIdLookup)
    {
        TradesList             = [];
        nameIdLookupGenerator  = nameIdLookup;
        LastTradeEntrySelector = new PQLastTradeEntrySelector(nameIdLookupGenerator);

        if (GetType() == typeof(PQLastTradedList)) SequenceId = 0;
    }

    public PQLastTradedList(ISourceTickerInfo sourceTickerInfo)
    {
        LastTradesSupportFlags = sourceTickerInfo.LastTradedFlags;
        nameIdLookupGenerator  = new PQNameIdLookupGenerator(PQFeedFields.LastTradedStringUpdates);
        LastTradeEntrySelector = new PQLastTradeEntrySelector(nameIdLookupGenerator);

        TradesList = new List<IPQLastTrade>();
        EnsureRelatedItemsAreConfigured(sourceTickerInfo);

        if (GetType() == typeof(PQLastTradedList)) SequenceId = 0;
    }

    public PQLastTradedList(ISourceTickerInfo sourceTickerInfo, IPQNameIdLookupGenerator nameIdLookup)
    {
        LastTradesSupportFlags = sourceTickerInfo.LastTradedFlags;
        nameIdLookupGenerator  = nameIdLookup;
        LastTradeEntrySelector = new PQLastTradeEntrySelector(nameIdLookupGenerator);

        TradesList = new List<IPQLastTrade>();
        EnsureRelatedItemsAreConfigured(sourceTickerInfo);

        if (GetType() == typeof(PQLastTradedList)) SequenceId = 0;
    }

    public PQLastTradedList(IEnumerable<IPQLastTrade> lastTrades) : this(lastTrades.ToList())
    {
        LastTradesSupportFlags = lastTrades.FirstOrDefault()?.SupportsLastTradedFlags ?? LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags;
        nameIdLookupGenerator  = SourcePopulatedNameIdGeneratorFrom(lastTrades);
        LastTradeEntrySelector = new PQLastTradeEntrySelector(nameIdLookupGenerator);

        this.TradesList = lastTrades.Select(lt => LastTradeEntrySelector.ConvertToExpectedImplementation(lt, NameIdLookup, true)).ToList();
        if (GetType() == typeof(PQLastTradedList)) SequenceId = 0;
    }

    public PQLastTradedList(List<IPQLastTrade> lastTrades)
    {
        LastTradesSupportFlags = lastTrades.FirstOrDefault()?.SupportsLastTradedFlags ?? LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags;
        nameIdLookupGenerator  = SourcePopulatedNameIdGeneratorFrom(lastTrades);
        LastTradeEntrySelector = new PQLastTradeEntrySelector(nameIdLookupGenerator);
        this.TradesList        = lastTrades;
        foreach (var pqLastTrade in lastTrades)
        {
            if (pqLastTrade is ISupportsPQNameIdLookupGenerator setNameGen)
            {
                setNameGen.NameIdLookup = nameIdLookupGenerator;
            }
        }

        if (GetType() == typeof(PQLastTradedList)) SequenceId = 0;
    }

    public PQLastTradedList(ILastTradedList toClone, IPQNameIdLookupGenerator? nameIdLookup = null)
    {
        LastTradesSupportFlags = toClone.LastTradesSupportFlags;
        nameIdLookupGenerator  = nameIdLookup ?? SourcePopulatedNameIdGeneratorFrom(toClone);
        LastTradeEntrySelector = new PQLastTradeEntrySelector(nameIdLookupGenerator);

        TradesList = toClone.Select(lt => LastTradeEntrySelector.ConvertToExpectedImplementation(lt, NameIdLookup, true)).ToList();
        EnsureRelatedItemsAreConfigured(toClone, NameIdLookup);

        if (GetType() == typeof(PQLastTradedList)) SequenceId = 0;
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
            foreach (var pqLastTrade in TradesList.OfType<ISupportsPQNameIdLookupGenerator>()) pqLastTrade.NameIdLookup = value;
        }
    }

    public virtual ushort MaxAllowedSize { get; set; }

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

    IMutableLastTrade IList<IMutableLastTrade>.this[int index]
    {
        get => this[index];
        set => this[index] = (IPQLastTrade)value;
    }

    public IPQLastTrade this[int i]
    {
        get
        {
            if (i > (ushort)PQDepthKey.DepthMask)
                throw new ArgumentException($"The number of last trades can not be greater than {(ushort)PQDepthKey.DepthMask}");
            while (i >= TradesList.Count)
            {
                TradesList.Add(LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags).CreateNewLastTradeEntry());
            }
            return TradesList[i];
        }
        set
        {
            while (TradesList.Count <= i)
                TradesList.Add(LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags).CreateNewLastTradeEntry());
            TradesList[i] = value;
            if (value is ISupportsPQNameIdLookupGenerator lastTradeWithNameId) lastTradeWithNameId.NameIdLookup = NameIdLookup;
        }
    }

    public int Capacity
    {
        get => TradesList.Count;
        set
        {
            if (value > PQFeedFieldsExtensions.SingleByteFieldIdMaxPossibleLastTrades)
                throw new ArgumentException("Expected PQRecentlyTraded Capacity to be less than or equal to " +
                                            PQFeedFieldsExtensions.SingleByteFieldIdMaxPossibleLastTrades);
            while (TradesList.Count < value)
            {
                var firstLastTrade = TradesList[0].Clone();
                firstLastTrade.StateReset();
                TradesList.Add(firstLastTrade);
            }
        }
    }

    public int Count
    {
        get
        {
            for (var i = TradesList.Count - 1; i >= 0; i--)
            {
                var layerAtLevel = TradesList[i];
                if (!layerAtLevel.IsEmpty) return i + 1;
            }

            return 0;
        }
        set
        {
            for (var i = TradesList.Count - 1; i >= value; i--)
            {
                var layerAtLevel                                             = TradesList[i];
                if (layerAtLevel is { IsEmpty: false }) layerAtLevel.IsEmpty = true;
            }
        }
    }

    IReadOnlyList<ILastTrade> ILastTradedList.LastTrades => LastTrades;

    public IReadOnlyList<IPQLastTrade> LastTrades => TradesList.Take(Count).ToList().AsReadOnly();

    public uint UpdateSequenceId => SequenceId;

    public bool IsReadOnly => false;

    public bool HasUpdates
    {
        get { return TradesList.Any(pqLt => pqLt.HasUpdates); }
        set
        {
            foreach (var pqLastTrade in TradesList) pqLastTrade.HasUpdates = value;
        }
    }

    public virtual bool IsEmpty
    {
        get => TradesList.All(lt => lt.IsEmpty); // do not include ElementShifts in IsEmpty
        set
        {
            foreach (var lastTrade in TradesList)
            {
                lastTrade.IsEmpty = value;
            }

            if (!value) return;
            SequenceId = 0;
        }
    }

    IMutableLastTradedList ITrackableReset<IMutableLastTradedList>.ResetWithTracking() => ResetWithTracking();

    IPQLastTradedList ITrackableReset<IPQLastTradedList>.ResetWithTracking() => ResetWithTracking();

    IPQLastTradedList IPQLastTradedList.ResetWithTracking() => ResetWithTracking();

    ITracksResetCappedCapacityList<IMutableLastTrade> ITrackableReset<ITracksResetCappedCapacityList<IMutableLastTrade>>.ResetWithTracking() =>
        ResetWithTracking();

    IMutableLastTradedList IMutableLastTradedList.ResetWithTracking() => ResetWithTracking();

    public virtual PQLastTradedList ResetWithTracking()
    {
        foreach (var mutableLastTrade in TradesList)
        {
            mutableLastTrade.ResetWithTracking();
        }
        return this;
    }

    public override void StateReset()
    {
        foreach (var lastTrade in TradesList) lastTrade.StateReset();
        NameIdLookup.Clear();

        SequenceId = 0;
        base.StateReset();
    }

    protected Func<IPQLastTrade> NewElementFactory =>
        () => LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags).CreateNewLastTradeEntry();

    bool ICollection<IMutableLastTrade>.Contains(IMutableLastTrade item) => Contains((IPQLastTrade)item);

    public bool Contains(IPQLastTrade item) => TradesList.Contains(item);

    void ICollection<IMutableLastTrade>.CopyTo(IMutableLastTrade[] array, int arrayIndex)
    {
        for (int i = 0; i < TradesList.Count && i + arrayIndex < array.Length; i++)
        {
            array[i + arrayIndex] = TradesList[i];
        }
    }

    public void CopyTo(IPQLastTrade[] array, int arrayIndex) => TradesList.CopyTo(array, arrayIndex);

    void IList<IMutableLastTrade>.Insert(int index, IMutableLastTrade item) => Insert(index, (IPQLastTrade)item);

    public void Insert(int index, IPQLastTrade item) => TradesList.Insert(index, item);

    int IList<IMutableLastTrade>.IndexOf(IMutableLastTrade item) => IndexOf((IPQLastTrade)item);

    public int IndexOf(IPQLastTrade item) => TradesList.IndexOf(item);

    bool ICollection<IMutableLastTrade>.Remove(IMutableLastTrade item) => Remove((IPQLastTrade)item);

    public bool Remove(IPQLastTrade toRemove) => TradesList.Remove(toRemove);

    public void RemoveAt(int index) => TradesList.RemoveAt(index);

    public void Clear()
    {
        foreach (var lastTrade in TradesList)
        {
            lastTrade.ResetWithTracking();
        }
    }

    public void UpdateStarted(uint updateSequenceId)
    {
        SequenceId = updateSequenceId;
    }

    public void UpdateComplete(uint updateSequenceId = 0)
    {
        if (HasUpdates) SequenceId++;
    }

    public int AppendEntryAtEnd()
    {
        var index = Count;
        TradesList.Add(LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags).CreateNewLastTradeEntry());
        return index;
    }

    void ICollection<IMutableLastTrade>.Add(IMutableLastTrade item) => Add((IPQLastTrade)item);

    public void Add(IPQLastTrade newLastTrade)
    {
        var nonEmptyCount = Count;
        if (TradesList.Count == nonEmptyCount)
            TradesList.Add(newLastTrade);
        else
            TradesList[nonEmptyCount] = newLastTrade;
    }

    public virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, PQMessageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        for (var i = 0; i < TradesList.Count; i++)
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

    public virtual IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, PQMessageFlags messageFlags)
    {
        return NameIdLookup.GetStringUpdates(snapShotTime, messageFlags);
    }

    public virtual bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        if (stringUpdate.Field.Id != PQFeedFields.LastTradedStringUpdates) return false;
        return NameIdLookup.UpdateFieldString(stringUpdate);
    }

    public void EnsureRelatedItemsAreConfigured(ISourceTickerInfo? referenceInstance)
    {
        LastTradesSupportFlags |= referenceInstance?.LastTradedFlags ?? LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags;
        var entriesFactory = LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags);
        var maxEntries     = PQFeedFieldsExtensions.SingleByteFieldIdMaxPossibleLastTrades;

        for (var i = 0; i < maxEntries; i++)
        {
            var currLastTrade = i < TradesList.Count ? TradesList[i] : null;
            if (i >= TradesList.Count || currLastTrade is not null)
                TradesList.Add(entriesFactory.CreateNewLastTradeEntry());
            else if (i < TradesList.Count && currLastTrade != null
                                          && !LastTradeEntrySelector.TypeCanWhollyContain
                                                 (entriesFactory.EntryCreationType, currLastTrade.GetType()))
                TradesList[i] = entriesFactory.UpgradeLayer(currLastTrade);
        }

        for (var i = TradesList.Count; i < maxEntries; i++) TradesList[i].StateReset();
    }

    IEnumerator<IMutableLastTrade> IEnumerable<IMutableLastTrade>.GetEnumerator() => GetEnumerator();

    public IEnumerator<IPQLastTrade> GetEnumerator() => TradesList.Take(Count).GetEnumerator();

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
            LastTradesSupportFlags |= referenceInstance.LastTradesSupportFlags;
            var entriesFactory = LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags);
            var maxEntries     = PQFeedFieldsExtensions.SingleByteFieldIdMaxPossibleLastTrades;

            for (var i = 0; i < maxEntries; i++)
            {
                var currLastTrade = i < TradesList.Count ? TradesList[i] : null;
                if (i >= TradesList.Count || currLastTrade is not null)
                    TradesList.Add(entriesFactory.CreateNewLastTradeEntry());
                else if (i < TradesList.Count && currLastTrade != null
                                              && !LastTradeEntrySelector.TypeCanWhollyContain
                                                     (entriesFactory.EntryCreationType, currLastTrade.GetType()))
                    TradesList[i] = entriesFactory.UpgradeLayer(currLastTrade);
            }
        }
        else
            TradesList.Clear();
    }

    private IPQNameIdLookupGenerator SourcePopulatedNameIdGeneratorFrom(IEnumerable<ILastTrade?> existing)
    {
        if (existing is IPQLastTradedList pqLastTradedList)
            return new PQNameIdLookupGenerator(pqLastTradedList.NameIdLookup, PQFeedFields.LastTradedStringUpdates);
        return existing.OfType<ISupportsPQNameIdLookupGenerator>().Any()
            ? new PQNameIdLookupGenerator(existing.OfType<ISupportsPQNameIdLookupGenerator>().Select(spqnilg => spqnilg.NameIdLookup).First())
            : new PQNameIdLookupGenerator(PQFeedFields.LastTradedStringUpdates);
    }

    object ICloneable.Clone() => Clone();

    ILastTradedList ICloneable<ILastTradedList>.Clone() => Clone();

    IPQLastTradedList IPQLastTradedList.Clone() => Clone();

    IMutableLastTradedList IMutableLastTradedList.Clone() => Clone();

    public override PQLastTradedList Clone() =>
        Recycler?.Borrow<PQLastTradedList>().CopyFrom(this) ?? new PQLastTradedList((ILastTradedList)this);

    public override PQLastTradedList CopyFrom(ILastTradedList source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is PQLastTradedList sourceRecentlyTraded) NameIdLookup.CopyFrom(sourceRecentlyTraded.NameIdLookup, copyMergeFlags);
        var requiresUpgrade = LastTradesSupportFlags != source.LastTradesSupportFlags;
        LastTradesSupportFlags |= source.LastTradesSupportFlags;
        if (TradesList.Count < source.Capacity)
            for (var i = Count; i < source.Capacity; i++)
            {
                var newEntry = LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags).CreateNewLastTradeEntry();
                TradesList.Add(newEntry);
            }

        for (var i = 0; i < source.Capacity; i++)
        {
            var sourceLayer = source[i];

            IPQLastTrade destinationLayer = TradesList[i];

            var upgradedDestinationLayer = LastTradeEntrySelector.SelectLastTradeEntry(destinationLayer, NameIdLookup, sourceLayer);
            if (i >= TradesList.Count)
                TradesList.Add(upgradedDestinationLayer);
            else if (!ReferenceEquals(upgradedDestinationLayer, destinationLayer)) TradesList[i] = upgradedDestinationLayer;

            upgradedDestinationLayer.CopyFrom(sourceLayer, copyMergeFlags);
        }

        for (var i = Capacity - 1; i >= source.Capacity; i--)
            if (TradesList[i] is IMutableLastTrade mutableLastTrade)
            {
                if (requiresUpgrade)
                {
                    var upgrade = LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags).CreateNewLastTradeEntry();
                    upgrade.CopyFrom(TradesList[i], copyMergeFlags);
                    TradesList[i] = upgrade;
                }
                mutableLastTrade.IsEmpty = true;
            }
        return this;
    }

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

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ILastTradedList, true);
    
    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        foreach (var lastTrade in TradesList)
        {
            hashCode.Add(lastTrade);
        }
        return hashCode.ToHashCode();
    }

    public string EachLastTradeByIndexOnNewLines()
    {
        var count = Count;
        var sb    = new StringBuilder(100 * count);
        for (var i = 0; i < count; i++)
        {
            var lastTrade = TradesList[i];
            sb.Append("\t\tLastTrades[").Append(i).Append("] = ").Append(lastTrade);
            if (i < count - 1)
            {
                sb.AppendLine(",");
            }
        }
        return sb.ToString();
    }
    

    protected string PQNonLastTradedListToStringMembers => $"{nameof(Count)}: {Count}, {nameof(MaxAllowedSize)}: {MaxAllowedSize:N0}";
    protected string PQLastTradedListToString           => $"{nameof(LastTrades)}: [\n{EachLastTradeByIndexOnNewLines()}]";

    public override string ToString() => $"{nameof(PQLastTradedList)}{{{PQNonLastTradedListToStringMembers}, {PQLastTradedListToString}}}";
}
