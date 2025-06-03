// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;

public interface IPQOrdersPriceVolumeLayer : IMutableOrdersPriceVolumeLayer, IPQOrdersCountPriceVolumeLayer
  , IPQSupportsStringUpdates<IPriceVolumeLayer>, ISupportsPQNameIdLookupGenerator, ITrackableReset<IPQOrdersPriceVolumeLayer>
  , IMutableTracksReorderingList<IPQAnonymousOrder, IAnonymousOrder>
{
    new IPQAnonymousOrder this[int index] { get; set; }

    new bool IsReadOnly { get; }

    new int Count { get; set; }

    new int Capacity { get; set; }

    new IReadOnlyList<ListShiftCommand> ShiftCommands { get; set; }

    new int? ClearRemainingElementsFromIndex { get; set; }

    new bool HasUnreliableListTracking { get; set; }

    new ushort MaxAllowedSize { get; set; }

    new ListShiftCommand ShiftElements(int byElements);

    new ListShiftCommand AppendShiftCommand(ListShiftCommand toAppendAtEnd);

    new void ClearShiftCommands();

    new ListShiftCommand ClearAll();

    new ListShiftCommand DeleteAt(int index);

    new ListShiftCommand ShiftElementsFrom(int byElements, int pinElementsFromIndex);

    new ListShiftCommand ShiftElementsUntil(int byElements, int pinElementsFromIndex);

    new ListShiftCommand ApplyListShiftCommand(ListShiftCommand shiftCommandToApply);

    new ListShiftCommand MoveSingleElementBy(IPQAnonymousOrder existingItem, int shift);

    new ListShiftCommand MoveSingleElementBy(int indexToMoveToEnd, int shift);

    new ListShiftCommand MoveToEnd(int indexToMoveToEnd);

    new ListShiftCommand MoveToStart(int indexToMoveToStart);

    new bool CalculateShift(DateTime asAtTime, IReadOnlyList<IAnonymousOrder> updatedCollection);

    new void RemoveAt(int index);

    new void Clear();

    new IPQNameIdLookupGenerator         NameIdLookup { get; set; }
    new IReadOnlyList<IPQAnonymousOrder> Orders       { get; }

    new IEnumerator<IPQAnonymousOrder> GetEnumerator();

    new IPQOrdersPriceVolumeLayer Clone();
    new IPQOrdersPriceVolumeLayer ResetWithTracking();
}

