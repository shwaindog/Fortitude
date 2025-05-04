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
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;

[Flags]
public enum OrderBookSideUpdatedFlags : byte
{
    None                             = 0
  , SourceOpenInterestVolumeUpdated  = 1
  , SourceOpenInterestVwapUpdated    = 2
  , AdapterOpenInterestVolumeUpdated = 4
  , AdapterOpenInterestVwapUpdated   = 8
}

public interface IPQOrderBookSide : IMutableOrderBookSide, IPQSupportsFieldUpdates<IOrderBookSide>,
    IPQSupportsStringUpdates<IOrderBookSide>, IEnumerable<IPQPriceVolumeLayer>, ICloneable<IPQOrderBookSide>,
    IRelatedItems<LayerFlags, ushort>, IRelatedItems<INameIdLookupGenerator>,
    ISupportsPQNameIdLookupGenerator

{
    new IPQPriceVolumeLayer? this[int level] { get; set; }
    IList<IPQPriceVolumeLayer?> AllLayers { get; set; }

    new bool HasUpdates { get; set; }

    new IPQNameIdLookupGenerator NameIdLookup { get; set; }

    decimal? SourceOpenInterestVolume  { get; set; }
    decimal? SourceOpenInterestVwap    { get; set; }
    decimal  AdapterOpenInterestVolume { get; set; }
    decimal  AdapterOpenInterestVwap   { get; set; }

    bool IsSourceOpenInterestVolumeUpdated  { get; set; }
    bool IsSourceOpenInterestVwapUpdated    { get; set; }
    bool IsAdapterOpenInterestVolumeUpdated { get; set; }
    bool IsAdapterOpenInterestVwapUpdated   { get; set; }

    new IPQOrderBookSide                 Clone();
    new IEnumerator<IPQPriceVolumeLayer> GetEnumerator();
}

