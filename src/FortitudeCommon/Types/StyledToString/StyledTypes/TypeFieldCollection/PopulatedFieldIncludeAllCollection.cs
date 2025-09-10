// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable.Strings;
#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldCollection;

public partial class SelectTypeCollectionField<TExt> where TExt : StyledTypeBuilder
{
    public TExt WhenPopulatedAddAll(string fieldName, bool[]? value) =>
        !stb.SkipBody && (value?.Any() ?? false) ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(string fieldName, bool?[]? value) =>
        !stb.SkipBody && (value?.Any() ?? false) ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TFmt>(string fieldName, TFmt[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipBody && (value?.Any() ?? false) ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TFmtStruct>(string fieldName, TFmtStruct?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipBody && (value?.Any() ?? false) ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TToStyle, TStylerType>
        (string fieldName, TToStyle[]? value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType =>
        !stb.SkipBody && (value?.Any() ?? false) ? AlwaysAddAll(fieldName, value, customTypeStyler) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(string fieldName, string?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipBody && (value?.Any() ?? false) ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllCharSequence<TCharSeq>(string fieldName, TCharSeq?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipBody && (value?.Any() ?? false) ? AlwaysAddAllCharSequence(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(string fieldName, StringBuilder?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipBody && (value?.Any() ?? false) ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TStyledObj>(string fieldName, TStyledObj[]? value)
        where TStyledObj : class, IStyledToStringObject =>
        !stb.SkipBody && (value?.Any() ?? false) ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;


    [CallsObjectToString]
    public TExt WhenPopulatedAddAllMatch<T>(string fieldName, T[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : class =>
        !stb.SkipBody && (value?.Any() ?? false) ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;


    public TExt WhenPopulatedAddAll(string fieldName, IReadOnlyList<bool>? value) =>
        !stb.SkipBody && (value?.Any() ?? false) ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(string fieldName, IReadOnlyList<bool?>? value) =>
        !stb.SkipBody && (value?.Any() ?? false) ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TFmt>(string fieldName, IReadOnlyList<TFmt>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipBody && (value?.Any() ?? false) ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TFmtStruct>(string fieldName, IReadOnlyList<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipBody && (value?.Any() ?? false) ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TToStyle, TStylerType>
        (string fieldName, IReadOnlyList<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType =>
        !stb.SkipBody && (value?.Any() ?? false) ? AlwaysAddAll(fieldName, value, customTypeStyler) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(string fieldName, IReadOnlyList<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipBody && (value?.Any() ?? false) ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllCharSequence<TCHarSeq>(string fieldName, IReadOnlyList<TCHarSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCHarSeq : ICharSequence =>
        !stb.SkipBody && (value?.Any() ?? false) ? AlwaysAddAllCharSequence(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(string fieldName, IReadOnlyList<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipBody && (value?.Any() ?? false) ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TStyledObj>(string fieldName, IReadOnlyList<TStyledObj>? value)
        where TStyledObj : class, IStyledToStringObject =>
        !stb.SkipBody && (value?.Any() ?? false) ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    [CallsObjectToString]
    public TExt WhenPopulatedAddAllMatch<T>(string fieldName, IReadOnlyList<T>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where T : class =>
        !stb.SkipBody && (value?.Any() ?? false) ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;
    
    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerable<bool>? value) =>
        !stb.SkipBody && (value?.Any() ?? false) ? AlwaysAddAllEnumerate(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerable<bool?>? value) =>
        !stb.SkipBody && (value?.Any() ?? false) ? AlwaysAddAllEnumerate(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate<TFmt>(string fieldName, IEnumerable<TFmt>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipBody && (value?.Any() ?? false) ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate<TFmtStruct>(string fieldName, IEnumerable<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipBody && (value?.Any() ?? false) ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate<TToStyle, TStylerType>
        (string fieldName, IEnumerable<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType =>
        !stb.SkipBody && (value?.Any() ?? false) ? AlwaysAddAllEnumerate(fieldName, value, customTypeStyler) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerable<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipBody && (value?.Any() ?? false) ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllCharSequenceEnumerate<TCharSeq>(string fieldName, IEnumerable<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipBody && (value?.Any() ?? false) ? AlwaysAddAllCharSequenceEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerable<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipBody && (value?.Any() ?? false) ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate<TStyledObj>(string fieldName, IEnumerable<TStyledObj>? value)
        where TStyledObj : class, IStyledToStringObject =>
        !stb.SkipBody && (value?.Any() ?? false) ? AlwaysAddAllEnumerate(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<T, TBase>(string fieldName, IEnumerable<T?>? value, CustomTypeStyler<TBase> customTypeStyler)
        where T : class, TBase where TBase: class =>
        !stb.SkipBody && (value?.Any() ?? false) ? AlwaysAddAllEnumerate(fieldName, value, customTypeStyler) : stb.StyleTypeBuilder;


    [CallsObjectToString]
    public TExt WhenPopulatedAddAllMatchEnumerate<T>(string fieldName, IEnumerable<T>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where T : class =>
        !stb.SkipBody && (value?.Any() ?? false) ? AlwaysAddAllMatchEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerator<bool>? value) 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.FieldNameJoin(fieldName);
            var elementType = typeof(bool);
            var itemCount   = 0;
            stb.StartCollection(elementType, true);
            while (hasValue)
            {
                stb.Sb.Append(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            stb.EndCollection(elementType, itemCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerator<bool?>? value) 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {   
            stb.FieldNameJoin(fieldName); 
            var elementType = typeof(bool?);
            var itemCount   = 0;
            stb.StartCollection(elementType, true);
            while (hasValue)
            {
                stb.Sb.Append(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            stb.EndCollection(elementType, itemCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllEnumerate<TFmt>(string fieldName, IEnumerator<TFmt>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TFmt : ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {   
            stb.FieldNameJoin(fieldName);
            var elementType = typeof(TFmt);
            var itemCount   = 0;
            stb.StartCollection(elementType, true);
            while (hasValue)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value!.Current, formatString)
                    : stb.Sb.Append(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            stb.EndCollection(elementType, itemCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllEnumerate<TFmtStruct>(string fieldName, IEnumerator<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {   
            stb.FieldNameJoin(fieldName);
            var elementType = typeof(TFmtStruct?);
            var itemCount   = 0;
            stb.StartCollection(elementType, true);
            while (hasValue)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value!.Current, formatString)
                    : stb.Sb.Append(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            stb.EndCollection(elementType, itemCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllEnumerate<TToStyle, TStylerType>
        (string fieldName, IEnumerator<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler) 
        where TToStyle : TStylerType
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {   
            stb.FieldNameJoin(fieldName);
            var elementType = typeof(TToStyle?);
            var itemCount   = 0;
            stb.StartCollection(elementType, true);
            while (hasValue)
            {
                stb.AppendOrNull(value!.Current, customTypeStyler);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            stb.EndCollection(elementType, itemCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }


    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerator<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {   
            stb.FieldNameJoin(fieldName);
            var elementType = typeof(string);
            var itemCount   = 0;
            stb.StartCollection(elementType, true);
            while (hasValue)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value!.Current, formatString)
                    : stb.Sb.Append(value!.Current ?? stb.Settings.NullStyle);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            stb.EndCollection(elementType, itemCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllCharSequenceEnumerate<TCharSeq>(string fieldName, IEnumerator<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {   
            stb.FieldNameJoin(fieldName);
            var elementType = typeof(TCharSeq);
            var itemCount   = 0;
            stb.StartCollection(elementType, true);
            while (hasValue)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value!.Current, formatString)
                    : stb.AppendOrNull(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            stb.EndCollection(elementType, itemCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerator<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {   
            stb.FieldNameJoin(fieldName);
            var elementType = typeof(StringBuilder);
            var itemCount   = 0;
            stb.StartCollection(elementType, true);
            while (hasValue)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value!.Current, formatString)
                    : stb.AppendOrNull(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            stb.EndCollection(elementType, itemCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TExt WhenPopulatedAddAllMatchEnumerate<T>(string fieldName, IEnumerator<T>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {   
            stb.FieldNameJoin(fieldName);
            var elementType = typeof(T);
            var itemCount   = 0;
            stb.StartCollection(elementType, true);
            while (hasValue)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value!.Current, formatString)
                    : stb.AppendObjectOrNull(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            stb.EndCollection(elementType, itemCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }
}
