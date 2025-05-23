// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeMarkets.Pricing.FeedEvents.LastTraded;

[Flags]
public enum LastTradedLifeCycleFlags : uint
{
    None                        = 0x00
  , Matched                     = 0x01  // Just a notification that a match has occured no quantity or counterparty details can be last looked rejected
  , Confirmed                   = 0x03  // Confirmed (accepted by market maker) with potential quantity
  , CounterPartyDetailsReceived = 0x04  // details of the counterparty received
  , Rejected                    = 0x09  // Match was last looked and rejected
  , DropCopyReceived            = 0x10  // internal trades can receive a secondary feed of trade details, amounts etc...
  , DropCopyMatched             = 0x20
  , DropCopyMismatched          = 0x40
  , DropCopyTimeout             = 0x80
  , Dead                        = 0x80 // expect no further updates
}
