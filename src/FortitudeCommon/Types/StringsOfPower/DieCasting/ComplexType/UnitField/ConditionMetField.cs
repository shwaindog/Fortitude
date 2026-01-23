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
    public TMold WhenConditionMetAdd (bool condition, ReadOnlySpan<char> fieldName, bool? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAdd(fieldName, value, formatString ?? "", formatFlags) 
            : stb.WasSkipped(typeof(bool), fieldName, formatFlags);

    public TMold WhenConditionMetAdd<TFmt> (bool condition, ReadOnlySpan<char> fieldName, TFmt value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable? =>
      condition
            ? AlwaysAdd(fieldName, value, formatString ?? "", formatFlags) 
            : stb.WasSkipped(value?.GetType() ?? typeof(TFmt), fieldName, formatFlags);

    public TMold WhenConditionMetAdd<TFmtStruct> (bool condition, ReadOnlySpan<char> fieldName, TFmtStruct? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable =>
      condition
            ? AlwaysAdd(fieldName, value, formatString ?? "", formatFlags) 
            : stb.WasSkipped(typeof(TFmtStruct?), fieldName, formatFlags);

    public TMold WhenConditionMetReveal<TCloaked, TRevealBase>(bool condition, ReadOnlySpan<char> fieldName, TCloaked value
      , PalantírReveal<TRevealBase> palantírReveal, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TCloaked : TRevealBase? 
        where TRevealBase : notnull =>
      condition
            ? AlwaysReveal(fieldName, value, palantírReveal, formatString ?? "", formatFlags) 
            : stb.WasSkipped(value?.GetType() ?? typeof(TCloaked), fieldName, formatFlags);
    
    public TMold WhenConditionMetReveal<TCloakedStruct>(bool condition, ReadOnlySpan<char> fieldName, TCloakedStruct? value
    , PalantírReveal<TCloakedStruct> palantírReveal, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TCloakedStruct : struct =>
      condition
            ? AlwaysReveal(fieldName, value, palantírReveal, formatString ?? "", formatFlags) 
            : stb.WasSkipped(typeof(TCloakedStruct?), fieldName, formatFlags);

    public TMold WhenConditionMetReveal<TBearer>(bool condition, ReadOnlySpan<char> fieldName, TBearer value
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer? =>
      condition
            ? AlwaysReveal(fieldName, value, formatString ?? "", formatFlags) 
            : stb.WasSkipped(value?.GetType() ?? typeof(TBearer), fieldName, formatFlags);

    public TMold WhenConditionMetReveal<TBearerStruct>(bool condition, ReadOnlySpan<char> fieldName, TBearerStruct? value
    , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer =>
      condition
            ? AlwaysReveal(fieldName, value, formatString ?? "", formatFlags) 
            : stb.WasSkipped(typeof(TBearerStruct?), fieldName, formatFlags);

    public TMold WhenConditionMetAdd(bool condition, ReadOnlySpan<char> fieldName, Span<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      condition
            ? AlwaysAdd(fieldName, value, formatString ?? "", formatFlags) 
            : stb.WasSkipped(typeof(Span<char>), fieldName, formatFlags);

    public TMold WhenConditionMetAdd(bool condition, ReadOnlySpan<char> fieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      condition
            ? AlwaysAdd(fieldName, value, formatString ?? "", formatFlags) 
            : stb.WasSkipped(typeof(ReadOnlySpan<char>), fieldName, formatFlags);

    public TMold WhenConditionMetAdd(bool condition, ReadOnlySpan<char> fieldName, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      condition
            ? AlwaysAdd(fieldName, value, formatString ?? "", formatFlags) 
            : stb.WasSkipped(typeof(string), fieldName, formatFlags);

    public TMold WhenConditionMetAdd(bool condition, ReadOnlySpan<char> fieldName, string? value, int startIndex, int count = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      condition
            ? AlwaysAdd(fieldName, value, startIndex, count, formatString ?? "", formatFlags) 
            : stb.WasSkipped(typeof(string), fieldName, formatFlags);

    public TMold WhenConditionMetAdd(bool condition, ReadOnlySpan<char> fieldName, char[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      condition
            ? AlwaysAdd(fieldName, value, formatString ?? "", formatFlags) 
            : stb.WasSkipped(typeof(char[]), fieldName, formatFlags);

    public TMold WhenConditionMetAdd(bool condition, ReadOnlySpan<char> fieldName, char[]? value, int startIndex, int count = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      condition
            ? AlwaysAdd(fieldName, value, startIndex, count, formatString ?? "", formatFlags) 
            : stb.WasSkipped(typeof(char[]), fieldName, formatFlags);

    public TMold WhenConditionMetAddCharSeq<TCharSeq>(bool condition, ReadOnlySpan<char> fieldName, TCharSeq value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence? =>
      condition
            ? AlwaysAddCharSeq(fieldName, value, formatString ?? "", formatFlags) 
            : stb.WasSkipped(value?.GetType() ?? typeof(TCharSeq), fieldName, formatFlags);

    public TMold WhenConditionMetAddCharSeq<TCharSeq>(bool condition, ReadOnlySpan<char> fieldName, TCharSeq value, int startIndex
      , int count = int.MaxValue, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCharSeq : ICharSequence? =>
      condition
            ? AlwaysAddCharSeq(fieldName, value, startIndex, count, formatString ?? "", formatFlags) 
            : stb.WasSkipped(value?.GetType() ?? typeof(TCharSeq), fieldName, formatFlags);

    public TMold WhenConditionMetAdd(bool condition, ReadOnlySpan<char> fieldName, StringBuilder? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) => 
        condition
            ? AlwaysAdd(fieldName, value, formatString ?? "", formatFlags) 
            : stb.WasSkipped(typeof(StringBuilder), fieldName, formatFlags);

    public TMold WhenConditionMetAdd(bool condition, ReadOnlySpan<char> fieldName, StringBuilder? value, int startIndex, int count = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) => 
        condition
            ? AlwaysAdd(fieldName, value, startIndex, count, formatString ?? "", formatFlags) 
            : stb.WasSkipped(typeof(StringBuilder), fieldName, formatFlags);
    
    public TMold WhenConditionMetAddMatch<TAny>(bool condition, ReadOnlySpan<char> fieldName, TAny value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddMatch(fieldName, value, formatString ?? "", formatFlags) 
            : stb.WasSkipped(value?.GetType() ?? typeof(TAny), fieldName, formatFlags);
    
    [CallsObjectToString]
    public TMold WhenConditionMetAddObject(bool condition, ReadOnlySpan<char> fieldName, object? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        condition
            ? AlwaysAddObject(fieldName, value, formatString ?? "", formatFlags) 
            : stb.WasSkipped(value?.GetType() ?? typeof(object), fieldName, formatFlags);
}
