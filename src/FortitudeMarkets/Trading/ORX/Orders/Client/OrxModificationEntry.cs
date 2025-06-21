#region

using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeMarkets.Trading.Orders.SpotOrders;

#endregion

namespace FortitudeMarkets.Trading.ORX.Orders.Client;

public class OrxModificationEntry
{
    public OrxModificationEntry() { }

    public OrxModificationEntry(OrxOrderId orderId, OrderSide side, decimal price, long size)
    {
        OrderId = orderId;
        Side = side;
        Price = price;
        Size = size;
    }

    [OrxMandatoryField(1)] public OrxOrderId? OrderId { get; set; }

    [OrxMandatoryField(3)]
    private uint Quantity
    {
        get => (uint)((Side == OrderSide.Offer ? -1 : 1) * (int)Size);
        set
        {
            var amount = (int)value;
            Side = amount < 0 ? OrderSide.Offer : OrderSide.Bid;
            Size = Math.Abs(amount);
        }
    }

    public long Size { get; set; }
    public OrderSide Side { get; set; }

    [OrxMandatoryField(4)] public decimal Price { get; set; }
}
