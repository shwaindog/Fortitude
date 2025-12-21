// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ValueType;

public class ValueTypeMold<TMold> : KnownTypeMolder<TMold> where TMold : TypeMolder
{
    public ValueTypeMold<TMold> InitializeValueTypeBuilder
    (
        Type typeBeingBuilt
      , ISecretStringOfPower master
      , MoldDieCastSettings typeSettings
      , string? typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId
      , FieldContentHandling createFormatFlags )
    {
        Initialize(typeBeingBuilt, master, typeSettings, typeName
                                       , remainingGraphDepth, typeFormatting, existingRefId, createFormatFlags);

        return this;
    }

    public override bool IsComplexType => false;

    protected ValueTypeDieCast<TMold> Stb => (ValueTypeDieCast<TMold>)MoldStateField!;

    public override void Start()
    {
        if (PortableState.AppenderSettings.SkipTypeParts.HasTypeStartFlag()) return;
        AppendOpening();
    }

    public override void AppendOpening()
    {
        if (IsComplexType)
            MoldStateField!.StyleFormatter.AppendComplexTypeOpening(MoldStateField);
        else
            MoldStateField!.StyleFormatter.AppendValueTypeOpening(MoldStateField);
    }

    public override void AppendClosing()
    {
        if (IsComplexType)
            MoldStateField!.StyleFormatter.AppendTypeClosing(MoldStateField);
        else
            MoldStateField!.StyleFormatter.AppendValueTypeClosing(MoldStateField);
    }

