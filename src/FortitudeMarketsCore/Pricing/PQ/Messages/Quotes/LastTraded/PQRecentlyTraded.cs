#region

using System.Collections;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LastTraded.LastTradeEntrySelector;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.SourceTickerInfo;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LastTraded;

public interface IPQRecentlyTraded : IMutableRecentlyTraded,
    IPQSupportsFieldUpdates<IRecentlyTraded>, IPQSupportsStringUpdates<IRecentlyTraded>,
    IEnumerable<IPQLastTrade>, IRelatedItem<IPQSourceTickerQuoteInfo>, IRelatedItem<IPQLastTrade>
{
    new IPQLastTrade? this[int index] { get; set; }
    new IPQRecentlyTraded Clone();
    new IEnumerator<IPQLastTrade> GetEnumerator();
}

public class PQRecentlyTraded : ReusableObject<IRecentlyTraded>, IPQRecentlyTraded
{
    private IList<IPQLastTrade?> lastTrades;

    public PQRecentlyTraded() => lastTrades = new List<IPQLastTrade?>();

    public PQRecentlyTraded(PQSourceTickerQuoteInfo sourceTickerQuoteInfo)
    {
        lastTrades = new List<IPQLastTrade?>();
        EnsureRelatedItemsAreConfigured(sourceTickerQuoteInfo);
    }

    public PQRecentlyTraded(IEnumerable<IPQLastTrade?> lastTrades)
    {
        this.lastTrades = lastTrades.ToList();
        EnsureRelatedItemsAreConfigured(this.lastTrades[0]);
    }

    public PQRecentlyTraded(IList<IPQLastTrade?> lastTrades)
    {
        this.lastTrades = lastTrades;
        EnsureRelatedItemsAreConfigured(lastTrades[0]);
    }

    public PQRecentlyTraded(IRecentlyTraded toClone)
    {
        lastTrades = toClone
            .Select(lt => (IPQLastTrade?)LastTradeEntrySelector.ConvertToExpectedImplementation(lt, true)).ToList();
    }

    public static IPQLastTradeTypeSelector LastTradeEntrySelector { get; set; } = new PQLastTradeEntrySelector();

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

