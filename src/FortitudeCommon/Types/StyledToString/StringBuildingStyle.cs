// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using static FortitudeCommon.Types.StyledToString.StringBuildingStyle;

namespace FortitudeCommon.Types.StyledToString;

[Flags]
public enum StringBuildingStyle
{
    PlainText = 0x00
  , Compact   = 0x01
  , Pretty    = 0x02
  , Log       = 0x04
  , Default   = 0x05
  , Json      = 0x08
}

public static class StringBuildingStyleExtensions
{ // ReSharper disable UnusedMember.Global
    public static bool IsDefault(this StringBuildingStyle style) => style.IsExactly(Default);

    public static bool IsJustLogCompact(this StringBuildingStyle style) => style.IsExactly(Log | Pretty);

    public static bool IsJustLogPretty(this StringBuildingStyle style) => style.IsExactly(Log | Pretty);

    public static bool IsJustJsonCompact(this StringBuildingStyle style) => style.IsExactly(Log | Compact);

    public static bool IsJustJsonPretty(this StringBuildingStyle style) => style.IsExactly(Json | Compact);

    public static bool AllowsUnstructured(this StringBuildingStyle style)  => style == PlainText || style.HasAnyOf(Log);

    public static bool IsLog(this StringBuildingStyle style) => (style & Log) > 0;

    public static bool IsLogNoJson(this StringBuildingStyle style) => (style & Log) > 0 && style.HasNoneOf(Json);

    public static bool IsNotJson(this StringBuildingStyle style) => style.HasNoneOf(Json);

    public static bool IsJson(this StringBuildingStyle style) => (style & Json) > 0;

    public static bool IsPretty(this StringBuildingStyle style) => (style & Pretty) > 0;

    public static bool IsCompact(this StringBuildingStyle style) => (style & Compact) > 0;

    public static bool HasAllOf(this StringBuildingStyle flags, StringBuildingStyle checkAllFound)    => (flags & checkAllFound) == checkAllFound;
    public static bool HasNoneOf(this StringBuildingStyle flags, StringBuildingStyle checkNonAreSet)  => (flags & checkNonAreSet) == 0;
    public static bool HasAnyOf(this StringBuildingStyle flags, StringBuildingStyle checkAnyAreFound) => (flags & checkAnyAreFound) > 0;
    public static bool IsExactly(this StringBuildingStyle flags, StringBuildingStyle checkAllFound)   => flags == checkAllFound;
}
