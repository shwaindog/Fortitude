// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable.Strings;
#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeOrderedCollection;

public partial class OrderedCollectionBuilder<TExt> where TExt : StyledTypeBuilder
{
    public TExt AddFiltered(bool[]? value, OrderedCollectionPredicate<bool> itemPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool);
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFiltered(bool?[]? value, OrderedCollectionPredicate<bool?> itemPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool?);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    
    public TExt AddFiltered(ReadOnlySpan<bool> value, OrderedCollectionPredicate<bool> itemPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFiltered(ReadOnlySpan<bool?> value, OrderedCollectionPredicate<bool?> itemPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool?);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFiltered(IReadOnlyList<bool>? value, OrderedCollectionPredicate<bool> itemPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFiltered(IReadOnlyList<bool?>? value, OrderedCollectionPredicate<bool?> itemPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool?);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFilteredEnumerate(IEnumerable<bool>? value, OrderedCollectionPredicate<bool> itemPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            var count = 0;
            foreach (var item in value)
            {
                if (!itemPredicate(count++, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendCollectionItem(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFilteredEnumerate(IEnumerable<bool?>? value, OrderedCollectionPredicate<bool?> itemPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool?);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            var count = 0;
            foreach (var item in value)
            {
                if (!itemPredicate(count++, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendCollectionItem(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFilteredEnumerate(IEnumerator<bool>? value, OrderedCollectionPredicate<bool> itemPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool);
        var any         = false;
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        if (hasValue)
        {
            var count = 0;
            while (hasValue)
            {
                var item = value!.Current;
                if (!itemPredicate(count++, item))
                {
                    hasValue = value.MoveNext();
                    continue;
                }
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                
                any = true;
                stb.AppendCollectionItem(item, itemCount);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFilteredEnumerate(IEnumerator<bool?>? value, OrderedCollectionPredicate<bool?> itemPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool?);
        var any         = false;
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        if (hasValue)
        {
            var count = 0;
            while (hasValue)
            {
                var item = value!.Current;
                if (!itemPredicate(count++, item))
                {
                    hasValue = value.MoveNext();
                    continue;
                }
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                
                any = true;
                stb.AppendCollectionItem(item, itemCount);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFiltered<TFmt, TBase>(TFmt[]? value, OrderedCollectionPredicate<TBase> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable, TBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmt);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItem(item, i, formatString);
                else
                    stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFiltered<TFmtStruct>(TFmtStruct?[]? value, OrderedCollectionPredicate<TFmtStruct?> itemPredicate
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
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItem(item, i, formatString);
                else
                    stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    
    public TExt AddFiltered<TFmt, TBase>(ReadOnlySpan<TFmt> value, OrderedCollectionPredicate<TBase> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable, TBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmt);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItem(item, i, formatString);
                else
                    stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFiltered<TFmtStruct>(ReadOnlySpan<TFmtStruct?> value, OrderedCollectionPredicate<TFmtStruct?> itemPredicate
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
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItem(item, i, formatString);
                else
                    stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFiltered<TFmt, TBase>(IReadOnlyList<TFmt>? value, OrderedCollectionPredicate<TBase> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable, TBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmt);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItem(item, i, formatString);
                else
                    stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFiltered<TFmtStruct>(IReadOnlyList<TFmtStruct?>? value, OrderedCollectionPredicate<TFmtStruct?> itemPredicate
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
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItem(item, i, formatString);
                else
                    stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFilteredEnumerate<TFmt, TBase>(IEnumerable<TFmt>? value, OrderedCollectionPredicate<TBase> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable, TBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmt);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            var count = 0;
            foreach (var item in value)
            {
                if (!itemPredicate(count++, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItem(item, itemCount, formatString);
                else
                    stb.AppendCollectionItem(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFilteredEnumerate<TFmtStruct>(IEnumerable<TFmtStruct?>? value, OrderedCollectionPredicate<TFmtStruct?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmtStruct?);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            var count = 0;
            foreach (var item in value)
            {
                if (!itemPredicate(count++, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItem(item, itemCount, formatString);
                else
                    stb.AppendCollectionItem(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFilteredEnumerate<TFmt, TBase>(IEnumerator<TFmt>? value, OrderedCollectionPredicate<TBase> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable, TBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmt);
        var any         = false;
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        if (hasValue)
        {
            var count = 0;
            while (hasValue)
            {
                var item = value!.Current;
                if (!itemPredicate(count++, item))
                {
                    hasValue = value.MoveNext();
                    continue;
                }
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItem(item, itemCount, formatString);
                else
                    stb.AppendCollectionItem(item, itemCount);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFilteredEnumerate<TFmtStruct>(IEnumerator<TFmtStruct?>? value, OrderedCollectionPredicate<TFmtStruct?> itemPredicate
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
            var count = 0;
            while (hasValue)
            {
                var item = value!.Current;
                if (!itemPredicate(count++, item))
                {
                    hasValue = value.MoveNext();
                    continue;
                }
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItem(item, itemCount, formatString);
                else
                    stb.AppendCollectionItem(item, itemCount);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFiltered<T, TBase1, TBase2>(T[]? value, OrderedCollectionPredicate<TBase1> itemPredicate
      , CustomTypeStyler<TBase2> customTypeStyler) where T : TBase1, TBase2
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(T);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendOrNull(item, customTypeStyler);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFiltered<T, TBase1, TBase2>(ReadOnlySpan<T> value, OrderedCollectionPredicate<TBase1> itemPredicate
      , CustomTypeStyler<TBase2> customTypeStyler) where T : TBase1, TBase2
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(T);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendOrNull(item, customTypeStyler);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFiltered<T, TBase1, TBase2>(IReadOnlyList<T>? value, OrderedCollectionPredicate<TBase1> itemPredicate
      , CustomTypeStyler<TBase2> customTypeStyler) where T : TBase1, TBase2
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(T);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendOrNull(item, customTypeStyler);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFilteredEnumerate<T, TBase1, TBase2>(IEnumerable<T>? value, OrderedCollectionPredicate<TBase1> itemPredicate
      , CustomTypeStyler<TBase2> customTypeStyler) where T : TBase1, TBase2
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(T);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            var count = 0;
            foreach (var item in value)
            {
                if (!itemPredicate(count++, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendOrNull(item, customTypeStyler);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFilteredEnumerate<T, TBase1, TBase2>(IEnumerator<T>? value, OrderedCollectionPredicate<TBase1> itemPredicate
      , CustomTypeStyler<TBase2> customTypeStyler) where T : TBase1, TBase2
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(T);
        var any         = false;
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        if (hasValue)
        {
            var count = 0;
            while (hasValue)
            {
                var item = value!.Current;
                if (!itemPredicate(count++, item))
                {
                    hasValue = value.MoveNext();
                    continue;
                }
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                
                any = true;
                stb.AppendOrNull(item, customTypeStyler);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFiltered(string?[]? value, OrderedCollectionPredicate<string?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(string);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }


    public TExt AddFiltered(ReadOnlySpan<string?> value, OrderedCollectionPredicate<string?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(string);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFiltered(IReadOnlyList<string?>? value, OrderedCollectionPredicate<string?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(string);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFilteredEnumerate(IEnumerable<string?>? value, OrderedCollectionPredicate<string?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(string);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            var count = 0;
            foreach (var item in value)
            {
                if (!itemPredicate(count++, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }


    public TExt AddFilteredEnumerate(IEnumerator<string?>? value, OrderedCollectionPredicate<string?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(string);
        var any         = false;
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        if (hasValue)
        {
            var count = 0;
            while (hasValue)
            {
                var item = value!.Current;
                if (!itemPredicate(count++, item))
                {
                    hasValue = value.MoveNext();
                    continue;
                }
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, itemCount);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFilteredCharSequence<TCharSeq>(TCharSeq?[]? value, OrderedCollectionPredicate<ICharSequence?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TCharSeq : ICharSequence
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCharSeq);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }


    public TExt AddFilteredCharSequence<TCharSeq>(ReadOnlySpan<TCharSeq?> value, OrderedCollectionPredicate<ICharSequence?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    where TCharSeq : ICharSequence
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCharSeq);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFilteredCharSequence<TCharSeq>(IReadOnlyList<TCharSeq?>? value, OrderedCollectionPredicate<ICharSequence?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    where TCharSeq : ICharSequence
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCharSeq);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFilteredCharSequenceEnumerate<TCharSeq>(IEnumerable<TCharSeq?>? value, OrderedCollectionPredicate<ICharSequence?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TCharSeq : ICharSequence
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCharSeq);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            var count = 0;
            foreach (var item in value)
            {
                if (!itemPredicate(count++, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }


    public TExt AddFilteredCharSequenceEnumerate<TCharSeq>(IEnumerator<TCharSeq?>? value, OrderedCollectionPredicate<ICharSequence?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    where TCharSeq : ICharSequence
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCharSeq);
        var any         = false;
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        if (hasValue)
        {
            var count = 0;
            while (hasValue)
            {
                var item = value!.Current;
                if (!itemPredicate(count++, item))
                {
                    hasValue = value.MoveNext();
                    continue;
                }
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, itemCount);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFiltered(StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(StringBuilder);
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }


    public TExt AddFiltered(ReadOnlySpan<StringBuilder?> value, OrderedCollectionPredicate<StringBuilder?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(StringBuilder);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFiltered(IReadOnlyList<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(StringBuilder);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFilteredEnumerate(IEnumerable<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(StringBuilder);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            var count = 0;
            foreach (var item in value)
            {
                if (!itemPredicate(count++, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }


    public TExt AddFilteredEnumerate(IEnumerator<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(StringBuilder);
        var any         = false;
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        if (hasValue)
        {
            var count = 0;
            while (hasValue)
            {
                var item = value!.Current;
                if (!itemPredicate(count++, item))
                {
                    hasValue = value.MoveNext();
                    continue;
                }
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, itemCount);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddFilteredStyled<TStyledObj, TBase>(TStyledObj[]? value, OrderedCollectionPredicate<TBase> itemPredicate)
    where TStyledObj : IStyledToStringObject, TBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TStyledObj);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }


    public TExt AddFilteredStyled<TStyledObj, TBase>(ReadOnlySpan<TStyledObj> value, OrderedCollectionPredicate<TBase> itemPredicate)
        where TStyledObj : IStyledToStringObject, TBase  
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TStyledObj);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFilteredStyled<TStyledObj, TBase>(IReadOnlyList<TStyledObj>? value, OrderedCollectionPredicate<TBase> itemPredicate)
        where TStyledObj : IStyledToStringObject, TBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TStyledObj);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddFilteredStyledEnumerate<TStyledObj, TBase>(IEnumerable<TStyledObj>? value, OrderedCollectionPredicate<TBase> itemPredicate)
        where TStyledObj : IStyledToStringObject, TBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TStyledObj);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            var count = 0;
            foreach (var item in value)
            {
                if (!itemPredicate(count++, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }


    public TExt AddFilteredStyledEnumerate<TStyledObj, TBase>(IEnumerator<TStyledObj>? value, OrderedCollectionPredicate<TBase> itemPredicate)
        where TStyledObj : IStyledToStringObject, TBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TStyledObj);
        var any         = false;
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        if (hasValue)
        {
            var count = 0;
            while (hasValue)
            {
                var item = value!.Current;
                if (!itemPredicate(count++, item))
                {
                    hasValue = value.MoveNext();
                    continue;
                }
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                
                any = true;
                stb.AppendOrNull(item);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    [CallsObjectToString] 
    public TExt AddFilteredMatch<T, TBase>(T[]? value, OrderedCollectionPredicate<TBase> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where T : TBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(T);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemMatchOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    [CallsObjectToString] 
    public TExt AddFilteredMatch<T, TBase>(ReadOnlySpan<T> value, OrderedCollectionPredicate<TBase> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where T : TBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(T);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemMatchOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    [CallsObjectToString] 
    public TExt AddFilteredMatch<T, TBase>(IReadOnlyList<T>? value, OrderedCollectionPredicate<TBase> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where T : TBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(T);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemMatchOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    [CallsObjectToString] 
    public TExt AddFilteredMatchEnumerate<T, TBase>(IEnumerable<T>? value, OrderedCollectionPredicate<TBase> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where T : TBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(T);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            var count = 0;
            foreach (var item in value)
            {
                if (!itemPredicate(count++, item)) continue;
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
    public TExt AddFilteredMatchEnumerate<T, TBase>(IEnumerator<T>? value, OrderedCollectionPredicate<TBase> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where T : TBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(T);
        var any         = false;
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        if (hasValue)
        {
            var count = 0;
            while (hasValue)
            {
                var item = value!.Current;
                if (!itemPredicate(count++, item))
                {
                    hasValue = value.MoveNext();
                    continue;
                }
                
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemMatchOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemMatchOrNull(item, itemCount);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

}
