// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeMarkets.Pricing.FeedEvents.DeltaUpdates;

[Flags]
public enum ListTransmissionFlags : ushort
{
    None                                 = 0x00_00 //
  , LimitByPeriodTime                    = 0x00_01 //
  , LimitByTradeCount                    = 0x00_02 //
  , DoNotUpdateFromOnTickUpdates         = 0x00_04 //
  , OnlyWhenCopyMergeFlagsKeepCacheItems = 0x00_08 //
  , DoNotAutoRemoveExpiredPeriod         = 0x01_00 // 
  , DoNotAutoRemoveExceededCount         = 0x02_00 // 
  , PublishesOnDeltaUpdates              = 0x04_00 // expected matched with  DoNotUpdateFromOnTickUpdates
  , PublishOnCompleteOrSnapshot          = 0x08_00 // if neither this or above set then it never publishes
  , ResetOnNewTradingDay                 = 0x10_00 // _
}


public static class LastTradedTransmissionFlagsExtensions
{
    public static bool HasLimitByPeriodTimeFlag(this ListTransmissionFlags flags) =>
        (flags & ListTransmissionFlags.LimitByPeriodTime) > 0;

    public static bool HasLimitByTradeCountFlag(this ListTransmissionFlags flags) =>
        (flags & ListTransmissionFlags.LimitByTradeCount) > 0;

    public static bool HasDoNotUpdateFromOnTickUpdatesFlag(this ListTransmissionFlags flags) =>
        (flags & ListTransmissionFlags.LimitByTradeCount) > 0;

    public static bool HasOnlyWhenCopyMergeFlagsKeepCacheItemsFlag(this ListTransmissionFlags flags) =>
        (flags & ListTransmissionFlags.OnlyWhenCopyMergeFlagsKeepCacheItems) > 0;

    public static bool HasDoNotAutoRemoveExpiredPeriodFlag(this ListTransmissionFlags flags) =>
        (flags & ListTransmissionFlags.DoNotAutoRemoveExpiredPeriod) > 0;

    public static bool HasDoNotAutoRemoveExceededCountFlag(this ListTransmissionFlags flags) =>
        (flags & ListTransmissionFlags.DoNotAutoRemoveExceededCount) > 0;

    public static bool HasPublishesOnDeltaUpdatesFlag(this ListTransmissionFlags flags) =>
        (flags & ListTransmissionFlags.PublishesOnDeltaUpdates) > 0;

    public static bool HasPublishOnCompleteOrSnapshotFlag(this ListTransmissionFlags flags) =>
        (flags & ListTransmissionFlags.PublishOnCompleteOrSnapshot) > 0;

    public static bool HasResetOnNewTradingDayFlag(this ListTransmissionFlags flags) =>
        (flags & ListTransmissionFlags.ResetOnNewTradingDay) > 0;
}
