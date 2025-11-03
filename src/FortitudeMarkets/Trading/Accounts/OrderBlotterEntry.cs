// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

#region

using System.Linq.Expressions;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Trading.Counterparties;
using FortitudeMarkets.Trading.Executions;
using FortitudeMarkets.Trading.Orders;
using FortitudeMarkets.Trading.Orders.Products;
using FortitudeMarkets.Trading.Orders.SpotOrders;
using FortitudeMarkets.Trading.Orders.Venues;

#endregion

namespace FortitudeMarkets.Trading.Accounts;

public interface IBlotterProperty : IPropertyChanged<IOrderBlotterEntry> { }

public class BlotterProperty<TProp>(Expression<Func<IOrderBlotterEntry?, TProp>> propertyGetExpression)
    : PropertyExtractor<IOrderBlotterEntry, TProp>(propertyGetExpression), IBlotterProperty { }

public interface IOrderBlotterEntry : IReusableObject<IOrderBlotterEntry>, IOrder, IAdditionalSpotOrderFields
  , IInterfacesComparable<IOrderBlotterEntry>
{
    IOrder AsOrder { get; }

    IRecyclableSet<IBlotterProperty> AllPopulatedFields(ISet<IBlotterProperty>? allWaysInclude = null);

    IRecyclableSet<IBlotterProperty> AllFields(ISet<IBlotterProperty>? toExclude = null);

    IRecyclableSet<IBlotterProperty> Changes(IOrderBlotterEntry previous);

    new IOrderBlotterEntry Clone();
}

public class OrderBlotterEntry : ReusableObject<IOrderBlotterEntry>, IOrderBlotterEntry
{
    private static readonly Recycler FieldsRecycler = new();

    public static readonly IBlotterProperty ProductTypeProperty    = new BlotterProperty<ProductType>(obe => obe!.ProductType);
    public static readonly IBlotterProperty CreationTimeProperty   = new BlotterProperty<DateTime>(obe => obe!.CreationTime);
    public static readonly IBlotterProperty LastUpdateTimeProperty = new BlotterProperty<DateTime>(obe => obe!.LastUpdateTime);
    public static readonly IBlotterProperty OrderIdProperty        = new BlotterProperty<IOrderId>(obe => obe!.OrderId);
    public static readonly IBlotterProperty TickerIdProperty       = new BlotterProperty<ushort>(obe => obe!.TickerId);
    public static readonly IBlotterProperty TickerProperty         = new BlotterProperty<string?>(obe => obe!.Ticker);
    public static readonly IBlotterProperty StatusProperty         = new BlotterProperty<OrderStatus>(obe => obe!.Status);
    public static readonly IBlotterProperty DoneTimeProperty       = new BlotterProperty<DateTime?>(obe => obe!.DoneTime);
    public static readonly IBlotterProperty SubmitTimeProperty     = new BlotterProperty<DateTime?>(obe => obe!.SubmitTime);
    public static readonly IBlotterProperty VenueOrdersProperty    = new BlotterProperty<IVenueOrders?>(obe => obe!.VenueOrders);
    public static readonly IBlotterProperty ExecutionsProperty     = new BlotterProperty<IExecutions?>(obe => obe!.Executions);
    public static readonly IBlotterProperty IsCompleteProperty     = new BlotterProperty<bool>(obe => obe!.IsComplete);
    public static readonly IBlotterProperty PartiesProperty        = new BlotterProperty<IParties>(obe => obe!.Parties);
    public static readonly IBlotterProperty IsErrorProperty        = new BlotterProperty<bool>(obe => obe!.IsError);
    public static readonly IBlotterProperty MessageProperty        = new BlotterProperty<string?>(obe => obe!.Message);
    public static readonly IBlotterProperty TimeInForceProperty    = new BlotterProperty<TimeInForce>(obe => obe!.TimeInForce);

    public static readonly IBlotterProperty VenueSelectionCriteriaProperty
        = new BlotterProperty<IVenueCriteria?>(obe => obe!.VenueSelectionCriteria);

