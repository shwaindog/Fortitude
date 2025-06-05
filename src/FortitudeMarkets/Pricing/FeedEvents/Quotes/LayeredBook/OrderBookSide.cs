// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using System.Text;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.LayerSelector;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;

public class OrderBookSide : ReusableQuoteElement<IOrderBookSide>, IMutableOrderBookSide
{
    private readonly IList<IMutablePriceVolumeLayer> allLayers;

    private readonly TrackListShiftsRegistry<IMutablePriceVolumeLayer, IPriceVolumeLayer> elementListShiftRegistry;

    private ushort maxPublishDepth;

    private LayerFlags layerFlags = LayerFlagsExtensions.PriceVolumeLayerFlags;

    private IMutableMarketAggregate? openInterestSide;

    private QuoteInstantBehaviorFlags cacheBehaviorFlags = QuoteInstantBehaviorFlags.None;

    public OrderBookSide()
    {
        elementListShiftRegistry = new TrackListShiftsRegistry<IMutablePriceVolumeLayer, IPriceVolumeLayer>(this, NewElementFactory, SamePrice);

        BookSide  = BookSide.Unknown;
        allLayers = new List<IMutablePriceVolumeLayer>();

        allLayers.Add(LayerSelector.CreateExpectedImplementation(LayerSupportedType, (QuoteLayerInstantBehaviorFlags)cacheBehaviorFlags));
    }

    public OrderBookSide
    (BookSide bookSide, LayerType layerType = LayerType.PriceVolume, QuoteInstantBehaviorFlags quoteBehavior = QuoteInstantBehaviorFlags.None
      , ushort maxPublishDepth = SourceTickerInfo.DefaultMaximumPublishedLayers, bool isLadder = false)
    {
        cacheBehaviorFlags       = quoteBehavior;
        elementListShiftRegistry = new TrackListShiftsRegistry<IMutablePriceVolumeLayer, IPriceVolumeLayer>(this, NewElementFactory, SamePrice);
        LayerSupportedFlags = allLayers?.FirstOrDefault()?.SupportsLayerFlags ?? LayerFlagsExtensions.PriceVolumeLayerFlags |
            (isLadder ? LayerFlags.Ladder : LayerFlags.None);

        MaxAllowedSize = maxPublishDepth;

        BookSide   =  bookSide;
        layerFlags |= LayerSupportedType.SupportedLayerFlags();
        allLayers  =  Enumerable.Repeat(LayerSelector.CreateExpectedImplementation(layerType, (QuoteLayerInstantBehaviorFlags)quoteBehavior), maxPublishDepth).ToList();
        IsLadder   =  isLadder;
    }

    public OrderBookSide
        (BookSide bookSide, ushort numBookLayers, QuoteInstantBehaviorFlags quoteBehavior = QuoteInstantBehaviorFlags.None, bool isLadder = false)
    {
        cacheBehaviorFlags       = quoteBehavior;
        elementListShiftRegistry = new TrackListShiftsRegistry<IMutablePriceVolumeLayer, IPriceVolumeLayer>(this, NewElementFactory, SamePrice);
        LayerSupportedFlags      = (isLadder ? LayerFlags.Ladder : LayerFlags.None);

        MaxAllowedSize = numBookLayers;

        BookSide  = bookSide;
        allLayers = Enumerable.Repeat(LayerSelector.CreateExpectedImplementation(LayerSupportedType, (QuoteLayerInstantBehaviorFlags)cacheBehaviorFlags), numBookLayers).ToList();
        IsLadder  = isLadder;
    }

