// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;

public class OrdersPriceVolumeLayer : OrdersCountPriceVolumeLayer, IMutableOrdersPriceVolumeLayer
{
    private readonly bool isCounterPartyOrders;

    private readonly TracksReorderingListRegistry<IMutableAnonymousOrder, IAnonymousOrder> elementShiftRegistry;

    private readonly IList<IMutableAnonymousOrder> orders;

    public OrdersPriceVolumeLayer()
    {
        orders = new List<IMutableAnonymousOrder>();

        elementShiftRegistry = new TracksReorderingListRegistry<IMutableAnonymousOrder, IAnonymousOrder>(this, NewElementFactory, SameTradeId);
    }

    public OrdersPriceVolumeLayer(LayerType layerType)
    {
        isCounterPartyOrders =
            layerType switch
            {
                LayerType.OrdersAnonymousPriceVolume => false
              , LayerType.OrdersFullPriceVolume => true
              , LayerType.FullSupportPriceVolume => true
              , _ => throw new ArgumentException($"Only expected to receive OrdersAnonymousPriceVolume or OrdersFullPriceVolume but got {layerType}")
            };

        orders = new List<IMutableAnonymousOrder>();

        elementShiftRegistry = new TracksReorderingListRegistry<IMutableAnonymousOrder, IAnonymousOrder>(this, NewElementFactory, SameTradeId);
    }

    public OrdersPriceVolumeLayer
    (LayerType layerType = LayerType.OrdersAnonymousPriceVolume
      , decimal price = 0m, decimal volume = 0m, uint ordersCount = 0, decimal internalVolume = 0
      , IEnumerable<IAnonymousOrder>? layerOrders = null) : base(price, volume, ordersCount, internalVolume)
    {
        isCounterPartyOrders =
            layerType switch
            {
                LayerType.OrdersAnonymousPriceVolume => false
              , LayerType.OrdersFullPriceVolume => true
              , LayerType.FullSupportPriceVolume => true
              , _ => throw new ArgumentException($"Only expected to receive OrdersAnonymousPriceVolume or OrdersFullPriceVolume but got {layerType}")
            };
        orders = layerOrders?.OfType<IMutableAnonymousOrder>().ToList() ?? new List<IMutableAnonymousOrder>();

        elementShiftRegistry = new TracksReorderingListRegistry<IMutableAnonymousOrder, IAnonymousOrder>(this, NewElementFactory, SameTradeId);
    }

    public OrdersPriceVolumeLayer(IPriceVolumeLayer toClone, LayerType layerType) : base(toClone)
    {
        var asLayerType = layerType;
        isCounterPartyOrders =
            isCounterPartyOrders =
                asLayerType switch
                {
                    LayerType.OrdersAnonymousPriceVolume => false
                  , LayerType.OrdersFullPriceVolume      => true
                  , LayerType.FullSupportPriceVolume     => true
                  , _ => throw new
                        ArgumentException($"Only expected to receive OrdersAnonymousPriceVolume or OrdersFullPriceVolume but got {asLayerType}")
                };
        if (toClone is IOrdersPriceVolumeLayer ordersToClone)
        {
            orders = new List<IMutableAnonymousOrder>((int)ordersToClone.OrdersCount);
            foreach (var orderLayerInfo in ordersToClone.Orders) AddLayer(orderLayerInfo);
        }
        else
        {
            orders = new List<IMutableAnonymousOrder>(0);
        }

        elementShiftRegistry = new TracksReorderingListRegistry<IMutableAnonymousOrder, IAnonymousOrder>(this, NewElementFactory, SameTradeId);
    }

    protected static Func<IAnonymousOrder, IAnonymousOrder, bool> SameTradeId = (lhs, rhs) => lhs.OrderId == rhs.OrderId;

    protected Func<IMutableAnonymousOrder> NewElementFactory => CreateNewBookOrderLayer;

