#region

using static FortitudeMarketsApi.Pricing.LastTraded.LastTradedFlags;

#endregion

namespace FortitudeMarketsApi.Pricing.LastTraded;

public enum LastTradeType
{
    None
    , Price
    , PricePaidOrGivenVolume
    , PriceLastTraderName
    , PriceLastTraderPaidOrGivenVolume
}

public static class LastTradeTypeExtensions
{
    public static LastTradeType MostCompactLayerType(this LastTradedFlags lastTradedFlags)
    {
        return lastTradedFlags switch
        {
            None => LastTradeType.None
            , _ when (lastTradedFlags & (LastTradedPrice | LastTradedTime)) > 0 &&
                     (lastTradedFlags & (LastTradedVolume | PaidOrGiven | TraderName)) == 0 => LastTradeType.Price
            , _ when ((lastTradedFlags & LastTradedVolume) | PaidOrGiven) > 0 && (lastTradedFlags & TraderName) == 0 => LastTradeType
                .PricePaidOrGivenVolume
            , _ when (lastTradedFlags & TraderName) > 0 && (lastTradedFlags & (PaidOrGiven | LastTradedVolume)) == 0 => LastTradeType
                .PriceLastTraderName
            , _ => LastTradeType.PriceLastTraderPaidOrGivenVolume
        };
    }
}
