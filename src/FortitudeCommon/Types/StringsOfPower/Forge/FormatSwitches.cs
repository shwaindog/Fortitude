// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using static FortitudeCommon.Types.StringsOfPower.Forge.FormatSwitches;

namespace FortitudeCommon.Types.StringsOfPower.Forge;

[Flags]
public enum FormatSwitches : uint
{
    DefaultCallerTypeFlags   = 0x00_00_00
  , NoAutoAddCallerType      = 0x00_00_01
  , EncodeBounds             = 0x00_00_02
  , EncodeInnerContent       = 0x00_00_04
  , EncodeAll                = 0x00_00_06
  , EnsureFormattedDelimited = 0x00_00_08
  , NoItemSeparator          = 0x00_00_10
  , UseAltItemSeparator      = 0x00_00_20
  , NoItemPadding            = 0x00_00_40
  , UseAltItemPadding        = 0x00_00_80
  , ToggleEncodeAsBase64     = 0x00_01_00
  , AsciiEscapeEncoding      = 0x00_02_00 // minimal backslash escaping for console control chars 0-32 and 127 - 159
  , JsamlEncoding            = 0x00_04_00 // Json + Yaml => Jsaml
  , AngleMlEncoding          = 0x00_08_00 // Not implemented just reserving
  , CustomEncoding           = 0x00_10_00 // Not implemented just reserving
  , EncodingMask             = 0x00_1F_00
  , NoEncoding               = 0x00_1F_00
  , DisableAutoDelimiting    = 0x00_20_00
  , AsStringContent          = 0x00_40_00
  , AsValueContent           = 0x00_80_00
  , AsCollection             = 0x01_00_00
  , ReformatMultiLine        = 0x02_00_00
  , NullBecomesEmpty         = 0x04_00_00
  , OnOneLine                = 0x08_00_00
}

public static class FormatSwitchesExtensions
{
    public const FormatSwitches None = 0;

    public static bool IsNone(this FormatSwitches flags) => flags == DefaultCallerTypeFlags;

    public static bool HasEncodeBoundsFlag(this FormatSwitches flags)   => (flags & EncodeBounds) > 0;
    
    public static bool HasEncodeInnerContent(this FormatSwitches flags) => (flags & EncodeInnerContent) > 0;

    public static bool HaEnsureFormattedDelimitedFlag(this FormatSwitches flags) => (flags & EnsureFormattedDelimited) > 0;

    public static bool HasNoAutoAddCallerTypeFlag(this FormatSwitches flags) => (flags & NoAutoAddCallerType) > 0;

    public static bool HasDisableAutoDelimiting(this FormatSwitches flags) => (flags & DisableAutoDelimiting) > 0;

    public static bool HasAsStringContentFlag(this FormatSwitches flags) => (flags & AsStringContent) > 0;

    public static bool HasAsValueContentFlag(this FormatSwitches flags) => (flags & AsValueContent) > 0;

    public static bool IsUnspecifiedContent(this FormatSwitches flags) => !(flags.HasAsStringContentFlag() || flags.HasAsValueContentFlag());

    public static bool ShouldDelimit(this FormatSwitches flags) => (flags & EnsureFormattedDelimited) > 0;

    public static bool TreatCharArrayAsString(this FormatSwitches flags) => flags.HasAsStringContentFlag() || flags.HasAsValueContentFlag();

    public static bool HasNoItemSeparatorFlag(this FormatSwitches flags) => (flags & NoItemSeparator) > 0;
    public static bool HasNoItemPaddingFlag(this FormatSwitches flags)   => (flags & NoItemPadding) > 0;
    public static bool ShouldAddItemSeparator(this FormatSwitches flags) => (flags & NoItemSeparator) == 0;
    public static bool UseMainItemSeparator(this FormatSwitches flags)   => (flags & UseAltItemSeparator) == 0;
    public static bool ShouldAddItemPadding(this FormatSwitches flags)   => (flags & NoItemPadding) == 0;
    public static bool UseMainItemPadding(this FormatSwitches flags)     => (flags & UseAltItemSeparator) == 0;
    
    public static bool HasAsCollectionFlag(this FormatSwitches flags)         => (flags & AsCollection) > 0;
    public static bool DoesNotHaveAsCollectionFlag(this FormatSwitches flags) => (flags & AsCollection) == 0;
}
