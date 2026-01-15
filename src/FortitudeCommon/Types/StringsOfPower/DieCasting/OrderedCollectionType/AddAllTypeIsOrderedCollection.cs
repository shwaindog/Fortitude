// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;

public partial class OrderedCollectionMold<TOCMold>
    where TOCMold : TypeMolder
{
    public TOCMold AddAll(bool[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<Memory<bool>>(value?.GetType(), "", formatFlags)) return stb.WasSkipped<Memory<bool>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(bool);
        stb.ConditionalCollectionPrefix(elementType, value?.Length > 0);

        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(bool?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<bool?[]>(value?.GetType(), "", formatFlags)) return stb.WasSkipped<bool?[]>(value?.GetType(), "", formatFlags);
        var elementType = typeof(bool?);
        stb.ConditionalCollectionPrefix(elementType, value?.Length > 0);

        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(Span<bool> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<Memory<bool>>(value.Length > 0 ? typeof(Span<bool>) : null, "", formatFlags))
            return stb.WasSkipped<Memory<bool>>(value.Length > 0 ? typeof(Span<bool>) : null, "", formatFlags);
        var elementType = typeof(bool);
        stb.ConditionalCollectionPrefix(elementType, value.Length > 0);

        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(ReadOnlySpan<bool> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<ReadOnlyMemory<bool>>(value.Length > 0 ? typeof(ReadOnlySpan<bool>) : null, "", formatFlags))
            return stb.WasSkipped<ReadOnlyMemory<bool>>(value.Length > 0 ? typeof(ReadOnlySpan<bool>) : null, "", formatFlags);
        var elementType = typeof(bool);
        stb.ConditionalCollectionPrefix(elementType, value.Length > 0);

        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(Span<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<Memory<bool?>>(value.Length > 0 ? typeof(Span<bool?>) : null, "", formatFlags))
            return stb.WasSkipped<Memory<bool?>>(value.Length > 0 ? typeof(Span<bool?>) : null, "", formatFlags);
        var elementType = typeof(bool?);
        stb.ConditionalCollectionPrefix(elementType, value.Length > 0);
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(ReadOnlySpan<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<ReadOnlyMemory<bool?>>(value.Length > 0 ? typeof(ReadOnlySpan<bool?>) : null, "", formatFlags))
            return stb.WasSkipped<ReadOnlyMemory<bool?>>(value.Length > 0 ? typeof(ReadOnlySpan<bool?>) : null, "", formatFlags);
        var elementType = typeof(bool?);
        stb.ConditionalCollectionPrefix(elementType, value.Length > 0);
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(IReadOnlyList<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<IReadOnlyList<bool>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyList<bool>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(bool);
        stb.ConditionalCollectionPrefix(elementType, value?.Count > 0);
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(IReadOnlyList<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<IReadOnlyList<bool?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyList<bool?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(bool?);
        stb.ConditionalCollectionPrefix(elementType, value?.Count > 0);
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate(IEnumerable<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<IEnumerable<bool>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerable<bool>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(bool);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            formatString ??= "";
            foreach (var item in value)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? itemCount : null, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate(IEnumerable<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<IEnumerable<bool?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerable<bool?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(bool);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            formatString ??= "";
            foreach (var item in value)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? itemCount : null, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate(IEnumerator<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<IEnumerator<bool>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerator<bool>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(bool);
        var any         = false;
        var itemCount   = 0;
        var hasValue    = value?.MoveNext() ?? false;
        if (hasValue)
        {
            formatString ??= "";
            while (hasValue)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value!.Current;

                any = true;
                stb.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | AsCollection);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? itemCount : null, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate(IEnumerator<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<IEnumerator<bool?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerator<bool?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(bool);
        var any         = false;
        var itemCount   = 0;
        var hasValue    = value?.MoveNext() ?? false;
        if (hasValue)
        {
            formatString ??= "";
            while (hasValue)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value!.Current;

                any = true;
                stb.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | AsCollection);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? itemCount : null, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll<TFmt>(TFmt?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable
    {
        if (stb.HasSkipBody<TFmt?[]>(value?.GetType(), "", formatFlags)) return stb.WasSkipped<TFmt?[]>(value?.GetType(), "", formatFlags);
        var elementType = typeof(TFmt);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? value?.Length : null, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll<TFmtStruct>(TFmtStruct?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.HasSkipBody<TFmtStruct?[]>(value?.GetType(), "", formatFlags)) return stb.WasSkipped<TFmtStruct?[]>(value?.GetType(), "", formatFlags);
        var elementType = typeof(TFmtStruct?);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? value?.Length : null, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll<TFmt>(Span<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        if (stb.HasSkipBody<Memory<TFmt>>(value.Length > 0 ? typeof(Span<TFmt>) : null, "", formatFlags))
            return stb.WasSkipped<Memory<TFmt>>(value.Length > 0 ? typeof(Span<TFmt>) : null, "", formatFlags);
        var elementType = typeof(TFmt);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll<TFmt>(ReadOnlySpan<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        if (stb.HasSkipBody<ReadOnlyMemory<TFmt>>(value.Length > 0 ? typeof(ReadOnlySpan<TFmt>) : null, "", formatFlags))
            return stb.WasSkipped<ReadOnlyMemory<TFmt>>(value.Length > 0 ? typeof(ReadOnlySpan<TFmt>) : null, "", formatFlags);
        var elementType = typeof(TFmt);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll<TFmtStruct>(Span<TFmtStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.HasSkipBody<Memory<TFmtStruct?>>(value.Length > 0 ? typeof(Span<TFmtStruct?>) : null, "", formatFlags))
            return stb.WasSkipped<Memory<TFmtStruct?>>(value.Length > 0 ? typeof(Span<TFmtStruct?>) : null, "", formatFlags);
        var elementType = typeof(TFmtStruct?);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll<TFmtStruct>(ReadOnlySpan<TFmtStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.HasSkipBody<ReadOnlyMemory<TFmtStruct?>>(value.Length > 0 ? typeof(ReadOnlySpan<TFmtStruct?>) : null, "", formatFlags))
            return stb.WasSkipped<ReadOnlyMemory<TFmtStruct?>>(value.Length > 0 ? typeof(ReadOnlySpan<TFmtStruct?>) : null, "", formatFlags);
        var elementType = typeof(TFmtStruct?);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll<TFmt>(IReadOnlyList<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable
    {
        if (stb.HasSkipBody<IReadOnlyList<TFmt?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyList<TFmt?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(TFmt);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll<TFmtStruct>(IReadOnlyList<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.HasSkipBody<IReadOnlyList<TFmtStruct?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyList<TFmtStruct?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(TFmtStruct?);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate<TFmt>(IEnumerable<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable
    {
        if (stb.HasSkipBody<IEnumerable<TFmt?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerable<TFmt?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(TFmt);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            formatString ??= "";
            foreach (var item in value)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? itemCount : null, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate<TFmtStruct>(IEnumerable<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.HasSkipBody<IEnumerable<TFmtStruct?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerable<TFmtStruct?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(TFmtStruct?);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            formatString ??= "";
            foreach (var item in value)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | AsCollection);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? itemCount : null, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate<TFmt>(IEnumerator<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable
    {
        if (stb.HasSkipBody<IEnumerator<TFmt?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerator<TFmt?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(TFmt);
        var any         = false;
        var itemCount   = 0;
        var hasValue    = value?.MoveNext() ?? false;
        if (hasValue)
        {
            formatString ??= "";
            while (hasValue)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                var item = value!.Current;
                stb.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | AsCollection);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? itemCount : null, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate<TFmtStruct>(IEnumerator<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.HasSkipBody<IEnumerator<TFmtStruct?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerator<TFmtStruct?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(TFmtStruct?);
        var any         = false;
        var itemCount   = 0;
        var hasValue    = value?.MoveNext() ?? false;
        if (hasValue)
        {
            formatString ??= "";
            while (hasValue)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                var item = value!.Current;
                stb.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | AsCollection);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? itemCount : null, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TCloaked, TRevealBase>(TCloaked?[]? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        if (stb.HasSkipBody<TCloaked?[]>(value?.GetType(), "", formatFlags)) return stb.WasSkipped<TCloaked?[]>(value?.GetType(), "", formatFlags);
        var elementType = typeof(TCloaked);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.RevealCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? value?.Length : null, "", formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }


    public TOCMold RevealAll<TCloakedStruct>(TCloakedStruct?[]? value, PalantírReveal<TCloakedStruct> palantírReveal
        , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        if (stb.HasSkipBody<TCloakedStruct?[]>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<TCloakedStruct?[]>(value?.GetType(), "", formatFlags);
        var elementType = typeof(TCloakedStruct);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.RevealNullableCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? value?.Length : null, "", formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TCloaked, TRevealBase>(Span<TCloaked> value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        if (stb.HasSkipBody<Memory<TCloaked>>(value.Length > 0 ? typeof(Span<TCloaked>) : null, "", formatFlags))
            return stb.WasSkipped<Memory<TCloaked>>(value.Length > 0 ? typeof(Span<TCloaked>) : null, "", formatFlags);
        var elementType = typeof(TCloaked);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.RevealCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, "", formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TCloakedStruct>(Span<TCloakedStruct?> value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        if (stb.HasSkipBody<Memory<TCloakedStruct>>(value.Length > 0 ? typeof(Span<TCloakedStruct>) : null, "", formatFlags))
            return stb.WasSkipped<Memory<TCloakedStruct>>(value.Length > 0 ? typeof(Span<TCloakedStruct>) : null, "", formatFlags);
        var elementType = typeof(TCloakedStruct);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.RevealNullableCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, "", formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TCloaked, TRevealBase>(ReadOnlySpan<TCloaked> value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        if (stb.HasSkipBody<ReadOnlyMemory<TCloaked>>(value.Length > 0 ? typeof(ReadOnlySpan<TCloaked>) : null, "", formatFlags))
            return stb.WasSkipped<ReadOnlyMemory<TCloaked>>(value.Length > 0 ? typeof(ReadOnlySpan<TCloaked>) : null, "", formatFlags);
        var elementType = typeof(TCloaked);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.RevealCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, "", formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TCloakedStruct>(ReadOnlySpan<TCloakedStruct?> value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        if (stb.HasSkipBody<ReadOnlyMemory<TCloakedStruct>>(value.Length > 0 ? typeof(ReadOnlySpan<TCloakedStruct>) : null, "", formatFlags))
            return stb.WasSkipped<ReadOnlyMemory<TCloakedStruct>>(value.Length > 0 ? typeof(ReadOnlySpan<TCloakedStruct>) : null, "", formatFlags);
        var elementType = typeof(TCloakedStruct);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.RevealNullableCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, "", formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TCloaked, TRevealBase>(IReadOnlyList<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        if (stb.HasSkipBody<IReadOnlyList<TCloaked?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyList<TCloaked?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(TCloaked);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.RevealCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count, "", formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TCloakedStruct>(IReadOnlyList<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        if (stb.HasSkipBody<IReadOnlyList<TCloakedStruct?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyList<TCloakedStruct?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(TCloakedStruct);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.RevealNullableCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count, "", formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAllEnumerate<TCloaked, TRevealBase>(IEnumerable<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        if (stb.HasSkipBody<IEnumerable<TCloaked?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerable<TCloaked?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(TCloaked);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            foreach (var item in value)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.RevealCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? itemCount : null, "", formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAllEnumerate<TCloakedStruct>(IEnumerable<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        if (stb.HasSkipBody<IEnumerable<TCloakedStruct?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerable<TCloakedStruct?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(TCloakedStruct);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            foreach (var item in value)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.RevealNullableCloakedBearerOrNull(item, palantírReveal);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? itemCount : null, "", formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAllEnumerate<TCloaked, TRevealBase>(IEnumerator<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        if (stb.HasSkipBody<IEnumerator<TCloaked?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerator<TCloaked?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(TCloaked);
        var any         = false;
        var itemCount   = 0;
        var hasValue    = value?.MoveNext() ?? false;
        if (hasValue)
        {
            while (hasValue)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                var item = value!.Current;
                stb.RevealCloakedBearerOrNull(item, palantírReveal);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? itemCount : null, "", formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAllEnumerate<TCloakedStruct>(IEnumerator<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        if (stb.HasSkipBody<IEnumerator<TCloakedStruct?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerator<TCloakedStruct?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(TCloakedStruct);
        var any         = false;
        var itemCount   = 0;
        var hasValue    = value?.MoveNext() ?? false;
        if (hasValue)
        {
            while (hasValue)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                var item = value!.Current;
                stb.RevealNullableCloakedBearerOrNull(item, palantírReveal);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? itemCount : null, "", formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TBearer>(TBearer[]? value, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        if (stb.HasSkipBody<TBearer[]>(value?.GetType(), "", formatFlags)) return stb.WasSkipped<TBearer[]>(value?.GetType()
       , "", formatFlags);
        var elementType = typeof(TBearer);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.RevealStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length, "", formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TBearerStruct>(TBearerStruct?[]? value, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.HasSkipBody<TBearerStruct[]>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<TBearerStruct[]>(value?.GetType(), "", formatFlags);
        var elementType = typeof(TBearerStruct);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.RevealNullableStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length, "", formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TBearer>(Span<TBearer> value, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        if (stb.HasSkipBody<Memory<TBearer>>(value.Length > 0 ? typeof(Span<TBearer>) : null, "", formatFlags))
            return stb.WasSkipped<Memory<TBearer>>(value.Length > 0 ? typeof(Span<TBearer>) : null, "", formatFlags);
        var elementType = typeof(TBearer);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.RevealStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, "", formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TBearerStruct>(Span<TBearerStruct?> value, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.HasSkipBody<Memory<TBearerStruct?>>(value.Length > 0 ? typeof(Span<TBearerStruct?>) : null, "", formatFlags))
            return stb.WasSkipped<Memory<TBearerStruct?>>(value.Length > 0 ? typeof(Span<TBearerStruct?>) : null, "", formatFlags);
        var elementType = typeof(TBearerStruct);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.RevealNullableStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, "", formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TBearer>(ReadOnlySpan<TBearer> value, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        if (stb.HasSkipBody<ReadOnlyMemory<TBearer?>>(value.Length > 0 ? typeof(ReadOnlySpan<TBearer?>) : null, "", formatFlags))
            return stb.WasSkipped<ReadOnlyMemory<TBearer?>>(value.Length > 0 ? typeof(ReadOnlySpan<TBearer?>) : null, "", formatFlags);
        var elementType = typeof(TBearer);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.RevealStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, "", formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TBearerStruct>(ReadOnlySpan<TBearerStruct?> value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.HasSkipBody<ReadOnlyMemory<TBearerStruct?>>(value.Length > 0 ? typeof(ReadOnlySpan<TBearerStruct?>) : null, "", formatFlags))
            return stb.WasSkipped<ReadOnlyMemory<TBearerStruct?>>(value.Length > 0 ? typeof(ReadOnlySpan<TBearerStruct?>) : null, "", formatFlags);
        var elementType = typeof(TBearerStruct);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.RevealNullableStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, "", formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TBearer>(IReadOnlyList<TBearer>? value, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        if (stb.HasSkipBody<IReadOnlyList<TBearer?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyList<TBearer?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(TBearer);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.RevealStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count, "", formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TBearerStruct>(IReadOnlyList<TBearerStruct?>? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.HasSkipBody<IReadOnlyList<TBearerStruct?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyList<TBearerStruct?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(TBearerStruct);
        var any         = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.RevealNullableStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count, "", formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAllEnumerate<TBearer>(IEnumerable<TBearer>? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        if (stb.HasSkipBody<IEnumerable<TBearer?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerable<TBearer?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(TBearer);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            foreach (var item in value)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.RevealStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? itemCount : null, "", formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAllEnumerate<TBearerStruct>(IEnumerable<TBearerStruct?>? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.HasSkipBody<IEnumerable<TBearerStruct>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerable<TBearerStruct>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(TBearerStruct);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            foreach (var item in value)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.RevealNullableStringBearerOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? itemCount : null, "", formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAllEnumerate<TBearer>(IEnumerator<TBearer>? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        if (stb.HasSkipBody<IEnumerator<TBearer?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerator<TBearer?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(TBearer);
        var any         = false;
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        if (hasValue)
        {
            while (hasValue)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value!.Current;

                any = true;
                stb.RevealStringBearerOrNull(item);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? itemCount : null, "", formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAllEnumerate<TBearerStruct>(IEnumerator<TBearerStruct?>? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.HasSkipBody<IEnumerable<TBearerStruct?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerable<TBearerStruct?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(TBearerStruct);
        var any         = false;
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        if (hasValue)
        {
            while (hasValue)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value!.Current;

                any = true;
                stb.RevealNullableStringBearerOrNull(item);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? itemCount : null, "", formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(string?[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<string?[]>(value?.GetType(), "", formatFlags)) return stb.WasSkipped<string?[]>(value?.GetType(), "", formatFlags);
        var elementType = typeof(string);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? value?.Length : null, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(Span<string> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<Memory<string>>(value.Length > 0 ? typeof(Span<string>) : null, "", formatFlags))
            return stb.WasSkipped<Memory<string>>(value.Length > 0 ? typeof(Span<string>) : null, "", formatFlags);
        var elementType = typeof(string);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllNullable(Span<string?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<Memory<string?>>(value.Length > 0 ? typeof(Span<string?>) : null, "", formatFlags))
            return stb.WasSkipped<Memory<string?>>(value.Length > 0 ? typeof(Span<string?>) : null, "", formatFlags);
        var elementType = typeof(string);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(ReadOnlySpan<string> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<ReadOnlyMemory<string>>(value.Length > 0 ? typeof(ReadOnlySpan<string>) : null, "", formatFlags))
            return stb.WasSkipped<ReadOnlyMemory<string>>(value.Length > 0 ? typeof(ReadOnlySpan<string>) : null, "", formatFlags);
        var elementType = typeof(string);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllNullable(ReadOnlySpan<string?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<ReadOnlyMemory<string?>>(value.Length > 0 ? typeof(ReadOnlySpan<string?>) : null, "", formatFlags))
            return stb.WasSkipped<ReadOnlyMemory<string?>>(value.Length > 0 ? typeof(ReadOnlySpan<string?>) : null, "", formatFlags);
        var elementType = typeof(string);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(IReadOnlyList<string?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<IReadOnlyList<string?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyList<string?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(string);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate(IEnumerable<string?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<IEnumerable<string?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerable<string?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(string);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            formatString ??= "";
            foreach (var item in value)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? itemCount : null, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate(IEnumerator<string?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<IEnumerator<string?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerator<string?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(string);
        var any         = false;
        var itemCount   = 0;
        var hasValue    = value?.MoveNext() ?? false;
        if (hasValue)
        {
            formatString ??= "";
            while (hasValue)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value!.Current;

                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString, formatFlags);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? itemCount : null, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllCharSeq<TCharSeq>(TCharSeq?[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        if (stb.HasSkipBody<TCharSeq?[]>(value?.GetType(), "", formatFlags)) return stb.WasSkipped<TCharSeq?[]>(value?.GetType(), "", formatFlags);
        var elementType = typeof(TCharSeq);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? value?.Length : null, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllCharSeq<TCharSeq>(Span<TCharSeq> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?
    {
        if (stb.HasSkipBody<Memory<TCharSeq>>(value.Length > 0 ? typeof(Span<TCharSeq>) : null, "", formatFlags))
            return stb.WasSkipped<Memory<TCharSeq>>(value.Length > 0 ? typeof(Span<TCharSeq>) : null, "", formatFlags);
        var elementType = typeof(TCharSeq);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllCharSeq<TCharSeq>(ReadOnlySpan<TCharSeq> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?
    {
        if (stb.HasSkipBody<ReadOnlyMemory<TCharSeq>>(value.Length > 0 ? typeof(ReadOnlySpan<TCharSeq>) : null, "", formatFlags))
            return stb.WasSkipped<ReadOnlyMemory<TCharSeq>>(value.Length > 0 ? typeof(ReadOnlySpan<TCharSeq>) : null, "", formatFlags);
        var elementType = typeof(TCharSeq);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllCharSeq<TCharSeq>(IReadOnlyList<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        if (stb.HasSkipBody<IReadOnlyList<TCharSeq?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyList<TCharSeq?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(TCharSeq);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllCharSeqEnumerate<TCharSeq>(IEnumerable<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        if (stb.HasSkipBody<IEnumerable<TCharSeq?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerable<TCharSeq?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(TCharSeq);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            formatString ??= "";
            foreach (var item in value)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? itemCount : null, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllCharSeqEnumerate<TCharSeq>(IEnumerator<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        if (stb.HasSkipBody<IEnumerator<TCharSeq?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerator<TCharSeq?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(TCharSeq);
        var any         = false;
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        if (hasValue)
        {
            formatString ??= "";
            while (hasValue)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value!.Current;

                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString, formatFlags);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? itemCount : null, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(StringBuilder?[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<IEnumerator<StringBuilder?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerator<StringBuilder?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(StringBuilder);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? value?.Length : null, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(Span<StringBuilder> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<Memory<StringBuilder>>(value.Length > 0 ? typeof(Span<StringBuilder>) : null, "", formatFlags))
            return stb.WasSkipped<Memory<StringBuilder>>(value.Length > 0 ? typeof(Span<StringBuilder>) : null, "", formatFlags);
        var elementType = typeof(StringBuilder);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllNullable(Span<StringBuilder?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<Memory<StringBuilder?>>(value.Length > 0 ? typeof(Span<StringBuilder?>) : null, "", formatFlags))
            return stb.WasSkipped<Memory<StringBuilder?>>(value.Length > 0 ? typeof(Span<StringBuilder?>) : null, "", formatFlags);
        var elementType = typeof(StringBuilder);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(ReadOnlySpan<StringBuilder> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<ReadOnlyMemory<StringBuilder>>(value.Length > 0 ? typeof(ReadOnlySpan<StringBuilder>) : null, "", formatFlags))
            return stb.WasSkipped<ReadOnlyMemory<StringBuilder>>(value.Length > 0 ? typeof(ReadOnlySpan<StringBuilder>) : null, "", formatFlags);
        var elementType = typeof(StringBuilder);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllNullable(ReadOnlySpan<StringBuilder?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<ReadOnlyMemory<StringBuilder?>>(value.Length > 0 ? typeof(ReadOnlySpan<StringBuilder?>) : null, "", formatFlags))
            return stb.WasSkipped<ReadOnlyMemory<StringBuilder?>>(value.Length > 0 ? typeof(ReadOnlySpan<StringBuilder?>) : null, "", formatFlags);
        var elementType = typeof(StringBuilder);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(IReadOnlyList<StringBuilder?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<IReadOnlyList<StringBuilder?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyList<StringBuilder?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(StringBuilder);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate(IEnumerable<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<IEnumerable<StringBuilder?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerable<StringBuilder?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(StringBuilder);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            formatString ??= "";
            foreach (var item in value)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? itemCount : null, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate(IEnumerator<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<IEnumerator<StringBuilder?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerator<StringBuilder?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(StringBuilder);
        var any         = false;
        var hasValue    = value?.MoveNext() ?? false;
        var itemCount   = 0;
        if (hasValue)
        {
            formatString ??= "";
            while (hasValue)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value!.Current;

                any = true;
                stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString, formatFlags);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? itemCount : null, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllMatch<TAny>(TAny[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<TAny[]>(value?.GetType(), "", formatFlags)) return stb.WasSkipped<TAny[]>(value?.GetType(), "", formatFlags);
        var elementType = typeof(TAny);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Length, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllMatch<TAny>(Span<TAny> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<Memory<TAny>>(value.Length > 0 ? typeof(Span<TAny?>) : null, "", formatFlags))
            return stb.WasSkipped<Memory<TAny>>(value.Length > 0 ? typeof(Span<TAny?>) : null, "", formatFlags);
        var elementType = typeof(TAny);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllMatch<TAny>(ReadOnlySpan<TAny> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<ReadOnlyMemory<TAny>>(value.Length > 0 ? typeof(ReadOnlySpan<TAny>) : null, "", formatFlags))
            return stb.WasSkipped<ReadOnlyMemory<TAny>>(value.Length > 0 ? typeof(ReadOnlySpan<TAny>) : null, "", formatFlags);
        var elementType = typeof(TAny);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllMatch<TAny>(IReadOnlyList<TAny>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<IReadOnlyList<TAny>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyList<TAny>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(TAny);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllMatchEnumerate<TAny>(IEnumerable<TAny>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<IEnumerable<TAny>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerable<TAny>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(TAny);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            formatString ??= "";
            foreach (var item in value)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, itemCount, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? itemCount : null, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllMatchEnumerate<TAny>(IEnumerator<TAny>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<IEnumerator<TAny>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerator<TAny>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(TAny);
        var any         = false;
        var itemCount   = 0;
        var hasValue    = value?.MoveNext() ?? false;
        if (hasValue)
        {
            formatString ??= "";
            while (hasValue)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value!.Current;

                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, itemCount, formatString, formatFlags);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? itemCount : null, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TOCMold AddAllObject(object?[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<object?[]>(value?.GetType(), "", formatFlags)) return stb.WasSkipped<object?[]>(value?.GetType(), "", formatFlags);
        var elementType = typeof(object);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? value?.Length : null, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TOCMold AddAllObject(Span<object> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<Memory<object>>(value.Length > 0 ? typeof(Span<object>) : null, "", formatFlags))
            return stb.WasSkipped<Memory<object>>(value.Length > 0 ? typeof(Span<object>) : null, "", formatFlags);
        var elementType = typeof(object);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TOCMold AddAllObjectNullable(Span<object?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<Memory<object?>>(value.Length > 0 ? typeof(Span<object?>) : null, "", formatFlags))
            return stb.WasSkipped<Memory<object?>>(value.Length > 0 ? typeof(Span<object?>) : null, "", formatFlags);
        var elementType = typeof(object);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TOCMold AddAllObject(ReadOnlySpan<object> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<ReadOnlyMemory<object>>(value.Length > 0 ? typeof(ReadOnlySpan<object>) : null, "", formatFlags))
            return stb.WasSkipped<ReadOnlyMemory<object>>(value.Length > 0 ? typeof(ReadOnlySpan<object>) : null, "", formatFlags);
        var elementType = typeof(object);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TOCMold AddAllObjectNullable(ReadOnlySpan<object?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<ReadOnlyMemory<object?>>(value.Length > 0 ? typeof(ReadOnlySpan<object?>) : null, "", formatFlags))
            return stb.WasSkipped<ReadOnlyMemory<object?>>(value.Length > 0 ? typeof(ReadOnlySpan<object?>) : null, "", formatFlags);
        var elementType = typeof(object);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value.Length, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TOCMold AddAllObject(IReadOnlyList<object?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<IReadOnlyList<object?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyList<object?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(object);
        var any         = false;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value[i];

                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, value?.Count, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TOCMold AddAllObjectEnumerate(IEnumerable<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<IEnumerable<object?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerable<object?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(object);
        var any         = false;
        var itemCount   = 0;
        if (value != null)
        {
            formatString ??= "";
            foreach (var item in value)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, itemCount, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? itemCount : null, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TOCMold AddAllObjectEnumerate(IEnumerator<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.HasSkipBody<IEnumerator<object?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerator<object?>>(value?.GetType(), "", formatFlags);
        var elementType = typeof(object);
        var any         = false;
        var itemCount   = 0;
        var hasValue    = value?.MoveNext() ?? false;
        if (hasValue)
        {
            formatString ??= "";
            while (hasValue)
            {
                if (!any) stb.ConditionalCollectionPrefix(elementType, true);
                var item = value!.Current;

                any = true;
                stb.AppendFormattedCollectionItemMatchOrNull(item, itemCount, formatString, formatFlags);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            if (!any) stb.ConditionalCollectionPrefix(elementType, false);
        }
        stb.ConditionalCollectionSuffix(elementType, any ? itemCount : null, formatString, formatFlags);
        return stb.WriteAsComplex ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
}
