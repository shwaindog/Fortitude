using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeOrderedCollection;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldCollection;

public partial class SelectTypeCollectionField<TExt> where TExt : TypeMolder
{
    public TExt AlwaysAddFiltered(ReadOnlySpan<char> fieldName, Span<bool> value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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
            formatString ??= "";
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else
                {
                    stb.GoToNextCollectionItemStart(elementType, i);
                }
                stb.AppendFormattedCollectionItem(item, i, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(ReadOnlySpan<char> fieldName, Span<bool?> value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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
            formatString ??= "";
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else
                {
                    stb.GoToNextCollectionItemStart(elementType, i);
                }
                stb.AppendFormattedCollectionItem(item, i, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmt>
    (ReadOnlySpan<char> fieldName, Span<TFmt> value, OrderedCollectionPredicate<TFmt> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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
            formatString ??= "";
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else
                {
                    stb.GoToNextCollectionItemStart(elementType, i);
                }
                stb.AppendFormattedCollectionItem(item, i, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredNullable<TFmt>
    (ReadOnlySpan<char> fieldName, Span<TFmt?> value, OrderedCollectionPredicate<TFmt> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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
            formatString ??= "";
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else
                {
                    stb.GoToNextCollectionItemStart(elementType, i);
                }
                stb.AppendFormattedCollectionItem(item, i, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmtStruct>
    (ReadOnlySpan<char> fieldName, Span<TFmtStruct?> value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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
            formatString ??= "";
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else
                {
                    stb.GoToNextCollectionItemStart(elementType, i);
                }
                stb.AppendFormattedCollectionItem(item, i, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    (ReadOnlySpan<char> fieldName, Span<TCloaked> value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TCloakedRevealBase> palantírReveal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCloaked : TCloakedFilterBase, TCloakedRevealBase
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.RevealCloakedBearerOrNull(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFilteredNullable<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    (ReadOnlySpan<char> fieldName, Span<TCloaked?> value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TCloakedRevealBase> palantírReveal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCloaked : TCloakedFilterBase, TCloakedRevealBase
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.RevealCloakedBearerOrNull(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TCloakedStruct>(ReadOnlySpan<char> fieldName, Span<TCloakedStruct?> value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.RevealNullableCloakedBearerOrNull(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TBearer, TBearerBase>(ReadOnlySpan<char> fieldName, Span<TBearer> value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer, TBearerBase
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.RevealStringBearerOrNull(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFilteredNullable<TBearer, TBearerBase>(ReadOnlySpan<char> fieldName, Span<TBearer?> value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TBearer : class, IStringBearer, TBearerBase
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.RevealStringBearerOrNull(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TBearerStruct>(ReadOnlySpan<char> fieldName, Span<TBearerStruct?> value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.RevealNullableStringBearerOrNull(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(ReadOnlySpan<char> fieldName, Span<string> value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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
            formatString ??= "";
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else
                {
                    stb.GoToNextCollectionItemStart(elementType, i);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredNullable(ReadOnlySpan<char> fieldName, Span<string?> value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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
            formatString ??= "";
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else
                {
                    stb.GoToNextCollectionItemStart(elementType, i);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredCharSeq<TCharSeq, TCharSeqBase>(ReadOnlySpan<char> fieldName, Span<TCharSeq> value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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
            formatString ??= "";
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else
                {
                    stb.GoToNextCollectionItemStart(elementType, i);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredCharSeqNullable<TCharSeq, TCharSeqBase>(ReadOnlySpan<char> fieldName, Span<TCharSeq?> value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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
            formatString ??= "";
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else
                {
                    stb.GoToNextCollectionItemStart(elementType, i);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(ReadOnlySpan<char> fieldName, Span<StringBuilder> value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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
            formatString ??= "";
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else
                {
                    stb.GoToNextCollectionItemStart(elementType, i);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredNullable(ReadOnlySpan<char> fieldName, Span<StringBuilder?> value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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
            formatString ??= "";
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else
                {
                    stb.GoToNextCollectionItemStart(elementType, i);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredMatch<TAny, TAnyBase>(ReadOnlySpan<char> fieldName, Span<TAny> value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TAny : TAnyBase
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

        var matchedItems = 0;
        if (value.Length > 0)
        {
            formatString ??= "";
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else
                {
                    stb.GoToNextCollectionItemStart(elementType, i);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredMatchNullable<TAny, TAnyBase>(ReadOnlySpan<char> fieldName, Span<TAny?> value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TAny : TAnyBase
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

        var matchedItems = 0;
        if (value.Length > 0)
        {
            formatString ??= "";
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else
                {
                    stb.GoToNextCollectionItemStart(elementType, i);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddFilteredObject(ReadOnlySpan<char> fieldName, Span<object> value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)  =>
        AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString);

    [CallsObjectToString]
    public TExt AlwaysAddFilteredObjectNullable(ReadOnlySpan<char> fieldName, Span<object?> value, OrderedCollectionPredicate<object?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        AlwaysAddFilteredMatchNullable(fieldName, value, filterPredicate, formatString);
    
    public TExt AlwaysAddFiltered(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool> value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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
            formatString ??= "";
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType);
                }
                else
                {
                    stb.GoToNextCollectionItemStart(elementType, i);
                }
                stb.AppendFormattedCollectionItem(item, i, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool?> value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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
            formatString ??= "";
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else
                {
                    stb.GoToNextCollectionItemStart(elementType, i);
                }
                stb.AppendFormattedCollectionItem(item, i, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmt>
    (ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmt> value, OrderedCollectionPredicate<TFmt> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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
            formatString ??= "";
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else
                {
                    stb.GoToNextCollectionItemStart(elementType, i);
                }
                stb.AppendFormattedCollectionItem(item, i, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredNullable<TFmt>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmt?> value
      , OrderedCollectionPredicate<TFmt> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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
            formatString ??= "";
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else
                {
                    stb.GoToNextCollectionItemStart(elementType, i);
                }
                stb.AppendFormattedCollectionItem(item, i, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmtStruct>
    (ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmtStruct?> value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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
            formatString ??= "";
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else
                {
                    stb.GoToNextCollectionItemStart(elementType, i);
                }
                stb.AppendFormattedCollectionItem(item, i, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    (ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloaked> value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TCloakedRevealBase> palantírReveal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCloaked : TCloakedFilterBase, TCloakedRevealBase
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.RevealCloakedBearerOrNull(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFilteredNullable<TCloaked, TCloakedFilterBase, TCloakedRevealBase>(ReadOnlySpan<char> fieldName
      , ReadOnlySpan<TCloaked?> value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TCloakedRevealBase> palantírReveal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCloaked : class, TCloakedFilterBase, TCloakedRevealBase
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.RevealCloakedBearerOrNull(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TCloakedStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloakedStruct?> value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.RevealNullableCloakedBearerOrNull(item, palantírReveal);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TBearer, TBearerBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearer> value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer, TBearerBase
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.RevealStringBearerOrNull(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFilteredNullable<TBearer, TBearerBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearer?> value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TBearer : class, IStringBearer, TBearerBase
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.RevealStringBearerOrNull(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TBearerStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearerStruct?> value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else { stb.GoToNextCollectionItemStart(elementType, matchedItems); }
                stb.RevealNullableStringBearerOrNull(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(ReadOnlySpan<char> fieldName, ReadOnlySpan<string> value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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
            formatString ??= "";
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else
                {
                    stb.GoToNextCollectionItemStart(elementType, i);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<string?> value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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
            formatString ??= "";
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else
                {
                    stb.GoToNextCollectionItemStart(elementType, i);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredCharSeq<TCharSeq, TCharSeqBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCharSeq> value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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
            formatString ??= "";
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else
                {
                    stb.GoToNextCollectionItemStart(elementType, i);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredCharSeqNullable<TCharSeq, TCharSeqBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCharSeq?> value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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
            formatString ??= "";
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else
                {
                    stb.GoToNextCollectionItemStart(elementType, i);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder> value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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
            formatString ??= "";
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else
                {
                    stb.GoToNextCollectionItemStart(elementType, i);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder?> value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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
            formatString ??= "";
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else
                {
                    stb.GoToNextCollectionItemStart(elementType, i);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredMatch<TAny, TAnyBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TAny> value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TAny : TAnyBase
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

        var matchedItems = 0;
        if (value.Length > 0)
        {
            formatString ??= "";
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else
                {
                    stb.GoToNextCollectionItemStart(elementType, i);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredMatchNullable<TAny, TAnyBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TAny?> value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TAny : TAnyBase
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

        var matchedItems = 0;
        if (value.Length > 0)
        {
            formatString ??= "";
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
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType); }
                else
                {
                    stb.GoToNextCollectionItemStart(elementType, i);
                }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0);
            }
        }
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddFilteredObject(ReadOnlySpan<char> fieldName, ReadOnlySpan<object> value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString);

    
    [CallsObjectToString]
    public TExt AlwaysAddFilteredObjectNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<object?> value, OrderedCollectionPredicate<object?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        AlwaysAddFilteredMatchNullable(fieldName, value, filterPredicate, formatString);

    public TExt AlwaysAddFiltered(string fieldName, bool[]? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<bool>? eocm = null;
        if (value != null)
        {
            formatString ??= "";
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
        stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(string fieldName, bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<bool?>? eocm = null;
        if (value != null)
        {
            formatString ??= "";
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
        stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmt>
    (string fieldName, TFmt?[]? value, OrderedCollectionPredicate<TFmt> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<TFmt>? eocm = null;
        if (value != null)
        {
            formatString ??= "";
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
        stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmtStruct>
    (string fieldName, TFmtStruct?[]? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<TFmtStruct?>? eocm = null;
        if (value != null)
        {
            formatString ??= "";
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
        stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    (string fieldName, TCloaked?[]? value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TCloakedRevealBase> palantírReveal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCloaked : TCloakedFilterBase, TCloakedRevealBase
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
        stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TCloakedStruct>(string fieldName, TCloakedStruct?[]? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct
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
        stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TBearer, TBearerBase>(string fieldName, TBearer?[]? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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
        stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TBearerStruct>(string fieldName, TBearerStruct?[]? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer
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
        stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered
    (string fieldName, string?[]? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<string>? eocm = null;
        if (value != null)
        {
            formatString ??= "";
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
        stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredCharSeq<TCharSeq, TCharSeqBase>
    (string fieldName, TCharSeq?[]? value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence, TCharSeqBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<TCharSeq>? eocm = null;
        if (value != null)
        {
            formatString ??= "";
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
        stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(string fieldName, StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<StringBuilder?>? eocm = null;
        if (value != null)
        {
            formatString ??= "";
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
        stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredMatch<TAny, TAnyBase>(string fieldName, TAny?[]? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TAny : TAnyBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<TAny>? eocm = null;
        if (value != null)
        {
            formatString ??= "";
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
                eocm ??= stb.Master.StartExplicitCollectionType<TAny>(value);
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
        stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddFilteredObject(string fieldName, object?[]? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString);

    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<bool>? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<bool>? eocm = null;
        if (value != null)
        {
            formatString ??= "";
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
        stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<bool?>? eocm = null;
        if (value != null)
        {
            formatString ??= "";
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
        stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmt>(string fieldName, IReadOnlyList<TFmt?>? value, OrderedCollectionPredicate<TFmt> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<TFmt>? eocm = null;
        if (value != null)
        {
            formatString ??= "";
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
        stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmtStruct>(string fieldName, IReadOnlyList<TFmtStruct?>? value
      , OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<TFmtStruct?>? eocm = null;
        if (value != null)
        {
            formatString ??= "";
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
        stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    (string fieldName, IReadOnlyList<TCloaked?>? value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TCloakedRevealBase> palantírReveal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCloaked : TCloakedFilterBase, TCloakedRevealBase
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
        stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TCloakedStruct>
    (string fieldName, IReadOnlyList<TCloakedStruct?>? value, OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct
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
        stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TBearer, TBearerBase>(string fieldName, IReadOnlyList<TBearer?>? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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
        stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TBearerStruct>(string fieldName, IReadOnlyList<TBearerStruct?>? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer
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
        stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<string?>? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<string?>? eocm = null;
        if (value != null)
        {
            formatString ??= "";
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
        stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredCharSeq<TCharSeq, TCharSeqBase>(string fieldName, IReadOnlyList<TCharSeq?>? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence, TCharSeqBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<TCharSeq?>? eocm = null;
        if (value != null)
        {
            formatString ??= "";
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
        stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<StringBuilder?>? eocm = null;
        if (value != null)
        {
            formatString ??= "";
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
        stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredMatch<TAny, TANyBase>(string fieldName, IReadOnlyList<TAny?>? value, OrderedCollectionPredicate<TANyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TAny : TANyBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<TAny>? eocm = null;
        if (value != null)
        {
            formatString ??= "";
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
                eocm ??= stb.Master.StartExplicitCollectionType<TAny>(value);
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
        stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddFilteredObject(string fieldName, IReadOnlyList<object?>? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString);
    
    public TExt AlwaysAddFilteredEnumerate(string fieldName, IEnumerable<bool>? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddFilteredEnumerate(value, filterPredicate, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate(string fieldName, IEnumerable<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddFilteredEnumerate(value, filterPredicate, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TFmt, TFmtBase>(string fieldName, IEnumerable<TFmt?>? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable, TFmtBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddFilteredEnumerate(value, filterPredicate, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TFmtStruct>(string fieldName, IEnumerable<TFmtStruct?>? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddFilteredEnumerate(value, filterPredicate, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFilteredEnumerate<TCloaked, TCloakedFilterBase, TCloakedBase>
        (string fieldName, IEnumerable<TCloaked?>? value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
          , PalantírReveal<TCloakedBase> palantírReveal, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TCloakedBase, TCloakedFilterBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealFilteredEnumerate(value, filterPredicate, palantírReveal).Complete();
        else
            stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFilteredEnumerate<TCloakedStruct>
        (string fieldName, IEnumerable<TCloakedStruct?>? value, OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
          , PalantírReveal<TCloakedStruct> palantírReveal, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealFilteredEnumerate(value, filterPredicate, palantírReveal).Complete();
        else
            stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFilteredEnumerate<TBearer, TBearerBase>(string fieldName, IEnumerable<TBearer?>? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer, TBearerBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealFilteredEnumerate(value, filterPredicate).Complete();
        else
            stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFilteredEnumerate<TBearerStruct>(string fieldName, IEnumerable<TBearerStruct?>? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealFilteredEnumerate(value, filterPredicate).Complete();
        else
            stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate(string fieldName, IEnumerable<string?>? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddFilteredEnumerate(value, filterPredicate, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredCharSeqEnumerate<TCharSeq, TCharSeqBase>(string fieldName, IEnumerable<TCharSeq?>? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence, TCharSeqBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddFilteredCharSeqEnumerate(value, filterPredicate, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate(string fieldName, IEnumerable<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddFilteredEnumerate(value, filterPredicate, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredMatchEnumerate<TAny, TAnyBase>(string fieldName, IEnumerable<TAny?>? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    where TAny : TAnyBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddFilteredMatchEnumerate(value, filterPredicate, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddFilteredObjectEnumerate(string fieldName, IEnumerable<object?>? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddFilteredObjectEnumerate(value, filterPredicate, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }
    
    public TExt AlwaysAddFilteredEnumerate(string fieldName, IEnumerator<bool>? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddFilteredEnumerate(value, filterPredicate, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate(string fieldName, IEnumerator<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddFilteredEnumerate(value, filterPredicate, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TFmt, TFmtBase>(string fieldName, IEnumerator<TFmt?>? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable, TFmtBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddFilteredEnumerate(value, filterPredicate, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TFmtStruct>(string fieldName, IEnumerator<TFmtStruct?>? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddFilteredEnumerate(value, filterPredicate, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFilteredEnumerate<TCloaked, TCloakedFilterBase, TCloakedBase>
        (string fieldName, IEnumerator<TCloaked?>? value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
          , PalantírReveal<TCloakedBase> palantírReveal, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TCloakedBase, TCloakedFilterBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealFilteredEnumerate(value, filterPredicate, palantírReveal).Complete();
        else
            stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFilteredEnumerate<TCloakedStruct>
        (string fieldName, IEnumerator<TCloakedStruct?>? value, OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
          , PalantírReveal<TCloakedStruct> palantírReveal, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealFilteredEnumerate(value, filterPredicate, palantírReveal).Complete();
        else
            stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFilteredEnumerate<TBearer, TBearerBase>(string fieldName, IEnumerator<TBearer?>? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer, TBearerBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealFilteredEnumerate(value, filterPredicate).Complete();
        else
            stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFilteredEnumerate<TBearerStruct>(string fieldName, IEnumerator<TBearerStruct?>? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).RevealFilteredEnumerate(value, filterPredicate).Complete();
        else
            stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate(string fieldName, IEnumerator<string?>? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddFilteredEnumerate(value, filterPredicate, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredCharSeqEnumerate<TCharSeq, TCharSeqBase>(string fieldName, IEnumerator<TCharSeq?>? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence, TCharSeqBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddFilteredCharSeqEnumerate(value, filterPredicate, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate(string fieldName, IEnumerator<StringBuilder?>? value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddFilteredEnumerate(value, filterPredicate, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredMatchEnumerate<TAny, TAnyBase>(string fieldName, IEnumerator<TAny?>? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    where TAny : TAnyBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddFilteredMatchEnumerate(value, filterPredicate, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddFilteredObjectEnumerate(string fieldName, IEnumerator<object?>? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddFilteredObjectEnumerate(value, filterPredicate, formatString).Complete();
        else
            stb.Sb.Append(stb.Settings.NullString);
        return stb.AddGoToNext();
    }
}