    public static readonly IBlotterProperty SideProperty  = new BlotterProperty<OrderSide?>(obe => obe!.Side);
    public static readonly IBlotterProperty SizeProperty  = new BlotterProperty<decimal?>(obe => obe!.Size);
    public static readonly IBlotterProperty TypeProperty  = new BlotterProperty<OrderType?>(obe => obe!.Type);
    public static readonly IBlotterProperty PriceProperty = new BlotterProperty<decimal?>(obe => obe!.Price);

    public static readonly IBlotterProperty ExecutedPriceProperty = new BlotterProperty<decimal?>(obe => obe!.ExecutedPrice);
    public static readonly IBlotterProperty ExecutedSizeProperty  = new BlotterProperty<decimal?>(obe => obe!.ExecutedSize);

    public static readonly IBlotterProperty AllowedPriceSlippageProperty  = new BlotterProperty<decimal?>(obe => obe!.AllowedPriceSlippage);
    public static readonly IBlotterProperty AllowedVolumeSlippageProperty = new BlotterProperty<decimal?>(obe => obe!.AllowedVolumeSlippage);
    public static readonly IBlotterProperty FillExpectationProperty       = new BlotterProperty<FillExpectation?>(obe => obe!.FillExpectation);
    public static readonly IBlotterProperty QuoteInformationProperty      = new BlotterProperty<IVenuePriceQuoteId?>(obe => obe!.QuoteInformation);

    public static readonly IBlotterProperty DisplaySizeProperty = new BlotterProperty<decimal?>(obe => obe!.DisplaySize);
    public static readonly IBlotterProperty SizeAtRiskProperty  = new BlotterProperty<decimal?>(obe => obe!.SizeAtRisk);

    private static readonly InsertionOrderSet<IBlotterProperty> AllOrderFields =
    [
        ProductTypeProperty, CreationTimeProperty, LastUpdateTimeProperty, OrderIdProperty, TickerIdProperty, TickerProperty, StatusProperty
      , DoneTimeProperty, SubmitTimeProperty, VenueSelectionCriteriaProperty, VenueOrdersProperty, ExecutionsProperty, IsCompleteProperty
      , PartiesProperty, IsErrorProperty, MessageProperty, TimeInForceProperty
    ];

    private static readonly InsertionOrderSet<IBlotterProperty> AllSpotOrderFields =
    [
        ProductTypeProperty, CreationTimeProperty, LastUpdateTimeProperty, OrderIdProperty, TickerIdProperty, TickerProperty, StatusProperty
      , SideProperty, SizeProperty, TypeProperty, PriceProperty, ExecutedPriceProperty, ExecutedSizeProperty, SizeAtRiskProperty
      , AllowedPriceSlippageProperty, AllowedVolumeSlippageProperty, FillExpectationProperty, QuoteInformationProperty, DoneTimeProperty
      , SubmitTimeProperty, VenueSelectionCriteriaProperty, VenueOrdersProperty, ExecutionsProperty, IsCompleteProperty, PartiesProperty
      , IsErrorProperty, MessageProperty, TimeInForceProperty, DisplaySizeProperty
    ];


    protected readonly IOrder WrappedOrder;

    static OrderBlotterEntry()
    {
        AllOrderFields.Recycler     = FieldsRecycler;
        AllSpotOrderFields.Recycler = FieldsRecycler;
    }

    public OrderBlotterEntry() => WrappedOrder = null!;

    protected OrderBlotterEntry(IOrderBlotterEntry toWrap) => WrappedOrder = toWrap.AsOrder.Clone();

    protected ISpotOrder? AsSpotOrder => WrappedOrder as ISpotOrder;

    public virtual ProductType ProductType => WrappedOrder.ProductType;

    public IOrder      AsOrder        => WrappedOrder;
    public IOrderId    OrderId        => WrappedOrder.OrderId;
    public ushort      TickerId       => WrappedOrder.TickerId;
    public IParties    Parties        => WrappedOrder.Parties;
    public OrderStatus Status         => WrappedOrder.Status;
    public TimeInForce TimeInForce    => WrappedOrder.TimeInForce;
    public DateTime    CreationTime   => WrappedOrder.CreationTime;
    public DateTime    LastUpdateTime => WrappedOrder.LastUpdateTime;

    public DateTime? SubmitTime => WrappedOrder.SubmitTime;
    public DateTime? DoneTime   => WrappedOrder.DoneTime;

    public IVenueCriteria? VenueSelectionCriteria => WrappedOrder.VenueSelectionCriteria;

