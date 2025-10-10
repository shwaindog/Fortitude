// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeOrderedCollection;

public partial class OrderedCollectionMold<TExt> 
    where TExt : TypeMolder
{
    public TExt AddAll(bool[]? value)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool);
        stb.ConditionalCollectionPrefix(elementType, (value?.Length ?? 0)  > 0);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddAll(bool?[]? value)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool?);
        stb.ConditionalCollectionPrefix(elementType, (value?.Length ?? 0) > 0);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddAll(Span<bool> value)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool);
        stb.ConditionalCollectionPrefix(elementType, value.Length > 0);
        
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddAll(ReadOnlySpan<bool> value)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool);
        stb.ConditionalCollectionPrefix(elementType, value.Length > 0);
        
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddAll(Span<bool?> value)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool?);
        stb.ConditionalCollectionPrefix(elementType, value.Length > 0);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddAll(ReadOnlySpan<bool?> value)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool?);
        stb.ConditionalCollectionPrefix(elementType, value.Length > 0);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddAll(IReadOnlyList<bool>? value)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool);
        stb.ConditionalCollectionPrefix(elementType, (value?.Count ?? 0) > 0);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddAll(IReadOnlyList<bool?>? value)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool?);
        stb.ConditionalCollectionPrefix(elementType, (value?.Count ?? 0) > 0);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate(IEnumerable<bool>? value)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            foreach (var item in value)
            {
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

    public TExt AddAllEnumerate(IEnumerable<bool?>? value)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            foreach (var item in value)
            {
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

    public TExt AddAllEnumerate(IEnumerator<bool>? value)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool);
        var any       = false;
        var itemCount = 0;
        var hasValue  = value?.MoveNext() ?? false;
        if (hasValue)
        {
            while (hasValue)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value!.Current;
                
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

    public TExt AddAllEnumerate(IEnumerator<bool?>? value)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(bool);
        var any       = false;
        var itemCount = 0;
        var hasValue  = value?.MoveNext() ?? false;
        if (hasValue)
        {
            while (hasValue)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value!.Current;
                
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

    public TExt AddAll<TFmt>(TFmt?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmt);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddAll<TFmtStruct>(TFmtStruct?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmtStruct?);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddAll<TFmt>(Span<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmt);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddAllNullable<TFmt>(Span<TFmt?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : class, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmt);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddAll<TFmt>(ReadOnlySpan<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmt);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddAllNullable<TFmt>(ReadOnlySpan<TFmt?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : class, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmt);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddAll<TFmtStruct>(Span<TFmtStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmtStruct?);
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddAll<TFmtStruct>(ReadOnlySpan<TFmtStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmtStruct?);
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddAll<TFmt>(IReadOnlyList<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmt);
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt AddAll<TFmtStruct>(IReadOnlyList<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmtStruct?);
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString ?? "");
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate<TFmt>(IEnumerable<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmt);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            foreach (var item in value)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, itemCount, formatString ?? "");
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate<TFmtStruct>(IEnumerable<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmtStruct?);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            foreach (var item in value)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, itemCount, formatString ?? "");
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate<TFmt>(IEnumerator<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmt);
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
                stb.AppendFormattedCollectionItem(item, itemCount, formatString ?? "");
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate<TFmtStruct>(IEnumerator<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TFmtStruct?);
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
                stb.AppendFormattedCollectionItem(item, itemCount, formatString ?? "");
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
    
    public TExt RevealAll<TCloaked, TCloakedBase>(TCloaked?[]? value, PalantírReveal<TCloakedBase> palantírReveal)
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
                stb.AppendOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    
    public TExt RevealAll<TCloakedStruct>(TCloakedStruct?[]? value, PalantírReveal<TCloakedStruct> palantírReveal)
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
                stb.AppendOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealAll<TCloaked, TCloakedBase>(Span<TCloaked> value, PalantírReveal<TCloakedBase> palantírReveal)
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
                stb.AppendOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealAllNullable<TCloaked, TCloakedBase>(Span<TCloaked?> value, PalantírReveal<TCloakedBase> palantírReveal)
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
                stb.AppendOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealAll<TCloakedStruct>(Span<TCloakedStruct?> value, PalantírReveal<TCloakedStruct> palantírReveal)
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
                stb.AppendOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealAll<TCloaked, TCloakedBase>(ReadOnlySpan<TCloaked> value, PalantírReveal<TCloakedBase> palantírReveal)
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
                stb.AppendOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealAllNullable<TCloaked, TCloakedBase>(ReadOnlySpan<TCloaked?> value, PalantírReveal<TCloakedBase> palantírReveal)
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
                stb.AppendOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealAll<TCloakedStruct>(ReadOnlySpan<TCloakedStruct?> value, PalantírReveal<TCloakedStruct> palantírReveal)
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
                stb.AppendOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealAll<TCloaked, TCloakedBase>(IReadOnlyList<TCloaked?>? value, PalantírReveal<TCloakedBase> palantírReveal)
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
                stb.AppendOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealAll<TCloakedStruct>(IReadOnlyList<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal)
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
                stb.AppendOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealAllEnumerate<TCloaked, TCloakedBase>(IEnumerable<TCloaked?>? value, PalantírReveal<TCloakedBase> palantírReveal)
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
                stb.AppendOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealAllEnumerate<TCloakedStruct>(IEnumerable<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal)
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
                stb.AppendOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealAllEnumerate<TCloaked, TCloakedBase>(IEnumerator<TCloaked?>? value, PalantírReveal<TCloakedBase> palantírReveal) 
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
                stb.AppendOrNull(item, palantírReveal);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealAllEnumerate<TCloakedStruct>(IEnumerator<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal) 
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
                stb.AppendOrNull(item, palantírReveal);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealAll<TBearer>(TBearer?[]? value)
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
                stb.AppendRevealBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0 );
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealAll<TBearer>(TBearer?[]? value)
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
                stb.AppendRevealBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0 );
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealAll<TBearer>(Span<TBearer> value) where TBearer : IStringBearer
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
                stb.AppendRevealBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealAllNullable<TBearer>(Span<TBearer?> value) where TBearer : class, IStringBearer
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
                stb.AppendRevealBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealAll<TBearerStruct>(Span<TBearerStruct?> value)
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
                stb.AppendRevealBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealAll<TBearer>(ReadOnlySpan<TBearer> value) where TBearer : IStringBearer
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
                stb.AppendRevealBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealAllNullable<TBearer>(ReadOnlySpan<TBearer?> value) where TBearer : class, IStringBearer
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
                stb.AppendRevealBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealAll<TBearerStruct>(ReadOnlySpan<TBearerStruct?> value)
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
                stb.AppendRevealBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealAll<TBearer>(IReadOnlyList<TBearer?>? value)
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
                stb.AppendRevealBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealAll<TBearerStruct>(IReadOnlyList<TBearerStruct?>? value)
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
                stb.AppendRevealBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealAllEnumerate<TBearer>(IEnumerable<TBearer?>? value)
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
                stb.AppendRevealBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealAllEnumerate<TBearerStruct>(IEnumerable<TBearerStruct?>? value)
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
                stb.AppendRevealBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealAllEnumerate<TBearer>(IEnumerator<TBearer?>? value)
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
                stb.AppendRevealBearerOrNull(item);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt RevealAllEnumerate<TBearerStruct>(IEnumerator<TBearerStruct?>? value)
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
                stb.AppendRevealBearerOrNull(item);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddAll(string?[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(string);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
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

    public TExt AddAll(Span<string> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(string);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
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

    public TExt AddAllNullable(Span<string?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(string);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
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

    public TExt AddAll(ReadOnlySpan<string> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(string);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
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

    public TExt AddAllNullable(ReadOnlySpan<string?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(string);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
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

    public TExt AddAll(IReadOnlyList<string?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(string);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
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

    public TExt AddAllEnumerate(IEnumerable<string?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(string);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            foreach (var item in value)
            {
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

    public TExt AddAllEnumerate(IEnumerator<string?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(string);
        var any         = false;
        var itemCount   = 0;
        var hasValue    = value?.MoveNext() ?? false;
        if (hasValue)
        {
            while (hasValue)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value!.Current;
                
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

    public TExt AddAllCharSeq<TCharSeq>(TCharSeq?[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TCharSeq : ICharSequence 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCharSeq);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
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
        return any ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddAllCharSeq<TCharSeq>(Span<TCharSeq> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  
        where TCharSeq : ICharSequence
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCharSeq);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
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

    public TExt AddAllCharSeqNullable<TCharSeq>(Span<TCharSeq?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  
        where TCharSeq : ICharSequence
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCharSeq);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
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

    public TExt AddAllCharSeq<TCharSeq>(ReadOnlySpan<TCharSeq> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  
        where TCharSeq : ICharSequence
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCharSeq);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
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

    public TExt AddAllCharSeqNullable<TCharSeq>(ReadOnlySpan<TCharSeq?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  
        where TCharSeq : ICharSequence
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCharSeq);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
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

    public TExt AddAllCharSeq<TCharSeq>(IReadOnlyList<TCharSeq?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  
        where TCharSeq : ICharSequence
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCharSeq);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
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

    public TExt AddAllCharSeqEnumerate<TCharSeq>(IEnumerable<TCharSeq?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TCharSeq : ICharSequence
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCharSeq);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            foreach (var item in value)
            {
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

    public TExt AddAllCharSeqEnumerate<TCharSeq>(IEnumerator<TCharSeq?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TCharSeq : ICharSequence
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TCharSeq);
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

    public TExt AddAll(StringBuilder?[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(StringBuilder);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
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

    public TExt AddAll(Span<StringBuilder> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(StringBuilder);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
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

    public TExt AddAllNullable(Span<StringBuilder?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(StringBuilder);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
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

    public TExt AddAll(ReadOnlySpan<StringBuilder> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(StringBuilder);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
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

    public TExt AddAllNullable(ReadOnlySpan<StringBuilder?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(StringBuilder);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
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

    public TExt AddAll(IReadOnlyList<StringBuilder?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(StringBuilder);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, i, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, i);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0 );
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate(IEnumerable<StringBuilder?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(StringBuilder);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            foreach (var item in value)
            {
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

    public TExt AddAllEnumerate(IEnumerator<StringBuilder?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(StringBuilder);
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

    public TExt AddAllMatch<TAny>(TAny[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TAny);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
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

    public TExt AddAllMatch<TAny>(Span<TAny> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TAny);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
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

    public TExt AddAllMatchNullable<TAny>(Span<TAny?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TAny);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
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

    public TExt AddAllMatch<TAny>(ReadOnlySpan<TAny> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TAny);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
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

    public TExt AddAllMatchNullable<TAny>(ReadOnlySpan<TAny?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TAny);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
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

    public TExt AddAllMatch<TAny>(IReadOnlyList<TAny>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TAny);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
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

    public TExt AddAllMatchEnumerate<TAny>(IEnumerable<TAny>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TAny);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            foreach (var item in value)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
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

    public TExt AddAllMatchEnumerate<TAny>(IEnumerator<TAny>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TAny);
        var any         = false;
        var itemCount   = 0;
        var hasValue    = value?.MoveNext() ?? false;
        if (hasValue)
        {
            while (hasValue)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value!.Current;
                
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
    
    [CallsObjectToString] 
    public TExt AddAllObject(object?[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(object);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
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
    public TExt AddAllObject(Span<object> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(object);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
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
    public TExt AddAllObjectNullable(Span<object?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(object);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
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
    public TExt AddAllObject(ReadOnlySpan<object> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(object);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
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
    public TExt AddAllObjectNullable(ReadOnlySpan<object?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(object);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
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
    public TExt AddAllObject(IReadOnlyList<object?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(object);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
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
    public TExt AddAllObjectEnumerate(IEnumerable<object?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(object);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            foreach (var item in value)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
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
    public TExt AddAllObjectEnumerate(IEnumerator<object?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(object);
        var any         = false;
        var itemCount   = 0;
        var hasValue    = value?.MoveNext() ?? false;
        if (hasValue)
        {
            while (hasValue)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value!.Current;
                
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
