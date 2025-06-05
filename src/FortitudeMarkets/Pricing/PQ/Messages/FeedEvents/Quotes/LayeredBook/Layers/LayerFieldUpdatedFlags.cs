// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;

[Flags]
public enum LayerFieldUpdatedFlags : ushort
{
    None                            = 0x00
  , PriceUpdatedFlag                = 0x01
  , VolumeUpdatedFlag               = 0x02
  , SourceNameUpdatedFlag           = 0x04
  , ExecutableUpdatedFlag           = 0x08
  , SourceQuoteRefUpdatedFlag       = 0x10
  , ValueDateUpdatedFlag            = 0x20
  , OrdersCountUpdatedFlag          = 0x40
  , OrdersInternalVolumeUpdatedFlag = 0x80
  , LayerBehaviorUpdatedFlag        = 0x01_00
}
