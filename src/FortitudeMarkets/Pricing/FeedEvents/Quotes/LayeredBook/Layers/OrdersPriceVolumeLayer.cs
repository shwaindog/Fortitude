// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers.LayerOrders;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;

public interface IOrdersPriceVolumeLayer : IOrdersCountPriceVolumeLayer,
    ICloneable<IOrdersPriceVolumeLayer>
{
    IReadOnlyList<IAnonymousOrderLayerInfo> Orders { get; }
    IAnonymousOrderLayerInfo? this[int i] { get; }
    new IOrdersPriceVolumeLayer Clone();
}

public interface IMutableOrdersPriceVolumeLayer : IOrdersPriceVolumeLayer, IMutableOrdersCountPriceVolumeLayer
  , ITrackableReset<IMutableOrdersPriceVolumeLayer>
{
    new IMutableAnonymousOrderLayerInfo? this[int i] { get; set; }

    new uint    OrdersCount    { get; }
    new decimal InternalVolume { get; }

    new IReadOnlyList<IMutableAnonymousOrderLayerInfo> Orders { get; }

    void Add(IAnonymousOrderLayerInfo toAdd);
    bool RemoveAt(int index);
    void ShiftOrders(int offset);

    new IMutableOrdersPriceVolumeLayer Clone();
    new IMutableOrdersPriceVolumeLayer ResetWithTracking();
}

public class OrdersPriceVolumeLayer : OrdersCountPriceVolumeLayer, IMutableOrdersPriceVolumeLayer
{
    private static readonly IReadOnlyList<IMutableAnonymousOrderLayerInfo> EmptyOrders = new List<IMutableAnonymousOrderLayerInfo>().AsReadOnly();
    private readonly        bool                                           isCounterPartyOrders;

    private readonly IList<IMutableAnonymousOrderLayerInfo>? orders;

    public OrdersPriceVolumeLayer() => orders = new List<IMutableAnonymousOrderLayerInfo>();

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

