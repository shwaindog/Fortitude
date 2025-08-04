// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable.Strings;
#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeOrderedCollection;

public interface IAddAllTypeIsOrderedCollection<out T> where T : StyledTypeBuilder
{
    T From(bool[]? value);
    T From(bool?[]? value);
    T From(ReadOnlySpan<bool> value);
    T From(ReadOnlySpan<bool?> value);
    T From(IReadOnlyList<bool>? value);
    T From(IReadOnlyList<bool?>? value);
    T From(ICollection<bool>? value);
    T From(ICollection<bool?>? value);
    T From(IEnumerable<bool>? value);
    T From(IEnumerable<bool?>? value);
    T From(IEnumerator<bool>? value);
    T From(IEnumerator<bool?>? value);

    T From<TNum> (TNum[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>;

    T From<TNum>(TNum?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>;

    T From<TNum>(ReadOnlySpan<TNum> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>;

    T From<TNum>(ReadOnlySpan<TNum?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>;

    T From<TNum>(IReadOnlyList<TNum>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>;

    T From<TNum>(IReadOnlyList<TNum?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>;

    T From<TNum>(ICollection<TNum>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>;

    T From<TNum>(ICollection<TNum?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>;

    T From<TNum>(IEnumerable<TNum>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>;

    T From<TNum>(IEnumerable<TNum?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>;

    T From<TNum>(IEnumerator<TNum>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>;

    T From<TNum>(IEnumerator<TNum?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>;

    T From<TStruct>(TStruct[]? value, StructStyler<TStruct> structStyler) where TStruct : struct;
    T From<TStruct>(TStruct?[]? value, StructStyler<TStruct> structStyler) where TStruct : struct;

    T From<TStruct>(ReadOnlySpan<TStruct> value, StructStyler<TStruct> structStyler) where TStruct : struct;
    T From<TStruct>(ReadOnlySpan<TStruct?> value, StructStyler<TStruct> structStyler) where TStruct : struct;
    T From<TStruct>(IReadOnlyList<TStruct>? value, StructStyler<TStruct> structStyler) where TStruct : struct;
    T From<TStruct>(IReadOnlyList<TStruct?>? value, StructStyler<TStruct> structStyler) where TStruct : struct;
    T From<TStruct>(ICollection<TStruct>? value, StructStyler<TStruct> structStyler) where TStruct : struct;
    T From<TStruct>(ICollection<TStruct?>? value, StructStyler<TStruct> structStyler) where TStruct : struct;
    T From<TStruct>(IEnumerable<TStruct>? value, StructStyler<TStruct> structStyler) where TStruct : struct;
    T From<TStruct>(IEnumerable<TStruct?>? value, StructStyler<TStruct> structStyler) where TStruct : struct;
    T From<TStruct>(IEnumerator<TStruct>? value, StructStyler<TStruct> structStyler) where TStruct : struct;
    T From<TStruct>(IEnumerator<TStruct?>? value, StructStyler<TStruct> structStyler) where TStruct : struct;

    T From (string?[]? value);
    T From(ReadOnlySpan<string?> value);
    T From(IReadOnlyList<string?>? value);
    T From(ICollection<string?>? value);
    T From(IEnumerable<string?>? value);
    T From(IEnumerator<string?>? value);

    T From(IStyledToStringObject?[]? value);
    T From(ReadOnlySpan<IStyledToStringObject?> value);
    T From(IReadOnlyList<IStyledToStringObject?>? value);
    T From(ICollection<IStyledToStringObject?>? value);
    T From(IEnumerable<IStyledToStringObject?>? value);
    T From(IEnumerator<IStyledToStringObject?>? value);

    T From(ICharSequence?[]? value);
    T From(ReadOnlySpan<ICharSequence?> value);
    T From(IReadOnlyList<ICharSequence?>? value);
    T From(ICollection<ICharSequence?>? value);
    T From(IEnumerable<ICharSequence?>? value);
    T From(IEnumerator<ICharSequence?>? value);


    T From(StringBuilder?[]? value);
    T From(ReadOnlySpan<StringBuilder?> value);
    T From(IReadOnlyList<StringBuilder?>? value);
    T From(ICollection<StringBuilder?>? value);
    T From(IEnumerable<StringBuilder?>? value);
    T From(IEnumerator<StringBuilder?>? value);
    
    [CallsObjectToString] T From(object?[]? value);
    [CallsObjectToString] T From(ReadOnlySpan<object?> value);
    [CallsObjectToString] T From(IReadOnlyList<object?>? value);
    [CallsObjectToString] T From(ICollection<object?>? value);
    [CallsObjectToString] T From(IEnumerable<object?>? value);
    [CallsObjectToString] T From(IEnumerator<object?>? value);
}

public class AddAllTypeIsOrderedCollection<TExt> : RecyclableObject, IAddAllTypeIsOrderedCollection<TExt>
    where TExt : StyledTypeBuilder
{
    private CollectionBuilderCompAccess<TExt> stb = null!;

    public AddAllTypeIsOrderedCollection<TExt> Initialize(CollectionBuilderCompAccess<TExt> styledComplexTypeBuilder)
    {
        stb = styledComplexTypeBuilder;

        return this;
    }
    
    public TExt From(bool[]? value)
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

    public TExt From(bool?[]? value)
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

    public TExt From(ReadOnlySpan<bool> value)
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

    public TExt From(ReadOnlySpan<bool?> value)
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

    public TExt From(IReadOnlyList<bool>? value)
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

    public TExt From(IReadOnlyList<bool?>? value)
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

    public TExt From(ICollection<bool>? value)  => From((IEnumerable<bool>?)value);
    public TExt From(ICollection<bool?>? value) => From((IEnumerable<bool?>?)value);

    public TExt From(IEnumerable<bool>? value)
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

    public TExt From(IEnumerable<bool?>? value)
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

    public TExt From(IEnumerator<bool>? value)
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

    public TExt From(IEnumerator<bool?>? value)
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

    public TExt From<TNum>(TNum[]? value
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
    
    public TExt From<TNum>(TNum?[]? value 
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
    
    public TExt From<TNum>(ReadOnlySpan<TNum> value
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
    
    public TExt From<TNum>(ReadOnlySpan<TNum?> value
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
    
    public TExt From<TNum>(IReadOnlyList<TNum>? value
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
    
    public TExt From<TNum>(IReadOnlyList<TNum?>? value
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

    public TExt From<TNum>(ICollection<TNum>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        From((IEnumerable<TNum>?)value, formatString);
    
    public TExt From<TNum>(ICollection<TNum?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        From((IEnumerable<TNum?>?)value, formatString);

    public TExt From<TNum>(IEnumerable<TNum>? value
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

    public TExt From<TNum>(IEnumerable<TNum?>? value
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

    public TExt From<TNum>(IEnumerator<TNum>? value
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

    public TExt From<TNum>(IEnumerator<TNum?>? value
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

    public TExt From<TStruct>(TStruct[]? value, StructStyler<TStruct> structStyler) where TStruct : struct
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

    public TExt From<TStruct>(TStruct?[]? value, StructStyler<TStruct> structStyler) where TStruct : struct
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

    public TExt From<TStruct>(ReadOnlySpan<TStruct> value, StructStyler<TStruct> structStyler) where TStruct : struct
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

    public TExt From<TStruct>(ReadOnlySpan<TStruct?> value, StructStyler<TStruct> structStyler) where TStruct : struct
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

    public TExt From<TStruct>(IReadOnlyList<TStruct>? value, StructStyler<TStruct> structStyler) where TStruct : struct
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

    public TExt From<TStruct>(IReadOnlyList<TStruct?>? value, StructStyler<TStruct> structStyler) where TStruct : struct
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

    public TExt From<TStruct>(ICollection<TStruct>? value, StructStyler<TStruct> structStyler) where TStruct : struct =>
        From((IEnumerable<TStruct>?)value, structStyler);

    public TExt From<TStruct>(ICollection<TStruct?>? value, StructStyler<TStruct> structStyler) where TStruct : struct =>
        From((IEnumerable<TStruct?>?)value, structStyler);

    public TExt From<TStruct>(IEnumerable<TStruct>? value, StructStyler<TStruct> structStyler) where TStruct : struct
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

    public TExt From<TStruct>(IEnumerable<TStruct?>? value, StructStyler<TStruct> structStyler) where TStruct : struct
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

    public TExt From<TStruct>(IEnumerator<TStruct>? value, StructStyler<TStruct> structStyler) where TStruct : struct
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

    public TExt From<TStruct>(IEnumerator<TStruct?>? value, StructStyler<TStruct> structStyler) where TStruct : struct
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


    public TExt From(string?[]? value)
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

    public TExt From(ReadOnlySpan<string?> value)
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

    public TExt From(IReadOnlyList<string?>? value)
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

    public TExt From(ICollection<string?>? value) => From((IEnumerable<string>?)value);

    public TExt From(IEnumerable<string?>? value)
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

    public TExt From(IEnumerator<string?>? value)
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


    public TExt From(IStyledToStringObject?[]? value)
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

    public TExt From(ReadOnlySpan<IStyledToStringObject?> value)
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

    public TExt From(IReadOnlyList<IStyledToStringObject?>? value)
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

    public TExt From(ICollection<IStyledToStringObject?>? value) => From((IEnumerable<IStyledToStringObject>?)value);

    public TExt From(IEnumerable<IStyledToStringObject?>? value)
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

    public TExt From(IEnumerator<IStyledToStringObject?>? value)
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

    public TExt From(ICharSequence?[]? value)
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

    public TExt From(ReadOnlySpan<ICharSequence?> value)
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

    public TExt From(IReadOnlyList<ICharSequence?>? value)
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

    public TExt From(ICollection<ICharSequence?>? value) => From((IEnumerable<ICharSequence>?)value);

    public TExt From(IEnumerable<ICharSequence?>? value)
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

    public TExt From(IEnumerator<ICharSequence?>? value)
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

    public TExt From(StringBuilder?[]? value)
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

    public TExt From(ReadOnlySpan<StringBuilder?> value)
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

    public TExt From(IReadOnlyList<StringBuilder?>? value)
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

    public TExt From(ICollection<StringBuilder?>? value) => From((IEnumerable<StringBuilder>?)value);

    public TExt From(IEnumerable<StringBuilder?>? value)
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

    public TExt From(IEnumerator<StringBuilder?>? value)
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
    public TExt From(object?[]? value)
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
    public TExt From(ReadOnlySpan<object?> value)
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
    public TExt From(IReadOnlyList<object?>? value)
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
    public TExt From(ICollection<object?>? value) => From((IEnumerable<object>?)value);

    [CallsObjectToString] 
    public TExt From(IEnumerable<object?>? value)
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
    public TExt From(IEnumerator<object?>? value)
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
