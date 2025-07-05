#region

using FortitudeCommon.Types;
using FortitudeMarkets.Trading.Orders.Venues;

#endregion

namespace FortitudeMarkets.Trading.Orders.SpotOrders;

public interface ISpotOrder : IOrder, ICloneable<ISpotOrder>
{
    OrderSide Side { get; }
    decimal Price { get; }
    decimal Size { get; }
    decimal DisplaySize { get; }
    OrderType Type { get; }
    decimal ExecutedPrice { get; }
    decimal ExecutedSize { get; }
    decimal SizeAtRisk { get; }
    decimal AllowedPriceSlippage { get; }
    decimal AllowedVolumeSlippage { get; }
    FillExpectation FillExpectation { get; }
    IVenuePriceQuoteId? QuoteInformation { get; }

    new ISpotOrder Clone();
}

public interface IMutableSpotOrder : IMutableOrder, ISpotOrder, ICloneable<IMutableSpotOrder>
{
    new OrderSide Side { get; set; }
    new decimal Price { get; set; }
    new decimal Size { get; set; }
    new decimal DisplaySize { get; set; }
    new OrderType Type { get; set; }
    new decimal ExecutedPrice { get; set; }
    new decimal ExecutedSize { get; set; }
    new decimal SizeAtRisk { get; set; }
    new decimal AllowedPriceSlippage { get; set; }
    new decimal AllowedVolumeSlippage { get; set; }
    new FillExpectation FillExpectation { get; set; }
    new IVenuePriceQuoteId? QuoteInformation { get; set; }

    new IMutableSpotOrder Clone();
}

public interface IAdditionalSpotOrderFields
{
    OrderSide? Side { get; }
    decimal? Price { get; }
    decimal? Size { get; }
    decimal? DisplaySize { get; }
    OrderType? Type { get; }
    decimal? ExecutedPrice { get; }
    decimal? ExecutedSize { get; }
    decimal? SizeAtRisk { get; }
    decimal? AllowedPriceSlippage { get; }
    decimal? AllowedVolumeSlippage { get; }
    FillExpectation? FillExpectation { get; }
    IVenuePriceQuoteId? QuoteInformation { get; }
}

public interface ISpotTransmittableOrder : ITransmittableOrder, IMutableSpotOrder, ICloneable<ISpotTransmittableOrder>
{
    IMutableSpotOrder AsSpotOrder { get; }

    new ISpotTransmittableOrder Clone();
}
