// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ValueType;

public class ValueTypeDieCast<TExt> : TypeMolderDieCast<TExt> where TExt : TypeMolder
{
    public bool ValueInComplexType { get; private set; }

    public ValueTypeDieCast<TExt> InitializeValueBuilderCompAccess
        (TExt externalTypeBuilder, TypeMolder.StyleTypeBuilderPortableState typeBuilderPortableState, bool isComplex)
    {
        Initialize(externalTypeBuilder, typeBuilderPortableState);

        ValueInComplexType          = isComplex && typeBuilderPortableState.Master.Style.AllowsUnstructured();
        OnFinishedWithStringBuilder = FinishUsingStringBuilder;

        return this;
    }

    private Action<IScopeDelimitedStringBuilder>? OnFinishedWithStringBuilder { get; set; }

    public bool NotJson => Style.IsNotJson();

    public TExt FieldValueNext(ReadOnlySpan<char> nonJsonfieldName, bool? value, string? formatString = null)
    {
        (NotJson && nonJsonfieldName.Length > 0 ? this.FieldNameJoin(nonJsonfieldName) : this).AppendFormattedOrNull(value, formatString);
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, string? formatString = null) where TFmt : ISpanFormattable
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (formatString != null)
            this.AppendMatchFormattedOrNull(value, formatString);
        else
            this.AppendValue(value);
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, string? formatString = null) where TFmt : struct, ISpanFormattable
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (formatString != null)
            this.AppendMatchFormattedOrNull(value, formatString);
        else
            this.AppendOrNull(value);
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrDefaultNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, ReadOnlySpan<char> defaultValue
      , string? formatString = null) where TFmt : ISpanFormattable
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value == null) { Sb.Append(defaultValue); }
        else
        {
            this.AppendFormattedOrNull(value, formatString ?? "");
        }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrDefaultNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, ReadOnlySpan<char> defaultValue
      , string? formatString = null) where TFmt : struct, ISpanFormattable
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value == null) { this.AppendFormattedOrNull(defaultValue, formatString); }
        else
        {
            this.AppendFormattedOrNull(value, formatString ?? "");
        }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueNext<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked value, PalantírReveal<TCloakedBase> palantírReveal)
        where TCloaked : TCloakedBase
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        palantírReveal(value, Master);
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrNullNext<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal)
        where TCloaked : TCloakedBase
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value == null) { Sb.Append(Settings.NullStyle); }
        else { palantírReveal(value, Master); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrDefaultNext<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal, ReadOnlySpan<char> defaultValue)
        where TCloaked : TCloakedBase
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value == null) { Sb.Append(defaultValue); }
        else { palantírReveal(value, Master); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrDefaultNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value, string defaultValue = "")
    where TBearer : IStringBearer
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null) { value.RevealState(Master); }
        else { Sb.Append(defaultValue); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrNullNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value)
        where TBearer : IStringBearer
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            value.RevealState(Master);
        }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public TExt ValueMatchOrNullNext<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value, string? formatString = null)
    {
        if (SkipBody) return StyleTypeBuilder;
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null) { this.AppendMatchFormattedOrNull(value, formatString ?? ""); }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public TExt ValueMatchOrDefaultNext<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value, string defaultValue = "", string? formatString = null)
    {
        if (SkipBody) return StyleTypeBuilder;
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null) { this.AppendMatchFormattedOrNull(value, formatString ?? ""); }
        else { this.AppendFormattedOrNull(defaultValue, formatString ?? ""); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, Span<char> value, ReadOnlySpan<char> fallbackValue
      , string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        this.AppendFormattedOrNull(value.Length != 0 ? value : fallbackValue, formatString);
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value, ReadOnlySpan<char> fallbackValue
      , string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        this.AppendFormattedOrNull(value.Length != 0 ? value : fallbackValue, formatString);
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrNullNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value, string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        this.AppendFormattedOrNull(value, formatString);
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueNext(ReadOnlySpan<char> nonJsonfieldName, string value, string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        this.AppendFormattedOrNull(value, formatString);
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var caplength = Math.Clamp(length, 0, value.Length - capStart);
            if (caplength > 0) { this.AppendFormattedOrNull(value, formatString, capStart, caplength); }
            else { this.AppendFormattedOrNull(defaultValue, formatString); }
        }
        else { this.AppendFormattedOrNull(defaultValue, formatString); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrNullNext(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length
      , string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var caplength = Math.Clamp(length, 0, value.Length - capStart);
            if (caplength > 0) { this.AppendFormattedOrNull(value, formatString, capStart, caplength); }
            else { Sb.Append(Settings.NullStyle); }
        }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueNext(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex = 0, int length = int.MaxValue, string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var caplength = Math.Clamp(length, 0, value.Length - capStart);
            this.AppendFormattedOrNull(value, formatString, capStart, caplength);
        }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex, int length, string defaultValue = ""
      , string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var caplength = Math.Clamp(length, 0, value.Length - capStart);
            if (caplength > 0) {this.AppendFormattedOrNull(value, formatString, capStart, caplength); }
            else { this.AppendFormattedOrNull(defaultValue, formatString); }
        }
        else { this.AppendFormattedOrNull(defaultValue, formatString); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrNullNext(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex, int length
      , string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var caplength = Math.Clamp(length, 0, value.Length - capStart);
            if (caplength > 0) { this.AppendFormattedOrNull(value, formatString, capStart, caplength); }
            else { Sb.Append(Settings.NullStyle); }
        }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueNext(ReadOnlySpan<char> nonJsonfieldName, ICharSequence value, int startIndex, int length
      , string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        var capStart  = Math.Clamp(startIndex, 0, value.Length);
        var caplength = Math.Clamp(length, 0, value.Length - capStart);
        if (caplength > 0) { this.AppendFormattedOrNull(value, formatString, capStart, caplength); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex, int length
      , string defaultValue = "", string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var caplength = Math.Clamp(length, 0, value.Length - capStart);
            if (caplength > 0) { this.AppendFormattedOrNull(value, formatString, capStart, caplength); }
            else { this.AppendFormattedOrNull(defaultValue, formatString); }
        }
        else { Sb.Append(defaultValue); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrNullNext(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex, int length
      , string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var caplength = Math.Clamp(length, 0, value.Length - capStart);
            if (caplength > 0) { this.AppendFormattedOrNull(value, formatString, capStart, caplength); }
            else { Sb.Append(Settings.NullStyle); }
        }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder value, int startIndex, int length, string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        var capStart  = Math.Clamp(startIndex, 0, value.Length);
        var caplength = Math.Clamp(length, 0, value.Length - capStart);
        if (caplength > 0) { this.AppendFormattedOrNull(value, formatString, capStart, caplength); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex, int length
      , string defaultValue = "", string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var caplength = Math.Clamp(length, 0, value.Length - capStart);
            if (caplength > 0) { this.AppendFormattedOrNull(value, formatString, capStart, caplength); }
            else { Sb.Append(defaultValue); }
        }
        else { Sb.Append(defaultValue); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrNullNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex, int length
      , string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var caplength = Math.Clamp(length, 0, value.Length - capStart);
            if (caplength > 0) { this.AppendFormattedOrNull(value, formatString, capStart, caplength); }
            else { Sb.Append(Settings.NullStyle); }
        }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringNext(ReadOnlySpan<char> nonJsonfieldName, bool? value, string? formatString = null) 
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        this.AppendFormattedOrNull(value, formatString);
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrNullNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, string? formatString = null) where TFmt : ISpanFormattable
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        formatString ??= StyledTypeBuilderExtensions.NoFormattingFormatString;
        Sb.Append("\"");
        this.AppendFormattedOrNull(value, formatString);
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrDefaultNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, string defaultValue = "", string? formatString = null) where TFmt : ISpanFormattable
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        formatString ??= StyledTypeBuilderExtensions.NoFormattingFormatString;
        Sb.Append("\"");
        if (value != null) { this.AppendFormattedOrNull(value, formatString); }
        else { this.AppendOrNull(defaultValue); }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrNullNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, string? formatString = null) where TFmt : struct, ISpanFormattable
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        formatString ??= StyledTypeBuilderExtensions.NoFormattingFormatString;
        Sb.Append("\"");
        this.AppendFormattedOrNull(value, formatString);
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrDefaultNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, string defaultValue = ""
      , string? formatString = null) where TFmt : struct, ISpanFormattable
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        formatString ??= StyledTypeBuilderExtensions.NoFormattingFormatString;
        Sb.Append("\"");
        if (value != null) { this.AppendFormattedOrNull(value, formatString); }
        else { this.AppendOrNull(defaultValue); }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }
    
    public TExt FieldStringRevealOrDefaultNext<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal, string defaultValue = "") where TCloaked : TCloakedBase
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value != null) { palantírReveal(value, Master); }
        else { Sb.Append(defaultValue); }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringRevealOrNullNext<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal)
        where TCloaked : TCloakedBase
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value == null) { Sb.Append(Settings.NullStyle); }
        else
        {
            Sb.Append("\"");
            palantírReveal(value, Master);
            Sb.Append("\"");
        }
        return ConditionalValueTypeSuffix();
    }
    
    public TExt FieldStringRevealOrDefaultNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value, string defaultValue = "")
        where TBearer : IStringBearer
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value != null) { value.RevealState(Master); }
        else { Sb.Append(defaultValue); }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringRevealOrNullNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value)
        where TBearer : IStringBearer
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            Sb.Append("\"");
            value.RevealState(Master);
            Sb.Append("\"");
        }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringNext(ReadOnlySpan<char> nonJsonfieldName, Span<char> value, string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        this.AppendFormattedOrNull(value, formatString);
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        this.AppendOrNull(value);
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value, string defaultValue = "", string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value.Length > 0)
        {
            this.AppendFormattedOrNull(value, formatString); 
        }
        else
        {
            this.AppendFormattedOrNull(defaultValue, formatString);
        }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value, string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        this.AppendFormattedOrNull(value, formatString);
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length, string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var caplength = Math.Clamp(length, 0, value.Length - capStart);
            if (caplength > 0) { this.AppendFormattedOrNull(value, formatString, capStart, caplength); }
            else { Sb.Append(Settings.NullStyle); }
        }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length
      , string defaultValue = "", string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var caplength = Math.Clamp(length, 0, value.Length - capStart);
            if (caplength > 0) { this.AppendFormattedOrNull(value, formatString, capStart, caplength); }
            else { this.AppendFormattedOrNull(defaultValue, formatString); }
        }
        else { this.AppendFormattedOrNull(defaultValue, formatString); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringNext(ReadOnlySpan<char> nonJsonfieldName, char[] value, int startIndex, int length, string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        var capStart  = Math.Clamp(startIndex, 0, value.Length);
        var caplength = Math.Clamp(length, 0, value.Length - capStart);
        this.AppendFormattedOrNull(value, formatString, capStart, caplength);
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex, int length, string defaultValue = "", string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            Sb.Append("\"");
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var caplength = Math.Clamp(length, 0, value.Length - capStart);
            if (caplength > 0) { this.AppendFormattedOrNull(value, formatString, capStart, caplength); }
            else { Sb.Append(defaultValue); }
            Sb.Append("\"");
        }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex, int length, string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            Sb.Append("\"");
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var caplength = Math.Clamp(length, 0, value.Length - capStart);
            if (caplength > 0) { this.AppendFormattedOrNull(value, formatString, capStart, caplength); }
            Sb.Append("\"");
        }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public IScopeDelimitedStringBuilder StartDelimitedStringBuilder()
    {
        if (Style.IsJson()) Sb.Append("\"");
        var scopedSb = (IScopeDelimitedStringBuilder)Sb;
        scopedSb.OnScopeEndedAction = OnFinishedWithStringBuilder;
        return scopedSb;
    }

    private void FinishUsingStringBuilder(IScopeDelimitedStringBuilder finishedBuilding)
    {
        if (Style.IsJson()) finishedBuilding.Append("\"");
    }

    public TExt FieldStringNext(ReadOnlySpan<char> nonJsonfieldName, ICharSequence value, string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        this.AppendFormattedOrNull(value, formatString);
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, string defaultValue, string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value != null) { this.AppendFormattedOrNull(value, formatString); }
        else { this.AppendFormattedOrNull(defaultValue, formatString); }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            Sb.Append("\"");
            this.AppendFormattedOrNull(value, formatString);
            Sb.Append("\"");
        }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringNext(ReadOnlySpan<char> nonJsonfieldName, ICharSequence value, int startIndex, int length, string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        var capStart  = Math.Clamp(startIndex, 0, value.Length);
        var caplength = Math.Clamp(length, 0, value.Length - capStart);
        if (caplength > 0) { this.AppendFormattedOrNull(value, formatString, capStart, caplength); }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex, int length
      , string defaultValue = "", string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var caplength = Math.Clamp(length, 0, value.Length - capStart);
            if (caplength > 0) { this.AppendFormattedOrNull(value, formatString, capStart, caplength); }
            else { this.AppendFormattedOrNull(defaultValue, formatString); }
        }
        else { this.AppendFormattedOrNull(defaultValue, formatString); }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex, int length, string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            Sb.Append("\"");
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var caplength = Math.Clamp(length, 0, value.Length - capStart);
            if (caplength > 0) { this.AppendFormattedOrNull(value, formatString, capStart, caplength); }
            Sb.Append("\"");
        }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder value, int startIndex, int length, string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        var capStart  = Math.Clamp(startIndex, 0, value.Length);
        var caplength = Math.Clamp(length, 0, value.Length - capStart);
        if (caplength > 0) { this.AppendFormattedOrNull(value, formatString, capStart, caplength); }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex, int length
      , string defaultValue = "", string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var caplength = Math.Clamp(length, 0, value.Length - capStart);
            if (caplength > 0) { this.AppendFormattedOrNull(value, formatString, capStart, caplength); }
            else { this.AppendFormattedOrNull(defaultValue, formatString); }
        }
        else { this.AppendFormattedOrNull(defaultValue, formatString); }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex, int length, string? formatString = null)
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            Sb.Append("\"");
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var caplength = Math.Clamp(length, 0, value.Length - capStart);
            if (caplength > 0) { this.AppendFormattedOrNull(value, formatString); }
            Sb.Append("\"");
        }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public TExt ConditionalValueTypeSuffix()
    {
        if (ValueInComplexType) { this.AddGoToNext(); }
        return StyleTypeBuilder;
    }
}
