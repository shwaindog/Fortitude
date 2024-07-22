// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.TickerInfo;

[Flags]
public enum SourceTickerInfoUpdatedFlags : uint
{
    None                   = 0x00_00_00
  , SourceTickerId         = 0x00_00_01
  , SourceName             = 0x00_00_02
  , TickerName             = 0x00_00_04
  , PublishedQuoteLevel    = 0x00_00_08
  , MarketClassification   = 0x00_00_10
  , RoundingPrecision      = 0x00_00_20
  , Pip                    = 0x00_00_40
  , SubscribeToPrices      = 0x00_00_80
  , TradingEnabled         = 0x00_01_80
  , MinSubmitSize          = 0x00_02_00
  , MaxSubmitSize          = 0x00_04_00
  , IncrementSize          = 0x00_08_00
  , MinimumQuoteLife       = 0x00_10_00
  , DefaultMaxValidMs      = 0x00_20_00
  , LayerFlags             = 0x00_40_00
  , MaximumPublishedLayers = 0x00_80_00
  , LastTradedFlags        = 0x01_00_00
}