    [JsonIgnore] public IReadOnlyList<IMutableAnonymousOrder> Orders => orders.Where(aoli => !aoli.IsEmpty).ToList().AsReadOnly();

    IReadOnlyList<IAnonymousOrder> IOrdersPriceVolumeLayer.Orders => orders.Where(aoli => !aoli.IsEmpty).ToList().AsReadOnly();


    [JsonIgnore] public override LayerType LayerType => isCounterPartyOrders ? LayerType.OrdersFullPriceVolume : LayerType.OrdersAnonymousPriceVolume;


    [JsonIgnore]
    public override LayerFlags SupportsLayerFlags =>
        isCounterPartyOrders
            ? LayerFlagsExtensions.AdditionalCounterPartyOrderFlags | base.SupportsLayerFlags
            : LayerFlagsExtensions.AdditionalAnonymousOrderFlags | base.SupportsLayerFlags;

    public ushort MaxAllowedSize { get; set; } = PQFeedFieldsExtensions.TwoByteFieldIdMaxBookDepth;

    IAnonymousOrder IReadOnlyList<IAnonymousOrder>.this[int index] => this[index];

    [JsonIgnore]
    public IMutableAnonymousOrder this[int i]
    {
        get
        {
            AssertMaxOrdersNotExceeded(i);
            for (var j = orders.Count; j <= i; j++) orders.Add(CreateNewBookOrderLayer());
            return orders[i];
        }
        set
        {
            AssertMaxOrdersNotExceeded(i);
            for (var j = orders.Count; j < i; j++) orders.Add(CreateNewBookOrderLayer());
            if (i < orders.Count)
            {
                orders[i] = value;
            }
            else
            {
                orders.Add(value);
            }
        }
    }

    int IReadOnlyCollection<IAnonymousOrder>.Count => (int)OrdersCount;

    int IReadOnlyCollection<IMutableAnonymousOrder>.Count => (int)OrdersCount;

    int ICollection<IMutableAnonymousOrder>.Count => (int)OrdersCount;

    int IMutableCapacityList<IMutableAnonymousOrder>.Count
    {
        get => (int)OrdersCount;
        set => OrdersCount = (uint)value;
    }
    int IMutableOrdersPriceVolumeLayer.Count
    {
        get => (int)OrdersCount;
        set => OrdersCount = (uint)value;
    }

    public override uint OrdersCount
    {
        get
        {
            var calcOrderCount = CountFromOrders();
            if (calcOrderCount > 0) return (uint)calcOrderCount;
            return base.OrdersCount;
        }
        set
        {
            base.OrdersCount = value;
            if (orders != null!)
            {
                for (var i = (orders.Count) - 1; i >= value; i--)
                {
                    var layerAtLevel = orders[i];

                    if (!layerAtLevel.IsEmpty) layerAtLevel.IsEmpty = true;
                }
            }
        }
    }

    public int Capacity
    {
        get => orders.Count;
        set
        {
            if (value > PQFeedFieldsExtensions.SingleByteFieldIdMaxPossibleLastTrades)
                throw new ArgumentException("Expected PQRecentlyTraded Capacity to be less than or equal to " +
                                            PQFeedFieldsExtensions.SingleByteFieldIdMaxPossibleLastTrades);
            var orderCount = CountFromOrders();
            while (orderCount < Math.Min(MaxAllowedSize, value))
            {
                var firstLastTrade = CreateNewBookOrderLayer();
                firstLastTrade.StateReset();
                orders.Add(firstLastTrade);
                orderCount++;
            }
        }
    }

