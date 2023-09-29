using System;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Products;
using FortitudeMarketsCore.Trading.ORX.Serialization;

namespace FortitudeMarketsCore.Trading.ORX.Orders.Client
{
    public class OrxModificationEntry
    {
        [OrxMandatoryField(1)]
        public OrxOrderId OrderId { get; set; }
        [OrxMandatoryField(3)]
        private uint Quantity
        {
            get => (uint)((Side == OrderSide.Offer ? -1 : 1) * (int)Size);
            set
            {
                int amount = (int)value;
                Side = amount < 0 ? OrderSide.Offer : OrderSide.Bid;
                Size = Math.Abs(amount);
            }
        }
        public long Size { get; set; }
        public OrderSide Side { get; set; }
        [OrxMandatoryField(4)]
        public decimal Price { get; set; }

        public OrxModificationEntry() { }
        
        public OrxModificationEntry(OrxOrderId orderId, OrderSide side, decimal price, long size)
        {
            OrderId = orderId;
            Side = side;
            Price = price;
            Size = size;
        }
    }
}
