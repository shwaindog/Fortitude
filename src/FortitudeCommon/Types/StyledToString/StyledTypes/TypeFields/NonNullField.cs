// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable.Strings;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFields;

public interface INonNullField<out T> : IRecyclableObject where T : StyledTypeBuilder
{
    T WithName
    (string fieldName, bool? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName<TNum>
    (string fieldName, TNum? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>;

    T WithName<TStruct>
    (string fieldName, TStruct? value
      , StructStyler<TStruct> structToString) where TStruct : struct;

    T WithName
    (string fieldName, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, string? value, int startIndex, int length);

    T WithName(string fieldName, IStyledToStringObject? value);

    T WithName
    (string fieldName, IFrozenString? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName
    (string fieldName, IStringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName
    (string fieldName, StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    [CallsObjectToString]
    T WithName
    (string fieldName, object? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);
}

public class NonNullField<TExt> : RecyclableObject, INonNullField<TExt>
    where TExt : StyledTypeBuilder
{
    private IStyleTypeBuilderComponentAccess<TExt> stb = null!;

    private IAlwaysIncludeField<TExt> aif = null!;

    public NonNullField<TExt> Initialize(IStyleTypeBuilderComponentAccess<TExt> styledComplexTypeBuilder
      , IAlwaysIncludeField<TExt> alwaysIncludeField)
    {
        stb = styledComplexTypeBuilder;
        aif = alwaysIncludeField;

        return this;
    }

    public TExt WithName (string fieldName, bool? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? aif.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName<TNum> (string fieldName, TNum? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value != null ? aif.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName<TStruct>(string fieldName, TStruct? value, StructStyler<TStruct> structToString)
        where TStruct : struct =>
        !Equals(value, null) ? aif.WithName(fieldName, value.Value, structToString) : stb.StyleTypeBuilder;

    public TExt WithName(string fieldName, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? aif.WithName(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WithName(string fieldName, string? value, int startIndex, int length) =>
        value != null ? aif.WithName(fieldName, value, startIndex, length) : stb.StyleTypeBuilder;

    public TExt WithName(string fieldName, IStyledToStringObject? value) => 
        value != null ? aif.WithName(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WithName(string fieldName, IFrozenString? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? aif.WithName(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WithName(string fieldName, IStringBuilder? value, string? formatString = null) =>
        value != null ? aif.WithName(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WithName(string fieldName, StringBuilder? value, string? formatString = null) => 
        value != null ? aif.WithName(fieldName, value) : stb.StyleTypeBuilder;


    [CallsObjectToString]
    public TExt WithName
    (string fieldName, object? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? aif.WithName(fieldName, value) : stb.StyleTypeBuilder;

    public override void StateReset()
    {
        stb = null!;
        aif = null!;
        base.StateReset();
    }
}
