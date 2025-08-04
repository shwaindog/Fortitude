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

public interface IAddFilteredTypeIsOrderedCollection<out T> where T : StyledTypeBuilder
{
    T From(bool[]? value, OrderedCollectionPredicate<bool> itemPredicate);
    T From(bool?[]? value, OrderedCollectionPredicate<bool?> itemPredicate);
    T From(ReadOnlySpan<bool> value, OrderedCollectionPredicate<bool> itemPredicate);
    T From(ReadOnlySpan<bool?> value, OrderedCollectionPredicate<bool?> itemPredicate);
    T From(IReadOnlyList<bool>? value, OrderedCollectionPredicate<bool> itemPredicate);
    T From(IReadOnlyList<bool?>? value, OrderedCollectionPredicate<bool?> itemPredicate);
    T From(ICollection<bool>? value, OrderedCollectionPredicate<bool> itemPredicate);
    T From(ICollection<bool?>? value, OrderedCollectionPredicate<bool?> itemPredicate);
    T From(IEnumerable<bool>? value, OrderedCollectionPredicate<bool> itemPredicate);
    T From(IEnumerable<bool?>? value, OrderedCollectionPredicate<bool?> itemPredicate);
    T From(IEnumerator<bool>? value, OrderedCollectionPredicate<bool> itemPredicate);
    T From(IEnumerator<bool?>? value, OrderedCollectionPredicate<bool?> itemPredicate);


