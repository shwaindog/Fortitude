// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.Mutable.Strings;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFields;

public partial class SelectTypeField<TExt> where TExt : StyledTypeBuilder
{
    public TExt WhenNonNullOrDefaultAdd
    (string fieldName, bool value, bool defaultValue = false
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != defaultValue ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;
    
    public TExt WhenNonNullOrDefaultAdd
    (string fieldName, bool? value, bool? defaultValue = false
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null && value != defaultValue ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;
    
    public TExt WhenNonNullOrDefaultAdd<TFmtStruct>(string fieldName, TFmtStruct? value, TFmtStruct? defaultValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  where TFmtStruct : struct, ISpanFormattable => 
        value != null && !Equals(value, defaultValue) ? AlwaysAdd(fieldName, value, formatString) : stb.StyleTypeBuilder;
    
    public TExt WhenNonNullOrDefaultAdd<TFmtStruct>(string fieldName, TFmtStruct value, TFmtStruct defaultValue = default
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  where TFmtStruct : struct, ISpanFormattable => 
        !Equals(value, defaultValue) ? AlwaysAdd(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAdd<TStruct>(string fieldName, TStruct? value
      , StructStyler<TStruct> structToString, TStruct? defaultValue = null) where TStruct : struct =>
        value != null && !Equals(value, defaultValue) ? AlwaysAdd(fieldName, value, structToString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAdd<TStruct>(string fieldName, TStruct value
      , StructStyler<TStruct> structToString, TStruct defaultValue = default) where TStruct : struct =>
        !Equals(value, defaultValue) ? AlwaysAdd(fieldName, value, structToString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAdd(string fieldName, string? value, string? defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
       value != null && value != defaultValue ? AlwaysAdd(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAdd(string fieldName, char[]? value, string? defaultValue = "") => 
       value is { Length: > 0 } ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAdd(string fieldName, IStyledToStringObject? value, IStyledToStringObject? defaultValue = null) => 
        value != null && !Equals(value, defaultValue) ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAdd(string fieldName, ICharSequence? value, string? defaultValue = "") =>
        value != null && defaultValue != null && value.Equals(defaultValue) ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAdd(string fieldName, StringBuilder? value, string? defaultValue = "") =>
        value != null && defaultValue != null && value.Equals(defaultValue) ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAdd(string fieldName, StringBuilder? value, int startIndex, int length, string? defaultValue)
    {
        if (value == null) return stb.StyleTypeBuilder;
        if (defaultValue != null)
        {
            if (value.Equals(defaultValue))
            {
                return stb.StyleTypeBuilder;
            }
        }
        return AlwaysAdd(fieldName, value, startIndex, length);
    }

    public TExt WhenNonNullOrDefaultAdd(string fieldName, ICharSequence? value, int startIndex, int length, string? defaultValue)
    {
        if (value == null) return stb.StyleTypeBuilder;
        if (defaultValue != null)
        {
            if (value.Equals(defaultValue))
            {
                return stb.StyleTypeBuilder;
            }
        }
        return AlwaysAdd(fieldName, value, startIndex, length);
    }

    public TExt WhenNonNullOrDefaultAdd(string fieldName, char[]? value, int startIndex, int length, string? defaultValue)
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
        return AlwaysAdd(fieldName, value, startIndex, length);
    }

    public TExt WhenNonNullOrDefaultAdd(string fieldName, string? value, int startIndex, int length, string? defaultValue)
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
        return AlwaysAdd(fieldName, value, startIndex, length);
    }
    
    [CallsObjectToString]
    public TExt WhenNonNullOrDefaultAdd(string fieldName, object? value, object? defaultValue
      ,[StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        value != null && value.Equals(defaultValue) ? AlwaysAdd(fieldName, value, formatString) : stb.StyleTypeBuilder;
}
