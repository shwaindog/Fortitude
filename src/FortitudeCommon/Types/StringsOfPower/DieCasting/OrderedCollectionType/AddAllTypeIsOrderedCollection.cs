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
        var actualType = typeof(bool[]);
        if (stb.HasSkipBody(actualType, "", formatFlags)) return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(bool?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(bool?[]);
        if (stb.HasSkipBody(actualType, "", formatFlags)) return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(Span<bool> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<bool>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(ReadOnlySpan<bool> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<bool>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(Span<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<bool?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(ReadOnlySpan<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<bool?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(IReadOnlyList<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<bool>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(IReadOnlyList<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<bool?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate(IEnumerable<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<bool>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate(IEnumerable<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<bool?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate(IEnumerator<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<bool>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate(IEnumerator<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<bool?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll<TFmt>(TFmt?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(TFmt?[]);
        if (stb.HasSkipBody(actualType, "", formatFlags)) return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll<TFmtStruct>(TFmtStruct?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(TFmtStruct?[]);
        if (stb.HasSkipBody(actualType, "", formatFlags)) return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll<TFmt>(Span<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        var actualType = typeof(Span<TFmt>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll<TFmt>(ReadOnlySpan<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        var actualType = typeof(ReadOnlySpan<TFmt>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll<TFmtStruct>(Span<TFmtStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = typeof(Span<TFmtStruct?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll<TFmtStruct>(ReadOnlySpan<TFmtStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = typeof(ReadOnlySpan<TFmtStruct?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll<TFmt>(IReadOnlyList<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TFmt?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll<TFmtStruct>(IReadOnlyList<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TFmtStruct?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate<TFmt>(IEnumerable<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TFmt?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate<TFmtStruct>(IEnumerable<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TFmtStruct?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate<TFmt>(IEnumerator<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TFmt?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate<TFmtStruct>(IEnumerator<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TFmtStruct?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TCloaked, TRevealBase>(TCloaked?[]? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TCloaked?[]);
        if (stb.HasSkipBody(actualType, "", formatFlags)) return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }


    public TOCMold RevealAll<TCloakedStruct>(TCloakedStruct?[]? value, PalantírReveal<TCloakedStruct> palantírReveal
        , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(TCloakedStruct?[]);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TCloaked, TRevealBase>(Span<TCloaked> value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = typeof(Span<TCloaked>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TCloakedStruct>(Span<TCloakedStruct?> value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType = typeof(Span<TCloakedStruct?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TCloaked, TRevealBase>(ReadOnlySpan<TCloaked> value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = typeof(ReadOnlySpan<TCloaked>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TCloakedStruct>(ReadOnlySpan<TCloakedStruct?> value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType = typeof(ReadOnlySpan<TCloakedStruct?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TCloaked, TRevealBase>(IReadOnlyList<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TCloaked?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TCloakedStruct>(IReadOnlyList<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TCloakedStruct?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAllEnumerate<TCloaked, TRevealBase>(IEnumerable<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TCloaked?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAllEnumerate<TCloakedStruct>(IEnumerable<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TCloakedStruct?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAllEnumerate<TCloaked, TRevealBase>(IEnumerator<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TCloaked?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAllEnumerate<TCloakedStruct>(IEnumerator<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TCloakedStruct?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TBearer>(TBearer[]? value, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(TBearer[]);
        if (stb.HasSkipBody(actualType, "", formatFlags)) return stb.WasSkipped(actualType
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TBearerStruct>(TBearerStruct?[]? value, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(TBearerStruct[]);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TBearer>(Span<TBearer> value, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        var actualType = typeof(Span<TBearer>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TBearerStruct>(Span<TBearerStruct?> value, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = typeof(Span<TBearerStruct?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TBearer>(ReadOnlySpan<TBearer> value, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        var actualType = typeof(ReadOnlySpan<TBearer>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TBearerStruct>(ReadOnlySpan<TBearerStruct?> value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = typeof(ReadOnlySpan<TBearerStruct?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TBearer>(IReadOnlyList<TBearer>? value, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TBearer?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAll<TBearerStruct>(IReadOnlyList<TBearerStruct?>? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TBearerStruct?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAllEnumerate<TBearer>(IEnumerable<TBearer>? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TBearer?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAllEnumerate<TBearerStruct>(IEnumerable<TBearerStruct?>? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TBearerStruct>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAllEnumerate<TBearer>(IEnumerator<TBearer>? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TBearer?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold RevealAllEnumerate<TBearerStruct>(IEnumerator<TBearerStruct?>? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TBearerStruct?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(string?[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(string?[]);
        if (stb.HasSkipBody(actualType, "", formatFlags)) return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(Span<string> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<string>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllNullable(Span<string?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<string?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(ReadOnlySpan<string> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<string>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllNullable(ReadOnlySpan<string?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<string?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(IReadOnlyList<string?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<string?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate(IEnumerable<string?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<string?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate(IEnumerator<string?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<string?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllCharSeq<TCharSeq>(TCharSeq?[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        var actualType = value?.GetType() ?? typeof(TCharSeq?[]);
        if (stb.HasSkipBody(actualType, "", formatFlags)) return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllCharSeq<TCharSeq>(Span<TCharSeq> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?
    {
        var actualType = typeof(Span<TCharSeq>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllCharSeq<TCharSeq>(ReadOnlySpan<TCharSeq> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?
    {
        var actualType = typeof(ReadOnlySpan<TCharSeq>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllCharSeq<TCharSeq>(IReadOnlyList<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TCharSeq?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllCharSeqEnumerate<TCharSeq>(IEnumerable<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TCharSeq?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllCharSeqEnumerate<TCharSeq>(IEnumerator<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TCharSeq?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(StringBuilder?[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<StringBuilder?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(Span<StringBuilder> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<StringBuilder>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllNullable(Span<StringBuilder?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<StringBuilder?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(ReadOnlySpan<StringBuilder> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<StringBuilder>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllNullable(ReadOnlySpan<StringBuilder?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<StringBuilder?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAll(IReadOnlyList<StringBuilder?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<StringBuilder?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate(IEnumerable<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<StringBuilder?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllEnumerate(IEnumerator<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<StringBuilder?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllMatch<TAny>(TAny[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(TAny[]);
        if (stb.HasSkipBody(actualType, "", formatFlags)) return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllMatch<TAny>(Span<TAny> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<TAny>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllMatch<TAny>(ReadOnlySpan<TAny> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<TAny>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllMatch<TAny>(IReadOnlyList<TAny>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TAny>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllMatchEnumerate<TAny>(IEnumerable<TAny>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TAny>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    public TOCMold AddAllMatchEnumerate<TAny>(IEnumerator<TAny>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TAny>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TOCMold AddAllObject(object?[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(object?[]);
        if (stb.HasSkipBody(actualType, "", formatFlags)) return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TOCMold AddAllObject(Span<object> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<object>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TOCMold AddAllObjectNullable(Span<object?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<object?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TOCMold AddAllObject(ReadOnlySpan<object> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<object>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TOCMold AddAllObjectNullable(ReadOnlySpan<object?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<object?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TOCMold AddAllObject(IReadOnlyList<object?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<object?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TOCMold AddAllObjectEnumerate(IEnumerable<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<object?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TOCMold AddAllObjectEnumerate(IEnumerator<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<object?>);
        if (stb.HasSkipBody(actualType, "", formatFlags))
            return stb.WasSkipped(actualType, "", formatFlags);
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
        return stb.SupportsMultipleFields ? stb.AddGoToNext() : stb.StyleTypeBuilder;
    }
}
