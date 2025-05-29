// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.LayerSelector;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;

public class OrderBookSide : ReusableObject<IOrderBookSide>, IMutableOrderBookSide
{
    protected IList<IMutablePriceVolumeLayer> AllLayers;

    private readonly TrackShiftsListRegistry<IMutablePriceVolumeLayer, IPriceVolumeLayer>? elementShiftRegistry;

    private ushort maxPublishDepth;

    private LayerFlags layerFlags = LayerFlagsExtensions.PriceVolumeLayerFlags;

    private IMutableMarketAggregate? openInterestSide;

    public OrderBookSide()
    {
        elementShiftRegistry = new TrackShiftsListRegistry<IMutablePriceVolumeLayer, IPriceVolumeLayer>(this, NewElementFactory, SamePrice);

        BookSide  = BookSide.Unknown;
        AllLayers = new List<IMutablePriceVolumeLayer>();

        AllLayers.Add(LayerSelector.CreateExpectedImplementation(LayerSupportedType));
    }

    public OrderBookSide
    (BookSide bookSide, LayerType layerType = LayerType.PriceVolume, ushort maxPublishDepth = SourceTickerInfo.DefaultMaximumPublishedLayers
      , bool isLadder = false)
    {
        elementShiftRegistry = new TrackShiftsListRegistry<IMutablePriceVolumeLayer, IPriceVolumeLayer>(this, NewElementFactory, SamePrice);
        LayerSupportedFlags = AllLayers?.FirstOrDefault()?.SupportsLayerFlags ?? LayerFlagsExtensions.PriceVolumeLayerFlags |
            (isLadder ? LayerFlags.Ladder : LayerFlags.None);

        MaxAllowedSize = maxPublishDepth;

        BookSide   =  bookSide;
        layerFlags |= LayerSupportedType.SupportedLayerFlags();
        AllLayers  =  Enumerable.Repeat(LayerSelector.CreateExpectedImplementation(layerType), maxPublishDepth).ToList();
        IsLadder   =  isLadder;
    }

    public OrderBookSide(BookSide bookSide, ushort numBookLayers, bool isLadder = false)
    {
        elementShiftRegistry = new TrackShiftsListRegistry<IMutablePriceVolumeLayer, IPriceVolumeLayer>(this, NewElementFactory, SamePrice);
        LayerSupportedFlags  = (isLadder ? LayerFlags.Ladder : LayerFlags.None);

        MaxAllowedSize = numBookLayers;

        BookSide  = bookSide;
        AllLayers = Enumerable.Repeat(LayerSelector.CreateExpectedImplementation(LayerSupportedType), numBookLayers).ToList();
        IsLadder  = isLadder;
    }

    public OrderBookSide(BookSide bookSide, IEnumerable<IPriceVolumeLayer> bookLayers)
    {
        elementShiftRegistry = new TrackShiftsListRegistry<IMutablePriceVolumeLayer, IPriceVolumeLayer>(this, NewElementFactory, SamePrice);
        LayerSupportedFlags  = bookLayers.FirstOrDefault()?.SupportsLayerFlags ?? LayerFlagsExtensions.PriceVolumeLayerFlags;

        MaxAllowedSize = (ushort)bookLayers.Count();

        BookSide = bookSide;
        AllLayers =
            bookLayers
                .Select(pvl => LayerSelector
                            .CreateExpectedImplementation(pvl.LayerType, pvl))
                .ToList();
    }

    public OrderBookSide(IOrderBookSide toClone)
    {
        elementShiftRegistry = new TrackShiftsListRegistry<IMutablePriceVolumeLayer, IPriceVolumeLayer>(this, NewElementFactory, SamePrice);
        elementShiftRegistry.CopyFrom(toClone.ShiftCommands);

        HasUnreliableListTracking = toClone.HasUnreliableListTracking;
        LayerSupportedFlags  = toClone.LayerSupportedFlags;
        DailyTickUpdateCount = toClone.DailyTickUpdateCount;

        MaxAllowedSize = toClone.MaxAllowedSize;


        BookSide   =  toClone.BookSide;
        layerFlags |= LayerSupportedType.SupportedLayerFlags();
        IsLadder   =  toClone.IsLadder;

        if (toClone.HasNonEmptyOpenInterest)
        {
            openInterestSide = new MarketAggregate(toClone.OpenInterestSide);
        }
        AllLayers =
            toClone
                .Select
                    (pvl => LayerSelector.CreateExpectedImplementation(pvl.LayerType, pvl))
                .ToList();
        Capacity = toClone.Capacity;
    }

