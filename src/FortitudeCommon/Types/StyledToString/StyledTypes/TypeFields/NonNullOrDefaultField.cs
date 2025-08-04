// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable.Strings;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFields;

public interface INonNullOrDefaultField<out T> : IRecyclableObject where T : StyledTypeBuilder
{
    T WithName(string fieldName, bool value, bool defaultValue = false
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, bool? value, bool? defaultValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName<TNum>(string fieldName, TNum value, TNum defaultValue = default
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>;

    T WithName<TNum>(string fieldName, TNum? value, TNum? defaultValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>;

    T WithName<TStruct>(string fieldName, TStruct? value
      , StructStyler<TStruct> structToString, TStruct? defaultValue = null) where TStruct : struct;

    T WithName<TStruct>(string fieldName, TStruct value
      , StructStyler<TStruct> structToString, TStruct defaultValue = default) where TStruct : struct;

    T WithName(string fieldName, string? value, string? defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, string? value, int startIndex, int length, string? defaultValue = "");

    T WithName(string fieldName, IStyledToStringObject? value, IStyledToStringObject? defaultValue = null);

    T WithName(string fieldName, IFrozenString? value, string? defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, IStringBuilder? value, string? defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, StringBuilder? value, string? defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);


    [CallsObjectToString]
    T WithName(string fieldName, object? value, object? defaultValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);
}

public class NonNullOrDefaultField<TExt> : RecyclableObject, INonNullOrDefaultField<TExt> 
    where TExt : StyledTypeBuilder
{
    private IStyleTypeBuilderComponentAccess<TExt>    stb = null!;

    private IAlwaysIncludeField<TExt> aif = null!;

    public NonNullOrDefaultField<TExt> Initialize(IStyleTypeBuilderComponentAccess<TExt> styledComplexTypeBuilder, IAlwaysIncludeField<TExt> alwaysIncludeField)
    {
        stb = styledComplexTypeBuilder;
        aif = alwaysIncludeField;

        return this;
    }

    public TExt WithName
    (string fieldName, bool value, bool defaultValue = false
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != defaultValue ? aif.WithName(fieldName, value) : stb.StyleTypeBuilder;
    
    public TExt WithName
    (string fieldName, bool? value, bool? defaultValue = false
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null && value != defaultValue ? aif.WithName(fieldName, value) : stb.StyleTypeBuilder;
    
    public TExt WithName<TNum>(string fieldName, TNum? value, TNum? defaultValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  where TNum : struct, INumber<TNum> => 
        value != null && !Equals(value, defaultValue) ? aif.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;
    
    public TExt WithName<TNum>(string fieldName, TNum value, TNum defaultValue = default
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  where TNum : struct, INumber<TNum> => 
        !Equals(value, defaultValue) ? aif.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName<TStruct>(string fieldName, TStruct? value
      , StructStyler<TStruct> structToString, TStruct? defaultValue = null) where TStruct : struct =>
        value != null && !Equals(value, defaultValue) ? aif.WithName(fieldName, value, structToString) : stb.StyleTypeBuilder;

    public TExt WithName<TStruct>(string fieldName, TStruct value
      , StructStyler<TStruct> structToString, TStruct defaultValue = default) where TStruct : struct =>
        !Equals(value, defaultValue) ? aif.WithName(fieldName, value, structToString) : stb.StyleTypeBuilder;

    public TExt WithName(string fieldName, string? value, string? defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
       value != null && value != defaultValue ? aif.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName(string fieldName, IStyledToStringObject? value, IStyledToStringObject? defaultValue = null) => 
        value != null && !Equals(value, defaultValue) ? aif.WithName(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WithName(string fieldName, IFrozenString? value, string? defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null && defaultValue != null && value.Equals(defaultValue) ? aif.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName(string fieldName, IStringBuilder? value, string? defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null && defaultValue != null && value.Equals(defaultValue) ? aif.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName(string fieldName, StringBuilder? value, string? defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null && defaultValue != null && value.Equals(defaultValue) ? aif.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName(string fieldName, string? value, int startIndex, int length, string? defaultValue)
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
        return aif.WithName(fieldName, value, startIndex, length);
    }
    
    [CallsObjectToString]
    public TExt WithName(string fieldName, object? value, object? defaultValue = null
      ,[StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        value != null && value.Equals(defaultValue) ? aif.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public override void StateReset()
    {
        stb = null!;
        aif = null!;
        base.StateReset();
    }
}