    public TMold AsValue(ReadOnlySpan<char> nonJsonfieldName, bool value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TMold AsValue(bool value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueNext("", value, formatString ?? "", formatFlags);

    public TMold AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, bool? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TMold AsValueOrNull(bool? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueNext("", value, formatString ?? "", formatFlags);

    public TMold AsValue<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) where TFmt : ISpanFormattable? =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, "0", formatString ?? "", formatFlags);

    public TMold AsValue<TFmt>(TFmt value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) where TFmt : ISpanFormattable? =>
        Stb.FieldValueOrDefaultNext("", value, "0", formatString ?? "", formatFlags);

    public TMold AsValueOrNull<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) where TFmt : class, ISpanFormattable =>
        Stb.FieldFmtValueOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TMold AsValueOrNull<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) where TFmtStruct : struct, ISpanFormattable =>
        Stb.FieldFmtValueOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TMold AsValueOrNull<TFmt>(TFmt? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent)
        where TFmt : ISpanFormattable =>
        Stb.FieldFmtValueOrNullNext("", value, formatString ?? "", formatFlags);

    public TMold AsValueOrNull<TFmt>(TFmt? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent)
        where TFmt : struct, ISpanFormattable =>
        Stb.FieldFmtValueOrNullNext("", value, formatString ?? "", formatFlags);

    public TMold AsValueOrDefault<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, TFmt defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) where TFmt : ISpanFormattable =>
        Stb.FieldFmtValueOrNullNext(nonJsonfieldName, value ?? defaultValue, formatString ?? "", formatFlags);

    public TMold AsValueOrDefault<TFmt>(TFmt? value, TFmt defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) where TFmt : ISpanFormattable =>
        Stb.FieldFmtValueOrNullNext("", value ?? defaultValue, formatString ?? "", formatFlags);

    public TMold AsValueOrDefault<TFmtStruct>(TFmtStruct? value, TFmtStruct defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) where TFmtStruct : struct, ISpanFormattable =>
        Stb.FieldFmtValueOrNullNext("", value ?? defaultValue, formatString ?? "", formatFlags);

    public TMold AsValueOrDefault<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value, TFmtStruct defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) where TFmtStruct : struct, ISpanFormattable =>
        Stb.FieldFmtValueOrNullNext(nonJsonfieldName, value ?? defaultValue, formatString ?? "", formatFlags);

    public TMold AsValueOrDefault<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, string defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) where TFmt : ISpanFormattable =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);

    public TMold AsValueOrDefault<TFmt>(TFmt? value, string defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) where TFmt : ISpanFormattable =>
        Stb.FieldValueOrDefaultNext("", value, defaultValue, formatString ?? "", formatFlags);

    public TMold AsValueOrDefault<TFmtStruct>(TFmtStruct? value, string defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent)
        where TFmtStruct : struct, ISpanFormattable =>
        Stb.FieldValueOrDefaultNext("", value, defaultValue, formatString ?? "", formatFlags);

    public TMold AsValueOrDefault<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value, string defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) where TFmtStruct : struct, ISpanFormattable =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);

    public TMold RevealAsValue<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked value
      , PalantírReveal<TCloakedBase> palantírReveal, string? formatString = null, FieldContentHandling formatFlags = AsValueContent) 
      where TCloaked : TCloakedBase
      where TCloakedBase : notnull =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, palantírReveal, formatString ?? "", formatFlags);

    public TMold RevealAsValue<TCloaked, TCloakedBase>(TCloaked value, PalantírReveal<TCloakedBase> palantírReveal
    , string? formatString = null, FieldContentHandling formatFlags = AsValueContent) 
      where TCloaked : TCloakedBase
      where TCloakedBase : notnull =>
        Stb.FieldValueOrNullNext("", value, palantírReveal, formatString ?? "", formatFlags);

    public TMold RevealAsValueOrNull<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal, string? formatString = null, FieldContentHandling formatFlags = AsValueContent) 
      where TCloaked : TCloakedBase
      where TCloakedBase : notnull =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, palantírReveal, formatString ?? "", formatFlags);

    public TMold RevealAsValueOrNull<TCloaked, TCloakedBase>(TCloaked? value, PalantírReveal<TCloakedBase> palantírReveal
    , string? formatString = null, FieldContentHandling formatFlags = AsValueContent)
        where TCloaked : TCloakedBase
        where TCloakedBase : notnull =>
        Stb.FieldValueOrNullNext("", value, palantírReveal, formatString ?? "", formatFlags);

    public TMold RevealAsValueOrDefault<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal, ReadOnlySpan<char> defaultValue
    , string? formatString = null, FieldContentHandling formatFlags = AsValueContent) 
      where TCloaked : TCloakedBase
      where TCloakedBase : notnull =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, palantírReveal, defaultValue, formatString ?? "", formatFlags);

    public TMold RevealAsValueOrDefault<TCloaked, TCloakedBase>(TCloaked? value, PalantírReveal<TCloakedBase> palantírReveal
      , ReadOnlySpan<char> defaultValue, string? formatString = null, FieldContentHandling formatFlags = AsValueContent) 
      where TCloaked : TCloakedBase
      where TCloakedBase : notnull =>
        Stb.FieldValueOrDefaultNext("", value, palantírReveal, defaultValue, formatString ?? "", formatFlags);

    public TMold RevealAsValue<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string? formatString = null, FieldContentHandling formatFlags = AsValueContent) 
      where TCloakedStruct : struct =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, palantírReveal, formatString ?? "", formatFlags);

    public TMold RevealAsValue<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
    , string? formatString = null, FieldContentHandling formatFlags = AsValueContent) where TCloakedStruct : struct =>
        Stb.FieldValueOrNullNext("", value, palantírReveal, formatString ?? "", formatFlags);

    public TMold RevealAsValueOrNull<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string? formatString = null, FieldContentHandling formatFlags = AsValueContent) 
        where TCloakedStruct : struct =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, palantírReveal, formatString ?? "", formatFlags);

    public TMold RevealAsValueOrNull<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FieldContentHandling formatFlags = AsValueContent) where TCloakedStruct : struct =>
        Stb.FieldValueOrNullNext("", value, palantírReveal, formatString ?? "", formatFlags);

    public TMold RevealAsValueOrDefault<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, ReadOnlySpan<char> defaultValue, string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent)
        where TCloakedStruct : struct =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, palantírReveal, defaultValue, formatFlags, formatString ?? "");

    public TMold RevealAsValueOrDefault<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
      , ReadOnlySpan<char> defaultValue, string? formatString = null, FieldContentHandling formatFlags = AsValueContent) 
        where TCloakedStruct : struct =>
        Stb.FieldValueOrDefaultNext("", value, palantírReveal, defaultValue, formatFlags, formatString ?? "");

    public TMold RevealAsValue<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer value, string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent)
        where TBearer : IStringBearer =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, formatFlags, formatString ?? "");

    public TMold RevealAsValue<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value, string? formatString = null
    , FieldContentHandling formatFlags = AsValueContent)
        where TBearer : struct, IStringBearer =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, formatFlags, formatString ?? "");

    public TMold RevealAsValueOrNull<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value
    , string? formatString = null, FieldContentHandling formatFlags = AsValueContent) where TBearer : IStringBearer =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, formatFlags, formatString ?? "");

    public TMold RevealAsValueOrNull<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value
      , string? formatString = null, FieldContentHandling formatFlags = AsValueContent) where TBearer : struct, IStringBearer =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, formatFlags, formatString ?? "");

    public TMold RevealAsValueOrDefault<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value, string defaultValue = ""
      , string? formatString = null, FieldContentHandling formatFlags = AsValueContent) where TBearer : IStringBearer =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, formatFlags, formatString ?? "");

    public TMold RevealAsValueOrDefault<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value, string defaultValue = ""
      , string? formatString = null, FieldContentHandling formatFlags = AsValueContent) where TBearer : struct, IStringBearer =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, formatFlags, formatString ?? "");

    public TMold AsValue(ReadOnlySpan<char> nonJsonfieldName, Span<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, "0", formatString ?? "", formatFlags);

    public TMold AsValueOrZero(Span<char> value, FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext("", value, "0", "", formatFlags);

    public TMold AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, Span<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TMold AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, Span<char> value, ReadOnlySpan<char> defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);

    public TMold AsValue(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, "0", formatString ?? "", formatFlags);

    public TMold AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TMold AsValueOrZero(ReadOnlySpan<char> value, FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext("", value, "0", "", formatFlags);

    public TMold AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value, ReadOnlySpan<char> defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);

    public TMold AsValue(Span<char> value, FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext("", value, "0", "", formatFlags);

    public TMold AsValue(ReadOnlySpan<char> value, FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext("", value, "0", "", formatFlags);

    public TMold AsValue(string value, FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext("", value, "0", "", formatFlags);

    public TMold AsValue(ReadOnlySpan<char> nonJsonfieldName, string value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, "0", formatString ?? "", formatFlags);

    public TMold AsValue(ReadOnlySpan<char> nonJsonfieldName, string value, int startIndex, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, "0", formatString ?? "", formatFlags);

    public TMold AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);

    public TMold AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length = int.MaxValue
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue, formatString ?? "", formatFlags);

    public TMold AsValue(ReadOnlySpan<char> nonJsonfieldName, char[] value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, 0, value?.Length ?? 0, "0", formatString ?? "", formatFlags);

    public TMold AsValue(ReadOnlySpan<char> nonJsonfieldName, char[] value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, "0", formatString ?? "", formatFlags);

    public TMold AsValue(char[] value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext("", value, startIndex, length, "0", formatString ?? "", formatFlags);

    public TMold AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);

    public TMold AsValueOrNull(char[]? value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrNullNext("", value, startIndex, length, formatString ?? "", formatFlags);

    public TMold AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex = 0, int length = int.MaxValue
      , string? defaultValue = null, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue ?? "0", formatString ?? "", formatFlags);

    public TMold AsValueOrDefault(char[]? value, int startIndex = 0, int length = int.MaxValue, string? defaultValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext("", value, startIndex, length, defaultValue ?? "0", formatString ?? "", formatFlags);

    public TMold AsValue(ReadOnlySpan<char> nonJsonfieldName, ICharSequence value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, 0, int.MaxValue, "0", formatString ?? "", formatFlags);

    public TMold AsValue(ICharSequence value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext("", value, 0, int.MaxValue, "0", formatString ?? "", formatFlags);

    public TMold AsValue(ReadOnlySpan<char> nonJsonfieldName, ICharSequence value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, "0", formatString ?? "", formatFlags);

    public TMold AsValue(ICharSequence value, int startIndex, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext("", value, startIndex, length, "0", formatString ?? "", formatFlags);

    public TMold AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);

    public TMold AsValueOrNull(ICharSequence? value, int startIndex, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrNullNext("", value, startIndex, length, formatString ?? "", formatFlags);

    public TMold AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex, int length = int.MaxValue
      , string? defaultValue = null, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue ?? "0", formatString ?? "", formatFlags);

    public TMold AsValueOrDefault(ICharSequence? value, int startIndex, int length = int.MaxValue, string? defaultValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext("", value, startIndex, length, defaultValue ?? "0", formatString ?? "", formatFlags);

    public TMold AsValue(ReadOnlySpan<char> nonJsonfieldName, StringBuilder value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, 0, int.MaxValue, "0", formatString ?? "", formatFlags);

    public TMold AsValue(StringBuilder value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext("", value, 0, int.MaxValue, "0", formatString ?? "", formatFlags);

    public TMold AsValue(ReadOnlySpan<char> nonJsonfieldName, StringBuilder value, int startIndex, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, "0", formatString ?? "", formatFlags);

    public TMold AsValue(StringBuilder value, int startIndex, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext("", value, startIndex, length, "0", formatString ?? "", formatFlags);

    public TMold AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);

    public TMold AsValueOrNull(StringBuilder? value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrNullNext("", value, startIndex, length, formatString ?? "", formatFlags);

    public TMold AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex = 0, int length = int.MaxValue
      , string? defaultValue = null, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue ?? "0", formatString ?? "", formatFlags);

    public TMold AsValueOrDefault(StringBuilder? value, int startIndex = 0, int length = int.MaxValue, string? defaultValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext("", value, startIndex, length, defaultValue ?? "0", formatString ?? "", formatFlags);

    public TMold AsValueMatch<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.ValueMatchOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TMold AsValueMatchOrNull<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.ValueMatchOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TMold AsValueMatchOrDefault<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny? value, string? defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.ValueMatchOrDefaultNext(nonJsonfieldName, value, defaultValue ?? "", formatString ?? "", formatFlags);

    public TMold AsString(ReadOnlySpan<char> nonJsonfieldName, bool value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TMold AsString(bool value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        AsString("", value, formatString, formatFlags);

    public TMold AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, bool? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TMold AsStringOrNull(bool? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        AsStringOrNull("", value, formatString ?? "", formatFlags);

    public TMold AsString<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) where TFmt : ISpanFormattable =>
        Stb.FieldStringOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TMold AsString<TFmt>(TFmt value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) where TFmt : ISpanFormattable =>
        AsString("", value, formatString, formatFlags);

    public TMold AsStringOrNull<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) where TFmt : class, ISpanFormattable =>
        Stb.FieldStringOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TMold AsStringOrNull<TFmt>(TFmt? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll)
        where TFmt : class, ISpanFormattable =>
        AsStringOrNull("", value, formatString, formatFlags);

    public TMold AsStringOrDefault<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, TFmt defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll)
        where TFmt : class, ISpanFormattable =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value ?? defaultValue, "", formatString ?? "", formatFlags);

    public TMold AsStringOrDefault<TFmt>(TFmt? value, TFmt defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) where TFmt : class, ISpanFormattable =>
        AsStringOrDefault("", value, defaultValue, formatString, formatFlags);

    public TMold AsStringOrDefault<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, string defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) where TFmt : class, ISpanFormattable =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);

    public TMold AsStringOrDefault<TFmt>(TFmt? value, string defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) where TFmt : class, ISpanFormattable =>
        AsStringOrDefault("", value, defaultValue, formatString, formatFlags);

    public TMold AsStringOrNull<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll)
        where TFmtStruct : struct, ISpanFormattable =>
        Stb.FieldStringOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TMold AsStringOrNull<TFmtStruct>(TFmtStruct? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) where TFmtStruct : struct, ISpanFormattable =>
        AsStringOrNull("", value, formatString, formatFlags);

    public TMold AsStringOrDefault<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value, TFmtStruct defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) where TFmtStruct : struct, ISpanFormattable =>
        Stb.FieldStringOrNullNext(nonJsonfieldName, value ?? defaultValue, formatString ?? "", formatFlags);

    public TMold AsStringOrDefault<TFmtStruct>(TFmtStruct? value, TFmtStruct defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll)
        where TFmtStruct : struct, ISpanFormattable =>
        AsStringOrDefault("", value, defaultValue, formatString, formatFlags);

    public TMold AsStringOrDefault<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value, string defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) where TFmtStruct : struct, ISpanFormattable =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);

    public TMold AsStringOrDefault<TFmtStruct>(TFmtStruct? value, string defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll)
        where TFmtStruct : struct, ISpanFormattable =>
        AsStringOrDefault("", value, defaultValue, formatString, formatFlags);

    public TMold RevealAsString<TCloaked, TCloakedBase>(TCloaked value, PalantírReveal<TCloakedBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll)
        where TCloaked : TCloakedBase
        where TCloakedBase : notnull =>
        RevealAsString("", value, palantírReveal, formatString, formatFlags);

    public TMold RevealAsString<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked value
      , PalantírReveal<TCloakedBase> palantírReveal, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll)
        where TCloaked : TCloakedBase
        where TCloakedBase : notnull =>
        Stb.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, palantírReveal, "", formatString, formatFlags);

    public TMold RevealAsStringOrNull<TCloaked, TCloakedBase>(TCloaked? value, PalantírReveal<TCloakedBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = EncodeAll)
        where TCloaked : TCloakedBase
        where TCloakedBase : notnull =>
        RevealAsStringOrNull("", value, palantírReveal, formatString, formatFlags);

    public TMold RevealAsStringOrNull<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll)
        where TCloaked : TCloakedBase
        where TCloakedBase : notnull =>
        Stb.FieldStringRevealOrNullNext(nonJsonfieldName, value, palantírReveal, formatString, formatFlags);

    public TMold RevealAsStringOrDefault<TCloaked, TCloakedBase>(TCloaked? value, PalantírReveal<TCloakedBase> palantírReveal
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll)
        where TCloaked : TCloakedBase
        where TCloakedBase : notnull =>
        RevealAsStringOrDefault("", value, palantírReveal, defaultValue, formatString, formatFlags);

    public TMold RevealAsStringOrDefault<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = EncodeAll)
        where TCloaked : TCloakedBase
        where TCloakedBase : notnull =>
        Stb.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, palantírReveal, defaultValue, formatString, formatFlags);

    public TMold RevealAsString<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = EncodeAll) 
      where TCloakedStruct : struct =>
        RevealAsString("", value, palantírReveal, formatString ?? "", formatFlags);

    public TMold RevealAsString<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll)
        where TCloakedStruct : struct =>
        Stb.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, palantírReveal, "", formatString ?? "", formatFlags);

    public TMold RevealAsStringOrNull<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = EncodeAll)
        where TCloakedStruct : struct =>
        RevealAsStringOrNull("", value, palantírReveal, formatString, formatFlags);

    public TMold RevealAsStringOrNull<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FieldContentHandling formatFlags = EncodeAll) where TCloakedStruct : struct =>
        Stb.FieldStringRevealOrNullNext(nonJsonfieldName, value, palantírReveal, formatString, formatFlags);

    public TMold RevealAsStringOrDefault<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FieldContentHandling formatFlags = EncodeAll) where TCloakedStruct : struct =>
        RevealAsStringOrDefault("", value, palantírReveal, defaultValue, formatString, formatFlags);

    public TMold RevealAsStringOrDefault<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string defaultValue = "", string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll)
        where TCloakedStruct : struct =>
        Stb.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, palantírReveal, defaultValue, formatString ?? "", formatFlags);

    public TMold RevealAsString<TBearer>(TBearer value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FieldContentHandling formatFlags = EncodeAll) 
        where TBearer : IStringBearer =>
        Stb.FieldStringRevealOrDefaultNext("", value, "", formatString ?? "", formatFlags);

    public TMold RevealAsString<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = EncodeAll) 
      where TBearer : IStringBearer =>
        Stb.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, "", formatString ?? "", formatFlags);

    public TMold RevealAsStringOrNull<TBearer>(TBearer? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FieldContentHandling formatFlags = EncodeAll)
        where TBearer : IStringBearer =>
        Stb.FieldStringRevealOrNullNext("", value, formatFlags);

    public TMold RevealAsStringOrNull<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = EncodeAll) 
      where TBearer : IStringBearer =>
        Stb.FieldStringRevealOrNullNext(nonJsonfieldName, value, formatFlags);

    public TMold RevealAsStringOrDefault<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = EncodeAll)
        where TBearer : IStringBearer => Stb.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);

    public TMold RevealAsStringOrDefault<TBearer>(TBearer? value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = EncodeAll)
        where TBearer : IStringBearer => Stb.FieldStringRevealOrDefaultNext("", value, defaultValue, formatString ?? "", formatFlags);

    public TMold RevealAsString<TBearerStruct>(TBearerStruct? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
    , FieldContentHandling formatFlags = EncodeAll)
        where TBearerStruct : struct, IStringBearer =>
        Stb.FieldStringRevealOrDefaultNext("", value, "", formatFlags, formatString ?? "");

    public TMold RevealAsString<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = EncodeAll) 
      where TBearerStruct : struct, IStringBearer =>
        Stb.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, "", formatFlags, formatString ?? "");

    public TMold RevealAsStringOrNull<TBearerStruct>(TBearerStruct? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = EncodeAll)
        where TBearerStruct : struct, IStringBearer =>
        Stb.FieldStringRevealOrNullNext("", value, formatFlags, formatString ?? "");

    public TMold RevealAsStringOrNull<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = EncodeAll) 
        where TBearerStruct : struct, IStringBearer =>
        Stb.FieldStringRevealOrNullNext(nonJsonfieldName, value, formatFlags, formatString ?? "");

    public TMold RevealAsStringOrDefault<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = EncodeAll)
        where TBearerStruct : struct, IStringBearer =>
        Stb.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, defaultValue, formatFlags, formatString ?? "");

    public TMold RevealAsStringOrDefault<TBearerStruct>(TBearerStruct? value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = EncodeAll)
        where TBearerStruct : struct, IStringBearer =>
        Stb.FieldStringRevealOrDefaultNext("", value, defaultValue, formatFlags, formatString ?? "");

    public TMold AsString(ReadOnlySpan<char> nonJsonfieldName, Span<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, "", formatString ?? "", formatFlags);

    public TMold AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, Span<char> value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);

    public TMold AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, Span<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TMold AsStringOrNull(Span<char> value, FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringNext("", value, formatFlags);

    public TMold AsString(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, "", formatString ?? "", formatFlags);

    public TMold AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);

    public TMold AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TMold AsStringOrNull(ReadOnlySpan<char> value, FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrNullNext("", value, "", formatFlags);

    public TMold AsString(Span<char> value, FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext("", value, "", "", formatFlags);

    public TMold AsString(ReadOnlySpan<char> value, FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext("", value, "", "", formatFlags);

    public TMold AsString(string value, FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext("", value, "", "", formatFlags);

    public TMold AsString(ReadOnlySpan<char> nonJsonfieldName, string value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, "", formatString ?? "", formatFlags);

    public TMold AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TMold AsString(ReadOnlySpan<char> nonJsonfieldName, string value, int startIndex, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, "", formatString ?? "", formatFlags);

    public TMold AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);

    public TMold AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length = int.MaxValue
      , string? defaultValue = null, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue ?? "", formatString ?? "", formatFlags);

    public TMold AsString(ReadOnlySpan<char> nonJsonfieldName, char[] value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, 0, int.MaxValue, "", formatString ?? "", formatFlags);

    public TMold AsString(ReadOnlySpan<char> nonJsonfieldName, char[] value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, "", formatString ?? "", formatFlags);

    public TMold AsString(char[] value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext("", value, startIndex, length, "", formatString ?? "", formatFlags);

    public TMold AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);

    public TMold AsStringOrNull(char[]? value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrNullNext("", value, startIndex, length, formatString ?? "", formatFlags);

    public TMold AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex = 0, int length = int.MaxValue
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue, formatString ?? "", formatFlags);

    public TMold AsStringOrDefault(char[]? value, int startIndex = 0, int length = int.MaxValue, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext("", value, startIndex, length, defaultValue, formatString ?? "", formatFlags);

    public TMold AsString(ReadOnlySpan<char> nonJsonfieldName, ICharSequence value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, 0, int.MaxValue, "", formatString ?? "", formatFlags);

    public TMold AsString(ICharSequence value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext("", value, 0, int.MaxValue, "", formatString ?? "", formatFlags);

    public TMold AsString(ReadOnlySpan<char> nonJsonfieldName, ICharSequence value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, "", formatString ?? "", formatFlags);

    public TMold AsString(ICharSequence value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext("", value, startIndex, length, "", formatString ?? "", formatFlags);

    public TMold AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);

    public TMold AsStringOrNull(ICharSequence? value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrNullNext("", value, startIndex, length, formatString ?? "", formatFlags);

    public TMold AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex = 0, int length = int.MaxValue
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue, formatString ?? "", formatFlags);

    public TMold AsStringOrDefault(ICharSequence? value, int startIndex = 0, int length = int.MaxValue, string fallbackValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext("", value, startIndex, length, fallbackValue, formatString ?? "", formatFlags);

    public TMold AsString(ReadOnlySpan<char> nonJsonfieldName, StringBuilder value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, 0, int.MaxValue, "", formatString ?? "", formatFlags);

    public TMold AsString(StringBuilder value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext("", value, 0, int.MaxValue, "", formatString ?? "", formatFlags);

    public TMold AsString(ReadOnlySpan<char> nonJsonfieldName, StringBuilder value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, "", formatString ?? "", formatFlags);

    public TMold AsString(StringBuilder value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext("", value, startIndex, length, "", formatString ?? "", formatFlags);

    public TMold AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);

    public TMold AsStringOrNull(StringBuilder? value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrNullNext("", value, startIndex, length, formatString ?? "", formatFlags);

    public TMold AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex = 0, int length = int.MaxValue
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue, formatString ?? "", formatFlags);

    public TMold AsStringOrDefault(StringBuilder? value, int startIndex = 0, int length = int.MaxValue, string fallbackValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext("", value, startIndex, length, fallbackValue, formatString ?? "", formatFlags);

    public TMold AsStringMatch<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.StringMatchOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TMold AsStringMatchOrNull<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.StringMatchOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TMold AsStringMatchOrDefault<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny? value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.StringMatchOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);

    public IScopeDelimitedStringBuilder StartDelimitedStringBuilder() => Stb.StartDelimitedStringBuilder();
}