    public OrderBookSide(BookSide bookSide, ISourceTickerInfo sourceTickerInfo)
    {
        elementShiftRegistry = new TrackShiftsListRegistry<IMutablePriceVolumeLayer, IPriceVolumeLayer>(this, NewElementFactory, SamePrice);
        LayerSupportedFlags  = sourceTickerInfo.LayerFlags;

        MaxAllowedSize = sourceTickerInfo.MaximumPublishedLayers;

        BookSide   =  bookSide;
        layerFlags |= LayerSupportedType.SupportedLayerFlags();
        AllLayers =
            Enumerable
                .Repeat(LayerSelector.CreateExpectedImplementation(LayerSupportedType), MaxAllowedSize)
                .ToList();
    }


    protected static Func<IPriceVolumeLayer, IPriceVolumeLayer, bool> SamePrice = (lhs, rhs) => lhs.Price == rhs.Price;

    protected Func<IMutablePriceVolumeLayer> NewElementFactory => () => LayerSelector.CreateExpectedImplementation(LayerSupportedType);

    ushort IMutableTracksShiftsList<IMutablePriceVolumeLayer, IPriceVolumeLayer>.MaxAllowedSize
    {
        get => MaxAllowedSize;
        set => MaxAllowedSize = value;
    }

    ushort IMutableCappedCapacityList<IMutablePriceVolumeLayer>.MaxAllowedSize
    {
        get => MaxAllowedSize;
        set => MaxAllowedSize = value;
    }

    public ushort MaxAllowedSize
    {
        get => maxPublishDepth;
        private set => maxPublishDepth = Math.Max((byte)1, Math.Min(value, PQFeedFieldsExtensions.TwoByteFieldIdMaxBookDepth));
    }

    public static ILayerFlagsSelector<IMutablePriceVolumeLayer> LayerSelector { get; set; } = new OrderBookLayerFactorySelector();

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

    IMarketAggregate IOrderBookSide.OpenInterestSide => OpenInterestSideSide!;

    IMutableMarketAggregate? IMutableOrderBookSide.OpenInterestSide
    {
        get => OpenInterestSideSide;
        set => OpenInterestSideSide = (MarketAggregate?)value;
    }

    public IMutableMarketAggregate? OpenInterestSideSide
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

    public bool IsEmpty
    {
        get => AllLayers.All(pvl => pvl.IsEmpty);
        set
        {
            foreach (var priceVolumeLayer in AllLayers)
            {
                priceVolumeLayer.IsEmpty = value;
            }
        }
    }

    public IReadOnlyList<ListShiftCommand> ShiftCommands
    {
        get => elementShiftRegistry!.ShiftCommands;
        set => elementShiftRegistry!.ShiftCommands = (List<ListShiftCommand>)value;
    }

    public int? ClearRemainingElementsFromIndex
    {
        get => elementShiftRegistry!.ClearRemainingElementsFromIndex;
        set => elementShiftRegistry!.ClearRemainingElementsFromIndex = (ushort?)value;
    }

    public bool HasUnreliableListTracking { get; set; }

    public bool CalculateShift(DateTime asAtTime, IReadOnlyList<IPriceVolumeLayer> updatedCollection) =>
        elementShiftRegistry!.CalculateShift(asAtTime, updatedCollection);

    ListShiftCommand IMutableTracksShiftsList<IMutablePriceVolumeLayer, IPriceVolumeLayer>.AppendShiftCommand
        (ListShiftCommand toAppendAtEnd) =>
        elementShiftRegistry!.AppendShiftCommand(toAppendAtEnd);

    ListShiftCommand IMutableOrderBookSide.AppendShiftCommand(ListShiftCommand toAppendAtEnd) =>
        elementShiftRegistry!.AppendShiftCommand(toAppendAtEnd);

    public void ClearShiftCommands()
    {
        elementShiftRegistry!.ClearShiftCommands();
    }

