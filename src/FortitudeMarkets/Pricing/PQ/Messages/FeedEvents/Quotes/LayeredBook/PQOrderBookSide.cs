// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using System.Text;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.LayerSelector;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook;

public interface IPQOrderBookSide : IMutableOrderBookSide, IPQSupportsNumberPrecisionFieldUpdates
  , IPQSupportsStringUpdates, ICloneable<IPQOrderBookSide>
  , IRelatedItems<LayerFlags, ushort>, IRelatedItems<INameIdLookupGenerator>
  , ISupportsPQNameIdLookupGenerator, ITrackableReset<IPQOrderBookSide>
  , IMutableTracksShiftsList<IPQPriceVolumeLayer, IPriceVolumeLayer>

{
    new IPQPriceVolumeLayer this[int index] { get; set; }

    new IReadOnlyList<ListShiftCommand> ShiftCommands { get; set; }

    new int? ClearRemainingElementsFromIndex { get; set; }

    new bool HasUnreliableListTracking { get; set; }

    new bool IsReadOnly { get; }

    new ListShiftCommand ClearAll();

    new ListShiftCommand DeleteAt(int index);

    new ListShiftCommand ApplyListShiftCommand(ListShiftCommand shiftCommandToApply);

    new int Capacity { get; set; }

    new ushort MaxAllowedSize { get; set; }

    new int Count { get; set; }

    new IReadOnlyList<IPQPriceVolumeLayer> AllLayers { get; }

    new IPQNameIdLookupGenerator NameIdLookup { get; set; }

    new IPQMarketAggregate? OpenInterestSide { get; set; }

    bool IsDailyTickUpdateCountUpdated { get; set; }

    new bool CalculateShift(DateTime asAtTime, IReadOnlyList<IPriceVolumeLayer> updatedCollection);

    new ListShiftCommand ShiftElements(int byElements);

    new ListShiftCommand AppendShiftCommand(ListShiftCommand toAppendAtEnd);

    new void ClearShiftCommands();

    new void Clear();

    new void RemoveAt(int index);

    new IPQOrderBookSide Clone();

    new IPQOrderBookSide ResetWithTracking();

    new IEnumerator<IPQPriceVolumeLayer> GetEnumerator();
}

public class PQOrderBookSide : ReusableQuoteElement<IOrderBookSide>, IPQOrderBookSide
{
    private readonly IList<IPQPriceVolumeLayer> allLayers;

    private readonly TrackListShiftsRegistry<IPQPriceVolumeLayer, IPriceVolumeLayer> elementListShiftRegistry;

    private IPQNameIdLookupGenerator nameIdLookupGenerator;
    private IPQMarketAggregate?      pqOpenInterestSide;
    private LayerFlags               layerFlags = LayerFlagsExtensions.PriceVolumeLayerFlags;

    protected OrderBookUpdatedFlags UpdatedFlags;

    private QuoteInstantBehaviorFlags cacheBehaviorFlags = QuoteInstantBehaviorFlags.None;

    protected uint SequenceId = uint.MaxValue;

    private ushort maxPublishDepth = 1;
    private uint   dailyTickUpdateCount;

    public PQOrderBookSide()
    {
        elementListShiftRegistry = new TrackListShiftsRegistry<IPQPriceVolumeLayer, IPriceVolumeLayer>(this, NewElementFactory, SamePrice);

        BookSide              = BookSide.Unknown;
        nameIdLookupGenerator = InitializeNewIdLookupGenerator();
        LayerSelector         = new PQOrderBookLayerFactorySelector(nameIdLookupGenerator);
        allLayers             = [new PQPriceVolumeLayer()];

        if (GetType() == typeof(PQOrderBookSide)) SequenceId = 0;
    }