    public IVenueOrders? VenueOrders => WrappedOrder.VenueOrders;
    public IExecutions?  Executions  => WrappedOrder.Executions;

    public bool    IsComplete => WrappedOrder.IsComplete;
    public bool    IsError    => WrappedOrder.IsError;
    public string? Ticker     => WrappedOrder.Ticker;
    public string? Message    => WrappedOrder.Message;


    #region SpotOrderFields

    public OrderSide? Side  => AsSpotOrder?.Side;
    public decimal?   Size  => AsSpotOrder?.Size;
    public OrderType? Type  => AsSpotOrder?.Type;
    public decimal?   Price => AsSpotOrder?.Price;

    public decimal? DisplaySize   => AsSpotOrder?.DisplaySize;
    public decimal? SizeAtRisk    => AsSpotOrder?.SizeAtRisk;
    public decimal? ExecutedPrice => AsSpotOrder?.ExecutedPrice;
    public decimal? ExecutedSize  => AsSpotOrder?.ExecutedSize;

    public decimal? AllowedPriceSlippage  => AsSpotOrder?.AllowedPriceSlippage;
    public decimal? AllowedVolumeSlippage => AsSpotOrder?.AllowedVolumeSlippage;

    public FillExpectation?    FillExpectation  => AsSpotOrder?.FillExpectation;
    public IVenuePriceQuoteId? QuoteInformation => AsSpotOrder?.QuoteInformation;

    #endregion SpotOrderFields

    public IRecyclableSet<IBlotterProperty> AllFields(ISet<IBlotterProperty>? toExclude = null)
    {
        var fullSetCopy =
            ProductType switch
            {
                ProductType.Spot => AllSpotOrderFields.Clone()

              , _ => AllSpotOrderFields.Clone()
            };

        if (toExclude.IsNotNullOrNone()) fullSetCopy.SymmetricExceptWith(toExclude);
        return fullSetCopy;
    }

    public IRecyclableSet<IBlotterProperty> AllPopulatedFields(ISet<IBlotterProperty>? allWaysInclude = null)
    {
        var fullSetCopy =
            ProductType switch
            {
                ProductType.Spot => AllSpotOrderFields.Clone()

              , _ => AllSpotOrderFields.Clone()
            };

        var populatedFields = RemoveUnpopulatedFields(fullSetCopy);

        if (allWaysInclude.IsNotNullOrNone()) populatedFields.UnionWith(allWaysInclude);
        return populatedFields;
    }

    public IRecyclableSet<IBlotterProperty> Changes(IOrderBlotterEntry previous)
    {
        var toTrim = AllFields();
        for (int i = 0; i < toTrim.Count; i++)
        {
            var propertyComparer = toTrim[i];
            if (!propertyComparer.IsPropertyDifferent(previous, this)) continue;
            toTrim.Remove(propertyComparer);
        }
        return toTrim;
    }

    public override void StateReset()
    {
        WrappedOrder.StateReset();
        base.StateReset();
    }

    object ICloneable.Clone() => Clone();

    IOrder ICloneable<IOrder>.Clone() => Clone();


