// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Trading.Orders.Products;

#endregion

namespace FortitudeMarkets.Trading.Orders.Client;

public class OrderAmend : ReusableObject<IOrderAmend>, IOrderAmend
{
    public OrderAmend() { }

    public OrderAmend(IOrderAmend toClone)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public OrderAmend
    (decimal newQuantity, decimal newPrice = -1m, OrderSide newSide = OrderSide.None,
        decimal newDisplaySize = -1m)
    {
        NewQuantity = newQuantity;

        NewPrice = newPrice;
        NewSide  = newSide;

        NewDisplaySize = newDisplaySize;
    }

    public decimal NewDisplaySize { get; set; }

    public decimal NewQuantity { get; set; }
    public decimal NewPrice    { get; set; }

    public OrderSide NewSide { get; set; }

    public override void StateReset()
    {
        NewDisplaySize = 0;

        NewQuantity = 0;
        NewPrice    = 0;
        NewSide     = OrderSide.None;
        base.StateReset();
    }

    public override IOrderAmend CopyFrom(IOrderAmend source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        NewDisplaySize = source.NewDisplaySize;

        NewQuantity = source.NewQuantity;
        NewPrice    = source.NewPrice;
        NewSide     = source.NewSide;
        return this;
    }

    public override IOrderAmend Clone() => Recycler?.Borrow<OrderAmend>().CopyFrom(this) ?? new OrderAmend(this);
}