    public OrderBookSide
        (BookSide bookSide, IEnumerable<IPriceVolumeLayer> bookLayers, QuoteInstantBehaviorFlags quoteBehavior = QuoteInstantBehaviorFlags.None)
    {
        cacheBehaviorFlags       = quoteBehavior;
        elementListShiftRegistry = new TrackListShiftsRegistry<IMutablePriceVolumeLayer, IPriceVolumeLayer>(this, NewElementFactory, SamePrice);
        LayerSupportedFlags      = bookLayers.FirstOrDefault()?.SupportsLayerFlags ?? LayerFlagsExtensions.PriceVolumeLayerFlags;

        MaxAllowedSize = (ushort)bookLayers.Count();

        BookSide = bookSide;
        allLayers =
            (bookLayers
             .Select(pvl => LayerSelector.UpgradeExistingLayer(pvl, pvl.LayerType, (QuoteLayerInstantBehaviorFlags)quoteBehavior, pvl, CopyMergeFlags.FullReplace))
             .ToList()
            );
    }

    public OrderBookSide(IOrderBookSide toClone)
    {
        cacheBehaviorFlags       = toClone.QuoteBehavior;
        elementListShiftRegistry = new TrackListShiftsRegistry<IMutablePriceVolumeLayer, IPriceVolumeLayer>(this, NewElementFactory, SamePrice);
        elementListShiftRegistry.CopyFrom(toClone.ShiftCommands);

        HasUnreliableListTracking = toClone.HasUnreliableListTracking;
        LayerSupportedFlags       = toClone.LayerSupportedFlags;
        DailyTickUpdateCount      = toClone.DailyTickUpdateCount;

        MaxAllowedSize = toClone.MaxAllowedSize;


        BookSide   =  toClone.BookSide;
        layerFlags |= LayerSupportedType.SupportedLayerFlags();
        IsLadder   =  toClone.IsLadder;

        if (toClone.HasNonEmptyOpenInterest)
        {
            openInterestSide = new MarketAggregate(toClone.OpenInterestSide);
        }
        allLayers =
            toClone
                .Select
                    (pvl => LayerSelector.CreateExpectedImplementation(pvl.LayerType, (QuoteLayerInstantBehaviorFlags)cacheBehaviorFlags, pvl))
                .ToList();
        Capacity = toClone.Capacity;
    }

    public OrderBookSide(BookSide bookSide, ISourceTickerInfo sourceTickerInfo)
    {
        cacheBehaviorFlags       = (QuoteInstantBehaviorFlags)sourceTickerInfo.QuoteBehaviorFlags;
        elementListShiftRegistry = new TrackListShiftsRegistry<IMutablePriceVolumeLayer, IPriceVolumeLayer>(this, NewElementFactory, SamePrice);
        LayerSupportedFlags      = sourceTickerInfo.LayerFlags;

        MaxAllowedSize = sourceTickerInfo.MaximumPublishedLayers;

        BookSide   =  bookSide;
        layerFlags |= LayerSupportedType.SupportedLayerFlags();
        allLayers =
            Enumerable
                .Repeat(LayerSelector.CreateExpectedImplementation(LayerSupportedType, (QuoteLayerInstantBehaviorFlags)cacheBehaviorFlags), MaxAllowedSize)
                .ToList();
    }


    protected static Func<IPriceVolumeLayer, IPriceVolumeLayer, bool> SamePrice = (lhs, rhs) => lhs.Price == rhs.Price;

    protected Func<IMutablePriceVolumeLayer> NewElementFactory => () => LayerSelector.CreateExpectedImplementation(LayerSupportedType, (QuoteLayerInstantBehaviorFlags)QuoteBehavior);

    public QuoteInstantBehaviorFlags QuoteBehavior => cacheBehaviorFlags = Parent?.QuoteBehavior ?? cacheBehaviorFlags;

    public IParentQuoteElement? Parent { get; set; }

    public ushort MaxAllowedSize
    {
        get => maxPublishDepth;
        set => maxPublishDepth = Math.Max((byte)1, Math.Min(value, PQFeedFieldsExtensions.TwoByteFieldIdMaxBookDepth));
    }

    public static ILayerFlagsSelector<IMutablePriceVolumeLayer> LayerSelector { get; set; } = new OrderBookLayerFactorySelector();

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

