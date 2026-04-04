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

public partial class SelectTypeCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenConditionMetAddFilteredIterateBool<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<bool>  =>
        condition
            ? AlwaysAddFilteredIterateBool(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);
    
    public TExt WhenConditionMetAddFilteredIterateBool<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<bool>?  =>
        condition
            ? AlwaysAddFilteredIterateBool(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateNullableBool<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : struct, IEnumerator<bool?>  =>
        condition
            ? AlwaysAddFilteredIterateNullableBool(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateNullableBool<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : IEnumerator<bool?>?  =>
        condition
            ? AlwaysAddFilteredIterateNullableBool(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);
    
    public TExt WhenConditionMetAddFilteredIterate<TEnumtr, TFmtBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : struct, IEnumerator =>
        condition
            ? AlwaysAddFilteredIterate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);
    
    public TExt WhenConditionMetAddFilteredIterate<TEnumtr, TFmtBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : IEnumerator? =>
        condition
            ? AlwaysAddFilteredIterate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterate<TEnumtr, TFmt, TFmtBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : struct, IEnumerator<TFmt>
        where TFmt : ISpanFormattable?, TFmtBase? =>
        condition
            ? AlwaysAddFilteredIterate<TEnumtr, TFmt, TFmtBase>(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterate<TEnumtr, TFmt, TFmtBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : IEnumerator<TFmt>?
        where TFmt : ISpanFormattable?, TFmtBase? =>
        condition
            ? AlwaysAddFilteredIterate<TEnumtr, TFmt, TFmtBase>(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateNullable<TEnumtr, TFmtStruct>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TFmtStruct?>
        where TFmtStruct : struct, ISpanFormattable =>
        condition
            ? AlwaysAddFilteredIterateNullable(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateNullable<TEnumtr, TFmtStruct>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TFmtStruct?>?
        where TFmtStruct : struct, ISpanFormattable =>
        condition
            ? AlwaysAddFilteredIterateNullable(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFilteredIterate<TEnumtr, TFilterBase, TRevealBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TRevealBase : notnull =>
        condition
            ? AlwaysRevealFilteredIterate(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFilteredIterate<TEnumtr, TFilterBase, TRevealBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TRevealBase : notnull =>
        condition
            ? AlwaysRevealFilteredIterate(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFilteredIterate<TEnumtr, TCloaked, TFilterBase, TRevealBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TCloaked>
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull =>
        condition
            ? AlwaysRevealFilteredIterate<TEnumtr, TCloaked, TFilterBase, TRevealBase>
                (fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFilteredIterate<TEnumtr, TCloaked, TFilterBase, TRevealBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TCloaked>?
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull =>
        condition
            ? AlwaysRevealFilteredIterate<TEnumtr, TCloaked, TFilterBase, TRevealBase>
                (fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFilteredIterateNullable<TEnumtr, TCloakedStruct>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TCloakedStruct?>
        where TCloakedStruct : struct =>
        condition
            ? AlwaysRevealFilteredIterateNullable(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFilteredIterateNullable<TEnumtr, TCloakedStruct>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TCloakedStruct?>?
        where TCloakedStruct : struct =>
        condition
            ? AlwaysRevealFilteredIterateNullable(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFilteredIterate<TEnumtr, TBearerBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator =>
        condition
            ? AlwaysRevealFilteredIterate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFilteredIterate<TEnumtr, TBearerBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator? =>
        condition
            ? AlwaysRevealFilteredIterate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFilteredIterate<TEnumtr, TBearer, TBearerBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TBearer>
        where TBearer : IStringBearer?, TBearerBase? =>
        condition
            ? AlwaysRevealFilteredIterate<TEnumtr, TBearer, TBearerBase>(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFilteredIterate<TEnumtr, TBearer, TBearerBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TBearer>?
        where TBearer : IStringBearer?, TBearerBase? =>
        condition
            ? AlwaysRevealFilteredIterate<TEnumtr, TBearer, TBearerBase>(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFilteredIterateNullable<TEnumtr, TBearerStruct>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : struct, IEnumerator<TBearerStruct?>
        where TBearerStruct : struct, IStringBearer =>
        condition
            ? AlwaysRevealFilteredIterateNullable(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFilteredIterateNullable<TEnumtr, TBearerStruct>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : IEnumerator<TBearerStruct?>?
        where TBearerStruct : struct, IStringBearer =>
        condition
            ? AlwaysRevealFilteredIterateNullable(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateString<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<string?> =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredIterateString(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateString<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<string?>? =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredIterateString(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateCharSeq<TEnumtr, TCharSeqBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredIterateCharSeq(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateCharSeq<TEnumtr, TCharSeqBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator? =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredIterateCharSeq(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateCharSeq<TEnumtr, TCharSeq, TCharSeqBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TCharSeq>
        where TCharSeq : ICharSequence?, TCharSeqBase? =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredIterateCharSeq<TEnumtr, TCharSeq, TCharSeqBase>(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateCharSeq<TEnumtr, TCharSeq, TCharSeqBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TCharSeq>?
        where TCharSeq : ICharSequence?, TCharSeqBase? =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredIterateCharSeq<TEnumtr, TCharSeq, TCharSeqBase>(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateStringBuilder<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<StringBuilder?> =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredIterateStringBuilder(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateStringBuilder<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<StringBuilder?>? =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredIterateStringBuilder(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateMatch<TEnumtr, TAnyBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredIterateMatch(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateMatch<TEnumtr, TAnyBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator? =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredIterateMatch(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateMatch<TEnumtr, TAny, TAnyBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TAny>
        where TAny : TAnyBase? =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredIterateMatch<TEnumtr, TAny, TAnyBase>(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateMatch<TEnumtr, TAny, TAnyBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TAny>?
        where TAny : TAnyBase? =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredIterateMatch<TEnumtr, TAny, TAnyBase>(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenConditionMetAddFilteredIterateObject<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<object?> =>
        WhenConditionMetAddFilteredIterateMatch<TEnumtr, object?, object>(condition, fieldName, value, filterPredicate, formatString, formatFlags);

    [CallsObjectToString]
    public TExt WhenConditionMetAddFilteredIterateObject<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<object?>? =>
        WhenConditionMetAddFilteredIterateMatch<TEnumtr, object?, object>(condition, fieldName, value, filterPredicate, formatString, formatFlags);
}
