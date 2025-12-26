// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;

namespace FortitudeCommon.Types.StringsOfPower.Options;

[Flags]
public enum StringStyle : ushort
{
    Default         = 0x00
  , Locked          = 0x01
  , Compact         = 0x02
  , Pretty          = 0x04
  , Log             = 0x08
  , CompactLog      = 0x0A
  , PrettyLog       = 0x0C
  , AnyLog          = 0x0E
  , Json            = 0x10
  , CompactJson     = 0x12
  , PrettyJson      = 0x14
  , AnyJson         = 0x16
  , AnyLogOrJson    = 0x1E  
  , Yaml            = 0x20 // not implemented just reserving
  , CompactYaml     = 0x22 // "
  , PrettyYaml      = 0x24 // "
  , Markup          = 0x40 // "
  , CompactMarkup   = 0x42 // "
  , PrettyMarkup    = 0x44 // "
  , Diagraph        = 0x80 // "
  , CompactDiagraph = 0x82 // "
  , PrettyDiagraph  = 0x84 // "
    
  , Custom        = 0x01_00 // Application specific formatting
  , CompactCustom = 0x01_82 // "
  , PrettyCustom  = 0x01_04 // "
    
  , AllStyles       = 0xFE
}

public static class StringStyleExtensions
{ // ReSharper disable UnusedMember.Global
    public static bool IsDefault(this StringStyle style) => style.IsExactly(Default);
    public static bool IsLocked(this StringStyle style) => (style & Locked) > 0;
    public static bool IsNotLocked(this StringStyle style) => (style & Locked) == 0;

    public static bool IsJustLogCompact(this StringStyle style) => style.IsExactly(Log | Pretty);

    public static bool IsJustLogPretty(this StringStyle style) => style.IsExactly(Log | Pretty);

    public static bool IsJustJsonCompact(this StringStyle style) => style.IsExactly(Log | Compact);

    public static bool IsJustJsonPretty(this StringStyle style) => style.IsExactly(Json | Compact);

    public static bool AllowsUnstructured(this StringStyle style) => style.HasAnyOf(Log);

    public static bool IsLog(this StringStyle style) => (style & Log) > 0;

    public static bool IsNotLog(this StringStyle style) => (style & Log) == 0;

    public static bool IsLogNoJson(this StringStyle style) => (style & Log) > 0 && style.HasNoneOf(Json);

    public static bool IsNotJson(this StringStyle style) => style.HasNoneOf(Json);

    public static bool IsJson(this StringStyle style) => (style & Json) > 0;

    public static bool IsPretty(this StringStyle style) => (style & Pretty) > 0;

    public static bool IsNotPretty(this StringStyle style) => (style & Pretty) == 0;

    public static bool IsCompact(this StringStyle style) => (style & Compact) > 0;

    public static bool HasAllOf(this StringStyle flags, StringStyle checkAllFound)    => (flags & checkAllFound) == checkAllFound;
    public static bool HasNoneOf(this StringStyle flags, StringStyle checkNonAreSet)  => (flags & checkNonAreSet) == 0;
    public static bool HasAnyOf(this StringStyle flags, StringStyle checkAnyAreFound) => (flags & checkAnyAreFound) > 0;
    public static bool IsExactly(this StringStyle flags, StringStyle checkAllFound)   => flags == checkAllFound;
}
