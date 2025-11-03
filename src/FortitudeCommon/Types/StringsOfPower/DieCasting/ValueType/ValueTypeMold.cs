// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ValueType;

public class ValueTypeMold<TExt> : KnownTypeMolder<TExt> where TExt : TypeMolder
{
    public ValueTypeMold<TExt> InitializeValueTypeBuilder
    (
        Type typeBeingBuilt
      , ISecretStringOfPower master
      , MoldDieCastSettings typeSettings
      , string typeName
      , int remainingGraphDepth
      , IStyledTypeFormatting typeFormatting
      , int existingRefId)
    {
        InitializeTypedStyledTypeBuilder(typeBeingBuilt, master, typeSettings, typeName, remainingGraphDepth, typeFormatting, existingRefId);

        return this;
    }

    public override bool IsComplexType => false;

    protected ValueTypeDieCast<TExt> Stb => (ValueTypeDieCast<TExt>)CompAccess;

    public override void Start()
    {
        if (PortableState.AppenderSettings.SkipTypeParts.HasTypeStartFlag()) return;
        AppendOpening();
    }

    public override void AppendOpening()
    {
        if (IsComplexType)
            CompAccess.StyleFormatter.AppendComplexTypeOpening(CompAccess.Sb, CompAccess.TypeBeingBuilt, CompAccess.TypeName);
        else
            CompAccess.StyleFormatter.AppendValueTypeOpening(CompAccess.Sb, CompAccess.TypeBeingBuilt);
    }

    public override void AppendClosing()
    {
        if (IsComplexType)
            CompAccess.StyleFormatter.AppendTypeClosing(CompAccess.Sb);
        else
            CompAccess.StyleFormatter.AppendValueTypeClosing(CompAccess.Sb, CompAccess.TypeBeingBuilt);
    }

