// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldCollection;

public partial class SelectTypeCollectionField<TExt> where TExt : TypeMolder
{
    public TExt AlwaysAddAll(ReadOnlySpan<char> fieldName, Span<bool> value)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(Span<bool>);
        var elementType    = typeof(bool);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendCollectionItem(item, i);
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll(ReadOnlySpan<char> fieldName, Span<bool?> value)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(Span<bool?>);
        var elementType    = typeof(bool?);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendCollectionItem(item, i);
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TFmt>(ReadOnlySpan<char> fieldName, Span<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(Span<TFmt>);
        var elementType    = typeof(TFmt);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllNullable<TFmt>(ReadOnlySpan<char> fieldName, Span<TFmt?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : class, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(Span<TFmt>);
        var elementType    = typeof(TFmt);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TStructFmt>(ReadOnlySpan<char> fieldName, Span<TStructFmt?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TStructFmt : struct, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(Span<TStructFmt>);
        var elementType    = typeof(TStructFmt);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TCloaked, TCloakedBase>(ReadOnlySpan<char> fieldName, Span<TCloaked> value
      , PalantírReveal<TCloakedBase> palantírReveal) where TCloaked : TCloakedBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(Span<TCloakedBase>);
        var elementType    = typeof(TCloakedBase);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                if (i > 0) stb.GoToNextCollectionItemStart(elementType, i);
                var item = value[i];
                stb.AppendOrNull(item, palantírReveal);
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAllNullable<TCloaked, TCloakedBase>(ReadOnlySpan<char> fieldName, Span<TCloaked?> value
      , PalantírReveal<TCloakedBase> palantírReveal) where TCloaked : class, TCloakedBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(Span<TCloakedBase>);
        var elementType    = typeof(TCloakedBase);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                if (i > 0) stb.GoToNextCollectionItemStart(elementType, i);
                var item = value[i];
                stb.AppendOrNull(item, palantírReveal);
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TCloakedStruct>(ReadOnlySpan<char> fieldName, Span<TCloakedStruct?> value, PalantírReveal<TCloakedStruct> palantírReveal)
        where TCloakedStruct : struct
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(Span<TCloakedStruct?>);
        var elementType    = typeof(TCloakedStruct?);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                if (i > 0) stb.GoToNextCollectionItemStart(elementType, i);
                var item = value[i];
                stb.AppendOrNull(item, palantírReveal);
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TBearer>(ReadOnlySpan<char> fieldName, Span<TBearer> value)
        where TBearer : IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(Span<TBearer>);
        var elementType    = typeof(TBearer);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                if (i > 0) stb.GoToNextCollectionItemStart(elementType, i);
                var item = value[i];
                stb.AppendRevealBearerOrNull(item);
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAllNullable<TBearer>(ReadOnlySpan<char> fieldName, Span<TBearer?> value)
        where TBearer : class, IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(Span<TBearer>);
        var elementType    = typeof(TBearer);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                if (i > 0) stb.GoToNextCollectionItemStart(elementType, i);
                var item = value[i];
                stb.AppendRevealBearerOrNull(item);
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TBearerStruct>(ReadOnlySpan<char> fieldName, Span<TBearerStruct?> value)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(Span<TBearerStruct?>);
        var elementType    = typeof(TBearerStruct?);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                if (i > 0) stb.GoToNextCollectionItemStart(elementType, i);
                var item = value[i];
                stb.AppendRevealBearerOrNull(item);
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll(ReadOnlySpan<char> fieldName, Span<string> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(Span<string>);
        var elementType    = typeof(string);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString ?? "");
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllNullable(ReadOnlySpan<char> fieldName, Span<string?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(Span<string>);
        var elementType    = typeof(string);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString ?? "");
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, Span<TCharSeq> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TCharSeq : ICharSequence
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(Span<TCharSeq>);
        var elementType    = typeof(TCharSeq);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString ?? "");
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllCharSeqNullable<TCharSeq>(ReadOnlySpan<char> fieldName, Span<TCharSeq?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TCharSeq : ICharSequence
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(Span<TCharSeq>);
        var elementType    = typeof(TCharSeq);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString ?? "");
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll(ReadOnlySpan<char> fieldName, Span<StringBuilder> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(Span<StringBuilder>);
        var elementType    = typeof(StringBuilder);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString ?? "");
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllNullable(ReadOnlySpan<char> fieldName, Span<StringBuilder?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(Span<StringBuilder>);
        var elementType    = typeof(StringBuilder);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString ?? "");
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllMatch<TAny>(ReadOnlySpan<char> fieldName, Span<TAny> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(Span<TAny>);
        var elementType    = typeof(TAny);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllObject(ReadOnlySpan<char> fieldName, Span<object> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(Span<object>);
        var elementType    = typeof(object);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllObjectNullable(ReadOnlySpan<char> fieldName, Span<object?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(Span<object>);
        var elementType    = typeof(object);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }
    
    public TExt AlwaysAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool> value)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(ReadOnlySpan<bool>);
        var elementType    = typeof(bool);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendCollectionItem(item, i);
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool?> value)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(ReadOnlySpan<bool?>);
        var elementType    = typeof(bool?);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendCollectionItem(item, i);
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TFmt>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(ReadOnlySpan<TFmt>);
        var elementType    = typeof(TFmt);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllNullable<TFmt>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmt?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : class, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(ReadOnlySpan<TFmt>);
        var elementType    = typeof(TFmt);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TStructFmt>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TStructFmt?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TStructFmt : struct, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(ReadOnlySpan<TStructFmt>);
        var elementType    = typeof(TStructFmt);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TCloaked, TCloakedBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloaked> value
      , PalantírReveal<TCloakedBase> palantírReveal) where TCloaked : TCloakedBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(ReadOnlySpan<TCloakedBase>);
        var elementType    = typeof(TCloakedBase);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                if (i > 0) stb.GoToNextCollectionItemStart(elementType, i);
                var item = value[i];
                stb.AppendOrNull(item, palantírReveal);
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAllNullable<TCloaked, TCloakedBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloaked?> value
      , PalantírReveal<TCloakedBase> palantírReveal) where TCloaked : class, TCloakedBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(ReadOnlySpan<TCloakedBase>);
        var elementType    = typeof(TCloakedBase);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                if (i > 0) stb.GoToNextCollectionItemStart(elementType, i);
                var item = value[i];
                stb.AppendOrNull(item, palantírReveal);
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TCloakedStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloakedStruct?> value
      , PalantírReveal<TCloakedStruct> palantírReveal)
        where TCloakedStruct : struct
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(ReadOnlySpan<TCloakedStruct?>);
        var elementType    = typeof(TCloakedStruct?);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                if (i > 0) stb.GoToNextCollectionItemStart(elementType, i);
                var item = value[i];
                stb.AppendOrNull(item, palantírReveal);
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TBearer>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearer> value)
        where TBearer : IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(ReadOnlySpan<TBearer>);
        var elementType    = typeof(TBearer);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                if (i > 0) stb.GoToNextCollectionItemStart(elementType, i);
                var item = value[i];
                stb.AppendRevealBearerOrNull(item);
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAllNullable<TBearer>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearer?> value)
        where TBearer : class, IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(ReadOnlySpan<TBearer>);
        var elementType    = typeof(TBearer);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                if (i > 0) stb.GoToNextCollectionItemStart(elementType, i);
                var item = value[i];
                stb.AppendRevealBearerOrNull(item);
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TBearerStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearerStruct?> value)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(ReadOnlySpan<TBearerStruct?>);
        var elementType    = typeof(TBearerStruct?);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                if (i > 0) stb.GoToNextCollectionItemStart(elementType, i);
                var item = value[i];
                stb.AppendRevealBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<string> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(ReadOnlySpan<string>);
        var elementType    = typeof(string);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString ?? "");
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<string?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(ReadOnlySpan<string>);
        var elementType    = typeof(string);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString ?? "");
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCharSeq> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TCharSeq : ICharSequence
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(ReadOnlySpan<TCharSeq>);
        var elementType    = typeof(TCharSeq);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString ?? "");
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllCharSeqNullable<TCharSeq>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCharSeq?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TCharSeq : ICharSequence
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(ReadOnlySpan<TCharSeq>);
        var elementType    = typeof(TCharSeq);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString ?? "");
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(ReadOnlySpan<StringBuilder>);
        var elementType    = typeof(StringBuilder);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString ?? "");
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(ReadOnlySpan<StringBuilder>);
        var elementType    = typeof(StringBuilder);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString ?? "");
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllMatch<TAny>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TAny> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(ReadOnlySpan<TAny>);
        var elementType    = typeof(TAny);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllMatchNullable<TAny>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TAny?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(ReadOnlySpan<TAny>);
        var elementType    = typeof(TAny);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllObject(ReadOnlySpan<char> fieldName, ReadOnlySpan<object> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(ReadOnlySpan<object>);
        var elementType    = typeof(object);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllObjectNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<object?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(ReadOnlySpan<object>);
        var elementType    = typeof(object);
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, elementType, value.Length);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll(string fieldName, bool[]? value)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;

        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAll(value).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll(string fieldName, bool?[]? value)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAll(value).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TFmt>(string fieldName, TFmt?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var collectionType = stb.Master.StartSimpleCollectionType(value);
            collectionType.AddAll(value, formatString);
            collectionType.Complete();
        }
        else { stb.Sb.Append(stb.Settings.NullStyle); }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TStructFmt>(string fieldName, TStructFmt?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TStructFmt : struct, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAll(value, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TCloaked, TCloakedBase>
        (string fieldName, TCloaked?[]? value, PalantírReveal<TCloakedBase> palantírReveal) where TCloaked : TCloakedBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealAll(value, palantírReveal).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TCloakedStruct>
        (string fieldName, TCloakedStruct?[]? value, PalantírReveal<TCloakedStruct> palantírReveal)
        where TCloakedStruct : struct
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealAll(value, palantírReveal).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TBearer>(string fieldName, TBearer?[]? value)
        where TBearer : IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealAll(value).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TBearerStruct>(string fieldName, TBearerStruct?[]? value)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealAll(value).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll(string fieldName, string?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAll(value, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllCharSeq<TCharSeq>(string fieldName, TCharSeq?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TCharSeq : ICharSequence
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAllCharSeq(value, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll(string fieldName, StringBuilder?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAll(value, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllMatch<TAny>(string fieldName, TAny?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAllMatch(value, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllObject(string fieldName, object?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAllMatch(value, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll(string fieldName, IReadOnlyList<bool>? value)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAll(value).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll(string fieldName, IReadOnlyList<bool?>? value)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAll(value).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TFmt>(string fieldName, IReadOnlyList<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAll(value, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TFmtStruct>(string fieldName, IReadOnlyList<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAll(value, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TCloaked, TCloakedBase>
        (string fieldName, IReadOnlyList<TCloaked?>? value, PalantírReveal<TCloakedBase> palantírReveal)
        where TCloaked : TCloakedBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealAll(value, palantírReveal).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TCloakedStruct>
        (string fieldName, IReadOnlyList<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal)
        where TCloakedStruct : struct
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealAll(value, palantírReveal).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TBearer>(string fieldName, IReadOnlyList<TBearer?>? value)
        where TBearer : IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealAll(value).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TBearerStruct>(string fieldName, IReadOnlyList<TBearerStruct?>? value)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealAll(value).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll(string fieldName, IReadOnlyList<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAll(value, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllCharSeq<TCharSeq>(string fieldName, IReadOnlyList<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TCharSeq : ICharSequence
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAllCharSeq(value, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll(string fieldName, IReadOnlyList<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAll(value, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllMatch<TAny>(string fieldName, IReadOnlyList<TAny>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAllMatch(value, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllObject(string fieldName, IReadOnlyList<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAllMatch(value, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerable<bool>? value)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAllEnumerate(value).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerable<bool?>? value)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAllEnumerate(value).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TFmt>(string fieldName, IEnumerable<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAllEnumerate(value, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TFmtStruct>(string fieldName, IEnumerable<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAllEnumerate(value, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAllEnumerate<TCloaked, TCloakedBase>
        (string fieldName, IEnumerable<TCloaked?>? value, PalantírReveal<TCloakedBase> palantírReveal)
        where TCloaked : TCloakedBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealAllEnumerate(value, palantírReveal).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAllEnumerate<TCloakedStruct>
        (string fieldName, IEnumerable<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal)
        where TCloakedStruct : struct
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealAllEnumerate(value, palantírReveal).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAllEnumerate<TBearer>(string fieldName, IEnumerable<TBearer?>? value)
        where TBearer : IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealAllEnumerate(value).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAllEnumerate<TBearerStruct>(string fieldName, IEnumerable<TBearerStruct?>? value)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealAllEnumerate(value).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerable<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAllEnumerate(value, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllCharSeqEnumerate<TCharSeq>(string fieldName, IEnumerable<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TCharSeq : ICharSequence
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAllCharSeqEnumerate(value, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerable<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAllEnumerate(value, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllMatchEnumerate<TAny>(string fieldName, IEnumerable<TAny>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAllMatchEnumerate(value, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllObjectEnumerate(string fieldName, IEnumerable<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAllMatchEnumerate(value, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerator<bool>? value)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
            stb.Master.StartSimpleCollectionType(value).AddAllEnumerate(value).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerator<bool?>? value)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
            stb.Master.StartSimpleCollectionType(value).AddAllEnumerate(value).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TFmt>(string fieldName, IEnumerator<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
            stb.Master.StartSimpleCollectionType(value).AddAllEnumerate(value, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TFmtStruct>(string fieldName, IEnumerator<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
            stb.Master.StartSimpleCollectionType(value).AddAllEnumerate(value, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAllEnumerate<TCloaked, TCloakedBase>
        (string fieldName, IEnumerator<TCloaked?>? value, PalantírReveal<TCloakedBase> palantírReveal)
        where TCloaked : TCloakedBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
            stb.Master.StartSimpleCollectionType(value).RevealAllEnumerate(value, palantírReveal).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAllEnumerate<TCloakedStruct>
        (string fieldName, IEnumerator<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal)
        where TCloakedStruct : struct
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
            stb.Master.StartSimpleCollectionType(value).RevealAllEnumerate(value, palantírReveal).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAllEnumerate<TBearer>(string fieldName, IEnumerator<TBearer?>? value)
        where TBearer : IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
            stb.Master.StartSimpleCollectionType(value).RevealAllEnumerate(value).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAllEnumerate<TBearerStruct>(string fieldName, IEnumerator<TBearerStruct?>? value)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
            stb.Master.StartSimpleCollectionType(value).RevealAllEnumerate(value).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerator<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
            stb.Master.StartSimpleCollectionType(value).AddAllEnumerate(value, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllCharSeqEnumerate<TCharSeq>(string fieldName, IEnumerator<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TCharSeq : ICharSequence
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
            stb.Master.StartSimpleCollectionType(value).AddAllCharSeqEnumerate(value, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerator<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
            stb.Master.StartSimpleCollectionType(value).AddAllEnumerate(value, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllMatchEnumerate<TAny>(string fieldName, IEnumerator<TAny>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
            stb.Master.StartSimpleCollectionType(value).AddAllMatchEnumerate(value, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllObjectEnumerate(string fieldName, IEnumerator<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
            stb.Master.StartSimpleCollectionType(value).AddAllMatchEnumerate(value, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }
}
