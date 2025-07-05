// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeMarkets.Trading.Accounts.Config;

public enum PositionsDirectionFlags
{
    Default  = 0x00
  , None     = 0x01
  , Reducing = 0x02
  , Opening  = 0x04
  , Both     = 0x06
}
