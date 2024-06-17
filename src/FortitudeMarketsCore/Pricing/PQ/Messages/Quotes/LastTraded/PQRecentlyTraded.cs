// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LastTraded.LastTradeEntrySelector;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LastTraded;

public interface IPQRecentlyTraded : IMutableRecentlyTraded,
    IPQSupportsFieldUpdates<IRecentlyTraded>, IPQSupportsStringUpdates<IRecentlyTraded>,
    IEnumerable<IPQLastTrade>, IRelatedItem<IPQNameIdLookupGenerator>, IRelatedItem<IPQSourceTickerQuoteInfo>
  , ISupportsPQNameIdLookupGenerator
{
    new IPQLastTrade? this[int index] { get; set; }
    new IPQRecentlyTraded         Clone();
    new IEnumerator<IPQLastTrade> GetEnumerator();
}

public class PQRecentlyTraded : ReusableObject<IRecentlyTraded>, IPQRecentlyTraded
{
    private readonly IList<IPQLastTrade?>     lastTrades;
    private          IPQNameIdLookupGenerator nameIdLookupGenerator = null!;

    public PQRecentlyTraded()
    {
        lastTrades   = new List<IPQLastTrade?>();
        NameIdLookup = new PQNameIdLookupGenerator(PQFieldKeys.LastTraderDictionaryUpsertCommand);
    }

    public PQRecentlyTraded(IPQSourceTickerQuoteInfo sourceTickerQuoteInfo)
    {
        LastTradesSupportFlags = sourceTickerQuoteInfo.LastTradedFlags;
        LastTradesOfType       = LastTradesSupportFlags.MostCompactLayerType();
        lastTrades             = new List<IPQLastTrade?>();
        NameIdLookup           = new PQNameIdLookupGenerator(PQFieldKeys.LastTraderDictionaryUpsertCommand);
        EnsureRelatedItemsAreConfigured(sourceTickerQuoteInfo);
    }

    public PQRecentlyTraded(IEnumerable<IPQLastTrade?> lastTrades) : this(lastTrades.ToList()) { }

    public PQRecentlyTraded(IList<IPQLastTrade?> lastTrades)
    {
        this.lastTrades = new List<IPQLastTrade?>();
        NameIdLookup    = SourcePopulatedNameIdGeneratorFrom(lastTrades);
        this.lastTrades = lastTrades.Select(lt => LastTradeEntrySelector.SelectLastTradeEntry(null, NameIdLookup, lt, true)).ToList();
    }

    public PQRecentlyTraded(IRecentlyTraded toClone)
    {
        lastTrades   = new List<IPQLastTrade?>(); // so NameIdLookup set doesn't blow up;
        NameIdLookup = SourcePopulatedNameIdGeneratorFrom(toClone);
        lastTrades   = toClone.Select(lt => LastTradeEntrySelector.SelectLastTradeEntry(null, NameIdLookup, lt, true)).ToList();
        EnsureRelatedItemsAreConfigured(toClone, NameIdLookup);
    }

    public IPQLastTradeTypeSelector LastTradeEntrySelector { get; set; } = null!;
    public LastTradeType            LastTradesOfType       { get; }      = LastTradeType.Price;

    public LastTradedFlags LastTradesSupportFlags { get; } = LastTradedFlags.LastTradedPrice | LastTradedFlags.LastTradedTime;

    public IPQNameIdLookupGenerator NameIdLookup
    {
        get => nameIdLookupGenerator;
        set
        {
            nameIdLookupGenerator  = value;
            LastTradeEntrySelector = new PQLastTradeEntrySelector(nameIdLookupGenerator);
            foreach (var pqLastTrade in lastTrades.OfType<ISupportsPQNameIdLookupGenerator>())
                pqLastTrade.NameIdLookup = value;
        }
    }

    public IPQLastTrade? this[int i]
    {
        get => lastTrades[i];
        set => lastTrades[i] = value;
    }

    ILastTrade? IRecentlyTraded.this[int i] => this[i];

