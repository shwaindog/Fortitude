// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Types.StringsOfPower.Forge;

[Flags]
public enum FormattingHandlingFlags : ushort
{
    DefaultCallerTypeFlags            = 0x_00_00
  , NoCallerTypeFlags                 = 0x_00_01
  , EncodeSuffixLast                  = 0x_00_02
  , EncodeSuffixPenultimate           = 0x_00_04
  , EncodeContent                     = 0x_00_08
  , EncodePrefixPreUltimate           = 0x_00_10
  , EncodePrefixFirst                 = 0x_00_20
  , EncodeEntireFormatted             = 0x_00_2E
  , EncodeAllButPrefixFirstSuffixLast = 0x_00_1C
  , EnsureFormattedDelimited          = 0x_00_40
  , DisableAutoDelimiting             = 0x_00_80
  , AsStringContent                   = 0x_01_00
  , SkipAllEncoding                   = 0x_02_00
  , EncodeAsBase64                    = 0x_04_00
  , AsciiEscapeEncoding               = 0x_08_00
  , JsonEncoding                      = 0x_10_00
  , YamlEncoding                      = 0x_20_00 // Not implemented just reserving
  , MlEncoding                        = 0x_40_00 // Not implemented just reserving
  , ReformatMultiLine                 = 0x_80_00
}

public static class FieldContentHandlingExtensions
{
    public const FormattingHandlingFlags None = 0; 
    
    public static bool IsNone(this FormattingHandlingFlags flags) => flags  == FormattingHandlingFlags.DefaultCallerTypeFlags; 
    
    public static bool HasDisableAddingAutoCallerTypeFlags(this FormattingHandlingFlags flags)   =>
        (flags & FormattingHandlingFlags.NoCallerTypeFlags) > 0;
    
    public static bool HasDisableAutoDelimiting(this FormattingHandlingFlags flags)    => 
        (flags & FormattingHandlingFlags.DisableAutoDelimiting) > 0;
    
    public static bool ShouldDelimit(this FormattingHandlingFlags flags)    => 
        (flags & FormattingHandlingFlags.EnsureFormattedDelimited) > 0;
    
} 