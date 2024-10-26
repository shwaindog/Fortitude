// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarkets.Pricing.Quotes.LastTraded;

[Flags]
public enum LastTradedFlags : ushort
{
    None             = 0x00
  , LastTradedPrice  = 0x01
  , LastTradedTime   = 0x02
  , LastTradedVolume = 0x04
  , PaidOrGiven      = 0x08
  , TraderName       = 0x10
}
