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
public partial class SelectTypeCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenPopulatedWithFilterEnumerateBool<TEnumbl>(string fieldName, TEnumbl? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<bool> =>
        value == null ? stb.Mold : WhenPopulatedWithFilterEnumerateBool(fieldName, value.Value, filterPredicate, formatString, formatFlags);
    
    public TExt WhenPopulatedWithFilterEnumerateBool<TEnumbl>(string fieldName, TEnumbl? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<bool>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddFilteredEnumerateBool(value, filterPredicate, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerateNullableBool<TEnumbl>(string fieldName, TEnumbl? value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<bool?> =>
        value == null ? stb.Mold : WhenPopulatedWithFilterEnumerateNullableBool(fieldName, value.Value, filterPredicate, formatString, formatFlags);

    public TExt WhenPopulatedWithFilterEnumerateNullableBool<TEnumbl>(string fieldName, TEnumbl? value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<bool?>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddFilteredEnumerateNullableBool(value, filterPredicate, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerate<TEnumbl, TFmtBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable =>
        value == null ? stb.Mold : WhenPopulatedWithFilterEnumerate(fieldName, value.Value, filterPredicate, formatString, formatFlags);

    public TExt WhenPopulatedWithFilterEnumerate<TEnumbl, TFmtBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddFilteredEnumerate(value, filterPredicate, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerate<TEnumbl, TFmt, TFmtBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TFmt>
        where TFmt : ISpanFormattable?, TFmtBase? =>
        value == null 
            ? stb.Mold 
            : WhenPopulatedWithFilterEnumerate<TEnumbl, TFmt, TFmtBase>(fieldName, value.Value, filterPredicate, formatString, formatFlags);

    public TExt WhenPopulatedWithFilterEnumerate<TEnumbl, TFmt, TFmtBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TFmt>?
        where TFmt : ISpanFormattable?, TFmtBase?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddFilteredEnumerate<TEnumbl, TFmt, TFmtBase>(value, filterPredicate, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerateNullable<TEnumbl, TFmtStruct>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TFmtStruct?>
        where TFmtStruct : struct, ISpanFormattable =>
        value == null ? stb.Mold : WhenPopulatedWithFilterEnumerateNullable(fieldName, value.Value, filterPredicate, formatString, formatFlags);

    public TExt WhenPopulatedWithFilterEnumerateNullable<TEnumbl, TFmtStruct>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TFmtStruct?>?
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddFilteredEnumerateNullable(value, filterPredicate, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }


    public TExt WhenPopulatedWithFilterRevealEnumerate<TEnumbl, TFilterBase, TRevealBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable     
        where TRevealBase : notnull =>
        value == null 
            ? stb.Mold 
            : WhenPopulatedWithFilterRevealEnumerate(fieldName, value.Value, filterPredicate, palantírReveal, formatString, formatFlags);


    public TExt WhenPopulatedWithFilterRevealEnumerate<TEnumbl, TFilterBase, TRevealBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?     
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.RevealFilteredEnumerate(value, filterPredicate, palantírReveal, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }


    public TExt WhenPopulatedWithFilterRevealEnumerate<TEnumbl, TCloaked, TFilterBase, TRevealBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TCloaked>     
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull =>
        value == null 
            ? stb.Mold 
            : WhenPopulatedWithFilterRevealEnumerate<TEnumbl, TCloaked, TFilterBase, TRevealBase>
                (fieldName, value.Value, filterPredicate, palantírReveal, formatString, formatFlags);


    public TExt WhenPopulatedWithFilterRevealEnumerate<TEnumbl, TCloaked, TFilterBase, TRevealBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCloaked>?     
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.RevealFilteredEnumerate<TEnumbl, TCloaked, TFilterBase, TRevealBase>(value, filterPredicate, palantírReveal, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedWithFilterRevealEnumerateNullable<TEnumbl, TCloakedStruct>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TCloakedStruct?>     
        where TCloakedStruct : struct =>
        value == null 
            ? stb.Mold 
            : WhenPopulatedWithFilterRevealEnumerateNullable(fieldName, value.Value, filterPredicate, palantírReveal, formatString, formatFlags);
    

    public TExt WhenPopulatedWithFilterRevealEnumerateNullable<TEnumbl, TCloakedStruct>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCloakedStruct?>?     
        where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.RevealFilteredEnumerateNullable(value, filterPredicate, palantírReveal, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }
    
    public TExt WhenPopulatedWithFilterRevealEnumerate<TEnumbl, TBearerBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable  =>
        value == null 
            ? stb.Mold 
            : WhenPopulatedWithFilterRevealEnumerate(fieldName, value.Value, filterPredicate, formatString, formatFlags);

    public TExt WhenPopulatedWithFilterRevealEnumerate<TEnumbl, TBearerBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?     
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.RevealFilteredEnumerate(value, filterPredicate, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedWithFilterRevealEnumerate<TEnumbl, TBearer, TBearerBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TBearer>     
        where TBearer : IStringBearer?, TBearerBase? =>
        value == null 
            ? stb.Mold 
            : WhenPopulatedWithFilterRevealEnumerate<TEnumbl, TBearer, TBearerBase>
                (fieldName, value.Value, filterPredicate, formatString, formatFlags);

    public TExt WhenPopulatedWithFilterRevealEnumerate<TEnumbl, TBearer, TBearerBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TBearer>?     
        where TBearer : IStringBearer?, TBearerBase?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.RevealFilteredEnumerate<TEnumbl, TBearer, TBearerBase>(value, filterPredicate, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedWithFilterRevealEnumerateNullable<TEnumbl, TBearerStruct>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TBearerStruct?>     
        where TBearerStruct : struct, IStringBearer =>
        value == null 
            ? stb.Mold 
            : WhenPopulatedWithFilterRevealEnumerateNullable(fieldName, value.Value, filterPredicate, formatString, formatFlags);
    
    
    public TExt WhenPopulatedWithFilterRevealEnumerateNullable<TEnumbl, TBearerStruct>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TBearerStruct?>?     
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.RevealFilteredEnumerateNullable(value, filterPredicate, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerateString<TEnumbl>(string fieldName, TEnumbl? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<string?> =>
        value == null 
            ? stb.Mold 
            : WhenPopulatedWithFilterEnumerateString(fieldName, value.Value, filterPredicate, formatString, formatFlags);

    public TExt WhenPopulatedWithFilterEnumerateString<TEnumbl>(string fieldName, TEnumbl? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<string?>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddFilteredEnumerateString(value, filterPredicate, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerateCharSeq<TEnumbl, TCharSeqBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable =>
        value == null 
            ? stb.Mold 
            : WhenPopulatedWithFilterEnumerateCharSeq(fieldName, value.Value, filterPredicate, formatString, formatFlags);

    public TExt WhenPopulatedWithFilterEnumerateCharSeq<TEnumbl, TCharSeqBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?     
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddFilteredEnumerateCharSeq(value, filterPredicate, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerateCharSeq<TEnumbl, TCharSeq, TCharSeqBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TCharSeq>     
        where TCharSeq : ICharSequence?, TCharSeqBase? =>
        value == null 
            ? stb.Mold 
            : WhenPopulatedWithFilterEnumerateCharSeq<TEnumbl, TCharSeq, TCharSeqBase>
                (fieldName, value.Value, filterPredicate, formatString, formatFlags);

    public TExt WhenPopulatedWithFilterEnumerateCharSeq<TEnumbl, TCharSeq, TCharSeqBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCharSeq>?     
        where TCharSeq : ICharSequence?, TCharSeqBase?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddFilteredEnumerateCharSeq<TEnumbl, TCharSeq, TCharSeqBase>(value, filterPredicate, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerateStringBuilder<TEnumbl>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<StringBuilder?> =>
        value == null 
            ? stb.Mold 
            : WhenPopulatedWithFilterEnumerateStringBuilder(fieldName, value.Value, filterPredicate, formatString, formatFlags);

    public TExt WhenPopulatedWithFilterEnumerateStringBuilder<TEnumbl>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<StringBuilder?>?     
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddFilteredEnumerateStringBuilder(value, filterPredicate, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }
    
    public TExt WhenPopulatedWithFilterEnumerateMatch<TEnumbl, TAnyBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable =>
        value == null 
            ? stb.Mold 
            : WhenPopulatedWithFilterEnumerateMatch(fieldName, value.Value, filterPredicate, formatString, formatFlags);
    
    public TExt WhenPopulatedWithFilterEnumerateMatch<TEnumbl, TAnyBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddFilteredEnumerateMatch(value, filterPredicate, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }
    
    public TExt WhenPopulatedWithFilterEnumerateMatch<TEnumbl, TAny, TAnyBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TAny>      
        where TAny : TAnyBase? =>
        value == null 
            ? stb.Mold 
            : WhenPopulatedWithFilterEnumerateMatch<TEnumbl, TAny, TAnyBase>(fieldName, value.Value, filterPredicate, formatString, formatFlags);
    
    public TExt WhenPopulatedWithFilterEnumerateMatch<TEnumbl, TAny, TAnyBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TAny>?      
        where TAny : TAnyBase?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddFilteredEnumerateMatch<TEnumbl, TAny, TAnyBase>(value, filterPredicate, formatString, formatFlags);
            var anyItems = sc.ItemCount > 0;
            sc.Complete();
            if (anyItems)
            {
                return stb.AddGoToNext();
            }
        }
        return stb.Mold;
    }

    public TExt WhenPopulatedWithFilterEnumerateObject<TEnumbl>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<object> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<object?> =>
        value == null 
            ? stb.Mold 
            : WhenPopulatedWithFilterEnumerateObject(fieldName, value.Value, filterPredicate, formatString, formatFlags);

    public TExt WhenPopulatedWithFilterEnumerateObject<TEnumbl>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<object> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<object?>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        if (value != null)
        {
            var sc = stb.Master.StartSimpleCollectionType(value, formatFlags | SuppressOpening);
            ((IOrderedCollectionExtendFunctionality)sc).BeforeFirstElementWriteFieldName(fieldName);
            sc.AddFilteredEnumerateObject(value, filterPredicate, formatString, formatFlags);
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
