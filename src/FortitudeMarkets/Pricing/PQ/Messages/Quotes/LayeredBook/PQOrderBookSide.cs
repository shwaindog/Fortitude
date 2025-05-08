// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Quotes.TickerInfo;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;

public interface IPQOrderBookSide : IMutableOrderBookSide, IPQSupportsFieldUpdates<IOrderBookSide>,
    IPQSupportsStringUpdates<IOrderBookSide>, IEnumerable<IPQPriceVolumeLayer>, ICloneable<IPQOrderBookSide>,
    IRelatedItems<LayerFlags, ushort>, IRelatedItems<INameIdLookupGenerator>,
    ISupportsPQNameIdLookupGenerator

{
    new IPQPriceVolumeLayer? this[int level] { get; set; }
    IReadOnlyList<IPQPriceVolumeLayer> AllLayers { get; }

    new bool HasUpdates { get; set; }

    new IPQNameIdLookupGenerator NameIdLookup { get; set; }

    new IPQMarketAggregate? OpenInterestSide { get; set; }

    bool IsDailyTickUpdateCountUpdated { get; set; }

    new IPQOrderBookSide Clone();

    new IEnumerator<IPQPriceVolumeLayer> GetEnumerator();
}

public class PQOrderBookSide : ReusableObject<IOrderBookSide>, IPQOrderBookSide
{
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQOrderBookSide));

    private IList<IPQPriceVolumeLayer> allLayers;

    private IPQNameIdLookupGenerator nameIdLookupGenerator;

    protected uint                NumOfUpdates = uint.MaxValue;
    private   IPQMarketAggregate? pqOpenInterestSide;

    protected OrderBookUpdatedFlags UpdatedFlags;

    private LayerFlags layerFlags      = LayerFlagsExtensions.PriceVolumeLayerFlags;
    private ushort     maxPublishDepth = 1;
    private uint       dailyTickUpdateCount;

    public PQOrderBookSide()
    {
        BookSide              = BookSide.Unknown;
        nameIdLookupGenerator = InitializeNewIdLookupGenerator();
        LayerSelector         = new PQOrderBookLayerFactorySelector(nameIdLookupGenerator);
        allLayers             = [new PQPriceVolumeLayer()];

        if (GetType() == typeof(PQOrderBookSide)) NumOfUpdates = 0;
    }

    public PQOrderBookSide
    (BookSide bookSide, LayerType layerType = LayerType.PriceVolume,
        int numBookLayers = SourceTickerInfo.DefaultMaximumPublishedLayers, bool isLadder = false,
        IPQNameIdLookupGenerator? nameIdLookup = null)
    {
        BookSide              =  bookSide;
        LayerSupportedFlags   =  layerType.SupportedLayerFlags();
        LayerSupportedFlags   |= LayerSupportedType.SupportedLayerFlags();
        IsLadder              =  isLadder;
        nameIdLookupGenerator =  nameIdLookup ?? InitializeNewIdLookupGenerator();
        LayerSelector         =  new PQOrderBookLayerFactorySelector(nameIdLookupGenerator);
        MaxPublishDepth       =  (ushort)numBookLayers;
        allLayers             =  new List<IPQPriceVolumeLayer>(MaxPublishDepth);
        for (var i = 0; i < MaxPublishDepth; i++)
        {
            var pqLayer = LayerSelector.CreateExpectedImplementation(LayerSupportedType, NameIdLookup);
            allLayers.Add(pqLayer);
        }

        if (GetType() == typeof(PQOrderBookSide)) NumOfUpdates = 0;
    }

    public PQOrderBookSide
        (BookSide bookSide, ISourceTickerInfo srcTickerInfo, IPQNameIdLookupGenerator? nameIdLookup = null)
    {
        BookSide            =  bookSide;
        LayerSupportedFlags =  srcTickerInfo.LayerFlags;
        layerFlags          |= LayerSupportedType.SupportedLayerFlags();
        MaxPublishDepth     =  srcTickerInfo.MaximumPublishedLayers;
        nameIdLookupGenerator = nameIdLookup ??
                                (srcTickerInfo is IPQSourceTickerInfo pqSrcTkrInfo
                                    ? InitializeNewIdLookupGenerator(pqSrcTkrInfo.NameIdLookup)
                                    : InitializeNewIdLookupGenerator());
        LayerSelector = new PQOrderBookLayerFactorySelector(nameIdLookupGenerator);

        allLayers = new List<IPQPriceVolumeLayer>(MaxPublishDepth);
        for (var i = 0; i < MaxPublishDepth; i++)
        {
            var pqLayer = LayerSelector.CreateExpectedImplementation(LayerSupportedType, NameIdLookup);
            allLayers.Add(pqLayer);
        }
        EnsureRelatedItemsAreConfigured(nameIdLookup);

        if (GetType() == typeof(PQOrderBookSide)) NumOfUpdates = 0;
    }

    public PQOrderBookSide
    (BookSide bookSide, IEnumerable<IPriceVolumeLayer>? bookLayers = null, bool isLadder = false,
        IPQNameIdLookupGenerator? nameIdLookup = null)
    {
        BookSide            = bookSide;
        LayerSupportedFlags = bookLayers?.FirstOrDefault()?.SupportsLayerFlags ?? LayerFlagsExtensions.PriceVolumeLayerFlags;
        IsLadder            = isLadder;
        MaxPublishDepth     = (ushort)(bookLayers?.Count() ?? 1);

        nameIdLookupGenerator = nameIdLookup ?? SourceOtherExistingOrNewPQNameIdNameLookup(bookLayers);
        LayerSelector         = new PQOrderBookLayerFactorySelector(nameIdLookupGenerator);
        allLayers =
            (bookLayers?.Select(pvl => LayerSelector.UpgradeExistingLayer(pvl, pvl.LayerType, pvl))
                       .OfType<IPQPriceVolumeLayer>()
                       .ToList() ?? [LayerSelector.CreateExpectedImplementation(LayerSupportedType, NameIdLookup)]
            );

        if (GetType() == typeof(PQOrderBookSide)) NumOfUpdates = 0;
    }

    public PQOrderBookSide(IOrderBookSide toClone, IPQNameIdLookupGenerator? nameIdLookup = null)
    {
        BookSide = toClone.BookSide;

        LayerSupportedFlags =  toClone.LayerSupportedFlags;
        layerFlags          |= LayerSupportedType.SupportedLayerFlags();

        if (toClone.HasNonEmptyOpenInterest)
        {
            pqOpenInterestSide = new PQMarketAggregate(toClone.MarketAggregateSide);
        }

        MaxPublishDepth       = toClone.MaxPublishDepth;
        DailyTickUpdateCount  = toClone.DailyTickUpdateCount;
        nameIdLookupGenerator = InitializeNewIdLookupGenerator(nameIdLookup, toClone);
        LayerSelector         = new PQOrderBookLayerFactorySelector(nameIdLookupGenerator);

        var size = toClone.Capacity;
        allLayers = new List<IPQPriceVolumeLayer>(size);
        Capacity  = toClone.Capacity;
        for (var i = 0; i < size; i++)
        {
            var sourceLayer = toClone[i];
            if (sourceLayer != null)
            {
                var pqLayer = LayerSelector.CreateExpectedImplementation(sourceLayer.LayerType, NameIdLookup, sourceLayer);
                allLayers.Add(pqLayer);
            }
            else
            {
                var pqLayer = LayerSelector.CreateExpectedImplementation(LayerSupportedType, NameIdLookup);
                pqLayer.IsEmpty = true;
                allLayers.Add(pqLayer);
            }
        }

        if (GetType() == typeof(PQOrderBookSide)) NumOfUpdates = 0;
    }

    public ushort MaxPublishDepth
    {
        get => maxPublishDepth;
        private set => maxPublishDepth = Math.Max((byte)1, Math.Min(value, PQQuoteFieldsExtensions.TwoByteFieldIdMaxBookDepth));
    }

    private IPQOrderBookLayerFactorySelector LayerSelector { get; set; }

    protected string PQOrderBookSideToStringMembers =>
        $"{nameof(Capacity)}: {Capacity}, {nameof(Count)}: {Count}, {nameof(IsLadder)}: {IsLadder}, " +
        $"{nameof(OpenInterestSide)}: {OpenInterestSide}, {nameof(DailyTickUpdateCount)}: {DailyTickUpdateCount}, " +
        $"{nameof(AllLayers)}:[{string.Join(", ", AllLayers.Take(Count))}]";

    INameIdLookup IHasNameIdLookup.NameIdLookup => NameIdLookup;

    public LayerType LayerSupportedType => LayerSupportedFlags.MostCompactLayerType();

    public LayerFlags LayerSupportedFlags
    {
        get => layerFlags;
        set => layerFlags = layerFlags.Unset(LayerFlags.Ladder) | value;
    }

    public bool IsLadder
    {
        get => layerFlags.HasLadder();
        set => LayerSupportedFlags = value ? LayerFlags.Ladder : LayerFlags.None;
    }

    public BookSide BookSide { get; }

    public IPQPriceVolumeLayer? this[int level]
    {
        get => level < AllLayers.Count && level >= 0 ? AllLayers[level] : null;
        set
        {
            if (value == null)
            {
                if (level < AllLayers.Count)
                {
                    AllLayers[level].IsEmpty = true;
                }
            }
            else
            {
                if (value is ISupportsPQNameIdLookupGenerator setNameIdLookup)
                {
                    setNameIdLookup.NameIdLookup = NameIdLookup;
                }
                allLayers[level] = value;
            }
        }
    }

    IPriceVolumeLayer? IOrderBookSide.this[int level] => this[level];

    IMutablePriceVolumeLayer? IMutableOrderBookSide.this[int level]
    {
        get => this[level];
        set => this[level] = value as IPQPriceVolumeLayer;
    }

    public IReadOnlyList<IPQPriceVolumeLayer> AllLayers => allLayers.AsReadOnly();

    public IPQNameIdLookupGenerator NameIdLookup
    {
        get => nameIdLookupGenerator;
        set
        {
            if (nameIdLookupGenerator == value) return;
            nameIdLookupGenerator = value;
            LayerSelector         = new PQOrderBookLayerFactorySelector(nameIdLookupGenerator);

            foreach (var pqPriceVolumeLayer in allLayers.OfType<ISupportsPQNameIdLookupGenerator>()) pqPriceVolumeLayer.NameIdLookup = value;
        }
    }

    public int Capacity
    {
        get => Math.Min(MaxPublishDepth, AllLayers.Count);
        set
        {
            if (value > PQQuoteFieldsExtensions.SingleByteFieldIdMaxBookDepth)
                throw new ArgumentException("Expected PQOrderBook Capacity to be less than or equal to " +
                                            PQQuoteFieldsExtensions.SingleByteFieldIdMaxBookDepth);

            for (var i = AllLayers.Count; i < value && AllLayers.Count > 0; i++)
            {
                var clonedFirstLayer = AllLayers[0].Clone();
                clonedFirstLayer.StateReset();
                allLayers.Add(clonedFirstLayer);
            }
        }
    }

    public int Count
    {
        get
        {
            for (var i = Math.Min(MaxPublishDepth, AllLayers.Count - 1); i >= 0; i--)
            {
                var layerAtLevel = AllLayers[i];
                if ((layerAtLevel.Price) > 0) return i + 1;
            }

            return 0;
        }
    }

    public bool HasUpdates
    {
        get
        {
            return UpdatedFlags != OrderBookUpdatedFlags.None
                || (HasNonEmptyOpenInterest 
                 && pqOpenInterestSide is {DataSource: not(MarketDataSource.None or MarketDataSource.Published), HasUpdates: true })
                || AllLayers.Any(pqpvl => pqpvl.HasUpdates);
        }
        set
        {
            foreach (var pqPvLayer in AllLayers) pqPvLayer.HasUpdates = value;
            NameIdLookup.HasUpdates = value;
            if (pqOpenInterestSide != null)
            {
                pqOpenInterestSide.HasUpdates = value;
            }
            if (value) return;
            UpdatedFlags = OrderBookUpdatedFlags.None;
        }
    }

    public bool HasNonEmptyOpenInterest
    {
        get => pqOpenInterestSide is { IsEmpty: false};
        set
        {
            if (value) return;
            if (pqOpenInterestSide != null)
            {
                pqOpenInterestSide.IsEmpty = true;
            }
        }
    }

    IMarketAggregate IOrderBookSide.MarketAggregateSide => OpenInterestSide!;
    IMutableMarketAggregate? IMutableOrderBookSide.OpenInterestSide
    {
        get => OpenInterestSide;
        set => OpenInterestSide = (IPQMarketAggregate?)value;
    }

    public IPQMarketAggregate? OpenInterestSide
    {
        get
        {
            if (HasNonEmptyOpenInterest && pqOpenInterestSide is {DataSource: not (MarketDataSource.None or MarketDataSource.Published) })
                return pqOpenInterestSide;

            var vwapResult = this.CalculateVwap();

            pqOpenInterestSide ??= new PQMarketAggregate();
            pqOpenInterestSide.DataSource =   MarketDataSource.Published;
            pqOpenInterestSide.UpdateTime =   DateTime.Now;
            pqOpenInterestSide.Volume     =   vwapResult.VolumeAchieved;
            pqOpenInterestSide.Vwap       =   vwapResult.AchievedVwap;
            return pqOpenInterestSide;
        }
        set
        {
            if (value != null)
            {
                pqOpenInterestSide ??= new PQMarketAggregate();

                pqOpenInterestSide.DataSource = value.DataSource;
                pqOpenInterestSide.UpdateTime = value.UpdateTime;
                pqOpenInterestSide.Volume     = value.Volume;
                pqOpenInterestSide.Vwap       = value.Vwap;
            }
            else if (pqOpenInterestSide != null)
            {
                pqOpenInterestSide.IsEmpty = true;
            }
        }
    }

    public uint DailyTickUpdateCount
    {
        get => dailyTickUpdateCount;
        set
        {
            IsDailyTickUpdateCountUpdated |= value != dailyTickUpdateCount || NumOfUpdates == 0;
            dailyTickUpdateCount          =  value;
        }
    }

    public bool IsDailyTickUpdateCountUpdated
    {
        get => (UpdatedFlags & OrderBookUpdatedFlags.IsDailyTickCountUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderBookUpdatedFlags.IsDailyTickCountUpdated;

            else if (IsDailyTickUpdateCountUpdated) UpdatedFlags ^= OrderBookUpdatedFlags.IsDailyTickCountUpdated;
        }
    }

    public uint UpdateCount => NumOfUpdates;

    public void UpdateComplete()
    {
        foreach (var pqPriceVolumeLayer in AllLayers) pqPriceVolumeLayer.UpdateComplete();
        NameIdLookup.UpdateComplete();
        pqOpenInterestSide?.UpdateComplete();
        if (HasUpdates)
        {
            NumOfUpdates++;
            HasUpdates = false;
        }
    }

    public IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;
        if (!updatedOnly || IsDailyTickUpdateCountUpdated)
        {
            yield return new PQFieldUpdate(PQQuoteFields.DailySidedTickCount, DailyTickUpdateCount);
        }
        if (pqOpenInterestSide != null)
        {
            foreach (var oiUpdates in pqOpenInterestSide.GetDeltaUpdateFields(snapShotTime, messageFlags, quotePublicationPrecisionSetting))
            {
                yield return oiUpdates.WithFieldId(PQQuoteFields.OpenInterestSided);
            }
        }
        for (var i = 0; i < AllLayers.Count; i++)
            if (this[i] is { } currentLayer)
                foreach (var layerFields in currentLayer.GetDeltaUpdateFields(snapShotTime,
                                                                              messageFlags, quotePublicationPrecisionSetting))
                {
                    var positionUpdate = layerFields.AtDepth((ushort)i);
                    yield return positionUpdate;
                }
    }

    public int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        if (pqFieldUpdate.Id == PQQuoteFields.DailySidedTickCount)
        {
            IsDailyTickUpdateCountUpdated = true;

            DailyTickUpdateCount = pqFieldUpdate.Payload;
            return 0;
        }
        if (pqFieldUpdate.Id is PQQuoteFields.OpenInterestSided)
        {
            pqOpenInterestSide ??= new PQMarketAggregate();
            return pqOpenInterestSide.UpdateField(pqFieldUpdate);
        }
        if (pqFieldUpdate.Id is >= PQQuoteFields.Price and < PQQuoteFields.AllLayersRangeEnd)
        {
            var index              = pqFieldUpdate.DepthIndex();
            var pqPriceVolumeLayer = this[index];
            return pqPriceVolumeLayer?.UpdateField(pqFieldUpdate) ?? -1;
        }
        return -1;
    }

    object ICloneable.Clone() => Clone();

    IOrderBookSide ICloneable<IOrderBookSide>.Clone() => Clone();

    IMutableOrderBookSide ICloneable<IMutableOrderBookSide>.Clone() => Clone();

    IMutableOrderBookSide IMutableOrderBookSide.Clone() => Clone();

    public override IPQOrderBookSide Clone() => (IPQOrderBookSide?)Recycler?.Borrow<PQOrderBookSide>().CopyFrom(this) ?? new PQOrderBookSide(this);

    public override IOrderBookSide CopyFrom(IOrderBookSide source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (!copyMergeFlags.HasSkipReferenceLookups() && source is PQOrderBookSide sourcePqOrderBook)
            NameIdLookup.CopyFrom(sourcePqOrderBook.NameIdLookup, copyMergeFlags);
        copyMergeFlags = copyMergeFlags.AddSkipReferenceLookups();
        if (source.HasNonEmptyOpenInterest)
        {
            pqOpenInterestSide ??= new PQMarketAggregate();
            pqOpenInterestSide.CopyFrom(source.MarketAggregateSide, copyMergeFlags);
        }
        else if (pqOpenInterestSide != null)
        {
            pqOpenInterestSide.IsEmpty = true;
        }
        DailyTickUpdateCount = source.DailyTickUpdateCount;
        LayerSupportedFlags  = source.LayerSupportedFlags;
        MaxPublishDepth      = source.MaxPublishDepth;
        var originalCount = Count;

        var allSourceLayers = source.Capacity;
        for (var i = 0; i < allSourceLayers; i++)
        {
            var sourceLayer = source[i];
            if (sourceLayer is null or { IsEmpty: true } && i >= originalCount) continue;
            var destinationLayer = this[i];

            if (i < AllLayers.Count)
                allLayers[i] = LayerSelector.UpgradeExistingLayer(destinationLayer, NameIdLookup, LayerSupportedType, sourceLayer, copyMergeFlags);
            else
                allLayers.Add(LayerSelector.CreateExpectedImplementation(LayerSupportedType, NameIdLookup, sourceLayer, copyMergeFlags));
            if (sourceLayer is { IsEmpty: false }) continue;
            if (destinationLayer is IMutablePriceVolumeLayer mutableDestinationLayer) mutableDestinationLayer.IsEmpty = true;
        }

        for (var i = source.Count; i < AllLayers.Count; i++)
            if (AllLayers[i] is IMutablePriceVolumeLayer mutablePvl)
                mutablePvl.IsEmpty = true;
        return this;
    }

    public int AppendEntryAtEnd()
    {
        var index = AllLayers.Count;
        allLayers.Add(LayerSelector.CreateExpectedImplementation(LayerSupportedType, NameIdLookup));
        return index;
    }

    public void EnsureRelatedItemsAreConfigured(INameIdLookupGenerator? otherNameIdLookupGenerator)
    {
        if (otherNameIdLookupGenerator != null && !ReferenceEquals(NameIdLookup, nameIdLookupGenerator))
        {
            NameIdLookup.CopyFrom(otherNameIdLookupGenerator);
            if (NameIdLookup.Count != otherNameIdLookupGenerator.Count) NameIdLookup.CopyFrom(otherNameIdLookupGenerator, CopyMergeFlags.FullReplace);
        }
    }

    public void EnsureRelatedItemsAreConfigured(LayerFlags newLayerFlags, ushort bookDepth)
    {
        LayerSupportedFlags = newLayerFlags;
        MaxPublishDepth     = bookDepth;

        var layerFactory = LayerSelector.FindForLayerFlags(LayerSupportedFlags);

        for (var i = 0; i < MaxPublishDepth; i++)
            if (i >= AllLayers.Count)
                allLayers.Add(layerFactory.CreateNewLayer());
            else if (i < AllLayers.Count && AllLayers[i] is { } currentLayer
                                         && !LayerSelector.OriginalCanWhollyContain(LayerSupportedFlags
                                                                                  , currentLayer.SupportsLayerFlags))
                allLayers[i] = layerFactory.UpgradeLayer(currentLayer);
        for (var i = MaxPublishDepth; i < AllLayers.Count; i++) allLayers.RemoveAt(i);
    }

    public virtual bool AreEquivalent(IOrderBookSide? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;

        var layerFlagsSame    = LayerSupportedFlags == other.LayerSupportedFlags;
        var deepestPricedSame = Count == other.Count;
        var maxPubDepthSame   = MaxPublishDepth == other.MaxPublishDepth;
        var openInterestSame  = HasNonEmptyOpenInterest == other.HasNonEmptyOpenInterest;
        if (openInterestSame && other.HasNonEmptyOpenInterest && HasNonEmptyOpenInterest)
        {
            openInterestSame = pqOpenInterestSide?.AreEquivalent(other.MarketAggregateSide, exactTypes) ?? false;
        }
        var bookLayersSame = true;
        for (int i = 0; i < Count; i++)
        {
            var localLayer  = this[i];
            var otherLayer  = other[i];
            var layerIsSame = localLayer?.AreEquivalent(otherLayer, exactTypes) ?? otherLayer == null;
            bookLayersSame &= layerIsSame;
        }

        var deepestPossibleSame = true;

        if (other is IMutableOrderBookSide mutableOrderBook) deepestPossibleSame = Capacity == mutableOrderBook.Capacity;

        var allAreSame = layerFlagsSame && deepestPossibleSame && deepestPricedSame
                      && maxPubDepthSame && bookLayersSame && openInterestSame;
        return allAreSame;
    }

    public IEnumerator<IPQPriceVolumeLayer> GetEnumerator() => AllLayers.Take(Count).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<IPriceVolumeLayer> IEnumerable<IPriceVolumeLayer>.GetEnumerator() => GetEnumerator();

    public override void StateReset()
    {
        for (var i = 0; i < AllLayers.Count; i++) AllLayers[i].StateReset();
        DailyTickUpdateCount = 0;
        NameIdLookup.Clear();
        pqOpenInterestSide?.StateReset();
        NumOfUpdates = 0;
        UpdatedFlags = OrderBookUpdatedFlags.None;
        base.StateReset();
    }

    public IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags)
    {
        if (AllLayers.Count <= 0) return [];
        // All layers share same dictionary or should do anyway
        return NameIdLookup.GetStringUpdates(snapShotTime, messageFlags);
    }

    public bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        // All layers share same dictionary or should do anyway
        if (stringUpdate.Field.Id != PQQuoteFields.LayerNameDictionaryUpsertCommand) return false;
        return NameIdLookup.UpdateFieldString(stringUpdate);
    }

    private IPQNameIdLookupGenerator SourceOtherExistingOrNewPQNameIdNameLookup(IEnumerable<IPriceVolumeLayer>? source)
    {
        IPQNameIdLookupGenerator thisLayDict;
        if (source is IPQOrderBookSide pqOrderBook)
            thisLayDict = InitializeNewIdLookupGenerator(pqOrderBook.NameIdLookup);
        else
            thisLayDict = InitializeNewIdLookupGenerator
                (
                 source?.OfType<ISupportsPQNameIdLookupGenerator>()
                       .Select(pvl => pvl.NameIdLookup)
                       .FirstOrDefault());

        return thisLayDict;
    }

    public IPQNameIdLookupGenerator InitializeNewIdLookupGenerator(IPQNameIdLookupGenerator? optionalExisting = null, IOrderBookSide? clone = null)
    {
        IPQNameIdLookupGenerator thisBookNameIdLookupGenerator = optionalExisting != null
            ? new PQNameIdLookupGenerator(optionalExisting, PQQuoteFields.LayerNameDictionaryUpsertCommand)
            : clone is IPQOrderBookSide pqClone
                ? new PQNameIdLookupGenerator(pqClone.NameIdLookup, PQQuoteFields.LayerNameDictionaryUpsertCommand)
                : new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand);
        return thisBookNameIdLookupGenerator;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IOrderBookSide, true);

    public override int GetHashCode() => AllLayers.GetHashCode();

    public override string ToString() => $"{GetType().Name}({PQOrderBookSideToStringMembers})";
}
