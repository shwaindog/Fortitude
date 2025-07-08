// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Core;

[Flags]
public enum MalformedFormatHandling : uint
{
    None                              = 0x00_00_00_00
  , DebugThrowOnIncompleteParams      = 0x00_00_00_01
  , ReleaseThrowOnIncompleteParams    = 0x00_00_00_02
  , DebugLogInfoOnIncompleteParams    = 0x00_00_00_04
  , DebugLogWarnOnIncompleteParams    = 0x00_00_00_08
  , DebugLogErrorOnIncompleteParams   = 0x00_00_00_10
  , ReleaseLogInfoOnIncompleteParams  = 0x00_00_00_20
  , ReleaseLogWarnOnIncompleteParams  = 0x00_00_00_40
  , ReleaseLogErrorOnIncompleteParams = 0x00_00_00_80
  , DebugThrowOnExcessiveParams       = 0x00_00_01_00
  , ReleaseThrowOnExcessiveParams     = 0x00_00_02_00
  , DebugLogInfoOnExcessiveParams     = 0x00_00_04_00
  , DebugLogWarnOnExcessiveParams     = 0x00_00_08_00
  , DebugLogErrorOnExcessiveParams    = 0x00_00_10_00
  , ReleaseLogInfoOnExcessiveParams   = 0x00_00_20_00
  , ReleaseLogWarnOnExcessiveParams   = 0x00_00_40_00
  , ReleaseLogErrorOnExcessiveParams  = 0x00_00_80_00

  , DefaultMalformedHandling = 0x00_00_41_41
}

public static class MalformedFormatHandlingExtensions
{
    public static bool HasDebugThrowOnIncompleteParamsFlag(this MalformedFormatHandling flags) =>
        (flags & MalformedFormatHandling.DebugThrowOnIncompleteParams) > 0;

    public static bool HasReleaseThrowOnIncompleteParamsFlag(this MalformedFormatHandling flags) =>
        (flags & MalformedFormatHandling.ReleaseThrowOnIncompleteParams) > 0;

    public static bool HasDebugLogInfoOnIncompleteParamsFlag(this MalformedFormatHandling flags) =>
        (flags & MalformedFormatHandling.DebugLogInfoOnIncompleteParams) > 0;

    public static bool HasDebugLogWarnOnIncompleteParamsFlag(this MalformedFormatHandling flags) =>
        (flags & MalformedFormatHandling.DebugLogWarnOnIncompleteParams) > 0;

    public static bool HasDebugLogErrorOnIncompleteParamsFlag(this MalformedFormatHandling flags) =>
        (flags & MalformedFormatHandling.DebugLogErrorOnIncompleteParams) > 0;

    public static bool HasReleaseLogInfoOnIncompleteParamsFlag(this MalformedFormatHandling flags) =>
        (flags & MalformedFormatHandling.ReleaseLogInfoOnIncompleteParams) > 0;

    public static bool HasReleaseLogWarnOnIncompleteParamsFlag(this MalformedFormatHandling flags) =>
        (flags & MalformedFormatHandling.ReleaseLogWarnOnIncompleteParams) > 0;

    public static bool HasReleaseLogErrorOnIncompleteParamsFlag(this MalformedFormatHandling flags) =>
        (flags & MalformedFormatHandling.ReleaseLogErrorOnIncompleteParams) > 0;

    public static bool HasDebugThrowOnExcessiveParamsFlag(this MalformedFormatHandling flags) =>
        (flags & MalformedFormatHandling.DebugThrowOnExcessiveParams) > 0;

    public static bool HasReleaseThrowOnExcessiveParamsFlag(this MalformedFormatHandling flags) =>
        (flags & MalformedFormatHandling.ReleaseThrowOnExcessiveParams) > 0;

    public static MalformedFormatHandling Unset(this MalformedFormatHandling flags, MalformedFormatHandling toUnset) => flags & ~toUnset;

    public static bool HasAllOf(this MalformedFormatHandling flags, MalformedFormatHandling checkAllFound) =>
        (flags & checkAllFound) == checkAllFound;

    public static bool HasNoneOf(this MalformedFormatHandling flags, MalformedFormatHandling checkNonAreSet) => (flags & checkNonAreSet) == 0;

    public static bool HasAnyOf(this MalformedFormatHandling flags, MalformedFormatHandling checkAnyAreFound) => (flags & checkAnyAreFound) > 0;

    public static bool IsExactly(this MalformedFormatHandling flags, MalformedFormatHandling checkAllFound) => flags == checkAllFound;
}
