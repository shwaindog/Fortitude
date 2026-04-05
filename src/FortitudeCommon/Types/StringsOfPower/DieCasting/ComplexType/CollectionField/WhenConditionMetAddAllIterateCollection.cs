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
    public TMold WhenConditionMetAddAllIterateBool<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<bool> =>
        condition
            ? AlwaysAddAllIterateBool(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllIterateBool<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<bool>? =>
        condition
            ? AlwaysAddAllIterateBool(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllIterateNullableBool<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<bool?> =>
        condition
            ? AlwaysAddAllIterateNullableBool(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllIterateNullableBool<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<bool?>? =>
        condition
            ? AlwaysAddAllIterateNullableBool(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllIterate<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : struct, IEnumerator =>
        condition
            ? AlwaysAddAllIterate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllIterate<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : IEnumerator? =>
        condition
            ? AlwaysAddAllIterate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllIterate<TEnumtr, TFmt>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : struct, IEnumerator<TFmt>
        where TFmt : ISpanFormattable? =>
        condition
            ? AlwaysAddAllIterate<TEnumtr, TFmt>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllIterate<TEnumtr, TFmt>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : IEnumerator<TFmt>?
        where TFmt : ISpanFormattable? =>
        condition
            ? AlwaysAddAllIterate<TEnumtr, TFmt>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllIterateNullable<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : struct, IEnumerator =>
        condition
            ? AlwaysAddAllIterateNullable(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllIterateNullable<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : IEnumerator? =>
        condition
            ? AlwaysAddAllIterateNullable(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllIterateNullable<TEnumtr, TFmtStruct>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : struct, IEnumerator<TFmtStruct?>
        where TFmtStruct : struct, ISpanFormattable =>
        condition
            ? AlwaysAddAllIterateNullable<TEnumtr, TFmtStruct>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllIterateNullable<TEnumtr, TFmtStruct>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : IEnumerator<TFmtStruct?>?
        where TFmtStruct : struct, ISpanFormattable =>
        condition
            ? AlwaysAddAllIterateNullable<TEnumtr, TFmtStruct>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetRevealAllIterate<TEnumtr, TRevealBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator 
        where TRevealBase : notnull =>
        condition
            ? AlwaysRevealAllIterate(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetRevealAllIterate<TEnumtr, TRevealBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator? 
        where TRevealBase : notnull =>
        condition
            ? AlwaysRevealAllIterate(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetRevealAllIterate<TEnumtr, TCloaked, TRevealBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TCloaked> 
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        condition
            ? AlwaysRevealAllIterate<TEnumtr, TCloaked, TRevealBase>(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetRevealAllIterate<TEnumtr, TCloaked, TRevealBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TCloaked>? 
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        condition
            ? AlwaysRevealAllIterate<TEnumtr, TCloaked, TRevealBase>(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetRevealAllIterateNullable<TEnumtr, TCloakedStruct>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : struct, IEnumerator<TCloakedStruct?> 
        where TCloakedStruct : struct =>
        condition
            ? AlwaysRevealAllIterateNullable(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetRevealAllIterateNullable<TEnumtr, TCloakedStruct>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : IEnumerator<TCloakedStruct?>? 
        where TCloakedStruct : struct =>
        condition
            ? AlwaysRevealAllIterateNullable(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetRevealAllIterate<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator  =>
        condition
            ? AlwaysRevealAllIterate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetRevealAllIterate<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?  =>
        condition
            ? AlwaysRevealAllIterate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetRevealAllIterate<TEnumtr, TBearer>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TBearer> 
        where TBearer : IStringBearer? =>
        condition
            ? AlwaysRevealAllIterate<TEnumtr, TBearer>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetRevealAllIterate<TEnumtr, TBearer>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TBearer>? 
        where TBearer : IStringBearer? =>
        condition
            ? AlwaysRevealAllIterate<TEnumtr, TBearer>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetRevealAllIterateNullable<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator  =>
        condition
            ? AlwaysRevealAllIterateNullable(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetRevealAllIterateNullable<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?  =>
        condition
            ? AlwaysRevealAllIterateNullable(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetRevealAllIterateNullable<TEnumtr, TBearerStruct>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TBearerStruct?>
        where TBearerStruct : struct, IStringBearer =>
        condition
            ? AlwaysRevealAllIterateNullable<TEnumtr, TBearerStruct>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetRevealAllIterateNullable<TEnumtr, TBearerStruct>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TBearerStruct?>? 
        where TBearerStruct : struct, IStringBearer =>
        condition
            ? AlwaysRevealAllIterateNullable<TEnumtr, TBearerStruct>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllIterateString<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<string?>  =>
        condition
            ? AlwaysAddAllIterateString(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllIterateString<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<string?>?  =>
        condition
            ? AlwaysAddAllIterateString(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllIterateCharSeq<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : struct, IEnumerator  =>
        condition
            ? AlwaysAddAllIterateCharSeq(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllIterateCharSeq<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : IEnumerator?  =>
        condition
            ? AlwaysAddAllIterateCharSeq(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllIterateCharSeq<TEnumtr, TCharSeq>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : struct, IEnumerator<TCharSeq> 
        where TCharSeq : ICharSequence? =>
        condition
            ? AlwaysAddAllIterateCharSeq<TEnumtr, TCharSeq>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllIterateCharSeq<TEnumtr, TCharSeq>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumtr : IEnumerator<TCharSeq>? 
        where TCharSeq : ICharSequence? =>
        condition
            ? AlwaysAddAllIterateCharSeq<TEnumtr, TCharSeq>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllIterateStringBuilder<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<StringBuilder?> =>
        condition
            ? AlwaysAddAllIterateStringBuilder(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllIterateStringBuilder<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<StringBuilder?>? =>
        condition
            ? AlwaysAddAllIterateStringBuilder(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllIterateMatch<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator =>
        condition
            ? AlwaysAddAllIterateMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllIterateMatch<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator? =>
        condition
            ? AlwaysAddAllIterateMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllIterateMatch<TEnumtr, TAny>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TAny> =>
        condition
            ? AlwaysAddAllIterateMatch<TEnumtr, TAny>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    public TMold WhenConditionMetAddAllIterateMatch<TEnumtr, TAny>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TAny>? =>
        condition
            ? AlwaysAddAllIterateMatch<TEnumtr, TAny>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumtr), fieldName, formatFlags);

    [CallsObjectToString]
    public TMold WhenConditionMetAddAllIterateObject<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<object?> =>
        WhenConditionMetAddAllIterateMatch<TEnumtr, object?>(condition, fieldName, value, formatString, formatFlags);

    [CallsObjectToString]
    public TMold WhenConditionMetAddAllIterateObject<TEnumtr>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<object?>? =>
        WhenConditionMetAddAllIterateMatch<TEnumtr, object?>(condition, fieldName, value, formatString, formatFlags);
}
