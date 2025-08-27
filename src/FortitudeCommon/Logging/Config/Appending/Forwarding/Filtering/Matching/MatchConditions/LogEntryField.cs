// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.MatchConditions;

[Flags]
public enum LogEntryField : byte
{
    Nothing     = 0x00
  , MessageBody = 0x01
  , LoggerName  = 0x02
  , SourceFile  = 0x04
  , MemberName  = 0x08
  , Any         = 0xFF
}