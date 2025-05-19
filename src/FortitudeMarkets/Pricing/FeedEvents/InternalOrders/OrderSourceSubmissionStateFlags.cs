// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeMarkets.Pricing.FeedEvents.InternalOrders;

public enum OrderSourceSubmissionStateFlags : uint
{
    None                                  = 0x00_00_00_00
  , IsActive                              = 0x00_00_00_01
  , IsClosed                              = 0x00_00_00_01
  , HasTakeProfit                         = 0x00_00_00_08
  , HasTakeStopLoss                       = 0x00_00_00_10
  , HasActiveOpenPosition                 = 0x00_00_00_20
  , HasCreatedEntryPosition               = 0x00_00_00_40
  , HasCreatedEntryPriceOnMarket          = 0x00_00_00_80
  , HasCreatedExitTakeProfitPriceOnMarket = 0x00_00_01_00
  , HasCreatedExitStopLossPriceOnMarket   = 0x00_00_02_00
  , IsStillTrackingTakeProfitPrice        = 0x00_00_04_00
  , IsStillTrackingStopLossPrice          = 0x00_00_08_00
  , HasChildOrders                        = 0x00_00_10_00
  , WillCreateChildOrders                 = 0x00_00_20_00
  , SourceAllowsSelfClosing               = 0x00_00_40_00
}
