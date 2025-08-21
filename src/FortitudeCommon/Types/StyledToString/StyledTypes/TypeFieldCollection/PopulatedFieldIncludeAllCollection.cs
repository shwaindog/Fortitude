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
    public TExt WhenPopulatedAddAll(string fieldName, bool[]? value) =>
        value?.Any() ?? false ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(string fieldName, bool?[]? value) =>
        value?.Any() ?? false ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TFmtStruct>(string fieldName, TFmtStruct[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        value?.Any() ?? false ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TStruct>
        (string fieldName, TStruct[]? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct =>
        value?.Any() ?? false ? AlwaysAddAll(fieldName, value, customTypeStyler) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(string fieldName, string?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value?.Any() ?? false ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(string fieldName, ICharSequence?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value?.Any() ?? false ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(string fieldName, StringBuilder?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value?.Any() ?? false ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TStyledObj>(string fieldName, TStyledObj[]? value)
        where TStyledObj : class, IStyledToStringObject =>
        value?.Any() ?? false ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<T, TBase>(string fieldName, T[]? value, CustomTypeStyler<TBase> customTypeStyler)
        where T : class, TBase where TBase: class =>
        value?.Any() ?? false ? AlwaysAddAll(fieldName, value, customTypeStyler) : stb.StyleTypeBuilder;


    [CallsObjectToString]
    public TExt WhenPopulatedAddAllMatch<T>(string fieldName, T[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : class =>
        value?.Any() ?? false ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;


    public TExt WhenPopulatedAddAll(string fieldName, IReadOnlyList<bool>? value) =>
        value?.Any() ?? false ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(string fieldName, IReadOnlyList<bool?>? value) =>
        value?.Any() ?? false ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TFmtStruct>(string fieldName, IReadOnlyList<TFmtStruct>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        value?.Any() ?? false ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TStruct>
        (string fieldName, IReadOnlyList<TStruct>? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct =>
        value?.Any() ?? false ? AlwaysAddAll(fieldName, value, customTypeStyler) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(string fieldName, IReadOnlyList<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value?.Any() ?? false ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(string fieldName, IReadOnlyList<ICharSequence?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value?.Any() ?? false ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(string fieldName, IReadOnlyList<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value?.Any() ?? false ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TStyledObj>(string fieldName, IReadOnlyList<TStyledObj>? value)
        where TStyledObj : class, IStyledToStringObject =>
        value?.Any() ?? false ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<T, TBase>(string fieldName, IReadOnlyList<T?>? value, CustomTypeStyler<TBase> customTypeStyler)
        where T : class, TBase where TBase: class =>
        value?.Any() ?? false ? AlwaysAddAll(fieldName, value, customTypeStyler) : stb.StyleTypeBuilder;


    [CallsObjectToString]
    public TExt WhenPopulatedAddAllMatch<T>(string fieldName, IReadOnlyList<T>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where T : class =>
        value?.Any() ?? false ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;


    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerable<bool>? value) =>
        value?.Any() ?? false ? AlwaysAddAllEnumerate(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerable<bool?>? value) =>
        value?.Any() ?? false ? AlwaysAddAllEnumerate(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate<TFmtStruct>(string fieldName, IEnumerable<TFmtStruct>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        value?.Any() ?? false ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate<TStruct>
        (string fieldName, IEnumerable<TStruct>? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct =>
        value?.Any() ?? false ? AlwaysAddAllEnumerate(fieldName, value, customTypeStyler) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerable<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value?.Any() ?? false ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerable<ICharSequence?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value?.Any() ?? false ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerable<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value?.Any() ?? false ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate<TStyledObj>(string fieldName, IEnumerable<TStyledObj>? value)
        where TStyledObj : class, IStyledToStringObject =>
        value?.Any() ?? false ? AlwaysAddAllEnumerate(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<T, TBase>(string fieldName, IEnumerable<T?>? value, CustomTypeStyler<TBase?> customTypeStyler)
        where T : class, TBase where TBase: class =>
        value?.Any() ?? false ? AlwaysAddAllEnumerate(fieldName, value, customTypeStyler) : stb.StyleTypeBuilder;


    [CallsObjectToString]
    public TExt WhenPopulatedAddAllMatchEnumerate<T>(string fieldName, IEnumerable<T>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where T : class =>
        value?.Any() ?? false ? AlwaysAddAllMatchEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerator<bool>? value) 
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

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerator<bool?>? value) 
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

    public TExt WhenPopulatedAddAllEnumerate<TFmtStruct>(string fieldName, IEnumerator<TFmtStruct>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TFmtStruct : struct, ISpanFormattable
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

    public TExt WhenPopulatedAddAllEnumerate<TStruct>
        (string fieldName, IEnumerator<TStruct>? value, CustomTypeStyler<TStruct> customTypeStyler) 
        where TStruct : struct
    {
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {   
            stb.FieldNameJoin(fieldName);
            stb.StartCollection();
            while (hasValue)
            {
                stb.AppendOrNull(value!.Current, customTypeStyler);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }


    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerator<string?>? value
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

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerator<ICharSequence?>? value
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

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerator<StringBuilder?>? value
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

    public TExt WhenPopulatedAddAllEnumerate<TStyledObj>(string fieldName, IEnumerator<TStyledObj>? value)
        where TStyledObj : class, IStyledToStringObject
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

    public TExt WhenPopulatedAddAllEnumerate<T, TBase>(string fieldName, IEnumerator<T?>? value, CustomTypeStyler<TBase?> customTypeStyler)
        where T : class, TBase
        where TBase : class
    {
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {   
            stb.FieldNameJoin(fieldName);
            stb.StartCollection();
            while (hasValue)
            {
                stb.AppendOrNull(value!.Current, customTypeStyler);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TExt WhenPopulatedAddAllMatchEnumerate<T>(string fieldName, IEnumerator<T>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    where T : class
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
