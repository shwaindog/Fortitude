﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Candles;

[Flags]
public enum CandleUpdatedFlags : uint
{
    None                        = 0x00_0000
  , CandlePeriodFlag           = 0x00_0001
  , StartTimeDateFlag           = 0x00_0002
  , StartTimeSub2MinFlag        = 0x00_0004
  , EndTimeDateFlag             = 0x00_0008
  , EndTimeSub2MinFlag          = 0x00_0010
  , StartBidPriceFlag           = 0x00_0020
  , StartAskPriceFlag           = 0x00_0040
  , HighestBidPriceFlag         = 0x00_0080
  , HighestAskPriceFlag         = 0x00_0100
  , LowestBidPriceFlag          = 0x00_0200
  , LowestAskPriceFlag          = 0x00_0400
  , EndBidPriceFlag             = 0x00_0800
  , EndAskPriceFlag             = 0x00_1000
  , TickCountFlag               = 0x00_2000
  , PeriodVolumeFlag            = 0x00_4000
  , CandleFlagsFlag = 0x00_8000
  , AverageBidPriceFlag         = 0x01_0000
  , AverageAskPriceFlag         = 0x02_0000
}
