// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification.CollectionItemResult;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;

public partial class OrderedCollectionMold<TOCMold> where TOCMold : TypeMolder
{
    public TOCMold AddFiltered(bool[]? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(bool[]);
        if (stb.HasSkipBody(actualType, "", formatFlags))
        {
            return stb.WasSkipped(actualType, "", formatFlags);
        }
        var elementType = typeof(bool);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFiltered(bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(bool?[]);
        if (stb.HasSkipBody(actualType, "", formatFlags)) return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(bool?);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFiltered(Span<bool> value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<bool>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(bool);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFiltered(Span<bool?> value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<bool?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(bool?);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFiltered(ReadOnlySpan<bool> value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<bool>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(bool);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFiltered(ReadOnlySpan<bool?> value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<bool?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(bool?);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFiltered(IReadOnlyList<bool>? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<bool>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(bool);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFiltered(IReadOnlyList<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<bool?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(bool?);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFilteredEnumerate(IEnumerable<bool>? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<bool>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(bool);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            formatString ??= "";
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate?.Invoke(count, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFilteredEnumerate(IEnumerable<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<bool?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(bool?);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            formatString ??= "";
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate?.Invoke(count, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFilteredEnumerate(IEnumerator<bool>? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<bool>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(bool);
        var any         = false;
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        if (hasValue)
        {
            formatString ??= "";
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
                var filterResult = filterPredicate?.Invoke(count, item) ?? IncludedContinueToNext;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);

                any = true;
                stb.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFilteredEnumerate(IEnumerator<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<bool?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(bool?);
        var any         = false;
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        if (hasValue)
        {
            formatString ??= "";
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
                var filterResult = filterPredicate?.Invoke(count, item) ?? IncludedContinueToNext;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);

                any = true;
                stb.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFiltered<TFmt, TFmtBase>(TFmt[]? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?, TFmtBase?
    {
        var actualType = value?.GetType() ?? typeof(TFmt[]);
        if (stb.HasSkipBody(actualType, "", formatFlags)) 
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TFmt);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFiltered<TFmtStruct>(TFmtStruct?[]? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(TFmtStruct?[]);
        if (stb.HasSkipBody(actualType, "", formatFlags)) return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TFmtStruct?);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFiltered<TFmt, TFmtBase>(Span<TFmt> value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?, TFmtBase?
    {
        var actualType = typeof(Span<TFmt>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TFmt);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFiltered<TFmtStruct>(Span<TFmtStruct?> value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = typeof(Span<TFmtStruct?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TFmtStruct?);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFiltered<TFmt, TFmtBase>(ReadOnlySpan<TFmt> value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?, TFmtBase?
    {
        var actualType = typeof(ReadOnlySpan<TFmt>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TFmt);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFiltered<TFmtStruct>(ReadOnlySpan<TFmtStruct?> value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = typeof(ReadOnlySpan<TFmtStruct?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TFmtStruct?);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFiltered<TFmt, TFmtBase>(IReadOnlyList<TFmt>? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?, TFmtBase?
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TFmt?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TFmt);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFiltered<TFmtStruct>(IReadOnlyList<TFmtStruct?>? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TFmtStruct?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TFmtStruct?);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFilteredEnumerate<TFmt, TFmtBase>(IEnumerable<TFmt?>? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable, TFmtBase
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TFmt?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TFmt);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            formatString ??= "";
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFilteredEnumerate<TFmtStruct>(IEnumerable<TFmtStruct?>? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TFmtStruct?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TFmtStruct?);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            formatString ??= "";
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate?.Invoke(count, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFilteredEnumerate<TFmt, TFmtBase>(IEnumerator<TFmt>? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?, TFmtBase?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TFmt?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TFmt);
        var any         = false;
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        if (hasValue)
        {
            formatString ??= "";
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
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);

                any = true;
                stb.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFilteredEnumerate<TFmtStruct>(IEnumerator<TFmtStruct?>? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TFmtStruct?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TFmtStruct?);
        var any         = false;
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        if (hasValue)
        {
            formatString ??= "";
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
                var filterResult = filterPredicate?.Invoke(count, item) ?? IncludedContinueToNext;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);

                any = true;
                stb.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
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
        if (stb.HasSkipBody(actualType, "", formatFlags)) return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCloaked);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.RevealCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length, "", formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealFiltered<TCloakedStruct>(TCloakedStruct?[]? value, OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(TCloakedStruct?[]);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCloakedStruct);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.RevealNullableCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length, "", formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealFiltered<TCloaked, TFilterBase, TRevealBase>(Span<TCloaked> value, OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = typeof(Span<TCloaked>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCloaked);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.RevealCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, "", formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealFiltered<TCloakedStruct>(Span<TCloakedStruct?> value, OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct
    {
        var actualType = typeof(Span<TCloakedStruct?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCloakedStruct);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.RevealNullableCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, "", formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
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
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCloaked);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.RevealCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, "", formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealFiltered<TCloakedStruct>(ReadOnlySpan<TCloakedStruct?> value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate, PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType = typeof(ReadOnlySpan<TCloakedStruct?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCloakedStruct);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.RevealNullableCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, "", formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealFiltered<TCloaked, TCloakedFilterBase, TCloakedRevealBase>(IReadOnlyList<TCloaked?>? value
      , OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate, PalantírReveal<TCloakedRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TCloakedFilterBase?, TCloakedRevealBase?
        where TCloakedRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TCloaked?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCloaked);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.RevealCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count, "", formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealFiltered<TCloakedStruct>(IReadOnlyList<TCloakedStruct?>? value,
        OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TCloakedStruct?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCloakedStruct);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.RevealNullableCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count, "", formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealFilteredEnumerate<TCloaked, TFilterBase, TRevealBase>(IEnumerable<TCloaked?>? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate, PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TCloaked?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCloaked);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.RevealCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount, "", formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealFilteredEnumerate<TCloakedStruct>(IEnumerable<TCloakedStruct?>? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate, PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TCloakedStruct?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCloakedStruct);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate?.Invoke(count, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.RevealNullableCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount, "", formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealFilteredEnumerate<TCloaked, TCloakedFilterBase, TCloakedRevealBase>(IEnumerator<TCloaked?>? value
      , OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate, PalantírReveal<TCloakedRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TCloakedFilterBase?, TCloakedRevealBase?
        where TCloakedRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TCloaked?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCloaked);
        var any         = false;
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
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
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);

                any = true;
                stb.RevealCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount, "", formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealFilteredEnumerate<TCloakedStruct>(IEnumerator<TCloakedStruct?>? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate, PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TCloakedStruct?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCloakedStruct);
        var any         = false;
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
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
                var filterResult = filterPredicate?.Invoke(count, item) ?? IncludedContinueToNext;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);

                any = true;
                stb.RevealNullableCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount, "", formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealFiltered<TBearer, TBearerBase>(TBearer[]? value, OrderedCollectionPredicate<TBearerBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?, TBearerBase?
    {
        var actualType = value?.GetType() ?? typeof(TBearer[]);
        if (stb.HasSkipBody(actualType, "", formatFlags)) 
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TBearer);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.RevealStringBearerOrNull(item, formatString ?? "", formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length, "", formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealFiltered<TBearerStruct>(TBearerStruct?[]? value, OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(TBearerStruct?[]);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TBearerStruct);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.RevealNullableStringBearerOrNull(item, formatString ?? "", formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length, formatString ?? "", formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealFiltered<TBearer, TBearerBase>(Span<TBearer> value, OrderedCollectionPredicate<TBearerBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?, TBearerBase?
    {
        var actualType = typeof(Span<TBearer>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TBearer);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.RevealStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, "", formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealFiltered<TBearerStruct>(Span<TBearerStruct?> value, OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = typeof(Span<TBearerStruct?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TBearerStruct);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.RevealNullableStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, "", formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealFiltered<TBearer, TBearerBase>(ReadOnlySpan<TBearer> value, OrderedCollectionPredicate<TBearerBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?, TBearerBase?
    {
        var actualType = typeof(ReadOnlySpan<TBearer>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TBearer);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.RevealStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, "", formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealFiltered<TBearerStruct>(ReadOnlySpan<TBearerStruct?> value, OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = typeof(ReadOnlySpan<TBearerStruct?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TBearerStruct);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.RevealNullableStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, "", formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealFiltered<TBearer, TBearerBase>(IReadOnlyList<TBearer>? value, OrderedCollectionPredicate<TBearerBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?, TBearerBase?
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TBearer>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TBearer);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.RevealStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count, "", formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealFiltered<TBearerStruct>(IReadOnlyList<TBearerStruct?>? value, OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TBearerStruct?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TBearerStruct);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.RevealNullableStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count, "", formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealFilteredEnumerate<TBearer, TBearerBase>(IEnumerable<TBearer>? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?, TBearerBase?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TBearer>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TBearer);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.RevealStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount, "", formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealFilteredEnumerate<TBearerStruct>(IEnumerable<TBearerStruct?>? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TBearerStruct?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TBearerStruct);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate?.Invoke(count, item) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.RevealNullableStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount, "", formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealFilteredEnumerate<TBearer, TBearerBase>(IEnumerator<TBearer?>? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer, TBearerBase
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TBearer?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TBearer);
        var any         = false;
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
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
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);

                any = true;
                stb.RevealStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount, "", formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealFilteredEnumerate<TBearerStruct>(IEnumerator<TBearerStruct?>? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TBearerStruct?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TBearerStruct);
        var any         = false;
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
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
                var filterResult = filterPredicate?.Invoke(count, item) ?? IncludedContinueToNext;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);

                any = true;
                stb.RevealNullableStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount, "", formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFiltered(string?[]? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(string?[]);
        if (stb.HasSkipBody(actualType, "", formatFlags)) return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(string);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFiltered(Span<string> value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<string>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(string);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFilteredNullable(Span<string?> value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<string?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(string);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFiltered(ReadOnlySpan<string> value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<string>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(string);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFilteredNullable(ReadOnlySpan<string?> value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<string?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(string);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFiltered(IReadOnlyList<string?>? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<string?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(string);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFilteredEnumerate(IEnumerable<string?>? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<string?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(string);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            formatString ??= "";
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }


    public TOCMold AddFilteredEnumerate(IEnumerator<string?>? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<string?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(string);
        var any         = false;
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        if (hasValue)
        {
            formatString ??= "";
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
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);

                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFilteredCharSeq<TCharSeq, TCharSeqBase>(TCharSeq[]? value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?, TCharSeqBase?
    {
        var actualType = value?.GetType() ?? typeof(TCharSeq[]);
        if (stb.HasSkipBody(actualType, "", formatFlags)) 
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCharSeq);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFilteredCharSeq<TCharSeq, TCharSeqBase>(Span<TCharSeq> value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?, TCharSeqBase?
    {
        var actualType = typeof(Span<TCharSeq>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCharSeq);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFilteredCharSeq<TCharSeq, TCharSeqBase>(ReadOnlySpan<TCharSeq> value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?, TCharSeqBase?
    {
        var actualType = typeof(ReadOnlySpan<TCharSeq>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCharSeq);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFilteredCharSeq<TCharSeq, TCharSeqBase>(IReadOnlyList<TCharSeq>? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?, TCharSeqBase?
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TCharSeq>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCharSeq);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFilteredCharSeqEnumerate<TCharSeq, TCharSeqBase>(IEnumerable<TCharSeq?>? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence, TCharSeqBase
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TCharSeq?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCharSeq);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            formatString ??= "";
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }


    public TOCMold AddFilteredCharSeqEnumerate<TCharSeq, TCharSeqBase>(IEnumerator<TCharSeq?>? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence, TCharSeqBase
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TCharSeq?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCharSeq);
        var any         = false;
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        if (hasValue)
        {
            formatString ??= "";
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
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);

                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString, formatFlags);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFiltered(StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(StringBuilder?[]);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(StringBuilder);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }


    public TOCMold AddFiltered(Span<StringBuilder> value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<StringBuilder>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(StringBuilder);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }


    public TOCMold AddFilteredNullable(Span<StringBuilder?> value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<StringBuilder?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(StringBuilder);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFiltered(ReadOnlySpan<StringBuilder> value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<StringBuilder>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(StringBuilder);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFilteredNullable(ReadOnlySpan<StringBuilder?> value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<StringBuilder?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(StringBuilder);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFiltered(IReadOnlyList<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<StringBuilder?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(StringBuilder);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFilteredEnumerate(IEnumerable<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<StringBuilder?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(StringBuilder);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            formatString ??= "";
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }


    public TOCMold AddFilteredEnumerate(IEnumerator<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<StringBuilder?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(StringBuilder);
        var any         = false;
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        if (hasValue)
        {
            formatString ??= "";
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
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);

                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFilteredMatch<TAny, TAnyBase>(TAny[]? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TAny : TAnyBase?
    {
        var actualType = value?.GetType() ?? typeof(TAny[]);
        if (stb.HasSkipBody(actualType, "", formatFlags)) 
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TAny);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFilteredMatch<TAny, TAnyBase>(Span<TAny> value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TAny : TAnyBase?
    {
        var actualType = typeof(Span<TAny>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TAny);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFilteredMatch<TAny, TAnyBase>(ReadOnlySpan<TAny> value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TAny : TAnyBase?
    {
        var actualType = typeof(ReadOnlySpan<TAny>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TAny);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFilteredMatch<TAny, TAnyBase>(IReadOnlyList<TAny>? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TAny : TAnyBase?
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TAny?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TAny);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFilteredMatchEnumerate<TAny, TAnyBase>(IEnumerable<TAny?>? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TAny : TAnyBase
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TAny?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TAny);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            formatString ??= "";
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, itemCount, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddFilteredMatchEnumerate<TAny, TAnyBase>(IEnumerator<TAny?>? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TAny : TAnyBase?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TAny?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TAny);
        var any         = false;
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        if (hasValue)
        {
            formatString ??= "";
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
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
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

                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, itemCount, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TOCMold AddFilteredObject(object?[]? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(object?[]);
        if (stb.HasSkipBody(actualType, "", formatFlags)) return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(object);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TOCMold AddFilteredObject(Span<object> value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<object>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(object);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TOCMold AddFilteredObjectNullable(Span<object?> value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<object?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(object);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TOCMold AddFilteredObject(ReadOnlySpan<object> value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<object>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(object);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TOCMold AddFilteredObjectNullable(ReadOnlySpan<object?> value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<object?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(object);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TOCMold AddFilteredObject(IReadOnlyList<object?>? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<object?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(object);
        var any         = false;
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
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TOCMold AddFilteredObjectEnumerate(IEnumerable<object?>? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<object?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(object);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            formatString ??= "";
            var count     = 0;
            var skipCount = 0;
            foreach (var item in value)
            {
                count++;
                if (skipCount-- > 0) continue;
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        skipCount = filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, itemCount, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TOCMold AddFilteredObjectEnumerate(IEnumerator<object?>? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<object?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(object);
        var any         = false;
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        if (hasValue)
        {
            formatString ??= "";
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
                var filterResult = filterPredicate?.Invoke(count, item!) ?? IncludedContinueToNext;
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

                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, itemCount, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount, formatString, formatFlags);
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
}