    T From<TNum>(TNum[]? value, OrderedCollectionPredicate<TNum> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>;

    T From<TNum>(TNum?[]? value, OrderedCollectionPredicate<TNum?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>;

    T From<TNum>(ReadOnlySpan<TNum> value, OrderedCollectionPredicate<TNum> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>;

    T From<TNum>(ReadOnlySpan<TNum?> value, OrderedCollectionPredicate<TNum?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>;

    T From<TNum>(IReadOnlyList<TNum>? value, OrderedCollectionPredicate<TNum> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>;

    T From<TNum>(IReadOnlyList<TNum?>? value, OrderedCollectionPredicate<TNum?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>;

    T From<TNum>(ICollection<TNum>? value, OrderedCollectionPredicate<TNum> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>;

    T From<TNum>(ICollection<TNum?>? value, OrderedCollectionPredicate<TNum?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>;

    T From<TNum>(IEnumerable<TNum>? value, OrderedCollectionPredicate<TNum> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>;

    T From<TNum>(IEnumerable<TNum?>? value, OrderedCollectionPredicate<TNum?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>;

    T From<TNum>(IEnumerator<TNum>? value, OrderedCollectionPredicate<TNum> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>;

    T From<TNum>(IEnumerator<TNum?>? value, OrderedCollectionPredicate<TNum?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum>;

    T From<TStruct>(TStruct[]? value, OrderedCollectionPredicate<TStruct> itemPredicate, StructStyler<TStruct> structStyler) where TStruct : struct;
    T From<TStruct>(TStruct?[]? value, OrderedCollectionPredicate<TStruct?> itemPredicate, StructStyler<TStruct> structStyler) where TStruct : struct;

    T From<TStruct>(ReadOnlySpan<TStruct> value, OrderedCollectionPredicate<TStruct> itemPredicate, StructStyler<TStruct> structStyler) where TStruct : struct;
    T From<TStruct>(ReadOnlySpan<TStruct?> value, OrderedCollectionPredicate<TStruct?> itemPredicate, StructStyler<TStruct> structStyler) where TStruct : struct;

    T From<TStruct>(IReadOnlyList<TStruct>? value, OrderedCollectionPredicate<TStruct> itemPredicate, StructStyler<TStruct> structStyler) where TStruct : struct;
    T From<TStruct>(IReadOnlyList<TStruct?>? value, OrderedCollectionPredicate<TStruct?> itemPredicate, StructStyler<TStruct> structStyler) where TStruct : struct;

    T From<TStruct>(ICollection<TStruct>? value, OrderedCollectionPredicate<TStruct> itemPredicate, StructStyler<TStruct> structStyler) where TStruct : struct;
    T From<TStruct>(ICollection<TStruct?>? value, OrderedCollectionPredicate<TStruct?> itemPredicate, StructStyler<TStruct> structStyler) where TStruct : struct;

    T From<TStruct>(IEnumerable<TStruct>? value, OrderedCollectionPredicate<TStruct> itemPredicate, StructStyler<TStruct> structStyler) where TStruct : struct;
    T From<TStruct>(IEnumerable<TStruct?>? value, OrderedCollectionPredicate<TStruct?> itemPredicate, StructStyler<TStruct> structStyler) where TStruct : struct;

    T From<TStruct>(IEnumerator<TStruct>? value, OrderedCollectionPredicate<TStruct> itemPredicate, StructStyler<TStruct> structStyler) where TStruct : struct;
    T From<TStruct>(IEnumerator<TStruct?>? value, OrderedCollectionPredicate<TStruct?> itemPredicate, StructStyler<TStruct> structStyler) where TStruct : struct;
    

    T From(string[]? value, OrderedCollectionPredicate<string?> itemPredicate);
    T From(ReadOnlySpan<string?> value, OrderedCollectionPredicate<string?> itemPredicate);
    T From(IReadOnlyList<string?>? value, OrderedCollectionPredicate<string?> itemPredicate);
    T From(ICollection<string?>? value, OrderedCollectionPredicate<string?> itemPredicate);
    T From(IEnumerable<string?>? value, OrderedCollectionPredicate<string?> itemPredicate);
    T From(IEnumerator<string?>? value, OrderedCollectionPredicate<string?> itemPredicate);
    
    T From(IStyledToStringObject?[]? value, OrderedCollectionPredicate<IStyledToStringObject?> itemPredicate);
    T From(ReadOnlySpan<IStyledToStringObject?> value, OrderedCollectionPredicate<IStyledToStringObject?> itemPredicate);
    T From(IReadOnlyList<IStyledToStringObject?>? value, OrderedCollectionPredicate<IStyledToStringObject?> itemPredicate);
    T From(ICollection<IStyledToStringObject?>? value, OrderedCollectionPredicate<IStyledToStringObject?> itemPredicate);
    T From(IEnumerable<IStyledToStringObject?>? value, OrderedCollectionPredicate<IStyledToStringObject?> itemPredicate);
    T From(IEnumerator<IStyledToStringObject?>? value, OrderedCollectionPredicate<IStyledToStringObject?> itemPredicate);

    T From(ICharSequence?[]? value, OrderedCollectionPredicate<ICharSequence?> itemPredicate);
    T From(ReadOnlySpan<ICharSequence?> value, OrderedCollectionPredicate<ICharSequence?> itemPredicate);
    T From(IReadOnlyList<ICharSequence?>? value, OrderedCollectionPredicate<ICharSequence?> itemPredicate);
    T From(ICollection<ICharSequence?>? value, OrderedCollectionPredicate<ICharSequence?> itemPredicate);
    T From(IEnumerable<ICharSequence?>? value, OrderedCollectionPredicate<ICharSequence?> itemPredicate);
    T From(IEnumerator<ICharSequence?>? value, OrderedCollectionPredicate<ICharSequence?> itemPredicate);

    T From(StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder?> itemPredicate);
    T From(ReadOnlySpan<StringBuilder?> value, OrderedCollectionPredicate<StringBuilder?> itemPredicate);
    T From(IReadOnlyList<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder?> itemPredicate);
    T From(ICollection<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder?> itemPredicate);
    T From(IEnumerable<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder?> itemPredicate);
    T From(IEnumerator<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder?> itemPredicate);

    
    [CallsObjectToString] T From(object?[]? value, OrderedCollectionPredicate<object?> itemPredicate);
    [CallsObjectToString] T From(ReadOnlySpan<object?> value, OrderedCollectionPredicate<object?> itemPredicate);
    [CallsObjectToString] T From(IReadOnlyList<object?>? value, OrderedCollectionPredicate<object?> itemPredicate);
    [CallsObjectToString] T From(ICollection<object?>? value, OrderedCollectionPredicate<object?> itemPredicate);
    [CallsObjectToString] T From(IEnumerable<object?>? value, OrderedCollectionPredicate<object?> itemPredicate);
    [CallsObjectToString] T From(IEnumerator<object?>? value, OrderedCollectionPredicate<object?> itemPredicate);
}

public class AddFilteredTypeIsOrderedCollection<TExt> : RecyclableObject, IAddFilteredTypeIsOrderedCollection<TExt>
    where TExt : StyledTypeBuilder
{
    private CollectionBuilderCompAccess<TExt> stb = null!;

    public AddFilteredTypeIsOrderedCollection<TExt> Initialize(CollectionBuilderCompAccess<TExt> styledComplexTypeBuilder)
    {
        stb = styledComplexTypeBuilder;

        return this;
    }

    public TExt From(bool[]? value, OrderedCollectionPredicate<bool> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                any = true;
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt From(bool?[]? value, OrderedCollectionPredicate<bool?> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
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
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }

    
    public TExt From(ReadOnlySpan<bool> value, OrderedCollectionPredicate<bool> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                any = true;
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt From(ReadOnlySpan<bool?> value, OrderedCollectionPredicate<bool?> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                any = true;
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt From(IReadOnlyList<bool>? value, OrderedCollectionPredicate<bool> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                any = true;
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt From(IReadOnlyList<bool?>? value, OrderedCollectionPredicate<bool?> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                any = true;
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt From(ICollection<bool>? value, OrderedCollectionPredicate<bool> itemPredicate)
        => From((IEnumerable<bool>?)value, itemPredicate);

    public TExt From(ICollection<bool?>? value, OrderedCollectionPredicate<bool?> itemPredicate) 
        => From((IEnumerable<bool?>?)value, itemPredicate);

    public TExt From(IEnumerable<bool>? value, OrderedCollectionPredicate<bool> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            var count = 0;
            foreach (var item in value)
            {
                if (!itemPredicate(count++, item)) continue;
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt From(IEnumerable<bool?>? value, OrderedCollectionPredicate<bool?> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            var count = 0;
            foreach (var item in value)
            {
                if (!itemPredicate(count++, item)) continue;
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt From(IEnumerator<bool>? value, OrderedCollectionPredicate<bool> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        var hasValue = value?.MoveNext() ?? false;
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
                stb.AppendOrNull(item);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt From(IEnumerator<bool?>? value, OrderedCollectionPredicate<bool?> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        var hasValue = value?.MoveNext() ?? false;
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
                stb.AppendOrNull(item);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt From<TNum>(TNum[]? value, OrderedCollectionPredicate<TNum> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                any = true;
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt From<TNum>(TNum?[]? value, OrderedCollectionPredicate<TNum?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                any = true;
                
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }

    
    public TExt From<TNum>(ReadOnlySpan<TNum> value, OrderedCollectionPredicate<TNum> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                any = true;
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt From<TNum>(ReadOnlySpan<TNum?> value, OrderedCollectionPredicate<TNum?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                any = true;
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt From<TNum>(IReadOnlyList<TNum>? value, OrderedCollectionPredicate<TNum> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                any = true;
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt From<TNum>(IReadOnlyList<TNum?>? value, OrderedCollectionPredicate<TNum?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                any = true;
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt From<TNum>(ICollection<TNum>? value, OrderedCollectionPredicate<TNum> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>
        => From((IEnumerable<TNum>?)value, itemPredicate);

    public TExt From<TNum>(ICollection<TNum?>? value, OrderedCollectionPredicate<TNum?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TNum : struct, INumber<TNum>
        => From((IEnumerable<TNum?>?)value, itemPredicate);

    public TExt From<TNum>(IEnumerable<TNum>? value, OrderedCollectionPredicate<TNum> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            var count = 0;
            foreach (var item in value)
            {
                if (!itemPredicate(count++, item)) continue;
                any = true;
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt From<TNum>(IEnumerable<TNum?>? value, OrderedCollectionPredicate<TNum?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            var count = 0;
            foreach (var item in value)
            {
                if (!itemPredicate(count++, item)) continue;
                any = true;
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt From<TNum>(IEnumerator<TNum>? value, OrderedCollectionPredicate<TNum> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        var hasValue = value?.MoveNext() ?? false;
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

    public TExt From<TNum>(IEnumerator<TNum?>? value, OrderedCollectionPredicate<TNum?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        var hasValue = value?.MoveNext() ?? false;
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

    public TExt From<TStruct>(TStruct[]? value, OrderedCollectionPredicate<TStruct> itemPredicate, StructStyler<TStruct> structStyler)
        where TStruct : struct
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                any = true;
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt From<TStruct>(TStruct?[]? value, OrderedCollectionPredicate<TStruct?> itemPredicate, StructStyler<TStruct> structStyler)
        where TStruct : struct
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
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
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }

    
    public TExt From<TStruct>(ReadOnlySpan<TStruct> value, OrderedCollectionPredicate<TStruct> itemPredicate, StructStyler<TStruct> structStyler)
        where TStruct : struct
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                any = true;
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt From<TStruct>(ReadOnlySpan<TStruct?> value, OrderedCollectionPredicate<TStruct?> itemPredicate, StructStyler<TStruct> structStyler)
        where TStruct : struct
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                any = true;
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt From<TStruct>(IReadOnlyList<TStruct>? value, OrderedCollectionPredicate<TStruct> itemPredicate, StructStyler<TStruct> structStyler)
        where TStruct : struct
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                any = true;
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt From<TStruct>(IReadOnlyList<TStruct?>? value, OrderedCollectionPredicate<TStruct?> itemPredicate, StructStyler<TStruct> structStyler)
        where TStruct : struct
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                any = true;
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt From<TStruct>(ICollection<TStruct>? value, OrderedCollectionPredicate<TStruct> itemPredicate, StructStyler<TStruct> structStyler)
        where TStruct : struct
        => From((IEnumerable<TStruct>?)value, itemPredicate, structStyler);

    public TExt From<TStruct>(ICollection<TStruct?>? value, OrderedCollectionPredicate<TStruct?> itemPredicate, StructStyler<TStruct> structStyler) 
        where TStruct : struct
        => From((IEnumerable<TStruct?>?)value, itemPredicate, structStyler);

    public TExt From<TStruct>(IEnumerable<TStruct>? value, OrderedCollectionPredicate<TStruct> itemPredicate, StructStyler<TStruct> structStyler)
        where TStruct : struct
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            var count = 0;
            foreach (var item in value)
            {
                if (!itemPredicate(count++, item)) continue;
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt From<TStruct>(IEnumerable<TStruct?>? value, OrderedCollectionPredicate<TStruct?> itemPredicate, StructStyler<TStruct> structStyler)
        where TStruct : struct
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            var count = 0;
            foreach (var item in value)
            {
                if (!itemPredicate(count++, item)) continue;
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt From<TStruct>(IEnumerator<TStruct>? value, OrderedCollectionPredicate<TStruct> itemPredicate, StructStyler<TStruct> structStyler)
        where TStruct : struct
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        var hasValue = value?.MoveNext() ?? false;
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
                stb.AppendOrNull(item);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt From<TStruct>(IEnumerator<TStruct?>? value, OrderedCollectionPredicate<TStruct?> itemPredicate, StructStyler<TStruct> structStyler)
        where TStruct : struct
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        var hasValue = value?.MoveNext() ?? false;
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
                stb.AppendOrNull(item);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }
    
    public TExt From(string?[]? value, OrderedCollectionPredicate<string?> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                any = true;
                stb.Sb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }


    public TExt From(ReadOnlySpan<string?> value, OrderedCollectionPredicate<string?> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                any = true;
                stb.Sb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt From(IReadOnlyList<string?>? value, OrderedCollectionPredicate<string?> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                any = true;
                stb.Sb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt From(ICollection<string?>? value, OrderedCollectionPredicate<string?> itemPredicate) 
        => From((IEnumerable<string?>?)value, itemPredicate);

    public TExt From(IEnumerable<string?>? value, OrderedCollectionPredicate<string?> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            var count = 0;
            foreach (var item in value)
            {
                if (!itemPredicate(count++, item)) continue;
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }


    public TExt From(IEnumerator<string?>? value, OrderedCollectionPredicate<string?> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        var hasValue = value?.MoveNext() ?? false;
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
                stb.AppendOrNull(item);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt From(IStyledToStringObject?[]? value, OrderedCollectionPredicate<IStyledToStringObject?> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }


    public TExt From(ReadOnlySpan<IStyledToStringObject?> value, OrderedCollectionPredicate<IStyledToStringObject?> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt From(IReadOnlyList<IStyledToStringObject?>? value, OrderedCollectionPredicate<IStyledToStringObject?> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt From(ICollection<IStyledToStringObject?>? value, OrderedCollectionPredicate<IStyledToStringObject?> itemPredicate) 
        => From((IEnumerable<IStyledToStringObject?>?)value, itemPredicate);

    public TExt From(IEnumerable<IStyledToStringObject?>? value, OrderedCollectionPredicate<IStyledToStringObject?> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            var count = 0;
            foreach (var item in value)
            {
                if (!itemPredicate(count++, item)) continue;
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }


    public TExt From(IEnumerator<IStyledToStringObject?>? value, OrderedCollectionPredicate<IStyledToStringObject?> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        var hasValue = value?.MoveNext() ?? false;
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
                stb.AppendOrNull(item);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt From(ICharSequence?[]? value, OrderedCollectionPredicate<ICharSequence?> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }


    public TExt From(ReadOnlySpan<ICharSequence?> value, OrderedCollectionPredicate<ICharSequence?> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt From(IReadOnlyList<ICharSequence?>? value, OrderedCollectionPredicate<ICharSequence?> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt From(ICollection<ICharSequence?>? value, OrderedCollectionPredicate<ICharSequence?> itemPredicate) 
        => From((IEnumerable<ICharSequence?>?)value, itemPredicate);

    public TExt From(IEnumerable<ICharSequence?>? value, OrderedCollectionPredicate<ICharSequence?> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            var count = 0;
            foreach (var item in value)
            {
                if (!itemPredicate(count++, item)) continue;
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }


    public TExt From(IEnumerator<ICharSequence?>? value, OrderedCollectionPredicate<ICharSequence?> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        var hasValue = value?.MoveNext() ?? false;
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
                stb.AppendOrNull(item);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    public TExt From(StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder?> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }


    public TExt From(ReadOnlySpan<StringBuilder?> value, OrderedCollectionPredicate<StringBuilder?> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt From(IReadOnlyList<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder?> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt From(ICollection<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder?> itemPredicate) 
        => From((IEnumerable<StringBuilder?>?)value, itemPredicate);

    public TExt From(IEnumerable<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder?> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            var count = 0;
            foreach (var item in value)
            {
                if (!itemPredicate(count++, item)) continue;
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }


    public TExt From(IEnumerator<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder?> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        var hasValue = value?.MoveNext() ?? false;
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
                stb.AppendOrNull(item);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    [CallsObjectToString] 
    public TExt From(object?[]? value, OrderedCollectionPredicate<object?> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }
    
    [CallsObjectToString] 
    public TExt From(ReadOnlySpan<object?> value, OrderedCollectionPredicate<object?> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }
    
    [CallsObjectToString] 
    public TExt From(IReadOnlyList<object?>? value, OrderedCollectionPredicate<object?> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!itemPredicate(i, item)) continue;
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }
    
    [CallsObjectToString] 
    public TExt From(ICollection<object?>? value, OrderedCollectionPredicate<object?> itemPredicate) 
        => From((IEnumerable<object?>?)value, itemPredicate);
    
    [CallsObjectToString] 
    public TExt From(IEnumerable<object?>? value, OrderedCollectionPredicate<object?> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any = false;
        if (value != null)
        {
            var count = 0;
            foreach (var item in value)
            {
                if (!itemPredicate(count++, item)) continue;
                any = true;
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

    [CallsObjectToString] 
    public TExt From(IEnumerator<object?>? value, OrderedCollectionPredicate<object?> itemPredicate)
    {
        stb.ConditionalCollectionPrefix();
        var any      = false;
        var hasValue = value?.MoveNext() ?? false;
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
                stb.AppendOrNull(item);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }

}
