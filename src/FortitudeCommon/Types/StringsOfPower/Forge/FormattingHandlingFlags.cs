// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Types.StringsOfPower.Forge;

[Flags]
public enum FormattingHandlingFlags : ushort
{
    DefaultCallerType        = 0x_00_00
  , NoAutoAddCallerType      = 0x_00_01
  , EncodeBounds             = 0x_00_02
  , EncodeInnerContent       = 0x_00_04
  , EncodeAll                = 0x_00_06
  , EnsureFormattedDelimited = 0x_00_08
  , DisableAutoDelimiting    = 0x_00_10
  , AsStringContent          = 0x_00_20
  , SkipAllEncoding          = 0x_00_40
  , EncodeAsBase64           = 0x_00_80
  , AsciiEscapeEncoding      = 0x_01_00
  , JsonEncoding             = 0x_02_00
  , YamlEncoding             = 0x_04_00 // Not implemented just reserving
  , MlEncoding               = 0x_08_00 // Not implemented just reserving
  , EncodingMask             = 0x_0F_00
  , ReformatMultiLine        = 0x_10_00
  , UnsetEncodeBounds        = 0x_20_00
  , UnsetEncodeContent       = 0x_40_00
}

public static class FieldContentHandlingExtensions
{
    public const FormattingHandlingFlags None = 0;

    public static bool IsNone(this FormattingHandlingFlags flags)              => flags == FormattingHandlingFlags.DefaultCallerType;
    public static bool HasEncodeBoundsFlag(this FormattingHandlingFlags flags) => flags == FormattingHandlingFlags.EncodeBounds;

    public static bool HasNoAutoAddCallerTypeFlag(this FormattingHandlingFlags flags) =>
        (flags & FormattingHandlingFlags.NoAutoAddCallerType) > 0;

    public static bool HasDisableAutoDelimiting(this FormattingHandlingFlags flags) =>
        (flags & FormattingHandlingFlags.DisableAutoDelimiting) > 0;

    public static bool ShouldDelimit(this FormattingHandlingFlags flags) =>
        (flags & FormattingHandlingFlags.EnsureFormattedDelimited) > 0;
}