    IReusableObject<IOrder> ITransferState<IReusableObject<IOrder>>.CopyFrom
        (IReusableObject<IOrder> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IOrder)source, copyMergeFlags);

    public override IOrderBlotterEntry CopyFrom(IOrderBlotterEntry source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        CopyFrom(source, copyMergeFlags);

    IOrder ITransferState<IOrder>.CopyFrom(IOrder source, CopyMergeFlags copyMergeFlags) => CopyFrom(source, copyMergeFlags);

    public bool AreEquivalent(IOrder? other, bool exactTypes = false)
    {
        if (other is not ITransmittableOrder transmittableOrder) return false;

        var wrappedSame = WrappedOrder.AreEquivalent(transmittableOrder.AsOrder, exactTypes);

        var allAreSame = wrappedSame;

        return allAreSame;
    }

    public virtual string OrderToStringMembers => WrappedOrder.OrderToStringMembers;

    private InsertionOrderSet<IBlotterProperty> RemoveUnpopulatedFields(InsertionOrderSet<IBlotterProperty> toTrimSet)
    {
        foreach (var fieldName in toTrimSet)
            switch (fieldName.PropertyName)
            {
                case nameof(Ticker):
                    if (Ticker.IsNullOrEmpty()) toTrimSet.Remove(TickerProperty);
                    break;
                case nameof(LastUpdateTime):
                    if (DoneTime == null) toTrimSet.Remove(LastUpdateTimeProperty);
                    break;
                case nameof(DoneTime):
                    if (DoneTime == null) toTrimSet.Remove(DoneTimeProperty);
                    break;
                case nameof(SubmitTime):
                    if (SubmitTime == null) toTrimSet.Remove(SubmitTimeProperty);
                    break;
                case nameof(VenueSelectionCriteria):
                    if (VenueSelectionCriteria == null) toTrimSet.Remove(VenueSelectionCriteriaProperty);
                    break;
                case nameof(VenueOrders):
                    if (VenueOrders.IsNullOrNone()) toTrimSet.Remove(VenueOrdersProperty);
                    break;
                case nameof(Executions):
                    if (Executions.IsNullOrNone()) toTrimSet.Remove(ExecutionsProperty);
                    break;
                case nameof(Message):
                    if (Message.IsNullOrEmpty()) toTrimSet.Remove(MessageProperty);
                    break;
                case nameof(Side):
                    if (Side == null) toTrimSet.Remove(SideProperty);
                    break;
                case nameof(Size):
                    if (Size == null) toTrimSet.Remove(SizeProperty);
                    break;
                case nameof(Type):
                    if (Type == null) toTrimSet.Remove(TypeProperty);
                    break;
                case nameof(Price):
                    if (Price == null) toTrimSet.Remove(PriceProperty);
                    break;
                case nameof(DisplaySize):
                    if (DisplaySize == null) toTrimSet.Remove(DisplaySizeProperty);
                    break;
                case nameof(SizeAtRisk):
                    if (SizeAtRisk == null) toTrimSet.Remove(SizeAtRiskProperty);
                    break;
                case nameof(ExecutedPrice):
                    if (ExecutedPrice == null) toTrimSet.Remove(ExecutedPriceProperty);
                    break;
                case nameof(ExecutedSize):
                    if (ExecutedSize == null) toTrimSet.Remove(ExecutedSizeProperty);
                    break;
                case nameof(AllowedPriceSlippage):
                    if (AllowedPriceSlippage == null) toTrimSet.Remove(AllowedVolumeSlippageProperty);
                    break;
                case nameof(AllowedVolumeSlippage):
                    if (AllowedVolumeSlippage == null) toTrimSet.Remove(AllowedVolumeSlippageProperty);
                    break;
                case nameof(FillExpectation):
                    if (FillExpectation == null) toTrimSet.Remove(FillExpectationProperty);
                    break;
                case nameof(QuoteInformation):
                    if (QuoteInformation == null) toTrimSet.Remove(QuoteInformationProperty);
                    break;
            }

        return toTrimSet;
    }

    public override OrderBlotterEntry Clone() =>
        Recycler?.Borrow<OrderBlotterEntry>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new OrderBlotterEntry(this);

    public OrderBlotterEntry CopyFrom(IOrder source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is IOrderBlotterEntry blotterOrder)
        {
            var sourceWrappedOrder = blotterOrder.AsOrder;
            WrappedOrder.CopyFrom(sourceWrappedOrder, copyMergeFlags);
        }
        else
        {
            WrappedOrder.CopyFrom(source, copyMergeFlags);
        }

        return this;
    }

    // Only OrderIds are compared
    public bool AreEquivalent(IOrderBlotterEntry? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var orderIdSame     = OrderId.AreEquivalent(other.OrderId);
        var tickerIdSame    = OrderId.AreEquivalent(other.OrderId);
        var createdTimeSame = CreationTime == other.CreationTime;

        var allAreSame = orderIdSame && tickerIdSame && createdTimeSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IOrderBlotterEntry, true);

    public override int GetHashCode()
    {
        var hashCode = OrderId.GetHashCode();
        hashCode = (hashCode * 397) ^ TickerId.GetHashCode();
        hashCode = (hashCode * 397) ^ CreationTime.GetHashCode();
        return hashCode;
    }

    public override string ToString() => $"{nameof(OrderBlotterEntry)}{{{OrderToStringMembers}}}";
}
