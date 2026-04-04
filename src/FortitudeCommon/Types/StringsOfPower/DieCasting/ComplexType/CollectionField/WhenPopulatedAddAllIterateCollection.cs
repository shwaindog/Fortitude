// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

// ReSharper disable PossibleMultipleEnumeration

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.CollectionField;

public partial class SelectTypeCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenPopulatedAddAllIterateBool<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<bool> =>
        value == null ? stb.Mold : WhenPopulatedAddAllIterateBool(fieldName, value.Value, formatString, formatFlags);

    public TExt WhenPopulatedAddAllIterateBool<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<bool>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddAllIterateBool(value, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedAddAllIterateNullableBool<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<bool?> =>
        value == null ? stb.Mold : WhenPopulatedAddAllIterateNullableBool(fieldName, value.Value, formatString, formatFlags);

    public TExt WhenPopulatedAddAllIterateNullableBool<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<bool?>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddAllIterateNullableBool(value, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedAddAllIterate<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator =>
        value == null ? stb.Mold : WhenPopulatedAddAllIterate(fieldName, value.Value, formatString, formatFlags);

    public TExt WhenPopulatedAddAllIterate<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddAllIterate(value, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedAddAllIterate<TEnumtr, TFmt>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TFmt?>
        where TFmt : ISpanFormattable? =>
        value == null ? stb.Mold : WhenPopulatedAddAllIterate<TEnumtr, TFmt>(fieldName, value.Value, formatString, formatFlags);

    public TExt WhenPopulatedAddAllIterate<TEnumtr, TFmt>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TFmt?>?
        where TFmt : ISpanFormattable?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddAllIterate<TEnumtr, TFmt>(value, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedAddAllIterateNullable<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator =>
        value == null ? stb.Mold : WhenPopulatedAddAllIterateNullable(fieldName, value.Value, formatString, formatFlags);

    public TExt WhenPopulatedAddAllIterateNullable<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddAllIterateNullable(value, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedAddAllIterateNullable<TEnumtr, TFmtStruct>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TFmtStruct?>
        where TFmtStruct : struct, ISpanFormattable =>
        value == null ? stb.Mold : WhenPopulatedAddAllIterateNullable<TEnumtr, TFmtStruct>(fieldName, value.Value, formatString, formatFlags);

    public TExt WhenPopulatedAddAllIterateNullable<TEnumtr, TFmtStruct>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TFmtStruct?>?
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddAllIterateNullable<TEnumtr, TFmtStruct>(value, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedRevealAllIterate<TEnumtr, TRevealBase>(string fieldName, TEnumtr? value
      , PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator 
        where TRevealBase : notnull =>
        value == null ? stb.Mold : WhenPopulatedRevealAllIterate(fieldName, value.Value, palantírReveal, formatString, formatFlags);

    public TExt WhenPopulatedRevealAllIterate<TEnumtr, TRevealBase>(string fieldName, TEnumtr? value
      , PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator? 
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.RevealAllIterate(value, palantírReveal, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedRevealAllIterate<TEnumtr, TCloaked, TRevealBase>(string fieldName, TEnumtr? value
      , PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TCloaked> 
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        value == null 
            ? stb.Mold 
            : WhenPopulatedRevealAllIterate<TEnumtr, TCloaked, TRevealBase>(fieldName, value.Value, palantírReveal, formatString, formatFlags);

    public TExt WhenPopulatedRevealAllIterate<TEnumtr, TCloaked, TRevealBase>(string fieldName, TEnumtr? value
      , PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TCloaked>? 
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.RevealAllIterate<TEnumtr, TCloaked, TRevealBase>(value, palantírReveal, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedRevealAllIterateNullable<TEnumtr, TCloakedStruct>(string fieldName, TEnumtr? value
      , PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TCloakedStruct?> 
        where TCloakedStruct : struct =>
        value == null ? stb.Mold : WhenPopulatedRevealAllIterateNullable(fieldName, value.Value, palantírReveal, formatString, formatFlags);

    public TExt WhenPopulatedRevealAllIterateNullable<TEnumtr, TCloakedStruct>(string fieldName, TEnumtr? value
      , PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TCloakedStruct?>? 
        where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.RevealAllIterateNullable(value, palantírReveal, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedRevealAllIterate<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator =>
        value == null ? stb.Mold : WhenPopulatedRevealAllIterate(fieldName, value.Value, formatString, formatFlags);

    public TExt WhenPopulatedRevealAllIterate<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator? 
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.RevealAllIterate(value, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedRevealAllIterate<TEnumtr, TBearer>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TBearer> 
        where TBearer : IStringBearer? =>
        value == null ? stb.Mold : WhenPopulatedRevealAllIterate(fieldName, value.Value, formatString, formatFlags);

    public TExt WhenPopulatedRevealAllIterate<TEnumtr, TBearer>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TBearer>? 
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.RevealAllIterate<TEnumtr, TBearer>(value, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedRevealAllIterateNullable<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator =>
        value == null ? stb.Mold : WhenPopulatedRevealAllIterateNullable(fieldName, value.Value, formatString, formatFlags);

    public TExt WhenPopulatedRevealAllIterateNullable<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator? 
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.RevealAllIterateNullable(value, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedRevealAllIterateNullable<TEnumtr, TBearerStruct>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TBearerStruct?> 
        where TBearerStruct : struct, IStringBearer =>
        value == null 
            ? stb.Mold 
            : WhenPopulatedRevealAllIterateNullable<TEnumtr, TBearerStruct>(fieldName, value.Value, formatString, formatFlags);

    public TExt WhenPopulatedRevealAllIterateNullable<TEnumtr, TBearerStruct>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TBearerStruct?>? 
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.RevealAllIterateNullable<TEnumtr, TBearerStruct>(value, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }


    public TExt WhenPopulatedAddAllIterateString<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<string?> =>
        value == null ? stb.Mold : WhenPopulatedAddAllIterateString(fieldName, value.Value, formatString, formatFlags);


    public TExt WhenPopulatedAddAllIterateString<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<string?>? 
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddAllIterateString(value, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedAddAllIterateCharSeq<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator =>
        value == null ? stb.Mold : WhenPopulatedAddAllIterateCharSeq(fieldName, value.Value, formatString, formatFlags);

    public TExt WhenPopulatedAddAllIterateCharSeq<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator? 
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddAllIterateCharSeq(value, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedAddAllIterateCharSeq<TEnumtr, TCharSeq>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TCharSeq> 
        where TCharSeq : ICharSequence? =>
        value == null ? stb.Mold : WhenPopulatedAddAllIterateCharSeq<TEnumtr, TCharSeq>(fieldName, value.Value, formatString, formatFlags);

    public TExt WhenPopulatedAddAllIterateCharSeq<TEnumtr, TCharSeq>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TCharSeq>? 
        where TCharSeq : ICharSequence?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddAllIterateCharSeq<TEnumtr, TCharSeq>(value, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedAddAllIterateStringBuilder<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<StringBuilder?> =>
        value == null ? stb.Mold : WhenPopulatedAddAllIterateStringBuilder(fieldName, value.Value, formatString, formatFlags);

    public TExt WhenPopulatedAddAllIterateStringBuilder<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<StringBuilder?>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddAllIterateStringBuilder(value, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedAddAllIterateMatch<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator =>
        value == null ? stb.Mold : WhenPopulatedAddAllIterateMatch(fieldName, value.Value, formatString, formatFlags);

    public TExt WhenPopulatedAddAllIterateMatch<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddAllIterateMatch(value, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedAddAllIterateMatch<TEnumtr, TAny>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TAny?> =>
        value == null ? stb.Mold : WhenPopulatedAddAllIterateMatch<TEnumtr, TAny>(fieldName, value.Value, formatString, formatFlags);

    public TExt WhenPopulatedAddAllIterateMatch<TEnumtr, TAny>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TAny?>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddAllIterateMatch<TEnumtr, TAny>(value, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    [CallsObjectToString]
    public TExt WhenPopulatedAddAllIterateObject<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<object?>  =>
        value == null ? stb.Mold : WhenPopulatedAddAllIterateObject(fieldName, value.Value, formatString, formatFlags);

    [CallsObjectToString]
    public TExt WhenPopulatedAddAllIterateObject<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<object?>? 
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddAllIterateObject(value, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }
}
