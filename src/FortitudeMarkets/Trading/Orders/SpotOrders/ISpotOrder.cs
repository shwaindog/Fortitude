#region

using FortitudeMarkets.Trading.Orders.Products;
using FortitudeMarkets.Trading.Orders.Venues;

#endregion

namespace FortitudeMarkets.Trading.Orders.SpotOrders;

public interface ISpotOrder : IOrder
{
    OrderSide Side { get; set; }
    decimal Price { get; set; }
    decimal Size { get; set; }
    decimal DisplaySize { get; set; }
    OrderType Type { get; set; }
    decimal ExecutedPrice { get; set; }
    decimal ExecutedSize { get; set; }
    decimal SizeAtRisk { get; set; }
    decimal AllowedPriceSlippage { get; set; }
    decimal AllowedVolumeSlippage { get; set; }
    FillExpectation FillExpectation { get; set; }
    IVenuePriceQuoteId? QuoteInformation { get; set; }
}
