// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using static FortitudeMarkets.Pricing.Quotes.LastTraded.LastTradedFlags;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LastTraded;

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

    public static LastTradedFlags SupportedLastTradedFlags(this LastTradeType lastTradedType)
    {
        switch (lastTradedType)
        {
            case LastTradeType.Price:                  return LastTradedPrice;
            case LastTradeType.PricePaidOrGivenVolume: return PaidOrGiven | LastTradedPrice | LastTradedVolume | LastTradedPrice;
            case LastTradeType.PriceLastTraderName:
            case LastTradeType.PriceLastTraderPaidOrGivenVolume:
                return TraderName | PaidOrGiven | LastTradedPrice | LastTradedVolume | LastTradedPrice;
            default: return None;
        }
    }
}
