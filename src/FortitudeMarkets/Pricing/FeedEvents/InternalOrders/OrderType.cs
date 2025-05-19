// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeMarkets.Pricing.FeedEvents.InternalOrders;

[Flags]
public enum OrderType : ushort
{
    None
  , Limit           = 0x_00_01
  , Stop            = 0x_00_02
  , Market          = 0x_10_04
  , PassiveLimit    = 0x_20_01
  , PassiveStop     = 0x_20_02
  , AggressiveLimit = 0x_10_01
  , Aggressive      = 0x_10_08
  , Passive         = 0x_20_09

}
