// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeOrderedCollection;

public partial class OrderedCollectionMold<TOCMold> 
    where TOCMold : TypeMolder
{
    public TOCMold AddAll(bool[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool);
        stb.ConditionalCollectionPrefix(elementType, (value?.Length ?? 0)  > 0);
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(bool?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool?);
        stb.ConditionalCollectionPrefix(elementType, (value?.Length ?? 0) > 0);
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(Span<bool> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool);
        stb.ConditionalCollectionPrefix(elementType, value.Length > 0);
        
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(ReadOnlySpan<bool> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool);
        stb.ConditionalCollectionPrefix(elementType, value.Length > 0);
        
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(Span<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool?);
        stb.ConditionalCollectionPrefix(elementType, value.Length > 0);
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(ReadOnlySpan<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool?);
        stb.ConditionalCollectionPrefix(elementType, value.Length > 0);
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(IReadOnlyList<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool);
        stb.ConditionalCollectionPrefix(elementType, (value?.Count ?? 0) > 0);
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(IReadOnlyList<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool?);
        stb.ConditionalCollectionPrefix(elementType, (value?.Count ?? 0) > 0);
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate(IEnumerable<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            formatString ??= "";
            foreach (var item in value)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, itemCount, formatString);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate(IEnumerable<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            formatString ??= "";
            foreach (var item in value)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, itemCount, formatString);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate(IEnumerator<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool);
        var any       = false;
        var itemCount = 0;
        var hasValue  = value?.MoveNext() ?? false;
        if (hasValue)
        {
            formatString ??= "";
            while (hasValue)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value!.Current;
                
                any = true;
                stb.AppendFormattedCollectionItem(item, itemCount, formatString);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate(IEnumerator<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool);
        var any       = false;
        var itemCount = 0;
        var hasValue  = value?.MoveNext() ?? false;
        if (hasValue)
        {
            formatString ??= "";
            while (hasValue)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value!.Current;
                
                any = true;
                stb.AppendFormattedCollectionItem(item, itemCount, formatString);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll<TFmt>(TFmt?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmt);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll<TFmtStruct>(TFmtStruct?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmtStruct?);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TOCMold AddAll<TFmt>(Span<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmt);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TOCMold AddAllNullable<TFmt>(Span<TFmt?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : class, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmt);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TOCMold AddAll<TFmt>(ReadOnlySpan<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmt);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TOCMold AddAllNullable<TFmt>(ReadOnlySpan<TFmt?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : class, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmt);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TOCMold AddAll<TFmtStruct>(Span<TFmtStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmtStruct?);
        var any      = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TOCMold AddAll<TFmtStruct>(ReadOnlySpan<TFmtStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmtStruct?);
        var any      = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TOCMold AddAll<TFmt>(IReadOnlyList<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmt);
        var any      = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TOCMold AddAll<TFmtStruct>(IReadOnlyList<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmtStruct?);
        var any      = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate<TFmt>(IEnumerable<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmt);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            formatString ??= "";
            foreach (var item in value)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, itemCount, formatString);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate<TFmtStruct>(IEnumerable<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmtStruct?);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            formatString ??= "";
            foreach (var item in value)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, itemCount, formatString);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate<TFmt>(IEnumerator<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmt);
        var any         = false;
        var itemCount   = 0;
        var hasValue    = value?.MoveNext() ?? false;
        if (hasValue)
        {
            formatString ??= "";
            while (hasValue)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                var item = value!.Current;
                stb.AppendFormattedCollectionItem(item, itemCount, formatString);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate<TFmtStruct>(IEnumerator<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmtStruct?);
        var any         = false;
        var itemCount   = 0;
        var hasValue    = value?.MoveNext() ?? false;
        if (hasValue)
        {
            formatString ??= "";
            while (hasValue)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                var item = value!.Current;
                stb.AppendFormattedCollectionItem(item, itemCount, formatString);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TOCMold RevealAll<TCloaked, TCloakedBase>(TCloaked?[]? value, PalantírReveal<TCloakedBase> palantírReveal)
        where TCloaked : TCloakedBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCloaked);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.RevealCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    
    public TOCMold RevealAll<TCloakedStruct>(TCloakedStruct?[]? value, PalantírReveal<TCloakedStruct> palantírReveal)
        where TCloakedStruct : struct
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCloakedStruct);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.RevealNullableCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TCloaked, TCloakedBase>(Span<TCloaked> value, PalantírReveal<TCloakedBase> palantírReveal)
        where TCloaked : TCloakedBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCloaked);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.RevealCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAllNullable<TCloaked, TCloakedBase>(Span<TCloaked?> value, PalantírReveal<TCloakedBase> palantírReveal)
        where TCloaked : class, TCloakedBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCloaked);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.RevealCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TCloakedStruct>(Span<TCloakedStruct?> value, PalantírReveal<TCloakedStruct> palantírReveal)
        where TCloakedStruct : struct
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCloakedStruct);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.RevealNullableCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TCloaked, TCloakedBase>(ReadOnlySpan<TCloaked> value, PalantírReveal<TCloakedBase> palantírReveal)
        where TCloaked : TCloakedBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCloaked);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.RevealCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAllNullable<TCloaked, TCloakedBase>(ReadOnlySpan<TCloaked?> value, PalantírReveal<TCloakedBase> palantírReveal)
        where TCloaked : class, TCloakedBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCloaked);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.RevealCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TCloakedStruct>(ReadOnlySpan<TCloakedStruct?> value, PalantírReveal<TCloakedStruct> palantírReveal)
        where TCloakedStruct : struct
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCloakedStruct);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.RevealNullableCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TCloaked, TCloakedBase>(IReadOnlyList<TCloaked?>? value, PalantírReveal<TCloakedBase> palantírReveal)
        where TCloaked : TCloakedBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCloaked);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.RevealCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TCloakedStruct>(IReadOnlyList<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal)
        where TCloakedStruct : struct
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCloakedStruct);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.RevealNullableCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAllEnumerate<TCloaked, TCloakedBase>(IEnumerable<TCloaked?>? value, PalantírReveal<TCloakedBase> palantírReveal)
        where TCloaked : TCloakedBase 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCloaked);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            foreach (var item in value)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.RevealCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAllEnumerate<TCloakedStruct>(IEnumerable<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal)
        where TCloakedStruct : struct 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCloakedStruct);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            foreach (var item in value)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.RevealNullableCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAllEnumerate<TCloaked, TCloakedBase>(IEnumerator<TCloaked?>? value, PalantírReveal<TCloakedBase> palantírReveal) 
        where TCloaked : TCloakedBase 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCloaked);
        var any         = false;
        var itemCount   = 0;
        var hasValue    = value?.MoveNext() ?? false;
        if (hasValue)
        {
            while (hasValue)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                var item = value!.Current;
                stb.RevealCloakedBearerOrNull(item, palantírReveal);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAllEnumerate<TCloakedStruct>(IEnumerator<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal) 
        where TCloakedStruct : struct 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCloakedStruct);
        var any         = false;
        var itemCount   = 0;
        var hasValue    = value?.MoveNext() ?? false;
        if (hasValue)
        {
            while (hasValue)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                var item = value!.Current;
                stb.RevealNullableCloakedBearerOrNull(item, palantírReveal);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TBearer>(TBearer?[]? value)
        where TBearer : IStringBearer
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TBearer);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.RevealStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0 );
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TBearer>(TBearer?[]? value)
        where TBearer : struct, IStringBearer
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TBearer);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.RevealNullableStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0 );
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TBearer>(Span<TBearer> value) where TBearer : IStringBearer
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TBearer);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.RevealStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAllNullable<TBearer>(Span<TBearer?> value) where TBearer : class, IStringBearer
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TBearer);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.RevealStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TBearerStruct>(Span<TBearerStruct?> value)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TBearerStruct);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.RevealNullableStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TBearer>(ReadOnlySpan<TBearer> value) where TBearer : IStringBearer
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TBearer);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.RevealStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAllNullable<TBearer>(ReadOnlySpan<TBearer?> value) where TBearer : class, IStringBearer
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TBearer);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.RevealStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TBearerStruct>(ReadOnlySpan<TBearerStruct?> value)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TBearerStruct);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.RevealNullableStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TBearer>(IReadOnlyList<TBearer?>? value)
        where TBearer : IStringBearer
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TBearer);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.RevealStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TBearerStruct>(IReadOnlyList<TBearerStruct?>? value)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TBearerStruct);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.RevealNullableStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAllEnumerate<TBearer>(IEnumerable<TBearer?>? value)
        where TBearer : IStringBearer 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TBearer);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            foreach (var item in value)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.RevealStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAllEnumerate<TBearerStruct>(IEnumerable<TBearerStruct?>? value)
        where TBearerStruct : struct, IStringBearer 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TBearerStruct);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            foreach (var item in value)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.RevealNullableStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAllEnumerate<TBearer>(IEnumerator<TBearer?>? value)
        where TBearer : IStringBearer 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TBearer);
        var any         = false;
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        if (hasValue)
        {
            while (hasValue)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value!.Current;
                
                any = true;
                stb.RevealStringBearerOrNull(item);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAllEnumerate<TBearerStruct>(IEnumerator<TBearerStruct?>? value)
        where TBearerStruct : struct, IStringBearer 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TBearerStruct);
        var any         = false;
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        if (hasValue)
        {
            while (hasValue)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value!.Current;
                
                any = true;
                stb.RevealNullableStringBearerOrNull(item);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(string?[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(string);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(Span<string> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(string);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllNullable(Span<string?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(string);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(ReadOnlySpan<string> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(string);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllNullable(ReadOnlySpan<string?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(string);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(IReadOnlyList<string?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(string);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate(IEnumerable<string?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(string);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            formatString ??= "";
            foreach (var item in value)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate(IEnumerator<string?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(string);
        var any         = false;
        var itemCount   = 0;
        var hasValue    = value?.MoveNext() ?? false;
        if (hasValue)
        {
            formatString ??= "";
            while (hasValue)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value!.Current;
                
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllCharSeq<TCharSeq>(TCharSeq?[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TCharSeq : ICharSequence 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCharSeq);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return any ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllCharSeq<TCharSeq>(Span<TCharSeq> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  
        where TCharSeq : ICharSequence
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCharSeq);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllCharSeqNullable<TCharSeq>(Span<TCharSeq?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  
        where TCharSeq : ICharSequence
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCharSeq);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllCharSeq<TCharSeq>(ReadOnlySpan<TCharSeq> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  
        where TCharSeq : ICharSequence
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCharSeq);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllCharSeqNullable<TCharSeq>(ReadOnlySpan<TCharSeq?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  
        where TCharSeq : ICharSequence
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCharSeq);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllCharSeq<TCharSeq>(IReadOnlyList<TCharSeq?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  
        where TCharSeq : ICharSequence
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCharSeq);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllCharSeqEnumerate<TCharSeq>(IEnumerable<TCharSeq?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TCharSeq : ICharSequence
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCharSeq);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            formatString ??= "";
            foreach (var item in value)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllCharSeqEnumerate<TCharSeq>(IEnumerator<TCharSeq?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TCharSeq : ICharSequence
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCharSeq);
        var any         = false;
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        if (hasValue)
        {
            formatString ??= "";
            while (hasValue)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value!.Current;
                
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(StringBuilder?[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(StringBuilder);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(Span<StringBuilder> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(StringBuilder);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllNullable(Span<StringBuilder?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(StringBuilder);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(ReadOnlySpan<StringBuilder> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(StringBuilder);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllNullable(ReadOnlySpan<StringBuilder?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(StringBuilder);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(IReadOnlyList<StringBuilder?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(StringBuilder);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0 );
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate(IEnumerable<StringBuilder?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(StringBuilder);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            formatString ??= "";
            foreach (var item in value)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate(IEnumerator<StringBuilder?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(StringBuilder);
        var any         = false;
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        if (hasValue)
        {
            formatString ??= "";
            while (hasValue)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value!.Current;
                
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllMatch<TAny>(TAny[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TAny);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllMatch<TAny>(Span<TAny> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TAny);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllMatchNullable<TAny>(Span<TAny?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TAny);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllMatch<TAny>(ReadOnlySpan<TAny> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TAny);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllMatchNullable<TAny>(ReadOnlySpan<TAny?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TAny);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllMatch<TAny>(IReadOnlyList<TAny>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TAny);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllMatchEnumerate<TAny>(IEnumerable<TAny>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TAny);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            formatString ??= "";
            foreach (var item in value)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, itemCount, formatString);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllMatchEnumerate<TAny>(IEnumerator<TAny>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TAny);
        var any         = false;
        var itemCount   = 0;
        var hasValue    = value?.MoveNext() ?? false;
        if (hasValue)
        {
            formatString ??= "";
            while (hasValue)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value!.Current;
                
                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, itemCount, formatString);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    [CallsObjectToString] 
    public TOCMold AddAllObject(object?[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(object);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString] 
    public TOCMold AddAllObject(Span<object> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(object);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString] 
    public TOCMold AddAllObjectNullable(Span<object?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(object);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString] 
    public TOCMold AddAllObject(ReadOnlySpan<object> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(object);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString] 
    public TOCMold AddAllObjectNullable(ReadOnlySpan<object?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(object);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString] 
    public TOCMold AddAllObject(IReadOnlyList<object?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(object);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString] 
    public TOCMold AddAllObjectEnumerate(IEnumerable<object?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(object);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            formatString ??= "";
            foreach (var item in value)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, itemCount, formatString);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString] 
    public TOCMold AddAllObjectEnumerate(IEnumerator<object?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(object);
        var any         = false;
        var itemCount   = 0;
        var hasValue    = value?.MoveNext() ?? false;
        if (hasValue)
        {
            formatString ??= "";
            while (hasValue)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value!.Current;
                
                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, itemCount, formatString);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
}