    public PQOrderBookSide(BookSide bookSide, LayerType layerType = LayerType.PriceVolume
      , QuoteInstantBehaviorFlags quoteBehavior = QuoteInstantBehaviorFlags.None,int numBookLayers = SourceTickerInfo.DefaultMaximumPublishedLayers
      , bool isLadder = false, IPQNameIdLookupGenerator? nameIdLookup = null)
    {
        cacheBehaviorFlags       = quoteBehavior;
        elementListShiftRegistry = new TrackListShiftsRegistry<IPQPriceVolumeLayer, IPriceVolumeLayer>(this, NewElementFactory, SamePrice);

        BookSide              =  bookSide;
        LayerSupportedFlags   =  layerType.SupportedLayerFlags();
        LayerSupportedFlags   |= LayerSupportedType.SupportedLayerFlags();
        IsLadder              =  isLadder;
        nameIdLookupGenerator =  nameIdLookup ?? InitializeNewIdLookupGenerator();
        LayerSelector         =  new PQOrderBookLayerFactorySelector(nameIdLookupGenerator);
        MaxAllowedSize        =  (ushort)numBookLayers;
        allLayers             =  new List<IPQPriceVolumeLayer>(MaxAllowedSize);
        for (var i = 0; i < MaxAllowedSize; i++)
        {
            var pqLayer = LayerSelector.CreateExpectedImplementation(LayerSupportedType, NameIdLookup, (QuoteLayerInstantBehaviorFlags)quoteBehavior);
            allLayers.Add(pqLayer);
        }

        if (GetType() == typeof(PQOrderBookSide)) SequenceId = 0;
    }

    public PQOrderBookSide(BookSide bookSide, ISourceTickerInfo srcTickerInfo, IPQNameIdLookupGenerator? nameIdLookup = null)
    {
        cacheBehaviorFlags       = (QuoteInstantBehaviorFlags)srcTickerInfo.QuoteBehaviorFlags;
        elementListShiftRegistry = new TrackListShiftsRegistry<IPQPriceVolumeLayer, IPriceVolumeLayer>(this, NewElementFactory, SamePrice);

        BookSide            =  bookSide;
        LayerSupportedFlags =  srcTickerInfo.LayerFlags;
        layerFlags          |= LayerSupportedType.SupportedLayerFlags();
        MaxAllowedSize      =  srcTickerInfo.MaximumPublishedLayers;
        nameIdLookupGenerator = nameIdLookup ??
                                (srcTickerInfo is IPQSourceTickerInfo pqSrcTkrInfo
                                    ? InitializeNewIdLookupGenerator(pqSrcTkrInfo.NameIdLookup)
                                    : InitializeNewIdLookupGenerator());
        LayerSelector = new PQOrderBookLayerFactorySelector(nameIdLookupGenerator);

        allLayers = new List<IPQPriceVolumeLayer>(MaxAllowedSize);
        for (var i = 0; i < MaxAllowedSize; i++)
        {
            var pqLayer = LayerSelector.CreateExpectedImplementation(LayerSupportedType, NameIdLookup, (QuoteLayerInstantBehaviorFlags)srcTickerInfo.QuoteBehaviorFlags);
            allLayers.Add(pqLayer);
        }
        EnsureRelatedItemsAreConfigured(nameIdLookup);

        if (GetType() == typeof(PQOrderBookSide)) SequenceId = 0;
    }

    public PQOrderBookSide
    (BookSide bookSide, IEnumerable<IPriceVolumeLayer>? bookLayers = null, QuoteInstantBehaviorFlags quoteBehavior = QuoteInstantBehaviorFlags.None
      , bool isLadder = false, IPQNameIdLookupGenerator? nameIdLookup = null)
    {
        cacheBehaviorFlags       = quoteBehavior;
        elementListShiftRegistry = new TrackListShiftsRegistry<IPQPriceVolumeLayer, IPriceVolumeLayer>(this, NewElementFactory, SamePrice);

        BookSide            = bookSide;
        LayerSupportedFlags = bookLayers?.FirstOrDefault()?.SupportsLayerFlags ?? LayerFlagsExtensions.PriceVolumeLayerFlags;
        IsLadder            = isLadder;
        MaxAllowedSize      = (ushort)(bookLayers?.Count() ?? 1);

        nameIdLookupGenerator = nameIdLookup ?? SourceOtherExistingOrNewPQNameIdNameLookup(bookLayers);
        LayerSelector         = new PQOrderBookLayerFactorySelector(nameIdLookupGenerator);
        allLayers =
            (bookLayers?.Select(pvl => LayerSelector.UpgradeExistingLayer(pvl, pvl.LayerType, (QuoteLayerInstantBehaviorFlags)quoteBehavior, pvl, CopyMergeFlags.FullReplace))
                       .OfType<IPQPriceVolumeLayer>()
                       .ToList() ?? [LayerSelector.CreateExpectedImplementation(LayerSupportedType, NameIdLookup, (QuoteLayerInstantBehaviorFlags)quoteBehavior)]
            );

        if (GetType() == typeof(PQOrderBookSide)) SequenceId = 0;
    }

