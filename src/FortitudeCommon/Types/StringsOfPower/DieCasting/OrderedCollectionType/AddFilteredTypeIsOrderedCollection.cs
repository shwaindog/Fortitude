// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification.CollectionItemResult;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.WrittenAsFlags;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;

public partial class OrderedCollectionMold<TOCMold> where TOCMold : TypeMolder
{
    public TOCMold AddFiltered(bool[]? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(bool[]);
        if (mws.HasSkipBody(actualType, "", formatFlags))
        {
            return mws.WasSkipped(actualType, "", formatFlags);
        }
        var elementType = typeof(bool);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Length, value?.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFiltered(bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(bool?[]);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(bool?);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Length, value?.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFiltered(Span<bool> value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<bool>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(bool);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFiltered(Span<bool?> value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<bool?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(bool?);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFiltered(ReadOnlySpan<bool> value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<bool>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(bool);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFiltered(ReadOnlySpan<bool?> value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<bool?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(bool?);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFiltered(IReadOnlyList<bool>? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<bool>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(bool);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFiltered(IReadOnlyList<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<bool?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(bool?);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFilteredEnumerateBool<TEnumbl>(TEnumbl? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<bool>?
    {
        mws.AddFilteredEnumerateBool(value, filterPredicate, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddFilteredEnumerateNullableBool<TEnumbl>(TEnumbl? value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<bool?>?
    {
        mws.AddFilteredEnumerateNullableBool(value, filterPredicate, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddFilteredIterate<TEnumtr>(TEnumtr? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<bool>?
    {
        mws.AddFilteredIterateBool(value, filterPredicate, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddFilteredIterateNullable<TEnumtr>(TEnumtr? value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<bool?>?
    {
        mws.AddFilteredIterateNullableBool(value, filterPredicate, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddFiltered<TFmt, TFmtBase>(TFmt[]? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?, TFmtBase?
    {
        var actualType = value?.GetType() ?? typeof(TFmt[]);
        if (mws.HasSkipBody(actualType, "", formatFlags)) 
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TFmt);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Length, value?.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFiltered<TFmtStruct>(TFmtStruct?[]? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(TFmtStruct?[]);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TFmtStruct?);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Length, value?.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFiltered<TFmt, TFmtBase>(Span<TFmt> value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?, TFmtBase?
    {
        var actualType = typeof(Span<TFmt>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TFmt);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFiltered<TFmtStruct>(Span<TFmtStruct?> value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = typeof(Span<TFmtStruct?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TFmtStruct?);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFiltered<TFmt, TFmtBase>(ReadOnlySpan<TFmt> value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?, TFmtBase?
    {
        var actualType = typeof(ReadOnlySpan<TFmt>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TFmt);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFiltered<TFmtStruct>(ReadOnlySpan<TFmtStruct?> value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = typeof(ReadOnlySpan<TFmtStruct?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TFmtStruct?);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFiltered<TFmt, TFmtBase>(IReadOnlyList<TFmt>? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?, TFmtBase?
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TFmt?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TFmt);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFiltered<TFmtStruct>(IReadOnlyList<TFmtStruct?>? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TFmtStruct?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TFmtStruct?);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFilteredEnumerate<TEnumbl, TFmt, TFmtBase>(TEnumbl? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TFmt>?
        where TFmt : ISpanFormattable?, TFmtBase?
    {
        mws.AddFilteredEnumerate<TEnumbl, TFmt, TFmtBase>(value, filterPredicate, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddFilteredEnumerateNullable<TEnumbl, TFmtStruct>(TEnumbl value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TFmtStruct?>?
        where TFmtStruct : struct, ISpanFormattable
    {
        mws.AddFilteredEnumerateNullable(value, filterPredicate, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddFilteredIterate<TEnumtr, TFmt, TFmtBase>(TEnumtr? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TFmt>?
        where TFmt : ISpanFormattable?, TFmtBase?
    {
        mws.AddFilteredIterate<TEnumtr, TFmt, TFmtBase>(value, filterPredicate, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddFilteredIterateNullable<TEnumtr, TFmtStruct>(TEnumtr? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TFmtStruct?>?
        where TFmtStruct : struct, ISpanFormattable
    {
        mws.AddFilteredIterateNullable(value, filterPredicate, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold RevealFiltered<TCloaked, TFilterBase, TRevealBase>(TCloaked[]? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TCloaked[]);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCloaked);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.RevealCloakedBearerOrNull(item, palantírReveal, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Length, value?.Length, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealFiltered<TCloakedStruct>(TCloakedStruct?[]? value, OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(TCloakedStruct?[]);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCloakedStruct);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.RevealNullableCloakedBearerOrNull(item, palantírReveal, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Length, value?.Length, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealFiltered<TCloaked, TFilterBase, TRevealBase>(Span<TCloaked> value, OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = typeof(Span<TCloaked>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCloaked);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.RevealCloakedBearerOrNull(item, palantírReveal, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealFiltered<TCloakedStruct>(Span<TCloakedStruct?> value, OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TCloakedStruct : struct
    {
        var actualType = typeof(Span<TCloakedStruct?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCloakedStruct);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.RevealNullableCloakedBearerOrNull(item, palantírReveal, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealFiltered<TCloaked, TFilterBase, TRevealBase>(ReadOnlySpan<TCloaked> value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = typeof(ReadOnlySpan<TCloaked>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCloaked);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.RevealCloakedBearerOrNull(item, palantírReveal, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealFiltered<TCloakedStruct>(ReadOnlySpan<TCloakedStruct?> value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate, PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType = typeof(ReadOnlySpan<TCloakedStruct?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCloakedStruct);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.RevealNullableCloakedBearerOrNull(item, palantírReveal, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealFiltered<TCloaked, TCloakedFilterBase, TCloakedRevealBase>(IReadOnlyList<TCloaked>? value
      , OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate, PalantírReveal<TCloakedRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TCloakedFilterBase?, TCloakedRevealBase?
        where TCloakedRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TCloaked?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCloaked);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.RevealCloakedBearerOrNull(item, palantírReveal, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealFiltered<TCloakedStruct>(IReadOnlyList<TCloakedStruct?>? value,
        OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TCloakedStruct?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCloakedStruct);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.RevealNullableCloakedBearerOrNull(item, palantírReveal, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealFilteredEnumerate<TEnumbl, TCloaked, TFilterBase, TRevealBase>(TEnumbl? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate, PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCloaked>?
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull
    {
        mws.RevealFilteredEnumerate<TEnumbl, TCloaked, TFilterBase, TRevealBase>(value, filterPredicate, palantírReveal, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold RevealFilteredEnumerateNullable<TEnumbl, TCloakedStruct>(TEnumbl? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate, PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCloakedStruct?>?
        where TCloakedStruct : struct
    {
        mws.RevealFilteredEnumerateNullable(value, filterPredicate, palantírReveal, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold RevealFilteredIterate<TEnumtr, TCloaked, TCloakedFilterBase, TCloakedRevealBase>(TEnumtr? value
      , OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate, PalantírReveal<TCloakedRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TCloaked>?
        where TCloaked : TCloakedFilterBase?, TCloakedRevealBase?
        where TCloakedRevealBase : notnull
    {
        mws.RevealFilteredIterate<TEnumtr, TCloaked, TCloakedFilterBase, TCloakedRevealBase>
            (value, filterPredicate, palantírReveal, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold RevealFilteredIterateNullable<TEnumtr, TCloakedStruct>(TEnumtr? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate, PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TCloakedStruct?>?
        where TCloakedStruct : struct
    {
        mws.RevealFilteredIterateNullable(value, filterPredicate, palantírReveal, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold RevealFiltered<TBearer, TBearerBase>(TBearer[]? value, OrderedCollectionPredicate<TBearerBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?, TBearerBase?
    {
        var actualType = value?.GetType() ?? typeof(TBearer[]);
        if (mws.HasSkipBody(actualType, "", formatFlags)) 
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TBearer);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.RevealStringBearerOrNull(item, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Length, value?.Length, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealFiltered<TBearerStruct>(TBearerStruct?[]? value, OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(TBearerStruct?[]);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TBearerStruct);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.RevealNullableStringBearerOrNull(item, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Length, value?.Length, formatString ?? "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealFiltered<TBearer, TBearerBase>(Span<TBearer> value, OrderedCollectionPredicate<TBearerBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?, TBearerBase?
    {
        var actualType = typeof(Span<TBearer>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TBearer);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.RevealStringBearerOrNull(item, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealFiltered<TBearerStruct>(Span<TBearerStruct?> value, OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = typeof(Span<TBearerStruct?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TBearerStruct);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.RevealNullableStringBearerOrNull(item, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealFiltered<TBearer, TBearerBase>(ReadOnlySpan<TBearer> value, OrderedCollectionPredicate<TBearerBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?, TBearerBase?
    {
        var actualType = typeof(ReadOnlySpan<TBearer>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TBearer);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.RevealStringBearerOrNull(item, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealFiltered<TBearerStruct>(ReadOnlySpan<TBearerStruct?> value, OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = typeof(ReadOnlySpan<TBearerStruct?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TBearerStruct);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.RevealNullableStringBearerOrNull(item, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealFiltered<TBearer, TBearerBase>(IReadOnlyList<TBearer>? value, OrderedCollectionPredicate<TBearerBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?, TBearerBase?
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TBearer>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TBearer);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.RevealStringBearerOrNull(item, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealFiltered<TBearerStruct>(IReadOnlyList<TBearerStruct?>? value, OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TBearerStruct?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TBearerStruct);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.RevealNullableStringBearerOrNull(item, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealFilteredEnumerate<TEnumbl, TBearer, TBearerBase>(TEnumbl? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TBearer>?
        where TBearer : IStringBearer?, TBearerBase?
    {
        mws.RevealFilteredEnumerate<TEnumbl, TBearer, TBearerBase>(value, filterPredicate, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold RevealFilteredEnumerateNullable<TEnumbl, TBearerStruct>(TEnumbl? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TBearerStruct?>?
        where TBearerStruct : struct, IStringBearer
    {
        mws.RevealFilteredEnumerateNullable(value, filterPredicate, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold RevealFilteredIterate<TEnumtr, TBearer, TBearerBase>(TEnumtr? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TBearer>?
        where TBearer : IStringBearer?, TBearerBase?
    {
        mws.RevealFilteredIterateStringBearer<TEnumtr, TBearer, TBearerBase>(value, filterPredicate, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold RevealFilteredIterateNullable<TEnumtr, TBearerStruct>(TEnumtr? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TBearerStruct?>?
        where TBearerStruct : struct, IStringBearer
    {
        mws.RevealFilteredIterateNullableStringBearer(value, filterPredicate, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddFiltered(string?[]? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(string?[]);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(string);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Length, value?.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFiltered(Span<string> value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<string>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(string);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFilteredNullable(Span<string?> value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<string?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(string);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFiltered(ReadOnlySpan<string> value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<string>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(string);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFilteredNullable(ReadOnlySpan<string?> value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<string?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(string);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFiltered(IReadOnlyList<string?>? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<string?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(string);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFilteredEnumerate<TEnumbl>(TEnumbl? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<string?>?
    {
        mws.AddFilteredStringEnumerate(value, filterPredicate, formatString, formatFlags);
        return mws.Mold;
    }


    public TOCMold AddFilteredIterate<TEnumtr>(TEnumtr? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<string?>?
    {
        mws.AddFilteredIterateString(value, filterPredicate, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddFilteredCharSeq<TCharSeq, TCharSeqBase>(TCharSeq[]? value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?, TCharSeqBase?
    {
        var actualType = value?.GetType() ?? typeof(TCharSeq[]);
        if (mws.HasSkipBody(actualType, "", formatFlags)) 
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCharSeq);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Length, value?.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFilteredCharSeq<TCharSeq, TCharSeqBase>(Span<TCharSeq> value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?, TCharSeqBase?
    {
        var actualType = typeof(Span<TCharSeq>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCharSeq);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFilteredCharSeq<TCharSeq, TCharSeqBase>(ReadOnlySpan<TCharSeq> value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?, TCharSeqBase?
    {
        var actualType = typeof(ReadOnlySpan<TCharSeq>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCharSeq);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFilteredCharSeq<TCharSeq, TCharSeqBase>(IReadOnlyList<TCharSeq>? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?, TCharSeqBase?
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TCharSeq>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCharSeq);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFilteredCharSeqEnumerate<TEnumbl, TCharSeq, TCharSeqBase>(TEnumbl? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCharSeq>?
        where TCharSeq : ICharSequence?, TCharSeqBase?
    {
        mws.AddFilteredCharSeqEnumerate<TEnumbl, TCharSeq, TCharSeqBase>(value, filterPredicate, formatString, formatFlags);
        return mws.Mold;
    }


    public TOCMold AddFilteredCharSeqIterate<TEnumtr, TCharSeq, TCharSeqBase>(TEnumtr? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TCharSeq>?
        where TCharSeq : ICharSequence?, TCharSeqBase?
    {
        mws.AddFilteredIterateCharSeq<TEnumtr, TCharSeq, TCharSeqBase>(value, filterPredicate, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddFiltered(StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(StringBuilder?[]);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(StringBuilder);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Length, value?.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }


    public TOCMold AddFiltered(Span<StringBuilder> value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<StringBuilder>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(StringBuilder);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }


    public TOCMold AddFilteredNullable(Span<StringBuilder?> value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<StringBuilder?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(StringBuilder);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFiltered(ReadOnlySpan<StringBuilder> value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<StringBuilder>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(StringBuilder);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFilteredNullable(ReadOnlySpan<StringBuilder?> value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<StringBuilder?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(StringBuilder);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFiltered(IReadOnlyList<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<StringBuilder?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(StringBuilder);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold,  elementType, value?.Count, value?.Count, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFilteredEnumerate<TEnumbl>(TEnumbl? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<StringBuilder?>?
    {
        mws.AddFilteredStringBuilderEnumerate(value, filterPredicate, formatString, formatFlags);
        return mws.Mold;
    }


    public TOCMold AddFilteredIterate<TEnumtr>(TEnumtr? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<StringBuilder?>?
    {
        mws.AddFilteredIterateStringBuilder(value, filterPredicate, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddFilteredMatch<TAny, TAnyBase>(TAny[]? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TAny : TAnyBase?
    {
        var actualType = value?.GetType() ?? typeof(TAny[]);
        if (mws.HasSkipBody(actualType, "", formatFlags)) 
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TAny);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Length, value?.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFilteredMatch<TAny, TAnyBase>(Span<TAny> value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TAny : TAnyBase?
    {
        var actualType = typeof(Span<TAny>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TAny);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFilteredMatch<TAny, TAnyBase>(ReadOnlySpan<TAny> value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TAny : TAnyBase?
    {
        var actualType = typeof(ReadOnlySpan<TAny>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TAny);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFilteredMatch<TAny, TAnyBase>(IReadOnlyList<TAny>? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TAny : TAnyBase?
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TAny?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TAny);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddFilteredMatchEnumerate<TEnumbl, TAny, TAnyBase>(TEnumbl? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TAny>?
        where TAny : TAnyBase?
    {
        mws.AddFilteredMatchEnumerate<TEnumbl, TAny, TAnyBase>(value, filterPredicate, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddFilteredMatchIterate<TEnumtr, TAny, TAnyBase>(TEnumtr? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TAny?>?
        where TAny : TAnyBase?
    {
        mws.AddFilteredMatchIterate<TEnumtr, TAny, TAnyBase>(value, filterPredicate, formatString, formatFlags);
        return mws.Mold;
    }

    [CallsObjectToString]
    public TOCMold AddFilteredObject(object?[]? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(object?[]);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(object);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Length, value?.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    [CallsObjectToString]
    public TOCMold AddFilteredObject(Span<object> value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<object>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(object);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    [CallsObjectToString]
    public TOCMold AddFilteredObjectNullable(Span<object?> value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<object?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(object);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    [CallsObjectToString]
    public TOCMold AddFilteredObject(ReadOnlySpan<object> value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<object>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(object);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    [CallsObjectToString]
    public TOCMold AddFilteredObjectNullable(ReadOnlySpan<object?> value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<object?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(object);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    [CallsObjectToString]
    public TOCMold AddFilteredObject(IReadOnlyList<object?>? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<object?>);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(object);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate?.Invoke(i + 1, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true)
                    {
                        break;
                    }
                }
                mws.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        }
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    [CallsObjectToString]
    public TOCMold AddFilteredObjectEnumerate<TEnumbl>(TEnumbl? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<object?>?
    {
        mws.AddFilteredObjectEnumerate(value, filterPredicate, formatString, formatFlags);
        return mws.Mold;
    }

    [CallsObjectToString]
    public TOCMold AddFilteredObjectIterate<TEnumtr>(TEnumtr? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<object?>?
    {
        mws.AddFilteredObjectIterate(value, filterPredicate, formatString, formatFlags);
        return mws.Mold;
    }
}
