// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
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
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                } else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
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
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                } else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
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
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                } else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
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

    public TExt WhenPopulatedWithFilter(ReadOnlySpan<char> fieldName, Span<StringBuilder> value, OrderedCollectionPredicate<StringBuilder> filterPredicate
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

    public TExt WhenPopulatedWithFilterNullable(ReadOnlySpan<char> fieldName, Span<StringBuilder?> value, OrderedCollectionPredicate<StringBuilder> filterPredicate
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

    public TExt WhenPopulatedWithFilterMatch<T, TBase>(ReadOnlySpan<char> fieldName, Span<T> value, OrderedCollectionPredicate<TBase> filterPredicate
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

    public TExt WhenPopulatedWithFilterMatchNullable<T, TBase>(ReadOnlySpan<char> fieldName, Span<T?> value, OrderedCollectionPredicate<TBase> filterPredicate
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
    public TExt WhenPopulatedWithFilterObject(ReadOnlySpan<char> fieldName, Span<object> value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString);

    [CallsObjectToString]
    public TExt WhenPopulatedWithFilterObjectNullable(ReadOnlySpan<char> fieldName, Span<object?> value, OrderedCollectionPredicate<object?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString);
    
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

    public TExt WhenPopulatedWithFilterNullable<TFmt>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmt?> value, OrderedCollectionPredicate<TFmt> filterPredicate
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
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                } else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
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
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                } else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
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
                    stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType);
                } else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
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

    public TExt WhenPopulatedWithFilterNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<string?> value, OrderedCollectionPredicate<string> filterPredicate
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

    public TExt WhenPopulatedWithFilter(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder> value, OrderedCollectionPredicate<StringBuilder> filterPredicate
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

    public TExt WhenPopulatedWithFilterNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder?> value, OrderedCollectionPredicate<StringBuilder> filterPredicate
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

    public TExt WhenPopulatedWithFilterMatch<T, TBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<T> value, OrderedCollectionPredicate<TBase> filterPredicate
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

    public TExt WhenPopulatedWithFilterMatchNullable<T, TBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<T?> value, OrderedCollectionPredicate<TBase> filterPredicate
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
    public TExt WhenPopulatedWithFilterObject(ReadOnlySpan<char> fieldName, ReadOnlySpan<object> value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  =>
        AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString);
    

    [CallsObjectToString]
    public TExt WhenPopulatedWithFilterObjectNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<object?> value, OrderedCollectionPredicate<object?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  =>
        AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString);
    
    public TExt WhenPopulatedWithFilter(string fieldName, bool[]? value, OrderedCollectionPredicate<bool> filterPredicate)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<bool>? eoctb = null;
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
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<bool[], bool>(value);
                }
                eoctb.AddElementAndGoToNextElement(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(string fieldName, bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<bool?>? eoctb = null;
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
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<bool?[], bool?>(value);
                }
                eoctb.AddElementAndGoToNextElement(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
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
        ExplicitOrderedCollectionMold<TFmt>? eoctb = null;
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
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<TFmt>(value);
                }
                eoctb.AddElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
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
        ExplicitOrderedCollectionMold<TFmtStruct?>? eoctb = null;
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
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<TFmtStruct?>(value);
                }
                eoctb.AddElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterReveal<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    (string fieldName, TCloaked?[]? value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TCloakedRevealBase> palantírReveal) where TCloaked : TCloakedFilterBase, TCloakedRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TCloaked>? eoctb = null;
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
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<TCloaked>(value);
                }
                eoctb.AddElementAndGoToNextElement(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterReveal<TCloakedStruct>
    (string fieldName, TCloakedStruct?[]? value, OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TCloakedStruct>? eoctb = null;
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
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<TCloakedStruct>(value);
                }
                eoctb.AddElementAndGoToNextElement(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterReveal<TBearer, TBearerBase>(string fieldName, TBearer?[]? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate)
        where TBearer : IStringBearer, TBearerBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TBearer>? eoctb = null;
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
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<TBearer>(value);
                }
                eoctb.AddBearerElementAndGoToNextElement(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterReveal<TBearerStruct>(string fieldName, TBearerStruct?[]? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TBearerStruct>? eoctb = null;
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
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<TBearerStruct>(value);
                }
                eoctb.AddBearerElementAndGoToNextElement(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter
    (string fieldName, string?[]? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<string?>? eoctb = null;
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
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<string?[], string?>(value);
                }
                eoctb.AddElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
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
        ExplicitOrderedCollectionMold<TCharSeq?>? eoctb = null;
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
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<TCharSeq?[], TCharSeq?>(value);
                }
                eoctb.AddCharSequenceElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(string fieldName, StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<StringBuilder?>? eoctb = null;
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
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<StringBuilder?[], StringBuilder?>(value);
                }
                eoctb.AddElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterMatch<T, TBase>(string fieldName, T?[]? value, OrderedCollectionPredicate<TBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : TBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<T>? eoctb = null;
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
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<T>(value);
                }
                eoctb.AddMatchElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
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
        ExplicitOrderedCollectionMold<bool?>? eoctb = null;
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
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<bool?>(value);
                }
                eoctb.AddElementAndGoToNextElement(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(string fieldName, IReadOnlyList<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<bool?>? eoctb = null;
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
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<bool?>(value);
                }
                eoctb.AddElementAndGoToNextElement(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TFmt>(string fieldName, IReadOnlyList<TFmt?>? value, OrderedCollectionPredicate<TFmt> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TFmt>? eoctb = null;
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
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<TFmt>(value);
                }
                eoctb.AddElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TFmtStruct>(string fieldName, IReadOnlyList<TFmtStruct?>? value
      , OrderedCollectionPredicate<TFmtStruct?> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TFmtStruct?>? eoctb = null;
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
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<TFmtStruct?>(value);
                }
                eoctb.AddElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterReveal<TCloaked, TCloakedFilterBase, TToStyleBase>
    (string fieldName, IReadOnlyList<TCloaked>? value, OrderedCollectionPredicate<TToStyleBase> filterPredicate
      , PalantírReveal<TCloakedFilterBase> palantírReveal) where TCloaked : TCloakedFilterBase, TToStyleBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TCloaked>? eoctb = null;
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
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<TCloaked>(value);
                }
                eoctb.AddElementAndGoToNextElement(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterReveal<TCloakedStruct>
    (string fieldName, IReadOnlyList<TCloakedStruct?>? value, OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TCloakedStruct>? eoctb = null;
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
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<TCloakedStruct>(value);
                }
                eoctb.AddElementAndGoToNextElement(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterReveal<TBearer, TBearerBase>(string fieldName, IReadOnlyList<TBearer?>? value, OrderedCollectionPredicate<TBearerBase?> filterPredicate)
        where TBearer : IStringBearer, TBearerBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TBearer>? eoctb = null;
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
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<TBearer>(value);
                }
                eoctb.AddBearerElementAndGoToNextElement(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterReveal<TBearerStruct>(string fieldName, IReadOnlyList<TBearerStruct?>? value, OrderedCollectionPredicate<TBearerStruct?> filterPredicate)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TBearerStruct>? eoctb = null;
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
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<TBearerStruct>(value);
                }
                eoctb.AddBearerElementAndGoToNextElement(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(string fieldName, IReadOnlyList<string?>? value, OrderedCollectionPredicate<string?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<string?>? eoctb = null;
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
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<string?>(value);
                }
                eoctb.AddElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterCharSeq<TCharSeq, TCharSeqFilterBase>(string fieldName, IReadOnlyList<TCharSeq?>? value
      , OrderedCollectionPredicate<TCharSeqFilterBase?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence, TCharSeqFilterBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TCharSeq>? eoctb = null;
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
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<TCharSeq>(value);
                }
                eoctb.AddCharSequenceElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(string fieldName, IReadOnlyList<StringBuilder?>? value
      , OrderedCollectionPredicate<StringBuilder?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<StringBuilder?>? eoctb = null;
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
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<StringBuilder?>(value);
                }
                eoctb.AddElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterMatch<T, TBase>(string fieldName, IReadOnlyList<T?>? value, OrderedCollectionPredicate<TBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : TBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<T>? eoctb = null;
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
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<T>(value);
                }
                eoctb.AddMatchElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TExt WhenPopulatedWithFilterObject(string fieldName, IReadOnlyList<object?>? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  =>
        WhenPopulatedWithFilterMatch(fieldName, value, filterPredicate, formatString);
}