    public PQOrderBookSide(IOrderBookSide toClone, IPQNameIdLookupGenerator? nameIdLookup = null)
    {
        cacheBehaviorFlags       = toClone.QuoteBehavior;
        elementListShiftRegistry = new TrackListShiftsRegistry<IPQPriceVolumeLayer, IPriceVolumeLayer>(this, NewElementFactory, SamePrice);

        MaxAllowedSize = toClone.MaxAllowedSize;
        elementListShiftRegistry.CopyFrom(toClone.ShiftCommands);

        BookSide = toClone.BookSide;

        LayerSupportedFlags =  toClone.LayerSupportedFlags;
        layerFlags          |= LayerSupportedType.SupportedLayerFlags();

        if (toClone.HasNonEmptyOpenInterest)
        {
            pqOpenInterestSide = new PQMarketAggregate(toClone.OpenInterestSide);
        }

        DailyTickUpdateCount  = toClone.DailyTickUpdateCount;
        nameIdLookupGenerator = InitializeNewIdLookupGenerator(nameIdLookup, toClone);
        LayerSelector         = new PQOrderBookLayerFactorySelector(nameIdLookupGenerator);

        var size = toClone.Capacity;
        allLayers = new List<IPQPriceVolumeLayer>(size);
        Capacity  = toClone.Capacity;
        for (var i = 0; i < size; i++)
        {
            var sourceLayer = toClone[i];
            var pqLayer = LayerSelector.CreateExpectedImplementation(sourceLayer.LayerType, NameIdLookup, (QuoteLayerInstantBehaviorFlags)cacheBehaviorFlags, sourceLayer, CopyMergeFlags.FullReplace);
            allLayers.Add(pqLayer);
        }

        if (GetType() == typeof(PQOrderBookSide)) SequenceId = 0;
    }


    protected static Func<IPriceVolumeLayer, IPriceVolumeLayer, bool> SamePrice = (lhs, rhs) => lhs.Price == rhs.Price;

    protected Func<IPQPriceVolumeLayer> NewElementFactory => () => LayerSelector.CreateExpectedImplementation(LayerSupportedType, NameIdLookup, (QuoteLayerInstantBehaviorFlags)QuoteBehavior);
    
    public QuoteInstantBehaviorFlags QuoteBehavior => cacheBehaviorFlags = Parent?.QuoteBehavior ?? cacheBehaviorFlags;

    public IParentQuoteElement? Parent { get; set; }

    public ushort MaxAllowedSize
    {
        get => maxPublishDepth;
        set => maxPublishDepth = Math.Max((byte)1, Math.Min(value, PQFeedFieldsExtensions.TwoByteFieldIdMaxBookDepth));
    }

    private IPQOrderBookLayerFactorySelector LayerSelector { get; set; }

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

    public IReadOnlyList<ListShiftCommand> ShiftCommands
    {
        get => elementListShiftRegistry.ShiftCommands;
        set => elementListShiftRegistry.ShiftCommands = (List<ListShiftCommand>)value;
    }

    public int? ClearRemainingElementsFromIndex
    {
        get => elementListShiftRegistry.ClearRemainingElementsFromIndex;
        set => elementListShiftRegistry.ClearRemainingElementsFromIndex = (ushort?)value;
    }

    public bool HasUnreliableListTracking
    {
        get => elementListShiftRegistry.HasUnreliableListTracking;
        set => elementListShiftRegistry.HasUnreliableListTracking = value;
    }

    public bool HasNonEmptyOpenInterest
    {
        get => pqOpenInterestSide is { IsEmpty: false };
        set
        {
            if (value) return;
            if (pqOpenInterestSide != null)
            {
                pqOpenInterestSide.IsEmpty = true;
            }
        }
    }

    public uint UpdateSequenceId => SequenceId;

    public BookSide BookSide { get; }

    IMutablePriceVolumeLayer IReadOnlyList<IMutablePriceVolumeLayer>.this[int index] => this[index];

    IPriceVolumeLayer IReadOnlyList<IPriceVolumeLayer>.this[int level] => this[level];

    IMutablePriceVolumeLayer IMutableOrderBookSide.this[int level]
    {
        get => this[level];
        set => this[level] = (IPQPriceVolumeLayer)value;
    }

