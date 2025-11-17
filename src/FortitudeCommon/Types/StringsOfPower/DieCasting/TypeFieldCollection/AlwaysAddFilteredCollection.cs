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
        var collectionType = typeof(Span<bool>);
        var elementType    = typeof(bool);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, formatString, formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
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
        var collectionType = typeof(Span<bool?>);
        var elementType    = typeof(bool?);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, formatString, formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmt, TFmtBase>
    (ReadOnlySpan<char> fieldName, Span<TFmt> value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable, TFmtBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<TFmt>);
        var elementType    = typeof(TFmt);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, formatString, formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredNullable<TFmt, TFmtBase>
    (ReadOnlySpan<char> fieldName, Span<TFmt?> value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable, TFmtBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<TFmt>);
        var elementType    = typeof(TFmt);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item!) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, formatString, formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
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
        var collectionType = typeof(Span<TFmtStruct?>);
        var elementType    = typeof(TFmtStruct?);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, matchedItems);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, formatString, formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
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
        var collectionType = typeof(Span<TCloaked>);
        var elementType    = typeof(TCloaked);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.RevealCloakedBearerOrNull(item, palantírReveal, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, matchedItems);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, "", formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
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
        var collectionType = typeof(Span<TCloaked>);
        var elementType    = typeof(TCloaked);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item!) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.RevealCloakedBearerOrNull(item, palantírReveal, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, matchedItems);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, "", formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
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
        var collectionType = typeof(Span<TCloakedStruct?>);
        var elementType    = typeof(TCloakedStruct?);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.RevealNullableCloakedBearerOrNull(item, palantírReveal, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, matchedItems);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, "", formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
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
        var collectionType = typeof(Span<TBearer>);
        var elementType    = typeof(TBearer);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.RevealStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, matchedItems);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, "", formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
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
        var collectionType = typeof(Span<TBearer>);
        var elementType    = typeof(TBearer);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item!) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.RevealStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, matchedItems);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, "", formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
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
        var collectionType = typeof(Span<TBearerStruct?>);
        var elementType    = typeof(TBearerStruct?);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.RevealNullableStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, matchedItems);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, "", formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
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
        var collectionType = typeof(Span<string>);
        var elementType    = typeof(string);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, formatString, formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
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
        var collectionType = typeof(Span<string>);
        var elementType    = typeof(string);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item!) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, formatString, formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
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
        var collectionType = typeof(Span<TCharSeq>);
        var elementType    = typeof(TCharSeq);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, formatString, formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
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
        var collectionType = typeof(Span<TCharSeq>);
        var elementType    = typeof(TCharSeq);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item!) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, formatString, formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
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
        var collectionType = typeof(Span<StringBuilder>);
        var elementType    = typeof(StringBuilder);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, formatString, formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
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
        var collectionType = typeof(Span<StringBuilder>);
        var elementType    = typeof(StringBuilder);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item!) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, formatString, formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
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
        var collectionType = typeof(Span<TAny>);
        var elementType    = typeof(TAny);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, formatString, formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
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
        var collectionType = typeof(Span<TAny>);
        var elementType    = typeof(TAny);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item!) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, formatString, formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
            }
        }
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddFilteredObject(ReadOnlySpan<char> fieldName, Span<object> value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)  =>
        AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags);

    [CallsObjectToString]
    public TExt AlwaysAddFilteredObjectNullable(ReadOnlySpan<char> fieldName, Span<object?> value, OrderedCollectionPredicate<object?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        AlwaysAddFilteredMatchNullable(fieldName, value, filterPredicate, formatString, formatFlags);
    
    public TExt AlwaysAddFiltered(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool> value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<bool>);
        var elementType    = typeof(bool);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
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
                    stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags);
                }
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, formatString, formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
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
        var collectionType = typeof(ReadOnlySpan<bool?>);
        var elementType    = typeof(bool?);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, formatString, formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmt, TFmtBase>
    (ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmt> value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable, TFmtBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<TFmt>);
        var elementType    = typeof(TFmt);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, formatString, formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
            }
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredNullable<TFmt, TFmtBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmt?> value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmt : class, ISpanFormattable, TFmtBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<TFmt>);
        var elementType    = typeof(TFmt);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item!) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, formatString, formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
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
        var collectionType = typeof(ReadOnlySpan<TFmtStruct?>);
        var elementType    = typeof(TFmtStruct?);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, formatString, formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
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
        var collectionType = typeof(ReadOnlySpan<TCloaked>);
        var elementType    = typeof(TCloaked);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.RevealCloakedBearerOrNull(item, palantírReveal, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, matchedItems);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, "", formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
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
        var collectionType = typeof(ReadOnlySpan<TCloaked>);
        var elementType    = typeof(TCloaked);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item!) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.RevealCloakedBearerOrNull(item, palantírReveal, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, matchedItems);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, "", formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
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
        var collectionType = typeof(ReadOnlySpan<TCloakedStruct?>);
        var elementType    = typeof(TCloakedStruct?);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.RevealNullableCloakedBearerOrNull(item, palantírReveal, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, matchedItems);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, "", formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
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
        var collectionType = typeof(ReadOnlySpan<TBearer>);
        var elementType    = typeof(TBearer);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.RevealStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, matchedItems);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, "", formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
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
        var collectionType = typeof(ReadOnlySpan<TBearer>);
        var elementType    = typeof(TBearer);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item!) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.RevealStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, matchedItems);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, "", formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
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
        var collectionType = typeof(ReadOnlySpan<TBearerStruct?>);
        var elementType    = typeof(TBearerStruct?);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.RevealNullableStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, matchedItems);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, "", formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, "", formatFlags);
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
        var collectionType = typeof(ReadOnlySpan<string>);
        var elementType    = typeof(string);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, formatString, formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
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
        var collectionType = typeof(ReadOnlySpan<string>);
        var elementType    = typeof(string);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item!) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, formatString, formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
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
        var collectionType = typeof(ReadOnlySpan<TCharSeq>);
        var elementType    = typeof(TCharSeq);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, formatString, formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
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
        var collectionType = typeof(ReadOnlySpan<TCharSeq>);
        var elementType    = typeof(TCharSeq);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item!) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, formatString, formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
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
        var collectionType = typeof(ReadOnlySpan<StringBuilder>);
        var elementType    = typeof(StringBuilder);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, formatString, formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
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
        var collectionType = typeof(ReadOnlySpan<StringBuilder>);
        var elementType    = typeof(StringBuilder);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item!) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, formatString, formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
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
        var collectionType = typeof(ReadOnlySpan<TAny>);
        var elementType    = typeof(TAny);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }

        var matchedItems = 0;
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, formatString, formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
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
        var collectionType = typeof(ReadOnlySpan<TAny>);
        var elementType    = typeof(TAny);

        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        var matchedItems = 0;
        if (value.Length > 0)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item!) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (matchedItems++ == 0) { stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, value.Length > 0, collectionType, formatFlags); }
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (matchedItems != 0) stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, matchedItems, formatString, formatFlags);
        }
        if (matchedItems == 0)
        {
            if (stb.Settings.EmptyCollectionWritesNull) { stb.Sb.Append(stb.Settings.NullString); }
            else
            {
                stb.StyleFormatter.FormatCollectionStart(stb.Sb, elementType, false, collectionType, formatFlags);
                stb.StyleFormatter.FormatCollectionEnd(stb.Sb, elementType, 0, formatString, formatFlags);
            }
        }
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddFilteredObject(ReadOnlySpan<char> fieldName, ReadOnlySpan<object> value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags);

    
    [CallsObjectToString]
    public TExt AlwaysAddFilteredObjectNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<object?> value, OrderedCollectionPredicate<object?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        AlwaysAddFilteredMatchNullable(fieldName, value, filterPredicate, formatString, formatFlags);

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
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
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
                eocm.AddElementAndGoToNextElement(item, formatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(bool), null, typeof(bool[]), formatFlags);
        stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(bool),   null, formatString, formatFlags);
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
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
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
                eocm.AddElementAndGoToNextElement(item, formatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(bool?), null, typeof(bool?[]), formatFlags);
        stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(bool?),   null, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmt, TFmtBase>
    (string fieldName, TFmt?[]? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable, TFmtBase
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
                var filterResult = filterPredicate?.Invoke(i+1, item!) ?? CollectionItemResult.IncludedContinueToNext;
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
                eocm.AddElementAndGoToNextElement(item, formatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TFmt), null, typeof(TFmt?[]), formatFlags);
        stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TFmt),   null, formatString, formatFlags);
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
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
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
                eocm.AddElementAndGoToNextElement(item, formatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TFmtStruct?), null, typeof(TFmtStruct?[]), formatFlags);
        stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TFmtStruct?),   null, formatString, formatFlags);
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
                var filterResult = filterPredicate?.Invoke(i+1, item!) ?? CollectionItemResult.IncludedContinueToNext;
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
                eocm.AddElementAndGoToNextElement(item, palantírReveal, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TCloaked), null, typeof(TCloaked?[]), formatFlags);
        stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TCloaked),   null, "", formatFlags);
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
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
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
                eocm.AddElementAndGoToNextElement(item, palantírReveal, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TCloakedStruct?), null, typeof(TCloakedStruct?[]), formatFlags);
        stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TCloakedStruct?),   null, "", formatFlags);
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
                var filterResult = filterPredicate?.Invoke(i+1, item!) ?? CollectionItemResult.IncludedContinueToNext;
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
        stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TBearer), null, typeof(TBearer?[]), formatFlags);
        stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TBearer),   null, "", formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TBearerStruct>(string fieldName, TBearerStruct?[]? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<TBearerStruct>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                eocm ??= stb.Master.StartExplicitCollectionType<TBearerStruct>(value);
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
        stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TBearerStruct?), null, typeof(TBearerStruct?[]), formatFlags);
        stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TBearerStruct?),   null, "", formatFlags);
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
                var filterResult = filterPredicate?.Invoke(i+1, item!) ?? CollectionItemResult.IncludedContinueToNext;
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
                eocm.AddElementAndGoToNextElement(item, formatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(string), null, typeof(string?[]), formatFlags);
        stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(string),   null, formatString, formatFlags);
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
                var filterResult = filterPredicate?.Invoke(i+1, item!) ?? CollectionItemResult.IncludedContinueToNext;
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
                eocm.AddCharSequenceElementAndGoToNextElement(item, formatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TCharSeq), null, typeof(TCharSeq?[]), formatFlags);
        stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TCharSeq),   null, formatString, formatFlags);
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
                var filterResult = filterPredicate?.Invoke(i+1, item!) ?? CollectionItemResult.IncludedContinueToNext;
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
                eocm.AddElementAndGoToNextElement(item, formatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(StringBuilder), null, typeof(StringBuilder?[]), formatFlags);
        stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(StringBuilder),   null, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredMatch<TAny, TAnyBase>(string fieldName, TAny?[]? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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
                var filterResult = filterPredicate?.Invoke(i+1, item!) ?? CollectionItemResult.IncludedContinueToNext;
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
                eocm.AddMatchElementAndGoToNextElement(item, formatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TAny?), null, typeof(TAny?[]), formatFlags);
        stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TAny?),   null, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddFilteredObject(string fieldName, object?[]? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags);

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
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
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
                eocm.AddElementAndGoToNextElement(item, formatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(bool), null, value?.GetType() ?? typeof(IReadOnlyList<bool>), formatFlags);
        stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(bool),   null, formatString, formatFlags);
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
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
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
                eocm.AddElementAndGoToNextElement(item, formatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(bool?), null, value?.GetType() ?? typeof(IReadOnlyList<bool?>), formatFlags);
        stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(bool?),   null, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmt, TFmtBase>(string fieldName, IReadOnlyList<TFmt?>? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable, TFmtBase
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
                var filterResult = filterPredicate?.Invoke(i+1, item!) ?? CollectionItemResult.IncludedContinueToNext;
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
                eocm.AddElementAndGoToNextElement(item, formatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TFmt), null, value?.GetType() ?? typeof(IReadOnlyList<TFmt>), formatFlags);
        stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TFmt),   null, formatString, formatFlags);
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
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
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
                eocm.AddElementAndGoToNextElement(item, formatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TFmtStruct?), null, value?.GetType() ?? typeof(IReadOnlyList<TFmtStruct?>), formatFlags);
        stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TFmtStruct?),   null, formatString, formatFlags);
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
                var filterResult = filterPredicate?.Invoke(i+1, item!) ?? CollectionItemResult.IncludedContinueToNext;
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
                eocm.AddElementAndGoToNextElement(item, palantírReveal, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TCloaked), null, value?.GetType() ?? typeof(IReadOnlyList<TCloaked?>), formatFlags);
        stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TCloaked),   null, "", formatFlags);
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
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
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
                eocm.AddElementAndGoToNextElement(item, palantírReveal, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TCloakedStruct?), null, value?.GetType() ?? typeof(IReadOnlyList<TCloakedStruct?>), formatFlags);
        stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TCloakedStruct?),   null, "", formatFlags);
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
                var filterResult = filterPredicate?.Invoke(i+1, item!) ?? CollectionItemResult.IncludedContinueToNext;
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
        stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TBearer?), null, value?.GetType() ?? typeof(IReadOnlyList<TBearer?>), formatFlags);
        stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TBearer?),   null, "", formatFlags);
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
                var filterResult = filterPredicate?.Invoke(i+1, item) ?? CollectionItemResult.IncludedContinueToNext;
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
        stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TBearerStruct?), null, value?.GetType() ?? typeof(IReadOnlyList<TBearerStruct?>), formatFlags);
        stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TBearerStruct?),   null, "", formatFlags);
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
                var filterResult = filterPredicate?.Invoke(i+1, item!) ?? CollectionItemResult.IncludedContinueToNext;
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
                eocm.AddElementAndGoToNextElement(item, formatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(string), null, value?.GetType() ?? typeof(IReadOnlyList<string>), formatFlags);
        stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(string),   null, formatString, formatFlags);
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
                var filterResult = filterPredicate?.Invoke(i+1, item!) ?? CollectionItemResult.IncludedContinueToNext;
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
                eocm.AddCharSequenceElementAndGoToNextElement(item, formatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TCharSeq?), null, value?.GetType() ?? typeof(IReadOnlyList<TCharSeq?>), formatFlags);
        stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TCharSeq?),   null, formatString, formatFlags);
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
                var filterResult = filterPredicate?.Invoke(i+1, item!) ?? CollectionItemResult.IncludedContinueToNext;
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
                eocm.AddElementAndGoToNextElement(item, formatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(StringBuilder), null, value?.GetType() ?? typeof(IReadOnlyList<StringBuilder>), formatFlags);
        stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(StringBuilder),   null, formatString, formatFlags);
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
                var filterResult = filterPredicate?.Invoke(i+1, item!) ?? CollectionItemResult.IncludedContinueToNext;
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
                eocm.AddMatchElementAndGoToNextElement(item, formatString, formatFlags);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TAny?), null, value?.GetType() ?? typeof(IReadOnlyList<TAny?>), formatFlags);
        stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TAny?),   null, formatString, formatFlags);
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddFilteredObject(string fieldName, IReadOnlyList<object?>? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags);
    
    public TExt AlwaysAddFilteredEnumerate(string fieldName, IEnumerable<bool>? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddFilteredEnumerate(value, filterPredicate, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(bool), null, value?.GetType() ?? typeof(IEnumerable<bool>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(bool),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate(string fieldName, IEnumerable<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddFilteredEnumerate(value, filterPredicate, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(bool?), null, value?.GetType() ?? typeof(IEnumerable<bool?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(bool?),   null, formatString, formatFlags);
        }
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
            stb.Master.StartSimpleCollectionType(value).AddFilteredEnumerate(value, filterPredicate, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TFmt), null, value?.GetType() ?? typeof(IEnumerable<TFmt>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TFmt),   null, formatString, formatFlags);
        }
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
            stb.Master.StartSimpleCollectionType(value).AddFilteredEnumerate(value, filterPredicate, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TFmtStruct?), null, value?.GetType() ?? typeof(IEnumerable<TFmtStruct?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TFmtStruct?),   null, formatString, formatFlags);
        }
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
            stb.Master.StartSimpleCollectionType(value).RevealFilteredEnumerate(value, filterPredicate, palantírReveal, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TCloaked), null, value?.GetType() ?? typeof(IEnumerable<TCloaked?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TCloaked),   null, "", formatFlags);
        }
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
            stb.Master.StartSimpleCollectionType(value).RevealFilteredEnumerate(value, filterPredicate, palantírReveal, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TCloakedStruct), null, value?.GetType() ?? typeof(IEnumerable<TCloakedStruct?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TCloakedStruct),   null, "", formatFlags);
        }
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
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TBearer), null, value?.GetType() ?? typeof(IEnumerable<TBearer?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TBearer),   null, "", formatFlags);
        }
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
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TBearerStruct?), null, value?.GetType() ?? typeof(IEnumerable<TBearerStruct?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TBearerStruct?),   null, "", formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate(string fieldName, IEnumerable<string?>? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddFilteredEnumerate(value, filterPredicate, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(string), null, value?.GetType() ?? typeof(IEnumerable<string>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(string),   null, formatString, formatFlags);
        }
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
            stb.Master.StartSimpleCollectionType(value).AddFilteredCharSeqEnumerate(value, filterPredicate, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TCharSeq), null, value?.GetType() ?? typeof(IEnumerable<TCharSeq>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TCharSeq),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate(string fieldName, IEnumerable<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddFilteredEnumerate(value, filterPredicate, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(StringBuilder), null, value?.GetType() ?? typeof(IEnumerable<StringBuilder>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(StringBuilder),   null, formatString, formatFlags);
        }
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
            stb.Master.StartSimpleCollectionType(value).AddFilteredMatchEnumerate(value, filterPredicate, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TAny?), null, value?.GetType() ?? typeof(IEnumerable<TAny?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TAny?),   null, formatString, formatFlags);
        }
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
            stb.Master.StartSimpleCollectionType(value).AddFilteredObjectEnumerate(value, filterPredicate, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(object), null, value?.GetType() ?? typeof(IEnumerable<object>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(object),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }
    
    public TExt AlwaysAddFilteredEnumerate(string fieldName, IEnumerator<bool>? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddFilteredEnumerate(value, filterPredicate, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(bool), null, value?.GetType() ?? typeof(IEnumerator<bool>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(bool),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate(string fieldName, IEnumerator<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddFilteredEnumerate(value, filterPredicate, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(bool?), null, value?.GetType() ?? typeof(IEnumerator<bool?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(bool?),   null, formatString, formatFlags);
        }
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
            stb.Master.StartSimpleCollectionType(value).AddFilteredEnumerate(value, filterPredicate, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TFmt), null, value?.GetType() ?? typeof(IEnumerator<TFmt>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TFmt),   null, formatString, formatFlags);
        }
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
            stb.Master.StartSimpleCollectionType(value).AddFilteredEnumerate(value, filterPredicate, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TFmtStruct?), null, value?.GetType() ?? typeof(IEnumerator<TFmtStruct?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TFmtStruct?),   null, formatString, formatFlags);
        }
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
            stb.Master.StartSimpleCollectionType(value).RevealFilteredEnumerate(value, filterPredicate, palantírReveal, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TCloaked), null, value?.GetType() ?? typeof(IEnumerator<TCloaked>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TCloaked),   null, "", formatFlags);
        }
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
            stb.Master.StartSimpleCollectionType(value).RevealFilteredEnumerate(value, filterPredicate, palantírReveal, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TCloakedStruct?), null, value?.GetType() ?? typeof(IEnumerator<TCloakedStruct?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TCloakedStruct?),   null, "", formatFlags);
        }
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
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TBearer), null, value?.GetType() ?? typeof(IEnumerator<TBearer>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TBearer),   null, "", formatFlags);
        }
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
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TBearerStruct?), null, value?.GetType() ?? typeof(IEnumerator<TBearerStruct?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TBearerStruct?),   null, "", formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate(string fieldName, IEnumerator<string?>? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddFilteredEnumerate(value, filterPredicate, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(string), null, value?.GetType() ?? typeof(IEnumerator<string?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(string),   null, formatString, formatFlags);
        }
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
            stb.Master.StartSimpleCollectionType(value).AddFilteredCharSeqEnumerate(value, filterPredicate, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TCharSeq), null, value?.GetType() ?? typeof(IEnumerator<TCharSeq?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TCharSeq),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate(string fieldName, IEnumerator<StringBuilder?>? value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddFilteredEnumerate(value, filterPredicate, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(StringBuilder), null, value?.GetType() 
                                                                                       ?? typeof(IEnumerator<StringBuilder>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(StringBuilder),   null, formatString, formatFlags);
        }
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
            stb.Master.StartSimpleCollectionType(value).AddFilteredMatchEnumerate(value, filterPredicate, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(TAny), null, value?.GetType() ?? typeof(IEnumerator<TAny?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(TAny),   null, formatString, formatFlags);
        }
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
            stb.Master.StartSimpleCollectionType(value).AddFilteredObjectEnumerate(value, filterPredicate, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb.Sb, typeof(object), null, value?.GetType() ?? typeof(IEnumerator<object>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb.Sb, typeof(object),   null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }
}