    public TExt AsValue(ReadOnlySpan<char> nonJsonfieldName, bool value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TExt AsValue(bool value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueNext("", value, formatString ?? "", formatFlags);

    public TExt AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, bool? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TExt AsValueOrNull(bool? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueNext("", value, formatString ?? "", formatFlags);

    public TExt AsValue<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) where TFmt : ISpanFormattable =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, "0", formatString ?? "");

    public TExt AsValue<TFmt>(TFmt value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) where TFmt : ISpanFormattable =>
        Stb.FieldValueOrDefaultNext("", value, "0", formatString ?? "", formatFlags);

    public TExt AsValueOrNull<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) where TFmt : class, ISpanFormattable =>
        Stb.FieldFmtValueOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TExt AsValueOrNull<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) where TFmtStruct : struct, ISpanFormattable =>
        Stb.FieldFmtValueOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TExt AsValueOrNull<TFmt>(TFmt? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) 
        where TFmt : ISpanFormattable =>
        Stb.FieldFmtValueOrNullNext("", value, formatString ?? "", formatFlags);

    public TExt AsValueOrNull<TFmt>(TFmt? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) 
        where TFmt : struct, ISpanFormattable =>
        Stb.FieldFmtValueOrNullNext("", value, formatString ?? "", formatFlags);

    public TExt AsValueOrDefault<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, TFmt defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) where TFmt : ISpanFormattable =>
        Stb.FieldFmtValueOrNullNext(nonJsonfieldName, value ?? defaultValue, formatString ?? "", formatFlags);

    public TExt AsValueOrDefault<TFmt>(TFmt? value, TFmt defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) where TFmt : ISpanFormattable =>
        Stb.FieldFmtValueOrNullNext("", value ?? defaultValue, formatString ?? "", formatFlags);

    public TExt AsValueOrDefault<TFmtStruct>(TFmtStruct? value, TFmtStruct defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) where TFmtStruct : struct, ISpanFormattable =>
        Stb.FieldFmtValueOrNullNext("", value ?? defaultValue, formatString ?? "", formatFlags);

    public TExt AsValueOrDefault<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value, TFmtStruct defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) where TFmtStruct : struct, ISpanFormattable =>
        Stb.FieldFmtValueOrNullNext(nonJsonfieldName, value ?? defaultValue, formatString ?? "", formatFlags);

    public TExt AsValueOrDefault<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) where TFmt : ISpanFormattable =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);

    public TExt AsValueOrDefault<TFmt>(TFmt? value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) where TFmt : ISpanFormattable =>
        Stb.FieldValueOrDefaultNext("", value, defaultValue, formatString ?? "", formatFlags);

    public TExt AsValueOrDefault<TFmtStruct>(TFmtStruct? value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent)
        where TFmtStruct : struct, ISpanFormattable =>
        Stb.FieldValueOrDefaultNext("", value, defaultValue, formatString ?? "", formatFlags);

    public TExt AsValueOrDefault<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) where TFmtStruct : struct, ISpanFormattable =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);

    public TExt RevealAsValue<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked value
      , PalantírReveal<TCloakedBase> palantírReveal, FieldContentHandling formatFlags = AsValueContent) where TCloaked : TCloakedBase =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, palantírReveal, formatFlags);

    public TExt RevealAsValue<TCloaked, TCloakedBase>(TCloaked value, PalantírReveal<TCloakedBase> palantírReveal
      , FieldContentHandling formatFlags = AsValueContent) where TCloaked : TCloakedBase =>
        Stb.FieldValueOrNullNext("", value, palantírReveal, formatFlags);

    public TExt RevealAsValueOrNull<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal, FieldContentHandling formatFlags = AsValueContent) where TCloaked : TCloakedBase =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, palantírReveal, formatFlags);

    public TExt RevealAsValueOrNull<TCloaked, TCloakedBase>(TCloaked? value, PalantírReveal<TCloakedBase> palantírReveal
      , FieldContentHandling formatFlags = AsValueContent)
        where TCloaked : TCloakedBase =>
        Stb.FieldValueOrNullNext("", value, palantírReveal, formatFlags);

    public TExt RevealAsValueOrDefault<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal, ReadOnlySpan<char> defaultValue
      , FieldContentHandling formatFlags = AsValueContent) where TCloaked : TCloakedBase =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, palantírReveal, defaultValue, formatFlags);

    public TExt RevealAsValueOrDefault<TCloaked, TCloakedBase>(TCloaked? value, PalantírReveal<TCloakedBase> palantírReveal
      , ReadOnlySpan<char> defaultValue, FieldContentHandling formatFlags = AsValueContent) where TCloaked : TCloakedBase =>
        Stb.FieldValueOrDefaultNext("", value, palantírReveal, defaultValue, formatFlags);

    public TExt RevealAsValue<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, FieldContentHandling formatFlags = AsValueContent) where TCloakedStruct : struct =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, palantírReveal, formatFlags);

    public TExt RevealAsValue<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
      , FieldContentHandling formatFlags = AsValueContent) where TCloakedStruct : struct =>
        Stb.FieldValueOrNullNext("", value, palantírReveal, formatFlags);

    public TExt RevealAsValueOrNull<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, FieldContentHandling formatFlags = AsValueContent) where TCloakedStruct : struct =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, palantírReveal, formatFlags);

    public TExt RevealAsValueOrNull<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
      , FieldContentHandling formatFlags = AsValueContent) where TCloakedStruct : struct =>
        Stb.FieldValueOrNullNext("", value, palantírReveal, formatFlags);

    public TExt RevealAsValueOrDefault<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, ReadOnlySpan<char> defaultValue, FieldContentHandling formatFlags = AsValueContent) 
        where TCloakedStruct : struct =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, palantírReveal, defaultValue, formatFlags);

    public TExt RevealAsValueOrDefault<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
      , ReadOnlySpan<char> defaultValue, FieldContentHandling formatFlags = AsValueContent) where TCloakedStruct : struct =>
        Stb.FieldValueOrDefaultNext("", value, palantírReveal, defaultValue, formatFlags);

    public TExt RevealAsValue<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer value, FieldContentHandling formatFlags = AsValueContent) 
        where TBearer : IStringBearer =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, formatFlags);

    public TExt RevealAsValue<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value, FieldContentHandling formatFlags = AsValueContent) 
        where TBearer : struct, IStringBearer =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, formatFlags);

    public TExt RevealAsValueOrNull<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value
      , FieldContentHandling formatFlags = AsValueContent) where TBearer : IStringBearer =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, formatFlags);

    public TExt RevealAsValueOrNull<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value
      , FieldContentHandling formatFlags = AsValueContent) where TBearer : struct, IStringBearer =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, formatFlags);

    public TExt RevealAsValueOrDefault<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value, string defaultValue = ""
      , FieldContentHandling formatFlags = AsValueContent) where TBearer : IStringBearer =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, formatFlags);

    public TExt RevealAsValueOrDefault<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value, string defaultValue = ""
      , FieldContentHandling formatFlags = AsValueContent) where TBearer : struct, IStringBearer =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, formatFlags);

    public TExt AsValue(ReadOnlySpan<char> nonJsonfieldName, Span<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, "0", formatString ?? "", formatFlags);

    public TExt AsValueOrZero(Span<char> value, FieldContentHandling formatFlags = AsValueContent) => 
        Stb.FieldValueOrDefaultNext("", value, "0", "",  formatFlags);

    public TExt AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, Span<char> value
        , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TExt AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, Span<char> value, ReadOnlySpan<char> defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);

    public TExt AsValue(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, "0", formatString ?? "", formatFlags);

    public TExt AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TExt AsValueOrZero(ReadOnlySpan<char> value, FieldContentHandling formatFlags = AsValueContent) => 
        Stb.FieldValueOrDefaultNext("", value, "0", "",  formatFlags);

    public TExt AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value, ReadOnlySpan<char> defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);

    public TExt AsValue(Span<char> value, FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext("", value, "0", "", formatFlags);

    public TExt AsValue(ReadOnlySpan<char> value, FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext("", value, "0", "", formatFlags);

    public TExt AsValue(string value, FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext("", value, "0", "", formatFlags);

    public TExt AsValue(ReadOnlySpan<char> nonJsonfieldName, string value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, "0", formatString ?? "", formatFlags);

    public TExt AsValue(ReadOnlySpan<char> nonJsonfieldName, string value, int startIndex, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, "0", formatString ?? "", formatFlags);

    public TExt AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);

    public TExt AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length = int.MaxValue
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue, formatString ?? "", formatFlags);

    public TExt AsValue(ReadOnlySpan<char> nonJsonfieldName, char[] value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, 0, value?.Length ?? 0, "0", formatString ?? "", formatFlags);

    public TExt AsValue(ReadOnlySpan<char> nonJsonfieldName, char[] value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, "0", formatString ?? "", formatFlags);

    public TExt AsValue(char[] value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext("", value, startIndex, length, "0", formatString ?? "", formatFlags);

    public TExt AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);

    public TExt AsValueOrNull(char[]? value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrNullNext("", value, startIndex, length, formatString ?? "", formatFlags);

    public TExt AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex = 0, int length = int.MaxValue
      , string? defaultValue = null , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue ?? "0", formatString ?? "", formatFlags);

    public TExt AsValueOrDefault(char[]? value, int startIndex = 0, int length = int.MaxValue, string? defaultValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext("", value, startIndex, length, defaultValue ?? "0", formatString ?? "", formatFlags);

    public TExt AsValue(ReadOnlySpan<char> nonJsonfieldName, ICharSequence value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, 0, int.MaxValue, "0", formatString ?? "", formatFlags);

    public TExt AsValue(ICharSequence value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext("", value, 0, int.MaxValue, "0", formatString ?? "", formatFlags);

    public TExt AsValue(ReadOnlySpan<char> nonJsonfieldName, ICharSequence value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, "0", formatString ?? "", formatFlags);

    public TExt AsValue(ICharSequence value, int startIndex = 0, int length = int.MaxValue 
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext("", value, startIndex, length, "0", formatString ?? "", formatFlags);

    public TExt AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);

    public TExt AsValueOrNull(ICharSequence? value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrNullNext("", value, startIndex, length, formatString ?? "", formatFlags);

    public TExt AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex = 0, int length = int.MaxValue
      , string? defaultValue = null, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue ?? "0", formatString ?? "", formatFlags);

    public TExt AsValueOrDefault(ICharSequence? value, int startIndex = 0, int length = int.MaxValue, string? defaultValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext("", value, startIndex, length, defaultValue ?? "0", formatString ?? "", formatFlags);

    public TExt AsValue(ReadOnlySpan<char> nonJsonfieldName, StringBuilder value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, 0, int.MaxValue, "0", formatString ?? "", formatFlags);

    public TExt AsValue(StringBuilder value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext("", value, 0, int.MaxValue, "0", formatString ?? "", formatFlags);

    public TExt AsValue(ReadOnlySpan<char> nonJsonfieldName, StringBuilder value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, "0", formatString ?? "", formatFlags);

    public TExt AsValue(StringBuilder value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext("", value, startIndex, length, "0", formatString ?? "", formatFlags);

    public TExt AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);

    public TExt AsValueOrNull(StringBuilder? value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrNullNext("", value, startIndex, length, formatString ?? "", formatFlags);

    public TExt AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex = 0, int length = int.MaxValue
      , string? defaultValue = null, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue ?? "0", formatString ?? "", formatFlags);

    public TExt AsValueOrDefault(StringBuilder? value, int startIndex = 0, int length = int.MaxValue, string? defaultValue = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.FieldValueOrDefaultNext("", value, startIndex, length, defaultValue ?? "0", formatString ?? "", formatFlags);

    public TExt AsValueMatch<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.ValueMatchOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TExt AsValueMatchOrNull<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.ValueMatchOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TExt AsValueMatchOrDefault<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny? value, string? defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null 
      , FieldContentHandling formatFlags = AsValueContent) =>
        Stb.ValueMatchOrDefaultNext(nonJsonfieldName, value, defaultValue ?? "", formatString ?? "", formatFlags);

    public TExt AsString(ReadOnlySpan<char> nonJsonfieldName, bool value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TExt AsString(bool value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        AsString("", value, formatString, formatFlags);

    public TExt AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, bool? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TExt AsStringOrNull(bool? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        AsStringOrNull("", value, formatString ?? "", formatFlags);

    public TExt AsString<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) where TFmt : ISpanFormattable =>
        Stb.FieldStringOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TExt AsString<TFmt>(TFmt value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) where TFmt : ISpanFormattable =>
        AsString("", value, formatString, formatFlags);

    public TExt AsStringOrNull<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) where TFmt : class, ISpanFormattable =>
        Stb.FieldStringOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TExt AsStringOrNull<TFmt>(TFmt? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) 
        where TFmt : class, ISpanFormattable =>
        AsStringOrNull("", value, formatString, formatFlags);

    public TExt AsStringOrDefault<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, TFmt defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll)
        where TFmt : class, ISpanFormattable =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value ?? defaultValue, "", formatString ?? "", formatFlags);

    public TExt AsStringOrDefault<TFmt>(TFmt? value, TFmt defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) where TFmt : class, ISpanFormattable =>
        AsStringOrDefault("", value, defaultValue, formatString, formatFlags);

    public TExt AsStringOrDefault<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) where TFmt : class, ISpanFormattable =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);

    public TExt AsStringOrDefault<TFmt>(TFmt? value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) where TFmt : class, ISpanFormattable =>
        AsStringOrDefault("", value, defaultValue, formatString, formatFlags);

    public TExt AsStringOrNull<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll)
        where TFmtStruct : struct, ISpanFormattable =>
        Stb.FieldStringOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TExt AsStringOrNull<TFmtStruct>(TFmtStruct? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) where TFmtStruct : struct, ISpanFormattable =>
        AsStringOrNull("", value, formatString, formatFlags);

    public TExt AsStringOrDefault<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value, TFmtStruct defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) where TFmtStruct : struct, ISpanFormattable =>
        Stb.FieldStringOrNullNext(nonJsonfieldName, value ?? defaultValue, formatString ?? "", formatFlags);

    public TExt AsStringOrDefault<TFmtStruct>(TFmtStruct? value, TFmtStruct defaultValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll)
        where TFmtStruct : struct, ISpanFormattable =>
        AsStringOrDefault("", value, defaultValue, formatString, formatFlags);

    public TExt AsStringOrDefault<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) where TFmtStruct : struct, ISpanFormattable =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);

    public TExt AsStringOrDefault<TFmtStruct>(TFmtStruct? value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll)
        where TFmtStruct : struct, ISpanFormattable =>
        AsStringOrDefault("", value, defaultValue, formatString, formatFlags);

    public TExt RevealAsString<TCloaked, TCloakedBase>(TCloaked value, PalantírReveal<TCloakedBase> palantírReveal
      , FieldContentHandling formatFlags = EncodeAll) where TCloaked : TCloakedBase =>
        RevealAsString("", value, palantírReveal, formatFlags);

    public TExt RevealAsString<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked value
      , PalantírReveal<TCloakedBase> palantírReveal, FieldContentHandling formatFlags = EncodeAll)
        where TCloaked : TCloakedBase =>
        Stb.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, palantírReveal, "", formatFlags);

    public TExt RevealAsStringOrNull<TCloaked, TCloakedBase>(TCloaked? value, PalantírReveal<TCloakedBase> palantírReveal
      , FieldContentHandling formatFlags = EncodeAll)
        where TCloaked : TCloakedBase =>
        RevealAsStringOrNull("", value, palantírReveal, formatFlags);

    public TExt RevealAsStringOrNull<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
        , PalantírReveal<TCloakedBase> palantírReveal, FieldContentHandling formatFlags = EncodeAll) where TCloaked : TCloakedBase =>
        Stb.FieldStringRevealOrNullNext(nonJsonfieldName, value, palantírReveal, formatFlags);

    public TExt RevealAsStringOrDefault<TCloaked, TCloakedBase>(TCloaked? value, PalantírReveal<TCloakedBase> palantírReveal
      , string defaultValue = "", FieldContentHandling formatFlags = EncodeAll) where TCloaked : TCloakedBase =>
        RevealAsStringOrDefault("", value, palantírReveal, defaultValue, formatFlags);

    public TExt RevealAsStringOrDefault<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal, string defaultValue = "", FieldContentHandling formatFlags = EncodeAll) 
        where TCloaked : TCloakedBase =>
        Stb.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, palantírReveal, defaultValue, formatFlags);

    public TExt RevealAsString<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
      , FieldContentHandling formatFlags = EncodeAll) where TCloakedStruct : struct =>
        RevealAsString("", value, palantírReveal, formatFlags);

    public TExt RevealAsString<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, FieldContentHandling formatFlags = EncodeAll)
        where TCloakedStruct : struct =>
        Stb.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, palantírReveal, "", formatFlags);

    public TExt RevealAsStringOrNull<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
      , FieldContentHandling formatFlags = EncodeAll)
        where TCloakedStruct : struct =>
        RevealAsStringOrNull("", value, palantírReveal, formatFlags);

    public TExt RevealAsStringOrNull<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, FieldContentHandling formatFlags = EncodeAll) where TCloakedStruct : struct =>
        Stb.FieldStringRevealOrNullNext(nonJsonfieldName, value, palantírReveal, formatFlags);

    public TExt RevealAsStringOrDefault<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string defaultValue = "", FieldContentHandling formatFlags = EncodeAll) where TCloakedStruct : struct =>
        RevealAsStringOrDefault("", value, palantírReveal, defaultValue, formatFlags);

    public TExt RevealAsStringOrDefault<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string defaultValue = "", FieldContentHandling formatFlags = EncodeAll) 
        where TCloakedStruct : struct =>
        Stb.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, palantírReveal, defaultValue, formatFlags);

    public TExt RevealAsString<TBearer>(TBearer value, FieldContentHandling formatFlags = EncodeAll) where TBearer : IStringBearer =>
        Stb.FieldStringRevealOrDefaultNext("", value, "", formatFlags);

    public TExt RevealAsString<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer value
      , FieldContentHandling formatFlags = EncodeAll) where TBearer : IStringBearer =>
        Stb.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, "", formatFlags);

    public TExt RevealAsStringOrNull<TBearer>(TBearer? value, FieldContentHandling formatFlags = EncodeAll) 
        where TBearer : IStringBearer =>
        Stb.FieldStringRevealOrNullNext("", value, formatFlags);

    public TExt RevealAsStringOrNull<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value
      , FieldContentHandling formatFlags = EncodeAll) where TBearer : IStringBearer =>
        Stb.FieldStringRevealOrNullNext(nonJsonfieldName, value, formatFlags);

    public TExt RevealAsStringOrDefault<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value, string defaultValue = ""
      , FieldContentHandling formatFlags = EncodeAll)
        where TBearer : IStringBearer => Stb.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, defaultValue, formatFlags);

    public TExt RevealAsStringOrDefault<TBearer>(TBearer? value, string defaultValue = ""
        , FieldContentHandling formatFlags = EncodeAll)
        where TBearer : IStringBearer => Stb.FieldStringRevealOrDefaultNext("", value, defaultValue, formatFlags);

    public TExt RevealAsString<TBearerStruct>(TBearerStruct? value, FieldContentHandling formatFlags = EncodeAll) 
        where TBearerStruct : struct, IStringBearer =>
        Stb.FieldStringRevealOrDefaultNext("", value, "", formatFlags);

    public TExt RevealAsString<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
      , FieldContentHandling formatFlags = EncodeAll) where TBearerStruct : struct, IStringBearer =>
        Stb.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, "", formatFlags);

    public TExt RevealAsStringOrNull<TBearerStruct>(TBearerStruct? value, FieldContentHandling formatFlags = EncodeAll) 
        where TBearerStruct : struct, IStringBearer =>
        Stb.FieldStringRevealOrNullNext("", value, formatFlags);

    public TExt RevealAsStringOrNull<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value
      , FieldContentHandling formatFlags = EncodeAll) where TBearerStruct : struct, IStringBearer =>
        Stb.FieldStringRevealOrNullNext(nonJsonfieldName, value, formatFlags);

    public TExt RevealAsStringOrDefault<TBearerStruct>(ReadOnlySpan<char> nonJsonfieldName, TBearerStruct? value, string defaultValue = ""
      , FieldContentHandling formatFlags = EncodeAll)
        where TBearerStruct : struct, IStringBearer => 
        Stb.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, defaultValue, formatFlags);

    public TExt RevealAsStringOrDefault<TBearerStruct>(TBearerStruct? value, string defaultValue = ""
      , FieldContentHandling formatFlags = EncodeAll)
        where TBearerStruct : struct, IStringBearer => 
        Stb.FieldStringRevealOrDefaultNext("", value, defaultValue, formatFlags);

    public TExt AsString(ReadOnlySpan<char> nonJsonfieldName, Span<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, "", formatString ?? "", formatFlags);

    public TExt AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, Span<char> value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);

    public TExt AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, Span<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TExt AsStringOrNull(Span<char> value, FieldContentHandling formatFlags = EncodeAll) => 
        Stb.FieldStringNext("", value, formatFlags);

    public TExt AsString(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, "", formatString ?? "", formatFlags);

    public TExt AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);

    public TExt AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TExt AsStringOrNull(ReadOnlySpan<char> value, FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrNullNext("", value, "", formatFlags);

    public TExt AsString(Span<char> value, FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext("", value, "", "", formatFlags);

    public TExt AsString(ReadOnlySpan<char> value, FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext("", value, "", "", formatFlags);

    public TExt AsString(string value, FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext("", value, "", "",  formatFlags);

    public TExt AsString(ReadOnlySpan<char> nonJsonfieldName, string value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, "", formatString ?? "", formatFlags);

    public TExt AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, string? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TExt AsString(ReadOnlySpan<char> nonJsonfieldName, string value, int startIndex, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
        , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, "",  formatString ?? "", formatFlags);

    public TExt AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);

    public TExt AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length = int.MaxValue
      , string? defaultValue = null, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue ?? "", formatString ?? "", formatFlags);

    public TExt AsString(ReadOnlySpan<char> nonJsonfieldName, char[] value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, 0, int.MaxValue, "", formatString ?? "", formatFlags);

    public TExt AsString(ReadOnlySpan<char> nonJsonfieldName, char[] value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, "", formatString ?? "", formatFlags);

    public TExt AsString(char[] value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext("", value, startIndex, length, "", formatString ?? "", formatFlags);

    public TExt AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);

    public TExt AsStringOrNull(char[]? value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrNullNext("", value, startIndex, length, formatString ?? "", formatFlags);

    public TExt AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex = 0, int length = int.MaxValue
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue, formatString ?? "", formatFlags);

    public TExt AsStringOrDefault(char[]? value, int startIndex = 0, int length = int.MaxValue, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext("", value, startIndex, length, defaultValue, formatString ?? "", formatFlags);

    public TExt AsString(ReadOnlySpan<char> nonJsonfieldName, ICharSequence value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, 0, int.MaxValue, "", formatString ?? "", formatFlags);

    public TExt AsString(ICharSequence value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext("", value, 0, int.MaxValue, "", formatString ?? "", formatFlags);

    public TExt AsString(ReadOnlySpan<char> nonJsonfieldName, ICharSequence value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, "", formatString ?? "", formatFlags);

    public TExt AsString(ICharSequence value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext("", value, startIndex, length, "", formatString ?? "", formatFlags);

    public TExt AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);

    public TExt AsStringOrNull(ICharSequence? value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrNullNext("", value, startIndex, length, formatString ?? "", formatFlags);

    public TExt AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex = 0, int length = int.MaxValue
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue, formatString ?? "", formatFlags);

    public TExt AsStringOrDefault(ICharSequence? value, int startIndex = 0, int length = int.MaxValue, string fallbackValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext("", value, startIndex, length, fallbackValue, formatString ?? "", formatFlags);

    public TExt AsString(ReadOnlySpan<char> nonJsonfieldName, StringBuilder value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, 0, int.MaxValue, "",  formatString ?? "", formatFlags);

    public TExt AsString(StringBuilder value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext("", value, 0, int.MaxValue, "", formatString ?? "", formatFlags);

    public TExt AsString(ReadOnlySpan<char> nonJsonfieldName, StringBuilder value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, "", formatString ?? "", formatFlags);

    public TExt AsString(StringBuilder value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext("", value, startIndex, length, "", formatString ?? "", formatFlags);

    public TExt AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrNullNext(nonJsonfieldName, value, startIndex, length, formatString ?? "", formatFlags);

    public TExt AsStringOrNull(StringBuilder? value, int startIndex = 0, int length = int.MaxValue
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrNullNext("", value, startIndex, length, formatString ?? "", formatFlags);

    public TExt AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex = 0, int length = int.MaxValue
      , string defaultValue = "", [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue, formatString ?? "", formatFlags);

    public TExt AsStringOrDefault(StringBuilder? value, int startIndex = 0, int length = int.MaxValue, string fallbackValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.FieldStringOrDefaultNext("", value, startIndex, length, fallbackValue, formatString ?? "", formatFlags);

    public TExt AsStringMatch<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.StringMatchOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TExt AsStringMatchOrNull<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.StringMatchOrNullNext(nonJsonfieldName, value, formatString ?? "", formatFlags);

    public TExt AsStringMatchOrDefault<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny? value, string defaultValue = ""
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FieldContentHandling formatFlags = EncodeAll) =>
        Stb.StringMatchOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString ?? "", formatFlags);

    public IScopeDelimitedStringBuilder StartDelimitedStringBuilder() => Stb.StartDelimitedStringBuilder();
}