    IPriceVolumeLayer IOrderBookSide.this[int index] => this[index];

    IMutablePriceVolumeLayer IMutableTracksShiftsList<IMutablePriceVolumeLayer, IPriceVolumeLayer>.this[int index]
    {
        get => this[index];
        set => this[index] = (IPQPriceVolumeLayer)value;
    }

    IMutablePriceVolumeLayer IList<IMutablePriceVolumeLayer>.this[int index]
    {
        get => this[index];
        set => this[index] = (IPQPriceVolumeLayer)value;
    }

    IMutablePriceVolumeLayer IMutableCapacityList<IMutablePriceVolumeLayer>.this[int i]
    {
        get => this[i];
        set => this[i] = (IPQPriceVolumeLayer)value;
    }

    public IPQPriceVolumeLayer this[int level]
    {
        get
        {
            if (level < allLayers.Count) return allLayers[level];
            if (level >= MaxAllowedSize) throw new ArgumentException("Error attempted to update a level beyond the maximum allowed book size");
            cacheBehaviorFlags = QuoteBehavior;
            while (Capacity <= level)
            {
                var pqLayer = LayerSelector.CreateExpectedImplementation(LayerSupportedType, NameIdLookup, (QuoteLayerInstantBehaviorFlags)cacheBehaviorFlags);
                allLayers.Add(pqLayer);
            }
            return allLayers[level];
        }
        set
        {
            if (value is ISupportsPQNameIdLookupGenerator setNameIdLookup)
            {
                setNameIdLookup.NameIdLookup = NameIdLookup;
            }
            HasUnreliableListTracking |= ShiftCommands.Any() && !ReferenceEquals(allLayers[level], value);
            allLayers[level]          =  value;
        }
    }

    IReadOnlyList<IPriceVolumeLayer> IOrderBookSide.AllLayers => AllLayers;