    IMutableLastTrade? IMutableRecentlyTraded.this[int i]
    {
        get => this[i];
        set => this[i] = value as IPQLastTrade;
    }

    public int Capacity
    {
        get => lastTrades.Count;
        set
        {
            if (value > PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades)
                throw new ArgumentException("Expected PQRecentlyTraded Capacity to be less than or equal to " +
                                            PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades);
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

    INameIdLookup? IHasNameIdLookup.NameIdLookup => NameIdLookup;

    public int AppendEntryAtEnd()
    {
        var index = Count;
        lastTrades.Add(LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags)!.CreateNewLastTradeEntry());
        return index;
    }

    public override void StateReset()
    {
        foreach (var lastTrade in lastTrades) lastTrade?.StateReset();
        base.StateReset();
    }

    public void Add(IMutableLastTrade newLastTrade)
    {
        if (lastTrades.Count == Count)
            lastTrades.Add((IPQLastTrade)newLastTrade);
        else
            lastTrades[Count] = (IPQLastTrade)newLastTrade;
    }

    public IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        for (var i = 0; i < lastTrades.Count; i++)
            if (this[i] is PQLastTrade lastTrade)
                foreach (var layerFields in lastTrade.GetDeltaUpdateFields(snapShotTime, messageFlags,
                                                                           quotePublicationPrecisionSetting))
                    yield return new PQFieldUpdate((byte)(layerFields.Id + i), layerFields.Value, layerFields.Flag);
    }


    public int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        if (pqFieldUpdate.Id is >= PQFieldKeys.LastTradedRangeStart and <= PQFieldKeys.LastTradedRangeEnd)
        {
            var index = (pqFieldUpdate.Id - PQFieldKeys.LastTradedRangeStart) %
                        PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades;
            var pqLastTrade = this[index] as PQLastTrade;
            var result      = pqLastTrade?.UpdateField(pqFieldUpdate) ?? -1;
            return result;
        }

