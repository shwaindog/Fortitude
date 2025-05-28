// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.LayerSelector;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.FeedEvents.Utils;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;

public class OrderBookSide : ReusableObject<IOrderBookSide>, IMutableOrderBookSide
{
    protected IList<IMutablePriceVolumeLayer> AllLayers;

    private readonly ListElementShiftRegistry<IMutablePriceVolumeLayer, IPriceVolumeLayer>? elementShiftRegistry;

    private ushort maxPublishDepth;

    private LayerFlags               layerFlags = LayerFlagsExtensions.PriceVolumeLayerFlags;
    private IMutableMarketAggregate? openInterestSide;

    public OrderBookSide()
    {
        elementShiftRegistry = new ListElementShiftRegistry<IMutablePriceVolumeLayer, IPriceVolumeLayer>(this, NewElementFactory, SamePrice);
        BookSide             = BookSide.Unknown;
        AllLayers            = new List<IMutablePriceVolumeLayer>();
        AllLayers.Add(LayerSelector.CreateExpectedImplementation(LayerSupportedType));
    }

    public OrderBookSide
    (BookSide bookSide, LayerType layerType = LayerType.PriceVolume, ushort maxPublishDepth = SourceTickerInfo.DefaultMaximumPublishedLayers
      , bool isLadder = false)
    {
        elementShiftRegistry = new ListElementShiftRegistry<IMutablePriceVolumeLayer, IPriceVolumeLayer>(this, NewElementFactory, SamePrice);

        BookSide        = bookSide;
        MaxPublishDepth = maxPublishDepth;
        LayerSupportedFlags = AllLayers?.FirstOrDefault()?.SupportsLayerFlags ?? LayerFlagsExtensions.PriceVolumeLayerFlags |
            (isLadder ? LayerFlags.Ladder : LayerFlags.None);
        layerFlags |= LayerSupportedType.SupportedLayerFlags();
        AllLayers  =  Enumerable.Repeat(LayerSelector.CreateExpectedImplementation(layerType), maxPublishDepth).ToList();
        IsLadder   =  isLadder;
    }

    public OrderBookSide(BookSide bookSide, ushort numBookLayers, bool isLadder = false)
    {
        elementShiftRegistry = new ListElementShiftRegistry<IMutablePriceVolumeLayer, IPriceVolumeLayer>(this, NewElementFactory, SamePrice);

        BookSide            = bookSide;
        MaxPublishDepth     = numBookLayers;
        LayerSupportedFlags = (isLadder ? LayerFlags.Ladder : LayerFlags.None);
        AllLayers           = Enumerable.Repeat(LayerSelector.CreateExpectedImplementation(LayerSupportedType), numBookLayers).ToList();
        IsLadder            = isLadder;
    }

    public OrderBookSide(BookSide bookSide, IEnumerable<IPriceVolumeLayer> bookLayers)
    {
        elementShiftRegistry = new ListElementShiftRegistry<IMutablePriceVolumeLayer, IPriceVolumeLayer>(this, NewElementFactory, SamePrice);

        BookSide = bookSide;

        LayerSupportedFlags = bookLayers.FirstOrDefault()?.SupportsLayerFlags ?? LayerFlagsExtensions.PriceVolumeLayerFlags;
        MaxPublishDepth     = (ushort)bookLayers.Count();
        this.AllLayers =
            bookLayers
                .Select(pvl => LayerSelector
                            .CreateExpectedImplementation(pvl.LayerType, pvl))
                .ToList();
    }

    public OrderBookSide(IOrderBookSide toClone)
    {
        elementShiftRegistry = new ListElementShiftRegistry<IMutablePriceVolumeLayer, IPriceVolumeLayer>(this, NewElementFactory, SamePrice);

        elementShiftRegistry.CopyFrom(toClone.ElementShifts);

        BookSide             =  toClone.BookSide;
        MaxPublishDepth      =  toClone.MaxPublishDepth;
        LayerSupportedFlags  =  toClone.LayerSupportedFlags;
        layerFlags           |= LayerSupportedType.SupportedLayerFlags();
        IsLadder             =  toClone.IsLadder;
        DailyTickUpdateCount =  toClone.DailyTickUpdateCount;
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
        elementShiftRegistry = new ListElementShiftRegistry<IMutablePriceVolumeLayer, IPriceVolumeLayer>(this, NewElementFactory, SamePrice);

        BookSide             = bookSide;

        LayerSupportedFlags =  sourceTickerInfo.LayerFlags;
        layerFlags          |= LayerSupportedType.SupportedLayerFlags();

        MaxPublishDepth = sourceTickerInfo.MaximumPublishedLayers;
        AllLayers =
            Enumerable
                .Repeat(LayerSelector.CreateExpectedImplementation(LayerSupportedType), MaxPublishDepth)
                .ToList();
    }


    protected static Func<IPriceVolumeLayer, IPriceVolumeLayer, bool> SamePrice = (lhs, rhs) => lhs.Price == rhs.Price;

    protected Func<IMutablePriceVolumeLayer> NewElementFactory => () => LayerSelector.CreateExpectedImplementation(LayerSupportedType);

    public ushort MaxPublishDepth
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

    public IReadOnlyList<ListShiftCommand> ElementShifts
    {
        get => elementShiftRegistry!.ShiftCommands;
        set => elementShiftRegistry!.ShiftCommands = (List<ListShiftCommand>)value;
    }

