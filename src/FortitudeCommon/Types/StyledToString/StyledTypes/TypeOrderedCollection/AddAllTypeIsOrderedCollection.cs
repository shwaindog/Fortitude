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
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                any = true;
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll(bool?[]? value)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                any = true;
                if (item != null)
                {
                    stb.Sb.Append(item); 
                }
                else
                {
                    stb.Sb.Append(stb.OwningAppender.NullStyle);
                }
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll(ReadOnlySpan<bool> value)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                any = true;
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll(ReadOnlySpan<bool?> value)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                any = true;
                if (item != null)
                {
                    stb.Sb.Append(item); 
                }
                else
                {
                    stb.Sb.Append(stb.OwningAppender.NullStyle);
                }
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll(IReadOnlyList<bool>? value)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                any = true;
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll(IReadOnlyList<bool?>? value)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                any = true;
                if (item != null)
                {
                    stb.Sb.Append(item); 
                }
                else
                {
                    stb.Sb.Append(stb.OwningAppender.NullStyle);
                }
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate(IEnumerable<bool>? value)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            foreach (var item in value)
            {
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate(IEnumerable<bool?>? value)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            foreach (var item in value)
            {
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate(IEnumerator<bool>? value)
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            while (hasValue)
            {
                var item = value!.Current;
                
                any = true;
                stb.AppendOrNull(item);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate(IEnumerator<bool?>? value)
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            while (hasValue)
            {
                var item = value!.Current;
                
                any = true;
                if (item != null)
                {
                    stb.Sb.Append(item); 
                }
                else
                {
                    stb.Sb.Append(stb.OwningAppender.NullStyle);
                }
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll<TFmtStruct>(TFmtStruct[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                
                any = true;
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }
    
    public TExt AddAll<TFmtStruct>(TFmtStruct?[]? value 
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                
                any = true;
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }
    
    public TExt AddAll<TFmtStruct>(ReadOnlySpan<TFmtStruct> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                
                any = true;
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }
    
    public TExt AddAll<TFmtStruct>(ReadOnlySpan<TFmtStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                
                any = true;
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }
    
    public TExt AddAll<TFmtStruct>(IReadOnlyList<TFmtStruct>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                
                any = true;
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }
    
    public TExt AddAll<TFmtStruct>(IReadOnlyList<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                
                any = true;
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate<TFmtStruct>(IEnumerable<TFmtStruct>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            foreach (var item in value)
            {
                any = true;
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate<TFmtStruct>(IEnumerable<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            foreach (var item in value)
            {
                any = true;
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.Sb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate<TFmtStruct>(IEnumerator<TFmtStruct>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            while (hasValue)
            {
                any = true;
                stb.Sb.Append(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate<TFmtStruct>(IEnumerator<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            while (hasValue)
            {
                any = true;
                stb.Sb.Append(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll<TStruct>(TStruct[]? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                
                any = true;
                customTypeStyler(item, stb.OwningAppender);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll<TStruct>(TStruct?[]? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                
                any = true;
                stb.AppendOrNull(item, customTypeStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll<TStruct>(ReadOnlySpan<TStruct> value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                
                any = true;
                customTypeStyler(item, stb.OwningAppender);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll<TStruct>(ReadOnlySpan<TStruct?> value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                
                any = true;
                stb.AppendOrNull(item, customTypeStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll<TStruct>(IReadOnlyList<TStruct>? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                
                any = true;
                customTypeStyler(item, stb.OwningAppender);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll<TStruct>(IReadOnlyList<TStruct?>? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                
                any = true;
                stb.AppendOrNull(item, customTypeStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate<TStruct>(IEnumerable<TStruct>? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            foreach (var item in value)
            {
                any = true;
                customTypeStyler(item, stb.OwningAppender);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate<TStruct>(IEnumerable<TStruct?>? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            foreach (var item in value)
            {
                any = true;
                stb.AppendOrNull(item, customTypeStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate<TStruct>(IEnumerator<TStruct>? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            while (hasValue)
            {
                var item = value!.Current;
                
                any = true;
                stb.Sb.Append(item);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate<TStruct>(IEnumerator<TStruct?>? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            while (hasValue)
            {
                var item = value!.Current;
                
                any = true;
                stb.AppendOrNull(item, customTypeStyler);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }


    public TExt AddAll(string?[]? value)
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                
                any = true;
                stb.Sb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll(ReadOnlySpan<string?> value)
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                
                any = true;
                stb.Sb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll(IReadOnlyList<string?>? value)
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                
                any = true;
                stb.Sb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate(IEnumerable<string?>? value)
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            foreach (var item in value)
            {
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate(IEnumerator<string?>? value)
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            while (hasValue)
            {
                var item = value!.Current;
                
                any = true;
                stb.AppendOrNull(item);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }


    public TExt AddAll<TStyledObj>(TStyledObj[]? value)
        where TStyledObj : class, IStyledToStringObject
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll<TStyledObj>(ReadOnlySpan<TStyledObj> value)
        where TStyledObj : class, IStyledToStringObject
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll<TStyledObj>(IReadOnlyList<TStyledObj>? value)
        where TStyledObj : class, IStyledToStringObject
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate<TStyledObj>(IEnumerable<TStyledObj>? value)
        where TStyledObj : class, IStyledToStringObject 
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            foreach (var item in value)
            {
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate<TStyledObj>(IEnumerator<TStyledObj>? value)
        where TStyledObj : class, IStyledToStringObject 
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            while (hasValue)
            {
                var item = value!.Current;
                
                any = true;
                stb.AppendOrNull(item);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll(ICharSequence?[]? value)
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll(ReadOnlySpan<ICharSequence?> value)
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll(IReadOnlyList<ICharSequence?>? value)
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate(IEnumerable<ICharSequence?>? value)
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            foreach (var item in value)
            {
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate(IEnumerator<ICharSequence?>? value)
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            while (hasValue)
            {
                var item = value!.Current;
                
                any = true;
                stb.AppendOrNull(item);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll(StringBuilder?[]? value)
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll(ReadOnlySpan<StringBuilder?> value)
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll(IReadOnlyList<StringBuilder?>? value)
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate(IEnumerable<StringBuilder?>? value)
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            foreach (var item in value)
            {
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAllEnumerate(IEnumerator<StringBuilder?>? value)
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            while (hasValue)
            {
                var item = value!.Current;
                
                any = true;
                stb.AppendOrNull(item);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    [CallsObjectToString] 
    public TExt AddAllMatch<T>(T[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : class
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                
                any = true;
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    [CallsObjectToString] 
    public TExt AddAllMatch<T>(ReadOnlySpan<T> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : class
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                
                any = true;
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    [CallsObjectToString] 
    public TExt AddAllMatch<T>(IReadOnlyList<T>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : class
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                
                any = true;
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    [CallsObjectToString] 
    public TExt AddAllMatchEnumerate<T>(IEnumerable<T>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : class
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            foreach (var item in value)
            {
                any = true;
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    [CallsObjectToString] 
    public TExt AddAllMatchEnumerate<T>(IEnumerator<T>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : class
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            while (hasValue)
            {
                var item = value!.Current;
                
                any = true;
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }
}