    public ListShiftCommand ShiftElements(int byElements) => elementShiftRegistry!.ShiftElements(byElements);

    public ListShiftCommand InsertAtStart(IMutablePriceVolumeLayer toInsertAtStart) => elementShiftRegistry!.InsertAtStart(toInsertAtStart);

    public bool AppendAtEnd(IMutablePriceVolumeLayer toAppendAtEnd) => elementShiftRegistry!.AppendAtEnd(toAppendAtEnd);

    public ListShiftCommand InsertAt(int index, IMutablePriceVolumeLayer toInsertAtStart) => elementShiftRegistry!.InsertAt(index, toInsertAtStart);

    public ListShiftCommand DeleteAt(int index) => elementShiftRegistry!.DeleteAt(index);

    public ListShiftCommand Delete(IMutablePriceVolumeLayer toDelete) => elementShiftRegistry!.Delete(toDelete);

    public ListShiftCommand ApplyListShiftCommand
        (ListShiftCommand shiftCommandToApply) =>
        elementShiftRegistry!.ApplyListShiftCommand(shiftCommandToApply);

    public ListShiftCommand ClearAll() => elementShiftRegistry!.ClearAll();

    IPriceVolumeLayer IOrderBookSide.this[int index] => this[index];

    public IMutablePriceVolumeLayer this[int level]
    {
        get => level < AllLayers.Count && level >= 0 ? AllLayers[level] : AllLayers[0];
        set => AllLayers[level] = value;
    }

    public BookSide BookSide { get; }

    IPriceVolumeLayer IReadOnlyList<IPriceVolumeLayer>.this[int level] => this[level];

    public int Count
    {
        get
        {
            for (var i = AllLayers.Count - 1; i >= 0; i--)
            {
                var layerAtLevel = AllLayers[i];
                if (!layerAtLevel.IsEmpty) return i + 1;
            }

            return 0;
        }
        set
        {
            for (var i = Capacity; i >= value; i--)
            {
                var layerAtLevel = AllLayers[i];
                layerAtLevel.IsEmpty = true;
            }
        }
    }

    public int Capacity
    {
        get => AllLayers.Count;
        set
        {
            if (value > PQFeedFieldsExtensions.SingleByteFieldIdMaxBookDepth)
                throw new ArgumentException("Expected OrderBook Capacity to be less than or equal to " +
                                            PQFeedFieldsExtensions.SingleByteFieldIdMaxBookDepth);
            while (AllLayers.Count < value)
            {
                var cloneFirstLayer = LayerSelector.CreateExpectedImplementation(LayerSupportedType);
                cloneFirstLayer.StateReset();
                AllLayers.Add(cloneFirstLayer);
            }
        }
    }

    public void Add(IMutablePriceVolumeLayer item)
    {
        HasUnreliableListTracking = ShiftCommands.Any();
        AllLayers.Add(item);
    }

    public void Clear()
    {
        HasUnreliableListTracking = ShiftCommands.Any();
        AllLayers.Clear();
    }

    public bool Contains(IMutablePriceVolumeLayer item) => AllLayers.Contains(item);

    public void CopyTo(IMutablePriceVolumeLayer[] array, int arrayIndex)
    {
        for (int i = 0; i < AllLayers.Count && i + arrayIndex < array.Length; i++)
        {
            array[i + arrayIndex] = AllLayers[i];
        }
    }

    public bool Remove(IMutablePriceVolumeLayer item)
    {
        HasUnreliableListTracking = ShiftCommands.Any();
        return AllLayers.Remove(item);
    }

    public bool IsReadOnly => false;

    public int IndexOf(IMutablePriceVolumeLayer item) => AllLayers.IndexOf(item);

    public void Insert(int index, IMutablePriceVolumeLayer item)
    {
        HasUnreliableListTracking = ShiftCommands.Any();
        AllLayers.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        HasUnreliableListTracking = ShiftCommands.Any();
        AllLayers.RemoveAt(index);
    }

    public int AppendEntryAtEnd()
    {
        HasUnreliableListTracking = ShiftCommands.Any();
        var index = AllLayers.Count;
        AllLayers.Add(LayerSelector.CreateExpectedImplementation(LayerSupportedType));
        return index;
    }

    IMutableOrderBookSide ICloneable<IMutableOrderBookSide>.Clone() => Clone();

