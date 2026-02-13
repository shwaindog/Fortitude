// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeMoldFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting;

[Flags]
public enum TypeMoldFlags : uint
{
    None                          = 0x00_00_00
  , IsEmptyFlag                   = 0x00_00_01
  , IsCompleteFlag                = 0x00_00_02
  , WroteTypeNameFlag             = 0x00_00_04
  , WroteTypeOpenFlag             = 0x00_00_10
  , WroteTypeCloseFlag            = 0x00_00_20
  , SuppressedTypeOpenFlag        = 0x00_00_40
  , SuppressedTypeCloseFlag       = 0x00_00_80
  , WroteCollectionOpenFlag       = 0x00_01_00
  , WroteCollectionCloseFlag      = 0x00_02_00
  , SuppressedCollectionOpenFlag  = 0x00_04_00
  , SuppressedCollectionCloseFlag = 0x00_08_00
  , WroteCollectionNameFlag       = 0x00_10_00
  , SkipBodyFlag                  = 0x00_20_00
  , SkipFieldsFlag                = 0x00_40_00
  , WroteRefIdFlag                = 0x00_80_00
  , WroteIdFlag                   = 0x01_00_00
  , WasDepthClippedFlag           = 0x02_00_00
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
    public static bool HasWroteTypeNameFlag(this TypeMoldFlags flags)             => (flags & WroteTypeNameFlag) > 0;
    public static bool HasWroteRefIdFlag(this TypeMoldFlags flags)                => (flags & WroteRefIdFlag) > 0;
    public static bool HasWroteTypeOpenFlag(this TypeMoldFlags flags)             => (flags & WroteTypeOpenFlag) > 0;
    public static bool HasWroteTypeCloseFlag(this TypeMoldFlags flags)            => (flags & WroteTypeCloseFlag) > 0;
    
    public static bool HasSuppressedTypeOpenFlag(this TypeMoldFlags flags)  => (flags & SuppressedTypeOpenFlag) > 0;
    
    public static bool HasSuppressedTypeCloseFlag(this TypeMoldFlags flags) => (flags & SuppressedTypeCloseFlag) > 0;
    
    public static bool HasWroteCollectionOpenFlag(this TypeMoldFlags flags)   => (flags & WroteCollectionOpenFlag) > 0;
    
    public static bool HasWroteCollectionCloseFlag(this TypeMoldFlags flags) => (flags & WroteCollectionCloseFlag) > 0;
    
    public static bool HasSuppressedCollectionOpenFlag(this TypeMoldFlags flags)  => (flags & SuppressedCollectionOpenFlag) > 0;
    
    public static bool HasSuppressedCollectionCloseFlag(this TypeMoldFlags flags) => (flags & SuppressedCollectionCloseFlag) > 0;
    
    public static bool HasWroteCollectionNameFlag(this TypeMoldFlags flags)        => (flags & WroteCollectionNameFlag) > 0;
    public static bool HasWasDepthClippedFlag(this TypeMoldFlags flags)      => (flags & WasDepthClippedFlag) > 0;

    public static TypeMoldFlags Unset(this TypeMoldFlags flags, TypeMoldFlags toUnset) => flags & ~toUnset;

    public static TypeMoldFlags SetTo(this TypeMoldFlags flags, TypeMoldFlags toToggle, bool setOrUnsetValue) =>
        setOrUnsetValue ? flags | toToggle : flags & ~toToggle;


    public static bool HasAllOf(this TypeMoldFlags flags, TypeMoldFlags checkAllFound)    => (flags & checkAllFound) == checkAllFound;
    public static bool HasNoneOf(this TypeMoldFlags flags, TypeMoldFlags checkNonAreSet)  => (flags & checkNonAreSet) == 0;
    public static bool HasAnyOf(this TypeMoldFlags flags, TypeMoldFlags checkAnyAreFound) => (flags & checkAnyAreFound) > 0;
    public static bool IsExactly(this TypeMoldFlags flags, TypeMoldFlags checkAllFound)   => flags == checkAllFound;
}