        return -1;
    }

    public IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags)
    {
        if (lastTrades.Any() && lastTrades[0] is IPQSupportsStringUpdates<ILastTrade> firstLastTrade)
            foreach (var pqLastTradeStringUpdate in firstLastTrade.GetStringUpdates(snapShotTime, messageFlags))
                yield return pqLastTradeStringUpdate;
    }

    public bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        if (stringUpdate.Field.Id != PQFieldKeys.LastTraderDictionaryUpsertCommand) return false;
        if (lastTrades.Any() && lastTrades[0] is IPQSupportsStringUpdates<ILastTrade> firstLastTrade)
        {
            // all trade layers share same dictionary
            firstLastTrade.UpdateFieldString(stringUpdate);
            return true;
        }

        return false;
    }

    public override IRecentlyTraded CopyFrom(IRecentlyTraded source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is PQRecentlyTraded sourceRecentlyTraded) NameIdLookup.CopyFrom(sourceRecentlyTraded.NameIdLookup, copyMergeFlags);
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

            var           foundAtIndex     = false;
            IPQLastTrade? destinationLayer = null;
            if (i < lastTrades.Count)
            {
                var newDestinationLayer = lastTrades[i] ?? destinationLayer;
                foundAtIndex     = !ReferenceEquals(newDestinationLayer, destinationLayer);
                destinationLayer = newDestinationLayer;
            }

            destinationLayer = LastTradeEntrySelector.SelectLastTradeEntry(
                                                                           foundAtIndex ? destinationLayer : destinationLayer?.Clone(), NameIdLookup
                                                                         , sourceLayer);
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

    public void EnsureRelatedItemsAreConfigured(IPQSourceTickerQuoteInfo? referenceInstance)
    {
        NameIdLookup = new PQNameIdLookupGenerator(PQFieldKeys.LastTraderDictionaryUpsertCommand);
        var entriesFactory = LastTradeEntrySelector.FindForLastTradeFlags(referenceInstance?.LastTradedFlags ?? LastTradedFlags.LastTradedPrice);
        var maxEntries     = PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades;

        for (var i = 0; i < maxEntries; i++)
        {
            var currLastTrade = i < lastTrades.Count ? lastTrades[i] : null;
            if (i >= lastTrades.Count || currLastTrade is not null)
                lastTrades.Add(entriesFactory!.CreateNewLastTradeEntry());
            else if (i < lastTrades.Count && currLastTrade != null
                                          && !LastTradeEntrySelector.TypeCanWholeyContain(
                                                                                          entriesFactory!.EntryCreationType,
                                                                                          currLastTrade.GetType()))
                lastTrades[i] = entriesFactory.UpgradeLayer(currLastTrade);
        }

        for (var i = lastTrades.Count; i < maxEntries; i++) lastTrades[i]?.StateReset();
    }

    object ICloneable.Clone() => Clone();

    IRecentlyTraded ICloneable<IRecentlyTraded>.Clone() => Clone();

    IPQRecentlyTraded IPQRecentlyTraded.Clone() => Clone();

    IMutableRecentlyTraded IMutableRecentlyTraded.Clone() => Clone();

    public bool AreEquivalent(IRecentlyTraded? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var bookLayersSame = exactTypes
            ? lastTrades.SequenceEqual(other)
            : lastTrades.Zip(other, (thisLastTrade, otherLastTrade) => new { thisLastTrade, otherLastTrade })
                        .All(joined =>
                                 joined.thisLastTrade != null && joined.thisLastTrade.AreEquivalent(joined.otherLastTrade));
        return bookLayersSame;
    }

    public IEnumerator<IPQLastTrade> GetEnumerator() => lastTrades.Take(Count).GetEnumerator()!;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<ILastTrade> IEnumerable<ILastTrade>.GetEnumerator() => GetEnumerator();

    IEnumerator<IMutableLastTrade> IMutableRecentlyTraded.GetEnumerator() => GetEnumerator();

    public void EnsureRelatedItemsAreConfigured(IPQNameIdLookupGenerator? otherNameIdLookupGenerator)
    {
        if (otherNameIdLookupGenerator != null) NameIdLookup.CopyFrom(otherNameIdLookupGenerator);
    }

    public void EnsureRelatedItemsAreConfigured(IRecentlyTraded? referenceInstance, INameIdLookup? nameIdLookup)
    {
        NameIdLookup = nameIdLookup as IPQNameIdLookupGenerator ?? new PQNameIdLookupGenerator(PQFieldKeys.LastTraderDictionaryUpsertCommand);
        if (referenceInstance != null)
            CopyFrom(referenceInstance, CopyMergeFlags.FullReplace);
        else
            lastTrades.Clear();
    }

    private IPQNameIdLookupGenerator SourcePopulatedNameIdGeneratorFrom(IEnumerable<ILastTrade?> existing)
    {
        if (existing is IPQRecentlyTraded pqRecentlyTraded)
            return new PQNameIdLookupGenerator(pqRecentlyTraded.NameIdLookup, PQFieldKeys.LastTraderDictionaryUpsertCommand);
        return existing.OfType<ISupportsPQNameIdLookupGenerator>().Any()
            ? new PQNameIdLookupGenerator(existing.OfType<ISupportsPQNameIdLookupGenerator>().Select(snilg => snilg.NameIdLookup).First())
            : new PQNameIdLookupGenerator(PQFieldKeys.LastTraderDictionaryUpsertCommand);
    }

    public override IPQRecentlyTraded Clone() =>
        (IPQRecentlyTraded?)Recycler?.Borrow<PQRecentlyTraded>().CopyFrom(this) ??
        new PQRecentlyTraded((IRecentlyTraded)this);

    public override string ToString() =>
        $"PQRecentlyTraded {{ {nameof(lastTrades)}: [{string.Join(",", (IEnumerable<ILastTrade>)this)}], " +
        $"{nameof(Count)}: {Count}, {nameof(HasUpdates)}: {HasUpdates} }}";

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IRecentlyTraded, true);

    public override int GetHashCode() => lastTrades.GetHashCode();
}
