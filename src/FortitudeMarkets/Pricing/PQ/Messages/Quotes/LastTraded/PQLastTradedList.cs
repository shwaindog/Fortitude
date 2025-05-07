// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

using System.Collections;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LastTraded.LastTradeEntrySelector;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.Quotes.LastTraded;

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.LastTraded;

public interface IPQLastTradedList : IMutableLastTradedList,
    IPQSupportsFieldUpdates<ILastTradedList>, IPQSupportsStringUpdates<ILastTradedList>,
    IEnumerable<IPQLastTrade>, IRelatedItems<IPQNameIdLookupGenerator>, IRelatedItems<IPQSourceTickerInfo>
  , ISupportsPQNameIdLookupGenerator
{
    new IPQLastTrade? this[int index] { get; set; }
    new IPQLastTradedList         Clone();
    new IEnumerator<IPQLastTrade> GetEnumerator();
}

public class PQLastTradedList : ReusableObject<ILastTradedList>, IPQLastTradedList
{
    private readonly IList<IPQLastTrade?> lastTrades;

    private IPQNameIdLookupGenerator nameIdLookupGenerator = null!;

    protected uint NumUpdates = uint.MaxValue;

    public PQLastTradedList()
    {
        lastTrades   = new List<IPQLastTrade?>();
        NameIdLookup = new PQNameIdLookupGenerator(PQQuoteFields.LastTradedDictionaryUpsertCommand);

        if (GetType() == typeof(PQLastTradedList)) NumUpdates = 0;
    }

    public PQLastTradedList(IPQSourceTickerInfo sourceTickerInfo)
    {
        LastTradesSupportFlags = sourceTickerInfo.LastTradedFlags;
        LastTradesOfType       = LastTradesSupportFlags.MostCompactLayerType();
        lastTrades             = new List<IPQLastTrade?>();
        NameIdLookup           = new PQNameIdLookupGenerator(PQQuoteFields.LastTradedDictionaryUpsertCommand);
        EnsureRelatedItemsAreConfigured(sourceTickerInfo);

        if (GetType() == typeof(PQLastTradedList)) NumUpdates = 0;
    }

    public PQLastTradedList(IEnumerable<IPQLastTrade?> lastTrades) : this(lastTrades.ToList())
    {
        if (GetType() == typeof(PQLastTradedList)) NumUpdates = 0;
    }

    public PQLastTradedList(IList<IPQLastTrade?> lastTrades)
    {
        this.lastTrades = new List<IPQLastTrade?>();
        NameIdLookup    = SourcePopulatedNameIdGeneratorFrom(lastTrades);
        this.lastTrades = lastTrades.Select(lt => LastTradeEntrySelector.SelectLastTradeEntry(null, NameIdLookup, lt, true)).ToList();

        if (GetType() == typeof(PQLastTradedList)) NumUpdates = 0;
    }

    public PQLastTradedList(IRecentlyTraded toClone)
    {
        lastTrades   = new List<IPQLastTrade?>(); // so NameIdLookup set doesn't blow up;
        NameIdLookup = SourcePopulatedNameIdGeneratorFrom(toClone);
        lastTrades   = toClone.Select(lt => LastTradeEntrySelector.SelectLastTradeEntry(null, NameIdLookup, lt, true)).ToList();
        EnsureRelatedItemsAreConfigured(toClone, NameIdLookup);

        if (GetType() == typeof(PQLastTradedList)) NumUpdates = 0;
    }

    public IPQLastTradeTypeSelector LastTradeEntrySelector { get; set; } = null!;

    public LastTradeType LastTradesOfType { get; } = LastTradeType.Price;

    public LastTradedFlags LastTradesSupportFlags { get; } = LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime;

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

