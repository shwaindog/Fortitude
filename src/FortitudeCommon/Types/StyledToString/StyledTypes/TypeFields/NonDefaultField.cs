// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable.Strings;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFields;

public interface INonDefaultField<out T> : IRecyclableObject where T : StyledTypeBuilder
{
    T WithName
    (string fieldName, bool value, bool defaultValue = false
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName<TNum>
    (string fieldName, TNum value, TNum defaultValue = default
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>;

    T WithName<TStruct>
    (string fieldName, TStruct value
      , StructStyler<TStruct> structToString, TStruct defaultValue = default) where TStruct : struct;

    T WithName
    (string fieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName
    (string fieldName, string value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, IStyledToStringObject? value, IStyledToStringObject? defaultValue = null);

    T WithName
    (string fieldName, IFrozenString value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName
    (string fieldName, IStringBuilder value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName
    (string fieldName, StringBuilder value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    [CallsObjectToString]
    T WithName
    (string fieldName, object value, object defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);
}

public class NonDefaultField<TExt> : RecyclableObject, INonDefaultField<TExt>
    where TExt : StyledTypeBuilder
{
    private IStyleTypeBuilderComponentAccess<TExt> stb = null!;

    private IAlwaysIncludeField<TExt> aif = null!;

    public NonDefaultField<TExt> Initialize(IStyleTypeBuilderComponentAccess<TExt> styledComplexTypeBuilder, IAlwaysIncludeField<TExt> alwaysIncludeField)
    {
        stb = styledComplexTypeBuilder;
        aif = alwaysIncludeField;

        return this;
    }

    public TExt WithName
    (string fieldName, bool value, bool defaultValue = false
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != defaultValue ? aif.WithName(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WithName<TNum>
    (string fieldName, TNum value, TNum defaultValue = default
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        !Equals(value, defaultValue) ? aif.WithName(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WithName<TStruct>
    (string fieldName, TStruct value
      , StructStyler<TStruct> structToString, TStruct defaultValue = default) where TStruct : struct =>
        !Equals(value, defaultValue) ? aif.WithName(fieldName, value, structToString) : stb.StyleTypeBuilder;

    public TExt WithName
    (string fieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value.Length > 0 ? aif.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName
    (string fieldName, string value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != defaultValue ? aif.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName(string fieldName, IStyledToStringObject? value, IStyledToStringObject? defaultValue = null) =>
        Equals(value, defaultValue) ? aif.WithName(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WithName
    (string fieldName, IFrozenString value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !value.Equals(defaultValue) ? aif.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName
    (string fieldName, IStringBuilder value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !value.Equals(defaultValue) ? aif.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName
    (string fieldName, StringBuilder value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !value.Equals(defaultValue) ? aif.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    [CallsObjectToString]
    public TExt WithName(string fieldName, object? value, object? defaultValue = null, string? formatString = null) =>
        !(value?.Equals(defaultValue) ?? defaultValue?.Equals(value) ?? true) ? aif.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public override void StateReset()
    {
        stb = null!;
        aif = null!;
        base.StateReset();
    }
}
