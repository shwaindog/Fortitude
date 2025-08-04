// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable.Strings;
#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldCollection;

public partial class SelectTypeCollectionField<TExt> where TExt : StyledTypeBuilder
{
    public TExt AddWhenPopulated(string fieldName, bool[]? value) =>
        value?.Any() ?? false ? AddAlways(fieldName, value) : stb.StyleTypeBuilder;

    public TExt AddWhenPopulated(string fieldName, bool?[]? value) =>
        value?.Any() ?? false ? AddAlways(fieldName, value) : stb.StyleTypeBuilder;

    public TExt AddWhenPopulated<TNum>(string fieldName, TNum[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value?.Any() ?? false ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt AddWhenPopulated<TNum>(string fieldName, TNum?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value?.Any() ?? false ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt AddWhenPopulated<TStruct>
        (string fieldName, TStruct[]? value, StructStyler<TStruct> structToString) where TStruct : struct =>
        value?.Any() ?? false ? AddAlways(fieldName, value, structToString) : stb.StyleTypeBuilder;

    public TExt AddWhenPopulated<TStruct>
        (string fieldName, TStruct?[]? value, StructStyler<TStruct> structToString) where TStruct : struct =>
        value?.Any() ?? false ? AddAlways(fieldName, value, structToString) : stb.StyleTypeBuilder;

    public TExt AddWhenPopulated(string fieldName, string?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value?.Any() ?? false ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt AddWhenPopulated(string fieldName, IStyledToStringObject?[]? value) =>
        value?.Any() ?? false ? AddAlways(fieldName, value) : stb.StyleTypeBuilder;

    public TExt AddWhenPopulated(string fieldName, IFrozenString?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value?.Any() ?? false ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt AddWhenPopulated(string fieldName, IStringBuilder?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value?.Any() ?? false ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt AddWhenPopulated(string fieldName, StringBuilder?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value?.Any() ?? false ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;


    [CallsObjectToString]
    public TExt AddWhenPopulated(string fieldName, object?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value?.Any() ?? false ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;


    public TExt AddWhenPopulated(string fieldName, IReadOnlyList<bool>? value) =>
        value?.Any() ?? false ? AddAlways(fieldName, value) : stb.StyleTypeBuilder;

    public TExt AddWhenPopulated(string fieldName, IReadOnlyList<bool?>? value) =>
        value?.Any() ?? false ? AddAlways(fieldName, value) : stb.StyleTypeBuilder;

    public TExt AddWhenPopulated<TNum>(string fieldName, IReadOnlyList<TNum>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value?.Any() ?? false ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt AddWhenPopulated<TNum>(string fieldName, IReadOnlyList<TNum?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value?.Any() ?? false ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt AddWhenPopulated<TStruct>
        (string fieldName, IReadOnlyList<TStruct>? value, StructStyler<TStruct> structToString) where TStruct : struct =>
        value?.Any() ?? false ? AddAlways(fieldName, value, structToString) : stb.StyleTypeBuilder;

    public TExt AddWhenPopulated<TStruct>
        (string fieldName, IReadOnlyList<TStruct?>? value, StructStyler<TStruct> structToString) where TStruct : struct =>
        value?.Any() ?? false ? AddAlways(fieldName, value, structToString) : stb.StyleTypeBuilder;

    public TExt AddWhenPopulated(string fieldName, IReadOnlyList<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value?.Any() ?? false ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt AddWhenPopulated(string fieldName, IReadOnlyList<IStyledToStringObject?>? value) =>
        value?.Any() ?? false ? AddAlways(fieldName, value) : stb.StyleTypeBuilder;

    public TExt AddWhenPopulated(string fieldName, IReadOnlyList<IFrozenString?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value?.Any() ?? false ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt AddWhenPopulated(string fieldName, IReadOnlyList<IStringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value?.Any() ?? false ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt AddWhenPopulated(string fieldName, IReadOnlyList<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value?.Any() ?? false ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;


    [CallsObjectToString]
    public TExt AddWhenPopulated(string fieldName, IReadOnlyList<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value?.Any() ?? false ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;


    public TExt AddWhenPopulated(string fieldName, IEnumerable<bool>? value) =>
        value?.Any() ?? false ? AddAlways(fieldName, value) : stb.StyleTypeBuilder;

    public TExt AddWhenPopulated(string fieldName, IEnumerable<bool?>? value) =>
        value?.Any() ?? false ? AddAlways(fieldName, value) : stb.StyleTypeBuilder;

    public TExt AddWhenPopulated<TNum>(string fieldName, IEnumerable<TNum>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value?.Any() ?? false ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt AddWhenPopulated<TNum>(string fieldName, IEnumerable<TNum?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value?.Any() ?? false ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt AddWhenPopulated<TStruct>
        (string fieldName, IEnumerable<TStruct>? value, StructStyler<TStruct> structToString) where TStruct : struct =>
        value?.Any() ?? false ? AddAlways(fieldName, value, structToString) : stb.StyleTypeBuilder;

    public TExt AddWhenPopulated<TStruct>
        (string fieldName, IEnumerable<TStruct?>? value, StructStyler<TStruct> structToString) where TStruct : struct =>
        value?.Any() ?? false ? AddAlways(fieldName, value, structToString) : stb.StyleTypeBuilder;

    public TExt AddWhenPopulated(string fieldName, IEnumerable<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value?.Any() ?? false ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt AddWhenPopulated(string fieldName, IEnumerable<IStyledToStringObject?>? value) =>
        value?.Any() ?? false ? AddAlways(fieldName, value) : stb.StyleTypeBuilder;

    public TExt AddWhenPopulated(string fieldName, IEnumerable<IFrozenString?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value?.Any() ?? false ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt AddWhenPopulated(string fieldName, IEnumerable<IStringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value?.Any() ?? false ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt AddWhenPopulated(string fieldName, IEnumerable<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value?.Any() ?? false ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;


    [CallsObjectToString]
    public TExt AddWhenPopulated(string fieldName, IEnumerable<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value?.Any() ?? false ? AddAlways(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt AddWhenPopulated(string fieldName, IEnumerator<bool>? value) 
    {
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.FieldNameJoin(fieldName);
            stb.StartCollection();
            while (hasValue)
            {
                stb.Sb.Append(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt AddWhenPopulated(string fieldName, IEnumerator<bool?>? value) 
    {
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {   
            stb.FieldNameJoin(fieldName); 
            stb.StartCollection();
            while (hasValue)
            {
                stb.Sb.Append(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt AddWhenPopulated<TNum>(string fieldName, IEnumerator<TNum>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TNum : struct, INumber<TNum>
    {
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {   
            stb.FieldNameJoin(fieldName);
            stb.StartCollection();
            while (hasValue)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value!.Current, formatString)
                    : stb.Sb.Append(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt AddWhenPopulated<TNum>(string fieldName, IEnumerator<TNum?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TNum : struct, INumber<TNum>
    {
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {   
            stb.FieldNameJoin(fieldName);
            stb.StartCollection();
            while (hasValue)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value!.Current, formatString)
                    : stb.Sb.AppendOrNull(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }
    

    public TExt AddWhenPopulated<TStruct>
        (string fieldName, IEnumerator<TStruct>? value, StructStyler<TStruct> structToString) 
        where TStruct : struct
    {
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {   
            stb.FieldNameJoin(fieldName);
            stb.StartCollection();
            while (hasValue)
            {
                structToString(value!.Current, stb.OwningAppender);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt AddWhenPopulated<TStruct>
        (string fieldName, IEnumerator<TStruct?>? value, StructStyler<TStruct> structToString) 
        where TStruct : struct
    {
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {   
            stb.FieldNameJoin(fieldName);
            stb.StartCollection();
            while (hasValue)
            {
                stb.AppendOrNull(value!.Current, structToString);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }


    public TExt AddWhenPopulated(string fieldName, IEnumerator<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {   
            stb.FieldNameJoin(fieldName);
            stb.StartCollection();
            while (hasValue)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value!.Current, formatString)
                    : stb.Sb.Append(value!.Current ?? stb.OwningAppender.NullStyle);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt AddWhenPopulated(string fieldName, IEnumerator<IFrozenString?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {   
            stb.FieldNameJoin(fieldName);
            stb.StartCollection();
            while (hasValue)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value!.Current, formatString)
                    : stb.AppendOrNull(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt AddWhenPopulated(string fieldName, IEnumerator<IStringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {   
            stb.FieldNameJoin(fieldName);
            stb.StartCollection();
            while (hasValue)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value!.Current, formatString)
                    : stb.AppendOrNull(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt AddWhenPopulated(string fieldName, IEnumerator<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {   
            stb.FieldNameJoin(fieldName);
            stb.StartCollection();
            while (hasValue)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value!.Current, formatString)
                    : stb.AppendOrNull(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt AddWhenPopulated(string fieldName, IEnumerator<IStyledToStringObject?>? value)
    {
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {   
            stb.FieldNameJoin(fieldName);
            stb.StartCollection();
            while (hasValue)
            {
                stb.AppendOrNull(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TExt AddWhenPopulated(string fieldName, IEnumerator<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {   
            stb.FieldNameJoin(fieldName);
            stb.StartCollection();
            while (hasValue)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value!.Current, formatString)
                    : stb.AppendOrNull(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }
}
