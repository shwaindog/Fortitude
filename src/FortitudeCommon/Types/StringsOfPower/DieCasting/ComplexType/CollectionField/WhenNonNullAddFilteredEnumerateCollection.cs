// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.CollectionField;

public partial class SelectTypeCollectionField<TMold> where TMold : TypeMolder
{
    public TMold WhenNonNullAddFilteredEnumerateBool<TEnumbl>(string fieldName, TEnumbl? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<bool> =>
        WhenConditionMetAddFilteredEnumerateBool(value != null, fieldName, value, filterPredicate, formatString, formatFlags);
    
    public TMold WhenNonNullAddFilteredEnumerateBool<TEnumbl>(string fieldName, TEnumbl? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<bool>? =>
        WhenConditionMetAddFilteredEnumerateBool(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredEnumerateNullableBool<TEnumbl>(string fieldName, TEnumbl?value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<bool?>?  =>
        WhenConditionMetAddFilteredEnumerateNullableBool(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredEnumerateNullableBool<TEnumbl>(string fieldName, TEnumbl?value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable<bool?>  =>
        WhenConditionMetAddFilteredEnumerateNullableBool(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredEnumerate<TEnumbl, TFmtBase>
    (string fieldName, TEnumbl? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable =>
        WhenConditionMetAddFilteredEnumerate(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredEnumerate<TEnumbl, TFmtBase>
    (string fieldName, TEnumbl? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable? =>
        WhenConditionMetAddFilteredEnumerate(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredEnumerate<TEnumbl, TFmt, TFmtBase>
    (string fieldName, TEnumbl? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable<TFmt>
      where TFmt : ISpanFormattable?, TFmtBase? =>
        WhenConditionMetAddFilteredEnumerate<TEnumbl, TFmt, TFmtBase>(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredEnumerate<TEnumbl, TFmt, TFmtBase>
    (string fieldName, TEnumbl? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<TFmt>?
      where TFmt : ISpanFormattable?, TFmtBase? =>
        WhenConditionMetAddFilteredEnumerate<TEnumbl, TFmt, TFmtBase>(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredEnumerateNullable<TEnumbl, TFmtStruct>
    (string fieldName, TEnumbl? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable<TFmtStruct?>
      where TFmtStruct : struct, ISpanFormattable =>
        WhenConditionMetAddFilteredEnumerateNullable(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredEnumerateNullable<TEnumbl, TFmtStruct>
    (string fieldName, TEnumbl? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<TFmtStruct?>?
      where TFmtStruct : struct, ISpanFormattable =>
        WhenConditionMetAddFilteredEnumerateNullable(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullRevealFilteredEnumerate<TEnumbl, TFilterBase, TRevealBase>
    (string fieldName, TEnumbl? value, OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TRevealBase : notnull =>
        WhenConditionMetRevealFilteredEnumerate(value != null, fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags);

    public TMold WhenNonNullRevealFilteredEnumerate<TEnumbl, TFilterBase, TRevealBase>
    (string fieldName, TEnumbl? value, OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TRevealBase : notnull =>
        WhenConditionMetRevealFilteredEnumerate(value != null, fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags);

    public TMold WhenNonNullRevealFilteredEnumerate<TEnumbl, TCloaked, TFilterBase, TRevealBase>
    (string fieldName, TEnumbl? value, OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TCloaked>
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull =>
        WhenConditionMetRevealFilteredEnumerate<TEnumbl, TCloaked, TFilterBase, TRevealBase>
          (value != null, fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags);

    public TMold WhenNonNullRevealFilteredEnumerate<TEnumbl, TCloaked, TFilterBase, TRevealBase>
    (string fieldName, TEnumbl? value, OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCloaked>?
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull =>
        WhenConditionMetRevealFilteredEnumerate<TEnumbl, TCloaked, TFilterBase, TRevealBase>
          (value != null, fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags);

    public TMold WhenNonNullRevealFilteredEnumerateNullable<TEnumbl, TCloakedStruct>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable<TCloakedStruct?>
      where TCloakedStruct : struct =>
        WhenConditionMetRevealFilteredEnumerateNullable(value != null, fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags);

    public TMold WhenNonNullRevealFilteredEnumerateNullable<TEnumbl, TCloakedStruct>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<TCloakedStruct?>?
      where TCloakedStruct : struct =>
        WhenConditionMetRevealFilteredEnumerateNullable(value != null, fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags);

    public TMold WhenNonNullRevealFilteredEnumerate<TEnumbl, TBearerBase>(string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable  =>
        WhenConditionMetRevealFilteredEnumerate(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullRevealFilteredEnumerate<TEnumbl, TBearerBase>(string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable?  =>
        WhenConditionMetRevealFilteredEnumerate(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullRevealFilteredEnumerate<TEnumbl, TBearer, TBearerBase>(string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable<TBearer>
      where TBearer : IStringBearer?, TBearerBase? =>
        WhenConditionMetRevealFilteredEnumerate<TEnumbl, TBearer, TBearerBase>
          (value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullRevealFilteredEnumerate<TEnumbl, TBearer, TBearerBase>(string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<TBearer>? 
      where TBearer : IStringBearer?, TBearerBase? =>
        WhenConditionMetRevealFilteredEnumerate<TEnumbl, TBearer, TBearerBase>
          (value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullRevealFilteredEnumerateNullable<TEnumbl, TBearerStruct>
    (string fieldName
    , TEnumbl? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable<TBearerStruct?> 
      where TBearerStruct : struct, IStringBearer =>
        WhenConditionMetRevealFilteredEnumerateNullable(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullRevealFilteredEnumerateNullable<TEnumbl, TBearerStruct>
    (string fieldName
    , TEnumbl? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<TBearerStruct?>? 
      where TBearerStruct : struct, IStringBearer =>
        WhenConditionMetRevealFilteredEnumerateNullable(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredEnumerateString<TEnumbl>
    (string fieldName, TEnumbl? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable<string?> =>
        WhenConditionMetAddFilteredEnumerateString(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredEnumerateString<TEnumbl>
    (string fieldName, TEnumbl? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable<string?>? =>
        WhenConditionMetAddFilteredEnumerateString(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredEnumerateCharSeq<TEnumbl, TCharSeqBase>
    (string fieldName, TEnumbl? value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : struct, IEnumerable =>
        WhenConditionMetAddFilteredEnumerateCharSeq(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredEnumerateCharSeq<TEnumbl, TCharSeqBase>
    (string fieldName, TEnumbl? value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : IEnumerable? =>
        WhenConditionMetAddFilteredEnumerateCharSeq(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredEnumerateCharSeq<TEnumbl, TCharSeq, TCharSeqBase>
    (string fieldName, TEnumbl? value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : struct, IEnumerable<TCharSeq>
        where TCharSeq : ICharSequence?, TCharSeqBase? =>
        WhenConditionMetAddFilteredEnumerateCharSeq<TEnumbl, TCharSeq, TCharSeqBase>
          (value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredEnumerateCharSeq<TEnumbl, TCharSeq, TCharSeqBase>
    (string fieldName, TEnumbl? value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : IEnumerable<TCharSeq>?
        where TCharSeq : ICharSequence?, TCharSeqBase? =>
        WhenConditionMetAddFilteredEnumerateCharSeq<TEnumbl, TCharSeq, TCharSeqBase>
          (value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredEnumerateStringBuilder<TEnumbl>
    (string fieldName, TEnumbl? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable<StringBuilder?> =>
        WhenConditionMetAddFilteredEnumerateStringBuilder(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredEnumerateStringBuilder<TEnumbl>
    (string fieldName, TEnumbl? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<StringBuilder?>? =>
        WhenConditionMetAddFilteredEnumerateStringBuilder(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredEnumerateMatch<TEnumbl, TAnyBase>
    (string fieldName, TEnumbl? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable =>
        WhenConditionMetAddFilteredEnumerateMatch(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredEnumerateMatch<TEnumbl, TAnyBase>
    (string fieldName, TEnumbl? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable? =>
        WhenConditionMetAddFilteredEnumerateMatch(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredEnumerateMatch<TEnumbl, TAny, TAnyBase>
    (string fieldName, TEnumbl? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TAny> 
        where TAny : TAnyBase? =>
        WhenConditionMetAddFilteredEnumerateMatch<TEnumbl, TAny, TAnyBase>(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredEnumerateMatch<TEnumbl, TAny, TAnyBase>
    (string fieldName, TEnumbl? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TAny>? 
        where TAny : TAnyBase? =>
        WhenConditionMetAddFilteredEnumerateMatch<TEnumbl, TAny, TAnyBase>(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    [CallsObjectToString]
    public TMold WhenNonNullAddFilteredEnumerateObject<TEnumbl>
    (string fieldName, TEnumbl? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable<object?> =>
        WhenNonNullAddFilteredEnumerateMatch<TEnumbl, object?, object>(fieldName, value, filterPredicate, formatString, formatFlags);

    [CallsObjectToString]
    public TMold WhenNonNullAddFilteredEnumerateObject<TEnumbl>
    (string fieldName, TEnumbl? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable<object?>? =>
        WhenNonNullAddFilteredEnumerateMatch<TEnumbl, object?, object>(fieldName, value, filterPredicate, formatString, formatFlags);

}
