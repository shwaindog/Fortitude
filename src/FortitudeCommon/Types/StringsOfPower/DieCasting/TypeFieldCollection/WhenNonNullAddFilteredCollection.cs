// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldCollection;

public partial class SelectTypeCollectionField<TExt> where TExt : TypeMolder
{
    
    public TExt WhenNonNullAddFiltered(ReadOnlySpan<char> fieldName, Span<bool> value, OrderedCollectionPredicate<bool> filterPredicate)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(Span<bool>);
        var elementType    = typeof(bool);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, true, collectionType);
                }
                stb.AppendCollectionItem(item, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFiltered(ReadOnlySpan<char> fieldName, Span<bool?> value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(Span<bool?>);
        var elementType    = typeof(bool?);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, true, collectionType);
                }
                stb.AppendCollectionItem(item, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFiltered<TFmt>
    (ReadOnlySpan<char> fieldName, Span<TFmt> value, OrderedCollectionPredicate<TFmt> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(Span<TFmt>);
        var elementType    = typeof(TFmt);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFilteredNullable<TFmt>
    (ReadOnlySpan<char> fieldName, Span<TFmt?> value, OrderedCollectionPredicate<TFmt> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : class, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(Span<TFmt>);
        var elementType    = typeof(TFmt);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFiltered<TFmtStruct>
    (ReadOnlySpan<char> fieldName, Span<TFmtStruct?> value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(Span<TFmtStruct?>);
        var elementType    = typeof(TFmtStruct?);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFilteredReveal<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    (ReadOnlySpan<char> fieldName, Span<TCloaked> value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TCloakedRevealBase> palantírReveal) where TCloaked : TCloakedFilterBase, TCloakedRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(Span<TCloaked>);
        var elementType    = typeof(TCloaked);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendOrNull(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFilteredRevealNullable<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    (ReadOnlySpan<char> fieldName, Span<TCloaked?> value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TCloakedRevealBase> palantírReveal) where TCloaked : class, TCloakedFilterBase, TCloakedRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(Span<TCloaked>);
        var elementType    = typeof(TCloaked);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendOrNull(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFilteredReveal<TCloakedStruct>(ReadOnlySpan<char> fieldName, Span<TCloakedStruct?> value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(Span<TCloakedStruct?>);
        var elementType    = typeof(TCloakedStruct?);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendOrNull(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFilteredReveal<TBearer, TBearerBase>(ReadOnlySpan<char> fieldName, Span<TBearer> value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate) where TBearer : IStringBearer, TBearerBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(Span<TBearer>);
        var elementType    = typeof(TBearer);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendRevealBearerOrNull(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFilteredRevealNullable<TBearer, TBearerBase>(ReadOnlySpan<char> fieldName, Span<TBearer?> value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate) where TBearer : class, IStringBearer, TBearerBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(Span<TBearer>);
        var elementType    = typeof(TBearer);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendRevealBearerOrNull(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFilteredReveal<TBearerStruct>(ReadOnlySpan<char> fieldName, Span<TBearerStruct?> value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate) where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(Span<TBearerStruct?>);
        var elementType    = typeof(TBearerStruct?);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendRevealBearerOrNull(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFiltered(ReadOnlySpan<char> fieldName, Span<string> value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(Span<string>);
        var elementType    = typeof(string);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFilteredNullable(ReadOnlySpan<char> fieldName, Span<string?> value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(Span<string>);
        var elementType    = typeof(string);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFilteredCharSeq<TCharSeq, TCharSeqBase>(ReadOnlySpan<char> fieldName, Span<TCharSeq> value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TCharSeq : ICharSequence, TCharSeqBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(Span<TCharSeq>);
        var elementType    = typeof(TCharSeq);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFilteredCharSeqNullable<TCharSeq, TCharSeqBase>(ReadOnlySpan<char> fieldName, Span<TCharSeq?> value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TCharSeq : ICharSequence, TCharSeqBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(Span<TCharSeq>);
        var elementType    = typeof(TCharSeq);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFiltered(ReadOnlySpan<char> fieldName, Span<StringBuilder> value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(Span<StringBuilder>);
        var elementType    = typeof(StringBuilder);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFilteredNullable(ReadOnlySpan<char> fieldName, Span<StringBuilder?> value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(Span<StringBuilder>);
        var elementType    = typeof(StringBuilder);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFilteredMatch<T, TBase>(ReadOnlySpan<char> fieldName, Span<T> value, OrderedCollectionPredicate<TBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : TBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(Span<T>);
        var elementType    = typeof(T);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFilteredMatchNullable<T, TBase>(ReadOnlySpan<char> fieldName, Span<T?> value
      , OrderedCollectionPredicate<TBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : TBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(Span<T>);
        var elementType    = typeof(T);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TExt WhenNonNullAddFilteredObject(ReadOnlySpan<char> fieldName, Span<object> value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        WhenNonNullAddFilteredMatch(fieldName, value, filterPredicate, formatString);

    [CallsObjectToString]
    public TExt WhenNonNullAddFilteredObjectNullable(ReadOnlySpan<char> fieldName, Span<object?> value
      , OrderedCollectionPredicate<object?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        WhenNonNullAddFilteredMatch(fieldName, value, filterPredicate, formatString);

    public TExt WhenNonNullAddFiltered(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool> value, OrderedCollectionPredicate<bool> filterPredicate)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(ReadOnlySpan<bool>);
        var elementType    = typeof(bool);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, true, collectionType);
                }
                stb.AppendCollectionItem(item, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFiltered(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool?> value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(ReadOnlySpan<bool?>);
        var elementType    = typeof(bool?);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, true, collectionType);
                }
                stb.AppendCollectionItem(item, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFiltered<TFmt>
    (ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmt> value, OrderedCollectionPredicate<TFmt> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(ReadOnlySpan<TFmt>);
        var elementType    = typeof(TFmt);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFilteredNullable<TFmt>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmt?> value
      , OrderedCollectionPredicate<TFmt> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : class, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(ReadOnlySpan<TFmt>);
        var elementType    = typeof(TFmt);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFiltered<TFmtStruct>
    (ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmtStruct?> value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(ReadOnlySpan<TFmtStruct?>);
        var elementType    = typeof(TFmtStruct?);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFilteredReveal<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    (ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloaked> value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TCloakedRevealBase> palantírReveal) where TCloaked : TCloakedFilterBase, TCloakedRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(ReadOnlySpan<TCloaked>);
        var elementType    = typeof(TCloaked);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendOrNull(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFilteredRevealNullable<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    (ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloaked?> value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TCloakedRevealBase> palantírReveal) where TCloaked : TCloakedFilterBase, TCloakedRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(ReadOnlySpan<TCloaked>);
        var elementType    = typeof(TCloaked);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendOrNull(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFilteredReveal<TCloakedStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloakedStruct?> value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(ReadOnlySpan<TCloakedStruct?>);
        var elementType    = typeof(TCloakedStruct?);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendOrNull(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFilteredReveal<TBearer, TBearerBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearer> value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate) where TBearer : IStringBearer, TBearerBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(ReadOnlySpan<TBearer>);
        var elementType    = typeof(TBearer);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendRevealBearerOrNull(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFilteredRevealNullable<TBearer, TBearerBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearer?> value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate) where TBearer : class, IStringBearer, TBearerBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(ReadOnlySpan<TBearer>);
        var elementType    = typeof(TBearer);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendRevealBearerOrNull(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFilteredReveal<TBearerStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearerStruct?> value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate) where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(ReadOnlySpan<TBearerStruct?>);
        var elementType    = typeof(TBearerStruct?);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendRevealBearerOrNull(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFiltered(ReadOnlySpan<char> fieldName, ReadOnlySpan<string> value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(ReadOnlySpan<string>);
        var elementType    = typeof(string);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFilteredNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<string?> value
      , OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(ReadOnlySpan<string>);
        var elementType    = typeof(string);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFilteredCharSeq<TCharSeq, TCharSeqBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCharSeq> value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TCharSeq : ICharSequence, TCharSeqBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(ReadOnlySpan<TCharSeq>);
        var elementType    = typeof(TCharSeq);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFilteredCharSeqNullable<TCharSeq, TCharSeqBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCharSeq?> value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TCharSeq : ICharSequence, TCharSeqBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(ReadOnlySpan<TCharSeq>);
        var elementType    = typeof(TCharSeq);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFiltered(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder> value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(ReadOnlySpan<StringBuilder>);
        var elementType    = typeof(StringBuilder);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFilteredNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder?> value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(ReadOnlySpan<StringBuilder>);
        var elementType    = typeof(StringBuilder);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFilteredMatch<T, TBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<T> value
      , OrderedCollectionPredicate<TBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : TBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(ReadOnlySpan<T>);
        var elementType    = typeof(T);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenNonNullAddFilteredMatchNullable<T, TBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<T?> value
      , OrderedCollectionPredicate<TBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : TBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(ReadOnlySpan<T>);
        var elementType    = typeof(T);

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TExt WhenNonNullAddFilteredObject(ReadOnlySpan<char> fieldName, ReadOnlySpan<object> value
      , OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        WhenNonNullAddFilteredMatch(fieldName, value, filterPredicate, formatString);


    [CallsObjectToString]
    public TExt WhenNonNullAddFilteredObjectNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<object?> value
      , OrderedCollectionPredicate<object?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        WhenNonNullAddFilteredMatch(fieldName, value, filterPredicate, formatString);
    
    public TExt WhenNonNullAddFiltered(string fieldName, bool[]? value, OrderedCollectionPredicate<bool> filterPredicate) =>
        !stb.SkipFields && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered(string fieldName, bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate) =>
        !stb.SkipFields && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TFmt>
    (string fieldName, TFmt?[]? value, OrderedCollectionPredicate<TFmt> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipFields && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TFmtStruct>
    (string fieldName, TFmtStruct?[]? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealFiltered<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    (string fieldName, TCloaked?[]? value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TCloakedRevealBase> palantírReveal) where TCloaked : TCloakedFilterBase, TCloakedRevealBase =>
        !stb.SkipFields && value != null ? AlwaysRevealFiltered(fieldName, value, filterPredicate, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealFiltered<TCloakedStruct>(string fieldName, TCloakedStruct?[]? value, OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct  =>
        !stb.SkipFields && value != null ? AlwaysRevealFiltered(fieldName, value, filterPredicate, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealFiltered<TBearer, TBearerBase>(string fieldName, TBearer?[]? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate) where TBearer : IStringBearer, TBearerBase =>
        !stb.SkipFields && value != null ? AlwaysRevealFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealFiltered<TBearerStruct>(string fieldName, TBearerStruct?[]? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate) where TBearerStruct : struct, IStringBearer =>
        !stb.SkipFields && value != null ? AlwaysRevealFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered
    (string fieldName, string?[]? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredCharSeq<TCharSeq, TCharSeqBase>
        (string fieldName, TCharSeq?[]? value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence, TCharSeqBase =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredCharSeq(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered
        (string fieldName, StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;
    
    public TExt WhenNonNullAddFilteredMatch<T, TBase>
    (string fieldName, T?[]? value, OrderedCollectionPredicate<TBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : TBase =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;
    
    [CallsObjectToString]
    public TExt WhenNonNullAddFilteredObject
    (string fieldName, object?[]? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered(string fieldName, IReadOnlyList<bool>? value, OrderedCollectionPredicate<bool> filterPredicate) =>
        !stb.SkipFields && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered(string fieldName, IReadOnlyList<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate) =>
        !stb.SkipFields && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TFmt>
    (string fieldName, IReadOnlyList<TFmt?>? value, OrderedCollectionPredicate<TFmt> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipFields && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TFmtStruct>
    (string fieldName, IReadOnlyList<TFmtStruct?>? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealFiltered<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    (string fieldName, IReadOnlyList<TCloaked?>? value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TCloakedRevealBase> palantírReveal) where TCloaked : TCloakedFilterBase, TCloakedRevealBase =>
        !stb.SkipFields && value != null ? AlwaysRevealFiltered(fieldName, value, filterPredicate, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealFiltered<TCloakedStruct>(string fieldName, IReadOnlyList<TCloakedStruct?>? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct  =>
        !stb.SkipFields && value != null ? AlwaysRevealFiltered(fieldName, value, filterPredicate, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealFiltered<TBearer, TBearerBase>(string fieldName, IReadOnlyList<TBearer?>? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate) where TBearer : IStringBearer, TBearerBase =>
        !stb.SkipFields && value != null ? AlwaysRevealFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealFiltered<TBearerStruct>(string fieldName, IReadOnlyList<TBearerStruct?>? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate) where TBearerStruct : struct, IStringBearer =>
        !stb.SkipFields && value != null ? AlwaysRevealFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered
    (string fieldName, IReadOnlyList<string?>? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredCharSeq<TCharSeq, TCharSeqBase>
        (string fieldName, IReadOnlyList<TCharSeq?>? value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TCharSeq : ICharSequence, TCharSeqBase =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredCharSeq(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered
        (string fieldName, IReadOnlyList<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder> filterPredicate) =>
        !stb.SkipFields && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredMatch<T, TBase>
    (string fieldName, IReadOnlyList<T?>? value, OrderedCollectionPredicate<TBase?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : TBase =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    [CallsObjectToString]
    public TExt WhenNonNullAddFilteredObject
    (string fieldName, IReadOnlyList<object?>? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredEnumerate(string fieldName, IEnumerable<bool>? value, OrderedCollectionPredicate<bool> filterPredicate) =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredEnumerate(string fieldName, IEnumerable<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate) =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredEnumerate<TFmt, TFmtBase>
    (string fieldName, IEnumerable<TFmt?>? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable, TFmtBase =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredEnumerate<TFmtStruct>
    (string fieldName, IEnumerable<TFmtStruct?>? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealFilteredEnumerate<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    (string fieldName, IEnumerable<TCloaked?>? value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TCloakedRevealBase> palantírReveal) where TCloaked : TCloakedFilterBase, TCloakedRevealBase =>
        !stb.SkipFields && value != null ? AlwaysRevealFilteredEnumerate(fieldName, value, filterPredicate, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealFilteredEnumerate<TCloakedStruct>(string fieldName, IEnumerable<TCloakedStruct?>? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct  =>
        !stb.SkipFields && value != null ? AlwaysRevealFilteredEnumerate(fieldName, value, filterPredicate, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealFilteredEnumerate<TBearer, TBearerBase>(string fieldName, IEnumerable<TBearer?>? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate) where TBearer : IStringBearer, TBearerBase =>
        !stb.SkipFields && value != null ? AlwaysRevealFilteredEnumerate(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealFilteredEnumerate<TBearerStruct>(string fieldName, IEnumerable<TBearerStruct?>? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate) where TBearerStruct : struct, IStringBearer =>
        !stb.SkipFields && value != null ? AlwaysRevealFilteredEnumerate(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredEnumerate
    (string fieldName, IEnumerable<string?>? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredCharSeqEnumerate<TCharSeq, TCharSeqBase>
        (string fieldName, IEnumerable<TCharSeq?>? value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TCharSeq : ICharSequence, TCharSeqBase =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredCharSeqEnumerate(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredEnumerate
        (string fieldName, IEnumerable<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredMatchEnumerate<T, TBase>
    (string fieldName, IEnumerable<T?>? value, OrderedCollectionPredicate<TBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : TBase =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredMatchEnumerate(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    [CallsObjectToString]
    public TExt WhenNonNullAddFilteredObjectEnumerate
    (string fieldName, IEnumerable<object?>? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredMatchEnumerate(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredEnumerate(string fieldName, IEnumerator<bool>? value, OrderedCollectionPredicate<bool> filterPredicate) =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredEnumerate(string fieldName, IEnumerator<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate) =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredEnumerate<TFmt, TFmtBase>
    (string fieldName, IEnumerator<TFmt?>? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable, TFmtBase =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredEnumerate<TFmtStruct>
    (string fieldName, IEnumerator<TFmtStruct?>? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealFilteredEnumerate<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    (string fieldName, IEnumerator<TCloaked?>? value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TCloakedRevealBase> palantírReveal) where TCloaked : TCloakedFilterBase, TCloakedRevealBase =>
        !stb.SkipFields && value != null ? AlwaysRevealFilteredEnumerate(fieldName, value, filterPredicate, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealFilteredEnumerate<TCloakedStruct>(string fieldName, IEnumerator<TCloakedStruct?>? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct  =>
        !stb.SkipFields && value != null ? AlwaysRevealFilteredEnumerate(fieldName, value, filterPredicate, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealFilteredEnumerate<TBearer, TBearerBase>(string fieldName, IEnumerator<TBearer?>? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate) where TBearer : IStringBearer, TBearerBase =>
        !stb.SkipFields && value != null ? AlwaysRevealFilteredEnumerate(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullRevealFilteredEnumerate<TBearerStruct>(string fieldName, IEnumerator<TBearerStruct?>? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate) where TBearerStruct : struct, IStringBearer =>
        !stb.SkipFields && value != null ? AlwaysRevealFilteredEnumerate(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredEnumerate
    (string fieldName, IEnumerator<string?>? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredCharSeqEnumerate<TCharSeq, TCharSeqBase>
        (string fieldName, IEnumerator<TCharSeq?>? value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TCharSeq : ICharSequence, TCharSeqBase =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredCharSeqEnumerate(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredEnumerate
        (string fieldName, IEnumerator<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredMatchEnumerate<T, TBase>
    (string fieldName, IEnumerator<T?>? value, OrderedCollectionPredicate<TBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : TBase =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredMatchEnumerate(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    [CallsObjectToString]
    public TExt WhenNonNullAddFilteredObjectEnumerate
    (string fieldName, IEnumerator<object?>? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredMatchEnumerate(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;
}
