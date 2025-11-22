// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using static FortitudeCommon.Types.StringsOfPower.Forge.FormattingHandlingFlags;

namespace FortitudeCommon.Types.StringsOfPower.Forge;

[Flags]
public enum FormattingHandlingFlags : ushort
{
    DefaultCallerTypeFlags   = 0x_00_00
  , NoAutoAddCallerType      = 0x_00_01
  , EncodeBounds             = 0x_00_02
  , EncodeInnerContent       = 0x_00_04
  , EncodeAll                = 0x_00_06
  , EnsureFormattedDelimited = 0x_00_08
  , DisableAutoDelimiting    = 0x_00_10
  , AsStringContent          = 0x_00_20
  , AsValueContent           = 0x_00_40
  , ReformatMultiLine        = 0x_00_80
  , NoItemSeparator          = 0x_01_00
  , UseAltItemSeparator      = 0x_02_00
  , NoItemPadding            = 0x_04_00
  , UseAltItemPadding        = 0x_08_00
  , ToggleEncodeAsBase64     = 0x_10_00
  , AsciiEscapeEncoding      = 0x_20_00 // minimal backslash escaping for console control chars 0-32 and 127 - 159
  , JsamlEncoding            = 0x_40_00 // Json + Yaml => Jsaml
  , MlEncoding               = 0x_80_00 // Not implemented just reserving
  , EncodingMask             = 0x_F0_00
}

public static class FieldContentHandlingExtensions
{
    public const FormattingHandlingFlags None = 0;

    public static bool IsNone(this FormattingHandlingFlags flags)              => flags == DefaultCallerTypeFlags;
    
    public static bool HasEncodeBoundsFlag(this FormattingHandlingFlags flags) => (flags & EncodeBounds) > 0;
    
    public static bool HaEnsureFormattedDelimitedFlag(this FormattingHandlingFlags flags) => 
        (flags & EnsureFormattedDelimited) > 0;

    public static bool HasNoAutoAddCallerTypeFlag(this FormattingHandlingFlags flags) =>
        (flags & NoAutoAddCallerType) > 0;

    public static bool HasDisableAutoDelimiting(this FormattingHandlingFlags flags) =>
        (flags & DisableAutoDelimiting) > 0;

    public static bool HasAsStringContentFlag(this FormattingHandlingFlags flags) =>
        (flags & AsStringContent) > 0;

    public static bool HasAsValueContentFlag(this FormattingHandlingFlags flags) =>
        (flags & AsValueContent) > 0;
    
    public static bool IsUnspecifiedContent(this FormattingHandlingFlags flags) =>
        !(flags.HasAsStringContentFlag() || flags.HasAsValueContentFlag());

    public static bool ShouldDelimit(this FormattingHandlingFlags flags) =>
        (flags & EnsureFormattedDelimited) > 0;
    
    public static bool TreatCharArrayAsString(this FormattingHandlingFlags flags) =>
        flags.HasAsStringContentFlag() || flags.HasAsValueContentFlag();
    
    public static bool HasNoItemSeparatorFlag(this FormattingHandlingFlags flags) => (flags & NoItemSeparator) > 0; 
    public static bool HasNoItemPaddingFlag(this FormattingHandlingFlags flags)   => (flags & NoItemPadding) > 0; 
    public static bool ShouldAddItemSeparator(this FormattingHandlingFlags flags) => (flags & NoItemSeparator) == 0; 
    public static bool UseMainItemSeparator(this FormattingHandlingFlags flags)   => (flags & UseAltItemSeparator) == 0; 
    public static bool ShouldAddItemPadding(this FormattingHandlingFlags flags)   => (flags & NoItemPadding) == 0; 
    public static bool UseMainItemPadding(this FormattingHandlingFlags flags) => (flags & UseAltItemSeparator) == 0; 
}
