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

    public TExt AddAll<TFmt>(TFmt[]? value
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
    
    public TExt AddAll<TFmt>(IReadOnlyList<TFmt>? value
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

    public TExt AddAllEnumerate<TFmt>(IEnumerable<TFmt>? value
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

    public TExt AddAllEnumerate<TFmt>(IEnumerator<TFmt>? value
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

    
    public TExt AddAll<TToStyle, TStylerType>(TToStyle?[]? value, StringBearerRevealState<TStylerType> stringBearerRevealState)
        where TToStyle : TStylerType
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TToStyle);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendOrNull(item, stringBearerRevealState);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddAll<TToStyle, TStylerType>(ReadOnlySpan<TToStyle?> value, StringBearerRevealState<TStylerType> stringBearerRevealState)
        where TToStyle : TStylerType
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TToStyle);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendOrNull(item, stringBearerRevealState);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddAll<TToStyle, TStylerType>(IReadOnlyList<TToStyle?>? value, StringBearerRevealState<TStylerType> stringBearerRevealState)
        where TToStyle : TStylerType
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TToStyle);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendOrNull(item, stringBearerRevealState);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate<TToStyle, TStylerType>(IEnumerable<TToStyle?>? value, StringBearerRevealState<TStylerType> stringBearerRevealState)
        where TToStyle : TStylerType 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TToStyle);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            foreach (var item in value)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendOrNull(item, stringBearerRevealState);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate<TToStyle, TStylerType>(IEnumerator<TToStyle>? value, StringBearerRevealState<TStylerType> stringBearerRevealState) 
        where TToStyle : TStylerType 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TToStyle);
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
                stb.AppendOrNull(item, stringBearerRevealState);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount);
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

    public TExt AddAll(ReadOnlySpan<string?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
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

    public TExt AddAllCharSequence<TCharSeq>(TCharSeq?[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
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

    public TExt AddAllCharSequence<TCharSeq>(ReadOnlySpan<TCharSeq?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  
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

    public TExt AddAllCharSequence<TCharSeq>(IReadOnlyList<TCharSeq?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  
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

    public TExt AddAllCharSequenceEnumerate<TCharSeq>(IEnumerable<TCharSeq?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
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

    public TExt AddAllCharSequenceEnumerate<TCharSeq>(IEnumerator<TCharSeq?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
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

    public TExt AddAll(ReadOnlySpan<StringBuilder?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
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

    public TExt AddAllStyled<TStyledObj>(TStyledObj[]? value)
        where TStyledObj : class, IStringBearer
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TStyledObj);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0 );
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddAllStyled<TStyledObj>(ReadOnlySpan<TStyledObj> value)
        where TStyledObj : class, IStringBearer
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TStyledObj);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddAllStyled<TStyledObj>(IReadOnlyList<TStyledObj>? value)
        where TStyledObj : class, IStringBearer
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TStyledObj);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                if(!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];
                
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TExt AddAllStyledEnumerate<TStyledObj>(IEnumerable<TStyledObj>? value)
        where TStyledObj : class, IStringBearer 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TStyledObj);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            foreach (var item in value)
            {
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

    public TExt AddAllStyledEnumerate<TStyledObj>(IEnumerator<TStyledObj>? value)
        where TStyledObj : class, IStringBearer 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(TStyledObj);
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
    public TExt AddAllMatch<T>(T[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(T);
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
    public TExt AddAllMatch<T>(ReadOnlySpan<T> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(T);
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
    public TExt AddAllMatch<T>(IReadOnlyList<T>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(T);
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
    public TExt AddAllMatchEnumerate<T>(IEnumerable<T>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(T);
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
    public TExt AddAllMatchEnumerate<T>(IEnumerator<T>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(T);
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
