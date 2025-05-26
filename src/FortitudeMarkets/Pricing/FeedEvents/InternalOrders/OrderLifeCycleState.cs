// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeMarkets.Pricing.FeedEvents.InternalOrders;

[Flags]
public enum OrderLifeCycleState : uint
{
    None                    = 0x00_00_00_00
  , Rejected                = 0x00_00_00_01
  , IsTracked               = 0x00_00_00_02
  , SentToSource            = 0x00_00_00_04
  , ConfirmedReceived       = 0x00_00_00_08
  , SourceRejected          = 0x00_00_00_10
  , ConfirmedActiveOnMarket = 0x00_00_00_20
  , ReportedMatch           = 0x00_00_00_40
  , NotifiedPartialFill     = 0x00_00_00_80
  , NotifiedCompleteFill    = 0x00_00_01_00
  , SentCancelRequest       = 0x00_00_02_00
  , SentAmendRequest        = 0x00_00_04_00
  , AmendRequestRejected    = 0x00_00_08_00
  , CancelRequestRejected   = 0x00_00_10_00
  , ConfirmedAmended        = 0x00_00_20_00
  , ConfirmedCanceled       = 0x00_00_40_00
  , Dead                    = 0x00_00_80_00
  , HasGoneUnknown          = 0x00_01_00_00
  , DropCopyDetailsReceived = 0x00_02_00_00
  , DropCopyDetailsMismatch = 0x00_04_00_00

  , FromRouter     = 0x01_00_00_00
  , FromAggregator = 0x02_00_00_00
  , FromAdapter    = 0x04_00_00_00
  , FromSource     = 0x08_00_00_00
  , FromSimulator  = 0x10_00_00_00
}
