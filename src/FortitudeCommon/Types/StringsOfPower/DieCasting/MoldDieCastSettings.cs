// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting;

public struct MoldDieCastSettings(SkipTypeParts skipTypeParts)
{
    public SkipTypeParts SkipTypeParts = skipTypeParts;
}

[Flags]
public enum SkipTypeParts : byte
{
    None      = 0x00
  , All       = 0xFF
  , TypeName  = 0x01
  , TypeStart = 0x02
  , TypeEnd   = 0x04
}

public static class IgnoreWriteFlagsExtensions
{
    public static bool HasTypeNameFlag(this SkipTypeParts flags) => (flags & SkipTypeParts.TypeName) > 0;

    public static bool HasTypeStartFlag(this SkipTypeParts flags) => (flags & SkipTypeParts.TypeStart) > 0;

    public static bool HasTypeEndFlag(this SkipTypeParts flags) => (flags & SkipTypeParts.TypeEnd) > 0;

    public static FieldContentHandling ToFormattingFlags(this SkipTypeParts toConvert)
    {
        var flags = FieldContentHandling.DefaultCallerTypeFlags;

        flags |= toConvert.HasTypeStartFlag() ? FieldContentHandling.SuppressOpening : FieldContentHandling.DefaultCallerTypeFlags;
        flags |= toConvert.HasTypeNameFlag() ? FieldContentHandling.LogSuppressTypeNames : FieldContentHandling.DefaultCallerTypeFlags;
        flags |= toConvert.HasTypeEndFlag() ? FieldContentHandling.SuppressOpening : FieldContentHandling.DefaultCallerTypeFlags;

        return flags;
    }
}