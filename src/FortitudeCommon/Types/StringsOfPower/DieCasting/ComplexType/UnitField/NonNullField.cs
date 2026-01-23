// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.UnitField;

public partial class SelectTypeField<TMold> where TMold : TypeMolder
{
    public TMold WhenNonNullAdd (ReadOnlySpan<char> fieldName, bool? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAdd(value != null, fieldName, value,  formatString, formatFlags);

    public TMold WhenNonNullAdd<TFmt> (ReadOnlySpan<char> fieldName, TFmt value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable? =>
        WhenConditionMetAdd(value != null, fieldName, value,  formatString, formatFlags);

    public TMold WhenNonNullAdd<TFmtStruct> (ReadOnlySpan<char> fieldName, TFmtStruct? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
        WhenConditionMetAdd(value != null, fieldName, value,  formatString, formatFlags);

    public TMold WhenNonNullReveal<TCloaked, TRevealBase>(ReadOnlySpan<char> fieldName, TCloaked value, PalantírReveal<TRevealBase> palantírReveal
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TCloaked : TRevealBase? 
        where TRevealBase : notnull =>
        WhenConditionMetReveal(!Equals(value, null), fieldName, value, palantírReveal,  formatString, formatFlags);
    
    public TMold WhenNonNullReveal<TCloakedStruct>(ReadOnlySpan<char> fieldName, TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct =>
        WhenConditionMetReveal(!Equals(value, null), fieldName, value, palantírReveal,  formatString, formatFlags);

    public TMold WhenNonNullReveal<TBearer>(ReadOnlySpan<char> fieldName, TBearer value
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer? =>
        WhenConditionMetReveal(!Equals(value, null), fieldName, value,  formatString, formatFlags);

    public TMold WhenNonNullReveal<TBearerStruct>(ReadOnlySpan<char> fieldName, TBearerStruct? value
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer =>
        WhenConditionMetReveal(!Equals(value, null), fieldName, value,  formatString, formatFlags);

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, Span<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAdd(value.Length > 0, fieldName, value,  formatString, formatFlags);

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAdd(value.Length > 0, fieldName, value,  formatString, formatFlags);

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAdd(value != null, fieldName, value,  formatString, formatFlags);

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, string? value, int startIndex, int count = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAdd(value != null, fieldName, value, startIndex, count,  formatString, formatFlags);

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, char[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAdd(value != null, fieldName, value,  formatString, formatFlags);

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, char[]? value, int startIndex, int count = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAdd(value != null, fieldName, value, startIndex, count,  formatString, formatFlags);

    public TMold WhenNonNullAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence? =>
        WhenConditionMetAddCharSeq(value != null, fieldName, value,  formatString, formatFlags);

    public TMold WhenNonNullAddCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, TCharSeq value, int startIndex, int count = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence? =>
        WhenConditionMetAddCharSeq(value != null, fieldName, value, startIndex, count,  formatString, formatFlags);

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) => 
        WhenConditionMetAdd(value != null, fieldName, value,  formatString, formatFlags);

    public TMold WhenNonNullAdd(ReadOnlySpan<char> fieldName, StringBuilder? value, int startIndex, int count = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) => 
        WhenConditionMetAdd(value != null, fieldName, value, startIndex, count,  formatString, formatFlags);
    
    public TMold WhenNonNullAddMatch<TAny>(ReadOnlySpan<char> fieldName, TAny value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddMatch(value != null, fieldName, value,  formatString, formatFlags);
    
    [CallsObjectToString]
    public TMold WhenNonNullAddObject(ReadOnlySpan<char> fieldName, object? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        WhenConditionMetAddObject(value != null, fieldName, value,  formatString, formatFlags);
}
