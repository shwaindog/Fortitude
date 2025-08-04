// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;
using FortitudeCommon.Types.Mutable.Strings;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFields;

public partial class SelectTypeField<TExt> where TExt : StyledTypeBuilder
{
    public TExt AddWhenNonNullOrDefault
    (string fieldName, bool value, bool defaultValue = false
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != defaultValue ? AddAlways(fieldName, value) : stb.StyleTypeBuilder;
    
    public TExt AddWhenNonNullOrDefault
    (string fieldName, bool? value, bool? defaultValue = false
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null && value != defaultValue ? AddAlways(fieldName, value) : stb.StyleTypeBuilder;
    
    public TExt AddWhenNonNullOrDefault<TNum>(string fieldName, TNum? value, TNum? defaultValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  where TNum : struct, INumber<TNum> => 
        value != null && !Equals(value, defaultValue) ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;
    
    public TExt AddWhenNonNullOrDefault<TNum>(string fieldName, TNum value, TNum defaultValue = default
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  where TNum : struct, INumber<TNum> => 
        !Equals(value, defaultValue) ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullOrDefault<TStruct>(string fieldName, TStruct? value
      , StructStyler<TStruct> structToString, TStruct? defaultValue = null) where TStruct : struct =>
        value != null && !Equals(value, defaultValue) ? AddAlways(fieldName, value, structToString) : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullOrDefault<TStruct>(string fieldName, TStruct value
      , StructStyler<TStruct> structToString, TStruct defaultValue = default) where TStruct : struct =>
        !Equals(value, defaultValue) ? AddAlways(fieldName, value, structToString) : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullOrDefault(string fieldName, string? value, string? defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
       value != null && value != defaultValue ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullOrDefault(string fieldName, string? value, int startIndex, int length = 0) => 
       value != null && length > 0 ? AddAlways(fieldName, value, startIndex, length) : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullOrDefault(string fieldName, char[]? value, int startIndex = 0, int length = 0) => 
       value != null && length > 0 ? AddAlways(fieldName, value, startIndex, length) : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullOrDefault(string fieldName, IStyledToStringObject? value, IStyledToStringObject? defaultValue = null) => 
        value != null && !Equals(value, defaultValue) ? AddAlways(fieldName, value) : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullOrDefault(string fieldName, IFrozenString? value, string? defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null && defaultValue != null && value.Equals(defaultValue) ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullOrDefault(string fieldName, IStringBuilder? value, string? defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null && defaultValue != null && value.Equals(defaultValue) ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullOrDefault(string fieldName, StringBuilder? value, string? defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null && defaultValue != null && value.Equals(defaultValue) ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullOrDefault(string fieldName, string? value, int startIndex, int length, string? defaultValue)
    {
        if (value == null) return stb.StyleTypeBuilder;
        if (defaultValue != null)
        {
            var valueSpan   = value.AsSpan();
            var defaultSpan = defaultValue.AsSpan();
            if (valueSpan == defaultSpan)
            {
                return stb.StyleTypeBuilder;
            }
        }
        return AddAlways(fieldName, value, startIndex, length);
    }
    
    [CallsObjectToString]
    public TExt AddWhenNonNullOrDefault(string fieldName, object? value, object? defaultValue
      ,[StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        value != null && value.Equals(defaultValue) ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;
}