    public IReadOnlyList<IPQPriceVolumeLayer> AllLayers => allLayers.Take(Count).ToList().AsReadOnly();

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
        get => Math.Min(MaxAllowedSize, allLayers.Count);
        set
        {
            if (value > PQFeedFieldsExtensions.SingleByteFieldIdMaxBookDepth)
                throw new ArgumentException("Expected PQOrderBook Capacity to be less than or equal to " +
                                            PQFeedFieldsExtensions.SingleByteFieldIdMaxBookDepth);

            for (var i = allLayers.Count; i < value && allLayers.Count > 0; i++)
            {
                var clonedFirstLayer = allLayers[0].Clone();
                clonedFirstLayer.StateReset();
                allLayers.Add(clonedFirstLayer);
            }
        }
    }

    public int Count
    {
        get
        {
            for (var i = Math.Min(MaxAllowedSize, allLayers.Count - 1); i >= 0; i--)
            {
                var layerAtLevel = allLayers[i];
                if ((layerAtLevel.Price) > 0) return i + 1;
            }

            return 0;
        }
        set
        {
            for (var i = Capacity; i >= value; i--)
            {
                var layerAtLevel = allLayers[i];
                layerAtLevel.IsEmpty = true;
            }
        }
    }

    IMarketAggregate IOrderBookSide.OpenInterestSide => OpenInterestSide!;

    IMutableMarketAggregate? IMutableOrderBookSide.OpenInterestSide
    {
        get => OpenInterestSide;
        set => OpenInterestSide = (IPQMarketAggregate?)value;
    }

    public IPQMarketAggregate? OpenInterestSide
    {
        get
        {
            if (HasNonEmptyOpenInterest && pqOpenInterestSide is { DataSource: not (MarketDataSource.None or MarketDataSource.Published) })
                return pqOpenInterestSide;

            var vwapResult = this.CalculateVwap();

            pqOpenInterestSide            ??= new PQMarketAggregate();
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
            IsDailyTickUpdateCountUpdated |= value != dailyTickUpdateCount || SequenceId == 0;
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

    public bool HasUpdates
    {
        get
        {
            return UpdatedFlags != OrderBookUpdatedFlags.None
                || (HasNonEmptyOpenInterest
                 && pqOpenInterestSide is { DataSource: not (MarketDataSource.None or MarketDataSource.Published), HasUpdates: true })
                || allLayers.Any(pqPvl => pqPvl.HasUpdates);
        }
        set
        {
            foreach (var pqPvLayer in allLayers) pqPvLayer.HasUpdates = value;
            NameIdLookup.HasUpdates = value;
            if (pqOpenInterestSide != null)
            {
                pqOpenInterestSide.HasUpdates = value;
            }
            if (value) return;
            ClearShiftCommands();
            UpdatedFlags = OrderBookUpdatedFlags.None;
        }
    }

    public bool IsEmpty
    {
        get => allLayers.All(pvl => pvl.IsEmpty);
        set
        {
            if (!value) return;
            ResetWithTracking();
        }
    }

    IMutableOrderBookSide ITrackableReset<IMutableOrderBookSide>.ResetWithTracking() => ResetWithTracking();

    IPQOrderBookSide ITrackableReset<IPQOrderBookSide>.ResetWithTracking() => ResetWithTracking();

    IPQOrderBookSide IPQOrderBookSide.ResetWithTracking() => ResetWithTracking();


    ITracksResetCappedCapacityList<IMutablePriceVolumeLayer> ITrackableReset<ITracksResetCappedCapacityList<IMutablePriceVolumeLayer>>.
        ResetWithTracking() => ResetWithTracking();

    ITracksResetCappedCapacityList<IPQPriceVolumeLayer> ITrackableReset<ITracksResetCappedCapacityList<IPQPriceVolumeLayer>>.ResetWithTracking() =>
        ResetWithTracking();

    IMutableOrderBookSide IMutableOrderBookSide.ResetWithTracking() => ResetWithTracking();

    public PQOrderBookSide ResetWithTracking()
    {
        for (var i = 0; i < allLayers.Count; i++) allLayers[i].ResetWithTracking();
        pqOpenInterestSide?.ResetWithTracking();
        ClearShiftCommands();
        DailyTickUpdateCount = 0;

        return this;
    }

    public override void StateReset()
    {
        for (var i = 0; i < allLayers.Count; i++) allLayers[i].StateReset();
        DailyTickUpdateCount = 0;
        NameIdLookup.Clear();
        pqOpenInterestSide?.StateReset();
        SequenceId   = 0;
        UpdatedFlags = OrderBookUpdatedFlags.None;
        base.StateReset();
    }

    public void UpdateStarted(uint updateSequenceId)
    {
        SequenceId = updateSequenceId;
        foreach (var priceVolumeLayer in allLayers)
        {
            priceVolumeLayer.UpdateStarted(updateSequenceId);
        }
    }

    public void UpdateComplete(uint updateSequenceId = 0)
    {
        foreach (var pqPriceVolumeLayer in allLayers) pqPriceVolumeLayer.UpdateComplete();
        elementListShiftRegistry.ClearShiftCommands();
        NameIdLookup.UpdateComplete(updateSequenceId);
        pqOpenInterestSide?.UpdateComplete(updateSequenceId);
        if (HasUpdates)
        {
            SequenceId++;
            HasUpdates = false;
        }
    }

    public int AppendEntryAtEnd()
    {
        var index = allLayers.Count;
        allLayers.Add(LayerSelector.CreateExpectedImplementation(LayerSupportedType, NameIdLookup, (QuoteLayerInstantBehaviorFlags)QuoteBehavior));
        return index;
    }

    public bool CalculateShift(DateTime asAtTime, IReadOnlyList<IPriceVolumeLayer> updatedCollection) =>
        elementListShiftRegistry.CalculateShift(asAtTime, updatedCollection);

    public ListShiftCommand AppendShiftCommand(ListShiftCommand toAppendAtEnd) => elementListShiftRegistry.AppendShiftCommand(toAppendAtEnd);

    public void ClearShiftCommands() => elementListShiftRegistry.ClearShiftCommands();

    public ListShiftCommand InsertAtStart(IMutablePriceVolumeLayer toInsertAtStart) =>
        elementListShiftRegistry.InsertAtStart((IPQPriceVolumeLayer)toInsertAtStart);

    public bool AppendAtEnd(IMutablePriceVolumeLayer toAppendAtEnd) => elementListShiftRegistry.AppendAtEnd((IPQPriceVolumeLayer)toAppendAtEnd);

    public ListShiftCommand InsertAt(int index, IMutablePriceVolumeLayer toInsertAtStart) =>
        elementListShiftRegistry.InsertAt(index, (IPQPriceVolumeLayer)toInsertAtStart);

    public ListShiftCommand Delete(IMutablePriceVolumeLayer toDelete) =>
        elementListShiftRegistry.Delete((IPQPriceVolumeLayer)toDelete);

    public ListShiftCommand ApplyListShiftCommand(ListShiftCommand shiftCommandToApply) =>
        elementListShiftRegistry.ApplyListShiftCommand(shiftCommandToApply);

    public ListShiftCommand ClearAll() => elementListShiftRegistry.ClearAll();

    public ListShiftCommand ShiftElements(int byElements) => elementListShiftRegistry.ShiftElements(byElements);

    public ListShiftCommand InsertAtStart(IPQPriceVolumeLayer toInsertAtStart) => elementListShiftRegistry.InsertAtStart(toInsertAtStart);

    public bool AppendAtEnd(IPQPriceVolumeLayer toAppendAtEnd) => elementListShiftRegistry.AppendAtEnd(toAppendAtEnd);

    public ListShiftCommand InsertAt(int index, IPQPriceVolumeLayer toInsertAtStart) => elementListShiftRegistry.InsertAt(index, toInsertAtStart);

    public ListShiftCommand DeleteAt(int index) => elementListShiftRegistry.DeleteAt(index);

    public ListShiftCommand Delete(IPQPriceVolumeLayer toDelete) => elementListShiftRegistry.Delete(toDelete);

    public void Add(IPQPriceVolumeLayer item)
    {
        HasUnreliableListTracking = ShiftCommands.Any();
        allLayers.Add(item);
    }

    public bool Contains(IPQPriceVolumeLayer item) => allLayers.Contains(item);

    public void CopyTo(IPQPriceVolumeLayer[] array, int arrayIndex)
    {
        for (int i = 0; i < allLayers.Count && i + arrayIndex < array.Length; i++)
        {
            array[i + arrayIndex] = allLayers[i];
        }
    }

    public bool Remove(IPQPriceVolumeLayer item)
    {
        HasUnreliableListTracking = ShiftCommands.Any();
        return allLayers.Remove(item);
    }

    public int IndexOf(IPQPriceVolumeLayer item) => allLayers.IndexOf(item);

    public void Insert(int index, IPQPriceVolumeLayer item)
    {
        HasUnreliableListTracking = ShiftCommands.Any();
        allLayers.Insert(index, item);
    }

    public void Add(IMutablePriceVolumeLayer item)
    {
        HasUnreliableListTracking = ShiftCommands.Any();
        allLayers.Add((IPQPriceVolumeLayer)item);
    }

    public void Clear()
    {
        HasUnreliableListTracking = ShiftCommands.Any();
        allLayers.Clear();
    }

    public bool Contains(IMutablePriceVolumeLayer item) => allLayers.Contains((IPQPriceVolumeLayer)item);

    public void CopyTo(IMutablePriceVolumeLayer[] array, int arrayIndex)
    {
        for (int i = 0; i < allLayers.Count && i + arrayIndex < array.Length; i++)
        {
            array[i + arrayIndex] = allLayers[i];
        }
    }

    public bool Remove(IMutablePriceVolumeLayer item)
    {
        HasUnreliableListTracking = ShiftCommands.Any();
        return allLayers.Remove((IPQPriceVolumeLayer)item);
    }

    public bool IsReadOnly => false;

    public int IndexOf(IMutablePriceVolumeLayer item) => allLayers.IndexOf((IPQPriceVolumeLayer)item);

    public void Insert(int index, IMutablePriceVolumeLayer item)
    {
        HasUnreliableListTracking = ShiftCommands.Any();
        allLayers.Insert(index, (IPQPriceVolumeLayer)item);
    }

    public void RemoveAt(int index)
    {
        HasUnreliableListTracking = ShiftCommands.Any();
        allLayers.RemoveAt(index);
    }

    IEnumerator<IMutablePriceVolumeLayer> IMutableOrderBookSide.GetEnumerator() => GetEnumerator();

    IEnumerator<IMutablePriceVolumeLayer> IEnumerable<IMutablePriceVolumeLayer>.GetEnumerator() => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<IPriceVolumeLayer> IEnumerable<IPriceVolumeLayer>.GetEnumerator() => GetEnumerator();

    public IEnumerator<IPQPriceVolumeLayer> GetEnumerator() => allLayers.Take(Count).GetEnumerator();

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
        MaxAllowedSize      = bookDepth;

        var layerFactory = LayerSelector.FindForLayerFlags(LayerSupportedFlags, (QuoteLayerInstantBehaviorFlags)QuoteBehavior);

        for (var i = 0; i < MaxAllowedSize; i++)
            if (i >= allLayers.Count)
                allLayers.Add(layerFactory.CreateNewLayer());
            else if (i < allLayers.Count && allLayers[i] is { } currentLayer
                                         && !LayerSelector.OriginalCanWhollyContain(LayerSupportedFlags
                                                                                  , currentLayer.SupportsLayerFlags))
                allLayers[i] = layerFactory.UpgradeLayer(currentLayer);
        for (var i = MaxAllowedSize; i < allLayers.Count; i++) allLayers.RemoveAt(i);
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
            ? new PQNameIdLookupGenerator(optionalExisting, PQFeedFields.QuoteLayerStringUpdates)
            : clone is IPQOrderBookSide pqClone
                ? new PQNameIdLookupGenerator(pqClone.NameIdLookup, PQFeedFields.QuoteLayerStringUpdates)
                : new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);
        return thisBookNameIdLookupGenerator;
    }

    public IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, Serdes.Serialization.PQMessageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var fullPicture = (messageFlags & Serdes.Serialization.PQMessageFlags.Complete) > 0;

        foreach (var shiftCommand in elementListShiftRegistry.ShiftCommands)
        {
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayers, PQPricingSubFieldKeys.CommandElementsShift
                                         , (uint)shiftCommand, (PQFieldFlags)shiftCommand);
        }
        if (fullPicture || IsDailyTickUpdateCountUpdated)
        {
            yield return new PQFieldUpdate(PQFeedFields.QuoteDailySidedTickCount, DailyTickUpdateCount);
        }
        if (pqOpenInterestSide != null)
        {
            foreach (var oiUpdates in pqOpenInterestSide.GetDeltaUpdateFields(snapShotTime, messageFlags, quotePublicationPrecisionSetting))
            {
                yield return oiUpdates.WithFieldId(PQFeedFields.QuoteOpenInterestSided);
            }
        }
        for (var i = 0; i < allLayers.Count; i++)
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
        switch (pqFieldUpdate.Id)
        {
            case PQFeedFields.QuoteLayers:
                var elementShift = (ListShiftCommand)(pqFieldUpdate);
                elementListShiftRegistry.AppendShiftCommand(elementShift);
                ApplyListShiftCommand(elementShift);
                break;
            case PQFeedFields.QuoteDailySidedTickCount:
                IsDailyTickUpdateCountUpdated = true;

                DailyTickUpdateCount = pqFieldUpdate.Payload;
                return 0;
            case PQFeedFields.QuoteOpenInterestSided:
                pqOpenInterestSide ??= new PQMarketAggregate();
                return pqOpenInterestSide.UpdateField(pqFieldUpdate);
            case PQFeedFields.QuoteLayerStringUpdates: return NameIdLookup.VerifyDictionaryAndExtractSize(pqFieldUpdate);
            default:
                if (pqFieldUpdate.Id is >= PQFeedFields.QuoteLayerPrice and < PQFeedFields.QuoteLayersRangeEnd)
                {
                    var index = pqFieldUpdate.DepthIndex();

                    var pqPriceVolumeLayer = this[index];
                    return pqPriceVolumeLayer.UpdateField(pqFieldUpdate);
                }
                break;
        }
        return -1;
    }

    public IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, Serdes.Serialization.PQMessageFlags messageFlags)
    {
        if (allLayers.Count <= 0) return [];
        // All layers share same dictionary or should do anyway
        return NameIdLookup.GetStringUpdates(snapShotTime, messageFlags);
    }

    public bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        // All layers share same dictionary or should do anyway
        if (stringUpdate.Field.Id != PQFeedFields.QuoteLayerStringUpdates) return false;
        return NameIdLookup.UpdateFieldString(stringUpdate);
    }

    object ICloneable.Clone() => Clone();

    IOrderBookSide ICloneable<IOrderBookSide>.Clone() => Clone();

    IMutableOrderBookSide ICloneable<IMutableOrderBookSide>.Clone() => Clone();

    IMutableOrderBookSide IMutableOrderBookSide.Clone() => Clone();

    public override IPQOrderBookSide Clone() => (IPQOrderBookSide?)Recycler?.Borrow<PQOrderBookSide>().CopyFrom(this) ?? new PQOrderBookSide(this);

    public override IOrderBookSide CopyFrom(IOrderBookSide source, QuoteInstantBehaviorFlags behaviorFlags
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        cacheBehaviorFlags = behaviorFlags;
        if (!copyMergeFlags.HasSkipReferenceLookups() && source is PQOrderBookSide sourcePqOrderBook)
            NameIdLookup.CopyFrom(sourcePqOrderBook.NameIdLookup, copyMergeFlags);
        MaxAllowedSize = source.MaxAllowedSize;
        elementListShiftRegistry.CopyFrom(source, copyMergeFlags);
        copyMergeFlags = copyMergeFlags.AddSkipReferenceLookups();
        if (source.HasNonEmptyOpenInterest)
        {
            pqOpenInterestSide ??= new PQMarketAggregate();
            pqOpenInterestSide.CopyFrom(source.OpenInterestSide, copyMergeFlags);
        }
        else if (pqOpenInterestSide != null)
        {
            pqOpenInterestSide.IsEmpty = true;
        }
        DailyTickUpdateCount = source.DailyTickUpdateCount;
        LayerSupportedFlags  = source.LayerSupportedFlags;

        var allSourceLayers = source.Capacity;
        for (var i = 0; i < allSourceLayers; i++)
        {
            var sourceLayer      = source[i];
            var destinationLayer = this[i];

            allLayers[i] = LayerSelector.UpgradeExistingLayer(destinationLayer, NameIdLookup, (QuoteLayerInstantBehaviorFlags)behaviorFlags, LayerSupportedType, sourceLayer, copyMergeFlags);

            destinationLayer.CopyFrom(sourceLayer, behaviorFlags, copyMergeFlags);
        }

        for (var i = source.Count; i < allLayers.Count; i++)
            if (allLayers[i] is IMutablePriceVolumeLayer mutablePvl)
                mutablePvl.IsEmpty = true;
        return this;
    }

    public override bool AreEquivalent(IOrderBookSide? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;

        var layerFlagsSame    = LayerSupportedFlags == other.LayerSupportedFlags;
        var deepestPricedSame = Count == other.Count;
        var maxPubDepthSame   = MaxAllowedSize == other.MaxAllowedSize;
        var openInterestSame  = HasNonEmptyOpenInterest == other.HasNonEmptyOpenInterest;
        if (openInterestSame && other.HasNonEmptyOpenInterest && HasNonEmptyOpenInterest)
        {
            openInterestSame = pqOpenInterestSide?.AreEquivalent(other.OpenInterestSide, exactTypes) ?? false;
        }
        var bookLayersSame = true;
        for (int i = 0; i < Count; i++)
        {
            var localLayer  = this[i];
            var otherLayer  = other[i];
            var layerIsSame = localLayer.AreEquivalent(otherLayer, exactTypes);
            bookLayersSame &= layerIsSame;
        }

        var deepestPossibleSame = true;

        // if (other is IMutableOrderBookSide mutableOrderBook) deepestPossibleSame = Capacity == mutableOrderBook.Capacity;

        var allAreSame = layerFlagsSame && deepestPossibleSame && deepestPricedSame
                      && maxPubDepthSame && bookLayersSame && openInterestSame;
        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IOrderBookSide, true);

    public override int GetHashCode() => allLayers.GetHashCode();

    public string EachLayerByIndexOnNewLines()
    {
        var countOfLayers = Count;
        var sb            = new StringBuilder(100 * countOfLayers);
        for (var i = 0; i < countOfLayers; i++)
        {
            var layer = allLayers[i];
            sb.Append("\t").Append(BookSide).Append("[").Append(i).Append("] = ").Append(layer);
            if (i < countOfLayers - 1)
            {
                sb.AppendLine(",");
            }
        }
        return sb.ToString();
    }

    protected string PQOrderBookSideToStringMembers =>
        $"{nameof(Capacity)}: {Capacity}, {nameof(MaxAllowedSize)}: {MaxAllowedSize}, {nameof(Count)}: {Count}, {nameof(IsLadder)}: {IsLadder}, " +
        $"{nameof(OpenInterestSide)}: {OpenInterestSide}, {nameof(DailyTickUpdateCount)}: {DailyTickUpdateCount}, " +
        $"{nameof(AllLayers)}: [\n{EachLayerByIndexOnNewLines()}]";

    public override string ToString() => $"{GetType().Name}({PQOrderBookSideToStringMembers})";
}