    public override decimal InternalVolume
    {
        get
        {
            var calcInternalVolume = InternalVolumeFromOrders();
            if (calcInternalVolume > 0) return calcInternalVolume;
            return base.InternalVolume;
        }
        set => base.InternalVolume = value;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public override bool IsEmpty
    {
        get => base.IsEmpty && Orders.All(tli => tli.IsEmpty);
        set
        {
            base.IsEmpty = value;
            if (!value) return;
            foreach (var orderLayerInfo in orders) orderLayerInfo.IsEmpty = true;
            base.IsEmpty = true;
        }
    }

    public IReadOnlyList<ListShiftCommand> ShiftCommands
    {
        get => elementShiftRegistry.ShiftCommands;
        set => elementShiftRegistry.ShiftCommands = value;
    }

    public int? ClearRemainingElementsFromIndex
    {
        get => elementShiftRegistry.ClearRemainingElementsFromIndex;
        set => elementShiftRegistry.ClearRemainingElementsFromIndex = value;
    }

    public bool HasUnreliableListTracking
    {
        get => elementShiftRegistry.HasUnreliableListTracking;
        set => elementShiftRegistry.HasUnreliableListTracking = value;
    }

    public bool CalculateShift(DateTime asAtTime, IReadOnlyList<IAnonymousOrder> updatedCollection) =>
        elementShiftRegistry.CalculateShift(asAtTime, updatedCollection);


    public ListShiftCommand AppendShiftCommand(ListShiftCommand toAppendAtEnd) => elementShiftRegistry.AppendShiftCommand(toAppendAtEnd);

    public void ClearShiftCommands() => elementShiftRegistry.ClearShiftCommands();

    public ListShiftCommand InsertAtStart(IMutableAnonymousOrder toInsertAtStart) => elementShiftRegistry.InsertAtStart(toInsertAtStart);

    public bool AppendAtEnd(IMutableAnonymousOrder toAppendAtEnd) => elementShiftRegistry.AppendAtEnd(toAppendAtEnd);

    public ListShiftCommand InsertAt(int index, IMutableAnonymousOrder toInsertAtStart) => elementShiftRegistry.InsertAt(index, toInsertAtStart);

    public ListShiftCommand DeleteAt(int index) => elementShiftRegistry.DeleteAt(index);

    public ListShiftCommand Delete(IMutableAnonymousOrder toDelete) => elementShiftRegistry.Delete(toDelete);

    public ListShiftCommand ApplyListShiftCommand(ListShiftCommand shiftCommandToApply) => elementShiftRegistry.ApplyListShiftCommand(shiftCommandToApply);

    public ListShiftCommand ClearAll() => elementShiftRegistry.ClearAll();

    public ListShiftCommand ShiftElements(int byElements) => elementShiftRegistry.ShiftElements(byElements);

    public ListShiftCommand MoveToStart(IMutableAnonymousOrder existingItem) => elementShiftRegistry.MoveToStart(existingItem);

    public ListShiftCommand MoveToStart(int indexToMoveToStart) => elementShiftRegistry.MoveToStart(indexToMoveToStart);

    public ListShiftCommand MoveToEnd(int indexToMoveToEnd) => elementShiftRegistry.MoveToEnd(indexToMoveToEnd);

    public ListShiftCommand MoveSingleElementBy(int indexToMoveToEnd, int shift) => elementShiftRegistry.MoveSingleElementBy(indexToMoveToEnd, shift);

    public ListShiftCommand MoveSingleElementBy(IMutableAnonymousOrder existingItem, int shift) =>
        elementShiftRegistry.MoveSingleElementBy(existingItem, shift);

    public ListShiftCommand MoveToEnd(IMutableAnonymousOrder existingItem) => elementShiftRegistry.MoveToEnd(existingItem);

    public ListShiftCommand ShiftElementsFrom(int byElements, int pinElementsFromIndex) => elementShiftRegistry.ShiftElementsFrom(byElements, pinElementsFromIndex);

    public ListShiftCommand ShiftElementsUntil(int byElements, int pinElementsFromIndex) => elementShiftRegistry.ShiftElementsUntil(byElements, pinElementsFromIndex);

    public void Add(IMutableAnonymousOrder item)
    {
        var index = CountFromOrders();
        if (index < Capacity)
        {
            this[index] = item;
        }
        else
        {
            orders.Add(item);
        }
    }

    public void Clear()
    {
        orders.Clear();
    }

    public bool Contains(IMutableAnonymousOrder item) => orders.Contains(item);

    public void CopyTo(IMutableAnonymousOrder[] array, int arrayIndex)
    {
        for (int i = 0; i < orders.Count && i + arrayIndex < array.Length; i++)
        {
            array[i + arrayIndex] = orders[i];
        }
    }

    public bool Remove(IMutableAnonymousOrder item) => orders.Remove(item);

    public bool IsReadOnly => false;

    public int IndexOf(IMutableAnonymousOrder item) => orders.IndexOf(item);

    public void Insert(int index, IMutableAnonymousOrder item) => orders.Insert(index, item);

    public void Add(IAnonymousOrder order)
    {
        var indexToUpdate = CountFromOrders();
        AssertMaxOrdersNotExceeded(indexToUpdate);
        if (indexToUpdate >= orders.Count)
        {
            AddLayer(order);
        }
        else
        {
            var entryToUpdate = orders[indexToUpdate];
            entryToUpdate.CopyFrom(order, CopyMergeFlags.FullReplace);
        }
    }

    public void RemoveAt(int index)
    {
        var orderAt = orders[index];
        orderAt.IsEmpty = true;
    }

    IMutableOrdersPriceVolumeLayer ITrackableReset<IMutableOrdersPriceVolumeLayer>.ResetWithTracking() => ResetWithTracking();

    IMutableOrdersPriceVolumeLayer IMutableOrdersPriceVolumeLayer.ResetWithTracking() => ResetWithTracking();

    ITracksResetCappedCapacityList<IMutableAnonymousOrder> ITrackableReset<ITracksResetCappedCapacityList<IMutableAnonymousOrder>>.
        ResetWithTracking() =>
        ResetWithTracking();

    public override OrdersPriceVolumeLayer ResetWithTracking()
    {
        foreach (var orderLayerInfo in Orders) orderLayerInfo.StateReset();
        base.ResetWithTracking();
        return this;
    }

    public override void StateReset()
    {
        foreach (var orderLayerInfo in Orders) orderLayerInfo.StateReset();
        base.StateReset();
    }

    IOrdersPriceVolumeLayer ICloneable<IOrdersPriceVolumeLayer>.Clone() => Clone();

    IOrdersPriceVolumeLayer IOrdersPriceVolumeLayer.Clone() => Clone();

    IMutableOrdersPriceVolumeLayer IMutableOrdersPriceVolumeLayer.Clone() => Clone();

    public override OrdersPriceVolumeLayer Clone() =>
        Recycler?.Borrow<OrdersPriceVolumeLayer>().CopyFrom(this)
     ?? new OrdersPriceVolumeLayer(this, LayerType);

    private void AddLayer(IAnonymousOrder toAdd)
    {
        orders.Add(ConvertToBookLayer(toAdd));
    }

    public IMutableAnonymousOrder ConvertToBookLayer(IAnonymousOrder toAdd)
    {
        if (LayerType.SupportsOrdersFullPriceVolume())
            return new ExternalCounterPartyOrder(toAdd)
            {
                Recycler = Recycler
            };
        return new AnonymousOrder(toAdd)
        {
            Recycler = Recycler
        };
    }

    public IMutableAnonymousOrder CreateNewBookOrderLayer()
    {
        if (LayerType.SupportsOrdersFullPriceVolume())
            return new ExternalCounterPartyOrder
            {
                Recycler = Recycler
            };
        return new AnonymousOrder
        {
            Recycler = Recycler
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<IAnonymousOrder> IEnumerable<IAnonymousOrder>.GetEnumerator() => GetEnumerator();

    IEnumerator<IMutableAnonymousOrder> IEnumerable<IMutableAnonymousOrder>.GetEnumerator() => GetEnumerator();

    public IEnumerator<IMutableAnonymousOrder> GetEnumerator() => orders.Take(CountFromOrders()).GetEnumerator();


    [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
    private void AssertMaxOrdersNotExceeded(int proposedNewIndex)
    {
        if (proposedNewIndex > ushort.MaxValue) throw new ArgumentOutOfRangeException($"Max Traders represented is {ushort.MaxValue}");
    }

    protected int CountFromOrders()
    {
        for (var i = (orders.Count) - 1; i >= 0; i--)
        {
            var layerAtLevel = orders[i];
            if (!layerAtLevel.IsEmpty) return (i + 1);
        }

        return 0;
    }

    protected decimal InternalVolumeFromOrders()
    {
        return orders
               .Where(aoli =>
                          aoli.GenesisFlags.IsInternalOrder()
                       && !aoli.GenesisFlags.HasVolumeNotPartOfLiquidity()
                       && !aoli.GenesisFlags.HasSyntheticForBacktestSimulation())
               .Sum(aoli => aoli.OrderRemainingVolume);
    }

    public override bool AreEquivalent(IPriceVolumeLayer? other, bool exactTypes = false)
    {
        if (!(other is IOrdersPriceVolumeLayer otherTvl)) return false;
        var baseSame = base.AreEquivalent(other, exactTypes);
        var traderDetailsSame = orders.Zip(otherTvl.Orders,
                                           (ftd, std) => ftd.AreEquivalent(std, exactTypes))
                                      .All(same => same);
        return baseSame && traderDetailsSame;
    }

    public override OrdersPriceVolumeLayer CopyFrom(IPriceVolumeLayer source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        var thisLayerGenesisFlags = LayerType.SupportsOrdersFullPriceVolume()
            ? IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags
            : OrderGenesisFlags.None;
        var addInfoMask = IAnonymousOrder.AllExceptExtraInfoFlags;
        if (source is IMutableOrdersPriceVolumeLayer ordersCountPriceVolumeLayer)
        {
            for (var i = 0; i < ordersCountPriceVolumeLayer.OrdersCount; i++)
            {
                var destOrder = ordersCountPriceVolumeLayer[i];
                if (i < orders.Count)
                {
                    var mutableAnonymousOrder = orders[i];
                    mutableAnonymousOrder.CopyFrom(ordersCountPriceVolumeLayer[i]);
                    var modifiedGenesisFlags = (destOrder.GenesisFlags & addInfoMask);
                    destOrder.GenesisFlags                        = modifiedGenesisFlags | thisLayerGenesisFlags;
                    mutableAnonymousOrder.EmptyIgnoreGenesisFlags = thisLayerGenesisFlags;
                }
                else
                {
                    var mutableAnonymousOrder = ConvertToBookLayer(ordersCountPriceVolumeLayer[i]);
                    var modifiedGenesisFlags  = (destOrder.GenesisFlags & addInfoMask);
                    destOrder.GenesisFlags                        = modifiedGenesisFlags | thisLayerGenesisFlags;
                    mutableAnonymousOrder.EmptyIgnoreGenesisFlags = thisLayerGenesisFlags;
                    orders.Add(mutableAnonymousOrder);
                }
            }

            for (var i = orders.Count - 1; i >= ordersCountPriceVolumeLayer.OrdersCount; i--) orders[i].StateReset();
        }
        return this;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IOrdersPriceVolumeLayer?)obj, true);

    public override int GetHashCode() => base.GetHashCode() ^ Orders.GetHashCode();

    protected string OrdersPriceVolumeLayerToStringMembers => $"{OrdersCountPriceVolumeLayerToStringMembers}, {JustOrdersToString}";

    protected string JustOrdersToString => $"{nameof(Orders)}: [{string.Join(", ", Orders)}]";

    public override string ToString() => $"{nameof(OrdersPriceVolumeLayer)}{{{OrdersPriceVolumeLayerToStringMembers}}}";
}
