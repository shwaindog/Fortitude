﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeOrderedCollection;
using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldCollection;

#pragma warning disable CS0618 // Type or member is obsolete
public partial class SelectTypeCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenPopulatedWithFilter(ReadOnlySpan<char> fieldName, Span<bool> value, OrderedCollectionPredicate<bool> filterPredicate)
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, true, collectionType);
                }
                stb.AppendCollectionItem(item, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(ReadOnlySpan<char> fieldName, Span<bool?> value, OrderedCollectionPredicate<bool?> filterPredicate)
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, true, collectionType);
                }
                stb.AppendCollectionItem(item, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TFmt>
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterNullable<TFmt>
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TFmtStruct>
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterReveal<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendOrNull(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterRevealNullable<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendOrNull(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterReveal<TCloakedStruct>(ReadOnlySpan<char> fieldName, Span<TCloakedStruct?> value
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendOrNull(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterReveal<TBearer, TBearerBase>(ReadOnlySpan<char> fieldName, Span<TBearer> value
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendRevealBearerOrNull(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterRevealNullable<TBearer, TBearerBase>(ReadOnlySpan<char> fieldName, Span<TBearer?> value
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendRevealBearerOrNull(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterReveal<TBearerStruct>(ReadOnlySpan<char> fieldName, Span<TBearerStruct?> value
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendRevealBearerOrNull(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(ReadOnlySpan<char> fieldName, Span<string> value, OrderedCollectionPredicate<string> filterPredicate
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterNullable(ReadOnlySpan<char> fieldName, Span<string?> value, OrderedCollectionPredicate<string> filterPredicate
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterCharSeq<TCharSeq, TCharSeqBase>(ReadOnlySpan<char> fieldName, Span<TCharSeq> value
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterCharSeqNullable<TCharSeq, TCharSeqBase>(ReadOnlySpan<char> fieldName, Span<TCharSeq?> value
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(ReadOnlySpan<char> fieldName, Span<StringBuilder> value
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterNullable(ReadOnlySpan<char> fieldName, Span<StringBuilder?> value
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterMatch<TAny, TAnyBase>(ReadOnlySpan<char> fieldName, Span<TAny> value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TAny : TAnyBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(Span<TAny>);
        var elementType    = typeof(TAny);

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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterMatchNullable<TAny, TAnyBase>(ReadOnlySpan<char> fieldName, Span<TAny?> value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TAny : TAnyBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(Span<TAny>);
        var elementType    = typeof(TAny);

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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TExt WhenPopulatedWithFilterObject(ReadOnlySpan<char> fieldName, Span<object> value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        WhenPopulatedWithFilterMatch(fieldName, value, filterPredicate, formatString);

    [CallsObjectToString]
    public TExt WhenPopulatedWithFilterObjectNullable(ReadOnlySpan<char> fieldName, Span<object?> value
      , OrderedCollectionPredicate<object?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        WhenPopulatedWithFilterMatch(fieldName, value, filterPredicate, formatString);

    public TExt WhenPopulatedWithFilter(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool> value, OrderedCollectionPredicate<bool> filterPredicate)
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, true, collectionType);
                }
                stb.AppendCollectionItem(item, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool?> value, OrderedCollectionPredicate<bool?> filterPredicate)
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, true, collectionType);
                }
                stb.AppendCollectionItem(item, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TFmt>
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterNullable<TFmt>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmt?> value
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TFmtStruct>
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterReveal<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendOrNull(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterRevealNullable<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendOrNull(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterReveal<TCloakedStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloakedStruct?> value
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendOrNull(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterReveal<TBearer, TBearerBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearer> value
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendRevealBearerOrNull(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterRevealNullable<TBearer, TBearerBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearer?> value
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendRevealBearerOrNull(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterReveal<TBearerStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearerStruct?> value
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendRevealBearerOrNull(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(ReadOnlySpan<char> fieldName, ReadOnlySpan<string> value, OrderedCollectionPredicate<string> filterPredicate
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<string?> value
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterCharSeq<TCharSeq, TCharSeqBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCharSeq> value
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterCharSeqNullable<TCharSeq, TCharSeqBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCharSeq?> value
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder> value
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder?> value
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterMatch<TAny, TAnyBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TAny> value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TAny : TAnyBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(ReadOnlySpan<TAny>);
        var elementType    = typeof(TAny);

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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterMatchNullable<TAny, TAnyBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TAny?> value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TAny : TAnyBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var collectionType = typeof(ReadOnlySpan<TAny>);
        var elementType    = typeof(TAny);

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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (matchedItems != 0)
        {
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TExt WhenPopulatedWithFilterObject(ReadOnlySpan<char> fieldName, ReadOnlySpan<object> value
      , OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        WhenPopulatedWithFilterMatch(fieldName, value, filterPredicate, formatString);


    [CallsObjectToString]
    public TExt WhenPopulatedWithFilterObjectNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<object?> value
      , OrderedCollectionPredicate<object?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        WhenPopulatedWithFilterMatch(fieldName, value, filterPredicate, formatString);

    public TExt WhenPopulatedWithFilter(string fieldName, bool[]? value, OrderedCollectionPredicate<bool> filterPredicate)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<bool>? eocm = null;
        if (value != null)
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
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<bool[], bool>(value);
                }
                eocm.AddElementAndGoToNextElement(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(string fieldName, bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<bool?>? eocm = null;
        if (value != null)
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
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<bool?[], bool?>(value);
                }
                eocm.AddElementAndGoToNextElement(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TFmt>
    (string fieldName, TFmt?[]? value, OrderedCollectionPredicate<TFmt> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TFmt>? eocm = null;
        if (value != null)
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
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<TFmt>(value);
                }
                eocm.AddElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TFmtStruct>
    (string fieldName, TFmtStruct?[]? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TFmtStruct?>? eocm = null;
        if (value != null)
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
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<TFmtStruct?>(value);
                }
                eocm.AddElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterReveal<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    (string fieldName, TCloaked?[]? value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TCloakedRevealBase> palantírReveal) where TCloaked : TCloakedFilterBase, TCloakedRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TCloaked>? eocm = null;
        if (value != null)
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
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<TCloaked>(value);
                }
                eocm.AddElementAndGoToNextElement(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterReveal<TCloakedStruct>
    (string fieldName, TCloakedStruct?[]? value, OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TCloakedStruct>? eocm = null;
        if (value != null)
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
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<TCloakedStruct>(value);
                }
                eocm.AddElementAndGoToNextElement(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterReveal<TBearer, TBearerBase>(string fieldName, TBearer?[]? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate)
        where TBearer : IStringBearer, TBearerBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TBearer>? eocm = null;
        if (value != null)
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
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<TBearer>(value);
                }
                eocm.AddBearerElementAndGoToNextElement(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterReveal<TBearerStruct>(string fieldName, TBearerStruct?[]? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TBearerStruct>? eocm = null;
        if (value != null)
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
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<TBearerStruct>(value);
                }
                eocm.AddBearerElementAndGoToNextElement(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter
    (string fieldName, string?[]? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<string?>? eocm = null;
        if (value != null)
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
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<string?[], string?>(value);
                }
                eocm.AddElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterCharSeq<TCharSeq, TCharSeqFilterBase>
    (string fieldName, TCharSeq?[]? value, OrderedCollectionPredicate<TCharSeqFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TCharSeq : ICharSequence, TCharSeqFilterBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TCharSeq?>? eocm = null;
        if (value != null)
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
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<TCharSeq?[], TCharSeq?>(value);
                }
                eocm.AddCharSequenceElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(string fieldName, StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<StringBuilder?>? eocm = null;
        if (value != null)
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
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<StringBuilder?[], StringBuilder?>(value);
                }
                eocm.AddElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterMatch<TAny, TAnyBase>(string fieldName, TAny?[]? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TAny : TAnyBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TAny>? eocm = null;
        if (value != null)
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
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<TAny>(value);
                }
                eocm.AddMatchElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TExt WhenPopulatedWithFilterObject(string fieldName, object?[]? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        WhenPopulatedWithFilterMatch(fieldName, value, filterPredicate, formatString);


    public TExt WhenPopulatedWithFilter(string fieldName, IReadOnlyList<bool>? value, OrderedCollectionPredicate<bool> filterPredicate)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<bool?>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
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
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<bool?>(value);
                }
                eocm.AddElementAndGoToNextElement(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(string fieldName, IReadOnlyList<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<bool?>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
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
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<bool?>(value);
                }
                eocm.AddElementAndGoToNextElement(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TFmt>(string fieldName, IReadOnlyList<TFmt?>? value, OrderedCollectionPredicate<TFmt> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        
        ExplicitOrderedCollectionMold<TFmt>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
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
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<TFmt>(value);
                }
                eocm.AddElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TFmtStruct>(string fieldName, IReadOnlyList<TFmtStruct?>? value
      , OrderedCollectionPredicate<TFmtStruct?> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TFmtStruct?>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
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
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<TFmtStruct?>(value);
                }
                eocm.AddElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterReveal<TCloaked, TCloakedFilterBase, TToStyleBase>
    (string fieldName, IReadOnlyList<TCloaked>? value, OrderedCollectionPredicate<TToStyleBase> filterPredicate
      , PalantírReveal<TCloakedFilterBase> palantírReveal) where TCloaked : TCloakedFilterBase, TToStyleBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TCloaked>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
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
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<TCloaked>(value);
                }
                eocm.AddElementAndGoToNextElement(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterReveal<TCloakedStruct>
    (string fieldName, IReadOnlyList<TCloakedStruct?>? value, OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TCloakedStruct>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
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
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<TCloakedStruct>(value);
                }
                eocm.AddElementAndGoToNextElement(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterReveal<TBearer, TBearerBase>(string fieldName, IReadOnlyList<TBearer?>? value
      , OrderedCollectionPredicate<TBearerBase?> filterPredicate)
        where TBearer : IStringBearer, TBearerBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TBearer>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
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
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<TBearer>(value);
                }
                eocm.AddBearerElementAndGoToNextElement(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterReveal<TBearerStruct>(string fieldName, IReadOnlyList<TBearerStruct?>? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TBearerStruct>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
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
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<TBearerStruct>(value);
                }
                eocm.AddBearerElementAndGoToNextElement(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(string fieldName, IReadOnlyList<string?>? value, OrderedCollectionPredicate<string?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<string?>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
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
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<string?>(value);
                }
                eocm.AddElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterCharSeq<TCharSeq, TCharSeqFilterBase>(string fieldName, IReadOnlyList<TCharSeq?>? value
      , OrderedCollectionPredicate<TCharSeqFilterBase?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence, TCharSeqFilterBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TCharSeq>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
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
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<TCharSeq>(value);
                }
                eocm.AddCharSequenceElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(string fieldName, IReadOnlyList<StringBuilder?>? value
      , OrderedCollectionPredicate<StringBuilder?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<StringBuilder?>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
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
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<StringBuilder?>(value);
                }
                eocm.AddElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterMatch<TAny, TAnyBase>(string fieldName, IReadOnlyList<TAny?>? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TAny : TAnyBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TAny>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
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
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<TAny>(value);
                }
                eocm.AddMatchElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TExt WhenPopulatedWithFilterObject(string fieldName, IReadOnlyList<object?>? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        WhenPopulatedWithFilterMatch(fieldName, value, filterPredicate, formatString);
    
    public TExt WhenPopulatedWithFilterEnumerate(string fieldName, IEnumerable<bool>? value, OrderedCollectionPredicate<bool> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool);
        var itemCount   = 0;
        
        ExplicitOrderedCollectionMold<bool>? eocm = null;
        if (value != null)
        {
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate(count, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<bool>(value);
                }
                stb.AppendCollectionItem(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterEnumerate(string fieldName, IEnumerable<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool?);
        var itemCount   = 0;
        
        ExplicitOrderedCollectionMold<bool?>? eocm = null;
        if (value != null)
        {
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate(count, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<bool?>(value);
                }
                stb.AppendCollectionItem(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterEnumerate<TFmt, TFmtBase>(string fieldName, IEnumerable<TFmt?>? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable, TFmtBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmt);
        var itemCount   = 0;
        
        ExplicitOrderedCollectionMold<TFmt>? eocm = null;
        if (value != null)
        {
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate(count, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<TFmt>(value);
                }
                stb.AppendFormattedCollectionItem(item, itemCount, formatString ?? "");
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterEnumerate<TFmtStruct>(string fieldName, IEnumerable<TFmtStruct?>? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmtStruct?);
        var itemCount   = 0;
        
        ExplicitOrderedCollectionMold<TFmtStruct?>? eocm = null;
        if (value != null)
        {
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate(count, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<TFmtStruct?>(value);
                }
                stb.AppendFormattedCollectionItem(item, itemCount, formatString ?? "");
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }
    
    
    public TExt WhenPopulatedWithFilterRevealEnumerate<TCloaked, TCloakedFilterBase, TCloakedRevealBase>(string fieldName, IEnumerable<TCloaked?>? value
      , OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate, PalantírReveal<TCloakedRevealBase> palantírReveal)
        where TCloaked : TCloakedFilterBase, TCloakedRevealBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCloaked);
        var itemCount   = 0;
        
        ExplicitOrderedCollectionMold<TCloaked>? eocm = null;
        if (value != null)
        {
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate(count, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<TCloaked>(value);
                }
                stb.AppendOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }
    
    public TExt WhenPopulatedWithFilterRevealEnumerate<TCloakedStruct>(string fieldName, IEnumerable<TCloakedStruct?>? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate, PalantírReveal<TCloakedStruct> palantírReveal)
        where TCloakedStruct : struct
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCloakedStruct);
        var itemCount   = 0;
        
        ExplicitOrderedCollectionMold<TCloakedStruct>? eocm = null;
        if (value != null)
        {
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate(count, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<TCloakedStruct>(value);
                }
                stb.AppendOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }
    

    public TExt WhenPopulatedWithFilterRevealEnumerate<TBearer, TBearerBase>(string fieldName, IEnumerable<TBearer?>? value, OrderedCollectionPredicate<TBearerBase> filterPredicate)
        where TBearer : IStringBearer, TBearerBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TBearer);
        var itemCount   = 0;
        
        ExplicitOrderedCollectionMold<TBearer>? eocm = null;
        if (value != null)
        {
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate(count, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<TBearer>(value);
                }
                stb.AppendRevealBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterRevealEnumerate<TBearerStruct>(string fieldName, IEnumerable<TBearerStruct?>? value, OrderedCollectionPredicate<TBearerStruct?> filterPredicate)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TBearerStruct);
        var itemCount   = 0;
        
        ExplicitOrderedCollectionMold<TBearerStruct>? eocm = null;
        if (value != null)
        {
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate(count, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<TBearerStruct>(value);
                }
                stb.AppendRevealBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }
    
    public TExt WhenPopulatedWithFilterEnumerate(string fieldName, IEnumerable<string?>? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(string);
        var itemCount   = 0;
        
        ExplicitOrderedCollectionMold<string>? eocm = null;
        if (value != null)
        {
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate(count, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<string>(value);
                }
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterCharSeqEnumerate<TCharSeq, TCharSeqBase>(string fieldName, IEnumerable<TCharSeq?>? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TCharSeq : ICharSequence, TCharSeqBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCharSeq);
        var itemCount   = 0;
        
        ExplicitOrderedCollectionMold<StringBuilder>? eocm = null;
        if (value != null)
        {
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate(count, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<StringBuilder>(value);
                }
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterEnumerate(string fieldName, IEnumerable<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(StringBuilder);
        var itemCount   = 0;

        ExplicitOrderedCollectionMold<StringBuilder>? eocm = null;
        if (value != null)
        {
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate(count, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<StringBuilder>(value);
                }
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }


    public TExt WhenPopulatedWithFilterMatchEnumerate<TAny, TAnyBase>(string fieldName, IEnumerable<TAny?>? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TAny : TAnyBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;

        var elementType = typeof(TAny);
        var itemCount   = 0;

        ExplicitOrderedCollectionMold<TAny>? eocm = null;
        if (value != null)
        {
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate(count, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<TAny>(value);
                }
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemMatchOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemMatchOrNull(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterObjectEnumerate(string fieldName, IEnumerable<object?>? value
      , OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;

        var elementType = typeof(object);
        var itemCount   = 0;

        ExplicitOrderedCollectionMold<object>? eocm = null;
        if (value != null)
        {
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate(count, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<object>(value);
                }
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemMatchOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemMatchOrNull(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterEnumerate(string fieldName, IEnumerator<bool>? value, OrderedCollectionPredicate<bool> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool);
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        
        ExplicitOrderedCollectionMold<bool>? eocm = null;
        if (hasValue)
        {
            var count     = 0;
            var skipCount = 0;
            while (hasValue)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue  = value!.MoveNext();
                    continue;
                }
                var item         = value!.Current;
                var filterResult = filterPredicate(count, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        hasValue  = value.MoveNext();
                        continue;
                    }
                    break;
                }
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<bool>(value);
                }
                stb.AppendCollectionItem(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterEnumerate(string fieldName, IEnumerator<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool?);
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        
        ExplicitOrderedCollectionMold<bool?>? eocm = null;
        if (hasValue)
        {
            var count     = 0;
            var skipCount = 0;
            while (hasValue)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue  = value!.MoveNext();
                    continue;
                }
                var item         = value!.Current;
                var filterResult = filterPredicate(count, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        hasValue  = value.MoveNext();
                        continue;
                    }
                    break;
                }
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<bool?>(value);
                }
                stb.AppendCollectionItem(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }
    

    public TExt WhenPopulatedWithFilterEnumerate<TFmt, TFmtBase>(string fieldName, IEnumerator<TFmt?>? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable, TFmtBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmt);
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        
        ExplicitOrderedCollectionMold<TFmt>? eocm = null;
        if (hasValue)
        {
            var count     = 0;
            var skipCount = 0;
            while (hasValue)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue  = value!.MoveNext();
                    continue;
                }
                var item         = value!.Current;
                var filterResult = filterPredicate(count, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        hasValue  = value.MoveNext();
                        continue;
                    }
                    break;
                }
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<TFmt>(value);
                }
                stb.AppendFormattedCollectionItem(item, itemCount, formatString ?? "");
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterEnumerate<TFmtStruct>(string fieldName, IEnumerator<TFmtStruct?>? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmtStruct?);
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        
        ExplicitOrderedCollectionMold<TFmtStruct?>? eocm = null;
        if (hasValue)
        {
            var count     = 0;
            var skipCount = 0;
            while (hasValue)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue  = value!.MoveNext();
                    continue;
                }
                var item         = value!.Current;
                var filterResult = filterPredicate(count, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        hasValue  = value.MoveNext();
                        continue;
                    }
                    break;
                }
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<TFmtStruct?>(value);
                }
                stb.AppendFormattedCollectionItem(item, itemCount, formatString ?? "");
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }
    

    public TExt WhenPopulatedWithFilterRevealEnumerate<TCloaked, TCloakedFilterBase, TCloakedRevealBase>(string fieldName, IEnumerator<TCloaked?>? value
      , OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate, PalantírReveal<TCloakedRevealBase> palantírReveal) 
        where TCloaked : TCloakedFilterBase, TCloakedRevealBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCloaked);
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        
        ExplicitOrderedCollectionMold<TCloaked>? eocm = null;
        if (hasValue)
        {
            var count     = 0;
            var skipCount = 0;
            while (hasValue)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue  = value!.MoveNext();
                    continue;
                }
                var item         = value!.Current;
                var filterResult = filterPredicate(count, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        hasValue  = value.MoveNext();
                        continue;
                    }
                    break;
                }
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<TCloaked>(value);
                }
                stb.AppendOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }
    
    public TExt WhenPopulatedWithFilterRevealEnumerate<TCloakedStruct>(string fieldName, IEnumerator<TCloakedStruct?>? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate, PalantírReveal<TCloakedStruct> palantírReveal) 
        where TCloakedStruct : struct
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCloakedStruct);
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        
        ExplicitOrderedCollectionMold<TCloakedStruct>? eocm = null;
        if (hasValue)
        {
            var count     = 0;
            var skipCount = 0;
            while (hasValue)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue  = value!.MoveNext();
                    continue;
                }
                var item         = value!.Current;
                var filterResult = filterPredicate(count, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        hasValue  = value.MoveNext();
                        continue;
                    }
                    break;
                }
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<TCloakedStruct>(value);
                }
                stb.AppendOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }
    
    
    public TExt WhenPopulatedWithFilterRevealEnumerate<TBearer, TBearerBase>(string fieldName, IEnumerator<TBearer?>? value, OrderedCollectionPredicate<TBearerBase> filterPredicate)
        where TBearer : IStringBearer, TBearerBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TBearer);
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        
        ExplicitOrderedCollectionMold<TBearer>? eocm = null;
        if (hasValue)
        {
            var count     = 0;
            var skipCount = 0;
            while (hasValue)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue  = value!.MoveNext();
                    continue;
                }
                var item         = value!.Current;
                var filterResult = filterPredicate(count, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        hasValue  = value.MoveNext();
                        continue;
                    }
                    break;
                }
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<TBearer>(value);
                }
                
                stb.AppendRevealBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }
    
    public TExt WhenPopulatedWithFilterRevealEnumerate<TBearerStruct>(string fieldName, IEnumerator<TBearerStruct?>? value, OrderedCollectionPredicate<TBearerStruct?> filterPredicate)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TBearerStruct);
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        
        ExplicitOrderedCollectionMold<TBearerStruct>? eocm = null;
        if (hasValue)
        {
            var count     = 0;
            var skipCount = 0;
            while (hasValue)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue  = value!.MoveNext();
                    continue;
                }
                var item         = value!.Current;
                var filterResult = filterPredicate(count, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        hasValue  = value.MoveNext();
                        continue;
                    }
                    break;
                }
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<TBearerStruct>(value);
                }
                
                stb.AppendRevealBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterEnumerate(string fieldName, IEnumerator<string?>? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(string);
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        
        ExplicitOrderedCollectionMold<string>? eocm = null;
        if (hasValue)
        {
            var count     = 0;
            var skipCount = 0;
            while (hasValue)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue  = value!.MoveNext();
                    continue;
                }
                var item         = value!.Current;
                var filterResult = filterPredicate(count, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        hasValue  = value.MoveNext();
                        continue;
                    }
                    break;
                }
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<string>(value);
                }
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }
    
    public TExt WhenPopulatedWithFilterCharSeqEnumerate<TCharSeq, TCharSeqBase>(string fieldName, IEnumerator<TCharSeq?>? value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence, TCharSeqBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCharSeq);
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        
        ExplicitOrderedCollectionMold<StringBuilder>? eocm = null;
        if (hasValue)
        {
            var count     = 0;
            var skipCount = 0;
            while (hasValue)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue  = value!.MoveNext();
                    continue;
                }
                var item         = value!.Current;
                var filterResult = filterPredicate(count, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        hasValue  = value.MoveNext();
                        continue;
                    }
                    break;
                }
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<StringBuilder>(value);
                }
                
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, itemCount);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterEnumerate(string fieldName, IEnumerator<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(StringBuilder);
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;

        ExplicitOrderedCollectionMold<StringBuilder>? eocm = null;
        if (hasValue)
        {
            var count     = 0;
            var skipCount = 0;
            while (hasValue)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue = value!.MoveNext();
                    continue;
                }
                var item         = value!.Current;
                var filterResult = filterPredicate(count, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        hasValue  = value.MoveNext();
                        continue;
                    }
                    break;
                }
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<StringBuilder>(value);
                }

                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }


    public TExt WhenPopulatedWithFilterMatchEnumerate<TAny, TAnyBase>(string fieldName, IEnumerator<TAny?>? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TAny : TAnyBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TAny);

        ExplicitOrderedCollectionMold<TAny>? eocm = null;

        var hasValue  = value?.MoveNext() ?? false;
        var itemCount = 0;
        if (hasValue)
        {
            var count     = 0;
            var skipCount = 0;
            while (hasValue)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue = value!.MoveNext();
                    continue;
                }
                var item         = value!.Current;
                var filterResult = filterPredicate(count, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        hasValue  = value.MoveNext();
                        continue;
                    }
                    break;
                }
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<TAny>(value);
                }
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemMatchOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemMatchOrNull(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterObjectEnumerate(string fieldName, IEnumerator<object?>? value
      , OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(object);

        ExplicitOrderedCollectionMold<object>? eocm = null;

        var hasValue  = value?.MoveNext() ?? false;
        var itemCount = 0;
        if (hasValue)
        {
            var count     = 0;
            var skipCount = 0;
            while (hasValue)
            {
                count++;
                if (skipCount-- > 0)
                {
                    hasValue = value!.MoveNext();
                    continue;
                }
                var item         = value!.Current;
                var filterResult = filterPredicate(count, item!);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        hasValue  = value.MoveNext();
                        continue;
                    }
                    break;
                }
                if (eocm == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eocm = stb.Master.StartExplicitCollectionType<object>(value);
                }
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemMatchOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemMatchOrNull(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }
}
