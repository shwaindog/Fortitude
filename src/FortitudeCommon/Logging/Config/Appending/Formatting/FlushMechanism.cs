// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Config.Appending.Formatting;

[Flags]
public enum FlushMechanism
{
    Default = 0x00
  , Writer  = 0x01
  , Async   = 0x02
  , Both    = 0x04
}
