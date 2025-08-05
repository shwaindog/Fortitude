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

    public TExt AddAll(ICollection<bool>? value)  => AddAll((IEnumerable<bool>?)value);
    public TExt AddAll(ICollection<bool?>? value) => AddAll((IEnumerable<bool?>?)value);

    public TExt AddAll(IEnumerable<bool>? value)
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

    public TExt AddAll(IEnumerable<bool?>? value)
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

    public TExt AddAll(IEnumerator<bool>? value)
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

    public TExt AddAll(IEnumerator<bool?>? value)
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

    public TExt AddAll<TNum>(TNum[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>
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
    
    public TExt AddAll<TNum>(TNum?[]? value 
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>
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
    
    public TExt AddAll<TNum>(ReadOnlySpan<TNum> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>
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
    
    public TExt AddAll<TNum>(ReadOnlySpan<TNum?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>
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
    
    public TExt AddAll<TNum>(IReadOnlyList<TNum>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>
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
    
    public TExt AddAll<TNum>(IReadOnlyList<TNum?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>
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

    public TExt AddAll<TNum>(ICollection<TNum>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        AddAll((IEnumerable<TNum>?)value, formatString);
    
    public TExt AddAll<TNum>(ICollection<TNum?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        AddAll((IEnumerable<TNum?>?)value, formatString);

    public TExt AddAll<TNum>(IEnumerable<TNum>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>
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

    public TExt AddAll<TNum>(IEnumerable<TNum?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>
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

    public TExt AddAll<TNum>(IEnumerator<TNum>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>
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

    public TExt AddAll<TNum>(IEnumerator<TNum?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>
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

    public TExt AddAll<TStruct>(TStruct[]? value, StructStyler<TStruct> structStyler) where TStruct : struct
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                
                any = true;
                structStyler(item, stb.OwningAppender);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll<TStruct>(TStruct?[]? value, StructStyler<TStruct> structStyler) where TStruct : struct
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                
                any = true;
                stb.AppendOrNull(item, structStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll<TStruct>(ReadOnlySpan<TStruct> value, StructStyler<TStruct> structStyler) where TStruct : struct
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                
                any = true;
                structStyler(item, stb.OwningAppender);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll<TStruct>(ReadOnlySpan<TStruct?> value, StructStyler<TStruct> structStyler) where TStruct : struct
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                
                any = true;
                stb.AppendOrNull(item, structStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll<TStruct>(IReadOnlyList<TStruct>? value, StructStyler<TStruct> structStyler) where TStruct : struct
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                
                any = true;
                structStyler(item, stb.OwningAppender);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll<TStruct>(IReadOnlyList<TStruct?>? value, StructStyler<TStruct> structStyler) where TStruct : struct
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                
                any = true;
                stb.AppendOrNull(item, structStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll<TStruct>(ICollection<TStruct>? value, StructStyler<TStruct> structStyler) where TStruct : struct =>
        AddAll((IEnumerable<TStruct>?)value, structStyler);

    public TExt AddAll<TStruct>(ICollection<TStruct?>? value, StructStyler<TStruct> structStyler) where TStruct : struct =>
        AddAll((IEnumerable<TStruct?>?)value, structStyler);

    public TExt AddAll<TStruct>(IEnumerable<TStruct>? value, StructStyler<TStruct> structStyler) where TStruct : struct
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            foreach (var item in value)
            {
                any = true;
                structStyler(item, stb.OwningAppender);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll<TStruct>(IEnumerable<TStruct?>? value, StructStyler<TStruct> structStyler) where TStruct : struct
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        if (value != null)
        {
            foreach (var item in value)
            {
                any = true;
                stb.AppendOrNull(item, structStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt AddAll<TStruct>(IEnumerator<TStruct>? value, StructStyler<TStruct> structStyler) where TStruct : struct
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

    public TExt AddAll<TStruct>(IEnumerator<TStruct?>? value, StructStyler<TStruct> structStyler) where TStruct : struct
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
                stb.AppendOrNull(item, structStyler);
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

    public TExt AddAll(ICollection<string?>? value) => AddAll((IEnumerable<string>?)value);

    public TExt AddAll(IEnumerable<string?>? value)
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

    public TExt AddAll(IEnumerator<string?>? value)
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


    public TExt AddAll(IStyledToStringObject?[]? value)
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

    public TExt AddAll(ReadOnlySpan<IStyledToStringObject?> value)
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

    public TExt AddAll(IReadOnlyList<IStyledToStringObject?>? value)
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

    public TExt AddAll(ICollection<IStyledToStringObject?>? value) => AddAll((IEnumerable<IStyledToStringObject>?)value);

    public TExt AddAll(IEnumerable<IStyledToStringObject?>? value)
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

    public TExt AddAll(IEnumerator<IStyledToStringObject?>? value)
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

    public TExt AddAll(ICollection<ICharSequence?>? value) => AddAll((IEnumerable<ICharSequence>?)value);

    public TExt AddAll(IEnumerable<ICharSequence?>? value)
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

    public TExt AddAll(IEnumerator<ICharSequence?>? value)
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

    public TExt AddAll(ICollection<StringBuilder?>? value) => AddAll((IEnumerable<StringBuilder>?)value);

    public TExt AddAll(IEnumerable<StringBuilder?>? value)
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

    public TExt AddAll(IEnumerator<StringBuilder?>? value)
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
    public TExt AddAll(object?[]? value)
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

    [CallsObjectToString] 
    public TExt AddAll(ReadOnlySpan<object?> value)
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

    [CallsObjectToString] 
    public TExt AddAll(IReadOnlyList<object?>? value)
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

    [CallsObjectToString] 
    public TExt AddAll(ICollection<object?>? value) => AddAll((IEnumerable<object>?)value);

    [CallsObjectToString] 
    public TExt AddAll(IEnumerable<object?>? value)
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

    [CallsObjectToString] 
    public TExt AddAll(IEnumerator<object?>? value)
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

}
