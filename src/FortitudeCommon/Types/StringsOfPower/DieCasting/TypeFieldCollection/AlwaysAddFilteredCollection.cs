using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeOrderedCollection;
using FortitudeCommon.Types.StringsOfPower.Forge;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldCollection;

public partial class SelectTypeCollectionField<TExt> where TExt : TypeMolder
{
    public TExt AlwaysAddFiltered(ReadOnlySpan<char> fieldName, Span<bool> value, OrderedCollectionPredicate<bool> filterPredicate)
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                stb.AppendCollectionItem(item, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(ReadOnlySpan<char> fieldName, Span<bool?> value, OrderedCollectionPredicate<bool?> filterPredicate)
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                stb.AppendCollectionItem(item, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmt>
    (ReadOnlySpan<char> fieldName, Span<TFmt> value, OrderedCollectionPredicate<TFmt> filterPredicate
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredNullable<TFmt>
    (ReadOnlySpan<char> fieldName, Span<TFmt?> value, OrderedCollectionPredicate<TFmt> filterPredicate
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmtStruct>
    (ReadOnlySpan<char> fieldName, Span<TFmtStruct?> value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    (ReadOnlySpan<char> fieldName, Span<TCloaked> value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TCloakedRevealBase> palantírReveal) where TCloaked : TCloakedFilterBase, TCloakedRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendOrNull(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFilteredNullable<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    (ReadOnlySpan<char> fieldName, Span<TCloaked?> value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TCloakedRevealBase> palantírReveal) where TCloaked : TCloakedFilterBase, TCloakedRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendOrNull(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TCloakedStruct>(ReadOnlySpan<char> fieldName, Span<TCloakedStruct?> value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendOrNull(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TBearer, TBearerBase>(ReadOnlySpan<char> fieldName, Span<TBearer> value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate) where TBearer : IStringBearer, TBearerBase
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendOrNull(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFilteredNullable<TBearer, TBearerBase>(ReadOnlySpan<char> fieldName, Span<TBearer?> value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate) where TBearer : class, IStringBearer, TBearerBase
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendOrNull(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TBearerStruct>(ReadOnlySpan<char> fieldName, Span<TBearerStruct?> value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate) where TBearerStruct : struct, IStringBearer
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendOrNull(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(ReadOnlySpan<char> fieldName, Span<string> value, OrderedCollectionPredicate<string> filterPredicate
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredNullable(ReadOnlySpan<char> fieldName, Span<string?> value, OrderedCollectionPredicate<string> filterPredicate
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredCharSeq<TCharSeq, TCharSeqBase>(ReadOnlySpan<char> fieldName, Span<TCharSeq> value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TCharSeq : ICharSequence, TCharSeqBase
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredCharSeqNullable<TCharSeq, TCharSeqBase>(ReadOnlySpan<char> fieldName, Span<TCharSeq?> value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TCharSeq : ICharSequence, TCharSeqBase
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(ReadOnlySpan<char> fieldName, Span<StringBuilder> value, OrderedCollectionPredicate<StringBuilder> filterPredicate
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredNullable(ReadOnlySpan<char> fieldName, Span<StringBuilder?> value, OrderedCollectionPredicate<StringBuilder> filterPredicate
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredMatch<T, TBase>(ReadOnlySpan<char> fieldName, Span<T> value, OrderedCollectionPredicate<TBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : TBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredMatchNullable<T, TBase>(ReadOnlySpan<char> fieldName, Span<T?> value, OrderedCollectionPredicate<TBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : TBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddFilteredObject(ReadOnlySpan<char> fieldName, Span<object> value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  =>
        AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString);

    [CallsObjectToString]
    public TExt AlwaysAddFilteredObjectNullable(ReadOnlySpan<char> fieldName, Span<object?> value, OrderedCollectionPredicate<object?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        AlwaysAddFilteredMatchNullable(fieldName, value, filterPredicate, formatString);
    
    public TExt AlwaysAddFiltered(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool> value, OrderedCollectionPredicate<bool> filterPredicate)
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                stb.AppendCollectionItem(item, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool?> value, OrderedCollectionPredicate<bool?> filterPredicate)
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                stb.AppendCollectionItem(item, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmt>
    (ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmt> value, OrderedCollectionPredicate<TFmt> filterPredicate
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredNullable<TFmt>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmt?> value
      , OrderedCollectionPredicate<TFmt> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmtStruct>
    (ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmtStruct?> value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    (ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloaked> value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TCloakedRevealBase> palantírReveal) where TCloaked : TCloakedFilterBase, TCloakedRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendOrNull(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFilteredNullable<TCloaked, TCloakedFilterBase, TCloakedRevealBase>(ReadOnlySpan<char> fieldName
      , ReadOnlySpan<TCloaked?> value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TCloakedRevealBase> palantírReveal) where TCloaked : class, TCloakedFilterBase, TCloakedRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendOrNull(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TCloakedStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloakedStruct?> value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendOrNull(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TBearer, TBearerBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearer> value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate) where TBearer : IStringBearer, TBearerBase
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendOrNull(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFilteredNullable<TBearer, TBearerBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearer?> value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate) where TBearer : class, IStringBearer, TBearerBase
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendOrNull(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TBearerStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearerStruct?> value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate) where TBearerStruct : struct, IStringBearer
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.AppendOrNull(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(ReadOnlySpan<char> fieldName, ReadOnlySpan<string> value, OrderedCollectionPredicate<string> filterPredicate
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<string?> value, OrderedCollectionPredicate<string> filterPredicate
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredCharSeq<TCharSeq, TCharSeqBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCharSeq> value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TCharSeq : ICharSequence, TCharSeqBase
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredCharSeqNullable<TCharSeq, TCharSeqBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCharSeq?> value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TCharSeq : ICharSequence, TCharSeqBase
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder> value, OrderedCollectionPredicate<StringBuilder> filterPredicate
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder?> value, OrderedCollectionPredicate<StringBuilder> filterPredicate
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredMatch<T, TBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<T> value, OrderedCollectionPredicate<TBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : TBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
        var collectionType = typeof(ReadOnlySpan<T>);
        var elementType    = typeof(T);

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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredMatchNullable<T, TBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<T?> value
      , OrderedCollectionPredicate<TBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : TBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value.Length == 0)
            if (stb.Settings.EmptyCollectionWritesNull)
            {
                stb.Sb.Append(stb.Settings.NullString);
                return stb.AddGoToNext();
            }
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb, elementType, value.Length > 0, collectionType); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString ?? "");
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddFilteredObject(ReadOnlySpan<char> fieldName, ReadOnlySpan<object> value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString);

    
    [CallsObjectToString]
    public TExt AlwaysAddFilteredObjectNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<object?> value, OrderedCollectionPredicate<object?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        AlwaysAddFilteredMatchNullable(fieldName, value, filterPredicate, formatString);

    public TExt AlwaysAddFiltered(string fieldName, bool[]? value, OrderedCollectionPredicate<bool> filterPredicate)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
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
                eocm ??= stb.Master.StartExplicitCollectionType<bool[], bool>(value);
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
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(string fieldName, bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
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
                eocm ??= stb.Master.StartExplicitCollectionType<bool?[], bool?>(value);
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
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmt>
    (string fieldName, TFmt?[]? value, OrderedCollectionPredicate<TFmt> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
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
                eocm ??= stb.Master.StartExplicitCollectionType<TFmt>(value);
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
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmtStruct>
    (string fieldName, TFmtStruct?[]? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
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
                eocm ??= stb.Master.StartExplicitCollectionType<TFmtStruct?>(value);
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
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    (string fieldName, TCloaked?[]? value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TCloakedRevealBase> palantírReveal) where TCloaked : TCloakedFilterBase, TCloakedRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
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
                eocm ??= stb.Master.StartExplicitCollectionType<TCloaked>(value);
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
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TCloakedStruct>(string fieldName, TCloakedStruct?[]? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
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
                eocm ??= stb.Master.StartExplicitCollectionType<TCloakedStruct>(value);
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
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TBearer, TBearerBase>(string fieldName, TBearer?[]? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate)
        where TBearer : IStringBearer, TBearerBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<TBearer?>? eocm = null;
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
                eocm ??= stb.Master.StartExplicitCollectionType<TBearer?>(value);
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
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TBearerStruct>(string fieldName, TBearerStruct?[]? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate) where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<TBearerStruct?>? eocm = null;
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
                eocm ??= stb.Master.StartExplicitCollectionType<TBearerStruct?>(value);
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
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered
    (string fieldName, string?[]? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<string>? eocm = null;
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
                eocm ??= stb.Master.StartExplicitCollectionType<string?[], string>(value);
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
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredCharSeq<TCharSeq, TCharSeqBase>
    (string fieldName, TCharSeq?[]? value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TCharSeq : ICharSequence, TCharSeqBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<TCharSeq>? eocm = null;
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
                eocm ??= stb.Master.StartExplicitCollectionType<TCharSeq>(value);
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
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(string fieldName, StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
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
                eocm ??= stb.Master.StartExplicitCollectionType<StringBuilder?[], StringBuilder?>(value);
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
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredMatch<T, TBase>(string fieldName, T?[]? value, OrderedCollectionPredicate<TBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : TBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<T>? eocm = null;
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
                eocm ??= stb.Master.StartExplicitCollectionType<T>(value);
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
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddFilteredObject(string fieldName, object?[]? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString);

    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<bool>? value, OrderedCollectionPredicate<bool> filterPredicate)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<bool>? eocm = null;
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
                eocm ??= stb.Master.StartExplicitCollectionType<bool>(value);
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
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
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
                eocm ??= stb.Master.StartExplicitCollectionType<bool?>(value);
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
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmt>(string fieldName, IReadOnlyList<TFmt?>? value, OrderedCollectionPredicate<TFmt> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
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
                eocm ??= stb.Master.StartExplicitCollectionType<TFmt>(value);
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
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmtStruct>(string fieldName, IReadOnlyList<TFmtStruct?>? value
      , OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
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
                eocm ??= stb.Master.StartExplicitCollectionType<TFmtStruct?>(value);
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
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    (string fieldName, IReadOnlyList<TCloaked?>? value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TCloakedRevealBase> palantírReveal) where TCloaked : TCloakedFilterBase, TCloakedRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<TCloaked>? eocm = null;
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
                eocm ??= stb.Master.StartExplicitCollectionType<TCloaked>(value);
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
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TCloakedStruct>
    (string fieldName, IReadOnlyList<TCloakedStruct?>? value, OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<TCloakedStruct?>? eocm = null;
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
                eocm ??= stb.Master.StartExplicitCollectionType<TCloakedStruct?>(value);
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
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TBearer, TBearerBase>(string fieldName, IReadOnlyList<TBearer?>? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate)
        where TBearer : IStringBearer, TBearerBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<TBearer>? eocm = null;
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
                eocm ??= stb.Master.StartExplicitCollectionType<TBearer>(value);
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
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TBearerStruct>(string fieldName, IReadOnlyList<TBearerStruct?>? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate) where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
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
                eocm ??= stb.Master.StartExplicitCollectionType<IReadOnlyList<TBearerStruct?>, TBearerStruct>(value);
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
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<string?>? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<string?>? eocm = null;
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
                eocm ??= stb.Master.StartExplicitCollectionType<IReadOnlyList<string?>, string?>(value);
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
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredCharSeq<TCharSeq, TCharSeqBase>(string fieldName, IReadOnlyList<TCharSeq?>? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TCharSeq : ICharSequence, TCharSeqBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<TCharSeq?>? eocm = null;
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
                eocm ??= stb.Master.StartExplicitCollectionType<IReadOnlyList<TCharSeq?>, TCharSeq?>(value);
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
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<StringBuilder?>? eocm = null;
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
                eocm ??= stb.Master.StartExplicitCollectionType<IReadOnlyList<StringBuilder?>, StringBuilder?>(value);
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
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredMatch<T, TBase>(string fieldName, IReadOnlyList<T?>? value, OrderedCollectionPredicate<TBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : TBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<T>? eocm = null;
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
                eocm ??= stb.Master.StartExplicitCollectionType<T>(value);
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
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddFilteredObject(string fieldName, IReadOnlyList<object?>? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString);
}
