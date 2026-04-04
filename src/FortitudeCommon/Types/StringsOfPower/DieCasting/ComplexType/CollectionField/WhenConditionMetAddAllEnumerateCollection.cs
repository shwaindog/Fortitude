// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.CollectionField;

public partial class SelectTypeCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenConditionMetAddAllEnumerateBool<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<bool> =>
        condition
            ? AlwaysAddAllEnumerateBool(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateBool<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<bool>? =>
        condition
            ? AlwaysAddAllEnumerateBool(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateNullableBool<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<bool?> =>
        condition
            ? AlwaysAddAllEnumerateNullableBool(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateNullableBool<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<bool?>? =>
        condition
            ? AlwaysAddAllEnumerateNullableBool(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerate<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable =>
        condition
            ? AlwaysAddAllEnumerate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerate<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable? =>
        condition
            ? AlwaysAddAllEnumerate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerate<TEnumbl, TFmt>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TFmt>
        where TFmt : ISpanFormattable? =>
        condition
            ? AlwaysAddAllEnumerate<TEnumbl, TFmt>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerate<TEnumbl, TFmt>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TFmt>?
        where TFmt : ISpanFormattable? =>
        condition
            ? AlwaysAddAllEnumerate<TEnumbl, TFmt>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateNullable<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable =>
        condition
            ? AlwaysAddAllEnumerateNullable(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateNullable<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable? =>
        condition
            ? AlwaysAddAllEnumerateNullable(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateNullable<TEnumbl, TFmtStruct>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TFmtStruct?>
        where TFmtStruct : struct, ISpanFormattable =>
        condition
            ? AlwaysAddAllEnumerateNullable<TEnumbl, TFmtStruct>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateNullable<TEnumbl, TFmtStruct>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TFmtStruct?>?
        where TFmtStruct : struct, ISpanFormattable =>
        condition
            ? AlwaysAddAllEnumerateNullable<TEnumbl, TFmtStruct>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAllEnumerate<TEnumbl, TRevealBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TRevealBase : notnull =>
        condition
            ? AlwaysRevealAllEnumerate(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAllEnumerate<TEnumbl, TRevealBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TRevealBase : notnull =>
        condition
            ? AlwaysRevealAllEnumerate(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAllEnumerate<TEnumbl, TCloaked, TRevealBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TCloaked>
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        condition
            ? AlwaysRevealAllEnumerate<TEnumbl, TCloaked, TRevealBase>(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAllEnumerate<TEnumbl, TCloaked, TRevealBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCloaked>?
        where TCloaked : TRevealBase?
        where TRevealBase : notnull =>
        condition
            ? AlwaysRevealAllEnumerate<TEnumbl, TCloaked, TRevealBase>(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAllEnumerateNullable<TEnumbl, TCloakedStruct>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TCloakedStruct?>
        where TCloakedStruct : struct =>
        condition
            ? AlwaysRevealAllEnumerateNullable(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAllEnumerateNullable<TEnumbl, TCloakedStruct>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCloakedStruct?>?
        where TCloakedStruct : struct =>
        condition
            ? AlwaysRevealAllEnumerateNullable(fieldName, value, palantírReveal, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAllEnumerate<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable =>
        condition
            ? AlwaysRevealAllEnumerate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAllEnumerate<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable? =>
        condition
            ? AlwaysRevealAllEnumerate(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAllEnumerate<TEnumbl, TBearer>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TBearer>
        where TBearer : IStringBearer? =>
        condition
            ? AlwaysRevealAllEnumerate<TEnumbl, TBearer>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAllEnumerate<TEnumbl, TBearer>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TBearer>?
        where TBearer : IStringBearer? =>
        condition
            ? AlwaysRevealAllEnumerate<TEnumbl, TBearer>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAllEnumerateNullable<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable =>
        condition
            ? AlwaysRevealAllEnumerateNullable(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAllEnumerateNullable<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable? =>
        condition
            ? AlwaysRevealAllEnumerateNullable(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAllEnumerateNullable<TEnumbl, TBearerStruct>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TBearerStruct?>
        where TBearerStruct : struct, IStringBearer =>
        condition
            ? AlwaysRevealAllEnumerateNullable<TEnumbl, TBearerStruct>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetRevealAllEnumerateNullable<TEnumbl, TBearerStruct>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TBearerStruct?>?
        where TBearerStruct : struct, IStringBearer =>
        condition
            ? AlwaysRevealAllEnumerateNullable<TEnumbl, TBearerStruct>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateString<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<string?> =>
        condition
            ? AlwaysAddAllStringEnumerateString(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateString<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<string?>? =>
        condition
            ? AlwaysAddAllStringEnumerateString(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateCharSeq<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : struct, IEnumerable =>
        condition
            ? AlwaysAddAllEnumerateCharSeq(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateCharSeq<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : IEnumerable? =>
        condition
            ? AlwaysAddAllEnumerateCharSeq(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateCharSeq<TEnumbl, TCharSeq>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : struct, IEnumerable<TCharSeq>
        where TCharSeq : ICharSequence? =>
        condition
            ? AlwaysAddAllEnumerateCharSeq<TEnumbl, TCharSeq>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateCharSeq<TEnumbl, TCharSeq>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : IEnumerable<TCharSeq>?
        where TCharSeq : ICharSequence? =>
        condition
            ? AlwaysAddAllEnumerateCharSeq<TEnumbl, TCharSeq>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateStringBuilder<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<StringBuilder?> =>
        condition
            ? AlwaysAddAllEnumerateStringBuilder(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateStringBuilder<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<StringBuilder?>? =>
        condition
            ? AlwaysAddAllEnumerateStringBuilder(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateMatch<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable =>
        condition
            ? AlwaysAddAllEnumerateMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateMatch<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable? =>
        condition
            ? AlwaysAddAllEnumerateMatch(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateMatch<TEnumbl, TAny>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TAny> =>
        condition
            ? AlwaysAddAllEnumerateMatch<TEnumbl, TAny>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateMatch<TEnumbl, TAny>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TAny>? =>
        condition
            ? AlwaysAddAllEnumerateMatch<TEnumbl, TAny>(fieldName, value, formatString, formatFlags)
            : stb.WasSkipped(typeof(TEnumbl), fieldName, formatFlags);

    [CallsObjectToString]
    public TExt WhenConditionMetAddAllEnumerateObject<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<object?> =>
        WhenConditionMetAddAllEnumerateMatch<TEnumbl, object?>(condition, fieldName, value, formatString, formatFlags);

    [CallsObjectToString]
    public TExt WhenConditionMetAddAllEnumerateObject<TEnumbl>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<object?>? =>
        WhenConditionMetAddAllEnumerateMatch<TEnumbl, object?>(condition, fieldName, value, formatString, formatFlags);
}
