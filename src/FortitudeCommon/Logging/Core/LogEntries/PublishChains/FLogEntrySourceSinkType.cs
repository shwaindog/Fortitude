// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Core.LogEntries.PublishChains;

[Flags]
public enum FLogEntrySourceSinkType : ushort
{
    Unknown           = 0x00
  , Source            = 0x01
  , Sink              = 0x01
  , InterceptionPoint = 0x01
  , Forwarding        = 0x01
  , Forking           = 0x01
  , Monitoring        = 0x02
  , Modifying         = 0x04
  , Filtering         = 0x08
  , Matching          = 0x10
  , EventTriggering   = 0x20
  , StateActivated    = 0x40

  , CacheReleasing = 0x40
}