public class PQOrdersPriceVolumeLayer : PQOrdersCountPriceVolumeLayer, IPQOrdersPriceVolumeLayer
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQOrdersPriceVolumeLayer));

    private static readonly IReadOnlyList<IPQAnonymousOrder> EmptyOrders = new List<IPQAnonymousOrder>().AsReadOnly();

    private readonly bool isCounterPartyOrders;

    private readonly IList<IPQAnonymousOrder> orders;

    private IPQNameIdLookupGenerator nameIdLookup = null!;

    private readonly TracksListReorderingRegistry<IPQAnonymousOrder, IAnonymousOrder> elementShiftRegistry;

    public PQOrdersPriceVolumeLayer(LayerType layerType, IPQNameIdLookupGenerator initialDictionary)
    {
        isCounterPartyOrders =
            layerType switch
            {
                LayerType.OrdersAnonymousPriceVolume => false
              , LayerType.OrdersFullPriceVolume => true
              , LayerType.FullSupportPriceVolume => true
              , _ => throw new ArgumentException($"Only expected to receive OrdersAnonymousPriceVolume or OrdersFullPriceVolume but got {layerType}")
            };

        NameIdLookup = initialDictionary;
        orders       = new List<IPQAnonymousOrder>(0);

        elementShiftRegistry = new TracksListReorderingRegistry<IPQAnonymousOrder, IAnonymousOrder>(this, NewElementFactory, SameTradeId);

        if (GetType() == typeof(PQOrdersPriceVolumeLayer)) SequenceId = 0;
    }

    public PQOrdersPriceVolumeLayer
    (IPQNameIdLookupGenerator traderIdToNameLookup, LayerType layerType, decimal price = 0m, decimal volume = 0m, uint ordersCount = 0
      , decimal internalVolume = 0,
        IEnumerable<IAnonymousOrder>? layerOrders = null)
        : base(price, volume, ordersCount, internalVolume)
    {
        isCounterPartyOrders =
            layerType switch
            {
                LayerType.OrdersAnonymousPriceVolume => false
              , LayerType.OrdersFullPriceVolume => true
              , LayerType.FullSupportPriceVolume => true
              , _ => throw new ArgumentException($"Only expected to receive OrdersAnonymousPriceVolume or OrdersFullPriceVolume but got {layerType}")
            };

        NameIdLookup = traderIdToNameLookup;
        orders       = new List<IPQAnonymousOrder>(0);

        elementShiftRegistry = new TracksListReorderingRegistry<IPQAnonymousOrder, IAnonymousOrder>(this, NewElementFactory, SameTradeId);

        if (layerOrders is not null)
            foreach (var orderLayerInfo in layerOrders)
                CopyAddLayer(orderLayerInfo);
        if (GetType() == typeof(PQOrdersPriceVolumeLayer)) SequenceId = 0;
    }

    public PQOrdersPriceVolumeLayer(IPriceVolumeLayer toClone, LayerType layerType, IPQNameIdLookupGenerator ipqNameIdLookupGenerator) : base(toClone)
    {
        isCounterPartyOrders =
            layerType switch
            {
                LayerType.OrdersAnonymousPriceVolume => false
              , LayerType.OrdersFullPriceVolume => true
              , LayerType.FullSupportPriceVolume => true
              , _ => throw new ArgumentException($"Only expected to receive OrdersAnonymousPriceVolume or OrdersFullPriceVolume but got {layerType}")
            };
        NameIdLookup = ipqNameIdLookupGenerator;
        if (toClone is IOrdersPriceVolumeLayer ordersToClone)
        {
            MaxAllowedSize = ordersToClone.MaxAllowedSize;
            orders = new List<IPQAnonymousOrder>((int)ordersToClone.OrdersCount);
            foreach (var orderLayerInfo in ordersToClone.Orders) CopyAddLayer(orderLayerInfo);
        }
        else
        {
            orders = new List<IPQAnonymousOrder>(0);
        }

        elementShiftRegistry = new TracksListReorderingRegistry<IPQAnonymousOrder, IAnonymousOrder>(this, NewElementFactory, SameTradeId);
        SetFlagsSame(toClone);
        if (GetType() == typeof(PQOrdersPriceVolumeLayer)) SequenceId = 0;
    }

    protected static Func<IAnonymousOrder, IAnonymousOrder, bool> SameTradeId = (lhs, rhs) => lhs.OrderId == rhs.OrderId;

    protected Func<IPQAnonymousOrder> NewElementFactory => CreateNewBookOrderLayer;

    [JsonIgnore] IReadOnlyList<IAnonymousOrder> IOrdersPriceVolumeLayer.Orders => orders.Take(CountFromOrders()).ToList().AsReadOnly();

    [JsonIgnore]
    IReadOnlyList<IMutableAnonymousOrder> IMutableOrdersPriceVolumeLayer.Orders => orders.Take(CountFromOrders()).ToList().AsReadOnly();

    public IReadOnlyList<IPQAnonymousOrder> Orders => orders.Take(CountFromOrders()).ToList().AsReadOnly();

    [JsonIgnore] public override LayerType LayerType => isCounterPartyOrders ? LayerType.OrdersFullPriceVolume : LayerType.OrdersAnonymousPriceVolume;

    [JsonIgnore]
    public override LayerFlags SupportsLayerFlags =>
        base.SupportsLayerFlags |
        (isCounterPartyOrders
            ? LayerFlagsExtensions.AdditionalCounterPartyOrderFlags
            : LayerFlagsExtensions.AdditionalAnonymousOrderFlags);

    public ushort MaxAllowedSize { get; set; } = PQFeedFieldsExtensions.TwoByteFieldIdMaxBookDepth;

    [JsonIgnore]
    IMutableAnonymousOrder IMutableOrdersPriceVolumeLayer.this[int i]
    {
        get => this[i];
        set => this[i] = (IPQAnonymousOrder)value;
    }

    IAnonymousOrder IReadOnlyList<IAnonymousOrder>.this[int index] => this[index];

    IMutableAnonymousOrder IReadOnlyList<IMutableAnonymousOrder>.this[int index] => this[index];

    IMutableAnonymousOrder IList<IMutableAnonymousOrder>.this[int index]
    {
        get => this[index];
        set => this[index] = (IPQAnonymousOrder)value;
    }
    IMutableAnonymousOrder IMutableCapacityList<IMutableAnonymousOrder>.this[int i]
    {
        get => this[i];
        set => this[i] = (IPQAnonymousOrder)value;
    }

    IMutableAnonymousOrder IMutableTracksShiftsList<IMutableAnonymousOrder, IAnonymousOrder>.this[int index]
    {
        get => this[index];
        set => this[index] = (IPQAnonymousOrder)value;
    }


    [JsonIgnore]
    public IPQAnonymousOrder this[int i]
    {
        get
        {
            AssertMaxTraderSizeNotExceeded(i);
            for (var j = Capacity; j <= i; j++) AppendLayer(CreateNewBookOrderLayer());
            return orders[i];
        }
        set
        {
            AssertMaxTraderSizeNotExceeded(i);
            for (var j = Capacity; j < i; j++) AppendLayer(CreateNewBookOrderLayer());
            if (i >= Capacity)
            {
                orders.Add(value);
                value.NameIdLookup = NameIdLookup;
            }
            else
            {
                orders[i]          = value;
                value.NameIdLookup = NameIdLookup;
            }
            base.OrdersCount = (uint)CountFromOrders();
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

    int IReadOnlyCollection<IPQAnonymousOrder>.Count => (int)OrdersCount;

    int ICollection<IPQAnonymousOrder>.Count => (int)OrdersCount;

    int IMutableCapacityList<IPQAnonymousOrder>.Count
    {
        get => (int)OrdersCount;
        set => OrdersCount = (uint)value;
    }

    int IPQOrdersPriceVolumeLayer.Count
    {
        get => (int)OrdersCount;
        set => OrdersCount = (uint)value;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
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
            if (orders != null!)
            {
                for (var i = Capacity - 1; i >= value; i--)
                {
                    var layerAtLevel = orders[i];

                    if (!layerAtLevel.IsEmpty) layerAtLevel.IsEmpty = true;
                }
            }
            base.OrdersCount = value;
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

    [JsonIgnore] INameIdLookup IHasNameIdLookup.NameIdLookup => NameIdLookup;

    [JsonIgnore]
    public virtual IPQNameIdLookupGenerator NameIdLookup
    {
        get => nameIdLookup;
        set
        {
            if (ReferenceEquals(nameIdLookup, value)) return;
            nameIdLookup = value;
            if (orders != null!)
            {
                foreach (var pqaoli in Orders.OfType<ISupportsPQNameIdLookupGenerator>()) pqaoli.NameIdLookup = nameIdLookup;
            }
        }
    }

    [JsonIgnore]
    public override bool HasUpdates
    {
        get { return base.HasUpdates || orders.Any(aoli => aoli.HasUpdates); }
        set
        {
            base.HasUpdates = value;
            foreach (var pqaoli in orders) pqaoli.HasUpdates = value;
            if (value) return;
            NameIdLookup.HasUpdates = value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public override bool IsEmpty
    {
        get => base.IsEmpty && (orders.All(aoli => aoli.IsEmpty));
        set
        {
            base.IsEmpty = value;
            if (!value) return;
            ResetWithTracking();
        }
    }

    public override void UpdateStarted(uint updateSequenceId)
    {
        foreach (var anonymousOrder in Orders)
        {
            anonymousOrder.UpdateStarted(updateSequenceId);
        }
        base.UpdateStarted(updateSequenceId);
    }

    public override void UpdateComplete(uint updateSequenceId = 0)
    {
        foreach (var pqaoli in orders) pqaoli.UpdateComplete(updateSequenceId);
        NameIdLookup.UpdateComplete(updateSequenceId);
        base.UpdateComplete(updateSequenceId);
    }

    public IReadOnlyList<ListShiftCommand> ShiftCommands
    {
        get => elementShiftRegistry.ShiftCommands;
        set => elementShiftRegistry.ShiftCommands = value;
    }

    public int? ClearRemainingElementsFromIndex
    {
        get => elementShiftRegistry.ClearRemainingElementsFromIndex;
        set
        {
            elementShiftRegistry.ClearRemainingElementsFromIndex = value;

            base.OrdersCount = (uint)CountFromOrders();
        }
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

    ListShiftCommand IMutableTracksShiftsList<IMutableAnonymousOrder, IAnonymousOrder>.InsertAtStart
        (IMutableAnonymousOrder toInsertAtStart) =>
        InsertAtStart((IPQAnonymousOrder)toInsertAtStart);

    bool IMutableTracksShiftsList<IMutableAnonymousOrder, IAnonymousOrder>.AppendAtEnd(IMutableAnonymousOrder toAppendAtEnd) =>
        AppendAtEnd((IPQAnonymousOrder)toAppendAtEnd);

    ListShiftCommand IMutableTracksShiftsList<IMutableAnonymousOrder, IAnonymousOrder>.InsertAt(int index, IMutableAnonymousOrder toInsertAtStart) =>
        InsertAt(index, (IPQAnonymousOrder)toInsertAtStart);

    ListShiftCommand IMutableTracksShiftsList<IMutableAnonymousOrder, IAnonymousOrder>.Delete(IMutableAnonymousOrder toDelete) =>
        Delete((IPQAnonymousOrder)toDelete);

    ListShiftCommand IMutableTracksReorderingList<IMutableAnonymousOrder, IAnonymousOrder>.MoveToStart(IMutableAnonymousOrder existingItem) =>
        MoveToStart((IPQAnonymousOrder)existingItem);

    ListShiftCommand IMutableTracksReorderingList<IMutableAnonymousOrder, IAnonymousOrder>.MoveSingleElementBy
        (IMutableAnonymousOrder existingItem, int shift) =>
        MoveSingleElementBy((IPQAnonymousOrder)existingItem, shift);

    ListShiftCommand IMutableTracksReorderingList<IMutableAnonymousOrder, IAnonymousOrder>.MoveToEnd(IMutableAnonymousOrder existingItem) =>
        MoveToEnd((IPQAnonymousOrder)existingItem);

    public ListShiftCommand InsertAtStart(IPQAnonymousOrder toInsertAtStart)
    {
        var result = elementShiftRegistry.InsertAtStart(toInsertAtStart);
        base.OrdersCount = (uint)CountFromOrders();
        return result;
    }

    public bool AppendAtEnd(IPQAnonymousOrder toAppendAtEnd)
    {
        var result = elementShiftRegistry.AppendAtEnd(toAppendAtEnd);
        base.OrdersCount = (uint)CountFromOrders();
        return result;
    }

    public ListShiftCommand InsertAt(int index, IPQAnonymousOrder toInsertAtStart)
    {
        var result = elementShiftRegistry.InsertAt(index, toInsertAtStart);
        base.OrdersCount = (uint)CountFromOrders();
        return result;
    }

    public ListShiftCommand Delete(IPQAnonymousOrder toDelete)
    {
        var result = elementShiftRegistry.Delete(toDelete);
        base.OrdersCount = (uint)CountFromOrders();
        return result;
    }

    public ListShiftCommand MoveToStart(IPQAnonymousOrder existingItem)
    {
        var result = elementShiftRegistry.MoveToStart(existingItem);
        base.OrdersCount = (uint)CountFromOrders();
        return result;
    }

    public ListShiftCommand MoveToEnd(IPQAnonymousOrder existingItem)
    {
        var result = elementShiftRegistry.MoveToEnd(existingItem);
        base.OrdersCount = (uint)CountFromOrders();
        return result;
    }

    public ListShiftCommand ShiftElements(int byElements)
    {
        var result = elementShiftRegistry.ShiftElements(byElements);
        base.OrdersCount = (uint)CountFromOrders();
        return result;
    }

    public ListShiftCommand ClearAll()
    {
        var result = elementShiftRegistry.ClearAll();
        base.OrdersCount = 0;
        return result;
    }

    public ListShiftCommand DeleteAt(int index)
    {
        var result = elementShiftRegistry.DeleteAt(index);
        base.OrdersCount = (uint)CountFromOrders();
        return result;
    }

    public ListShiftCommand ShiftElementsFrom(int byElements, int pinElementsFromIndex)
    {
        var result = elementShiftRegistry.ShiftElementsFrom(byElements, pinElementsFromIndex);
        base.OrdersCount = (uint)CountFromOrders();
        return result;
    }

    public ListShiftCommand ShiftElementsUntil(int byElements, int pinElementsFromIndex)
    {
        var result = elementShiftRegistry.ShiftElementsUntil(byElements, pinElementsFromIndex);
        base.OrdersCount = (uint)CountFromOrders();
        return result;
    }

    public ListShiftCommand ApplyListShiftCommand(ListShiftCommand shiftCommandToApply)
    {
        var result = elementShiftRegistry.ApplyListShiftCommand(shiftCommandToApply);
        base.OrdersCount = (uint)CountFromOrders();
        return result;
    }

    public ListShiftCommand MoveSingleElementBy(IPQAnonymousOrder existingItem, int shift)
    {
        var result = elementShiftRegistry.MoveSingleElementBy(existingItem, shift);
        base.OrdersCount = (uint)CountFromOrders();
        return result;
    }

    public ListShiftCommand MoveSingleElementBy(int indexToMoveToEnd, int shift) => elementShiftRegistry.MoveSingleElementBy(indexToMoveToEnd, shift);

    public ListShiftCommand MoveToEnd(int indexToMoveToEnd) => elementShiftRegistry.MoveToEnd(indexToMoveToEnd);

    public ListShiftCommand MoveToStart(int indexToMoveToStart) => elementShiftRegistry.MoveToStart(indexToMoveToStart);

    IMutableOrdersPriceVolumeLayer ITrackableReset<IMutableOrdersPriceVolumeLayer>.ResetWithTracking() => ResetWithTracking();

    IMutableOrdersPriceVolumeLayer IMutableOrdersPriceVolumeLayer.ResetWithTracking() => ResetWithTracking();

    IPQOrdersPriceVolumeLayer ITrackableReset<IPQOrdersPriceVolumeLayer>.ResetWithTracking() => ResetWithTracking();

    IPQOrdersPriceVolumeLayer IPQOrdersPriceVolumeLayer.ResetWithTracking() => ResetWithTracking();

    ITracksResetCappedCapacityList<IMutableAnonymousOrder> ITrackableReset<ITracksResetCappedCapacityList<IMutableAnonymousOrder>>.
        ResetWithTracking() => ResetWithTracking();

    ITracksResetCappedCapacityList<IPQAnonymousOrder> ITrackableReset<ITracksResetCappedCapacityList<IPQAnonymousOrder>>.ResetWithTracking() =>
        ResetWithTracking();

    public override PQOrdersPriceVolumeLayer ResetWithTracking()
    {
        foreach (var pqTraderLayerInfo in Orders) pqTraderLayerInfo.ResetWithTracking();
        base.ResetWithTracking();
        return this;
    }

    public override void StateReset()
    {
        foreach (var pqTraderLayerInfo in Orders) pqTraderLayerInfo.StateReset();
        base.StateReset();
    }

    void ICollection<IMutableAnonymousOrder>.Add(IMutableAnonymousOrder item) => Add((IPQAnonymousOrder)item);

    bool ICollection<IMutableAnonymousOrder>.Contains(IMutableAnonymousOrder item) => Contains((IPQAnonymousOrder)item);

    void ICollection<IMutableAnonymousOrder>.CopyTo(IMutableAnonymousOrder[] array, int arrayIndex)
    {
        for (int i = 0; i < orders.Count && i + arrayIndex < array.Length; i++)
        {
            array[i + arrayIndex] = orders[i];
        }
    }

    bool ICollection<IMutableAnonymousOrder>.Remove(IMutableAnonymousOrder item) => Remove((IPQAnonymousOrder)item);

    int IList<IMutableAnonymousOrder>.IndexOf(IMutableAnonymousOrder item) => IndexOf((IPQAnonymousOrder)item);

    void IList<IMutableAnonymousOrder>.Insert(int index, IMutableAnonymousOrder item)
    {
        Insert(index, (IPQAnonymousOrder)item);
    }

    public void Add(IMutableAnonymousOrder order)
    {
        var indexToUpdate = (int)OrdersCount;
        AssertMaxTraderSizeNotExceeded(indexToUpdate);
        if (indexToUpdate >= Capacity)
        {
            CopyAddLayer(order);
        }
        else
        {
            var entryToUpdate = this[indexToUpdate];
            entryToUpdate.CopyFrom(order, CopyMergeFlags.FullReplace);
            base.OrdersCount = (uint)CountFromOrders();
        }
    }

    public void Add(IPQAnonymousOrder item)
    {
        int index = CountFromOrders();
        item.NameIdLookup = NameIdLookup;
        if (index < Capacity)
        {
            this[index] = item;
        }
        else
        {
            orders.Add(item);
            base.OrdersCount = (uint)CountFromOrders();
        }
    }

    public void Clear()
    {
        orders.Clear();
        base.OrdersCount = 0;
    }

    public bool Contains(IPQAnonymousOrder item) => orders.Contains(item);

    public void CopyTo(IPQAnonymousOrder[] array, int arrayIndex)
    {
        for (int i = 0; i < orders.Count && i + arrayIndex < array.Length; i++)
        {
            array[i + arrayIndex] = orders[i];
        }
    }

    public bool Remove(IPQAnonymousOrder item)
    {
        var result = orders.Remove(item);
        base.OrdersCount = (uint)CountFromOrders();
        return result;
    }

    public bool IsReadOnly => false;

    public int IndexOf(IPQAnonymousOrder item) => orders.IndexOf(item);

    public void Insert(int index, IPQAnonymousOrder item)
    {
        orders.Insert(index, item);
        base.OrdersCount = (uint)CountFromOrders();
    }

    public void RemoveAt(int index)
    {
        orders.RemoveAt(index);
        base.OrdersCount = (uint)CountFromOrders();
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, Serdes.Serialization.PQMessageFlags messageFlags
      , IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (messageFlags & Serdes.Serialization.PQMessageFlags.Complete) == 0;


        foreach (var shiftCommand in elementShiftRegistry!.ShiftCommands)
        {
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerOrders, PQPricingSubFieldKeys.CommandElementsShift
                                         , (uint)shiftCommand, (PQFieldFlags)shiftCommand);
        }

        foreach (var pqFieldUpdate in base.GetDeltaUpdateFields(snapShotTime, messageFlags,
                                                                quotePublicationPrecisionSetting))
            yield return pqFieldUpdate;

        var numberOfTraderInfos = Math.Min(ushort.MaxValue, Capacity);
        for (ushort i = 0; i < numberOfTraderInfos && i < Capacity; i++)
        {
            var tli = orders[i];
            foreach (var orderFu in tli.GetDeltaUpdateFields(snapShotTime, messageFlags, quotePublicationPrecisionSetting))
                yield return orderFu.WithAuxiliary(i);
            if (i + 1 == numberOfTraderInfos) break;
        }
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        // assume the book has already forwarded this through to the correct layer
        switch (pqFieldUpdate.Id)
        {
            case PQFeedFields.QuoteLayerStringUpdates:
                return NameIdLookup.VerifyDictionaryAndExtractSize(pqFieldUpdate);
            case PQFeedFields.QuoteLayerOrders:
                if (pqFieldUpdate.PricingSubId is PQPricingSubFieldKeys.CommandElementsShift)
                {
                    var elementShift = (ListShiftCommand)(pqFieldUpdate);
                    elementShiftRegistry.AppendShiftCommand(elementShift);
                    ApplyListShiftCommand(elementShift);
                }
                else
                {
                    var index          = pqFieldUpdate.AuxiliaryPayload;
                    var orderLayerInfo = this[index];
                    return orderLayerInfo.UpdateField(pqFieldUpdate);
                }
                break;
        }

        return base.UpdateField(pqFieldUpdate);
    }

    public virtual IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, Serdes.Serialization.PQMessageFlags messageFlags)
    {
        var numberOfTraderInfos = Math.Min(ushort.MaxValue, Capacity);
        for (ushort i = 0; i < numberOfTraderInfos && i < Capacity; i++)
        {
            var anonOrder = orders?[i];
            if (anonOrder is null or { IsEmpty: true, HasUpdates: false } || i + 1 == ushort.MaxValue) continue;
            foreach (var stringUpdate in anonOrder.GetStringUpdates(snapShotTime, messageFlags)) yield return stringUpdate.WithAuxiliary(i);
            if (i + 1 == numberOfTraderInfos) break;
        }
    }

    public virtual bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        if (stringUpdate.Field.Id != PQFeedFields.QuoteLayerStringUpdates) return false;
        return NameIdLookup.UpdateFieldString(stringUpdate);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<IAnonymousOrder> IEnumerable<IAnonymousOrder>.GetEnumerator() => GetEnumerator();

    IEnumerator<IMutableAnonymousOrder> IEnumerable<IMutableAnonymousOrder>.GetEnumerator() => GetEnumerator();

    IEnumerator<IMutableAnonymousOrder> IMutableOrdersPriceVolumeLayer.GetEnumerator() => GetEnumerator();

    public IEnumerator<IPQAnonymousOrder> GetEnumerator() => orders.Take(CountFromOrders()).GetEnumerator();

    IPQOrdersPriceVolumeLayer IPQOrdersPriceVolumeLayer.Clone() => Clone();

    IOrdersPriceVolumeLayer ICloneable<IOrdersPriceVolumeLayer>.Clone() => Clone();

    IOrdersPriceVolumeLayer IOrdersPriceVolumeLayer.Clone() => Clone();

    IMutableOrdersPriceVolumeLayer IMutableOrdersPriceVolumeLayer.Clone() => Clone();

    IMutablePriceVolumeLayer ICloneable<IMutablePriceVolumeLayer>.Clone() => Clone();

    IMutablePriceVolumeLayer IMutablePriceVolumeLayer.Clone() => Clone();

    public override PQOrdersPriceVolumeLayer Clone() => new(this, LayerType, NameIdLookup);


    [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
    private void AssertMaxTraderSizeNotExceeded(int proposedNewIndex)
    {
        if (proposedNewIndex > ushort.MaxValue)
            throw new ArgumentOutOfRangeException($"Max Traders represented is {ushort.MaxValue}. Got {proposedNewIndex}");
    }

    private void AppendLayer(IPQAnonymousOrder toAdd)
    {
        orders.Add(toAdd);
    }

    private void CopyAddLayer(IAnonymousOrder toAdd)
    {
        orders.Add(ConvertToBookLayer(toAdd));
        base.OrdersCount = (uint)CountFromOrders();
    }

    public IPQAnonymousOrder ConvertToBookLayer(IAnonymousOrder toAdd)
    {
        if (LayerType.SupportsOrdersFullPriceVolume())
            return new PQExternalCounterPartyOrder(toAdd, NameIdLookup)
            {
                Recycler = Recycler
            };
        return new PQAnonymousOrder(toAdd, NameIdLookup)
        {
            Recycler = Recycler
        };
    }

    public IPQAnonymousOrder CreateNewBookOrderLayer()
    {
        if (LayerType.SupportsOrdersFullPriceVolume())
            return new PQExternalCounterPartyOrder(NameIdLookup)
            {
                Recycler = Recycler
            };
        return new PQAnonymousOrder(NameIdLookup)
        {
            Recycler = Recycler
        };
    }

    protected int CountFromOrders()
    {
        for (var i = Capacity - 1; i >= 0; i--)
        {
            var layerAtLevel = orders[i];
            if (!layerAtLevel.IsEmpty) return (i + 1);
        }
        return 0;
    }

    protected decimal InternalVolumeFromOrders()
    {
        return Orders
               .Where(aoli =>
                          aoli.GenesisFlags.IsInternalOrder()
                       && !aoli.GenesisFlags.HasVolumeNotPartOfLiquidity()
                       && !aoli.GenesisFlags.HasSyntheticForBacktestSimulation())
               .Sum(aoli => aoli.OrderRemainingVolume);
    }

    public override bool AreEquivalent(IPriceVolumeLayer? other, bool exactTypes = false)
    {
        if (!(other is IOrdersPriceVolumeLayer traderPvLayer)) return false;
        var baseSame        = base.AreEquivalent(other, exactTypes);
        var countFromOrders = CountFromOrders();
        var ordersCountSame = countFromOrders == traderPvLayer.Orders.Count;

        var orderStillSame = true;
        if (ordersCountSame)
        {
            for (int i = 0; i < countFromOrders && orderStillSame; i++)
            {
                var localOrder = this[i];
                var otherOrder = traderPvLayer.Orders[i];

                if (localOrder.IsEmpty && otherOrder.IsEmpty) continue;
                orderStillSame = localOrder.AreEquivalent(otherOrder, exactTypes);
            }
        }

        var allAreSame = baseSame && orderStillSame && ordersCountSame;
        return allAreSame;
    }

    public override PQOrdersPriceVolumeLayer CopyFrom(IPriceVolumeLayer source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        var isFullReplace = copyMergeFlags.HasFullReplace();
        var pqopvl        = source as IPQOrdersPriceVolumeLayer;
        var opvl          = source as IOrdersPriceVolumeLayer;

        var thisLayerGenesisFlags = LayerType.SupportsOrdersFullPriceVolume()
            ? IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags
            : OrderGenesisFlags.None;
        var addInfoMask = IAnonymousOrder.AllExceptExtraInfoFlags;
        if (source is ISupportsPQNameIdLookupGenerator pqNameIdLookupGenerator && !copyMergeFlags.HasSkipReferenceLookups())
            NameIdLookup.CopyFrom(pqNameIdLookupGenerator.NameIdLookup, copyMergeFlags);

        if (opvl != null)
        {
            elementShiftRegistry.CopyFrom(opvl, copyMergeFlags);

            var existingOrdersCountUpdated = IsOrdersCountUpdated;
            var existingCount              = (uint)CountFromOrders();
            for (var j = 0; j < opvl.Orders.Count; j++)
            {
                var sourceOrder = opvl[j];
                var destOrder   = this[j];

                var hasSourceIsGenesisFlagsUpdate = sourceOrder is IPQAnonymousOrder;
                var originalGenesisUpdated = sourceOrder is IPQAnonymousOrder pqAnonymousOrder
                    ? pqAnonymousOrder.IsOrderIdUpdated | destOrder.IsGenesisFlagsUpdated
                    : destOrder.IsGenesisFlagsUpdated;
                var originalGenesisFlags = destOrder.GenesisFlags;
                destOrder.CopyFrom(sourceOrder, copyMergeFlags);
                var modifiedGenesisFlags = (destOrder.GenesisFlags & addInfoMask);
                destOrder.GenesisFlags            = modifiedGenesisFlags | thisLayerGenesisFlags;
                destOrder.EmptyIgnoreGenesisFlags = thisLayerGenesisFlags;
                destOrder.IsGenesisFlagsUpdated
                    = hasSourceIsGenesisFlagsUpdate
                        ? originalGenesisUpdated
                        : !thisLayerGenesisFlags.IgnoringAreSame(originalGenesisFlags, modifiedGenesisFlags) || originalGenesisUpdated;
            }
            for (var i = opvl.Orders.Count; i < Capacity; i++) orders[i].ResetWithTracking();
            var newOrderCount = (uint)CountFromOrders();
            base.OrdersCount     = newOrderCount;
            IsOrdersCountUpdated = existingOrdersCountUpdated || existingCount != newOrderCount;
        }

        base.CopyFrom(source, copyMergeFlags);
        if (pqopvl != null && isFullReplace) SetFlagsSame(source);

        return this;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IPriceVolumeLayer?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            return (base.GetHashCode() * 397) ^ orders.GetHashCode();
        }
    }
    
    public string EachOrderByIndexOnNewLines()
    {
        var countFromOrders = CountFromOrders();
        var sb              = new StringBuilder(100 * countFromOrders);
        for (var i = 0; i < countFromOrders; i++)
        {
            var order = Orders[i];
            sb.Append("[").Append(i).Append("] = ").Append(order);
            if (i < countFromOrders - 1)
            {
                sb.AppendLine(",");
            }
        }
        return sb.ToString();
    }

    protected string PQJustOrdersToStringMembers => $"{nameof(Orders)}: [{EachOrderByIndexOnNewLines()}]";

    protected string PQOrdersPriceVolumeLayerToStringMembers => 
        $"{PQOrdersCountVolumeLayerToStringMembers}, {nameof(MaxAllowedSize)}: {MaxAllowedSize:N0}, {PQJustOrdersToStringMembers}";

    public override string ToString() => $"{GetType().Name}({PQOrdersPriceVolumeLayerToStringMembers}, {UpdatedFlagsToString})";
}
