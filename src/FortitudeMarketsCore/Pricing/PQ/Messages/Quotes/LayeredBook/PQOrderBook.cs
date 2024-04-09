#region

using System.Collections;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.SourceTickerInfo;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook;

public interface IPQOrderBook : IMutableOrderBook, IPQSupportsFieldUpdates<IOrderBook>,
    IPQSupportsStringUpdates<IOrderBook>, IEnumerable<IPQPriceVolumeLayer>, ICloneable<IPQOrderBook>,
    IRelatedItem<IPQSourceTickerQuoteInfo>, IRelatedItem<IPQPriceVolumeLayer>

{
    new IPQPriceVolumeLayer? this[int level] { get; set; }
    IList<IPQPriceVolumeLayer?> AllLayers { get; set; }
    new bool HasUpdates { get; set; }
    new IPQOrderBook Clone();
    new IEnumerator<IPQPriceVolumeLayer> GetEnumerator();
}

public class PQOrderBook : ReusableObject<IOrderBook>, IPQOrderBook
{
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQOrderBook));

    public PQOrderBook() => AllLayers = new List<IPQPriceVolumeLayer?>();

    public PQOrderBook(IPQSourceTickerQuoteInfo srcTickerQuoteInfo)
    {
        AllLayers = new List<IPQPriceVolumeLayer?>();
        EnsureRelatedItemsAreConfigured(srcTickerQuoteInfo);
    }

    public PQOrderBook(IEnumerable<IPriceVolumeLayer>? bookLayers = null)
    {
        AllLayers = bookLayers?
            .Select(pvl => (IPQPriceVolumeLayer?)LayerSelector.ConvertToExpectedImplementation(pvl))
            .ToList() ?? new List<IPQPriceVolumeLayer?>();
        if (AllLayers.Any())
        {
            AllLayers[0]?.EnsureRelatedItemsAreConfigured(null as IPQSourceTickerQuoteInfo);
            EnsureRelatedItemsAreConfigured(AllLayers[0]);
        }
    }

    public PQOrderBook(IOrderBook toClone)
    {
        var size = toClone.Capacity;
        AllLayers = new List<IPQPriceVolumeLayer?>(size);
        Capacity = toClone.Capacity;
        for (var i = 0; i < size; i++)
        {
            var pqLayer = (IPQPriceVolumeLayer?)LayerSelector.ConvertToExpectedImplementation(toClone[i], true);
            AllLayers.Add(pqLayer);
        }

        if (AllLayers.Count == 0 && toClone.Count == 0)
            AllLayers.Add((IPQPriceVolumeLayer?)LayerSelector.ConvertToExpectedImplementation(toClone[0], true));
    }

    public static IPQOrderBookLayerFactorySelector LayerSelector { get; set; }
        = new PQOrderBookLayerFactorySelector();

    protected string PQOrderBookToStringMembers =>
        $"{nameof(Capacity)}: {Capacity}, {nameof(Count)}: {Count}, " +
        $"{nameof(AllLayers)}:[{string.Join(", ", AllLayers.Take(Count))}]";

    public IPQPriceVolumeLayer? this[int level]
    {
        get => AllLayers[level];
        set
        {
            if (value == null && level == AllLayers.Count - 1)
            {
                AllLayers.RemoveAt(level);
                return;
            }

            AllLayers[level] = value;
        }
    }

    IPriceVolumeLayer? IOrderBook.this[int level] => this[level];

    IMutablePriceVolumeLayer? IMutableOrderBook.this[int level]
    {
        get => this[level];
        set => this[level] = value as IPQPriceVolumeLayer;
    }

    public IList<IPQPriceVolumeLayer?> AllLayers { get; set; }

    public int Capacity
    {
        get => AllLayers.Count;
        set
        {
            if (value > PQFieldKeys.SingleByteFieldIdMaxBookDepth)
                throw new ArgumentException("Expected PQOrderBook Capacity to be less than or equal to " +
                                            PQFieldKeys.SingleByteFieldIdMaxBookDepth);

            for (var i = AllLayers.Count; i < value && AllLayers.Count > 0; i++)
            {
                var clonedFirstLayer = AllLayers[0]?.Clone();
                clonedFirstLayer?.StateReset();
                if (clonedFirstLayer != null) AllLayers.Add(clonedFirstLayer);
            }
        }
    }

    public int Count
    {
        get
        {
            for (var i = AllLayers.Count - 1; i >= 0; i--)
            {
                var layerAtLevel = AllLayers[i];
                if ((layerAtLevel?.Price ?? 0) > 0) return i + 1;
            }

            return 0;
        }
    }

    public bool HasUpdates
    {
        get { return AllLayers.Any(pqpvl => pqpvl?.HasUpdates ?? false); }
        set
        {
            foreach (var pqPvLayer in AllLayers.Where(pql => pql is not null)) pqPvLayer!.HasUpdates = value;
        }
    }

    public override void StateReset()
    {
        for (var i = 0; i < AllLayers.Count; i++) AllLayers[i]?.StateReset();
        base.StateReset();
    }

    public IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, UpdateStyle updateStyle,
        IPQQuotePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        for (var i = 0; i < AllLayers.Count; i++)
            if (this[i] is { } currentLayer)
                foreach (var layerFields in currentLayer.GetDeltaUpdateFields(snapShotTime,
                             updateStyle, quotePublicationPrecisionSetting))
                {
                    var positionUpdate = new PQFieldUpdate((ushort)(layerFields.Id + i), layerFields.Value
                        , layerFields.Flag);
                    // logger.Info("Generating FieldUpdate: {0}", positionUpdate);
                    yield return positionUpdate;
                }
    }

    public int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        if (pqFieldUpdate.Id is >= PQFieldKeys.FirstLayersRangeStart and <= PQFieldKeys.FirstLayersRangeEnd)
        {
            var index = (pqFieldUpdate.Id - PQFieldKeys.LayerPriceOffset) % PQFieldKeys.SingleByteFieldIdMaxBookDepth;
            var pqPriceVolumeLayer = this[index];
            // logger.Debug("Recieved pqFieldUpdate: {}", pqFieldUpdate);
            return pqPriceVolumeLayer?.UpdateField(pqFieldUpdate) ?? -1;
        }

        if (pqFieldUpdate.Id is >= PQFieldKeys.SecondLayersRangeStart and <= PQFieldKeys.SecondLayersRangeEnd)
        {
            var index = (pqFieldUpdate.Id - PQFieldKeys.SecondLayersRangeStart) %
                        PQFieldKeys.SingleByteFieldIdMaxBookDepth;
            var pqPriceVolumeLayer = this[index];
            return pqPriceVolumeLayer?.UpdateField(pqFieldUpdate) ?? -1;
        }

        return -1;
    }


    public IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, UpdateStyle updatedStyle)
    {
        if (AllLayers.Count <= 0) yield break;
        // All layers share same dictionary or should do anyway
        if (!(this[0] is IPQSupportsStringUpdates<IPriceVolumeLayer> stringUpdateable)) yield break;
        foreach (var stringUpdate in stringUpdateable.GetStringUpdates(snapShotTime, updatedStyle))
            yield return stringUpdate;
    }

    public bool UpdateFieldString(PQFieldStringUpdate updates)
    {
        if (AllLayers.Count > 0)
            //all layers share same dictionary so update any layers dictionary.
            if (this[0] is IPQSupportsStringUpdates<IPriceVolumeLayer> stringUpdateable)
                return stringUpdateable.UpdateFieldString(updates);
        return false;
    }

    object ICloneable.Clone() => Clone();

    IOrderBook ICloneable<IOrderBook>.Clone() => Clone();

    IMutableOrderBook ICloneable<IMutableOrderBook>.Clone() => Clone();

    IMutableOrderBook IMutableOrderBook.Clone() => Clone();

    public override IPQOrderBook Clone() => (IPQOrderBook?)Recycler?.Borrow<PQOrderBook>().CopyFrom(this) ?? new PQOrderBook(this);

    public override IOrderBook CopyFrom(IOrderBook source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        for (var i = 0; i < source.Count; i++)
        {
            var sourcelayer = source[i];
            if (sourcelayer == null || sourcelayer.IsEmpty)
            {
                AllLayers[i]?.StateReset();
                continue;
            }

            IPQPriceVolumeLayer? destinationLayer = null;
            var foundAtIndex = false;
            if (i < AllLayers.Count)
            {
                var newDestinationLayer = AllLayers[i];
                foundAtIndex = !ReferenceEquals(newDestinationLayer, destinationLayer);
                destinationLayer = newDestinationLayer;
            }

            destinationLayer = LayerSelector.SelectPriceVolumeLayer(destinationLayer, sourcelayer);
            AllLayers[i] = destinationLayer;
            destinationLayer?.CopyFrom(sourcelayer);
        }

        for (var i = source.Count; i < AllLayers.Count; i++) AllLayers[i]?.StateReset();
        return this;
    }

    public void EnsureRelatedItemsAreConfigured(IPQSourceTickerQuoteInfo? referenceInstance)
    {
        int maxBookDepth = Math.Max((byte)1, Math.Min(referenceInstance!.MaximumPublishedLayers,
            PQFieldKeys.SingleByteFieldIdMaxBookDepth));

        var layerFactory = LayerSelector.FindForLayerFlags(referenceInstance);

        for (var i = 0; i < maxBookDepth; i++)
            if (i >= AllLayers.Count || AllLayers[i] == null)
                AllLayers.Add(layerFactory.CreateNewLayer());
            else if (i < AllLayers.Count && AllLayers[i] is { } currentLayer
                                         && !LayerSelector.TypeCanWholeyContain(layerFactory.LayerCreationType
                                             , currentLayer.GetType()))
                AllLayers[i] = layerFactory.UpgradeLayer(currentLayer);
        for (var i = maxBookDepth; i < AllLayers.Count; i++) AllLayers.RemoveAt(i);
    }

    public void EnsureRelatedItemsAreConfigured(IPQPriceVolumeLayer? referenceInstance)
    {
        var numberOfItemsToCopy = Capacity;
        // first layer copied twice only on bidBook layer.
        for (var i = 0; i < numberOfItemsToCopy; i++) AllLayers[i]?.EnsureRelatedItemsAreConfigured(referenceInstance);
    }

    public virtual bool AreEquivalent(IOrderBook? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;

        var deepestPricedSame = Count == other.Count;
        var bookLayersSame = exactTypes ?
            AllLayers.SequenceEqual(other) :
            AllLayers.Zip(other, (thisLayer, otherLayer) => new { thisLayer, otherLayer })
                .All(joined => joined.thisLayer != null && joined.thisLayer.AreEquivalent(joined.otherLayer));

        var deepestPossibleSame = true;
        if (other is IMutableOrderBook mutableOrderBook) deepestPossibleSame = Capacity == mutableOrderBook.Capacity;

        return deepestPossibleSame && deepestPricedSame && bookLayersSame;
    }

    public IEnumerator<IPQPriceVolumeLayer> GetEnumerator() => AllLayers.Take(Count).GetEnumerator()!;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<IPriceVolumeLayer> IEnumerable<IPriceVolumeLayer>.GetEnumerator() => GetEnumerator();

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IOrderBook, true);

    public override int GetHashCode() => AllLayers != null ? AllLayers.GetHashCode() : 0;

    public override string ToString() => $"{GetType().Name}({PQOrderBookToStringMembers})";
}
