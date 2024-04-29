#region

using System.Collections;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.SourceTickerInfo;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook;

public interface IPQOrderBook : IMutableOrderBook, IPQSupportsFieldUpdates<IOrderBook>,
    IPQSupportsStringUpdates<IOrderBook>, IEnumerable<IPQPriceVolumeLayer>, ICloneable<IPQOrderBook>,
    IRelatedItem<IPQSourceTickerQuoteInfo>, IRelatedItem<IPQNameIdLookupGenerator>,
    ISupportsPQNameIdLookupGenerator

{
    new IPQPriceVolumeLayer? this[int level] { get; set; }
    IList<IPQPriceVolumeLayer?> AllLayers { get; set; }
    new bool HasUpdates { get; set; }
    new IPQNameIdLookupGenerator NameIdLookup { get; set; }
    new IPQOrderBook Clone();
    new IEnumerator<IPQPriceVolumeLayer> GetEnumerator();
}

public class PQOrderBook : ReusableObject<IOrderBook>, IPQOrderBook
{
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQOrderBook));

    private IList<IPQPriceVolumeLayer?>? allLayers;

    private IPQNameIdLookupGenerator nameIdLookupGenerator = null!;

    public PQOrderBook()
    {
        BookSide = BookSide.Unknown;
        NameIdLookup
            = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand);
    }

    public PQOrderBook(BookSide bookSide)
    {
        BookSide = bookSide;
        NameIdLookup
            = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand);
    }

    public PQOrderBook(BookSide bookSide, IPQSourceTickerQuoteInfo srcTickerQuoteInfo)
    {
        BookSide = bookSide;
        NameIdLookup
            = new PQNameIdLookupGenerator(srcTickerQuoteInfo.NameIdLookup, PQFieldKeys.LayerNameDictionaryUpsertCommand);
        EnsureRelatedItemsAreConfigured(srcTickerQuoteInfo);
    }

    public PQOrderBook(BookSide bookSide, IEnumerable<IPriceVolumeLayer>? bookLayers = null)
    {
        BookSide = bookSide;
        NameIdLookup = SourceOtherExistingOrNewPQNameIdNameLookup(bookLayers);
        AllLayers = bookLayers?
            .Select(pvl => (IPQPriceVolumeLayer?)LayerSelector.ConvertToExpectedImplementation(pvl))
            .ToList() ?? new List<IPQPriceVolumeLayer?>();
    }

    public PQOrderBook(IOrderBook toClone)
    {
        BookSide = toClone.BookSide;
        NameIdLookup = SourceOtherExistingOrNewPQNameIdNameLookup(toClone);
        var size = toClone.Capacity;
        AllLayers = new List<IPQPriceVolumeLayer?>(size);
        Capacity = toClone.Capacity;
        for (var i = 0; i < size; i++)
        {
            var pqLayer = (IPQPriceVolumeLayer?)LayerSelector.ConvertToExpectedImplementation(toClone[i], true);
            AllLayers.Add(pqLayer);
        }
    }

    private IPQOrderBookLayerFactorySelector LayerSelector { get; set; } = null!;

    protected string PQOrderBookToStringMembers =>
        $"{nameof(Capacity)}: {Capacity}, {nameof(Count)}: {Count}, " +
        $"{nameof(AllLayers)}:[{string.Join(", ", AllLayers.Take(Count))}]";

    public BookSide BookSide { get; }

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

    public IList<IPQPriceVolumeLayer?> AllLayers
    {
        get => allLayers ??= new List<IPQPriceVolumeLayer?>();
        set => allLayers = value;
    }

    INameIdLookup? IHasNameIdLookup.NameIdLookup => NameIdLookup;

    public IPQNameIdLookupGenerator NameIdLookup
    {
        get => nameIdLookupGenerator;
        set
        {
            nameIdLookupGenerator = value;
            LayerSelector = new PQOrderBookLayerFactorySelector(nameIdLookupGenerator);
            if (allLayers != null)
                foreach (var pqPriceVolumeLayer in allLayers.OfType<ISupportsPQNameIdLookupGenerator>())
                    pqPriceVolumeLayer.NameIdLookup = value;
        }
    }

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

    public IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, PQMessageFlags messageFlags,
        IPQQuotePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        for (var i = 0; i < AllLayers.Count; i++)
            if (this[i] is { } currentLayer)
                foreach (var layerFields in currentLayer.GetDeltaUpdateFields(snapShotTime,
                             messageFlags, quotePublicationPrecisionSetting))
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


    public IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, PQMessageFlags messageFlags)
    {
        if (AllLayers.Count <= 0) yield break;
        // All layers share same dictionary or should do anyway
        if (!(this[0] is IPQSupportsStringUpdates<IPriceVolumeLayer> stringUpdateable)) yield break;
        foreach (var stringUpdate in stringUpdateable.GetStringUpdates(snapShotTime, messageFlags))
            if (BookSide == BookSide.AskBook)
                yield return PQFieldStringUpdate.SetFieldFlag(stringUpdate, PQFieldFlags.IsAskSideFlag);
            else
                yield return stringUpdate;
    }

    public bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        if (AllLayers.Count > 0)
            //all layers share same dictionary so update any layers dictionary.
            return NameIdLookup.UpdateFieldString(stringUpdate);

        return false;
    }

    object ICloneable.Clone() => Clone();

    IOrderBook ICloneable<IOrderBook>.Clone() => Clone();

    IMutableOrderBook ICloneable<IMutableOrderBook>.Clone() => Clone();

    IMutableOrderBook IMutableOrderBook.Clone() => Clone();

    public override IPQOrderBook Clone() => (IPQOrderBook?)Recycler?.Borrow<PQOrderBook>().CopyFrom(this) ?? new PQOrderBook(this);

    public override IOrderBook CopyFrom(IOrderBook source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        var thisLayDict = SourceOtherExistingOrNewPQNameIdNameLookup(source);


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

            destinationLayer = LayerSelector.SelectPriceVolumeLayer(destinationLayer, thisLayDict, sourcelayer);
            AllLayers[i] = destinationLayer;
            destinationLayer?.CopyFrom(sourcelayer);
        }

        for (var i = source.Count; i < AllLayers.Count; i++) AllLayers[i]?.StateReset();
        return this;
    }

    public void EnsureRelatedItemsAreConfigured(IPQNameIdLookupGenerator? otherNameIdLookupGenerator)
    {
        if (otherNameIdLookupGenerator != null) NameIdLookup.CopyFrom(otherNameIdLookupGenerator);
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

    public void EnsureRelatedItemsAreConfigured(IPQSourceTickerQuoteInfo? referenceInstance)
    {
        int maxBookDepth = Math.Max((byte)1, Math.Min(referenceInstance!.MaximumPublishedLayers, PQFieldKeys.SingleByteFieldIdMaxBookDepth));

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

    private PQNameIdLookupGenerator SourceOtherExistingOrNewPQNameIdNameLookup(IEnumerable<IPriceVolumeLayer>? source)
    {
        PQNameIdLookupGenerator thisLayDict;
        if (source is IPQOrderBook { NameIdLookup: not null } pqOrderBook)
            thisLayDict = new PQNameIdLookupGenerator(pqOrderBook.NameIdLookup, PQFieldKeys.LayerNameDictionaryUpsertCommand);
        else if (source?.Any(pvl => pvl is IHasNameIdLookup { NameIdLookup: not null }) == true)
            thisLayDict = new PQNameIdLookupGenerator(
                source.OfType<IHasNameIdLookup>().Where(pvl => pvl is { NameIdLookup: not null })
                    .Select(pvl => pvl.NameIdLookup)
                    .First()!, PQFieldKeys.LayerNameDictionaryUpsertCommand);
        else
            thisLayDict = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand);

        return thisLayDict;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IOrderBook, true);

    public override int GetHashCode() => AllLayers != null ? AllLayers.GetHashCode() : 0;

    public override string ToString() => $"{GetType().Name}({PQOrderBookToStringMembers})";
}
