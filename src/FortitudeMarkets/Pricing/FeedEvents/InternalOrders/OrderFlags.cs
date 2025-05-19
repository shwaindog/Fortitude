// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeMarkets.Pricing.FeedEvents.InternalOrders;

[Flags]
public enum OrderFlags : uint
{
    None                        = 0x00_00_00_00
  , FromAdapter                 = 0x00_00_00_01
  , IsActive                    = 0x00_00_00_02
  , IsExternalOrder             = 0x00_00_00_04
  , HasExternalCounterPartyInfo = 0x00_00_00_08
  , IsInternal                  = 0x00_00_00_10
  , HasInternalOrderInfo        = 0x00_00_00_20

    // virtual is for tracking intended with children being actual
  , IsInternalVirtual           = 0x00_00_01_00
  , IsExternalSyntheticTracking = 0x00_00_02_00
  , IsMarketSubmittable         = 0x00_00_04_00

  , IsOneCancelTheOtherEntry      = 0x00_00_10_00
  , IsOneCancelTheOtherTakeProfit = 0x00_00_20_00
  , IsOneCancelTheOtherStopLoss   = 0x00_00_40_00

    // 
  , IsParentOrder = 0x00_01_00_00
  , IsChildOrder  = 0x00_02_00_00
  , HasTrackingId = 0x00_04_00_00

    // Internal Orders
  , HasTargetTakeProfitPrice = 0x00_10_00_00
  , HasTargetStopLossPrice   = 0x00_20_00_00
  , HasClosingOrder          = 0x00_40_00_00

    // backtest simulator
  , IsComparisonAccountOrder      = 0x08_00_00_00
  , FromSimulator                 = 0x80_00_00_00
  , IsSimulatorNoLimitsOrder      = 0x90_00_00_00
  , IsSimulatorSimLimitsOnlyOrder = 0xB0_00_00_00
  , IsSimulatorAllLimitsOrder     = 0xF0_00_00_00
}


public static class OrderFlagsExtensions
{

    public static bool IsExternalOrder(this OrderFlags flags)             => (flags & OrderFlags.IsExternalOrder) > 0;
    public static bool HasExternalCounterPartyInfo(this OrderFlags flags) => (flags & OrderFlags.HasExternalCounterPartyInfo) > 0;
    public static bool IsInternalOrder(this OrderFlags flags)             => (flags & OrderFlags.IsInternal) > 0;
    public static bool HasInternalOrderInfo(this OrderFlags flags)        => (flags & OrderFlags.HasInternalOrderInfo) > 0;
    
    public static bool HasAllOf(this OrderFlags flags, OrderFlags checkAllFound)    => (flags & checkAllFound) == checkAllFound;
    public static bool HasNoneOf(this OrderFlags flags, OrderFlags checkNonAreSet)  => (flags & checkNonAreSet) == 0;
    public static bool HasAnyOf(this OrderFlags flags, OrderFlags checkAnyAreFound) => (flags & checkAnyAreFound) > 0;
    public static bool IsExactly(this OrderFlags flags, OrderFlags checkAllFound)   => flags == checkAllFound;
}