// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

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

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.LastTraded;

public interface IPQRecentlyTraded : IMutableRecentlyTraded,
    IPQSupportsFieldUpdates<IRecentlyTraded>, IPQSupportsStringUpdates<IRecentlyTraded>,
    IEnumerable<IPQLastTrade>, IRelatedItems<IPQNameIdLookupGenerator>, IRelatedItems<IPQSourceTickerInfo>
  , ISupportsPQNameIdLookupGenerator
{
    new IPQLastTrade? this[int index] { get; set; }
    new IPQRecentlyTraded         Clone();
    new IEnumerator<IPQLastTrade> GetEnumerator();
}

public class PQRecentlyTraded : ReusableObject<IRecentlyTraded>, IPQRecentlyTraded
{
    private readonly IList<IPQLastTrade?> lastTrades;

    private IPQNameIdLookupGenerator nameIdLookupGenerator = null!;

    protected uint NumUpdates = uint.MaxValue;

    public PQRecentlyTraded()
    {
        lastTrades   = new List<IPQLastTrade?>();
        NameIdLookup = new PQNameIdLookupGenerator(PQQuoteFields.LastTradedDictionaryUpsertCommand);

        if (GetType() == typeof(PQRecentlyTraded)) NumUpdates = 0;
    }

    public PQRecentlyTraded(IPQSourceTickerInfo sourceTickerInfo)
    {
        LastTradesSupportFlags = sourceTickerInfo.LastTradedFlags;
        LastTradesOfType       = LastTradesSupportFlags.MostCompactLayerType();
        lastTrades             = new List<IPQLastTrade?>();
        NameIdLookup           = new PQNameIdLookupGenerator(PQQuoteFields.LastTradedDictionaryUpsertCommand);
        EnsureRelatedItemsAreConfigured(sourceTickerInfo);

        if (GetType() == typeof(PQRecentlyTraded)) NumUpdates = 0;
    }

    public PQRecentlyTraded(IEnumerable<IPQLastTrade?> lastTrades) : this(lastTrades.ToList())
    {
        if (GetType() == typeof(PQRecentlyTraded)) NumUpdates = 0;
    }

    public PQRecentlyTraded(IList<IPQLastTrade?> lastTrades)
    {
        this.lastTrades = new List<IPQLastTrade?>();
        NameIdLookup    = SourcePopulatedNameIdGeneratorFrom(lastTrades);
        this.lastTrades = lastTrades.Select(lt => LastTradeEntrySelector.SelectLastTradeEntry(null, NameIdLookup, lt, true)).ToList();

        if (GetType() == typeof(PQRecentlyTraded)) NumUpdates = 0;
    }

    public PQRecentlyTraded(IRecentlyTraded toClone)
    {
        lastTrades   = new List<IPQLastTrade?>(); // so NameIdLookup set doesn't blow up;
        NameIdLookup = SourcePopulatedNameIdGeneratorFrom(toClone);
        lastTrades   = toClone.Select(lt => LastTradeEntrySelector.SelectLastTradeEntry(null, NameIdLookup, lt, true)).ToList();
        EnsureRelatedItemsAreConfigured(toClone, NameIdLookup);

        if (GetType() == typeof(PQRecentlyTraded)) NumUpdates = 0;
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
        get => lastTrades[i];
        set
        {
            lastTrades[i] = value;
            if (value is ISupportsPQNameIdLookupGenerator lastTradeWithNameId) lastTradeWithNameId.NameIdLookup = NameIdLookup;
        }
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

    public IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
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


    public int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        if (pqFieldUpdate.Id is >= PQQuoteFields.LastTradedOrderId and <= PQQuoteFields.LastTradedTraderId)
        {
            var index       = pqFieldUpdate.DepthIndex();
            var pqLastTrade = this[index] as PQLastTrade;
            var result      = pqLastTrade?.UpdateField(pqFieldUpdate) ?? -1;
            return result;
        }

        return -1;
    }

    public IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags)
    {
        for (var i = 0; i < lastTrades.Count; i++)
            if (lastTrades[i] is IPQSupportsStringUpdates<ILastTrade> pqLastTradeStringUpdate)
                foreach (var stringUpdate in pqLastTradeStringUpdate.GetStringUpdates(snapShotTime, messageFlags))
                    yield return stringUpdate.WithDepth((PQDepthKey)i);
    }

    public bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
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
        var maxEntries     = PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades;

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

    IRecentlyTraded ICloneable<IRecentlyTraded>.Clone() => Clone();

    IPQRecentlyTraded IPQRecentlyTraded.Clone() => Clone();

    IMutableRecentlyTraded IMutableRecentlyTraded.Clone() => Clone();

    public bool AreEquivalent(IRecentlyTraded? other, bool exactTypes = false)
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

    IEnumerator<IMutableLastTrade> IMutableRecentlyTraded.GetEnumerator() => GetEnumerator();

    public void EnsureRelatedItemsAreConfigured(IPQNameIdLookupGenerator? otherNameIdLookupGenerator)
    {
        if (otherNameIdLookupGenerator != null)
        {
            NameIdLookup.CopyFrom(otherNameIdLookupGenerator);
            if (NameIdLookup.Count != otherNameIdLookupGenerator.Count) NameIdLookup.CopyFrom(otherNameIdLookupGenerator, CopyMergeFlags.FullReplace);
        }
    }

    public void EnsureRelatedItemsAreConfigured(IRecentlyTraded? referenceInstance, INameIdLookup? nameIdLookup)
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

    public override IPQRecentlyTraded Clone() =>
        (IPQRecentlyTraded?)Recycler?.Borrow<PQRecentlyTraded>().CopyFrom(this) ??
        new PQRecentlyTraded((IRecentlyTraded)this);

    public override string ToString() =>
        $"PQRecentlyTraded {{ {nameof(lastTrades)}: [{string.Join(",", (IEnumerable<ILastTrade>)this)}], " +
        $"{nameof(Count)}: {Count}, {nameof(HasUpdates)}: {HasUpdates} }}";

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IRecentlyTraded, true);

    public override int GetHashCode() => lastTrades.GetHashCode();
}
