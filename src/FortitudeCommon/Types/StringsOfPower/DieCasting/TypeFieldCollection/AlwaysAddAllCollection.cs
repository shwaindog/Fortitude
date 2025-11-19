// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldCollection;

public partial class SelectTypeCollectionField<TExt> where TExt : TypeMolder
{
    public TExt AlwaysAddAll(ReadOnlySpan<char> fieldName, Span<bool> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<bool>);
        var elementType    = typeof(bool);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll(ReadOnlySpan<char> fieldName, Span<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<bool?>);
        var elementType    = typeof(bool?);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TFmt>(ReadOnlySpan<char> fieldName, Span<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<TFmt>);
        var elementType    = typeof(TFmt);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllNullable<TFmt>(ReadOnlySpan<char> fieldName, Span<TFmt?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmt : class, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<TFmt>);
        var elementType    = typeof(TFmt);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TStructFmt>(ReadOnlySpan<char> fieldName, Span<TStructFmt?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TStructFmt : struct, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<TStructFmt>);
        var elementType    = typeof(TStructFmt);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TCloaked, TRevealBase>(ReadOnlySpan<char> fieldName, Span<TCloaked> value
      , PalantírReveal<TRevealBase> palantírReveal, FieldContentHandling formatFlags = DefaultCallerTypeFlags) 
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<TRevealBase>);
        var elementType    = typeof(TRevealBase);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.RevealCloakedBearerOrNull(item, palantírReveal, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TCloakedStruct>(ReadOnlySpan<char> fieldName, Span<TCloakedStruct?> value
      , PalantírReveal<TCloakedStruct> palantírReveal, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<TCloakedStruct?>);
        var elementType    = typeof(TCloakedStruct?);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.RevealNullableCloakedBearerOrNull(item, palantírReveal, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TBearer>(ReadOnlySpan<char> fieldName, Span<TBearer> value, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<TBearer>);
        var elementType    = typeof(TBearer);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.RevealStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TBearerStruct>(ReadOnlySpan<char> fieldName, Span<TBearerStruct?> value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<TBearerStruct?>);
        var elementType    = typeof(TBearerStruct?);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.RevealNullableStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll(ReadOnlySpan<char> fieldName, Span<string> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<string>);
        var elementType    = typeof(string);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllNullable(ReadOnlySpan<char> fieldName, Span<string?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<string>);
        var elementType    = typeof(string);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, Span<TCharSeq> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<TCharSeq>);
        var elementType    = typeof(TCharSeq);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType,   0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllCharSeqNullable<TCharSeq>(ReadOnlySpan<char> fieldName, Span<TCharSeq?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<TCharSeq>);
        var elementType    = typeof(TCharSeq);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType,   0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll(ReadOnlySpan<char> fieldName, Span<StringBuilder> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<StringBuilder>);
        var elementType    = typeof(StringBuilder);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllNullable(ReadOnlySpan<char> fieldName, Span<StringBuilder?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<StringBuilder>);
        var elementType    = typeof(StringBuilder);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllMatch<TAny>(ReadOnlySpan<char> fieldName, Span<TAny> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<TAny>);
        var elementType    = typeof(TAny);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllObject(ReadOnlySpan<char> fieldName, Span<object> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<object>);
        var elementType    = typeof(object);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllObjectNullable(ReadOnlySpan<char> fieldName, Span<object?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<object>);
        var elementType    = typeof(object);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, formatString, formatFlags);
        return stb.AddGoToNext();
    }
    
    public TExt AlwaysAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<bool>);
        var elementType    = typeof(bool);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType,   0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<bool?>);
        var elementType    = typeof(bool?);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TFmt>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<TFmt>);
        var elementType    = typeof(TFmt);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllNullable<TFmt>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmt?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmt : class, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<TFmt>);
        var elementType    = typeof(TFmt);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TStructFmt>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TStructFmt?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TStructFmt : struct, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<TStructFmt>);
        var elementType    = typeof(TStructFmt);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TCloaked, TRevealBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloaked> value
      , PalantírReveal<TRevealBase> palantírReveal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) 
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<TRevealBase>);
        var elementType    = typeof(TRevealBase);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.RevealCloakedBearerOrNull(item, palantírReveal, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TCloakedStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloakedStruct?> value
      , PalantírReveal<TCloakedStruct> palantírReveal, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<TCloakedStruct?>);
        var elementType    = typeof(TCloakedStruct?);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.RevealNullableCloakedBearerOrNull(item, palantírReveal, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TBearer>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearer> value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<TBearer>);
        var elementType    = typeof(TBearer);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.RevealStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TBearerStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearerStruct?> value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<TBearerStruct?>);
        var elementType    = typeof(TBearerStruct?);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.RevealNullableStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<string> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<string>);
        var elementType    = typeof(string);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<string?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<string>);
        var elementType    = typeof(string);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCharSeq> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<TCharSeq>);
        var elementType    = typeof(TCharSeq);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllCharSeqNullable<TCharSeq>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCharSeq?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<TCharSeq>);
        var elementType    = typeof(TCharSeq);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<StringBuilder>);
        var elementType    = typeof(StringBuilder);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<StringBuilder>);
        var elementType    = typeof(StringBuilder);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllMatch<TAny>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TAny> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<TAny>);
        var elementType    = typeof(TAny);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllMatchNullable<TAny>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TAny?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<TAny?>);
        var elementType    = typeof(TAny?);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, null, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType,   null, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllObject(ReadOnlySpan<char> fieldName, ReadOnlySpan<object> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<object>);
        var elementType    = typeof(object);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllObjectNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<object?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<object>);
        var elementType    = typeof(object);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType, formatFlags);
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, value.Length, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll(string fieldName, bool[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;

        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAll(value, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(bool), null, value?.GetType() ?? typeof(bool[]), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(bool),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll(string fieldName, bool?[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAll(value, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(bool?), null, value?.GetType() ?? 
                                                                                  typeof(bool?[]), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(bool?),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TFmt>(string fieldName, TFmt?[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var collectionType = stb.Master.StartSimpleCollectionType(value);
            collectionType.AddAll(value, formatString, formatFlags);
            collectionType.Complete();
        }
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TFmt), null, value?.GetType() ?? typeof(TFmt[]), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TFmt),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TStructFmt>(string fieldName, TStructFmt?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TStructFmt : struct, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAll(value, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TStructFmt?), null, value?.GetType() ?? typeof(TStructFmt?[]), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TStructFmt?),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TCloaked, TRevealBase>
        (string fieldName, TCloaked?[]? value, PalantírReveal<TRevealBase> palantírReveal
          , FieldContentHandling formatFlags = DefaultCallerTypeFlags) 
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealAll(value, palantírReveal, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TCloaked), null, value?.GetType() ?? 
                                                                                     typeof(TCloaked[]), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TCloaked),   null, "", formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TCloakedStruct>
        (string fieldName, TCloakedStruct?[]? value, PalantírReveal<TCloakedStruct> palantírReveal
          , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealAll(value, palantírReveal, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TCloakedStruct?), null, value?.GetType() ?? 
                                                                                           typeof(TCloakedStruct?[]), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TCloakedStruct?),   null, "", formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TBearer>(string fieldName, TBearer?[]? value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealAll(value).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(string), null, value?.GetType() ?? 
                                                                                   typeof(TBearer[]), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(string),   null, "", formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TBearerStruct>(string fieldName, TBearerStruct?[]? value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealAll(value).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(string), null, value?.GetType() ?? 
                                                                                   typeof(TBearerStruct?[]), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(string),   null, "", formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll(string fieldName, string?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAll(value, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(string), null, value?.GetType() ?? 
                                                                                    typeof(string[]), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(string),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllCharSeq<TCharSeq>(string fieldName, TCharSeq?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAllCharSeq(value, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TCharSeq?), null, value?.GetType() ?? 
                                                                                      typeof(TCharSeq[]), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TCharSeq?),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll(string fieldName, StringBuilder?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAll(value, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(StringBuilder), null, value?.GetType() ?? 
                                                                                          typeof(StringBuilder[]), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(StringBuilder),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllMatch<TAny>(string fieldName, TAny?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAllMatch(value, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TAny), null, value?.GetType() ?? 
                                                                                 typeof(TAny[]), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TAny),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllObject(string fieldName, object?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAllMatch(value, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(object), null, value?.GetType() ?? 
                                                                                   typeof(object[]), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(object),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll(string fieldName, IReadOnlyList<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAll(value, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(bool), null, value?.GetType() ?? 
                                                                                  typeof(IReadOnlyList<bool>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(bool),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll(string fieldName, IReadOnlyList<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAll(value, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(bool?), null, value?.GetType() ?? 
                                                                                  typeof(IReadOnlyList<bool?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(bool?),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TFmt>(string fieldName, IReadOnlyList<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAll(value, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TFmt?), null, value?.GetType() ?? 
                                                                                  typeof(IReadOnlyList<TFmt?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TFmt?),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TFmtStruct>(string fieldName, IReadOnlyList<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAll(value, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TFmtStruct?), null, value?.GetType() ?? 
                                                                                        typeof(IReadOnlyList<TFmtStruct?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TFmtStruct?),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TCloaked, TRevealBase>
        (string fieldName, IReadOnlyList<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal
          , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealAll(value, palantírReveal, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TRevealBase), null, value?.GetType() ?? 
                                                                                         typeof(IReadOnlyList<TRevealBase>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TRevealBase),   null, "", formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TCloakedStruct>
        (string fieldName, IReadOnlyList<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal
          , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealAll(value, palantírReveal, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TCloakedStruct), null, value?.GetType() ?? 
                                                                                           typeof(IReadOnlyList<TCloakedStruct>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TCloakedStruct),   null, "", formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TBearer>(string fieldName, IReadOnlyList<TBearer?>? value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealAll(value).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TBearer?), null, value?.GetType() ?? 
                                                                                     typeof(IReadOnlyList<TBearer?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TBearer?),   null, "", formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAll<TBearerStruct>(string fieldName, IReadOnlyList<TBearerStruct?>? value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealAll(value).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TBearerStruct?), null, value?.GetType() ?? 
                                                                                           typeof(IReadOnlyList<TBearerStruct?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TBearerStruct?),   null, "", formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll(string fieldName, IReadOnlyList<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAll(value, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(string), null, value?.GetType() ?? 
                                                                                   typeof(IReadOnlyList<string>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(string),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllCharSeq<TCharSeq>(string fieldName, IReadOnlyList<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAllCharSeq(value, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TCharSeq?), null, value?.GetType() ?? 
                                                                                      typeof(IReadOnlyList<TCharSeq?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TCharSeq?),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll(string fieldName, IReadOnlyList<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAll(value, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(StringBuilder), null, value?.GetType() ?? 
                                                                                          typeof(IReadOnlyList<StringBuilder>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(StringBuilder),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllMatch<TAny>(string fieldName, IReadOnlyList<TAny>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAllMatch(value, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TAny), null, value?.GetType() ?? 
                                                                                 typeof(IReadOnlyList<TAny>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TAny),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllObject(string fieldName, IReadOnlyList<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAllMatch(value, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(object), null, value?.GetType() ?? 
                                                                                   typeof(IReadOnlyList<object>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(object),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerable<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAllEnumerate(value, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(bool), null, value?.GetType() ?? typeof(IEnumerable<bool>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(bool),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerable<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAllEnumerate(value, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(bool?), null, value?.GetType() ?? typeof(IEnumerable<bool?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(bool?),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TFmt>(string fieldName, IEnumerable<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAllEnumerate(value, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TFmt?), null, value?.GetType() ?? 
                                                                                   typeof(IEnumerable<TFmt?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TFmt?),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TFmtStruct>(string fieldName, IEnumerable<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAllEnumerate(value, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TFmtStruct?), null, value?.GetType() ?? 
                                                                                       typeof(IEnumerable<TFmtStruct?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TFmtStruct?),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAllEnumerate<TCloaked, TRevealBase>
        (string fieldName, IEnumerable<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal
          , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealAllEnumerate(value, palantírReveal, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TRevealBase), null, value?.GetType() ?? 
                                                                                         typeof(IEnumerable<TRevealBase>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TRevealBase),   null, "", formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAllEnumerate<TCloakedStruct>
        (string fieldName, IEnumerable<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal
          , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealAllEnumerate(value, palantírReveal, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TCloakedStruct?), null, value?.GetType() ?? 
                                                                                            typeof(IEnumerable<TCloakedStruct?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TCloakedStruct?),   null, "", formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAllEnumerate<TBearer>(string fieldName, IEnumerable<TBearer?>? value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealAllEnumerate(value).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TBearer?), null, value?.GetType() ?? 
                                                                                     typeof(IEnumerable<TBearer?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TBearer?),   null, "", formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAllEnumerate<TBearerStruct>(string fieldName, IEnumerable<TBearerStruct?>? value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealAllEnumerate(value).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TBearerStruct?), null, value?.GetType() ?? 
                                                                                           typeof(IEnumerable<TBearerStruct?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TBearerStruct?),   null, "", formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerable<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAllEnumerate(value, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(string), null, value?.GetType() ?? 
                                                                                   typeof(IEnumerable<string>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(string),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllCharSeqEnumerate<TCharSeq>(string fieldName, IEnumerable<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAllCharSeqEnumerate(value, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TCharSeq), null, value?.GetType() ?? 
                                                                                     typeof(IEnumerable<TCharSeq>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TCharSeq),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerable<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAllEnumerate(value, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(StringBuilder), null, value?.GetType() ?? 
                                                                                          typeof(IEnumerable<StringBuilder>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(StringBuilder),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllMatchEnumerate<TAny>(string fieldName, IEnumerable<TAny>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAllMatchEnumerate(value, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TAny), null, value?.GetType() ?? typeof(IEnumerable<TAny>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TAny),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllObjectEnumerate(string fieldName, IEnumerable<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAllMatchEnumerate(value, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(bool), null, value?.GetType() ?? typeof(IEnumerable<object?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(bool),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerator<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            value!.Reset();
            stb.Master.StartSimpleCollectionType(value).AddAllEnumerate(value, formatString, formatFlags).Complete();
        }
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(bool),  value != null ? false : null, value?.GetType() ?? 
                                                     typeof(IEnumerator<bool>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(bool),  value != null ? 0 : null
                                                 , formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerator<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            value!.Reset();
            stb.Master.StartSimpleCollectionType(value).AddAllEnumerate(value, formatString, formatFlags).Complete();
        }
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(bool?),  value != null ? false : null, value?.GetType() ?? 
                                                     typeof(IEnumerator<bool?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(bool?),  value != null ? 0 : null
                                                 , formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TFmt>(string fieldName, IEnumerator<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            value!.Reset();
            stb.Master.StartSimpleCollectionType(value).AddAllEnumerate(value, formatString, formatFlags).Complete();
        }
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TFmt?),  value != null ? false : null, value?.GetType() ?? 
                                                     typeof(IEnumerator<TFmt?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TFmt?),  value != null ? 0 : null
                                                 , formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TFmtStruct>(string fieldName, IEnumerator<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            value!.Reset();
            stb.Master.StartSimpleCollectionType(value).AddAllEnumerate(value, formatString, formatFlags).Complete();
        }
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TFmtStruct?),  value != null ? false : null, value?.GetType() ?? 
                                                     typeof(IEnumerator<TFmtStruct?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TFmtStruct?),  value != null ? 0 : null
                                                 , formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAllEnumerate<TCloaked, TRevealBase>
        (string fieldName, IEnumerator<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal
          , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            value!.Reset();
            stb.Master.StartSimpleCollectionType(value).RevealAllEnumerate(value, palantírReveal, formatFlags).Complete();
        }
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TCloaked), value != null ? false : null, value?.GetType() ?? 
                                                                                      typeof(IEnumerator<TCloaked>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TCloaked),  value != null ? 0 : null
                                                 , "", formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAllEnumerate<TCloakedStruct>
        (string fieldName, IEnumerator<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal
          , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            value!.Reset();
            stb.Master.StartSimpleCollectionType(value).RevealAllEnumerate(value, palantírReveal, formatFlags).Complete();
        }
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TCloakedStruct), value != null ? false : null, value?.GetType() ?? 
                                                                                            typeof(IEnumerator<TCloakedStruct>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TCloakedStruct),  value != null ? 0 : null
                                                 , "", formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAllEnumerate<TBearer>(string fieldName, IEnumerator<TBearer?>? value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            value!.Reset();
            stb.Master.StartSimpleCollectionType(value).RevealAllEnumerate(value).Complete();
        }
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TBearer), value != null ? false : null, value?.GetType() ?? 
                                                                                     typeof(IEnumerator<TBearer>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TBearer),  value != null ? 0 : null
                                                 , "", formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealAllEnumerate<TBearerStruct>(string fieldName, IEnumerator<TBearerStruct?>? value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            value!.Reset();
            stb.Master.StartSimpleCollectionType(value).RevealAllEnumerate(value).Complete();
        }
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TBearerStruct?),  value != null ? false : null, value?.GetType() ?? 
                                                                                           typeof(IEnumerator<TBearerStruct?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TBearerStruct?),  value != null ? 0 : null
                                                 , "", formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerator<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            value!.Reset();
            stb.Master.StartSimpleCollectionType(value).AddAllEnumerate(value, formatString, formatFlags).Complete();
        }
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(string), value != null ? false : null, value?.GetType() ?? 
                                                                                            typeof(IEnumerator<string?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(string),  value != null ? 0 : null
                                                 , formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllCharSeqEnumerate<TCharSeq>(string fieldName, IEnumerator<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            value!.Reset();
            stb.Master.StartSimpleCollectionType(value).AddAllCharSeqEnumerate(value, formatString, formatFlags).Complete();
        }
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TCharSeq?), value != null ? false : null, value?.GetType() ?? 
                                                                                       typeof(IEnumerator<TCharSeq?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TCharSeq?),  value != null ? 0 : null
                                                 , formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerator<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            value!.Reset();
            stb.Master.StartSimpleCollectionType(value).AddAllEnumerate(value, formatString, formatFlags).Complete();
        }
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(StringBuilder), value != null ? false : null, value?.GetType() ?? 
                                                                                           typeof(IEnumerator<StringBuilder>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(StringBuilder),  value != null ? 0 : null
                                                 , formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllMatchEnumerate<TAny>(string fieldName, IEnumerator<TAny>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            value!.Reset();
            stb.Master.StartSimpleCollectionType(value).AddAllMatchEnumerate(value, formatString, formatFlags).Complete();
        }
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TAny), value != null ? false : null, value?.GetType() ?? 
                                                                                  typeof(IEnumerator<TAny>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TAny),  value != null ? 0 : null
                                                 , formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllObjectEnumerate(string fieldName, IEnumerator<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            value!.Reset();
            stb.Master.StartSimpleCollectionType(value).AddAllMatchEnumerate(value, formatString, formatFlags).Complete();
        }
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(object), value != null ? false : null, value?.GetType() ?? 
                                                                                    typeof(IEnumerator<object>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(object),  value != null ? 0 : null
                                                 , formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }
}