    IMutableOrderBookSide IMutableOrderBookSide.Clone() => Clone();

    IOrderBookSide ICloneable<IOrderBookSide>.Clone() => Clone();

    public override OrderBookSide Clone() => Recycler?.Borrow<OrderBookSide>().CopyFrom(this) ?? new OrderBookSide(this);

    public bool AreEquivalent(IOrderBookSide? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;

        var ladderSame       = IsLadder == other.IsLadder;
        var countSame        = Count == other.Count;
        var maxPubDepthSame  = MaxAllowedSize == other.MaxAllowedSize;
        var openInterestSame = HasNonEmptyOpenInterest == other.HasNonEmptyOpenInterest;
        if (openInterestSame && other.HasNonEmptyOpenInterest && HasNonEmptyOpenInterest)
        {
            openInterestSame = Equals(openInterestSide, other.OpenInterestSide);
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

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<IPriceVolumeLayer> IEnumerable<IPriceVolumeLayer>.GetEnumerator() => GetEnumerator();

    public IEnumerator<IMutablePriceVolumeLayer> GetEnumerator() => AllLayers.Take(Count).GetEnumerator();


    IMutableOrderBookSide ITrackableReset<IMutableOrderBookSide>.ResetWithTracking() => ResetWithTracking();


    ITracksResetCappedCapacityList<IMutablePriceVolumeLayer> ITrackableReset<ITracksResetCappedCapacityList<IMutablePriceVolumeLayer>>.
        ResetWithTracking() => ResetWithTracking();

    IMutableOrderBookSide IMutableOrderBookSide.ResetWithTracking() => ResetWithTracking();

    public OrderBookSide ResetWithTracking()
    {
        for (var i = 0; i < AllLayers.Count; i++) (AllLayers[i]).ResetWithTracking();
        openInterestSide?.ResetWithTracking();

        return this;
    }

    public override void StateReset()
    {
        for (var i = 0; i < AllLayers.Count; i++) (AllLayers[i]).StateReset();
        openInterestSide?.StateReset();
        base.StateReset();
    }


    public override OrderBookSide CopyFrom(IOrderBookSide source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        LayerSupportedFlags = source.LayerSupportedFlags;
        MaxAllowedSize      = source.MaxAllowedSize;
        if (source.HasNonEmptyOpenInterest)
        {
            openInterestSide ??= new MarketAggregate();
            openInterestSide.CopyFrom(source.OpenInterestSide, copyMergeFlags);
        }
        else if (openInterestSide != null)
        {
            openInterestSide.IsEmpty = true;
        }
        var originalCount   = Count;
        var allSourceLayers = source.Capacity;
        for (var i = 0; i < allSourceLayers; i++)
        {
            var sourceLayer = source[i];
            if (sourceLayer is null or { IsEmpty: true } && i >= originalCount) continue;
            var destinationLayer = this[i];

            if (i < AllLayers.Count)
                AllLayers[i] = LayerSelector.UpgradeExistingLayer(destinationLayer, LayerSupportedType, sourceLayer);
            else
                AllLayers.Add(LayerSelector.CreateExpectedImplementation(LayerSupportedType, sourceLayer));

            if (sourceLayer is { IsEmpty: false }) continue;
            if (destinationLayer is { } mutablePriceVolumeLayer) mutablePriceVolumeLayer.IsEmpty = true;
        }

        for (var i = source.Count; i < AllLayers.Count; i++) AllLayers[i].IsEmpty = true;
        return this;
    }


    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IOrderBookSide?)obj, true);

    public override int GetHashCode() => AllLayers.GetHashCode();

    protected string OrderBookSideToStringMembers =>
        $"{nameof(Capacity)}: {Capacity}, {nameof(MaxAllowedSize)}: {MaxAllowedSize}, {nameof(Count)}: {Count}, {nameof(LayerSupportedFlags)}: {LayerSupportedFlags}, " +
        $"{nameof(OpenInterestSideSide)}: {OpenInterestSideSide}, {nameof(AllLayers)}: [{string.Join(", ", AllLayers.Take(Count))}]";

    public override string ToString() => $"{nameof(OrderBookSide)}{{{OrderBookSideToStringMembers}}}";
}
