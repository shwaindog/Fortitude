// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeOrderedCollection;

public partial class OrderedCollectionMold<TExt> where TExt : TypeMolder
{
    public TExt AddFiltered(bool[]? value, OrderedCollectionPredicate<bool> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool);
        var any = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFiltered(bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool?);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFiltered(Span<bool> value, OrderedCollectionPredicate<bool> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFiltered(Span<bool?> value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool?);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFiltered(ReadOnlySpan<bool> value, OrderedCollectionPredicate<bool> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFiltered(ReadOnlySpan<bool?> value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool?);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFiltered(IReadOnlyList<bool>? value, OrderedCollectionPredicate<bool> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFiltered(IReadOnlyList<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool?);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFilteredEnumerate(IEnumerable<bool>? value, OrderedCollectionPredicate<bool> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool);
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendCollectionItem(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFilteredEnumerate(IEnumerable<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool?);
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendCollectionItem(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFilteredEnumerate(IEnumerator<bool>? value, OrderedCollectionPredicate<bool> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool);
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                
                any = true;
                stb.AppendCollectionItem(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFilteredEnumerate(IEnumerator<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool?);
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                
                any = true;
                stb.AppendCollectionItem(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFiltered<TFmt, TFmtBase>(TFmt?[]? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable, TFmtBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmt);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFiltered<TFmtStruct>(TFmtStruct?[]? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmtStruct?);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFiltered<TFmt, TFmtBase>(Span<TFmt> value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable, TFmtBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmt);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFilteredNullable<TFmt, TFmtBase>(Span<TFmt?> value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : class, ISpanFormattable, TFmtBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmt);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFiltered<TFmtStruct>(Span<TFmtStruct?> value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmtStruct?);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFiltered<TFmt, TFmtBase>(ReadOnlySpan<TFmt> value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable, TFmtBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmt);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFilteredNullable<TFmt, TFmtBase>(ReadOnlySpan<TFmt?> value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : class, ISpanFormattable, TFmtBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmt);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFiltered<TFmtStruct>(ReadOnlySpan<TFmtStruct?> value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmtStruct?);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFiltered<TFmt, TFmtBase>(IReadOnlyList<TFmt?>? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable, TFmtBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmt);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFiltered<TFmtStruct>(IReadOnlyList<TFmtStruct?>? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmtStruct?);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFilteredEnumerate<TFmt, TFmtBase>(IEnumerable<TFmt?>? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable, TFmtBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmt);
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, itemCount, formatString ?? "");
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFilteredEnumerate<TFmtStruct>(IEnumerable<TFmtStruct?>? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmtStruct?);
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, itemCount, formatString ?? "");
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFilteredEnumerate<TFmt, TFmtBase>(IEnumerator<TFmt?>? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable, TFmtBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmt);
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                
                any = true;
                stb.AppendFormattedCollectionItem(item, itemCount, formatString ?? "");
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFilteredEnumerate<TFmtStruct>(IEnumerator<TFmtStruct?>? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmtStruct?);
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                
                any = true;
                stb.AppendFormattedCollectionItem(item, itemCount, formatString ?? "");
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt RevealFiltered<TCloaked, TCloakedFilterBase, TCloakedRevealBase>(TCloaked?[]? value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TCloakedRevealBase> palantírReveal) where TCloaked : TCloakedFilterBase, TCloakedRevealBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCloaked);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt RevealFiltered<TCloakedStruct>(TCloakedStruct?[]? value, OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCloakedStruct);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt RevealFiltered<TCloaked, TCloakedFilterBase, TCloakedRevealBase>(Span<TCloaked> value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TCloakedRevealBase> palantírReveal) where TCloaked : TCloakedFilterBase, TCloakedRevealBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCloaked);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt RevealFilteredNullable<TCloaked, TCloakedFilterBase, TCloakedRevealBase>(Span<TCloaked?> value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TCloakedRevealBase> palantírReveal) where TCloaked : class, TCloakedFilterBase, TCloakedRevealBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCloaked);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt RevealFiltered<TCloakedStruct>(Span<TCloakedStruct?> value, OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCloakedStruct);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt RevealFiltered<TCloaked, TCloakedFilterBase, TCloakedRevealBase>(ReadOnlySpan<TCloaked> value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TCloakedRevealBase> palantírReveal) where TCloaked : TCloakedFilterBase, TCloakedRevealBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCloaked);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt RevealFiltered<TCloaked, TCloakedFilterBase, TCloakedRevealBase>(IReadOnlyList<TCloaked?>? value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
      , PalantírReveal<TCloakedRevealBase> palantírReveal) where TCloaked : TCloakedFilterBase, TCloakedRevealBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCloaked);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt RevealFiltered<TCloakedStruct>(IReadOnlyList<TCloakedStruct?>? value, 
        OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCloakedStruct);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt RevealFilteredEnumerate<TCloaked, TCloakedFilterBase, TCloakedRevealBase>(IEnumerable<TCloaked?>? value
      , OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate, PalantírReveal<TCloakedRevealBase> palantírReveal)
        where TCloaked : TCloakedFilterBase, TCloakedRevealBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt RevealFilteredEnumerate<TCloakedStruct>(IEnumerable<TCloakedStruct?>? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate, PalantírReveal<TCloakedStruct> palantírReveal)
        where TCloakedStruct : struct
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealFilteredEnumerate<TCloaked, TCloakedFilterBase, TCloakedRevealBase>(IEnumerator<TCloaked?>? value
      , OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate, PalantírReveal<TCloakedRevealBase> palantírReveal) 
        where TCloaked : TCloakedFilterBase, TCloakedRevealBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                
                any = true;
                stb.AppendOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt RevealFilteredEnumerate<TCloakedStruct>(IEnumerator<TCloakedStruct?>? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate, PalantírReveal<TCloakedStruct> palantírReveal) 
        where TCloakedStruct : struct
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                
                any = true;
                stb.AppendOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt RevealFiltered<TBearer, TBearerBase>(TBearer?[]? value, OrderedCollectionPredicate<TBearerBase> filterPredicate)
    where TBearer : IStringBearer, TBearerBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TBearer);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendRevealBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt RevealFiltered<TBearerStruct>(TBearerStruct?[]? value, OrderedCollectionPredicate<TBearerStruct?> filterPredicate)
    where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TBearerStruct);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendRevealBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }


    public TExt RevealFiltered<TBearer, TBearerBase>(Span<TBearer> value, OrderedCollectionPredicate<TBearerBase> filterPredicate)
        where TBearer : IStringBearer, TBearerBase  
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TBearer);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendRevealBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt RevealFilteredNullable<TBearer, TBearerBase>(Span<TBearer?> value, OrderedCollectionPredicate<TBearerBase> filterPredicate)
        where TBearer : class, IStringBearer, TBearerBase  
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TBearer);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendRevealBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt RevealFiltered<TBearerStruct>(Span<TBearerStruct?> value, OrderedCollectionPredicate<TBearerStruct?> filterPredicate)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TBearerStruct);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendRevealBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt RevealFiltered<TBearer, TBearerBase>(ReadOnlySpan<TBearer> value, OrderedCollectionPredicate<TBearerBase> filterPredicate)
        where TBearer : IStringBearer, TBearerBase  
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TBearer);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendRevealBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealFilteredNullable<TBearer, TBearerBase>(ReadOnlySpan<TBearer?> value, OrderedCollectionPredicate<TBearerBase> filterPredicate)
        where TBearer : class, IStringBearer, TBearerBase  
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TBearer);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendRevealBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt RevealFiltered<TBearerStruct>(ReadOnlySpan<TBearerStruct?> value, OrderedCollectionPredicate<TBearerStruct?> filterPredicate)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TBearerStruct);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendRevealBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealFiltered<TBearer, TBearerBase>(IReadOnlyList<TBearer?>? value, OrderedCollectionPredicate<TBearerBase> filterPredicate)
        where TBearer : IStringBearer, TBearerBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TBearer);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendRevealBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealFiltered<TBearerStruct>(IReadOnlyList<TBearerStruct?>? value, OrderedCollectionPredicate<TBearerStruct?> filterPredicate)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TBearerStruct);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendRevealBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealFilteredEnumerate<TBearer, TBearerBase>(IEnumerable<TBearer?>? value, OrderedCollectionPredicate<TBearerBase> filterPredicate)
        where TBearer : IStringBearer, TBearerBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendRevealBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealFilteredEnumerate<TBearerStruct>(IEnumerable<TBearerStruct?>? value, OrderedCollectionPredicate<TBearerStruct?> filterPredicate)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendRevealBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt RevealFilteredEnumerate<TBearer, TBearerBase>(IEnumerator<TBearer?>? value, OrderedCollectionPredicate<TBearerBase> filterPredicate)
        where TBearer : IStringBearer, TBearerBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                
                any = true;
                stb.AppendRevealBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt RevealFilteredEnumerate<TBearerStruct>(IEnumerator<TBearerStruct?>? value, OrderedCollectionPredicate<TBearerStruct?> filterPredicate)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                
                any = true;
                stb.AppendRevealBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFiltered(string?[]? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(string);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFiltered(Span<string> value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(string);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFilteredNullable(Span<string?> value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(string);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFiltered(ReadOnlySpan<string> value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(string);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFilteredNullable(ReadOnlySpan<string?> value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(string);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFiltered(IReadOnlyList<string?>? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(string);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFilteredEnumerate(IEnumerable<string?>? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(string);
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }


    public TExt AddFilteredEnumerate(IEnumerator<string?>? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(string);
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFilteredCharSeq<TCharSeq, TCharSeqBase>(TCharSeq?[]? value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TCharSeq : ICharSequence, TCharSeqBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCharSeq);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFilteredCharSeq<TCharSeq, TCharSeqBase>(Span<TCharSeq> value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    where TCharSeq : ICharSequence, TCharSeqBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCharSeq);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFilteredCharSeqNullable<TCharSeq, TCharSeqBase>(Span<TCharSeq?> value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    where TCharSeq : ICharSequence, TCharSeqBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCharSeq);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFilteredCharSeq<TCharSeq, TCharSeqBase>(ReadOnlySpan<TCharSeq> value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    where TCharSeq : ICharSequence, TCharSeqBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCharSeq);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFilteredCharSeqNullable<TCharSeq, TCharSeqBase>(ReadOnlySpan<TCharSeq?> value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    where TCharSeq : ICharSequence, TCharSeqBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCharSeq);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFilteredCharSeq<TCharSeq, TCharSeqBase>(IReadOnlyList<TCharSeq?>? value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    where TCharSeq : ICharSequence, TCharSeqBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCharSeq);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFilteredCharSeqEnumerate<TCharSeq, TCharSeqBase>(IEnumerable<TCharSeq?>? value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TCharSeq : ICharSequence, TCharSeqBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCharSeq);
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }


    public TExt AddFilteredCharSeqEnumerate<TCharSeq, TCharSeqBase>(IEnumerator<TCharSeq?>? value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    where TCharSeq : ICharSequence, TCharSeqBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCharSeq);
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, itemCount);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFiltered(StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(StringBuilder);
        var any = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }


    public TExt AddFiltered(Span<StringBuilder> value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(StringBuilder);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }


    public TExt AddFilteredNullable(Span<StringBuilder?> value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(StringBuilder);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFiltered(ReadOnlySpan<StringBuilder> value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(StringBuilder);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFilteredNullable(ReadOnlySpan<StringBuilder?> value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(StringBuilder);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFiltered(IReadOnlyList<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(StringBuilder);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFilteredEnumerate(IEnumerable<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(StringBuilder);
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }


    public TExt AddFilteredEnumerate(IEnumerator<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(StringBuilder);
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFilteredMatch<TAny, TAnyBase>(TAny?[]? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TAny : TAnyBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TAny);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemMatchOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFilteredMatch<TAny, TAnyBase>(Span<TAny> value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TAny : TAnyBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TAny);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemMatchOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFilteredMatchNullable<TAny, TAnyBase>(Span<TAny?> value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TAny : TAnyBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TAny);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemMatchOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFilteredMatch<TAny, TAnyBase>(ReadOnlySpan<TAny> value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TAny : TAnyBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TAny);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemMatchOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFilteredMatchNullable<TAny, TAnyBase>(ReadOnlySpan<TAny?> value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TAny : TAnyBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TAny);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemMatchOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFilteredMatch<TAny, TAnyBase>(IReadOnlyList<TAny?>? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TAny : TAnyBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TAny);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemMatchOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFilteredMatchEnumerate<TAny, TAnyBase>(IEnumerable<TAny?>? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TAny : TAnyBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TAny);
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
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemMatchOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemMatchOrNull(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFilteredMatchEnumerate<TAny, TAnyBase>(IEnumerator<TAny?>? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TAny : TAnyBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TAny);
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
                
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemMatchOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemMatchOrNull(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString] 
    public TExt AddFilteredObject(object?[]? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(object);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemMatchOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    [CallsObjectToString] 
    public TExt AddFilteredObject(Span<object> value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(object);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemMatchOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    [CallsObjectToString] 
    public TExt AddFilteredObjectNullable(Span<object?> value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(object);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemMatchOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    [CallsObjectToString] 
    public TExt AddFilteredObject(ReadOnlySpan<object> value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(object);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemMatchOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    [CallsObjectToString] 
    public TExt AddFilteredObjectNullable(ReadOnlySpan<object?> value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(object);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemMatchOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    [CallsObjectToString] 
    public TExt AddFilteredObject(IReadOnlyList<object?>? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(object);
        var any         = false;
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
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemMatchOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    [CallsObjectToString] 
    public TExt AddFilteredObjectEnumerate(IEnumerable<object?>? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(object);
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
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemMatchOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemMatchOrNull(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString] 
    public TExt AddFilteredObjectEnumerate(IEnumerator<object?>? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(object);
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
                
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemMatchOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemMatchOrNull(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
                if (filterResult is { KeepProcessing: false }) break;
                skipCount = filterResult.SkipNextCount;
                hasValue  = value.MoveNext();
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

}
