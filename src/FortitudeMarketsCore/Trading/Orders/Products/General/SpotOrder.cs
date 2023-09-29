using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Client;
using FortitudeMarketsApi.Trading.Orders.Products;
using FortitudeMarketsApi.Trading.Orders.Products.General;
using FortitudeMarketsApi.Trading.Orders.Venues;

namespace FortitudeMarketsCore.Trading.Orders.Products.General
{
    public class SpotOrder : ProductOrder, ISpotOrder
    {
        public SpotOrder(ISpotOrder toClone) : base(toClone)
        {
            Side = toClone.Side;
            Ticker = toClone.Ticker;
            Price = toClone.Price;
            Size = toClone.Size;
            DisplaySize = toClone.DisplaySize;
            Type = toClone.Type;
            ExecutedPrice = toClone.ExecutedPrice;
            ExecutedSize = toClone.ExecutedSize;
            SizeAtRisk = toClone.SizeAtRisk;
            AllowedPriceSlippage = toClone.AllowedPriceSlippage;
            AllowedVolumeSlippage = toClone.AllowedVolumeSlippage;
            FillExpectation = toClone.FillExpectation;
            QuoteInformation = toClone.QuoteInformation;
        }

        public SpotOrder()
        {
        }

        public SpotOrder(OrderSide side, string ticker, decimal price, decimal size,
            OrderType type, decimal displaySize = 0m, decimal allowedPriceSlippage = 0m,
            decimal allowedVolumeSlippage = 0m, decimal executedPrice = 0m, decimal executedSize = 0m,
            decimal sizeAtRisk = 0m, FillExpectation fillExpectation = FillExpectation.Complete,
            IVenuePriceQuoteId quoteInformation = null)
            : this(side, (MutableString)ticker, price, size, type, displaySize, allowedPriceSlippage, 
                allowedVolumeSlippage, executedPrice, executedSize, sizeAtRisk, fillExpectation, quoteInformation)
        {
        }

        public SpotOrder(OrderSide side, IMutableString ticker, decimal price, decimal size,
            OrderType type, decimal displaySize = 0m, decimal allowedPriceSlippage = 0m, 
            decimal allowedVolumeSlippage = 0m, decimal executedPrice = 0m, decimal executedSize = 0m, 
            decimal sizeAtRisk = 0m, FillExpectation fillExpectation = FillExpectation.Complete, 
            IVenuePriceQuoteId quoteInformation = null)
        {
            Side = side;
            Ticker = ticker;
            Price = price;
            Size = size;
            DisplaySize = displaySize;
            Type = type;
            ExecutedPrice = executedPrice;
            ExecutedSize = executedSize;
            SizeAtRisk = sizeAtRisk;
            AllowedPriceSlippage = allowedPriceSlippage;
            AllowedVolumeSlippage = allowedVolumeSlippage;
            FillExpectation = fillExpectation;
            QuoteInformation = quoteInformation;
        }

        public override ProductType ProductType => ProductType.Spot;
        public OrderSide Side { get; set; }
        public IMutableString Ticker { get; set; }
        public decimal Price { get; set; }
        public decimal Size { get; set; }
        public decimal DisplaySize { get; set; }
        public OrderType Type { get; set; }
        public decimal ExecutedPrice { get; set; }
        public decimal ExecutedSize { get; set; }
        public decimal SizeAtRisk { get; set; }
        public decimal AllowedPriceSlippage { get; set; }
        public decimal AllowedVolumeSlippage { get; set; }
        public FillExpectation FillExpectation { get; set; }
        public IVenuePriceQuoteId QuoteInformation { get; set; }

        public void SetError(string msg, long sizeAtRisk)
        {
            SetError((MutableString)msg, sizeAtRisk);
        }

        public void SetError(IMutableString msg, long sizeAtRisk)
        {
            Message = msg;
            SizeAtRisk = sizeAtRisk;
            Order.Status = OrderStatus.Dead;
        }

        public override bool IsError => Message != null || Size < ExecutedSize;

        public override void RegisterExecution(IExecution execution)
        {
            ExecutedPrice = (ExecutedPrice * ExecutedSize + execution.Price * execution.Quantity) / 
                            (ExecutedSize + execution.Quantity);
            ExecutedSize += execution.Quantity;
        }

        public override void ApplyAmendment(IOrderAmend amendment)
        {
            Price = amendment.NewPrice;
            Size = amendment.NewQuantity;
            Side = amendment.NewSide;
        }

        public override bool RequiresAmendment(IOrderAmend amendment)
        {
            return amendment.NewPrice != Price ||
                   amendment.NewQuantity != Size ||
                   amendment.NewSide != Side;
        }

        public override void CopyFrom(IProductOrder source, IRecycler recycler)
        {
            base.CopyFrom(source, recycler);
            if (source is ISpotOrder spotOrder)
            {
                Side = spotOrder.Side;
                Ticker = spotOrder.Ticker;
                Price = spotOrder.Price;
                Size = spotOrder.Size;
                DisplaySize = spotOrder.DisplaySize;
                Type = spotOrder.Type;
                ExecutedPrice = spotOrder.ExecutedPrice;
                ExecutedSize = spotOrder.ExecutedSize;
                SizeAtRisk = spotOrder.SizeAtRisk;
                AllowedPriceSlippage = spotOrder.AllowedPriceSlippage;
                AllowedVolumeSlippage = spotOrder.AllowedVolumeSlippage;
                FillExpectation = spotOrder.FillExpectation;
                QuoteInformation = spotOrder.QuoteInformation;
            }
        }

        public override IProductOrder Clone()
        {
            return new SpotOrder(this);
        }
    }
}