    public IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, PQMessageFlags messageFlags,
        IPQQuotePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
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
            var result = pqLastTrade?.UpdateField(pqFieldUpdate) ?? -1;
            return result;
        }

        return -1;
    }

    public IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, PQMessageFlags messageFlags)
    {
        if (lastTrades.Any() && lastTrades[0] is IPQSupportsStringUpdates<ILastTrade> firstLastTrade)
            foreach (var pqLastTradeStringUpdate in firstLastTrade.GetStringUpdates(snapShotTime, messageFlags))
                yield return pqLastTradeStringUpdate;
    }

    public bool UpdateFieldString(PQFieldStringUpdate updates)
    {
        if (updates.Field.Id != PQFieldKeys.LastTraderDictionaryUpsertCommand) return false;
        if (lastTrades.Any() && lastTrades[0] is IPQSupportsStringUpdates<ILastTrade> firstLastTrade)
        {
            // all trade layers share same dictionary
            firstLastTrade.UpdateFieldString(updates);
            return true;
        }

        return false;
    }

    public override IRecentlyTraded CopyFrom(IRecentlyTraded source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        IPQLastTrade? destinationLayer = null;
        ILastTrade? sourcelayer = null;
        if (lastTrades.Count < source.Capacity)
        {
            var oldList = lastTrades;
            lastTrades = new List<IPQLastTrade?>(source.Capacity);
            foreach (var toCopy in oldList)
                lastTrades.Add(toCopy);
        }

        for (var i = 0; i < source.Capacity; i++)
        {
            sourcelayer = source[i];
            if (i < lastTrades.Count && (sourcelayer == null || sourcelayer.IsEmpty))
            {
                lastTrades[i]?.StateReset();
                continue;
            }

            var foundAtIndex = false;
            if (i < lastTrades.Count)
            {
                var newDestinationLayer = lastTrades[i] ?? destinationLayer;
                foundAtIndex = !ReferenceEquals(newDestinationLayer, destinationLayer);
                destinationLayer = newDestinationLayer;
            }

            destinationLayer = LastTradeEntrySelector.SelectLastTradeEntry(
                foundAtIndex ? destinationLayer : destinationLayer?.Clone(), sourcelayer);
            if (i >= lastTrades.Count)
                lastTrades.Add(destinationLayer);
            else
                lastTrades[i] = destinationLayer;

            destinationLayer?.CopyFrom(sourcelayer!);
        }

        for (var i = source.Capacity; i < lastTrades.Count; i++) lastTrades[i]?.StateReset();
        return this;
    }

    public void EnsureRelatedItemsAreConfigured(IPQSourceTickerQuoteInfo? referenceInstance)
    {
        var entiresFactory = LastTradeEntrySelector.FindForLastTradeFlags(referenceInstance);

        var maxEntries = PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades;

        IPQLastTrade? firstEntry = null;
        for (var i = 0; i < maxEntries; i++)
        {
            var currLastTrade = i < lastTrades.Count ? lastTrades[i] : null;
            if (i >= lastTrades.Count || currLastTrade is not null)
                lastTrades.Add(entiresFactory!.CreateNewLastTradeEntry());
            else if (i < lastTrades.Count && currLastTrade != null
                                          && !LastTradeEntrySelector.TypeCanWholeyContain(
                                              entiresFactory!.EntryCreationType,
                                              currLastTrade.GetType()))
                lastTrades[i] = entiresFactory.UpgradeLayer(currLastTrade);
            if (i == 0 && currLastTrade != null)
            {
                firstEntry = currLastTrade;
                currLastTrade.EnsureRelatedItemsAreConfigured(referenceInstance);
            }
            else
            {
                lastTrades[i]?.EnsureRelatedItemsAreConfigured(firstEntry);
            }
        }

        for (var i = lastTrades.Count; i < lastTrades.Count; i++) lastTrades.RemoveAt(i);
    }

    public void EnsureRelatedItemsAreConfigured(IPQLastTrade? referenceInstance)
    {
        for (var i = 0; i < lastTrades.Count; i++) lastTrades[i]?.EnsureRelatedItemsAreConfigured(referenceInstance);
    }

    object ICloneable.Clone() => Clone();

    IRecentlyTraded ICloneable<IRecentlyTraded>.Clone() => Clone();

    IPQRecentlyTraded IPQRecentlyTraded.Clone() => Clone();

    IMutableRecentlyTraded IMutableRecentlyTraded.Clone() => Clone();

    public bool AreEquivalent(IRecentlyTraded? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var bookLayersSame = exactTypes ?
            lastTrades.SequenceEqual(other) :
            lastTrades.Zip(other, (thisLastTrade, otherLastTrade) => new { thisLastTrade, otherLastTrade })
                .All(joined =>
                    joined.thisLastTrade != null && joined.thisLastTrade.AreEquivalent(joined.otherLastTrade));
        return bookLayersSame;
    }

    public IEnumerator<IPQLastTrade> GetEnumerator() => lastTrades.Take(Count).GetEnumerator()!;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<ILastTrade> IEnumerable<ILastTrade>.GetEnumerator() => GetEnumerator();

    IEnumerator<IMutableLastTrade> IMutableRecentlyTraded.GetEnumerator() => GetEnumerator();

    public override IPQRecentlyTraded Clone() =>
        (IPQRecentlyTraded?)Recycler?.Borrow<PQRecentlyTraded>().CopyFrom(this) ??
        new PQRecentlyTraded((IRecentlyTraded)this);

    public override string ToString() =>
        $"PQRecentlyTraded {{ {nameof(lastTrades)}: [{string.Join(",", (IEnumerable<ILastTrade>)this)}], " +
        $"{nameof(Count)}: {Count}, {nameof(HasUpdates)}: {HasUpdates} }}";

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IRecentlyTraded, true);

    public override int GetHashCode() => lastTrades?.GetHashCode() ?? 0;
}
