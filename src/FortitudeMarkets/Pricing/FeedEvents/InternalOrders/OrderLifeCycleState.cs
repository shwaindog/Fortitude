// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeMarkets.Pricing.FeedEvents.InternalOrders;

[Flags]
public enum OrderLifeCycleState : uint
{
    None                        = 0x00
  , AdapterRejected             = 0x00_00_00_01
  , IsTracked                   = 0x00_00_00_02
  , AdapterSentToSource         = 0x00_00_00_04
  , SourceReceived              = 0x00_00_00_08
  , SourceRejected              = 0x00_00_00_10
  , SourceActiveOnMarket        = 0x00_00_00_20
  , SourceReportedMatch         = 0x00_00_00_40
  , SourcePartialFill           = 0x00_00_00_80
  , SourceCompleteFill          = 0x00_00_01_00
  , SourceNotifiedCancelRequest = 0x00_00_02_00
  , SourceNotifiedAmendRequest  = 0x00_00_04_00
  , SourceAmendRejected         = 0x00_00_08_00
  , SourceCancelRejected        = 0x00_00_10_00
  , SourceAmended               = 0x00_00_20_00
  , SourceDead                  = 0x00_00_40_00
  , AdapterDead                 = 0x00_00_80_00
  , AdapterHasGoneUnknown       = 0x00_01_00_00

  , IsVirtual     = 0x01_00_00_00 // Never submitted to source
  , FromAdapter   = 0x02_00_00_00
  , FromSimulator = 0x04_00_00_00
}
