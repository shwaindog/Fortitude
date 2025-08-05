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

    public TExt AddFiltered(bool?[]? value, OrderedCollectionPredicate<bool?> itemPredicate)
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

    
    public TExt AddFiltered(ReadOnlySpan<bool> value, OrderedCollectionPredicate<bool> itemPredicate)
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

    public TExt AddFiltered(ReadOnlySpan<bool?> value, OrderedCollectionPredicate<bool?> itemPredicate)
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

    public TExt AddFiltered(IReadOnlyList<bool>? value, OrderedCollectionPredicate<bool> itemPredicate)
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

    public TExt AddFiltered(IReadOnlyList<bool?>? value, OrderedCollectionPredicate<bool?> itemPredicate)
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

    public TExt AddFiltered(ICollection<bool>? value, OrderedCollectionPredicate<bool> itemPredicate)
        => AddFiltered((IEnumerable<bool>?)value, itemPredicate);

    public TExt AddFiltered(ICollection<bool?>? value, OrderedCollectionPredicate<bool?> itemPredicate) 
        => AddFiltered((IEnumerable<bool?>?)value, itemPredicate);

    public TExt AddFiltered(IEnumerable<bool>? value, OrderedCollectionPredicate<bool> itemPredicate)
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

    public TExt AddFiltered(IEnumerable<bool?>? value, OrderedCollectionPredicate<bool?> itemPredicate)
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

    public TExt AddFiltered(IEnumerator<bool>? value, OrderedCollectionPredicate<bool> itemPredicate)
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

    public TExt AddFiltered(IEnumerator<bool?>? value, OrderedCollectionPredicate<bool?> itemPredicate)
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

    public TExt AddFiltered<TNum>(TNum[]? value, OrderedCollectionPredicate<TNum> itemPredicate
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

    public TExt AddFiltered<TNum>(TNum?[]? value, OrderedCollectionPredicate<TNum?> itemPredicate
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

    
    public TExt AddFiltered<TNum>(ReadOnlySpan<TNum> value, OrderedCollectionPredicate<TNum> itemPredicate
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

    public TExt AddFiltered<TNum>(ReadOnlySpan<TNum?> value, OrderedCollectionPredicate<TNum?> itemPredicate
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

    public TExt AddFiltered<TNum>(IReadOnlyList<TNum>? value, OrderedCollectionPredicate<TNum> itemPredicate
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

    public TExt AddFiltered<TNum>(IReadOnlyList<TNum?>? value, OrderedCollectionPredicate<TNum?> itemPredicate
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

    public TExt AddFiltered<TNum>(ICollection<TNum>? value, OrderedCollectionPredicate<TNum> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>
        => AddFiltered((IEnumerable<TNum>?)value, itemPredicate);

    public TExt AddFiltered<TNum>(ICollection<TNum?>? value, OrderedCollectionPredicate<TNum?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TNum : struct, INumber<TNum>
        => AddFiltered((IEnumerable<TNum?>?)value, itemPredicate);

    public TExt AddFiltered<TNum>(IEnumerable<TNum>? value, OrderedCollectionPredicate<TNum> itemPredicate
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

    public TExt AddFiltered<TNum>(IEnumerable<TNum?>? value, OrderedCollectionPredicate<TNum?> itemPredicate
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

    public TExt AddFiltered<TNum>(IEnumerator<TNum>? value, OrderedCollectionPredicate<TNum> itemPredicate
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

    public TExt AddFiltered<TNum>(IEnumerator<TNum?>? value, OrderedCollectionPredicate<TNum?> itemPredicate
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

    public TExt AddFiltered<TStruct>(TStruct[]? value, OrderedCollectionPredicate<TStruct> itemPredicate, StructStyler<TStruct> structStyler)
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

    public TExt AddFiltered<TStruct>(TStruct?[]? value, OrderedCollectionPredicate<TStruct?> itemPredicate, StructStyler<TStruct> structStyler)
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

    
    public TExt AddFiltered<TStruct>(ReadOnlySpan<TStruct> value, OrderedCollectionPredicate<TStruct> itemPredicate, StructStyler<TStruct> structStyler)
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

    public TExt AddFiltered<TStruct>(ReadOnlySpan<TStruct?> value, OrderedCollectionPredicate<TStruct?> itemPredicate, StructStyler<TStruct> structStyler)
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

    public TExt AddFiltered<TStruct>(IReadOnlyList<TStruct>? value, OrderedCollectionPredicate<TStruct> itemPredicate, StructStyler<TStruct> structStyler)
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

    public TExt AddFiltered<TStruct>(IReadOnlyList<TStruct?>? value, OrderedCollectionPredicate<TStruct?> itemPredicate, StructStyler<TStruct> structStyler)
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

    public TExt AddFiltered<TStruct>(ICollection<TStruct>? value, OrderedCollectionPredicate<TStruct> itemPredicate, StructStyler<TStruct> structStyler)
        where TStruct : struct
        => AddFiltered((IEnumerable<TStruct>?)value, itemPredicate, structStyler);

    public TExt AddFiltered<TStruct>(ICollection<TStruct?>? value, OrderedCollectionPredicate<TStruct?> itemPredicate, StructStyler<TStruct> structStyler) 
        where TStruct : struct
        => AddFiltered((IEnumerable<TStruct?>?)value, itemPredicate, structStyler);

    public TExt AddFiltered<TStruct>(IEnumerable<TStruct>? value, OrderedCollectionPredicate<TStruct> itemPredicate, StructStyler<TStruct> structStyler)
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

    public TExt AddFiltered<TStruct>(IEnumerable<TStruct?>? value, OrderedCollectionPredicate<TStruct?> itemPredicate, StructStyler<TStruct> structStyler)
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

    public TExt AddFiltered<TStruct>(IEnumerator<TStruct>? value, OrderedCollectionPredicate<TStruct> itemPredicate, StructStyler<TStruct> structStyler)
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

    public TExt AddFiltered<TStruct>(IEnumerator<TStruct?>? value, OrderedCollectionPredicate<TStruct?> itemPredicate, StructStyler<TStruct> structStyler)
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
    
    public TExt AddFiltered(string?[]? value, OrderedCollectionPredicate<string?> itemPredicate)
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


    public TExt AddFiltered(ReadOnlySpan<string?> value, OrderedCollectionPredicate<string?> itemPredicate)
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

    public TExt AddFiltered(IReadOnlyList<string?>? value, OrderedCollectionPredicate<string?> itemPredicate)
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

    public TExt AddFiltered(ICollection<string?>? value, OrderedCollectionPredicate<string?> itemPredicate) 
        => AddFiltered((IEnumerable<string?>?)value, itemPredicate);

    public TExt AddFiltered(IEnumerable<string?>? value, OrderedCollectionPredicate<string?> itemPredicate)
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


    public TExt AddFiltered(IEnumerator<string?>? value, OrderedCollectionPredicate<string?> itemPredicate)
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

    public TExt AddFiltered(IStyledToStringObject?[]? value, OrderedCollectionPredicate<IStyledToStringObject?> itemPredicate)
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


    public TExt AddFiltered(ReadOnlySpan<IStyledToStringObject?> value, OrderedCollectionPredicate<IStyledToStringObject?> itemPredicate)
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

    public TExt AddFiltered(IReadOnlyList<IStyledToStringObject?>? value, OrderedCollectionPredicate<IStyledToStringObject?> itemPredicate)
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

    public TExt AddFiltered(ICollection<IStyledToStringObject?>? value, OrderedCollectionPredicate<IStyledToStringObject?> itemPredicate) 
        => AddFiltered((IEnumerable<IStyledToStringObject?>?)value, itemPredicate);

    public TExt AddFiltered(IEnumerable<IStyledToStringObject?>? value, OrderedCollectionPredicate<IStyledToStringObject?> itemPredicate)
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


    public TExt AddFiltered(IEnumerator<IStyledToStringObject?>? value, OrderedCollectionPredicate<IStyledToStringObject?> itemPredicate)
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

    public TExt AddFiltered(ICharSequence?[]? value, OrderedCollectionPredicate<ICharSequence?> itemPredicate)
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


    public TExt AddFiltered(ReadOnlySpan<ICharSequence?> value, OrderedCollectionPredicate<ICharSequence?> itemPredicate)
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

    public TExt AddFiltered(IReadOnlyList<ICharSequence?>? value, OrderedCollectionPredicate<ICharSequence?> itemPredicate)
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

    public TExt AddFiltered(ICollection<ICharSequence?>? value, OrderedCollectionPredicate<ICharSequence?> itemPredicate) 
        => AddFiltered((IEnumerable<ICharSequence?>?)value, itemPredicate);

    public TExt AddFiltered(IEnumerable<ICharSequence?>? value, OrderedCollectionPredicate<ICharSequence?> itemPredicate)
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


    public TExt AddFiltered(IEnumerator<ICharSequence?>? value, OrderedCollectionPredicate<ICharSequence?> itemPredicate)
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

    public TExt AddFiltered(StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder?> itemPredicate)
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


    public TExt AddFiltered(ReadOnlySpan<StringBuilder?> value, OrderedCollectionPredicate<StringBuilder?> itemPredicate)
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

    public TExt AddFiltered(IReadOnlyList<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder?> itemPredicate)
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

    public TExt AddFiltered(ICollection<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder?> itemPredicate) 
        => AddFiltered((IEnumerable<StringBuilder?>?)value, itemPredicate);

    public TExt AddFiltered(IEnumerable<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder?> itemPredicate)
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


    public TExt AddFiltered(IEnumerator<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder?> itemPredicate)
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
    public TExt AddFiltered(object?[]? value, OrderedCollectionPredicate<object?> itemPredicate)
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
    public TExt AddFiltered(ReadOnlySpan<object?> value, OrderedCollectionPredicate<object?> itemPredicate)
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
    public TExt AddFiltered(IReadOnlyList<object?>? value, OrderedCollectionPredicate<object?> itemPredicate)
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
    public TExt AddFiltered(ICollection<object?>? value, OrderedCollectionPredicate<object?> itemPredicate) 
        => AddFiltered((IEnumerable<object?>?)value, itemPredicate);
    
    [CallsObjectToString] 
    public TExt AddFiltered(IEnumerable<object?>? value, OrderedCollectionPredicate<object?> itemPredicate)
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
    public TExt AddFiltered(IEnumerator<object?>? value, OrderedCollectionPredicate<object?> itemPredicate)
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
