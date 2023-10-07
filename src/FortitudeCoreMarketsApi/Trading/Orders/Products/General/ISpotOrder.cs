using FortitudeCommon.Types.Mutable;
using FortitudeMarketsApi.Trading.Orders.Venues;

namespace FortitudeMarketsApi.Trading.Orders.Products.General
{
    public interface ISpotOrder : IProductOrder
    {
        OrderSide Side { get; set; }
        IMutableString Ticker { get; set; }
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
        IVenuePriceQuoteId QuoteInformation { get; set; }
    }
}
