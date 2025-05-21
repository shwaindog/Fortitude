// Licensed under the MIT license.                                                                                                            
// Copyright Alexis Sawenko 2025 all rights reserved                                                                                          

namespace FortitudeMarkets.Pricing.FeedEvents.InternalOrders;

[Flags]
public enum OrderGenesisFlags : uint
{
    None                                      = 0x00_00_00_00 //  Permutations                                         
  , FromAdapter                               = 0x00_00_00_01 //  1. Parent Order is Market Entry - Submitted             
  , IsAliveOnAdapter                          = 0x00_00_00_02 //         Child is Either SL or TP
  , IsExternalOrder                           = 0x00_00_00_04 //  2. Parent Order is virtual Target Price
  , HasExternalCounterPartyInfo               = 0x00_00_00_08 //         Child is fill attempts from different venues
  , IsInternal                                = 0x00_00_00_10 //  3. No Parent is Managed Source position with TP and SL
  , HasInternalOrderInfo                      = 0x00_00_00_20 //  4. Parent is just position entry with TBD child TP and SL
  , HasTrackingId                             = 0x00_00_00_40 //  5. No Parent just position change order no TP or SL
  , VolumeNotPartOfLiquidity                  = 0x00_00_00_80 //  6. Parent is Managed Source position Order
  , InternalIsSourceManagedPositionOrder      = 0x00_00_01_00 //        Child orders are SL and TP
  , InternalIsJustPositionChangeOrder         = 0x00_00_02_00 //  7. Parent is a date or contract roll with back to back child orders
  , InternalHasTargetTakeProfitPrice          = 0x00_00_04_00 //  8. Order can be linked as closing a previous entry order (used to calc pnl)    
  , InternalHasTargetStopLossPrice            = 0x00_00_08_00 //
  , InternalIsParentOrder                     = 0x00_00_10_00 //                    
  , InternalIsChildOrder                      = 0x00_00_20_00 //  
  , InternalHasLinkedClosingOrder             = 0x00_00_40_00 //  
  , InternalIsVirtualNonSubmittable           = 0x00_00_80_00 //  
  , IsSynthetic                               = 0x00_80_00_00 // Synthetics can be generated in Prod as part of estimating queue positions
  , SyntheticGeneratedFromSnapshot            = 0x00_81_00_00
  , SyntheticGeneratedFromImpliedDeltaUpdate  = 0x00_82_00_00
  , SyntheticGeneratedFromOrderCountAndVolume = 0x00_84_00_00
  , SyntheticGeneratedIsOrderAggregate        = 0x00_88_00_00
  , SyntheticGeneratedToMeetBookVolume        = 0x00_90_00_00
  , SyntheticForBacktestSimulation            = 0x00_A0_00_00
  , FromRouter                                = 0x01_00_00_00
  , FromAggregator                            = 0x02_00_00_00
    // backtest simulator
  , IsComparisonAccountOrder      = 0x08_00_00_00
  , FromSimulator                 = 0x80_00_00_00
  , IsSimulatorNoLimitsOrder      = 0x90_00_00_00
  , IsSimulatorSimLimitsOnlyOrder = 0xB0_00_00_00
  , IsSimulatorAllLimitsOrder     = 0xF0_00_00_00

  , AllFlagsMask = 0xFB_FF_FF_FF
}

public static class OrderFlagsExtensions
{
    public static bool IsExternalOrder(this OrderGenesisFlags genesisFlags) => (genesisFlags & OrderGenesisFlags.IsExternalOrder) > 0;

    public static bool HasExternalCounterPartyInfo
        (this OrderGenesisFlags genesisFlags) =>
        (genesisFlags & OrderGenesisFlags.HasExternalCounterPartyInfo) > 0;

    public static bool IsInternalOrder(this OrderGenesisFlags genesisFlags)      => (genesisFlags & OrderGenesisFlags.IsInternal) > 0;
    public static bool HasInternalOrderInfo(this OrderGenesisFlags genesisFlags) => (genesisFlags & OrderGenesisFlags.HasInternalOrderInfo) > 0;

    public static bool HasVolumeNotPartOfLiquidity
        (this OrderGenesisFlags genesisFlags) =>
        (genesisFlags & OrderGenesisFlags.VolumeNotPartOfLiquidity) > 0;

    public static bool HasSyntheticForBacktestSimulation
        (this OrderGenesisFlags genesisFlags) =>
        (genesisFlags & OrderGenesisFlags.SyntheticForBacktestSimulation) > 0;

    public static bool HasAllOf
        (this OrderGenesisFlags genesisFlags, OrderGenesisFlags checkAllFound) =>
        (genesisFlags & checkAllFound) == checkAllFound;


    public static bool IgnoringAreSame(this OrderGenesisFlags ignoreThese, OrderGenesisFlags lhs, OrderGenesisFlags rhs) =>
        (lhs & ~ignoreThese) == (rhs & ~ignoreThese);

    public static bool AnyButNone(this OrderGenesisFlags check) => (check & OrderGenesisFlags.AllFlagsMask) != OrderGenesisFlags.None;

    public static bool HasNoneOf(this OrderGenesisFlags genesisFlags, OrderGenesisFlags checkNonAreSet)  => (genesisFlags & checkNonAreSet) == 0;
    public static bool HasAnyOf(this OrderGenesisFlags genesisFlags, OrderGenesisFlags checkAnyAreFound) => (genesisFlags & checkAnyAreFound) > 0;
    public static bool IsExactly(this OrderGenesisFlags genesisFlags, OrderGenesisFlags checkAllFound)   => genesisFlags == checkAllFound;

    public static OrderGenesisFlags ToOrderGenesisFlags(this uint toConvert) => (OrderGenesisFlags)toConvert;
}
