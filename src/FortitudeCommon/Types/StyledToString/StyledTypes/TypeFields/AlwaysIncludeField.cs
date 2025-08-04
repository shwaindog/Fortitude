// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable.Strings;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFields;

public interface IAlwaysIncludeField<out T> : IRecyclableObject where T : StyledTypeBuilder
{
    T WithName(string fieldName, bool value);

    T WithName(string fieldName, bool? value);

    T WithName<TNum>(string fieldName, TNum value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>;

    T WithName<TNum>(string fieldName, TNum? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>;

    T WithName<TStruct>(string fieldName, TStruct? value, StructStyler<TStruct> structToString) where TStruct : struct;

    T WithName(string fieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, string? value, int startIndex, int length);

    T WithName(string fieldName, IStyledToStringObject? value);

    T WithName(string fieldName, IFrozenString? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, IStringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    [CallsObjectToString]
    T WithName(string fieldName, object? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);
    
    T WithNameAndFormatting<TNum>(string fieldName, TNum value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) where TNum : struct, INumber<TNum>;

    T WithNameAndFormatting<TNum>(string fieldName, TNum? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) where TNum : struct, INumber<TNum>;

    T WithNameAndFormatting(string fieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString);

    T WithNameAndFormatting(string fieldName, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString);

    T WithNameAndFormatting(string fieldName, IFrozenString? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString);

    T WithNameAndFormatting(string fieldName, IStringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString);

    T WithNameAndFormatting(string fieldName, StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString);


    [CallsObjectToString]
    T WithNameAndFormatting(string fieldName, object? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString);
}

public class AlwaysIncludeField<TExt> : RecyclableObject, IAlwaysIncludeField<TExt> 
    where TExt : StyledTypeBuilder
{
    private IStyleTypeBuilderComponentAccess<TExt> stb = null!;

    public AlwaysIncludeField<TExt> Initialize(IStyleTypeBuilderComponentAccess<TExt> styledComplexTypeBuilder)
    {
        stb = styledComplexTypeBuilder;

        return this;
    }

    public IStringBuilder Sb => stb.Sb;

    public TExt WithName(string fieldName, bool value) => 
        stb.FieldNameJoin(fieldName).Append(value).AddGoToNext(stb);

    public TExt WithName(string fieldName, bool? value) => 
        stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext(stb);

    public TExt WithName<TNum>(string fieldName, TNum value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> => 
        formatString.IsNotNullOrEmpty() 
            ? WithNameAndFormatting(fieldName, value, formatString) 
            : stb.FieldNameJoin(fieldName).Append(value).AddGoToNext(stb);

    public TExt WithName<TNum>(string fieldName, TNum? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> => 
        formatString.IsNotNullOrEmpty() 
            ? WithNameAndFormatting(fieldName, value, formatString) 
            : stb.FieldNameJoin(fieldName).AppendOrNull(value).AddGoToNext(stb);

    public TExt WithName<TStruct>(string fieldName, TStruct? value
      , StructStyler<TStruct> structToString) where TStruct : struct =>
        stb.FieldNameJoin(fieldName, stb).AppendOrNull(value, structToString).AddGoToNext(stb);

    public TExt WithName(string fieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        formatString.IsNotNullOrEmpty() 
            ? stb.FieldNameJoin(fieldName).AppendFormatted(value, formatString).AddGoToNext(stb) 
            : stb.FieldNameJoin(fieldName).Append(value).AddGoToNext(stb);

    public TExt WithName(string fieldName, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        formatString.IsNotNullOrEmpty() 
            ? WithNameAndFormatting(fieldName, value, formatString) 
            : stb.FieldNameJoin(fieldName).Append(value ?? "null").AddGoToNext(stb);

    public TExt WithName(string fieldName, string? value, int startIndex, int length) =>
        stb.FieldNameJoin(fieldName).AddNullOrValue(value, startIndex, length, stb);

    public TExt WithName(string fieldName, IStyledToStringObject? value) => 
        stb.FieldNameJoin(fieldName).AddNullOrValue(value, stb);

    public TExt WithName(string fieldName, IFrozenString? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        formatString.IsNotNullOrEmpty() 
            ? WithNameAndFormatting(fieldName, value, formatString) 
            : stb.FieldNameJoin(fieldName).AddNullOrValue(value, stb);

    public TExt WithName(string fieldName, IStringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        formatString.IsNotNullOrEmpty() 
            ? WithNameAndFormatting(fieldName, value, formatString) 
            : stb.FieldNameJoin(fieldName).AddNullOrValue(value, stb);

    public TExt WithName(string fieldName, StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        formatString.IsNotNullOrEmpty() 
            ? WithNameAndFormatting(fieldName, value, formatString) 
            : stb.FieldNameJoin(fieldName).AddNullOrValue(value, stb);

    [CallsObjectToString]
    public TExt WithName(string fieldName, object? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) => 
        formatString.IsNotNullOrEmpty() 
            ? WithNameAndFormatting(fieldName, value, formatString) 
            : stb.FieldNameJoin(fieldName).AddNullOrValue(value, stb);

    public TExt WithNameAndFormatting<TNum>(string fieldName, TNum value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) where TNum : struct, INumber<TNum> => 
        stb.FieldNameJoin(fieldName).AppendFormatted(value, formatString).AddGoToNext(stb);

    public TExt WithNameAndFormatting<TNum>(string fieldName, TNum? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) where TNum : struct, INumber<TNum> => 
        stb.FieldNameJoin(fieldName).AppendFormattedOrNull(value, formatString).AddGoToNext(stb);

    public TExt WithNameAndFormatting(string fieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) =>
        stb.FieldNameJoin(fieldName).AppendFormatted(value, formatString).AddGoToNext(stb);

    public TExt WithNameAndFormatting(string fieldName, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) =>
        stb.FieldNameJoin(fieldName, stb).AppendFormattedOrNull(value, formatString).AddGoToNext(stb);

    public TExt WithNameAndFormatting(string fieldName, IFrozenString? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) =>
        stb.FieldNameJoin(fieldName, stb).AppendFormattedOrNull(value, formatString).AddGoToNext(stb);

    public TExt WithNameAndFormatting(string fieldName, IStringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) => 
        stb.FieldNameJoin(fieldName, stb).AppendFormattedOrNull(value, formatString).AddGoToNext(stb);

    public TExt WithNameAndFormatting(string fieldName, StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) => 
        stb.FieldNameJoin(fieldName, stb).AppendFormattedOrNull(value, formatString).AddGoToNext(stb);

    public TExt WithNameAndFormatting(string fieldName, object? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string formatString) => 
        stb.FieldNameJoin(fieldName, stb).AppendFormattedOrNull(value, formatString).AddGoToNext(stb);

    public override void StateReset()
    {
        stb = null!;
        base.StateReset();
    }
}

