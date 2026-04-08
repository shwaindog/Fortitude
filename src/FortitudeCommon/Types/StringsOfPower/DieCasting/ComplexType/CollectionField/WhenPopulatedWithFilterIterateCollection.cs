// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.CollectionField;

#pragma warning disable CS0618 // Type or member is obsolete
public partial class SelectTypeCollectionField<TMold> where TMold : TypeMolder
{
    
    public TMold WhenPopulatedWithFilterIterateBool<TEnumtr>(string fieldName, TEnumtr? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<bool> =>
        value == null ? stb.Mold : WhenPopulatedWithFilterIterateBool(fieldName, value.Value, filterPredicate, formatString, formatFlags);
    
    public TMold WhenPopulatedWithFilterIterateBool<TEnumtr>(string fieldName, TEnumtr? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<bool>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddFilteredIterateBool(value, filterPredicate, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TMold WhenPopulatedWithFilterIterateNullableBool<TEnumtr>(
        string fieldName,
        TEnumtr? value
      , OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<bool?> =>
        value == null ? stb.Mold : WhenPopulatedWithFilterIterateNullableBool(fieldName, value.Value, filterPredicate, formatString, formatFlags);

    public TMold WhenPopulatedWithFilterIterateNullableBool<TEnumtr>(
        string fieldName,
        TEnumtr? value
      , OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<bool?>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddFilteredIterateNullableBool(value, filterPredicate, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }
    
    public TMold WhenPopulatedWithFilterIterate<TEnumtr, TFmtBase>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator =>
        value == null ? stb.Mold : WhenPopulatedWithFilterIterate(fieldName, value.Value, filterPredicate, formatString, formatFlags);

    public TMold WhenPopulatedWithFilterIterate<TEnumtr, TFmtBase>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddFilteredIterate(value, filterPredicate, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }
    
    public TMold WhenPopulatedWithFilterIterate<TEnumtr, TFmt, TFmtBase>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TFmt>
        where TFmt : ISpanFormattable?, TFmtBase? =>
        value == null ? stb.Mold : WhenPopulatedWithFilterIterate<TEnumtr, TFmt, TFmtBase>
            (fieldName, value.Value, filterPredicate, formatString, formatFlags);
    
    public TMold WhenPopulatedWithFilterIterate<TEnumtr, TFmt, TFmtBase>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TFmt>?
        where TFmt : ISpanFormattable?, TFmtBase?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddFilteredIterate<TEnumtr, TFmt, TFmtBase>(value, filterPredicate, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TMold WhenPopulatedWithFilterIterateNullable<TEnumtr, TFmtStruct>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TFmtStruct?>
        where TFmtStruct : struct, ISpanFormattable =>
        value == null ? stb.Mold : WhenPopulatedWithFilterIterateNullable(fieldName, value.Value, filterPredicate, formatString, formatFlags);

    public TMold WhenPopulatedWithFilterIterateNullable<TEnumtr, TFmtStruct>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TFmtStruct?>?
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddFilteredIterateNullable(value, filterPredicate, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TMold WhenPopulatedWithFilterRevealIterate<TEnumtr, TFilterBase, TRevealBase>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TRevealBase : notnull =>
        value == null 
            ? stb.Mold 
            : WhenPopulatedWithFilterRevealIterate(fieldName, value.Value, filterPredicate, palantírReveal, formatString, formatFlags);

    public TMold WhenPopulatedWithFilterRevealIterate<TEnumtr, TFilterBase, TRevealBase>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.RevealFilteredIterate(value, filterPredicate, palantírReveal, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TMold WhenPopulatedWithFilterRevealIterate<TEnumtr, TCloaked, TFilterBase, TRevealBase>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TCloaked>
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull =>
        value == null 
            ? stb.Mold 
            : WhenPopulatedWithFilterRevealIterate<TEnumtr, TCloaked, TFilterBase, TRevealBase>
                (fieldName, value.Value, filterPredicate, palantírReveal, formatString, formatFlags);

    public TMold WhenPopulatedWithFilterRevealIterate<TEnumtr, TCloaked, TFilterBase, TRevealBase>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TCloaked>?
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.RevealFilteredIterate<TEnumtr, TCloaked, TFilterBase, TRevealBase>(value, filterPredicate, palantírReveal, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TMold WhenPopulatedWithFilterRevealIterateNullable<TEnumtr, TCloakedStruct>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TCloakedStruct?>
        where TCloakedStruct : struct =>
        value == null 
            ? stb.Mold 
            : WhenPopulatedWithFilterRevealIterateNullable(fieldName, value.Value, filterPredicate, palantírReveal, formatString, formatFlags);

    public TMold WhenPopulatedWithFilterRevealIterateNullable<TEnumtr, TCloakedStruct>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TCloakedStruct?>?
        where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.RevealFilteredIterateNullable(value, filterPredicate, palantírReveal, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TMold WhenPopulatedWithFilterRevealIterate<TEnumtr, TBearerBase>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator =>
        value == null 
            ? stb.Mold 
            : WhenPopulatedWithFilterRevealIterate(fieldName, value.Value, filterPredicate, formatString, formatFlags);

    public TMold WhenPopulatedWithFilterRevealIterate<TEnumtr, TBearerBase>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.RevealFilteredIterate(value, filterPredicate, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TMold WhenPopulatedWithFilterRevealIterate<TEnumtr, TBearer, TBearerBase>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TBearer>
        where TBearer : IStringBearer?, TBearerBase? =>
        value == null 
            ? stb.Mold 
            : WhenPopulatedWithFilterRevealIterate<TEnumtr, TBearer, TBearerBase>
                (fieldName, value.Value, filterPredicate, formatString, formatFlags);

    public TMold WhenPopulatedWithFilterRevealIterate<TEnumtr, TBearer, TBearerBase>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TBearer>?
        where TBearer : IStringBearer?, TBearerBase?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.RevealFilteredIterate<TEnumtr, TBearer, TBearerBase>(value, filterPredicate, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TMold WhenPopulatedWithFilterRevealIterateNullable<TEnumtr, TBearerStruct>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TBearerStruct?>
        where TBearerStruct : struct, IStringBearer =>
        value == null ? stb.Mold : WhenPopulatedWithFilterRevealIterateNullable(fieldName, value.Value, filterPredicate, formatString, formatFlags);

    public TMold WhenPopulatedWithFilterRevealIterateNullable<TEnumtr, TBearerStruct>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TBearerStruct?>?
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.RevealFilteredIterateNullable(value, filterPredicate, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TMold WhenPopulatedWithFilterIterateString<TEnumtr>(string fieldName, TEnumtr? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<string?> =>
        value == null ? stb.Mold : WhenPopulatedWithFilterIterateString(fieldName, value.Value, filterPredicate, formatString, formatFlags);

    public TMold WhenPopulatedWithFilterIterateString<TEnumtr>(string fieldName, TEnumtr? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<string?>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddFilteredIterateString(value, filterPredicate, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TMold WhenPopulatedWithFilterIterateCharSeq<TEnumtr, TCharSeqBase>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator =>
        value == null ? stb.Mold : WhenPopulatedWithFilterIterateCharSeq(fieldName, value.Value, filterPredicate, formatString, formatFlags);

    public TMold WhenPopulatedWithFilterIterateCharSeq<TEnumtr, TCharSeqBase>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddFilteredIterateCharSeq(value, filterPredicate, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TMold WhenPopulatedWithFilterIterateCharSeq<TEnumtr, TCharSeq, TCharSeqBase>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TCharSeq>
        where TCharSeq : ICharSequence?, TCharSeqBase? =>
        value == null 
            ? stb.Mold 
            : WhenPopulatedWithFilterIterateCharSeq<TEnumtr, TCharSeq, TCharSeqBase>
                (fieldName, value.Value, filterPredicate, formatString, formatFlags);

    public TMold WhenPopulatedWithFilterIterateCharSeq<TEnumtr, TCharSeq, TCharSeqBase>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TCharSeq>?
        where TCharSeq : ICharSequence?, TCharSeqBase?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddFilteredIterateCharSeq<TEnumtr, TCharSeq, TCharSeqBase>(value, filterPredicate, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TMold WhenPopulatedWithFilterIterateStringBuilder<TEnumtr>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<StringBuilder?> =>
        value == null ? stb.Mold : WhenPopulatedWithFilterIterateStringBuilder(fieldName, value.Value, filterPredicate, formatString, formatFlags);

    public TMold WhenPopulatedWithFilterIterateStringBuilder<TEnumtr>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<StringBuilder?>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddFilteredIterateStringBuilder(value, filterPredicate, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }
    
    public TMold WhenPopulatedWithFilterIterateMatch<TEnumtr, TAnyBase>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator =>
        value == null ? stb.Mold : WhenPopulatedWithFilterIterateMatch(fieldName, value.Value, filterPredicate, formatString, formatFlags);
    
    public TMold WhenPopulatedWithFilterIterateMatch<TEnumtr, TAnyBase>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddFilteredIterateMatch(value, filterPredicate, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }
    
    public TMold WhenPopulatedWithFilterIterateMatch<TEnumtr, TAny, TAnyBase>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TAny>
        where TAny : TAnyBase =>
        value == null 
            ? stb.Mold 
            : WhenPopulatedWithFilterIterateMatch<TEnumtr, TAny, TAnyBase>(fieldName, value.Value, filterPredicate, formatString, formatFlags);
    
    public TMold WhenPopulatedWithFilterIterateMatch<TEnumtr, TAny, TAnyBase>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TAny>?
        where TAny : TAnyBase
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddFilteredIterateMatch<TEnumtr, TAny, TAnyBase>(value, filterPredicate, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TMold WhenPopulatedWithFilterIterateObject<TEnumtr>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<object> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<object?> =>
        value == null 
            ? stb.Mold 
            : WhenPopulatedWithFilterIterateObject(fieldName, value.Value, filterPredicate, formatString, formatFlags);

    public TMold WhenPopulatedWithFilterIterateObject<TEnumtr>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<object> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<object?>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddFilteredIterateObject(value, filterPredicate, formatString, formatFlags);
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
