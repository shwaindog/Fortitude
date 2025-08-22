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

    public TExt AddFilteredEnumerate(IEnumerable<bool>? value, OrderedCollectionPredicate<bool> itemPredicate)
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

    public TExt AddFilteredEnumerate(IEnumerable<bool?>? value, OrderedCollectionPredicate<bool?> itemPredicate)
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

    public TExt AddFilteredEnumerate(IEnumerator<bool>? value, OrderedCollectionPredicate<bool> itemPredicate)
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

    public TExt AddFilteredEnumerate(IEnumerator<bool?>? value, OrderedCollectionPredicate<bool?> itemPredicate)
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

    public TExt AddFiltered<TFmtStruct>(TFmtStruct[]? value, OrderedCollectionPredicate<TFmtStruct> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
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

    public TExt AddFiltered<TFmtStruct>(TFmtStruct?[]? value, OrderedCollectionPredicate<TFmtStruct?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
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

    
    public TExt AddFiltered<TFmtStruct>(ReadOnlySpan<TFmtStruct> value, OrderedCollectionPredicate<TFmtStruct> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
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

    public TExt AddFiltered<TFmtStruct>(ReadOnlySpan<TFmtStruct?> value, OrderedCollectionPredicate<TFmtStruct?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
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
    
    public TExt AddFiltered<TFmtStruct>(IReadOnlyList<TFmtStruct>? value, OrderedCollectionPredicate<TFmtStruct> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
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

    public TExt AddFilteredEnumerate<TFmtStruct>(IEnumerable<TFmtStruct?>? value, OrderedCollectionPredicate<TFmtStruct?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
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

    public TExt AddFilteredEnumerate<TFmtStruct>(IEnumerator<TFmtStruct?>? value, OrderedCollectionPredicate<TFmtStruct?> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
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

    public TExt AddFiltered<TStruct>(TStruct[]? value, OrderedCollectionPredicate<TStruct> itemPredicate, CustomTypeStyler<TStruct> customTypeStyler)
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

    public TExt AddFiltered<TStruct>(TStruct?[]? value, OrderedCollectionPredicate<TStruct?> itemPredicate, CustomTypeStyler<TStruct> customTypeStyler)
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

    
    public TExt AddFiltered<TStruct>(ReadOnlySpan<TStruct> value, OrderedCollectionPredicate<TStruct> itemPredicate, CustomTypeStyler<TStruct> customTypeStyler)
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

    public TExt AddFiltered<TStruct>(ReadOnlySpan<TStruct?> value, OrderedCollectionPredicate<TStruct?> itemPredicate, CustomTypeStyler<TStruct> customTypeStyler)
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

    public TExt AddFiltered<TStruct>(IReadOnlyList<TStruct>? value, OrderedCollectionPredicate<TStruct> itemPredicate, CustomTypeStyler<TStruct> customTypeStyler)
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

    public TExt AddFiltered<TStruct>(IReadOnlyList<TStruct?>? value, OrderedCollectionPredicate<TStruct?> itemPredicate, CustomTypeStyler<TStruct> customTypeStyler)
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

    public TExt AddFilteredEnumerate<TStruct>(IEnumerable<TStruct>? value, OrderedCollectionPredicate<TStruct> itemPredicate, CustomTypeStyler<TStruct> customTypeStyler)
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

    public TExt AddFilteredEnumerate<TStruct>(IEnumerable<TStruct?>? value, OrderedCollectionPredicate<TStruct?> itemPredicate, CustomTypeStyler<TStruct> customTypeStyler)
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

    public TExt AddFilteredEnumerate<TStruct>(IEnumerator<TStruct>? value, OrderedCollectionPredicate<TStruct> itemPredicate, CustomTypeStyler<TStruct> customTypeStyler)
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

    public TExt AddFilteredEnumerate<TStruct>(IEnumerator<TStruct?>? value, OrderedCollectionPredicate<TStruct?> itemPredicate, CustomTypeStyler<TStruct> customTypeStyler)
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

    public TExt AddFilteredEnumerate(IEnumerable<string?>? value, OrderedCollectionPredicate<string?> itemPredicate)
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


    public TExt AddFilteredEnumerate(IEnumerator<string?>? value, OrderedCollectionPredicate<string?> itemPredicate)
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

    public TExt AddFilteredEnumerate(IEnumerable<ICharSequence?>? value, OrderedCollectionPredicate<ICharSequence?> itemPredicate)
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


    public TExt AddFilteredEnumerate(IEnumerator<ICharSequence?>? value, OrderedCollectionPredicate<ICharSequence?> itemPredicate)
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

    public TExt AddFilteredEnumerate(IEnumerable<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder?> itemPredicate)
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


    public TExt AddFilteredEnumerate(IEnumerator<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder?> itemPredicate)
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
    
    public TExt AddFiltered<TStyledObj>(TStyledObj[]? value, OrderedCollectionPredicate<IStyledToStringObject> itemPredicate)
    where TStyledObj : class, IStyledToStringObject
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


    public TExt AddFiltered<TStyledObj>(ReadOnlySpan<TStyledObj> value, OrderedCollectionPredicate<IStyledToStringObject> itemPredicate)
        where TStyledObj : class, IStyledToStringObject
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

    public TExt AddFiltered<TStyledObj>(IReadOnlyList<TStyledObj>? value, OrderedCollectionPredicate<TStyledObj> itemPredicate)
        where TStyledObj : class, IStyledToStringObject
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

    public TExt AddFilteredEnumerate<TStyledObj>(IEnumerable<TStyledObj>? value, OrderedCollectionPredicate<TStyledObj> itemPredicate)
        where TStyledObj : class, IStyledToStringObject
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


    public TExt AddFilteredEnumerate<TStyledObj>(IEnumerator<TStyledObj>? value, OrderedCollectionPredicate<TStyledObj> itemPredicate)
        where TStyledObj : class, IStyledToStringObject
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

    public TExt AddFiltered<T, TBase>(T?[]? value, OrderedCollectionPredicate<TBase?> itemPredicate
      , CustomTypeStyler<TBase?> customTypeStyler) where T : class, TBase where TBase: class
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
                stb.AppendOrNull(item, customTypeStyler);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }
    
    public TExt AddFiltered<T, TBase>(ReadOnlySpan<T?> value, OrderedCollectionPredicate<TBase?> itemPredicate
      , CustomTypeStyler<TBase?> customTypeStyler) where T : class, TBase where TBase: class
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
                stb.AppendOrNull(item, customTypeStyler);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AddFiltered<T, TBase>(IReadOnlyList<T?>? value, OrderedCollectionPredicate<TBase?> itemPredicate
      , CustomTypeStyler<TBase?> customTypeStyler) where T : class, TBase where TBase: class
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
                stb.AppendOrNull(item, customTypeStyler);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        stb.ConditionalCollectionSuffix();
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AddFilteredEnumerate<T, TBase>(IEnumerable<T?>? value, OrderedCollectionPredicate<TBase?> itemPredicate
      , CustomTypeStyler<TBase?> customTypeStyler) where T : class, TBase where TBase: class
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
                stb.AppendOrNull(item, customTypeStyler);
                stb.GoToNextCollectionItemStart();
            }
            if (any) stb.RemoveLastWhiteSpacedCommaIfFound();
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }
    
    public TExt AddFilteredEnumerate<T, TBase>(IEnumerator<T?>? value, OrderedCollectionPredicate<TBase?> itemPredicate
      , CustomTypeStyler<TBase?> customTypeStyler) where T : class, TBase where TBase: class
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
                stb.AppendOrNull(item, customTypeStyler);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
        }
        any |= stb.ConditionalCollectionSuffix();
        return any ? stb.Sb.AddGoToNext(stb) : stb.StyleTypeBuilder;
    }
    
    [CallsObjectToString] 
    public TExt AddFilteredMatch<T>(T[]? value, OrderedCollectionPredicate<T> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : class
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
    
    [CallsObjectToString] 
    public TExt AddFilteredMatch<T>(ReadOnlySpan<T> value, OrderedCollectionPredicate<T> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : class
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
    
    [CallsObjectToString] 
    public TExt AddFilteredMatch<T>(IReadOnlyList<T>? value, OrderedCollectionPredicate<T> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : class
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
    
    [CallsObjectToString] 
    public TExt AddFilteredMatchEnumerate<T>(IEnumerable<T>? value, OrderedCollectionPredicate<T> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : class
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

    [CallsObjectToString] 
    public TExt AddFilteredMatchEnumerate<T>(IEnumerator<T>? value, OrderedCollectionPredicate<T> itemPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : class
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

}
