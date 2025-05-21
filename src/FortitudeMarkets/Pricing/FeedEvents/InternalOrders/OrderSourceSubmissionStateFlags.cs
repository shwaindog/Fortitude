// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeMarkets.Pricing.FeedEvents.InternalOrders;

public enum OrderSourceSubmissionStateFlags : uint
{
    None                                   = 0x00_00_00_00
  , IsAdapterActive                        = 0x00_00_00_01
  , IsStrategyActive                       = 0x00_00_00_02
  , IsSourceActive                         = 0x00_00_00_04
  , IsSourceDead                           = 0x00_00_00_08
  , IsAdapterDead                          = 0x00_00_00_10
  , IsStrategyDead                         = 0x00_00_00_20
  , DeadOnPartiallyFilled                  = 0x00_00_00_40
  , KillOnPartiallyFilledTimeout           = 0x00_00_00_80
  , DeadOnFullyFilled                      = 0x00_00_01_00
  , DeadWhenRemainingOpenPositionZero      = 0x00_00_02_00
  , DeadOnTakeProfitOrStopLoss             = 0x00_00_04_00
  , DeadOnChildrenDead                     = 0x00_00_08_00
  , AdapterEnforcedTakeProfitStopLoss      = 0x00_00_10_00
  , SourceEnforcedTakeProfitStopLoss       = 0x00_00_20_00
  , StrategyManagedTakeProfitStopLoss      = 0x00_00_40_00
  , IsSourceManagedPositionEntryOrder      = 0x00_00_80_00
  , IsSourceManagedPositionTakeProfitOrder = 0x00_01_00_00
  , IsSourceManagedPositionStopLossOrder   = 0x00_02_00_00
  , HasActiveOpenPosition                  = 0x00_04_00_00
  , HasCreatedEntryPosition                = 0x00_08_00_00
  , HasCreatedEntryPriceOnMarket           = 0x00_10_00_00
  , HasCreatedExitTakeProfitPriceOnMarket  = 0x00_20_00_00
  , HasCreatedExitStopLossPriceOnMarket    = 0x00_40_00_00
  , IsStillTrackingTakeProfitPrice         = 0x00_80_00_00
  , IsStillTrackingStopLossPrice           = 0x01_00_00_00
  , HasChildOrders                         = 0x02_00_00_00
  , WillCreateChildOrders                  = 0x04_00_00_00
  , SourceAllowsSelfClosing                = 0x08_00_00_00
}