    public int? ClearedElementsAfterIndex
    {
        get => elementShiftRegistry!.ClearRemainingElementsAt;
        set => elementShiftRegistry!.ClearRemainingElementsAt = (ushort?)value;
    }

    public bool HasRandomAccessUpdates { get; set; }

    public void CalculateShift(DateTime asAtTime, IReadOnlyList<IPriceVolumeLayer> updatedCollection)
    {
        elementShiftRegistry!.CalculateShift(asAtTime, updatedCollection);
    }

    public ListShiftCommand InsertAtStart(IMutablePriceVolumeLayer toInsertAtStart) => elementShiftRegistry!.InsertAtStart(toInsertAtStart);

    public bool AppendAtEnd(IMutablePriceVolumeLayer toAppendAtEnd) => elementShiftRegistry!.AppendAtEnd(toAppendAtEnd);

    public ListShiftCommand InsertAt(int index, IMutablePriceVolumeLayer toInsertAtStart) => elementShiftRegistry!.InsertAt(index, toInsertAtStart);

    public ListShiftCommand DeleteAt(int index) => elementShiftRegistry!.DeleteAt(index);

    public ListShiftCommand Delete(IMutablePriceVolumeLayer toDelete) => elementShiftRegistry!.Delete(toDelete);

    public ListShiftCommand ApplyElementShift(ListShiftCommand shiftCommandToApply) => elementShiftRegistry!.ApplyElementShift(shiftCommandToApply);

    public ListShiftCommand MoveToStart(IMutablePriceVolumeLayer existingItem) => elementShiftRegistry!.MoveToStart(existingItem);

    public ListShiftCommand MoveSingleElementToStart(int indexToMoveToStart) => elementShiftRegistry!.MoveToStart(indexToMoveToStart);

    public ListShiftCommand MoveSingleElementToEnd(int indexToMoveToEnd) => elementShiftRegistry!.MoveToEnd(indexToMoveToEnd);

    public ListShiftCommand MoveSingleElementBy(int indexToMoveToEnd, int shift) =>
        elementShiftRegistry!.MoveSingleElementBy(indexToMoveToEnd, shift);

    public ListShiftCommand MoveSingleElementBy(IMutablePriceVolumeLayer existingItem, int shift) =>
        elementShiftRegistry!.MoveSingleElementBy(existingItem, shift);

    public ListShiftCommand MoveToEnd(IMutablePriceVolumeLayer existingItem) => elementShiftRegistry!.MoveToEnd(existingItem);

    public ListShiftCommand ClearAll() => elementShiftRegistry!.ClearAll();

    public ListShiftCommand ShiftElementsFrom(int byElements, int pinElementsFromIndex) =>
        elementShiftRegistry!.ShiftElementsFrom(byElements, pinElementsFromIndex);

    public ListShiftCommand ShiftElementsUntil(int byElements, int pinElementsFromIndex) =>
        elementShiftRegistry!.ShiftElementsUntil(byElements, pinElementsFromIndex);

    IPriceVolumeLayer ISupportsElementsShift<IOrderBookSide, IPriceVolumeLayer>.this[int index] => this[index];

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
        AllLayers.Add(item);
    }

    public void Clear()
    {
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

    public bool Remove(IMutablePriceVolumeLayer item) => AllLayers.Remove(item);

    public bool IsReadOnly => false;

    public int IndexOf(IMutablePriceVolumeLayer item) => AllLayers.IndexOf(item);

    public void Insert(int index, IMutablePriceVolumeLayer item)
    {
        AllLayers.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        AllLayers.RemoveAt(index);
    }

    public int AppendEntryAtEnd()
    {
        var index = AllLayers.Count;
        AllLayers.Add(LayerSelector.CreateExpectedImplementation(LayerSupportedType));
        return index;
    }

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
        var maxPubDepthSame  = MaxPublishDepth == other.MaxPublishDepth;
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


    public OrderBookSide ResetWithTracking()
    {
        for (var i = 0; i < AllLayers.Count; i++) (AllLayers[i]).ResetWithTracking();
        openInterestSide?.ResetWithTracking();

        return this;
    }

    IMutableOrderBookSide ITrackableReset<IMutableOrderBookSide>.ResetWithTracking() => ResetWithTracking();

    public override void StateReset()
    {
        for (var i = 0; i < AllLayers.Count; i++) (AllLayers[i]).StateReset();
        openInterestSide?.StateReset();
        base.StateReset();
    }


    public override OrderBookSide CopyFrom(IOrderBookSide source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        LayerSupportedFlags = source.LayerSupportedFlags;
        MaxPublishDepth     = source.MaxPublishDepth;
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
        $"{nameof(Capacity)}: {Capacity}, {nameof(MaxPublishDepth)}: {MaxPublishDepth}, {nameof(Count)}: {Count}, {nameof(LayerSupportedFlags)}: {LayerSupportedFlags}, " +
        $"{nameof(OpenInterestSideSide)}: {OpenInterestSideSide}, {nameof(AllLayers)}: [{string.Join(", ", AllLayers.Take(Count))}]";

    public override string ToString() => $"{nameof(OrderBookSide)}{{{OrderBookSideToStringMembers}}}";
}
