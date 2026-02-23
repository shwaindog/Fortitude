// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;
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
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(bool);
        var any         = false;

        TrackedInstanceMold? valueMold = null;

        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];
                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Length, value?.Length
                                      , formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAll(bool?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(bool?[]);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(bool?);
        var any         = false;

        TrackedInstanceMold? valueMold = null;

        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];
                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Length, value?.Length
                                      , formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAll(Span<bool> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<bool>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(bool);
        var any         = false;

        TrackedInstanceMold? valueMold = null;

        if (value != null && valueMold?.ShouldSuppressBody != true)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];
                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAll(ReadOnlySpan<bool> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<bool>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(bool);
        var any         = false;

        TrackedInstanceMold? valueMold = null;

        if (value != null && valueMold?.ShouldSuppressBody != true)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];
                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAll(Span<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<bool?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(bool?);
        var any         = false;

        TrackedInstanceMold? valueMold = null;

        if (value != null && valueMold?.ShouldSuppressBody != true)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];
                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAll(ReadOnlySpan<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<bool?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(bool?);
        var any         = false;

        TrackedInstanceMold? valueMold = null;

        if (value != null && valueMold?.ShouldSuppressBody != true)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];
                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAll(IReadOnlyList<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<bool>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(bool);
        var any         = false;

        TrackedInstanceMold? valueMold = null;

        if (value != null && valueMold?.ShouldSuppressBody != true)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];
                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAll(IReadOnlyList<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<bool?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(bool?);
        var any         = false;

        TrackedInstanceMold? valueMold = null;

        if (value != null && valueMold?.ShouldSuppressBody != true)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];
                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllEnumerate(IEnumerable<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<bool>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var  elementType     = typeof(bool);
        var  any             = false;
        var  itemCount       = 0;
        int? collectionItems = null;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            foreach (var item in value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                mws.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | AsCollection);
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, itemCount, collectionItems, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllEnumerate(IEnumerable<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<bool?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var  elementType     = typeof(bool);
        var  any             = false;
        var  itemCount       = 0;
        int? collectionItems = null;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            foreach (var item in value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                mws.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | AsCollection);
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, itemCount, collectionItems, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllEnumerate(IEnumerator<bool>? value, bool? hasValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<bool>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var  elementType     = typeof(bool);
        var  any             = false;
        var  itemCount       = 0;
        int? collectionItems = value == null ? null : 0;
        hasValue        ??= value?.MoveNext() ?? false;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            formatString ??= "";
            while (hasValue.Value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value!.Current;
                mws.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | AsCollection);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, itemCount, collectionItems, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllEnumerate(IEnumerator<bool?>? value, bool? hasValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<bool?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var  elementType     = typeof(bool);
        var  any             = false;
        var  itemCount       = 0;
        int? collectionItems = value == null ? null : 0;
        hasValue        ??= value?.MoveNext() ?? false;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            formatString ??= "";
            while (hasValue.Value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value!.Current;

                mws.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | AsCollection);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, itemCount, collectionItems, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAll<TFmt>(TFmt[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        var actualType = value?.GetType() ?? typeof(TFmt?[]);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TFmt);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, any ? value?.Length : null
                                      , value?.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAll<TFmtStruct>(TFmtStruct?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(TFmtStruct?[]);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TFmtStruct?);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, any ? value?.Length : null
                                      , value?.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAll<TFmt>(Span<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        var actualType = typeof(Span<TFmt>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TFmt);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAll<TFmt>(ReadOnlySpan<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        var actualType = typeof(ReadOnlySpan<TFmt>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TFmt);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAll<TFmtStruct>(Span<TFmtStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = typeof(Span<TFmtStruct?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TFmtStruct?);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAll<TFmtStruct>(ReadOnlySpan<TFmtStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = typeof(ReadOnlySpan<TFmtStruct?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TFmtStruct?);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAll<TFmt>(IReadOnlyList<TFmt>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TFmt?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TFmt);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAll<TFmtStruct>(IReadOnlyList<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TFmtStruct?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TFmtStruct?);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItem(item, i, formatString, formatFlags | AsCollection);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllEnumerate<TFmt>(IEnumerable<TFmt>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TFmt?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var  elementType     = typeof(TFmt);
        var  any             = false;
        var  itemCount       = 0;
        int? collectionItems = null;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            foreach (var item in value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                mws.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | AsCollection);
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllEnumerate<TFmtStruct>(IEnumerable<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TFmtStruct?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var  elementType     = typeof(TFmtStruct?);
        var  any             = false;
        var  itemCount       = 0;
        int? collectionItems = null;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            foreach (var item in value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                mws.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | AsCollection);
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllEnumerate<TFmt>(IEnumerator<TFmt>? value, bool? hasValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TFmt?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var  elementType     = typeof(TFmt);
        var  any             = false;
        var  itemCount       = 0;
        int? collectionItems = value == null ? null : 0;
        hasValue        ??= value?.MoveNext() ?? false;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            formatString ??= "";
            while (hasValue.Value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value!.Current;
                mws.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | AsCollection);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllEnumerate<TFmtStruct>(IEnumerator<TFmtStruct?>? value, bool? hasValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TFmtStruct?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var  elementType     = typeof(TFmtStruct?);
        var  any             = false;
        var  itemCount       = 0;
        int? collectionItems = value == null ? null : 0;
        hasValue        ??= value?.MoveNext() ?? false;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            formatString ??= "";
            while (hasValue.Value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value!.Current;
                mws.AppendFormattedCollectionItem(item, itemCount, formatString, formatFlags | AsCollection);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealAll<TCloaked, TRevealBase>(TCloaked?[]? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TCloaked?[]);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCloaked);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealCloakedBearerOrNull(item, palantírReveal, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, any ? value?.Length : null, value?.Length, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }


    public TOCMold RevealAll<TCloakedStruct>(TCloakedStruct?[]? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(TCloakedStruct?[]);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCloakedStruct);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealNullableCloakedBearerOrNull(item, palantírReveal, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, any ? value?.Length : null, value?.Length, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealAll<TCloaked, TRevealBase>(Span<TCloaked> value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = typeof(Span<TCloaked>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCloaked);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealCloakedBearerOrNull(item, palantírReveal, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealAll<TCloakedStruct>(Span<TCloakedStruct?> value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType = typeof(Span<TCloakedStruct?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCloakedStruct);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealNullableCloakedBearerOrNull(item, palantírReveal, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealAll<TCloaked, TRevealBase>(ReadOnlySpan<TCloaked> value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = typeof(ReadOnlySpan<TCloaked>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCloaked);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealCloakedBearerOrNull(item, palantírReveal, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealAll<TCloakedStruct>(ReadOnlySpan<TCloakedStruct?> value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType = typeof(ReadOnlySpan<TCloakedStruct?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCloakedStruct);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealNullableCloakedBearerOrNull(item, palantírReveal, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealAll<TCloaked, TRevealBase>(IReadOnlyList<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TCloaked?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCloaked);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealCloakedBearerOrNull(item, palantírReveal, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealAll<TCloakedStruct>(IReadOnlyList<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TCloakedStruct?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCloakedStruct);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealNullableCloakedBearerOrNull(item, palantírReveal, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealAllEnumerate<TCloaked, TRevealBase>(IEnumerable<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TCloaked?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var  elementType     = typeof(TCloaked);
        var  any             = false;
        var  itemCount       = 0;
        int? collectionItems = null;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            foreach (var item in value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                mws.RevealCloakedBearerOrNull(item, palantírReveal, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealAllEnumerate<TCloakedStruct>(IEnumerable<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TCloakedStruct?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var  elementType     = typeof(TCloakedStruct);
        var  any             = false;
        var  itemCount       = 0;
        int? collectionItems = null;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            foreach (var item in value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                mws.RevealNullableCloakedBearerOrNull(item, palantírReveal, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealAllEnumerate<TCloaked, TRevealBase>(IEnumerator<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal
      , bool? hasValue = null, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TCloaked?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var  elementType     = typeof(TCloaked);
        var  any             = false;
        var  itemCount       = 0;
        int? collectionItems = value == null ? null : 0;
        hasValue        ??= value?.MoveNext() ?? false;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            while (hasValue.Value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value!.Current;
                mws.RevealCloakedBearerOrNull(item, palantírReveal, formatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealAllEnumerate<TCloakedStruct>(IEnumerator<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal
      , bool? hasValue = null, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TCloakedStruct?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var  elementType     = typeof(TCloakedStruct);
        var  any             = false;
        var  itemCount       = 0;
        int? collectionItems = value == null ? null : 0;
        hasValue        ??= value?.MoveNext() ?? false;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            while (hasValue.Value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value!.Current;
                mws.RevealNullableCloakedBearerOrNull(item, palantírReveal, formatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealAll<TBearer>(TBearer[]? value, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(TBearer[]);
        if (mws.HasSkipBody(actualType, "", formatFlags))
            return mws.WasSkipped(actualType
                                , "", formatFlags);
        var elementType = typeof(TBearer);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealStringBearerOrNull(item, formatString ?? "", formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Length, value?.Length, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealAll<TBearerStruct>(TBearerStruct?[]? value, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(TBearerStruct[]);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TBearerStruct);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealNullableStringBearerOrNull(item, formatString ?? "", formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Length, value?.Length, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealAll<TBearer>(Span<TBearer> value, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        var actualType = typeof(Span<TBearer>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TBearer);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealStringBearerOrNull(item, formatString ?? "", formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealAll<TBearerStruct>(Span<TBearerStruct?> value, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = typeof(Span<TBearerStruct?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TBearerStruct);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealNullableStringBearerOrNull(item, formatString ?? "", formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealAll<TBearer>(ReadOnlySpan<TBearer> value, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        var actualType = typeof(ReadOnlySpan<TBearer>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TBearer);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealStringBearerOrNull(item, formatString ?? "", formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealAll<TBearerStruct>(ReadOnlySpan<TBearerStruct?> value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = typeof(ReadOnlySpan<TBearerStruct?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TBearerStruct);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealNullableStringBearerOrNull(item, formatString ?? "", formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealAll<TBearer>(IReadOnlyList<TBearer>? value, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TBearer?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TBearer);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealStringBearerOrNull(item, formatString ?? "", formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealAll<TBearerStruct>(IReadOnlyList<TBearerStruct?>? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TBearerStruct?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TBearerStruct);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealNullableStringBearerOrNull(item, formatString ?? "", formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealAllEnumerate<TBearer>(IEnumerable<TBearer>? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TBearer?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var  elementType     = typeof(TBearer);
        var  any             = false;
        var  itemCount       = 0;
        int? collectionItems = null;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            foreach (var item in value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                mws.RevealStringBearerOrNull(item, formatString ?? "", formatFlags);
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealAllEnumerate<TBearerStruct>(IEnumerable<TBearerStruct?>? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TBearerStruct>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var  elementType     = typeof(TBearerStruct);
        var  any             = false;
        var  itemCount       = 0;
        int? collectionItems = null;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            foreach (var item in value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                mws.RevealNullableStringBearerOrNull(item, formatString ?? "", formatFlags);
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealAllEnumerate<TBearer>(IEnumerator<TBearer>? value, bool? hasValue = null, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TBearer?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var  elementType     = typeof(TBearer);
        var  any             = false;
        hasValue        ??= value?.MoveNext() ?? false;
        var  itemCount       = 0;
        int? collectionItems = value == null ? null : 0;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            while (hasValue.Value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value!.Current;

                mws.RevealStringBearerOrNull(item, formatString ?? "", formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealAllEnumerate<TBearerStruct>(IEnumerator<TBearerStruct?>? value, bool? hasValue = null, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TBearerStruct?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var  elementType     = typeof(TBearerStruct);
        var  any             = false;
        hasValue        ??= value?.MoveNext() ?? false;
        var  itemCount       = 0;
        int? collectionItems = value == null ? null : 0;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            while (hasValue.Value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value!.Current;

                mws.RevealNullableStringBearerOrNull(item, formatString ?? "", formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAll(string?[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(string?[]);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(string);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, any ? value?.Length : null, value?.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAll(Span<string> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<string>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(string);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllNullable(Span<string?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<string?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(string);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAll(ReadOnlySpan<string> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<string>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(string);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllNullable(ReadOnlySpan<string?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<string?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(string);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAll(IReadOnlyList<string?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<string?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(string);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllEnumerate(IEnumerable<string?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<string?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var  elementType     = typeof(string);
        var  any             = false;
        var  itemCount       = 0;
        int? collectionItems = null;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            foreach (var item in value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                mws.AppendFormattedCollectionItemOrNull(item, itemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllEnumerate(IEnumerator<string?>? value, bool? hasValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<string?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var  elementType     = typeof(string);
        var  any             = false;
        var  itemCount       = 0;
        int? collectionItems = value == null ? null : 0;
        hasValue        ??= value?.MoveNext() ?? false;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            formatString ??= "";
            while (hasValue.Value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value!.Current;

                mws.AppendFormattedCollectionItemOrNull(item, itemCount, formatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllCharSeq<TCharSeq>(TCharSeq[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?
    {
        var actualType = value?.GetType() ?? typeof(TCharSeq[]);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCharSeq);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Length, value?.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllCharSeq<TCharSeq>(Span<TCharSeq> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?
    {
        var actualType = typeof(Span<TCharSeq>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCharSeq);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllCharSeq<TCharSeq>(ReadOnlySpan<TCharSeq> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?
    {
        var actualType = typeof(ReadOnlySpan<TCharSeq>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCharSeq);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllCharSeq<TCharSeq>(IReadOnlyList<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TCharSeq?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TCharSeq);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllCharSeqEnumerate<TCharSeq>(IEnumerable<TCharSeq>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TCharSeq>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var  elementType     = typeof(TCharSeq);
        var  any             = false;
        var  itemCount       = 0;
        int? collectionItems = null;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            foreach (var item in value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                mws.AppendFormattedCollectionItemOrNull(item, itemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, any ? itemCount : null, collectionItems, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllCharSeqEnumerate<TCharSeq>(IEnumerator<TCharSeq>? value, bool? hasValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TCharSeq?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var  elementType     = typeof(TCharSeq);
        var  any             = false;
        hasValue        ??= value?.MoveNext() ?? false;
        var  itemCount       = 0;
        int? collectionItems = value == null ? null : 0;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            formatString ??= "";
            while (hasValue.Value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value!.Current;

                any = true;
                mws.AppendFormattedCollectionItemOrNull(item, itemCount, formatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAll(StringBuilder?[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(StringBuilder[]);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(StringBuilder);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Length, value?.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAll(Span<StringBuilder> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<StringBuilder>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(StringBuilder);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllNullable(Span<StringBuilder?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<StringBuilder?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(StringBuilder);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAll(ReadOnlySpan<StringBuilder> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<StringBuilder>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(StringBuilder);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllNullable(ReadOnlySpan<StringBuilder?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<StringBuilder?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(StringBuilder);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAll(IReadOnlyList<StringBuilder?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<StringBuilder?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(StringBuilder);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllEnumerate(IEnumerable<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<StringBuilder?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var  elementType     = typeof(StringBuilder);
        var  any             = false;
        var  itemCount       = 0;
        int? collectionItems = null;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            foreach (var item in value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                mws.AppendFormattedCollectionItemOrNull(item, itemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllEnumerate(IEnumerator<StringBuilder?>? value, bool? hasValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<StringBuilder?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var  elementType     = typeof(StringBuilder);
        var  any             = false;
        hasValue        ??= value?.MoveNext() ?? false;
        var  itemCount       = 0;
        int? collectionItems = value == null ? null : 0;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            formatString ??= "";
            while (hasValue.Value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value!.Current;

                mws.AppendFormattedCollectionItemOrNull(item, itemCount, formatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllMatch<TAny>(TAny[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(TAny[]);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TAny);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Length, value?.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllMatch<TAny>(Span<TAny> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<TAny>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TAny);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllMatch<TAny>(ReadOnlySpan<TAny> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<TAny>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TAny);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllMatch<TAny>(IReadOnlyList<TAny>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TAny>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(TAny);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllMatchEnumerate<TAny>(IEnumerable<TAny>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TAny>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var  elementType     = typeof(TAny);
        var  any             = false;
        var  itemCount       = 0;
        int? collectionItems = null;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            foreach (var item in value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                mws.AppendFormattedCollectionItemMatchOrNull(item, itemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllMatchEnumerate<TAny>(IEnumerator<TAny>? value, bool? hasValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TAny>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var  elementType     = typeof(TAny);
        var  any             = false;
        var  itemCount       = 0;
        hasValue        ??= value?.MoveNext() ?? false;
        int? collectionItems = value == null ? null : 0;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            formatString ??= "";
            while (hasValue.Value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value!.Current;

                mws.AppendFormattedCollectionItemMatchOrNull(item, itemCount, formatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    [CallsObjectToString]
    public TOCMold AddAllObject(object?[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(object?[]);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(object);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Length, value?.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    [CallsObjectToString]
    public TOCMold AddAllObject(Span<object> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<object>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(object);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    [CallsObjectToString]
    public TOCMold AddAllObjectNullable(Span<object?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<object?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(object);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    [CallsObjectToString]
    public TOCMold AddAllObject(ReadOnlySpan<object> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<object>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(object);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    [CallsObjectToString]
    public TOCMold AddAllObjectNullable(ReadOnlySpan<object?> value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<object?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(object);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Length; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    [CallsObjectToString]
    public TOCMold AddAllObject(IReadOnlyList<object?>? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<object?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var elementType = typeof(object);
        var any         = false;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            for (var i = 0; i < value.Count; i++)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemMatchOrNull(item, i, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, i);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    [CallsObjectToString]
    public TOCMold AddAllObjectEnumerate(IEnumerable<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<object?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var  elementType     = typeof(object);
        var  any             = false;
        var  itemCount       = 0;
        int? collectionItems = null;

        TrackedInstanceMold? valueMold = null;
        if (value != null)
        {
            formatString ??= "";
            foreach (var item in value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                mws.AppendFormattedCollectionItemMatchOrNull(item, itemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    [CallsObjectToString]
    public TOCMold AddAllObjectEnumerate(IEnumerator<object?>? value, bool? hasValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<object?>);
        if (mws.HasSkipBody(actualType, "", formatFlags)) return mws.WasSkipped(actualType, "", formatFlags);
        var  elementType     = typeof(object);
        var  any             = false;
        var  itemCount       = 0;
        int? collectionItems = value == null ? null : 0;
        hasValue        ??= value?.MoveNext() ?? false;

        TrackedInstanceMold? valueMold = null;
        if (hasValue.Value)
        {
            formatString ??= "";
            while (hasValue.Value)
            {
                if (!any)
                {
                    valueMold = mws.ConditionalCollectionPrefix(value, elementType, true, formatFlags);
                    any       = true;
                    if (valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value!.Current;

                mws.AppendFormattedCollectionItemMatchOrNull(item, itemCount, formatString, formatFlags);
                hasValue = value.MoveNext();
                mws.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            collectionItems = itemCount;
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value != null ? itemCount : null, collectionItems, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }
}
