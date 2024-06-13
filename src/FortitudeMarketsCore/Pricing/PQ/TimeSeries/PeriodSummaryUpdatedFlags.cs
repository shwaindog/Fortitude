// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarketsCore.Pricing.PQ.TimeSeries;

[Flags]
public enum PeriodSummaryUpdatedFlags : uint
{
    None                       = 0x00_0000
  , StartTimeDateFlag          = 0x00_0001
  , StartTimeSubHourFlag       = 0x00_0002
  , EndTimeDateFlag            = 0x00_0004
  , EndTimeSubHourFlag         = 0x00_0008
  , StartBidPriceFlag          = 0x00_0010
  , StartAskPriceFlag          = 0x00_0020
  , HighestBidPriceFlag        = 0x00_0040
  , HighestAskPriceFlag        = 0x00_0080
  , LowestBidPriceFlag         = 0x00_0100
  , LowestAskPriceFlag         = 0x00_0200
  , EndBidPriceFlag            = 0x00_0400
  , EndAskPriceFlag            = 0x00_0800
  , TickCountFlag              = 0x00_1000
  , PeriodVolumeLowerBytesFlag = 0x00_2000
  , PeriodVolumeUpperBytesFlag = 0x00_4000
  , AverageBidPriceFlag        = 0x00_8000
  , AverageAskPriceFlag        = 0x01_0000
}
