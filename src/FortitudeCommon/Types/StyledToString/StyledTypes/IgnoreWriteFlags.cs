// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Types.StyledToString.StyledTypes;

[Flags]
public enum IgnoreWriteFlags : byte
{
    None      = 0x00
  , All       = 0xFF
  , TypeName  = 0x01
  , TypeStart = 0x02
  , TypeEnd   = 0x04
}

public static class IgnoreWriteFlagsExtensions
{
    public static bool HasTypeNameFlag(this IgnoreWriteFlags flags) => (flags & IgnoreWriteFlags.TypeName) > 0;

    public static bool HasTypeStartFlag(this IgnoreWriteFlags flags) => (flags & IgnoreWriteFlags.TypeStart) > 0;

    public static bool HasTypeEndFlag(this IgnoreWriteFlags flags) => (flags & IgnoreWriteFlags.TypeEnd) > 0;
}
