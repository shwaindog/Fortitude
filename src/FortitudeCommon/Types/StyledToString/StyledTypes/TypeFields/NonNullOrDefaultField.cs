// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable.Strings;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFields;

public partial class SelectTypeField<TExt> where TExt : StyledTypeBuilder
{
    private static readonly ConcurrentDictionary<Type, object?> EmptyConstructInstance = new();
    
    public TExt WhenNonNullOrDefaultAdd
    (string fieldName, bool? value, bool? defaultValue = false) =>
        !stb.SkipFields && value != null && value != defaultValue ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;
    
    public TExt WhenNonNullOrDefaultAdd<TFmt>(string fieldName, TFmt? value, TFmt? defaultValue = default
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  where TFmt : ISpanFormattable => 
        !stb.SkipFields && value != null && !Equals(value, defaultValue) ? AlwaysAdd(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAdd<TToStyle, TStylerType>(string fieldName, TToStyle? value
      , CustomTypeStyler<TStylerType> customTypeStyler, TToStyle? defaultValue = default) where TToStyle : TStylerType =>
        !stb.SkipFields && value != null && !Equals(value, defaultValue) ? AlwaysAdd(fieldName, value, customTypeStyler) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAdd(string fieldName, string? value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        !stb.SkipFields && value != null && value != defaultValue ? AlwaysAdd(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAdd(string fieldName, char[]? value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        !stb.SkipFields && value is { Length: > 0 } && !value.IsEquivalentTo(defaultValue) ? AlwaysAdd(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAddStyled<TStyled>(string fieldName, TStyled? value, TStyled? defaultValue = default) 
        where TStyled : IStyledToStringObject => 
        !stb.SkipFields && value != null && !Equals(value, defaultValue) ? AlwaysAdd(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAdd(string fieldName, ICharSequence? value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null && !value.IsEquivalentTo(defaultValue) ? AlwaysAdd(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAdd(string fieldName, StringBuilder? value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null && !value.IsEquivalentTo(defaultValue) ? AlwaysAdd(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullOrDefaultAdd(string fieldName, StringBuilder? value, int startIndex, int length = int.MaxValue
      , string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields || value == null) return stb.StyleTypeBuilder;
        if (value.IsEquivalentTo(defaultValue, startIndex, length))
        {
            return stb.StyleTypeBuilder;
        }
        return AlwaysAdd(fieldName, value, startIndex, length, formatString);
    }

    public TExt WhenNonNullOrDefaultAdd(string fieldName, ICharSequence? value, int startIndex, int length = int.MaxValue
      , string? formatString = null, string defaultValue = "")
    {
        if (stb.SkipFields || value == null) return stb.StyleTypeBuilder;
        if (value.IsEquivalentTo(defaultValue, startIndex, length))
        {
            return stb.StyleTypeBuilder;
        }
        return AlwaysAdd(fieldName, value, startIndex, length, formatString);
    }

    public TExt WhenNonNullOrDefaultAdd(string fieldName, char[]? value, int startIndex, int length = int.MaxValue
      , string? formatString = null, string defaultValue = "")
    {
        if (stb.SkipFields || value == null) return stb.StyleTypeBuilder;
        if (value.IsEquivalentTo(defaultValue, startIndex, length))
        {
            return stb.StyleTypeBuilder;
        }
        return AlwaysAdd(fieldName, value, startIndex, length, formatString);
    }

    public TExt WhenNonNullOrDefaultAdd(string fieldName, string? value, int startIndex, int length = int.MaxValue
      , string? formatString = null, string defaultValue = "")
    {
        if (stb.SkipFields || value == null) return stb.StyleTypeBuilder;
        if (value.IsEquivalentTo(defaultValue, startIndex, length))
        {
            return stb.StyleTypeBuilder;
        }
        return AlwaysAdd(fieldName, value, startIndex, length, formatString);
    }
    
    public TExt WhenNonNullOrDefaultAddMatch<T>(string fieldName, T? value, T? defaultValue = default
      ,[StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        !stb.SkipFields && value != null && value.Equals(defaultValue) ? AlwaysAddMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;
    
    [CallsObjectToString]
    public TExt WhenNonNullOrDefaultAddObject(string fieldName, object? value, object? defaultValue = null
      ,[StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        !stb.SkipFields && value != null && value.Equals(defaultValue) ? AlwaysAddObject(fieldName, value, formatString) : stb.StyleTypeBuilder;
}