    public bool HasNonEmptyOpenInterest
    {
        get => openInterestSide is { IsEmpty: false };
        set
        {
            if (value) return;
            if (openInterestSide != null)
            {
                openInterestSide.IsEmpty = true;
            }
        }
    }

    IMarketAggregate IOrderBookSide.OpenInterestSide => OpenInterestSide!;

    public IMutableMarketAggregate? OpenInterestSide
    {
        get
        {
            if (HasNonEmptyOpenInterest && openInterestSide is not { DataSource: (MarketDataSource.Published or MarketDataSource.None) })
                return openInterestSide;
            var vwapResult = this.CalculateVwap();

            openInterestSide            ??= new MarketAggregate();
            openInterestSide.DataSource =   MarketDataSource.Published;
            openInterestSide.UpdateTime =   DateTime.Now;
            openInterestSide.Volume     =   vwapResult.VolumeAchieved;
            openInterestSide.Vwap       =   vwapResult.AchievedVwap;
            return openInterestSide;
        }
        set
        {
            if (value != null)
            {
                openInterestSide ??= new MarketAggregate();

                openInterestSide.DataSource = value.DataSource;
                openInterestSide.UpdateTime = value.UpdateTime;
                openInterestSide.Volume     = value.Volume;
                openInterestSide.Vwap       = value.Vwap;
            }
            else if (openInterestSide != null)
            {
                openInterestSide.IsEmpty = true;
            }
        }
    }

    public uint DailyTickUpdateCount { get; set; }

    public IReadOnlyList<IPriceVolumeLayer> AllLayers => allLayers.Take(Count).ToList().AsReadOnly();