    public IPQLastTrade? this[int i]
    {
        get 
        {
            while (i >= lastTrades.Count && i < PQQuoteFieldsExtensions.SingleByteFieldIdMaxPossibleLastTrades)
            {
                var factory = LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags);
                lastTrades.Add(factory!.CreateNewLastTradeEntry());
            }
            return lastTrades[i];
        }
        set
        {
            lastTrades[i] = value;
            if (value is ISupportsPQNameIdLookupGenerator lastTradeWithNameId) lastTradeWithNameId.NameIdLookup = NameIdLookup;
        }
    }

    ILastTrade? ILastTradedList.this[int i] => this[i];

    IMutableLastTrade? IMutableLastTradedList.this[int i]
    {
        get => this[i];
        set => this[i] = value as IPQLastTrade;
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
                var firstLastTrade = lastTrades[0]?.Clone();
                firstLastTrade?.StateReset();
                if (firstLastTrade != null) lastTrades.Add(firstLastTrade);
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
                if (!(layerAtLevel?.IsEmpty ?? true)) return i + 1;
            }

            return 0;
        }
    }

    public bool HasUpdates
    {
        get { return lastTrades.Any(pqpvl => pqpvl?.HasUpdates ?? false); }
        set
        {
            foreach (var pqLastTrade in lastTrades)
                if (pqLastTrade != null)
                    pqLastTrade.HasUpdates = value;
        }
    }

    public bool HasLastTrades => Count > 0;

    INameIdLookup IHasNameIdLookup.NameIdLookup => NameIdLookup;

    public uint UpdateCount => throw new NotImplementedException();

    public void UpdateComplete()
    {
        if (HasUpdates) NumUpdates++;
    }

    public int AppendEntryAtEnd()
    {
        var index = Count;
        lastTrades.Add(LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags)!.CreateNewLastTradeEntry());
        return index;
    }

    public override void StateReset()
    {
        foreach (var lastTrade in lastTrades) lastTrade?.StateReset();
        NameIdLookup.Clear();

        NumUpdates = 0;
        base.StateReset();
    }

    public void Add(IMutableLastTrade newLastTrade)
    {
        if (lastTrades.Count == Count)
            lastTrades.Add((IPQLastTrade)newLastTrade);
        else
            lastTrades[Count] = (IPQLastTrade)newLastTrade;
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
        if (pqFieldUpdate.Id is PQQuoteFields.LastTradedTickTrades)
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
        for (var i = 0; i < lastTrades.Count; i++)
            if (lastTrades[i] is IPQSupportsStringUpdates<ILastTrade> pqLastTradeStringUpdate)
                foreach (var stringUpdate in pqLastTradeStringUpdate.GetStringUpdates(snapShotTime, messageFlags))
                    yield return stringUpdate.WithDepth((PQDepthKey)i);
    }

    public virtual bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        if (stringUpdate.Field.Id != PQQuoteFields.LastTradedDictionaryUpsertCommand) return false;
        var depth = stringUpdate.Field.DepthId.KeyToDepth();

        if (depth < lastTrades.Count && lastTrades[depth] is IPQSupportsStringUpdates<ILastTrade> pqLastTradeStringUpdate)
        {
            pqLastTradeStringUpdate.UpdateFieldString(stringUpdate);
            return true;
        }

        return false;
    }

    public override PQLastTradedList CopyFrom(ILastTradedList source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is PQLastTradedList sourceRecentlyTraded) NameIdLookup.CopyFrom(sourceRecentlyTraded.NameIdLookup, copyMergeFlags);
        if (lastTrades.Count < source.Capacity)
            for (var i = Count; i < source.Capacity; i++)
            {
                var newEntry = LastTradeEntrySelector.SelectLastTradeEntry(null, NameIdLookup, source[i]);
                lastTrades.Add(newEntry);
            }

        for (var i = 0; i < source.Capacity; i++)
        {
            var sourceLayer = source[i];
            if (i < lastTrades.Count && (sourceLayer == null || sourceLayer.IsEmpty))
            {
                lastTrades[i]?.StateReset();
                continue;
            }

            var foundAtIndex = false;

            IPQLastTrade? destinationLayer = null;
            if (i < lastTrades.Count)
            {
                var newDestinationLayer = lastTrades[i] ?? destinationLayer;
                foundAtIndex     = !ReferenceEquals(newDestinationLayer, destinationLayer);
                destinationLayer = newDestinationLayer;
            }

            destinationLayer = LastTradeEntrySelector.SelectLastTradeEntry
                (foundAtIndex ? destinationLayer : destinationLayer?.Clone(), NameIdLookup, sourceLayer);
            if (i >= lastTrades.Count)
                lastTrades.Add(destinationLayer);
            else
                lastTrades[i] = destinationLayer;

            destinationLayer?.CopyFrom(sourceLayer!, copyMergeFlags);
        }

        for (var i = source.Capacity; i < lastTrades.Count; i++)
            if (lastTrades[i] is IMutableLastTrade mutableLastTrade)
                mutableLastTrade.IsEmpty = true;
        return this;
    }

    public void EnsureRelatedItemsAreConfigured(IPQSourceTickerInfo? referenceInstance)
    {
        NameIdLookup = new PQNameIdLookupGenerator(PQQuoteFields.LastTradedDictionaryUpsertCommand);
        var entriesFactory = LastTradeEntrySelector.FindForLastTradeFlags(referenceInstance?.LastTradedFlags ?? LastTradedFlags.LastTradedPrice);
        var maxEntries     = PQQuoteFieldsExtensions.SingleByteFieldIdMaxPossibleLastTrades;

        for (var i = 0; i < maxEntries; i++)
        {
            var currLastTrade = i < lastTrades.Count ? lastTrades[i] : null;
            if (i >= lastTrades.Count || currLastTrade is not null)
                lastTrades.Add(entriesFactory!.CreateNewLastTradeEntry());
            else if (i < lastTrades.Count && currLastTrade != null
                                          && !LastTradeEntrySelector.TypeCanWholeyContain
                                                 (entriesFactory!.EntryCreationType, currLastTrade.GetType()))
                lastTrades[i] = entriesFactory.UpgradeLayer(currLastTrade);
        }

        for (var i = lastTrades.Count; i < maxEntries; i++) lastTrades[i]?.StateReset();
    }

    object ICloneable.Clone() => Clone();

    ILastTradedList ICloneable<ILastTradedList>.Clone() => Clone();

    IPQLastTradedList IPQLastTradedList.Clone() => Clone();

    IMutableLastTradedList IMutableLastTradedList.Clone() => Clone();

    public bool AreEquivalent(ILastTradedList? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var lastTradesSame = exactTypes
            ? lastTrades.SequenceEqual(other)
            : lastTrades.Zip(other, (thisLastTrade, otherLastTrade) => new { thisLastTrade, otherLastTrade })
                        .All(joined =>
                                 joined.thisLastTrade != null && joined.thisLastTrade.AreEquivalent(joined.otherLastTrade, exactTypes));
        return lastTradesSame;
    }

    public IEnumerator<IPQLastTrade> GetEnumerator() => lastTrades.Take(Count).GetEnumerator()!;

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
        NameIdLookup = nameIdLookup as IPQNameIdLookupGenerator ?? new PQNameIdLookupGenerator(PQQuoteFields.LastTradedDictionaryUpsertCommand);
        if (referenceInstance != null)
            CopyFrom(referenceInstance, CopyMergeFlags.FullReplace);
        else
            lastTrades.Clear();
    }

    private IPQNameIdLookupGenerator SourcePopulatedNameIdGeneratorFrom(IEnumerable<ILastTrade?> existing)
    {
        if (existing is IPQRecentlyTraded pqRecentlyTraded)
            return new PQNameIdLookupGenerator(pqRecentlyTraded.NameIdLookup, PQQuoteFields.LastTradedDictionaryUpsertCommand);
        return existing.OfType<ISupportsPQNameIdLookupGenerator>().Any()
            ? new PQNameIdLookupGenerator(existing.OfType<ISupportsPQNameIdLookupGenerator>().Select(snilg => snilg.NameIdLookup).First())
            : new PQNameIdLookupGenerator(PQQuoteFields.LastTradedDictionaryUpsertCommand);
    }

    public override PQLastTradedList Clone() =>
        Recycler?.Borrow<PQLastTradedList>().CopyFrom(this) ??
        new PQLastTradedList((IRecentlyTraded)this);
    
    protected string PQLastTradedListToStringMembers => $"LastTrades: [{string.Join(", ", lastTrades)}], {nameof(Count)}: {Count}";

    public override string ToString() =>
        $"{nameof(PQLastTradedList)}{{{PQLastTradedListToStringMembers}}}";

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ILastTradedList, true);

    public override int GetHashCode() => lastTrades.GetHashCode();
}
