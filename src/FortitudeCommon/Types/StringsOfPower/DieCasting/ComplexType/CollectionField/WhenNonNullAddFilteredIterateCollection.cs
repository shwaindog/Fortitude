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
    public TMold WhenNonNullAddFilteredIterateBool<TEnumtr>(string fieldName, TEnumtr? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : struct, IEnumerator<bool> =>
        WhenConditionMetAddFilteredIterateBool(value != null, fieldName, value, filterPredicate, formatString, formatFlags);
    
    public TMold WhenNonNullAddFilteredIterateBool<TEnumtr>(string fieldName, TEnumtr? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator<bool>? =>
        WhenConditionMetAddFilteredIterateBool(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateNullableBool<TEnumtr>(string fieldName, TEnumtr? value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : struct, IEnumerator<bool?>  =>
        WhenConditionMetAddFilteredIterateNullableBool(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateNullableBool<TEnumtr>(string fieldName, TEnumtr? value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator<bool?>?  =>
        WhenConditionMetAddFilteredIterateNullableBool(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterate<TEnumtr, TFmtBase>
    (string fieldName, TEnumtr? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator  =>
        WhenConditionMetAddFilteredIterate(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterate<TEnumtr, TFmtBase>
    (string fieldName, TEnumtr? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator?  =>
        WhenConditionMetAddFilteredIterate(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterate<TEnumtr, TFmt, TFmtBase>
    (string fieldName, TEnumtr? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator<TFmt> 
      where TFmt : ISpanFormattable?, TFmtBase? =>
        WhenConditionMetAddFilteredIterate<TEnumtr, TFmt, TFmtBase>(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterate<TEnumtr, TFmt, TFmtBase>
    (string fieldName, TEnumtr? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator<TFmt>? 
      where TFmt : ISpanFormattable?, TFmtBase? =>
        WhenConditionMetAddFilteredIterate<TEnumtr, TFmt, TFmtBase>(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateNullable<TEnumtr, TFmtStruct>
    (string fieldName, TEnumtr? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : struct, IEnumerator<TFmtStruct?>
        where TFmtStruct : struct, ISpanFormattable =>
        WhenConditionMetAddFilteredIterateNullable(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateNullable<TEnumtr, TFmtStruct>
    (string fieldName, TEnumtr? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : IEnumerator<TFmtStruct?>?
        where TFmtStruct : struct, ISpanFormattable =>
        WhenConditionMetAddFilteredIterateNullable(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullRevealFilteredIterate<TEnumtr, TFilterBase, TRevealBase>
    (string fieldName, TEnumtr? value, OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : struct, IEnumerator
        where TRevealBase : notnull =>
        WhenConditionMetRevealFilteredIterate(value != null, fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags);

    public TMold WhenNonNullRevealFilteredIterate<TEnumtr, TFilterBase, TRevealBase>
    (string fieldName, TEnumtr? value, OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : IEnumerator?
        where TRevealBase : notnull =>
        WhenConditionMetRevealFilteredIterate(value != null, fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags);

    public TMold WhenNonNullRevealFilteredIterate<TEnumtr, TCloaked, TFilterBase, TRevealBase>
    (string fieldName, TEnumtr? value, OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : struct, IEnumerator<TCloaked>
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull =>
        WhenConditionMetRevealFilteredIterate<TEnumtr, TCloaked, TFilterBase, TRevealBase>
          (value != null, fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags);

    public TMold WhenNonNullRevealFilteredIterate<TEnumtr, TCloaked, TFilterBase, TRevealBase>
    (string fieldName, TEnumtr? value, OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : IEnumerator<TCloaked>?
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull =>
        WhenConditionMetRevealFilteredIterate<TEnumtr, TCloaked, TFilterBase, TRevealBase>
          (value != null, fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags);

    public TMold WhenNonNullRevealFilteredIterateNullable<TEnumtr, TCloakedStruct>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator<TCloakedStruct?>
      where TCloakedStruct : struct =>
        WhenConditionMetRevealFilteredIterateNullable(value != null, fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags);

    public TMold WhenNonNullRevealFilteredIterateNullable<TEnumtr, TCloakedStruct>(string fieldName, TEnumtr? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator<TCloakedStruct?>?
      where TCloakedStruct : struct =>
        WhenConditionMetRevealFilteredIterateNullable(value != null, fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags);

    public TMold WhenNonNullRevealFilteredIterate<TEnumtr, TBearerBase>(string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : struct, IEnumerator =>
        WhenConditionMetRevealFilteredIterate(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullRevealFilteredIterate<TEnumtr, TBearerBase>(string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator? =>
        WhenConditionMetRevealFilteredIterate(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullRevealFilteredIterate<TEnumtr, TBearer, TBearerBase>(string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : struct, IEnumerator<TBearer>
      where TBearer : IStringBearer?, TBearerBase? =>
        WhenConditionMetRevealFilteredIterate<TEnumtr, TBearer, TBearerBase>(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullRevealFilteredIterate<TEnumtr, TBearer, TBearerBase>(string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator<TBearer>?
      where TBearer : IStringBearer?, TBearerBase? =>
        WhenConditionMetRevealFilteredIterate<TEnumtr, TBearer, TBearerBase>(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullRevealFilteredIterateNullable<TEnumtr, TBearerStruct>(
      string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
       where TEnumtr : struct, IEnumerator<TBearerStruct?>
       where TBearerStruct : struct, IStringBearer =>
        WhenConditionMetRevealFilteredIterateNullable(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullRevealFilteredIterateNullable<TEnumtr, TBearerStruct>(
      string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
       where TEnumtr : IEnumerator<TBearerStruct?>?
       where TBearerStruct : struct, IStringBearer =>
        WhenConditionMetRevealFilteredIterateNullable(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateString<TEnumtr>
    (string fieldName, TEnumtr? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : struct, IEnumerator<string?> =>
        WhenConditionMetAddFilteredIterateString(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateString<TEnumtr>
    (string fieldName, TEnumtr? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator<string?>? =>
        WhenConditionMetAddFilteredIterateString(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateCharSeq<TEnumtr, TCharSeqBase>
    (string fieldName, TEnumtr? value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator =>
        WhenConditionMetAddFilteredIterateCharSeq(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateCharSeq<TEnumtr, TCharSeqBase>
    (string fieldName, TEnumtr? value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator? =>
        WhenConditionMetAddFilteredIterateCharSeq(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateCharSeq<TEnumtr, TCharSeq, TCharSeqBase>
    (string fieldName, TEnumtr? value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TCharSeq>
        where TCharSeq : ICharSequence?, TCharSeqBase? =>
        WhenConditionMetAddFilteredIterateCharSeq<TEnumtr, TCharSeq, TCharSeqBase>
          (value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateCharSeq<TEnumtr, TCharSeq, TCharSeqBase>
    (string fieldName, TEnumtr? value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TCharSeq>?
        where TCharSeq : ICharSequence?, TCharSeqBase? =>
        WhenConditionMetAddFilteredIterateCharSeq<TEnumtr, TCharSeq, TCharSeqBase>
          (value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateStringBuilder<TEnumtr>
    (string fieldName, TEnumtr? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator<StringBuilder?> =>
        WhenConditionMetAddFilteredIterateStringBuilder(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateStringBuilder<TEnumtr>
    (string fieldName, TEnumtr? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator<StringBuilder?>? =>
        WhenConditionMetAddFilteredIterateStringBuilder(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateMatch<TEnumtr, TAnyBase>
    (string fieldName, TEnumtr? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TEnumtr : struct, IEnumerator =>
        WhenConditionMetAddFilteredIterateMatch(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateMatch<TEnumtr, TAnyBase>
    (string fieldName, TEnumtr? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TEnumtr : IEnumerator? =>
        WhenConditionMetAddFilteredIterateMatch(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateMatch<TEnumtr, TAny, TAnyBase>
    (string fieldName, TEnumtr? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TEnumtr : struct, IEnumerator<TAny>
        where TAny : TAnyBase? =>
        WhenConditionMetAddFilteredIterateMatch<TEnumtr, TAny, TAnyBase>(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateMatch<TEnumtr, TAny, TAnyBase>
    (string fieldName, TEnumtr? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TEnumtr : IEnumerator<TAny>?
        where TAny : TAnyBase? =>
        WhenConditionMetAddFilteredIterateMatch<TEnumtr, TAny, TAnyBase>(value != null, fieldName, value, filterPredicate, formatString, formatFlags);

    [CallsObjectToString]
    public TMold WhenNonNullAddFilteredIterateObject<TEnumtr>
    (string fieldName, TEnumtr? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : struct, IEnumerator<object?>
      => WhenNonNullAddFilteredIterateMatch<TEnumtr, object?, object>(fieldName, value, filterPredicate, formatString, formatFlags);

    [CallsObjectToString]
    public TMold WhenNonNullAddFilteredIterateObject<TEnumtr>
    (string fieldName, TEnumtr? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator<object?>?
      => WhenNonNullAddFilteredIterateMatch<TEnumtr, object?, object>(fieldName, value, filterPredicate, formatString, formatFlags);

}
