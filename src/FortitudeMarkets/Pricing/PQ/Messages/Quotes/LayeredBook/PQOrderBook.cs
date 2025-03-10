// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;

public interface IPQOrderBook : IMutableOrderBook, IPQSupportsFieldUpdates<IOrderBook>,
    IPQSupportsStringUpdates<IOrderBook>, IEnumerable<IPQPriceVolumeLayer>, ICloneable<IPQOrderBook>,
    IRelatedItem<IPQSourceTickerInfo>, IRelatedLinkedItem<LayerFlags, IPQNameIdLookupGenerator>,
    ISupportsPQNameIdLookupGenerator

{
    new IPQPriceVolumeLayer? this[int level] { get; set; }
    IList<IPQPriceVolumeLayer?>          AllLayers    { get; set; }
    new bool                             HasUpdates   { get; set; }
    new IPQNameIdLookupGenerator         NameIdLookup { get; set; }
    new IPQOrderBook                     Clone();
    new IEnumerator<IPQPriceVolumeLayer> GetEnumerator();
}

public class PQOrderBook : ReusableObject<IOrderBook>, IPQOrderBook
{
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQOrderBook));

    private IList<IPQPriceVolumeLayer?>? allLayers;

    private IPQNameIdLookupGenerator nameIdLookupGenerator = null!;

    public PQOrderBook()
    {
        BookSide     = BookSide.Unknown;
        NameIdLookup = InitializeNewIdLookupGenerator();
    }

    public PQOrderBook(BookSide bookSide, LayerType layerType = LayerType.PriceVolume, bool isLadder = false)
    {
        BookSide     = bookSide;
        IsLadder     = isLadder;
        NameIdLookup = InitializeNewIdLookupGenerator();
        LayersOfType = layerType;
    }

    public PQOrderBook(BookSide bookSide, IPQSourceTickerInfo srcTickerInfo)
    {
        BookSide     = bookSide;
        IsLadder     = srcTickerInfo.LayerFlags.HasLadder();
        NameIdLookup = InitializeNewIdLookupGenerator(srcTickerInfo.NameIdLookup);
        EnsureRelatedItemsAreConfigured(srcTickerInfo);
    }

    public PQOrderBook(BookSide bookSide, IEnumerable<IPriceVolumeLayer>? bookLayers = null, bool isLadder = false)
    {
        BookSide     = bookSide;
        IsLadder     = isLadder;
        NameIdLookup = SourceOtherExistingOrNewPQNameIdNameLookup(bookLayers);
        AllLayers = bookLayers?
                    .Select(pvl => (IPQPriceVolumeLayer?)LayerSelector.UpgradeExistingLayer(pvl, pvl.LayerType, pvl))
                    .ToList() ?? new List<IPQPriceVolumeLayer?>();
        LayersOfType = bookLayers?.FirstOrDefault()?.LayerType ?? LayerType.PriceVolume;
    }

    public PQOrderBook(IOrderBook toClone)
    {
        BookSide     = toClone.BookSide;
        NameIdLookup = SourceOtherExistingOrNewPQNameIdNameLookup(toClone);
        IsLadder     = toClone.IsLadder;
        var size = toClone.Capacity;
        AllLayers = new List<IPQPriceVolumeLayer?>(size);
        Capacity  = toClone.Capacity;
        for (var i = 0; i < size; i++)
        {
            var sourceLayer = toClone[i];
            if (sourceLayer != null)
            {
                var pqLayer = (IPQPriceVolumeLayer?)LayerSelector.CreateExpectedImplementation(sourceLayer.LayerType, NameIdLookup, sourceLayer);
                AllLayers.Add(pqLayer);
            }
            else
            {
                AllLayers.Add(null);
            }
        }
    }

    private IPQOrderBookLayerFactorySelector LayerSelector { get; set; } = null!;

    protected string PQOrderBookToStringMembers =>
        $"{nameof(Capacity)}: {Capacity}, {nameof(Count)}: {Count}, {nameof(IsLadder)}: {IsLadder}, " +
        $"{nameof(AllLayers)}:[{string.Join(", ", AllLayers.Take(Count))}]";

    public LayerType LayersOfType { get; private set; } = LayerType.PriceVolume;

    public LayerFlags LayersSupportsLayerFlags => LayersOfType.SupportedLayerFlags();

    public bool IsLadder { get; private set; }

    public BookSide BookSide { get; }

    public IPQPriceVolumeLayer? this[int level]
    {
        get => level < AllLayers.Count && level >= 0 ? AllLayers[level] : null;
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
            LayerSelector         = new PQOrderBookLayerFactorySelector(nameIdLookupGenerator);

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
            NameIdLookup.HasUpdates = value;
        }
    }

    public override void StateReset()
    {
        for (var i = 0; i < AllLayers.Count; i++) AllLayers[i]?.StateReset();
        NameIdLookup.Clear();
        NameIdLookup.GetOrAddId(TraderPriceVolumeLayer.TraderCountOnlyName);
        base.StateReset();
    }

    public IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        for (var i = 0; i < AllLayers.Count; i++)
            if (this[i] is { } currentLayer)
                foreach (var layerFields in currentLayer.GetDeltaUpdateFields(snapShotTime,
                                                                              messageFlags, quotePublicationPrecisionSetting))
                {
                    PQFieldUpdate positionUpdate;
                    if (layerFields.Id is >= PQFieldKeys.FirstLayersRangeStart and < PQFieldKeys.SecondLayersRangeStart)
                    {
                        if (i < PQFieldKeys.SingleByteFieldIdMaxBookDepth)
                            positionUpdate = new PQFieldUpdate((ushort)(layerFields.Id + i), layerFields.Value
                                                             , layerFields.Flag);
                        else
                            positionUpdate =
                                new PQFieldUpdate
                                    ((ushort)(layerFields.Id + PQFieldKeys.FirstToSecondLayersOffset + i - PQFieldKeys.SingleByteFieldIdMaxBookDepth),
                                     layerFields.Value, (byte)(layerFields.Flag | PQFieldFlags.IsExtendedFieldId));
                    }
                    else
                    {
                        positionUpdate =
                            new PQFieldUpdate
                                ((ushort)(layerFields.Id + i - PQFieldKeys.SingleByteFieldIdMaxBookDepth),
                                 layerFields.Value, (byte)(layerFields.Flag | PQFieldFlags.IsExtendedFieldId));
                    }
                    yield return positionUpdate;
                }
    }

    public int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        if (pqFieldUpdate.Id is >= PQFieldKeys.FirstLayersRangeStart and < PQFieldKeys.FirstLayersRangeEnd)
        {
            var index              = (pqFieldUpdate.Id - PQFieldKeys.LayerPriceOffset) % PQFieldKeys.SingleByteFieldIdMaxBookDepth;
            var pqPriceVolumeLayer = this[index];
            return pqPriceVolumeLayer?.UpdateField(pqFieldUpdate) ?? -1;
        }
        if (pqFieldUpdate.Id is >= PQFieldKeys.FirstLayerExtendedRangeStart and < PQFieldKeys.FirstLayerExtendedRangeEnd)
        {
            var index              = (pqFieldUpdate.Id - PQFieldKeys.FirstLayerExtendedRangeStart) % PQFieldKeys.SingleByteFieldIdMaxBookDepth;
            var pqPriceVolumeLayer = this[index];
            return pqPriceVolumeLayer?.UpdateField(pqFieldUpdate) ?? -1;
        }
        if (pqFieldUpdate.Id is >= PQFieldKeys.SecondLayersRangeStart and < PQFieldKeys.ThirdLayersRangeStart)
        {
            var index = (pqFieldUpdate.Id - PQFieldKeys.SecondLayersRangeStart) %
                PQFieldKeys.SingleByteFieldIdMaxBookDepth + PQFieldKeys.SingleByteFieldIdMaxBookDepth;
            var pqPriceVolumeLayer = this[index];
            var remapFieldUpdate = new PQFieldUpdate((ushort)(pqFieldUpdate.Id - PQFieldKeys.FirstToSecondLayersOffset), pqFieldUpdate.Value
                                                   , pqFieldUpdate.Flag);
            return pqPriceVolumeLayer?.UpdateField(remapFieldUpdate) ?? -1;
        }
        if (pqFieldUpdate.Id is >= PQFieldKeys.SecondLayerExtendedRangeStart and < PQFieldKeys.SecondLayerExtendedRangeEnd)
        {
            var index = (pqFieldUpdate.Id - PQFieldKeys.SecondLayerExtendedRangeStart) % PQFieldKeys.SingleByteFieldIdMaxBookDepth +
                        PQFieldKeys.SingleByteFieldIdMaxBookDepth;

            var pqPriceVolumeLayer = this[index];
            var remapFieldUpdate = new PQFieldUpdate((ushort)(pqFieldUpdate.Id - PQFieldKeys.FirstToSecondLayersOffset), pqFieldUpdate.Value
                                                   , pqFieldUpdate.Flag);
            return pqPriceVolumeLayer?.UpdateField(remapFieldUpdate) ?? -1;
        }

        return -1;
    }


    public IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags)
    {
        if (AllLayers.Count <= 0) yield break;
        // All layers share same dictionary or should do anyway
        foreach (var stringUpdate in NameIdLookup.GetStringUpdates(snapShotTime, messageFlags))
            if (BookSide == BookSide.AskBook)
                yield return PQFieldStringUpdate.SetFieldFlag(stringUpdate, PQFieldFlags.IsAskSideFlag);
            else
                yield return stringUpdate;
    }

    public bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        if (stringUpdate.Field.Id == PQFieldKeys.LayerNameDictionaryUpsertCommand) return NameIdLookup.UpdateFieldString(stringUpdate);
        return false;
    }

    object ICloneable.Clone() => Clone();

    IOrderBook ICloneable<IOrderBook>.Clone() => Clone();

    IMutableOrderBook ICloneable<IMutableOrderBook>.Clone() => Clone();

    IMutableOrderBook IMutableOrderBook.Clone() => Clone();

    public override IPQOrderBook Clone() => (IPQOrderBook?)Recycler?.Borrow<PQOrderBook>().CopyFrom(this) ?? new PQOrderBook(this);

    public override IOrderBook CopyFrom(IOrderBook source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is PQOrderBook sourcePqOrderBook) NameIdLookup.CopyFrom(sourcePqOrderBook.NameIdLookup, copyMergeFlags);
        LayersOfType = source.LayersOfType;
        IsLadder     = source.IsLadder;

        for (var i = 0; i < source.Count; i++)
        {
            var sourceLayer      = source[i];
            var destinationLayer = this[i];

            if (i < AllLayers.Count)
                AllLayers[i] = LayerSelector.UpgradeExistingLayer(destinationLayer, NameIdLookup, LayersOfType, sourceLayer, copyMergeFlags);
            else
                AllLayers.Add(LayerSelector.CreateExpectedImplementation(LayersOfType, NameIdLookup, sourceLayer, copyMergeFlags));
            if (sourceLayer is { IsEmpty: false }) continue;
            if (destinationLayer is IMutablePriceVolumeLayer mutableDestinationLayer) mutableDestinationLayer.IsEmpty = true;
        }
        for (var i = source.Count; i < source.Capacity; i++)
            AllLayers.Add(LayerSelector.CreateExpectedImplementation(LayersOfType, NameIdLookup, null, copyMergeFlags));

        for (var i = source.Count; i < AllLayers.Count; i++)
            if (AllLayers[i] is IMutablePriceVolumeLayer mutablePvl)
                mutablePvl.IsEmpty = true;
        return this;
    }

    public int AppendEntryAtEnd()
    {
        var index = AllLayers.Count;
        AllLayers.Add(LayerSelector.CreateExpectedImplementation(LayersOfType, NameIdLookup));
        return index;
    }

    public void EnsureRelatedItemsAreConfigured(LayerFlags layerFlags, IPQNameIdLookupGenerator? otherNameIdLookupGenerator)
    {
        if (otherNameIdLookupGenerator != null)
        {
            NameIdLookup.CopyFrom(otherNameIdLookupGenerator);
            LayersOfType = layerFlags.MostCompactLayerType();
        }
    }

    public void EnsureRelatedItemsAreConfigured(IPQSourceTickerInfo? referenceInstance)
    {
        if (referenceInstance is { NameIdLookup: not null }) NameIdLookup.CopyFrom(referenceInstance.NameIdLookup);
        LayersOfType = referenceInstance?.LayerFlags.MostCompactLayerType() ?? LayersOfType;
        IsLadder     = referenceInstance?.LayerFlags.HasLadder() ?? false;
        int maxBookDepth = Math.Max((byte)1, Math.Min(referenceInstance!.MaximumPublishedLayers, PQFieldKeys.SingleByteFieldIdMaxBookDepth));

        var layerFactory = LayerSelector.FindForLayerFlags(referenceInstance);

        for (var i = 0; i < maxBookDepth; i++)
            if (i >= AllLayers.Count || AllLayers[i] == null)
                AllLayers.Add(layerFactory.CreateNewLayer());
            else if (i < AllLayers.Count && AllLayers[i] is { } currentLayer
                                         && !LayerSelector.OriginalCanWhollyContain(LayersSupportsLayerFlags
                                                                                  , currentLayer.SupportsLayerFlags))
                AllLayers[i] = layerFactory.UpgradeLayer(currentLayer);
        for (var i = maxBookDepth; i < AllLayers.Count; i++) AllLayers.RemoveAt(i);
    }

    public virtual bool AreEquivalent(IOrderBook? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;

        var isLadderSame      = IsLadder == other.IsLadder;
        var deepestPricedSame = Count == other.Count;
        var bookLayersSame = exactTypes
            ? AllLayers.SequenceEqual(other)
            : AllLayers.Zip(other, (thisLayer, otherLayer) => new { thisLayer, otherLayer })
                       .All(joined => joined.thisLayer != null && joined.thisLayer.AreEquivalent(joined.otherLayer));

        var deepestPossibleSame                                              = true;
        if (other is IMutableOrderBook mutableOrderBook) deepestPossibleSame = Capacity == mutableOrderBook.Capacity;

        var allAreSame = isLadderSame && deepestPossibleSame && deepestPricedSame && bookLayersSame;
        return allAreSame;
    }

    public IEnumerator<IPQPriceVolumeLayer> GetEnumerator() => AllLayers.Take(Count).GetEnumerator()!;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<IPriceVolumeLayer> IEnumerable<IPriceVolumeLayer>.GetEnumerator() => GetEnumerator();

    private IPQNameIdLookupGenerator SourceOtherExistingOrNewPQNameIdNameLookup(IEnumerable<IPriceVolumeLayer>? source)
    {
        IPQNameIdLookupGenerator thisLayDict;
        if (source is IPQOrderBook { NameIdLookup: not null } pqOrderBook)
            thisLayDict = InitializeNewIdLookupGenerator(pqOrderBook.NameIdLookup);
        else
            thisLayDict = InitializeNewIdLookupGenerator(
                                                         source?.OfType<ISupportsPQNameIdLookupGenerator>()
                                                               ?.Where(pvl => pvl is { NameIdLookup: not null })
                                                               ?.Select(pvl => pvl.NameIdLookup)
                                                               ?.FirstOrDefault());

        return thisLayDict;
    }

    public IPQNameIdLookupGenerator InitializeNewIdLookupGenerator(IPQNameIdLookupGenerator? optionalExisting = null)
    {
        IPQNameIdLookupGenerator thisBookNameIdLookupGenerator = optionalExisting != null
            ? new PQNameIdLookupGenerator(optionalExisting, PQFieldKeys.LayerNameDictionaryUpsertCommand)
            : new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand);
        thisBookNameIdLookupGenerator.GetOrAddId(TraderPriceVolumeLayer.TraderCountOnlyName);
        return thisBookNameIdLookupGenerator;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IOrderBook, true);

    public override int GetHashCode() => AllLayers != null ? AllLayers.GetHashCode() : 0;

    public override string ToString() => $"{GetType().Name}({PQOrderBookToStringMembers})";
}
