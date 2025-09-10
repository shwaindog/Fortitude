// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable.Strings;
#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeOrderedCollection;

public partial class OrderedCollectionBuilder<TExt> 
    where TExt : StyledTypeBuilder
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
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
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
                if (item != null)
                {
                    stb.Sb.Append(item); 
                }
                else
                {
                    stb.Sb.Append(stb.Settings.NullStyle);
                }
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
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
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
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
                if (item != null)
                {
                    stb.Sb.Append(item); 
                }
                else
                {
                    stb.Sb.Append(stb.Settings.NullStyle);
                }
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
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
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
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
                if (item != null)
                {
                    stb.Sb.Append(item); 
                }
                else
                {
                    stb.Sb.Append(stb.Settings.NullStyle);
                }
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
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
                stb.AppendObjectOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
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
                stb.AppendObjectOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
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
                stb.AppendObjectOrNull(item);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
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
                if (item != null)
                {
                    stb.Sb.Append(item); 
                }
                else
                {
                    stb.Sb.Append(stb.Settings.NullStyle);
                }
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
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
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
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
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
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
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
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
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
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
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
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
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
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
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
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
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
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
                stb.Sb.Append(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
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
                stb.Sb.Append(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    
    public TExt AddAll<TToStyle, TStylerType>(TToStyle?[]? value, CustomTypeStyler<TStylerType> customTypeStyler)
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
                stb.AppendOrNull(item, customTypeStyler);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll<TToStyle, TStylerType>(ReadOnlySpan<TToStyle?> value, CustomTypeStyler<TStylerType> customTypeStyler)
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
                stb.AppendOrNull(item, customTypeStyler);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll<TToStyle, TStylerType>(IReadOnlyList<TToStyle?>? value, CustomTypeStyler<TStylerType> customTypeStyler)
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
                stb.AppendOrNull(item, customTypeStyler);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate<TToStyle, TStylerType>(IEnumerable<TToStyle?>? value, CustomTypeStyler<TStylerType> customTypeStyler)
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
                stb.AppendOrNull(item, customTypeStyler);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate<TToStyle, TStylerType>(IEnumerator<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler) 
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
                stb.AppendOrNull(item, customTypeStyler);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }


    public TExt AddAll(string?[]? value)
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
                stb.Sb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll(ReadOnlySpan<string?> value)
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
                stb.Sb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll(IReadOnlyList<string?>? value)
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
                stb.Sb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate(IEnumerable<string?>? value)
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
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate(IEnumerator<string?>? value)
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
                stb.AppendOrNull(item);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllCharSequence<T>(T?[]? value) where T : class, ICharSequence 
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
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllCharSequence<T>(ReadOnlySpan<T?> value)  where T : class, ICharSequence
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
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllCharSequence<T>(IReadOnlyList<T?>? value)  where T : class, ICharSequence
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
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllCharSequenceEnumerate<T>(IEnumerable<T?>? value) where T : class, ICharSequence
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
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllCharSequenceEnumerate<T>(IEnumerator<T?>? value) where T : class, ICharSequence
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var elementType = typeof(T);
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
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll(StringBuilder?[]? value)
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
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll(ReadOnlySpan<StringBuilder?> value)
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
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll(IReadOnlyList<StringBuilder?>? value)
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
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0 );
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate(IEnumerable<StringBuilder?>? value)
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
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate(IEnumerator<StringBuilder?>? value)
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
                stb.AppendOrNull(item);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllStyled<TStyledObj>(TStyledObj[]? value)
        where TStyledObj : class, IStyledToStringObject
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
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0 );
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllStyled<TStyledObj>(ReadOnlySpan<TStyledObj> value)
        where TStyledObj : class, IStyledToStringObject
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
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllStyled<TStyledObj>(IReadOnlyList<TStyledObj>? value)
        where TStyledObj : class, IStyledToStringObject
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
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllStyledEnumerate<TStyledObj>(IEnumerable<TStyledObj>? value)
        where TStyledObj : class, IStyledToStringObject 
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
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllStyledEnumerate<TStyledObj>(IEnumerator<TStyledObj>? value)
        where TStyledObj : class, IStyledToStringObject 
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
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
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
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendObjectOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length ?? 0);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
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
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendObjectOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
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
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendObjectOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count ?? 0);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
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
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendObjectOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
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
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendObjectOrNull(item);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
            else stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix(elementType, itemCount);
        return stb.CollectionInComplexType ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }
}
