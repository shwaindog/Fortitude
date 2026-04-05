// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.CollectionField;

public partial class SelectTypeCollectionField<TMold> where TMold : TypeMolder
{
    public TMold WhenNonNullAddAllIterateBool<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : struct, IEnumerator<bool> =>
        WhenConditionMetAddAllIterateBool(value != null, fieldName, value, formatString, formatFlags);
    
    public TMold WhenNonNullAddAllIterateBool<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator<bool>? =>
        WhenConditionMetAddAllIterateBool(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllIterateNullableBool<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : struct, IEnumerator<bool?> =>
        WhenConditionMetAddAllIterateNullableBool(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllIterateNullableBool<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator<bool?>? =>
        WhenConditionMetAddAllIterateNullableBool(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllIterate<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : struct, IEnumerator =>
        WhenConditionMetAddAllIterate(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllIterate<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator? =>
        WhenConditionMetAddAllIterate(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllIterate<TEnumtr, TFmt>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : struct, IEnumerator<TFmt>
      where TFmt : ISpanFormattable? =>
        WhenConditionMetAddAllIterate<TEnumtr, TFmt>(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllIterate<TEnumtr, TFmt>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator<TFmt>?
      where TFmt : ISpanFormattable? =>
        WhenConditionMetAddAllIterate<TEnumtr, TFmt>(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllIterateNullable<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : struct, IEnumerator =>
        WhenConditionMetAddAllIterateNullable(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllIterateNullable<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator? =>
        WhenConditionMetAddAllIterateNullable(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllIterateNullable<TEnumtr, TFmtStruct>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : struct, IEnumerator<TFmtStruct?>
      where TFmtStruct : struct, ISpanFormattable =>
        WhenConditionMetAddAllIterateNullable<TEnumtr, TFmtStruct>(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllIterateNullable<TEnumtr, TFmtStruct>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator<TFmtStruct?>?
      where TFmtStruct : struct, ISpanFormattable =>
        WhenConditionMetAddAllIterateNullable<TEnumtr, TFmtStruct>(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullRevealAllIterate<TEnumtr, TRevealBase>(string fieldName, TEnumtr? value
    , PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator 
        where TRevealBase : notnull =>
        WhenConditionMetRevealAllIterate(value != null, fieldName, value, palantírReveal, formatString, formatFlags);

    public TMold WhenNonNullRevealAllIterate<TEnumtr, TRevealBase>(string fieldName, TEnumtr? value
    , PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator? 
        where TRevealBase : notnull =>
        WhenConditionMetRevealAllIterate(value != null, fieldName, value, palantírReveal, formatString, formatFlags);

    public TMold WhenNonNullRevealAllIterate<TEnumtr, TCloaked, TRevealBase>(string fieldName, TEnumtr? value
    , PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TCloaked> 
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        WhenConditionMetRevealAllIterate<TEnumtr, TCloaked, TRevealBase>(value != null, fieldName, value, palantírReveal, formatString, formatFlags);

    public TMold WhenNonNullRevealAllIterate<TEnumtr, TCloaked, TRevealBase>(string fieldName, TEnumtr? value
    , PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TCloaked>? 
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        WhenConditionMetRevealAllIterate<TEnumtr, TCloaked, TRevealBase>(value != null, fieldName, value, palantírReveal, formatString, formatFlags);

    public TMold WhenNonNullRevealAllIterateNullable<TEnumtr, TCloakedStruct>(string fieldName, TEnumtr? value
    , PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : struct, IEnumerator<TCloakedStruct?> 
      where TCloakedStruct : struct =>
        WhenConditionMetRevealAllIterateNullable(value != null, fieldName, value, palantírReveal, formatString, formatFlags);

    public TMold WhenNonNullRevealAllIterateNullable<TEnumtr, TCloakedStruct>(string fieldName, TEnumtr? value
    , PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator<TCloakedStruct?>? 
      where TCloakedStruct : struct =>
        WhenConditionMetRevealAllIterateNullable(value != null, fieldName, value, palantírReveal, formatString, formatFlags);

    public TMold WhenNonNullRevealAllIterate<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator  =>
        WhenConditionMetRevealAllIterate(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullRevealAllIterate<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?  =>
        WhenConditionMetRevealAllIterate(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullRevealAllIterate<TEnumtr, TBearer>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TBearer> 
        where TBearer : IStringBearer? =>
        WhenConditionMetRevealAllIterate<TEnumtr, TBearer>(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullRevealAllIterate<TEnumtr, TBearer>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TBearer>? 
        where TBearer : IStringBearer? =>
        WhenConditionMetRevealAllIterate<TEnumtr, TBearer>(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullRevealAllIterateNullable<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator  =>
        WhenConditionMetRevealAllIterateNullable(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullRevealAllIterateNullable<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?  =>
        WhenConditionMetRevealAllIterateNullable(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullRevealAllIterateNullable<TEnumtr, TBearerStruct>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TBearerStruct?> 
        where TBearerStruct : struct, IStringBearer =>
        WhenConditionMetRevealAllIterateNullable<TEnumtr, TBearerStruct>(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullRevealAllIterateNullable<TEnumtr, TBearerStruct>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TBearerStruct?>? 
        where TBearerStruct : struct, IStringBearer =>
        WhenConditionMetRevealAllIterateNullable<TEnumtr, TBearerStruct>(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllIterateString<TEnumtr>(string fieldName, TEnumtr? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : struct, IEnumerator<string?>  =>
        WhenConditionMetAddAllIterateString(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllIterateString<TEnumtr>(string fieldName, TEnumtr? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator<string?>?  =>
        WhenConditionMetAddAllIterateString(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllIterateCharSeq<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : struct, IEnumerator  =>
        WhenConditionMetAddAllIterateCharSeq(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllIterateCharSeq<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator?  =>
        WhenConditionMetAddAllIterateCharSeq(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllIterateCharSeq<TEnumtr, TCharSeq>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : struct, IEnumerator<TCharSeq> 
      where TCharSeq : ICharSequence? =>
        WhenConditionMetAddAllIterateCharSeq<TEnumtr, TCharSeq>(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllIterateCharSeq<TEnumtr, TCharSeq>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator<TCharSeq>? 
      where TCharSeq : ICharSequence? =>
        WhenConditionMetAddAllIterateCharSeq<TEnumtr, TCharSeq>(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllIterateStringBuilder<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : struct, IEnumerator<StringBuilder?> =>
        WhenConditionMetAddAllIterateStringBuilder(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllIterateStringBuilder<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator<StringBuilder?>? =>
        WhenConditionMetAddAllIterateStringBuilder(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllIterateMatch<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : struct, IEnumerator =>
        WhenConditionMetAddAllIterateMatch(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllIterateMatch<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator? =>
        WhenConditionMetAddAllIterateMatch(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllIterateMatch<TEnumtr, TAny>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : struct, IEnumerator<TAny> =>
        WhenConditionMetAddAllIterateMatch<TEnumtr, TAny>(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllIterateMatch<TEnumtr, TAny>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator<TAny>? =>
        WhenConditionMetAddAllIterateMatch<TEnumtr, TAny>(value != null, fieldName, value, formatString, formatFlags);

    [CallsObjectToString]
    public TMold WhenNonNullAddAllIterateObject<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : struct, IEnumerator<object?> =>
        WhenNonNullAddAllIterateMatch<TEnumtr, object?>(fieldName, value, formatString, formatFlags);

    [CallsObjectToString]
    public TMold WhenNonNullAddAllIterateObject<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator<object?>? =>
        WhenNonNullAddAllIterateMatch<TEnumtr, object?>(fieldName, value, formatString, formatFlags);

}
