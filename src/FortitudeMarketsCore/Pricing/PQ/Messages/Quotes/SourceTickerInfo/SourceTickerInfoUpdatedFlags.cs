// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.SourceTickerInfo;

[Flags]
public enum SourceTickerInfoUpdatedFlags : uint
{
    None                   = 0x0000
  , SourceTickerId         = 0x0001
  , SourceName             = 0x0002
  , TickerName             = 0x0004
  , PublishedQuoteLevel    = 0x0008
  , MarketClassification   = 0x0010
  , RoundingPrecision      = 0x0020
  , MinSubmitSize          = 0x0040
  , MaxSubmitSize          = 0x0080
  , IncrementSize          = 0x0100
  , MinimumQuoteLife       = 0x0200
  , LayerFlags             = 0x0400
  , MaximumPublishedLayers = 0x0800
  , LastTradedFlags        = 0x1000
}
