// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeMoldFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting;

[Flags]
public enum TypeMoldFlags : uint
{
    None                     = 0x00_00_00
  , IsEmptyFlag              = 0x00_00_01
  , IsCompleteFlag           = 0x00_00_02
  , WroteOuterTypeNameFlag   = 0x00_00_04
  , WroteOuterTypeOpenFlag   = 0x00_00_08
  , WroteOuterTypeCloseFlag  = 0x00_00_10
  , InnerSameAsOuterTypeFlag = 0x00_00_20
  , WroteInnerTypeOpenFlag   = 0x00_00_40
  , WroteInnerTypeCloseFlag  = 0x00_00_80
  , WroteCInnerTypeNameFlag  = 0x00_01_00
  , SkipBodyFlag             = 0x00_02_00
  , SkipFieldsFlag           = 0x00_04_00
  , WroteRefIdFlag           = 0x00_08_00
  , WroteIdFlag              = 0x00_10_00
  , WasDepthClippedFlag      = 0x00_20_00
}

public static class TypeMoldFlagsExtensions
{
    public static bool HasIsEmptyFlag(this TypeMoldFlags flags)    => (flags & IsEmptyFlag) > 0;
    public static bool HasIsCompleteFlag(this TypeMoldFlags flags) => (flags & IsCompleteFlag) > 0;
    public static bool HasSkipBodyFlag(this TypeMoldFlags flags)   => (flags & SkipBodyFlag) > 0;
    public static bool HasSkipFieldsFlag(this TypeMoldFlags flags) => (flags & SkipFieldsFlag) > 0;

    // public static bool HasWriteAsAttributeFlag(this TypeMoldFlags flags) => (flags & WriteAsAttributeFlag) > 0;
    // public static bool HasWriteAsContentFlag(this TypeMoldFlags flags)   => (flags & WriteAsContentFlag) > 0;
    // public static bool HasWriteAsComplexFlag(this TypeMoldFlags flags)   => (flags & WriteAsComplexFlag) > 0;
    public static bool HasWroteOuterTypeNameFlag(this TypeMoldFlags flags)   => (flags & WroteOuterTypeNameFlag) > 0;
    public static bool HasWroteRefIdFlag(this TypeMoldFlags flags)           => (flags & WroteRefIdFlag) > 0;
    public static bool HasWroteOuterTypeOpenFlag(this TypeMoldFlags flags)   => (flags & WroteOuterTypeOpenFlag) > 0;
    public static bool HasWroteOuterTypeCloseFlag(this TypeMoldFlags flags)  => (flags & WroteOuterTypeCloseFlag) > 0;
    public static bool HasInnerSameAsOuterTypeFlag(this TypeMoldFlags flags) => (flags & InnerSameAsOuterTypeFlag) > 0;

    public static bool HasWroteInnerTypeOpenFlag(this TypeMoldFlags flags) => (flags & WroteInnerTypeOpenFlag) > 0;

    public static bool HasWroteInnerTypeCloseFlag(this TypeMoldFlags flags) => (flags & WroteInnerTypeCloseFlag) > 0;

    public static bool HasWroteCInnerTypeNameFlag(this TypeMoldFlags flags) => (flags & WroteCInnerTypeNameFlag) > 0;
    public static bool HasWasDepthClippedFlag(this TypeMoldFlags flags)     => (flags & WasDepthClippedFlag) > 0;

    public static TypeMoldFlags Unset(this TypeMoldFlags flags, TypeMoldFlags toUnset) => flags & ~toUnset;

    public static TypeMoldFlags SetTo(this TypeMoldFlags flags, TypeMoldFlags toToggle, bool setOrUnsetValue) =>
        setOrUnsetValue ? flags | toToggle : flags & ~toToggle;


    public static bool HasAllOf(this TypeMoldFlags flags, TypeMoldFlags checkAllFound)    => (flags & checkAllFound) == checkAllFound;
    public static bool HasNoneOf(this TypeMoldFlags flags, TypeMoldFlags checkNonAreSet)  => (flags & checkNonAreSet) == 0;
    public static bool HasAnyOf(this TypeMoldFlags flags, TypeMoldFlags checkAnyAreFound) => (flags & checkAnyAreFound) > 0;
    public static bool IsExactly(this TypeMoldFlags flags, TypeMoldFlags checkAllFound)   => flags == checkAllFound;
}