public class PQOrderBookSide : ReusableObject<IOrderBookSide>, IPQOrderBookSide
{
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQOrderBookSide));

    private decimal adapterOpenInterestVolume;
    private decimal adapterOpenInterestVwap;

    private IList<IPQPriceVolumeLayer> allLayers;

    private IPQNameIdLookupGenerator nameIdLookupGenerator = null!;

    protected uint     NumOfUpdates = uint.MaxValue;
    private   decimal? sourceOpenInterestVolume;
    private   decimal? sourceOpenInterestVwap;

    protected OrderBookSideUpdatedFlags UpdatedFlags;

    private LayerFlags layerFlags      = LayerFlagsExtensions.PriceVolumeLayerFlags;
    private ushort     maxPublishDepth = 1;

    public PQOrderBookSide()
    {
        BookSide     = BookSide.Unknown;
        NameIdLookup = InitializeNewIdLookupGenerator();
        allLayers    = [new PQPriceVolumeLayer()];

        if (GetType() == typeof(PQOrderBookSide)) NumOfUpdates = 0;
    }

    public PQOrderBookSide
    (BookSide bookSide, LayerType layerType = LayerType.PriceVolume,
        int numBookLayers = SourceTickerInfo.DefaultMaximumPublishedLayers, bool isLadder = false,
        IPQNameIdLookupGenerator? nameIdLookupGenerator = null)
    {
        BookSide                 =  bookSide;
        LayerSupportedFlags =  layerType.SupportedLayerFlags();
        LayerSupportedFlags |= LayerSupportedType.SupportedLayerFlags();
        IsLadder                 =  isLadder;
        NameIdLookup             =  nameIdLookupGenerator ?? InitializeNewIdLookupGenerator();
        MaxPublishDepth          =  (ushort)numBookLayers;
        allLayers                =  new List<IPQPriceVolumeLayer>(MaxPublishDepth);
        for (var i = 0; i < MaxPublishDepth; i++)
        {
            var pqLayer = LayerSelector.CreateExpectedImplementation(LayerSupportedType, NameIdLookup);
            allLayers.Add(pqLayer);
        }

        if (GetType() == typeof(PQOrderBookSide)) NumOfUpdates = 0;
    }

    public PQOrderBookSide
        (BookSide bookSide, ISourceTickerInfo srcTickerInfo, IPQNameIdLookupGenerator? nameIdLookupGenerator = null)
    {
        BookSide                 =  bookSide;
        LayerSupportedFlags =  srcTickerInfo.LayerFlags;
        layerFlags               |= LayerSupportedType.SupportedLayerFlags();
        MaxPublishDepth          =  srcTickerInfo?.MaximumPublishedLayers ?? 1;
        NameIdLookup = nameIdLookupGenerator ??
                       (srcTickerInfo is IPQSourceTickerInfo pqSrcTkrInfo
                           ? InitializeNewIdLookupGenerator(pqSrcTkrInfo.NameIdLookup)
                           : InitializeNewIdLookupGenerator());
        
        allLayers = new List<IPQPriceVolumeLayer>(MaxPublishDepth);
        for (var i = 0; i < MaxPublishDepth; i++)
        {
            var pqLayer = LayerSelector.CreateExpectedImplementation(LayerSupportedType, NameIdLookup);
            AllLayers.Add(pqLayer);
        }
        EnsureRelatedItemsAreConfigured(nameIdLookupGenerator);

        if (GetType() == typeof(PQOrderBookSide)) NumOfUpdates = 0;
    }

    public PQOrderBookSide
    (BookSide bookSide, IEnumerable<IPriceVolumeLayer>? bookLayers = null, bool isLadder = false,
        IPQNameIdLookupGenerator? nameIdLookupGenerator = null)
    {
        BookSide                 = bookSide;
        LayerSupportedFlags = bookLayers?.FirstOrDefault()?.SupportsLayerFlags ?? LayerFlagsExtensions.PriceVolumeLayerFlags;
        IsLadder                 = isLadder;
        MaxPublishDepth          = (ushort)(bookLayers?.Count() ?? 1);

        NameIdLookup = nameIdLookupGenerator ?? SourceOtherExistingOrNewPQNameIdNameLookup(bookLayers);
        allLayers =
            (bookLayers?.Select(pvl => LayerSelector.UpgradeExistingLayer(pvl, pvl.LayerType, pvl))
                       .OfType<IPQPriceVolumeLayer>()
                       .ToList() ?? [LayerSelector.CreateExpectedImplementation(LayerSupportedType, NameIdLookup)]
            );

        if (GetType() == typeof(PQOrderBookSide)) NumOfUpdates = 0;
    }

    public PQOrderBookSide(IOrderBookSide toClone, IPQNameIdLookupGenerator? nameIdLookupGenerator = null)
    {
        BookSide = toClone.BookSide;

        LayerSupportedFlags =  toClone.LayerSupportedFlags;
        layerFlags               |= LayerSupportedType.SupportedLayerFlags();

        MaxPublishDepth = toClone.MaxPublishDepth;
        NameIdLookup    = InitializeNewIdLookupGenerator(nameIdLookupGenerator, toClone);

        var size = toClone.Capacity;
        allLayers = new List<IPQPriceVolumeLayer>(size);
        Capacity  = toClone.Capacity;
        for (var i = 0; i < size; i++)
        {
            var sourceLayer = toClone[i];
            if (sourceLayer != null)
            {
                var pqLayer = LayerSelector.CreateExpectedImplementation(sourceLayer.LayerType, NameIdLookup, sourceLayer);
                AllLayers.Add(pqLayer);
            }
            else
            {
                var pqLayer = LayerSelector.CreateExpectedImplementation(LayerSupportedType, NameIdLookup);
                pqLayer.IsEmpty = true;
                AllLayers.Add(pqLayer);
            }
        }

        if (GetType() == typeof(PQOrderBookSide)) NumOfUpdates = 0;
    }

    public ushort MaxPublishDepth
    {
        get => maxPublishDepth;
        private set => maxPublishDepth = Math.Max((byte)1, Math.Min(value, PQFieldKeys.TwoByteFieldIdMaxBookDepth));
    }

    private IPQOrderBookLayerFactorySelector LayerSelector { get; set; } = null!;

    protected string PQOrderBookSideToStringMembers =>
        $"{nameof(Capacity)}: {Capacity}, {nameof(Count)}: {Count}, {nameof(IsLadder)}: {IsLadder}, " +
        $"{nameof(AllLayers)}:[{string.Join(", ", AllLayers.Take(Count))}]";

    INameIdLookup? IHasNameIdLookup.NameIdLookup => NameIdLookup;

    public LayerType LayerSupportedType
    {
        get => LayerSupportedFlags.MostCompactLayerType();
        private set => LayerSupportedFlags |= value.SupportedLayerFlags();
    }

    public LayerFlags LayerSupportedFlags
    {
        get => layerFlags;
        set => layerFlags = layerFlags.Unset(LayerFlags.Ladder) | value;
    }

    public bool IsLadder
    {
        get => layerFlags.HasLadder();
        set => layerFlags = value ? layerFlags | LayerFlags.Ladder : layerFlags.Unset(LayerFlags.Ladder);
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
                AllLayers[level] = value;
            }
        }
    }

    IPriceVolumeLayer? IOrderBookSide.this[int level] => this[level];

    IMutablePriceVolumeLayer? IMutableOrderBookSide.this[int level]
    {
        get => this[level];
        set => this[level] = value as IPQPriceVolumeLayer;
    }

    public IList<IPQPriceVolumeLayer> AllLayers
    {
        get => allLayers;
        set => allLayers = value;
    }

    public IPQNameIdLookupGenerator NameIdLookup
    {
        get => nameIdLookupGenerator;
        set
        {
            if (nameIdLookupGenerator == value) return;
            nameIdLookupGenerator = value;
            LayerSelector         = new PQOrderBookLayerFactorySelector(nameIdLookupGenerator);

            if (allLayers != null)
                foreach (var pqPriceVolumeLayer in allLayers.OfType<ISupportsPQNameIdLookupGenerator>())
                    pqPriceVolumeLayer.NameIdLookup = value;
        }
    }

    public int Capacity
    {
        get => Math.Min(MaxPublishDepth, AllLayers.Count);
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
            for (var i = Math.Min(MaxPublishDepth, AllLayers.Count - 1); i >= 0; i--)
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

    public decimal? SourceOpenInterestVolume
    {
        get => sourceOpenInterestVolume;
        set
        {
            IsSourceOpenInterestVolumeUpdated |= sourceOpenInterestVolume != value || NumOfUpdates == 0;
            sourceOpenInterestVolume          =  value;
        }
    }
    public decimal? SourceOpenInterestVwap
    {
        get => sourceOpenInterestVwap;
        set
        {
            IsSourceOpenInterestVwapUpdated |= sourceOpenInterestVwap != value || NumOfUpdates == 0;
            sourceOpenInterestVwap          =  value;
        }
    }
    public decimal AdapterOpenInterestVolume
    {
        get => adapterOpenInterestVolume;
        set
        {
            IsAdapterOpenInterestVolumeUpdated |= adapterOpenInterestVolume != value || NumOfUpdates == 0;
            adapterOpenInterestVolume          =  value;
        }
    }
    public decimal AdapterOpenInterestVwap
    {
        get => adapterOpenInterestVwap;
        set
        {
            IsAdapterOpenInterestVwapUpdated |= adapterOpenInterestVwap != value || NumOfUpdates == 0;
            adapterOpenInterestVwap          =  value;
        }
    }

    public bool IsSourceOpenInterestVolumeUpdated
    {
        get => (UpdatedFlags & OrderBookSideUpdatedFlags.SourceOpenInterestVolumeUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderBookSideUpdatedFlags.SourceOpenInterestVolumeUpdated;

            else if (IsSourceOpenInterestVolumeUpdated) UpdatedFlags ^= OrderBookSideUpdatedFlags.SourceOpenInterestVolumeUpdated;
        }
    }

    public bool IsSourceOpenInterestVwapUpdated
    {
        get => (UpdatedFlags & OrderBookSideUpdatedFlags.SourceOpenInterestVwapUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderBookSideUpdatedFlags.SourceOpenInterestVwapUpdated;

            else if (IsSourceOpenInterestVwapUpdated) UpdatedFlags ^= OrderBookSideUpdatedFlags.SourceOpenInterestVwapUpdated;
        }
    }

    public bool IsAdapterOpenInterestVolumeUpdated
    {
        get => (UpdatedFlags & OrderBookSideUpdatedFlags.AdapterOpenInterestVolumeUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderBookSideUpdatedFlags.AdapterOpenInterestVolumeUpdated;

            else if (IsAdapterOpenInterestVolumeUpdated) UpdatedFlags ^= OrderBookSideUpdatedFlags.AdapterOpenInterestVolumeUpdated;
        }
    }

    public bool IsAdapterOpenInterestVwapUpdated
    {
        get => (UpdatedFlags & OrderBookSideUpdatedFlags.AdapterOpenInterestVwapUpdated) > 0;
        set
        {
            if (value)
                UpdatedFlags |= OrderBookSideUpdatedFlags.AdapterOpenInterestVwapUpdated;

            else if (IsAdapterOpenInterestVwapUpdated) UpdatedFlags ^= OrderBookSideUpdatedFlags.AdapterOpenInterestVwapUpdated;
        }
    }

    public OpenInterest? SourceOpenInterest
    {
        get
        {
            if (sourceOpenInterestVolume != null || sourceOpenInterestVwap != null)
                return new OpenInterest(MarketDataSource.Venue, sourceOpenInterestVolume ?? 0m, sourceOpenInterestVwap ?? 0);
            return null;
        }
        set
        {
            SourceOpenInterestVolume = value?.Volume;
            SourceOpenInterestVwap   = value?.Vwap;
        }
    }
    public OpenInterest AdapterOpenInterest
    {
        get => new(MarketDataSource.Venue, adapterOpenInterestVolume, adapterOpenInterestVwap);
        set
        {
            SourceOpenInterestVolume = value.Volume;
            SourceOpenInterestVwap   = value.Vwap;
        }
    }

    public OpenInterest PublishedOpenInterest
    {
        get
        {
            var vwapResult = this.CalculateVwap();
            return new OpenInterest(MarketDataSource.Published, vwapResult.VolumeAchieved, vwapResult.AchievedVwap);
        }
    }

    public uint UpdateCount => NumOfUpdates;

    public void UpdateComplete()
    {
        foreach (var pqPriceVolumeLayer in AllLayers.OfType<IPQPriceVolumeLayer>()) pqPriceVolumeLayer.UpdateComplete();
        NameIdLookup.UpdateComplete();
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

        LayerSupportedFlags = source.LayerSupportedFlags;
        MaxPublishDepth          = source.MaxPublishDepth;
        var originalCount = Count;

        var allSourceLayers = source.Capacity;
        for (var i = 0; i < allSourceLayers; i++)
        {
            var sourceLayer = source[i];
            if (sourceLayer is null or { IsEmpty: true } && i >= originalCount) continue;
            var destinationLayer = this[i];

            if (i < AllLayers.Count)
                AllLayers[i] = LayerSelector.UpgradeExistingLayer(destinationLayer, NameIdLookup, LayerSupportedType, sourceLayer, copyMergeFlags);
            else
                AllLayers.Add(LayerSelector.CreateExpectedImplementation(LayerSupportedType, NameIdLookup, sourceLayer, copyMergeFlags));
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
        AllLayers.Add(LayerSelector.CreateExpectedImplementation(LayerSupportedType, NameIdLookup));
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
        MaxPublishDepth          = bookDepth;

        var layerFactory = LayerSelector.FindForLayerFlags(LayerSupportedFlags);

        for (var i = 0; i < MaxPublishDepth; i++)
            if (i >= AllLayers.Count || AllLayers[i] == null)
                AllLayers.Add(layerFactory.CreateNewLayer());
            else if (i < AllLayers.Count && AllLayers[i] is { } currentLayer
                                         && !LayerSelector.OriginalCanWhollyContain(LayerSupportedFlags
                                                                                  , currentLayer.SupportsLayerFlags))
                AllLayers[i] = layerFactory.UpgradeLayer(currentLayer);
        for (var i = MaxPublishDepth; i < AllLayers.Count; i++) AllLayers.RemoveAt(i);
    }

    public virtual bool AreEquivalent(IOrderBookSide? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;

        var layerFlagsSame    = LayerSupportedFlags == other.LayerSupportedFlags;
        var deepestPricedSame = Count == other.Count;
        var maxPubDepthSame   = MaxPublishDepth == other.MaxPublishDepth;
        var bookLayersSame    = true;
        for (int i = 0; i < Count; i++)
        {
            var localLayer  = this[i];
            var otherLayer  = other[i];
            var layerIsSame = localLayer?.AreEquivalent(otherLayer, exactTypes) ?? otherLayer == null;
            bookLayersSame &= layerIsSame;
        }

        var deepestPossibleSame = true;

        if (other is IMutableOrderBookSide mutableOrderBook) deepestPossibleSame = Capacity == mutableOrderBook.Capacity;

        var allAreSame = layerFlagsSame && deepestPossibleSame && deepestPricedSame && maxPubDepthSame && bookLayersSame;
        return allAreSame;
    }

    public IEnumerator<IPQPriceVolumeLayer> GetEnumerator() => AllLayers.Take(Count).GetEnumerator()!;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<IPriceVolumeLayer> IEnumerable<IPriceVolumeLayer>.GetEnumerator() => GetEnumerator();

    public override void StateReset()
    {
        for (var i = 0; i < AllLayers.Count; i++) AllLayers[i]?.StateReset();
        NameIdLookup.Clear();
        NumOfUpdates = 0;
        base.StateReset();
    }

    public IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags)
    {
        if (AllLayers.Count <= 0) yield break;
        // All layers share same dictionary or should do anyway
        foreach (var stringUpdate in NameIdLookup.GetStringUpdates(snapShotTime, messageFlags))
            if (BookSide == BookSide.AskBook)
                yield return stringUpdate.WithDepth(PQDepthKey.AskSide);
            else
                yield return stringUpdate;
    }

    public bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        if (stringUpdate.Field.Id == PQQuoteFields.LayerNameDictionaryUpsertCommand) return NameIdLookup.UpdateFieldString(stringUpdate);
        return false;
    }

    private IPQNameIdLookupGenerator SourceOtherExistingOrNewPQNameIdNameLookup(IEnumerable<IPriceVolumeLayer>? source)
    {
        IPQNameIdLookupGenerator thisLayDict;
        if (source is IPQOrderBookSide { NameIdLookup: not null } pqOrderBook)
            thisLayDict = InitializeNewIdLookupGenerator(pqOrderBook.NameIdLookup);
        else
            thisLayDict = InitializeNewIdLookupGenerator
                (
                 source?.OfType<ISupportsPQNameIdLookupGenerator>()
                       ?.Where(pvl => pvl is { NameIdLookup: not null })
                       ?.Select(pvl => pvl.NameIdLookup)
                       ?.FirstOrDefault());

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

    public override int GetHashCode() => AllLayers != null ? AllLayers.GetHashCode() : 0;

    public override string ToString() => $"{GetType().Name}({PQOrderBookSideToStringMembers})";
}
