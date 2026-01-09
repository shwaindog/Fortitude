// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Types.StringsOfPower.Forge.Crucible;

public enum FormatStyle : byte
{
    None = 0
  , Json = 1
  , Yaml = 2
  , AngleMl = 3  
}

public static class FormatStyleExtensions
{ // ReSharper disable UnusedMember.Global

    public static bool IsNone(this FormatStyle style) => style == FormatStyle.None;

    public static bool IsNotNone(this FormatStyle style) => style != FormatStyle.None;

    public static bool IsJson(this FormatStyle style) => style == FormatStyle.Json;
    
    public static bool IsNotJson(this FormatStyle style) => style != FormatStyle.Json;

    public static bool IsYaml(this FormatStyle style) => style == FormatStyle.Yaml;
    
    public static bool IsNotYaml(this FormatStyle style) => style != FormatStyle.Yaml;

    public static bool IsAngleMl(this FormatStyle style) => style == FormatStyle.AngleMl;
    
    public static bool IsNotAngleMl(this FormatStyle style) => style != FormatStyle.AngleMl;
}