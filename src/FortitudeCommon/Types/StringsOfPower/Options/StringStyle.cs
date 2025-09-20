// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;

namespace FortitudeCommon.Types.StringsOfPower.Options;

[Flags]
public enum StringStyle
{
    PlainText = 0x00
  , Compact   = 0x01
  , Pretty    = 0x02
  , Log       = 0x04
  , Default   = 0x04
  , Json      = 0x08
}

public static class StringStyleExtensions
{ // ReSharper disable UnusedMember.Global
    public static bool IsDefault(this StringStyle style) => style.IsExactly(Default);

    public static bool IsJustLogCompact(this StringStyle style) => style.IsExactly(Log | Pretty);

    public static bool IsJustLogPretty(this StringStyle style) => style.IsExactly(Log | Pretty);

    public static bool IsJustJsonCompact(this StringStyle style) => style.IsExactly(Log | Compact);

    public static bool IsJustJsonPretty(this StringStyle style) => style.IsExactly(Json | Compact);

    public static bool AllowsUnstructured(this StringStyle style)  => style == PlainText || style.HasAnyOf(Log);

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