    public bool IsEmpty
    {
        get => allLayers.All(pvl => pvl.IsEmpty);
        set
        {
            foreach (var priceVolumeLayer in allLayers)
            {
                priceVolumeLayer.IsEmpty = value;
            }
        }
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

    public bool CalculateShift(DateTime asAtTime, IReadOnlyList<IPriceVolumeLayer> updatedCollection) =>
        elementListShiftRegistry.CalculateShift(asAtTime, updatedCollection);

    ListShiftCommand IMutableTracksShiftsList<IMutablePriceVolumeLayer, IPriceVolumeLayer>.AppendShiftCommand
        (ListShiftCommand toAppendAtEnd) =>
        elementListShiftRegistry.AppendShiftCommand(toAppendAtEnd);

    ListShiftCommand IMutableOrderBookSide.AppendShiftCommand(ListShiftCommand toAppendAtEnd) =>
        elementListShiftRegistry.AppendShiftCommand(toAppendAtEnd);

    public void ClearShiftCommands()
    {
        elementListShiftRegistry.ClearShiftCommands();
    }

    public ListShiftCommand ShiftElements(int byElements) => elementListShiftRegistry.ShiftElements(byElements);

    public ListShiftCommand InsertAtStart(IMutablePriceVolumeLayer toInsertAtStart) => elementListShiftRegistry.InsertAtStart(toInsertAtStart);

    public bool AppendAtEnd(IMutablePriceVolumeLayer toAppendAtEnd) => elementListShiftRegistry.AppendAtEnd(toAppendAtEnd);

    public ListShiftCommand InsertAt
        (int index, IMutablePriceVolumeLayer toInsertAtStart) =>
        elementListShiftRegistry.InsertAt(index, toInsertAtStart);

    public ListShiftCommand DeleteAt(int index) => elementListShiftRegistry.DeleteAt(index);

    public ListShiftCommand Delete(IMutablePriceVolumeLayer toDelete) => elementListShiftRegistry.Delete(toDelete);

    public ListShiftCommand ApplyListShiftCommand
        (ListShiftCommand shiftCommandToApply) =>
        elementListShiftRegistry.ApplyListShiftCommand(shiftCommandToApply);

    public ListShiftCommand ClearAll() => elementListShiftRegistry.ClearAll();

    IPriceVolumeLayer IOrderBookSide.this[int index] => this[index];

    public IMutablePriceVolumeLayer this[int level]
    {
        get
        {
            if (level < allLayers.Count) return allLayers[level];
            if (level >= MaxAllowedSize) throw new ArgumentException("Error attempted to update a level beyond the maximum allowed book size");
            cacheBehaviorFlags = QuoteBehavior;
            while (Capacity <= level)
            {
                var pqLayer = LayerSelector.CreateExpectedImplementation(LayerSupportedType, (QuoteLayerInstantBehaviorFlags)cacheBehaviorFlags);
                allLayers.Add(pqLayer);
            }
            return allLayers[level];
        }
        set
        {
            HasUnreliableListTracking |= ShiftCommands.Any() && !ReferenceEquals(allLayers[level], value);
            allLayers[level]          =  value;
        }
    }

    public BookSide BookSide { get; }

    IPriceVolumeLayer IReadOnlyList<IPriceVolumeLayer>.this[int level] => this[level];

    public int Count
    {
        get
        {
            for (var i = allLayers.Count - 1; i >= 0; i--)
            {
                var layerAtLevel = allLayers[i];
                if (!layerAtLevel.IsEmpty) return i + 1;
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

    public int Capacity
    {
        get => allLayers.Count;
        set
        {
            if (value > PQFeedFieldsExtensions.SingleByteFieldIdMaxBookDepth)
                throw new ArgumentException("Expected OrderBook Capacity to be less than or equal to " +
                                            PQFeedFieldsExtensions.SingleByteFieldIdMaxBookDepth);
            cacheBehaviorFlags = QuoteBehavior;
            while (allLayers.Count < value)
            {
                var cloneFirstLayer = LayerSelector.CreateExpectedImplementation(LayerSupportedType, (QuoteLayerInstantBehaviorFlags)cacheBehaviorFlags);
                cloneFirstLayer.StateReset();
                allLayers.Add(cloneFirstLayer);
            }
        }
    }

    public void Add(IMutablePriceVolumeLayer item)
    {
        HasUnreliableListTracking = ShiftCommands.Any();
        allLayers.Add(item);
    }

    public void Clear()
    {
        HasUnreliableListTracking = ShiftCommands.Any();
        allLayers.Clear();
    }

    public bool Contains(IMutablePriceVolumeLayer item) => allLayers.Contains(item);

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
        return allLayers.Remove(item);
    }

    public bool IsReadOnly => false;

    public int IndexOf(IMutablePriceVolumeLayer item) => allLayers.IndexOf(item);

    public void Insert(int index, IMutablePriceVolumeLayer item)
    {
        HasUnreliableListTracking = ShiftCommands.Any();
        allLayers.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        HasUnreliableListTracking = ShiftCommands.Any();
        allLayers.RemoveAt(index);
    }

    public int AppendEntryAtEnd()
    {
        HasUnreliableListTracking = ShiftCommands.Any();
        var index = allLayers.Count;
        allLayers.Add(LayerSelector.CreateExpectedImplementation(LayerSupportedType, (QuoteLayerInstantBehaviorFlags)QuoteBehavior));
        return index;
    }

    IMutableOrderBookSide ITrackableReset<IMutableOrderBookSide>.ResetWithTracking() => ResetWithTracking();


    ITracksResetCappedCapacityList<IMutablePriceVolumeLayer> ITrackableReset<ITracksResetCappedCapacityList<IMutablePriceVolumeLayer>>.
        ResetWithTracking() =>
        ResetWithTracking();

    IMutableOrderBookSide IMutableOrderBookSide.ResetWithTracking() => ResetWithTracking();

    public OrderBookSide ResetWithTracking()
    {
        for (var i = 0; i < allLayers.Count; i++) (allLayers[i]).ResetWithTracking();
        openInterestSide?.ResetWithTracking();

        return this;
    }

    public override void StateReset()
    {
        for (var i = 0; i < allLayers.Count; i++) (allLayers[i]).StateReset();
        openInterestSide?.StateReset();
        base.StateReset();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<IPriceVolumeLayer> IEnumerable<IPriceVolumeLayer>.GetEnumerator() => GetEnumerator();

    public IEnumerator<IMutablePriceVolumeLayer> GetEnumerator() => allLayers.Take(Count).GetEnumerator();

    IMutableOrderBookSide ICloneable<IMutableOrderBookSide>.Clone() => Clone();

    IMutableOrderBookSide IMutableOrderBookSide.Clone() => Clone();

    IOrderBookSide ICloneable<IOrderBookSide>.Clone() => Clone();

    public override OrderBookSide Clone() =>
        Recycler?.Borrow<OrderBookSide>().CopyFrom(this, QuoteInstantBehaviorFlags.DisableUpgradeLayer) ?? new OrderBookSide(this);

    public override OrderBookSide CopyFrom
    (IOrderBookSide source, QuoteInstantBehaviorFlags behaviorFlags
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        cacheBehaviorFlags  = behaviorFlags;
        LayerSupportedFlags = source.LayerSupportedFlags;
        MaxAllowedSize      = source.MaxAllowedSize;
        elementListShiftRegistry.CopyFrom(source, copyMergeFlags);
        if (source.HasNonEmptyOpenInterest)
        {
            openInterestSide ??= new MarketAggregate();
            openInterestSide.CopyFrom(source.OpenInterestSide, copyMergeFlags);
        }
        else if (openInterestSide != null)
        {
            openInterestSide.IsEmpty = true;
        }
        var allSourceLayers = source.Capacity;
        for (var i = 0; i < allSourceLayers; i++)
        {
            var sourceLayer      = source[i];
            var destinationLayer = this[i];

            allLayers[i] = LayerSelector.UpgradeExistingLayer
                (destinationLayer, LayerSupportedType, (QuoteLayerInstantBehaviorFlags)cacheBehaviorFlags, sourceLayer);

            destinationLayer.CopyFrom(sourceLayer, behaviorFlags, copyMergeFlags);
        }

        for (var i = source.Count; i < allLayers.Count; i++) allLayers[i].IsEmpty = true;
        return this;
    }

    public override bool AreEquivalent(IOrderBookSide? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;

        var ladderSame       = IsLadder == other.IsLadder;
        var countSame        = Count == other.Count;
        var maxPubDepthSame  = MaxAllowedSize == other.MaxAllowedSize;
        var openInterestSame = HasNonEmptyOpenInterest == other.HasNonEmptyOpenInterest;
        if (openInterestSame && other.HasNonEmptyOpenInterest && HasNonEmptyOpenInterest)
        {
            openInterestSame = openInterestSide?.AreEquivalent(other.OpenInterestSide, exactTypes) ?? false;
        }
        var bookLayersSame = true;
        for (int i = 0; i < Count; i++)
        {
            var localLayer  = this[i];
            var otherLayer  = other[i];
            var layerIsSame = localLayer.AreEquivalent(otherLayer, exactTypes);
            bookLayersSame &= layerIsSame;
        }
        var allAreSame = ladderSame && countSame && maxPubDepthSame && bookLayersSame && openInterestSame;
        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IOrderBookSide?)obj, true);

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


    protected string OrderBookSideToStringMembers =>
        $"{nameof(Capacity)}: {Capacity}, {nameof(MaxAllowedSize)}: {MaxAllowedSize}, {nameof(Count)}: {Count}, {nameof(LayerSupportedFlags)}: {LayerSupportedFlags}, " +
        $"{nameof(OpenInterestSide)}: {OpenInterestSide}, {nameof(AllLayers)}: [\n{EachLayerByIndexOnNewLines()}]";

    public override string ToString() => $"{nameof(OrderBookSide)}{{{OrderBookSideToStringMembers}}}";
}
