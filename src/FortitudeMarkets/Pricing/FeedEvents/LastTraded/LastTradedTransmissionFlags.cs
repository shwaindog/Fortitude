// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeMarkets.Pricing.FeedEvents.LastTraded;

[Flags]
public enum LastTradedTransmissionFlags : ushort
{
    None                                 = 0x00_00 // _
  , LimitByPeriodTime                    = 0x00_01 // _
  , LimitByTradeCount                    = 0x00_02 // at roughly 150+ string lenh bytes- 100 last trades updates is  ~15k
  , DoNotUpdateFromOnTickLastTraded      = 0x00_04 // _
  , OnlyWhenCopyMergeFlagsKeepCacheItems = 0x00_08 // _
  , DoNotAutoRemoveExpiredPeriod         = 0x01_00 // should only be set on internal last trades 
  , DoNotAutoRemoveExceededCount         = 0x02_00 // "
  , PublishesOnDeltaUpdates              = 0x04_00 // "  expected matched with  DoNotUpdateFromOnTickLastTraded
  , PublishOnCompleteOrSnapshot          = 0x08_00 // if neither this or above set then it never publishes
  , ResetOnNewTradingDay                 = 0x10_00 // _
}