        orders = new List<IMutableAnonymousOrderLayerInfo>();
    }

    public OrdersPriceVolumeLayer
    (LayerType layerType = LayerType.OrdersAnonymousPriceVolume
      , decimal price = 0m, decimal volume = 0m, uint ordersCount = 0, decimal internalVolume = 0
      , IEnumerable<IAnonymousOrderLayerInfo>? layerOrders = null) : base(price, volume, ordersCount, internalVolume)
    {
        isCounterPartyOrders =
            layerType switch
            {
                LayerType.OrdersAnonymousPriceVolume => false
              , LayerType.OrdersFullPriceVolume => true
              , LayerType.FullSupportPriceVolume => true
              , _ => throw new ArgumentException($"Only expected to receive OrdersAnonymousPriceVolume or OrdersFullPriceVolume but got {layerType}")
            };
        orders = layerOrders?.OfType<IMutableAnonymousOrderLayerInfo>().ToList() ?? new List<IMutableAnonymousOrderLayerInfo>();
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
            orders = new List<IMutableAnonymousOrderLayerInfo>((int)ordersToClone.OrdersCount);
            foreach (var orderLayerInfo in ordersToClone.Orders) AddLayer(orderLayerInfo);
        }
        else
        {
            orders = new List<IMutableAnonymousOrderLayerInfo>(0);
        }
    }

    protected string OrdersPriceVolumeLayerToStringMembers => $"{OrdersCountPriceVolumeLayerToStringMembers}, {JustOrdersToString}";
    protected string JustOrdersToString                    => $"{nameof(Orders)}: [{string.Join(", ", Orders)}]";

    [JsonIgnore]
    public IReadOnlyList<IMutableAnonymousOrderLayerInfo> Orders => orders?.Where(aoli => !aoli.IsEmpty).ToList().AsReadOnly() ?? EmptyOrders;

    IReadOnlyList<IAnonymousOrderLayerInfo> IOrdersPriceVolumeLayer.Orders =>
        orders?.Where(aoli => !aoli.IsEmpty).ToList().AsReadOnly() ?? EmptyOrders;

    IAnonymousOrderLayerInfo? IOrdersPriceVolumeLayer.this[int i] => this[i];

    [JsonIgnore] public override LayerType LayerType => isCounterPartyOrders ? LayerType.OrdersFullPriceVolume : LayerType.OrdersAnonymousPriceVolume;


    [JsonIgnore]
    public override LayerFlags SupportsLayerFlags =>
        isCounterPartyOrders
            ? LayerFlagsExtensions.AdditionalCounterPartyOrderFlags | base.SupportsLayerFlags
            : LayerFlagsExtensions.AdditionalAnonymousOrderFlags | base.SupportsLayerFlags;


    [JsonIgnore]
    public IMutableAnonymousOrderLayerInfo? this[int i]
    {
        get
        {
            AssertMaxOrdersNotExceeded(i);
            for (var j = orders?.Count ?? 0; j <= i; j++) orders?.Add(CreateNewBookOrderLayer());
            return orders?[i];
        }
        set
        {
            AssertMaxOrdersNotExceeded(i);
            for (var j = orders?.Count ?? 0; j < i; j++) orders?.Add(CreateNewBookOrderLayer());
            if (orders?.Count == i && value != null)
            {
                orders.Add(value);
            }
            else if (orders != null)
            {
                if (value != null)
                    orders[i] = value;
                else
                    orders[i].StateReset();
            }
        }
    }

    public override uint OrdersCount
    {
        get
        {
            var calcOrderCount = CountFromOrders();
            if (calcOrderCount > 0) return calcOrderCount;
            return base.OrdersCount;
        }
        set
        {
            for (var i = (orders?.Count ?? 0) - 1; i >= value; i--)
            {
                var layerAtLevel                                          = orders?[i];
                if (!layerAtLevel?.IsEmpty ?? true) layerAtLevel!.IsEmpty = true;
            }
            base.OrdersCount = value;
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
            foreach (var orderLayerInfo in orders ?? []) orderLayerInfo.IsEmpty = true;
            base.IsEmpty = true;
        }
    }

    public void Add(IAnonymousOrderLayerInfo order)
    {
        if (orders == null) return;
        var indexToUpdate = (int)CountFromOrders();
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

    public bool RemoveAt(int index)
    {
        var orderAt = orders![index]!;
        orderAt.IsEmpty = true;
        return true;
    }

    public void ShiftOrders(int offset)
    {
        if (orders == null || orders.Count == 0) return;
        if (offset > 0)
            for (var i = 0; i < offset; i++)
            {
                IMutableAnonymousOrderLayerInfo? toInsert;
                if (orders[^1].IsEmpty)
                {
                    var allOrdersCount = orders.Count;
                    toInsert = orders[allOrdersCount - 1];
                    orders.RemoveAt(allOrdersCount - 1);
                }
                else
                {
                    toInsert = CreateNewBookOrderLayer();
                }
                orders.Insert(0, toInsert);
            }
        else if (offset < 0)
            for (var i = offset; i < 0; i++)
            {
                var toResetAtEnd = Orders[0];
                orders.RemoveAt(0);
                toResetAtEnd.StateReset();
                orders.Add(toResetAtEnd);
            }
    }

    IMutableOrdersPriceVolumeLayer ITrackableReset<IMutableOrdersPriceVolumeLayer>.ResetWithTracking() => ResetWithTracking();

    IMutableOrdersPriceVolumeLayer IMutableOrdersPriceVolumeLayer.ResetWithTracking() => ResetWithTracking();

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

    public override bool AreEquivalent(IPriceVolumeLayer? other, bool exactTypes = false)
    {
        if (!(other is IOrdersPriceVolumeLayer otherTvl)) return false;
        var baseSame = base.AreEquivalent(other, exactTypes);
        var traderDetailsSame = orders!.Zip(otherTvl.Orders,
                                            (ftd, std) => ftd.AreEquivalent(std, exactTypes))
                                       .All(same => same);
        return baseSame && traderDetailsSame;
    }

    public override OrdersPriceVolumeLayer CopyFrom
    (IPriceVolumeLayer source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (orders != null && source is IMutableOrdersPriceVolumeLayer ordersCountPriceVolumeLayer)
        {
            for (var i = 0; i < ordersCountPriceVolumeLayer.OrdersCount; i++)
                if (i < orders.Count)
                    orders[i].CopyFrom(ordersCountPriceVolumeLayer[i]!);
                else
                    orders.Add(ConvertToBookLayer(ordersCountPriceVolumeLayer[i]!));

            for (var i = orders.Count - 1; i >= ordersCountPriceVolumeLayer.OrdersCount; i--) orders[i].StateReset();
        }

        return this;
    }

    public override OrdersPriceVolumeLayer Clone() =>
        Recycler?.Borrow<OrdersPriceVolumeLayer>().CopyFrom(this)
     ?? new OrdersPriceVolumeLayer(this, LayerType);

    private void AddLayer(IAnonymousOrderLayerInfo toAdd)
    {
        orders?.Add(ConvertToBookLayer(toAdd));
    }

    public IMutableAnonymousOrderLayerInfo ConvertToBookLayer(IAnonymousOrderLayerInfo toAdd)
    {
        if (LayerType.SupportsOrdersFullPriceVolume())
            return new ExternalCounterPartyOrderLayerInfo(toAdd)
            {
                Recycler = Recycler
            };
        return new AnonymousOrderLayerInfo(toAdd)
        {
            Recycler = Recycler
        };
    }

    public IMutableAnonymousOrderLayerInfo CreateNewBookOrderLayer()
    {
        if (LayerType.SupportsOrdersFullPriceVolume())
            return new ExternalCounterPartyOrderLayerInfo
            {
                Recycler = Recycler
            };
        return new AnonymousOrderLayerInfo
        {
            Recycler = Recycler
        };
    }


    [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
    private void AssertMaxOrdersNotExceeded(int proposedNewIndex)
    {
        if (proposedNewIndex > ushort.MaxValue) throw new ArgumentOutOfRangeException($"Max Traders represented is {ushort.MaxValue}");
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IOrdersPriceVolumeLayer?)obj, true);

    public override int GetHashCode() => base.GetHashCode() ^ Orders.GetHashCode();

    public override string ToString() => $"{nameof(OrdersPriceVolumeLayer)}{{{OrdersPriceVolumeLayerToStringMembers}}}";

    protected uint CountFromOrders()
    {
        for (var i = (orders?.Count ?? 0) - 1; i >= 0; i--)
        {
            var layerAtLevel = orders?[i];
            if (!layerAtLevel?.IsEmpty ?? true) return (uint)(i + 1);
        }

        return 0;
    }

    protected decimal InternalVolumeFromOrders()
    {
        return orders
               ?.Where(aoli =>
                           aoli.OrderLayerFlags.HasIsInternallyCreatedOrder()
                        && !aoli.OrderLayerFlags.HasNotLayerVolume()
                        && !aoli.OrderLayerFlags.HasIsSyntheticTrackingOrder())
               .Sum(aoli => aoli.OrderRemainingVolume) ?? 0m;
    }
}
