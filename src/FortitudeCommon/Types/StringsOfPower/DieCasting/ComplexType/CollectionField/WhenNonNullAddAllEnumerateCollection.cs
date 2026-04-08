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
    public TMold WhenNonNullAddAllEnumerateBool<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable<bool> =>
        WhenConditionMetAddAllEnumerateBool(value != null, fieldName, value, formatString, formatFlags);
    
    public TMold WhenNonNullAddAllEnumerateBool<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<bool>? =>
        WhenConditionMetAddAllEnumerateBool(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateNullableBool<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable<bool?> =>
        WhenConditionMetAddAllEnumerateNullableBool(value != null, fieldName, value, formatString, formatFlags);
    
    public TMold WhenNonNullAddAllEnumerateNullableBool<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<bool?>? =>
        WhenConditionMetAddAllEnumerateNullableBool(value != null, fieldName, value, formatString, formatFlags);
    
    public TMold WhenNonNullAddAllEnumerate<TEnumbl>(string fieldName, TEnumbl? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable =>
        WhenConditionMetAddAllEnumerate(value != null, fieldName, value, formatString, formatFlags);
    
    public TMold WhenNonNullAddAllEnumerate<TEnumbl>(string fieldName, TEnumbl? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable? =>
        WhenConditionMetAddAllEnumerate(value != null, fieldName, value, formatString, formatFlags);
    
    public TMold WhenNonNullAddAllEnumerate<TEnumbl, TFmt>(string fieldName, TEnumbl? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable<TFmt>
      where TFmt : ISpanFormattable? =>
        WhenConditionMetAddAllEnumerate<TEnumbl, TFmt>(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerate<TEnumbl, TFmt>(string fieldName, TEnumbl? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<TFmt>?
      where TFmt : ISpanFormattable? =>
        WhenConditionMetAddAllEnumerate<TEnumbl, TFmt>(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateNullable<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable =>
        WhenConditionMetAddAllEnumerateNullable(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateNullable<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable? =>
        WhenConditionMetAddAllEnumerateNullable(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateNullable<TEnumbl, TFmtStruct>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable<TFmtStruct?>
      where TFmtStruct : struct, ISpanFormattable =>
        WhenConditionMetAddAllEnumerateNullable<TEnumbl, TFmtStruct>(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateNullable<TEnumbl, TFmtStruct>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<TFmtStruct?>?
      where TFmtStruct : struct, ISpanFormattable =>
        WhenConditionMetAddAllEnumerateNullable<TEnumbl, TFmtStruct>(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullRevealAllEnumerate<TEnumbl, TRevealBase>(string fieldName, TEnumbl? value
    , PalantírReveal<TRevealBase> palantírReveal
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable
        where TRevealBase : notnull =>
        WhenConditionMetRevealAllEnumerate(value != null, fieldName, value, palantírReveal, formatString, formatFlags);

    public TMold WhenNonNullRevealAllEnumerate<TEnumbl, TRevealBase>(string fieldName, TEnumbl? value
    , PalantírReveal<TRevealBase> palantírReveal
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable?
        where TRevealBase : notnull =>
        WhenConditionMetRevealAllEnumerate(value != null, fieldName, value, palantírReveal, formatString, formatFlags);

    public TMold WhenNonNullRevealAllEnumerate<TEnumbl, TCloaked, TRevealBase>(string fieldName, TEnumbl? value
    , PalantírReveal<TRevealBase> palantírReveal
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable<TCloaked>
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        WhenConditionMetRevealAllEnumerate<TEnumbl, TCloaked, TRevealBase>(value != null, fieldName, value, palantírReveal, formatString, formatFlags);

    public TMold WhenNonNullRevealAllEnumerate<TEnumbl, TCloaked, TRevealBase>(string fieldName, TEnumbl? value
    , PalantírReveal<TRevealBase> palantírReveal
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<TCloaked>?
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        WhenConditionMetRevealAllEnumerate<TEnumbl, TCloaked, TRevealBase>(value != null, fieldName, value, palantírReveal, formatString, formatFlags);

    public TMold WhenNonNullRevealAllEnumerateNullable<TEnumbl, TCloakedStruct>(string fieldName, TEnumbl? value
    , PalantírReveal<TCloakedStruct> palantírReveal
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable<TCloakedStruct?>
      where TCloakedStruct : struct =>
        WhenConditionMetRevealAllEnumerateNullable(value != null, fieldName, value, palantírReveal, formatString, formatFlags);

    public TMold WhenNonNullRevealAllEnumerateNullable<TEnumbl, TCloakedStruct>(string fieldName, TEnumbl? value
    , PalantírReveal<TCloakedStruct> palantírReveal
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<TCloakedStruct?>?
      where TCloakedStruct : struct =>
        WhenConditionMetRevealAllEnumerateNullable(value != null, fieldName, value, palantírReveal, formatString, formatFlags);

    public TMold WhenNonNullRevealAllEnumerate<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable =>
        WhenConditionMetRevealAllEnumerate(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullRevealAllEnumerate<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable? =>
        WhenConditionMetRevealAllEnumerate(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullRevealAllEnumerate<TEnumbl, TBearer>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TBearer>
        where TBearer : IStringBearer? =>
        WhenConditionMetRevealAllEnumerate<TEnumbl, TBearer>(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullRevealAllEnumerate<TEnumbl, TBearer>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TBearer>?
        where TBearer : IStringBearer? =>
        WhenConditionMetRevealAllEnumerate<TEnumbl, TBearer>(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullRevealAllEnumerateNullable<TEnumbl, TBearerStruct>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TBearerStruct?>
        where TBearerStruct : struct, IStringBearer =>
        WhenConditionMetRevealAllEnumerateNullable<TEnumbl, TBearerStruct>(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullRevealAllEnumerateNullable<TEnumbl, TBearerStruct>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TBearerStruct?>?
        where TBearerStruct : struct, IStringBearer =>
        WhenConditionMetRevealAllEnumerateNullable<TEnumbl, TBearerStruct>(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateString<TEnumbl>(string fieldName, TEnumbl? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable<string?> =>
        WhenConditionMetAddAllEnumerateString(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateString<TEnumbl>(string fieldName, TEnumbl? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<string?>? =>
        WhenConditionMetAddAllEnumerateString(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateCharSeq<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable =>
        WhenConditionMetAddAllEnumerateCharSeq(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateCharSeq<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable? =>
        WhenConditionMetAddAllEnumerateCharSeq(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateCharSeq<TEnumbl, TCharSeq>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable<TCharSeq>
      where TCharSeq : ICharSequence? =>
        WhenConditionMetAddAllEnumerateCharSeq<TEnumbl, TCharSeq>(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateCharSeq<TEnumbl, TCharSeq>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<TCharSeq>?
      where TCharSeq : ICharSequence? =>
        WhenConditionMetAddAllEnumerateCharSeq<TEnumbl, TCharSeq>(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateStringBuilder<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable<StringBuilder?> =>
        WhenConditionMetAddAllEnumerateStringBuilder(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateStringBuilder<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<StringBuilder?>? =>
        WhenConditionMetAddAllEnumerateStringBuilder(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateMatch<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable =>
        WhenConditionMetAddAllEnumerateMatch(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateMatch<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable? =>
        WhenConditionMetAddAllEnumerateMatch(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateMatch<TEnumbl, TAny>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable<TAny> =>
        WhenConditionMetAddAllEnumerateMatch<TEnumbl, TAny>(value != null, fieldName, value, formatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateMatch<TEnumbl, TAny>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<TAny>? =>
        WhenConditionMetAddAllEnumerateMatch<TEnumbl, TAny>(value != null, fieldName, value, formatString, formatFlags);

    [CallsObjectToString]
    public TMold WhenNonNullAddAllEnumerateObject<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable<object?> =>
        WhenNonNullAddAllEnumerateMatch<TEnumbl, object?>(fieldName, value, formatString, formatFlags);

    [CallsObjectToString]
    public TMold WhenNonNullAddAllEnumerateObject<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable<object?>? =>
        WhenNonNullAddAllEnumerateMatch<TEnumbl, object?>(fieldName, value, formatString, formatFlags);

}
