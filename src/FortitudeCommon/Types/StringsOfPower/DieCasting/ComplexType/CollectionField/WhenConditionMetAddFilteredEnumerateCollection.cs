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
    public TExt WhenConditionMetAddFilteredEnumerateBool<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<bool> =>
        condition
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);
    
    public TExt WhenConditionMetAddFilteredEnumerateBool<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<bool>? =>
        condition
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);
    

    public TExt WhenConditionMetAddFilteredEnumerateNullableBool<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : struct, IEnumerable<bool?> =>
        condition
            ? AlwaysAddFilteredEnumerateNullable(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateNullableBool<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : IEnumerable<bool?>? =>
        condition
            ? AlwaysAddFilteredEnumerateNullable(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerate<TEnumbl, TFmtBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable  =>
        condition
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);
    

    public TExt WhenConditionMetAddFilteredEnumerate<TEnumbl, TFmtBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?  =>
        condition
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);
    

    public TExt WhenConditionMetAddFilteredEnumerate<TEnumbl, TFmt, TFmtBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TFmt> 
        where TFmt : ISpanFormattable?, TFmtBase? =>
        condition
            ? AlwaysAddFilteredEnumerate<TEnumbl, TFmt, TFmtBase>(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerate<TEnumbl, TFmt, TFmtBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TFmt>? 
        where TFmt : ISpanFormattable?, TFmtBase? =>
        condition
            ? AlwaysAddFilteredEnumerate<TEnumbl, TFmt, TFmtBase>(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateNullable<TEnumbl, TFmtStruct>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TFmtStruct?> 
        where TFmtStruct : struct, ISpanFormattable =>
        condition
            ? AlwaysAddFilteredEnumerateNullable(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateNullable<TEnumbl, TFmtStruct>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TFmtStruct?>? 
        where TFmtStruct : struct, ISpanFormattable =>
        condition
            ? AlwaysAddFilteredEnumerateNullable(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFilteredEnumerate<TEnumbl, TFilterBase, TRevealBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TRevealBase : notnull =>
        condition
            ? AlwaysRevealFilteredEnumerate(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);
    

    public TExt WhenConditionMetRevealFilteredEnumerate<TEnumbl, TFilterBase, TRevealBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TRevealBase : notnull =>
        condition
            ? AlwaysRevealFilteredEnumerate(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);
    

    public TExt WhenConditionMetRevealFilteredEnumerate<TEnumbl, TCloaked, TFilterBase, TRevealBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TCloaked>
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull =>
        condition
            ? AlwaysRevealFilteredEnumerate<TEnumbl, TCloaked, TFilterBase, TRevealBase>
                (fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFilteredEnumerate<TEnumbl, TCloaked, TFilterBase, TRevealBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCloaked>?
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull =>
        condition
            ? AlwaysRevealFilteredEnumerate<TEnumbl, TCloaked, TFilterBase, TRevealBase>
                (fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFilteredEnumerateNullable<TEnumbl, TCloakedStruct>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TCloakedStruct?>
        where TCloakedStruct : struct =>
        condition
            ? AlwaysRevealFilteredEnumerateNullable(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFilteredEnumerateNullable<TEnumbl, TCloakedStruct>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCloakedStruct?>?
        where TCloakedStruct : struct =>
        condition
            ? AlwaysRevealFilteredEnumerateNullable(fieldName, value, filterPredicate, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFilteredEnumerate<TEnumbl, TBearerBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable =>
        condition
            ? AlwaysRevealFilteredEnumerate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);
    

    public TExt WhenConditionMetRevealFilteredEnumerate<TEnumbl, TBearerBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable? =>
        condition
            ? AlwaysRevealFilteredEnumerate(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);
    

    public TExt WhenConditionMetRevealFilteredEnumerate<TEnumbl, TBearer, TBearerBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TBearer>
        where TBearer : IStringBearer?, TBearerBase? =>
        condition
            ? AlwaysRevealFilteredEnumerate<TEnumbl, TBearer, TBearerBase>(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFilteredEnumerate<TEnumbl, TBearer, TBearerBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TBearer>?
        where TBearer : IStringBearer?, TBearerBase? =>
        condition
            ? AlwaysRevealFilteredEnumerate<TEnumbl, TBearer, TBearerBase>(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFilteredEnumerateNullable<TEnumbl, TBearerStruct>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TBearerStruct?>
        where TBearerStruct : struct, IStringBearer =>
        condition
            ? AlwaysRevealFilteredEnumerateNullable(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetRevealFilteredEnumerateNullable<TEnumbl, TBearerStruct>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TBearerStruct?>?
        where TBearerStruct : struct, IStringBearer =>
        condition
            ? AlwaysRevealFilteredEnumerateNullable(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateString<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<string?> =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredEnumerateString(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateString<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<string?>? =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredEnumerateString(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateCharSeq<TEnumbl, TCharSeqBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredEnumerateCharSeq(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    
    public TExt WhenConditionMetAddFilteredEnumerateCharSeq<TEnumbl, TCharSeqBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable? =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredEnumerateCharSeq(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);
    

    public TExt WhenConditionMetAddFilteredEnumerateCharSeq<TEnumbl, TCharSeq, TCharSeqBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TCharSeq>
        where TCharSeq : ICharSequence?, TCharSeqBase? =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredEnumerateCharSeq<TEnumbl, TCharSeq, TCharSeqBase>(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateCharSeq<TEnumbl, TCharSeq, TCharSeqBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCharSeq>?
        where TCharSeq : ICharSequence?, TCharSeqBase? =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredEnumerateCharSeq<TEnumbl, TCharSeq, TCharSeqBase>(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateStringBuilder<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : struct, IEnumerable<StringBuilder?> =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredEnumerateStringBuilder(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateStringBuilder<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : IEnumerable<StringBuilder?>? =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredEnumerateStringBuilder(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateMatch<TEnumbl, TAnyBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : struct, IEnumerable =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredEnumerateMatch(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);
    

    public TExt WhenConditionMetAddFilteredEnumerateMatch<TEnumbl, TAnyBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : IEnumerable? =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredEnumerateMatch(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);
    

    public TExt WhenConditionMetAddFilteredEnumerateMatch<TEnumbl, TAny, TAnyBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : struct, IEnumerable<TAny> 
        where TAny : TAnyBase? =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredEnumerateMatch<TEnumbl, TAny, TAnyBase>(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateMatch<TEnumbl, TAny, TAnyBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : IEnumerable<TAny>? 
        where TAny : TAnyBase? =>
        !stb.SkipFields && condition
            ? AlwaysAddFilteredEnumerateMatch<TEnumbl, TAny, TAnyBase>(fieldName, value, filterPredicate, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenConditionMetAddFilteredEnumerateObject<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<object?>  =>
        WhenConditionMetAddFilteredEnumerateMatch<TEnumbl, object?, object>(condition, fieldName, value, filterPredicate, formatString, formatFlags);

    [CallsObjectToString]
    public TExt WhenConditionMetAddFilteredEnumerateObject<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<object?>?  =>
        WhenConditionMetAddFilteredEnumerateMatch<TEnumbl, object?, object>(condition, fieldName, value, filterPredicate, formatString, formatFlags);
}
