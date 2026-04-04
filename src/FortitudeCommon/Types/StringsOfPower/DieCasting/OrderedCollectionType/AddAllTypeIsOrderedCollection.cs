// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.WrittenAsFlags;

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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];
                mws.AppendFormattedCollectionItem(item, mws.ItemCount, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];
                mws.AppendFormattedCollectionItem(item, mws.ItemCount, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];
                mws.AppendFormattedCollectionItem(item, mws.ItemCount, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];
                mws.AppendFormattedCollectionItem(item, mws.ItemCount, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];
                mws.AppendFormattedCollectionItem(item, mws.ItemCount, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];
                mws.AppendFormattedCollectionItem(item, mws.ItemCount, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];
                mws.AppendFormattedCollectionItem(item, mws.ItemCount, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];
                mws.AppendFormattedCollectionItem(item, mws.ItemCount, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllEnumerateBool<TEnumbl>(TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<bool>
    {
        mws.AddAllEnumerateBool(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddAllEnumerateBool<TEnumbl>(TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<bool>?
    {
        mws.AddAllEnumerateBool(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddAllEnumerateNullableBool<TEnumbl>(TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<bool?>
    {
        mws.AddAllEnumerateNullableBool(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddAllEnumerateNullableBool<TEnumbl>(TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<bool?>?
    {
        mws.AddAllEnumerateNullableBool(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddAllIterateBool<TEnumtr>(TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<bool>
    {
        mws.AddAllIterateBool(value, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    public TOCMold AddAllIterateBool<TEnumtr>(TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : IEnumerator<bool>?
    {
        mws.AddAllIterateBool(value, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    public TOCMold AddAllIterateNullableBool<TEnumtr>(TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<bool?>
    {
        mws.AddAllIterateNullableBool(value, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    public TOCMold AddAllIterateNullableBool<TEnumtr>(TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : IEnumerator<bool?>?
    {
        mws.AddAllIterateNullableBool(value, formatString, formatFlags, hasValue);
        return mws.Mold;
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItem(item, mws.ItemCount, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItem(item, mws.ItemCount, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItem(item, mws.ItemCount, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItem(item, mws.ItemCount, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItem(item, mws.ItemCount, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItem(item, mws.ItemCount, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItem(item, mws.ItemCount, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItem(item, mws.ItemCount, formatString, formatFlags | FormatFlags.AsCollection);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllEnumerate<TEnumbl>(TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : struct, IEnumerable
    {
        mws.AddAllEnumerate(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddAllEnumerate<TEnumbl>(TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : IEnumerable?
    {
        mws.AddAllEnumerate(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddAllEnumerate<TEnumbl, TFmt>(TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : struct, IEnumerable<TFmt>
        where TFmt : ISpanFormattable?
    {
        mws.AddAllEnumerate<TEnumbl, TFmt>(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddAllEnumerate<TEnumbl, TFmt>(TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : IEnumerable<TFmt>?
        where TFmt : ISpanFormattable?
    {
        mws.AddAllEnumerate<TEnumbl, TFmt>(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddAllEnumerateNullable<TEnumbl>(TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : struct, IEnumerable
    {
        mws.AddAllEnumerate(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddAllEnumerateNullable<TEnumbl>(TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : IEnumerable?
    {
        mws.AddAllEnumerate(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddAllEnumerateNullable<TEnumbl, TFmtStruct>(TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TFmtStruct?>
        where TFmtStruct : struct, ISpanFormattable
    {
        mws.AddAllEnumerateNullable<TEnumbl, TFmtStruct>(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddAllEnumerateNullable<TEnumbl, TFmtStruct>(TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : IEnumerable<TFmtStruct?>?
        where TFmtStruct : struct, ISpanFormattable
    {
        mws.AddAllEnumerateNullable<TEnumbl, TFmtStruct>(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddAllIterate<TEnumtr>(TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null) 
        where TEnumtr : struct, IEnumerator
    {
        mws.AddAllIterate(value, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    public TOCMold AddAllIterate<TEnumtr>(TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null) 
        where TEnumtr : IEnumerator?
    {
        mws.AddAllIterate(value, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    public TOCMold AddAllIterate<TEnumtr, TFmt>(TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null) 
        where TEnumtr : struct, IEnumerator<TFmt?>
        where TFmt : ISpanFormattable?
    {
        mws.AddAllIterate<TEnumtr, TFmt>(value, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    public TOCMold AddAllIterate<TEnumtr, TFmt>(TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null) 
        where TEnumtr : IEnumerator<TFmt?>?
        where TFmt : ISpanFormattable?
    {
        mws.AddAllIterate<TEnumtr, TFmt>(value, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    public TOCMold AddAllIterateNullable<TEnumtr>(TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : struct, IEnumerator
    {
        mws.AddAllIterateNullable(value, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    public TOCMold AddAllIterateNullable<TEnumtr>(TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : IEnumerator?
    {
        mws.AddAllIterateNullable(value, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    public TOCMold AddAllIterateNullable<TEnumtr, TFmtStruct>(TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<TFmtStruct?>
        where TFmtStruct : struct, ISpanFormattable
    {
        mws.AddAllIterateNullable<TEnumtr, TFmtStruct>(value, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    public TOCMold AddAllIterateNullable<TEnumtr, TFmtStruct>(TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : IEnumerator<TFmtStruct?>?
        where TFmtStruct : struct, ISpanFormattable
    {
        mws.AddAllIterateNullable<TEnumtr, TFmtStruct>(value, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    public TOCMold RevealAll<TCloaked, TRevealBase>(TCloaked[]? value, PalantírReveal<TRevealBase> palantírReveal
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealCloakedBearerOrNull(item, palantírReveal, formatString, formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealNullableCloakedBearerOrNull(item, palantírReveal, formatString, formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealCloakedBearerOrNull(item, palantírReveal, formatString, formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealNullableCloakedBearerOrNull(item, palantírReveal, formatString, formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealCloakedBearerOrNull(item, palantírReveal, formatString, formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealNullableCloakedBearerOrNull(item, palantírReveal, formatString, formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealAll<TCloaked, TRevealBase>(IReadOnlyList<TCloaked>? value, PalantírReveal<TRevealBase> palantírReveal
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealCloakedBearerOrNull(item, palantírReveal, formatString, formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealNullableCloakedBearerOrNull(item, palantírReveal, formatString, formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealAllEnumerate<TEnumbl, TRevealBase>(TEnumbl? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TRevealBase : notnull
    {
        mws.RevealAllEnumerate(value, palantírReveal, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold RevealAllEnumerate<TEnumbl, TRevealBase>(TEnumbl? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TRevealBase : notnull
    {
        mws.RevealAllEnumerate(value, palantírReveal, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold RevealAllEnumerate<TEnumbl, TCloaked, TRevealBase>(TEnumbl? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TCloaked>
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        mws.RevealAllEnumerate<TEnumbl, TCloaked, TRevealBase>(value, palantírReveal, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold RevealAllEnumerate<TEnumbl, TCloaked, TRevealBase>(TEnumbl? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCloaked>?
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        mws.RevealAllEnumerate<TEnumbl, TCloaked, TRevealBase>(value, palantírReveal, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold RevealAllEnumerateNullable<TEnumbl, TCloakedStruct>(TEnumbl? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TCloakedStruct?>
        where TCloakedStruct : struct
    {
        mws.RevealAllEnumerateNullable(value, palantírReveal, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold RevealAllEnumerateNullable<TEnumbl, TCloakedStruct>(TEnumbl? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCloakedStruct?>?
        where TCloakedStruct : struct
    {
        mws.RevealAllEnumerateNullable(value, palantírReveal, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold RevealAllIterate<TEnumtr, TRevealBase>(TEnumtr? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : struct, IEnumerator 
        where TRevealBase : notnull
    {
        mws.RevealAllIterate(value, palantírReveal, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    public TOCMold RevealAllIterate<TEnumtr, TRevealBase>(TEnumtr? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : IEnumerator? 
        where TRevealBase : notnull
    {
        mws.RevealAllIterate(value, palantírReveal, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    public TOCMold RevealAllIterate<TEnumtr, TCloaked, TRevealBase>(TEnumtr? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags , bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<TCloaked> 
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        mws.RevealAllIterate<TEnumtr, TCloaked, TRevealBase>(value, palantírReveal, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    public TOCMold RevealAllIterate<TEnumtr, TCloaked, TRevealBase>(TEnumtr? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : IEnumerator<TCloaked>? 
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        mws.RevealAllIterate<TEnumtr, TCloaked, TRevealBase>(value, palantírReveal, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    public TOCMold RevealAllIterateNullable<TEnumtr, TCloakedStruct>(TEnumtr? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<TCloakedStruct?> 
        where TCloakedStruct : struct
    {
        mws.RevealAllIterateNullable(value, palantírReveal, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    public TOCMold RevealAllIterateNullable<TEnumtr, TCloakedStruct>(TEnumtr? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : IEnumerator<TCloakedStruct?>? 
        where TCloakedStruct : struct
    {
        mws.RevealAllIterateNullable(value, palantírReveal, formatString, formatFlags, hasValue);
        return mws.Mold;
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealStringBearerOrNull(item, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealNullableStringBearerOrNull(item, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealStringBearerOrNull(item, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealNullableStringBearerOrNull(item, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealStringBearerOrNull(item, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealNullableStringBearerOrNull(item, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealStringBearerOrNull(item, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.RevealNullableStringBearerOrNull(item, formatString ?? "", formatFlags, AsCollectionItem);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, "", formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold RevealAllEnumerate<TEnumbl>(TEnumbl? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
    {
        mws.RevealAllEnumerate(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold RevealAllEnumerate<TEnumbl>(TEnumbl? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
    {
        mws.RevealAllEnumerate(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold RevealAllEnumerate<TEnumbl, TBearer>(TEnumbl? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TBearer>
        where TBearer : IStringBearer?
    {
        mws.RevealAllEnumerate<TEnumbl, TBearer>(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold RevealAllEnumerate<TEnumbl, TBearer>(TEnumbl? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TBearer>?
        where TBearer : IStringBearer?
    {
        mws.RevealAllEnumerate<TEnumbl, TBearer>(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold RevealAllEnumerateNullable<TEnumbl>(TEnumbl? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
    {
        mws.RevealAllEnumerate(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold RevealAllEnumerateNullable<TEnumbl>(TEnumbl? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
    {
        mws.RevealAllEnumerate(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold RevealAllEnumerateNullable<TEnumbl, TBearerStruct>(TEnumbl? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TBearerStruct?>
        where TBearerStruct : struct, IStringBearer
    {
        mws.RevealAllEnumerateNullable<TEnumbl, TBearerStruct>(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold RevealAllEnumerateNullable<TEnumbl, TBearerStruct>(TEnumbl? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TBearerStruct?>?
        where TBearerStruct : struct, IStringBearer
    {
        mws.RevealAllEnumerateNullable<TEnumbl, TBearerStruct>(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold RevealAllIterate<TEnumtr>(TEnumtr? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : struct, IEnumerator 
    {
        mws.RevealAllIterate(value, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    public TOCMold RevealAllIterate<TEnumtr>(TEnumtr? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : IEnumerator? 
    {
        mws.RevealAllIterate(value, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    public TOCMold RevealAllIterate<TEnumtr, TBearer>(TEnumtr? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<TBearer> 
        where TBearer : IStringBearer?
    {
        mws.RevealAllIterate<TEnumtr, TBearer>(value, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    public TOCMold RevealAllIterate<TEnumtr, TBearer>(TEnumtr? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : IEnumerator<TBearer>? 
        where TBearer : IStringBearer?
    {
        mws.RevealAllIterate<TEnumtr, TBearer>(value, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    public TOCMold RevealAllIterateNullable<TEnumtr>(TEnumtr? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : struct, IEnumerator 
    {
        mws.RevealAllIterate(value, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    public TOCMold RevealAllIterateNullable<TEnumtr>(TEnumtr? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : IEnumerator? 
    {
        mws.RevealAllIterate(value, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    public TOCMold RevealAllIterateNullable<TEnumtr, TBearerStruct>(TEnumtr? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<TBearerStruct?> 
        where TBearerStruct : struct, IStringBearer
    {
        mws.RevealAllIterateNullableStringBearer<TEnumtr, TBearerStruct>(value, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    public TOCMold RevealAllIterateNullable<TEnumtr, TBearerStruct>(TEnumtr? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : IEnumerator<TBearerStruct?>? 
        where TBearerStruct : struct, IStringBearer
    {
        mws.RevealAllIterateNullableStringBearer<TEnumtr, TBearerStruct>(value, formatString, formatFlags, hasValue);
        return mws.Mold;
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllEnumerateString<TEnumbl>(TEnumbl? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<string?>
    {
        mws.AddAllEnumerateString(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddAllEnumerateString<TEnumbl>(TEnumbl? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<string?>?
    {
        mws.AddAllEnumerateString(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddAllIterateString<TEnumtr>(TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<string?>
    {
        mws.AddAllIterateString(value, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    public TOCMold AddAllIterateString<TEnumtr>(TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : IEnumerator<string?>?
    {
        mws.AddAllIterateString(value, formatString, formatFlags, hasValue);
        return mws.Mold;
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(actualType, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value.Length, value.Length, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllCharSeq<TCharSeq>(IReadOnlyList<TCharSeq>? value
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllEnumerateCharSeq<TEnumbl>(TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
    {
        mws.AddAllEnumerateCharSeq(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddAllEnumerateCharSeq<TEnumbl>(TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
    {
        mws.AddAllEnumerateCharSeq(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddAllEnumerateCharSeq<TEnumbl, TCharSeq>(TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TCharSeq>
        where TCharSeq : ICharSequence?
    {
        mws.AddAllEnumerateCharSeq<TEnumbl, TCharSeq>(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddAllEnumerateCharSeq<TEnumbl, TCharSeq>(TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCharSeq>?
        where TCharSeq : ICharSequence?
    {
        mws.AddAllEnumerateCharSeq<TEnumbl, TCharSeq>(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddAllIterateCharSeq<TEnumtr>(TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : struct, IEnumerator
    {
        mws.AddAllIterateCharSeq(value, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    public TOCMold AddAllIterateCharSeq<TEnumtr>(TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : IEnumerator?
    {
        mws.AddAllIterateCharSeq(value, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    public TOCMold AddAllIterateCharSeq<TEnumtr, TCharSeq>(TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<TCharSeq>
        where TCharSeq : ICharSequence?
    {
        mws.AddAllIterateCharSeq<TEnumtr, TCharSeq>(value, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    public TOCMold AddAllIterateCharSeq<TEnumtr, TCharSeq>(TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : IEnumerator<TCharSeq>?
        where TCharSeq : ICharSequence?
    {
        mws.AddAllIterateCharSeq<TEnumtr, TCharSeq>(value, formatString, formatFlags, hasValue);
        return mws.Mold;
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllEnumerateStringBuilder<TEnumbl>(TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<StringBuilder?>
    {
        mws.AddAllEnumerateStringBuilder(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddAllEnumerateStringBuilder<TEnumbl>(TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<StringBuilder?>?
    {
        mws.AddAllEnumerateStringBuilder(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddAllIterateStringBuilder<TEnumtr>(TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<StringBuilder?>
    {
        mws.AddAllIterateStringBuilder(value, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    public TOCMold AddAllIterateStringBuilder<TEnumtr>(TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : IEnumerator<StringBuilder?>?
    {
        mws.AddAllIterateStringBuilder(value, formatString, formatFlags, hasValue);
        return mws.Mold;
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemMatchOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemMatchOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemMatchOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemMatchOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    public TOCMold AddAllEnumerateMatch<TEnumbl>(TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
    {
        mws.AddAllEnumerateMatch(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddAllEnumerateMatch<TEnumbl>(TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
    {
        mws.AddAllEnumerateMatch(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddAllEnumerateMatch<TEnumbl, TAny>(TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TAny>
    {
        mws.AddAllEnumerateMatch<TEnumbl, TAny>(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddAllEnumerateMatch<TEnumbl, TAny>(TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TAny>?
    {
        mws.AddAllEnumerateMatch<TEnumbl, TAny>(value, formatString, formatFlags);
        return mws.Mold;
    }

    public TOCMold AddAllIterateMatch<TEnumtr>(TEnumtr? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : struct, IEnumerator
    {
        mws.AddAllIterateMatch(value, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    public TOCMold AddAllIterateMatch<TEnumtr>(TEnumtr? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : IEnumerator?
    {
        mws.AddAllIterateMatch(value, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    public TOCMold AddAllIterateMatch<TEnumtr, TAny>(TEnumtr? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<TAny?>
    {
        mws.AddAllIterateMatch<TEnumtr, TAny>(value, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    public TOCMold AddAllIterateMatch<TEnumtr, TAny>(TEnumtr? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : IEnumerator<TAny?>?
    {
        mws.AddAllIterateMatch<TEnumtr, TAny>(value, formatString, formatFlags, hasValue);
        return mws.Mold;
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemMatchOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemMatchOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemMatchOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemMatchOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemMatchOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
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
                    if (mws.SkipBody || valueMold?.ShouldSuppressBody == true) { break; }
                }
                var item = value[i];

                mws.AppendFormattedCollectionItemMatchOrNull(item, mws.ItemCount, formatString, formatFlags);
                mws.GoToNextCollectionItemStart(elementType, mws.ItemCount++);
            }
        }
        if (!any && valueMold is not { ShouldSuppressBody: true })
            valueMold = mws.ConditionalCollectionPrefix(value, elementType, false, formatFlags);
        mws.ConditionalCollectionSuffix(valueMold, elementType, value?.Count, value?.Count, formatString, formatFlags);
        return mws.SupportsMultipleFields ? mws.AddGoToNext() : mws.Mold;
    }

    [CallsObjectToString]
    public TOCMold AddAllEnumerateObject<TEnumbl>(TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<object?>
    {
        mws.AddAllEnumerateObject(value, formatString, formatFlags);
        return mws.Mold;
    }

    [CallsObjectToString]
    public TOCMold AddAllEnumerateObject<TEnumbl>(TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<object?>?
    {
        mws.AddAllEnumerateObject(value, formatString, formatFlags);
        return mws.Mold;
    }

    [CallsObjectToString]
    public TOCMold AddAllIterateObject<TEnumtr>(TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : struct, IEnumerator<object?>
    {
        mws.AddAllIterateObject(value, formatString, formatFlags, hasValue);
        return mws.Mold;
    }

    [CallsObjectToString]
    public TOCMold AddAllIterateObject<TEnumtr>(TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags, bool? hasValue = null)
        where TEnumtr : IEnumerator<object?>?
    {
        mws.AddAllIterateObject(value, formatString, formatFlags, hasValue);
        return mws.Mold;
    }
}
