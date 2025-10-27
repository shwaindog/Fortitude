// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
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
        (ValueInComplexType && nonJsonfieldName.Length > 0 ? this.FieldNameJoin(nonJsonfieldName) : this).AppendFormattedOrNull(value, formatString);
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldFmtValueOrNullNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, string? formatString = null) 
        where TFmt : ISpanFormattable
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        this.AppendFormatted(value, formatString ?? "");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldFmtValueOrNullNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, string? formatString = null) 
        where TFmt : struct, ISpanFormattable
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        this.AppendNullableFormattedOrNull(value, formatString ?? "" );
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrDefaultNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, ReadOnlySpan<char> defaultValue
      , string? formatString = null) where TFmt : ISpanFormattable
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value == null) { StyleFormatter.Format(defaultValue, 0, Sb, formatString ?? ""); }
        else
        {
            this.AppendFormatted(value, formatString ?? "");
        }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrDefaultNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, ReadOnlySpan<char> defaultValue
      , string? formatString = null) where TFmt : struct, ISpanFormattable
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value == null) { StyleFormatter.Format(defaultValue, 0, Sb, formatString ?? ""); }
        else
        {
            this.AppendNullableFormattedOrNull(value, formatString ?? "");
        }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueNext<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked value, PalantírReveal<TCloakedBase> palantírReveal)
        where TCloaked : TCloakedBase
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value == null) { Sb.Append(Settings.NullStyle); }
        else { palantírReveal(value, Master); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrNullNext<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal)
        where TCloakedStruct : struct
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value == null) { Sb.Append(Settings.NullStyle); }
        else { palantírReveal(value.Value, Master); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrNullNext<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal)
        where TCloaked : TCloakedBase
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value == null) { Sb.Append(Settings.NullStyle); }
        else { palantírReveal(value, Master); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrDefaultNext<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal, ReadOnlySpan<char> defaultValue)
        where TCloaked : TCloakedBase
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value == null) { StyleFormatter.Format(defaultValue, 0, Sb, ""); }
        else { palantírReveal(value, Master); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrDefaultNext<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, ReadOnlySpan<char> defaultValue)
        where TCloakedStruct : struct
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value == null) { StyleFormatter.Format(defaultValue, 0, Sb, ""); }
        else { palantírReveal(value.Value, Master); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrDefaultNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value, string defaultValue = "")
    where TBearer : IStringBearer
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null) { value.RevealState(Master); }
        else { Sb.Append(defaultValue); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrDefaultNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value, string defaultValue = "")
    where TBearer : struct, IStringBearer
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null) { value.Value.RevealState(Master); }
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

    public TExt FieldValueOrNullNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value)
        where TBearer : struct, IStringBearer
    {
        if (NotJson && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            value.Value.RevealState(Master);
        }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, Span<char> value, ReadOnlySpan<char> fallbackValue
      , string? formatString = null)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        StyleFormatter.Format(value.Length != 0 ? value : fallbackValue, 0, Sb, formatString);
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value, ReadOnlySpan<char> fallbackValue
      , string? formatString = null)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        StyleFormatter.Format(value.Length != 0 ? value : fallbackValue, 0, Sb, formatString);
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrNullNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value, string? formatString = null)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value.Length != 0) { StyleFormatter.Format(value, 0, Sb, formatString); }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string? formatString = null)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var caplength = Math.Clamp(length, 0, value.Length - capStart);
            if (caplength > 0) 
            { StyleFormatter.Format(value, capStart, Sb, formatString, caplength); }
            else
            {
                StyleFormatter.Format(defaultValue, 0, Sb, formatString ?? "");
            }
        }
        else { StyleFormatter.Format(defaultValue, 0, Sb, formatString ?? ""); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrNullNext(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length
      , string? formatString = null)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0) { StyleFormatter.Format(value, capStart, Sb, formatString, capLength); }
            else
            {
                if (formatString?.Length > 0)
                {
                    ((ReadOnlySpan<char>)formatString).ExtractExtendedStringFormatStages(out var prefix, out _, out _
                                                                                       , out _, out _, out _, out var suffix);
                    if (prefix.Length > 0 || suffix.Length > 0)
                    {
                        StyleFormatter.Format( "",0, Sb, formatString);
                        return ConditionalValueTypeSuffix();
                    }
                }
                Sb.Append(Settings.NullStyle);
            }
        }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueNext(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex = 0, int length = int.MaxValue, string? formatString = null)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
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
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var caplength = Math.Clamp(length, 0, value.Length - capStart);
            if (caplength > 0) { StyleFormatter.Format(value, capStart, Sb, formatString, caplength); }
            else
            {
                StyleFormatter.Format(defaultValue,0, Sb, formatString ?? "");
            }
        }
        else { StyleFormatter.Format(defaultValue, 0, Sb, formatString ?? ""); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrNullNext(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex, int length
      , string? formatString = null)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var caplength = Math.Clamp(length, 0, value.Length - capStart);
            if (caplength > 0) { StyleFormatter.Format(value, capStart, Sb, formatString, caplength); }
            else
            {
                if (formatString?.Length > 0)
                {
                    ((ReadOnlySpan<char>)formatString).ExtractExtendedStringFormatStages(out var prefix, out _, out _
                                                                                       , out _, out _, out _, out var suffix);
                    if (prefix.Length > 0 || suffix.Length > 0)
                    {
                        StyleFormatter.Format("", 0, Sb, formatString);
                        return ConditionalValueTypeSuffix();
                    }
                }
                Sb.Append(Settings.NullStyle);
            }
        }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex, int length
      , string defaultValue = "", string? formatString = null)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var caplength = Math.Clamp(length, 0, value.Length - capStart);
            if (caplength > 0) { StyleFormatter.Format(value, capStart, Sb, formatString, caplength); }
            else
            {
                if (formatString?.Length > 0)
                {
                    ((ReadOnlySpan<char>)formatString).ExtractExtendedStringFormatStages(out var prefix, out _, out _
                                                                                       , out _, out _, out _, out var suffix);
                    if (prefix.Length > 0 || suffix.Length > 0)
                    {
                        StyleFormatter.Format(defaultValue, 0, Sb, formatString);
                        return ConditionalValueTypeSuffix();
                    }
                }
                StyleFormatter.Format(defaultValue, 0, Sb, formatString ?? "");
            }
        }
        else { StyleFormatter.Format(defaultValue, 0, Sb, formatString ?? ""); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrNullNext(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex, int length
      , string? formatString = null)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var caplength = Math.Clamp(length, 0, value.Length - capStart);
            if (caplength > 0) { StyleFormatter.Format(value, capStart, Sb, formatString, caplength); }
            else
            {
                if (formatString?.Length > 0)
                {
                    ((ReadOnlySpan<char>)formatString).ExtractExtendedStringFormatStages(out var prefix, out _, out _
                                                                                       , out _, out _, out _, out var suffix);
                    if (prefix.Length > 0 || suffix.Length > 0)
                    {
                        Sb.Append(prefix);
                        Sb.Append(suffix);
                        return ConditionalValueTypeSuffix();
                    }
                }
                Sb.Append(Settings.NullStyle);
            }
        }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex, int length
      , string defaultValue = "", string? formatString = null)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var caplength = Math.Clamp(length, 0, value.Length - capStart);
            if (caplength > 0)
            {
                StyleFormatter.Format(value, capStart, Sb, formatString, caplength);
            }
            else
            {
                if (formatString?.Length > 0)
                {
                    ((ReadOnlySpan<char>)formatString).ExtractExtendedStringFormatStages(out var prefix, out _, out _
                                                                                       , out _, out _, out _, out var suffix);
                    if (prefix.Length > 0 || suffix.Length > 0)
                    {
                        Sb.Append(prefix);
                        Sb.Append(suffix);
                        return ConditionalValueTypeSuffix();
                    }
                }
                StyleFormatter.Format(defaultValue, 0, Sb, formatString ?? "");
            }
        }
        else { StyleFormatter.Format(defaultValue, 0, Sb, formatString ?? ""); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldValueOrNullNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex, int length
      , string? formatString = null)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var caplength = Math.Clamp(length, 0, value.Length - capStart);
            if (caplength > 0) { StyleFormatter.Format(value, capStart, Sb, formatString, caplength); }
            else
            {
                if (formatString?.Length > 0)
                {
                    ((ReadOnlySpan<char>)formatString).ExtractExtendedStringFormatStages(out var prefix, out _, out _
                                                                                       , out _, out _, out _, out var suffix);
                    if (prefix.Length > 0 || suffix.Length > 0)
                    {
                        Sb.Append(prefix);
                        Sb.Append(suffix);
                        return ConditionalValueTypeSuffix();
                    }
                }
                Sb.Append(Settings.NullStyle);
            }
        }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public TExt ValueMatchOrNullNext<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value, string? formatString = null)
    {
        if (SkipBody) return StyleTypeBuilder;
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null) { this.AppendMatchFormattedOrNull(value, formatString ?? "", FieldContentHandling.DisableAutoDelimiting); }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public TExt ValueMatchOrDefaultNext<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value, string defaultValue = "", string? formatString = null)
    {
        if (SkipBody) return StyleTypeBuilder;
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            
            this.AppendMatchFormattedOrNull(value, formatString ?? "", FieldContentHandling.DisableAutoDelimiting);
        }
        else
        {
            StyleFormatter.Format(defaultValue, 0, Sb, formatString ?? ""); 
        }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringNext(ReadOnlySpan<char> nonJsonfieldName, bool? value, string? formatString = null) 
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value == null)
        {
            Sb.Append(Settings.NullStyle);
        }
        else
        {
            Sb.Append("\"");
            this.AppendFormattedOrNull(value, formatString);
            Sb.Append("\"");
        }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrNullNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, string? formatString = null) where TFmt : ISpanFormattable
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        formatString ??= StyledTypeBuilderExtensions.NoFormattingFormatString;
        if (value == null)
        {
            Sb.Append(Settings.NullStyle);
        }
        else
        {
            Sb.Append("\"");
            this.AppendFormattedOrNull(value, formatString);
            Sb.Append("\"");
        }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrDefaultNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, string defaultValue = "", string? formatString = null) where TFmt : ISpanFormattable
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        formatString ??= "";
        Sb.Append("\"");
        if (value != null) { this.AppendFormattedOrNull(value, formatString); }
        else { StyleFormatter.Format(defaultValue, 0, Sb, formatString); }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrNullNext<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value, string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        formatString ??= StyledTypeBuilderExtensions.NoFormattingFormatString;
        if (value == null)
        {
            Sb.Append(Settings.NullStyle);
        }
        else
        {
            Sb.Append("\"");
            this.AppendFormattedOrNull(value, formatString);
            Sb.Append("\"");
        }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrDefaultNext<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value, string defaultValue = ""
      , string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        formatString ??= "";
        Sb.Append("\"");
        if (value != null) { this.AppendFormattedOrNull(value, formatString); }
        else { StyleFormatter.Format(defaultValue, 0, Sb, formatString); }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }
    
    public TExt FieldStringRevealOrDefaultNext<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal, string defaultValue = "") where TCloaked : TCloakedBase
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value != null) { palantírReveal(value, Master); }
        else { StyleFormatter.Format(defaultValue, 0, Sb, ""); }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }
    
    public TExt FieldStringRevealOrDefaultNext<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string defaultValue = "") where TCloakedStruct : struct
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value != null) { palantírReveal(value.Value, Master); }
        else { StyleFormatter.Format(defaultValue, 0, Sb, ""); }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringRevealOrNullNext<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal)
        where TCloaked : TCloakedBase
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value == null) { Sb.Append(Settings.NullStyle); }
        else
        {
            Sb.Append("\"");
            palantírReveal(value, Master);
            Sb.Append("\"");
        }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringRevealOrNullNext<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value == null) { Sb.Append(Settings.NullStyle); }
        else
        {
            Sb.Append("\"");
            palantírReveal(value.Value, Master);
            Sb.Append("\"");
        }
        return ConditionalValueTypeSuffix();
    }
    
    public TExt FieldStringRevealOrDefaultNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value, string defaultValue = "")
        where TBearer : IStringBearer
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value != null) { value.RevealState(Master); }
        else { StyleFormatter.Format(defaultValue, 0, Sb, ""); }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }
    
    public TExt FieldStringRevealOrDefaultNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value, string defaultValue = "")
        where TBearer : struct, IStringBearer
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value != null) { value.Value.RevealState(Master); }
        else { StyleFormatter.Format(defaultValue, 0, Sb, ""); }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringRevealOrNullNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value)
        where TBearer : IStringBearer
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            Sb.Append("\"");
            value.RevealState(Master);
            Sb.Append("\"");
        }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringRevealOrNullNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value)
        where TBearer : struct, IStringBearer
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            Sb.Append("\"");
            value.Value.RevealState(Master);
            Sb.Append("\"");
        }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringNext(ReadOnlySpan<char> nonJsonfieldName, Span<char> value, string? formatString = null)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        this.AppendFormattedOrNull(value, formatString);
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        this.AppendOrNull(value);
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value, string defaultValue = "", string? formatString = null)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value.Length > 0)
        {
            StyleFormatter.Format(value, 0, Sb, formatString, value.Length);
        }
        else
        {
            StyleFormatter.Format(defaultValue, 0, Sb, formatString ?? "");
        }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value, string? formatString = null)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value.Length == 0)
        {
            Sb.Append(Settings.NullStyle); 
        }
        else
        {
            Sb.Append("\"");
            StyleFormatter.Format(value, 0, Sb, formatString, value.Length);
            Sb.Append("\"");
        }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length, string? formatString = null)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var caplength = Math.Clamp(length, 0, Math.Max(0, value.Length - capStart));
            if (caplength > 0)
            {
                Sb.Append("\"");
                StyleFormatter.Format(value, capStart, Sb, formatString, caplength);
                Sb.Append("\"");
            }
            else
            {
                if (formatString?.Length > 0)
                {
                    ((ReadOnlySpan<char>)formatString).ExtractExtendedStringFormatStages(out var prefix, out _, out _
                                                                                       , out _, out _, out _, out var suffix);
                    if (prefix.Length > 0 || suffix.Length > 0)
                    {
                        Sb.Append("\"");
                        StyleFormatter.Format("", 0, Sb, formatString);
                        Sb.Append("\"");
                        return ConditionalValueTypeSuffix();
                    }
                }
                Sb.Append(Settings.NullStyle);
            }
        }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length
      , string defaultValue = "", string? formatString = null)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, Math.Max(0, value.Length - capStart));
            if (capLength > 0)
            {
                StyleFormatter.Format(value, capStart, Sb, formatString, capLength);
            }
            else
            {
                StyleFormatter.Format(defaultValue, 0, Sb, formatString);
                Sb.Append("\"");
                return ConditionalValueTypeSuffix();
            }
        }
        else { StyleFormatter.Format(defaultValue, 0, Sb, formatString ?? ""); }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringNext(ReadOnlySpan<char> nonJsonfieldName, char[] value, int startIndex, int length, string? formatString = null)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value != null!)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, Math.Max(0, value.Length - capStart));
            this.AppendFormattedOrNull(value, formatString, capStart, capLength);
        }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex, int length, string defaultValue = "", string? formatString = null)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            Sb.Append("\"");
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, Math.Max(0, value.Length - capStart));
            if (capLength > 0) { this.AppendFormattedOrNull(value, formatString, capStart, capLength); }
            else { StyleFormatter.Format(defaultValue, 0, Sb, formatString ?? ""); }
            Sb.Append("\"");
        }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex, int length, string? formatString = null)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            Sb.Append("\"");
            if (value != null!)
            {
                var capStart  = Math.Clamp(startIndex, 0, value.Length);
                var capLength = Math.Clamp(length, 0, Math.Max(0, value.Length - capStart));
                if (capLength > 0) { this.AppendFormattedOrNull(value, formatString, capStart, capLength); }
            }
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
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        this.AppendFormattedOrNull(value, formatString);
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, string defaultValue, string? formatString = null)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value != null) { this.AppendFormattedOrNull(value, formatString); }
        else { StyleFormatter.Format(defaultValue, 0, Sb, formatString ?? ""); }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, string? formatString = null)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
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
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value != null!)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, Math.Max(0, value.Length - capStart));
            if (capLength > 0) { this.AppendFormattedOrNull(value, formatString, capStart, capLength); }
        }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex, int length
      , string defaultValue = "", string? formatString = null)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, Math.Max(0, value.Length - capStart));
            if (capLength > 0) { this.AppendFormattedOrNull(value, formatString, capStart, capLength); }
            else { StyleFormatter.Format(defaultValue, 0, Sb, formatString ?? ""); }
        }
        else { StyleFormatter.Format(defaultValue, 0, Sb, formatString ?? ""); }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex, int length, string? formatString = null)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            Sb.Append("\"");
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, Math.Max(0, value.Length - capStart));
            if (capLength > 0) { this.AppendFormattedOrNull(value, formatString, capStart, capLength); }
            Sb.Append("\"");
        }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder value, int startIndex, int length, string? formatString = null)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value != null!)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, Math.Max(0, value.Length - capStart));
            if (capLength > 0) { this.AppendFormattedOrNull(value, formatString, capStart, capLength); }
        }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex, int length
      , string defaultValue = "", string? formatString = null)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var caplength = Math.Clamp(length, 0, Math.Max(0, value.Length - capStart));
            if (caplength > 0) { this.AppendFormattedOrNull(value, formatString, capStart, caplength); }
            else { StyleFormatter.Format(defaultValue, 0, Sb, formatString ?? ""); }
        }
        else { StyleFormatter.Format(defaultValue, 0, Sb, formatString ?? ""); }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TExt FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex, int length, string? formatString = null)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            Sb.Append("\"");
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var caplength = Math.Clamp(length, 0, Math.Max(0, value.Length - capStart));
            if (caplength > 0) { this.AppendFormattedOrNull(value, formatString); }
            Sb.Append("\"");
        }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public TExt StringMatchOrNullNext<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value, string? formatString = null)
    {
        if (SkipBody) return StyleTypeBuilder;
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var isStringLike = typeof(TAny).IsAnyTypeHoldingChars();
            if(!isStringLike) Sb.Append("\"");
            this.AppendMatchFormattedOrNull(value, formatString ?? "");
            if(!isStringLike) Sb.Append("\"");
        }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public TExt StringMatchOrDefaultNext<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value, string defaultValue = "", string? formatString = null)
    {
        if (SkipBody) return StyleTypeBuilder;
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var isStringLike = typeof(TAny).IsAnyTypeHoldingChars();
            if(!isStringLike) Sb.Append("\"");
            this.AppendMatchFormattedOrNull(value, formatString ?? "");
            if(!isStringLike) Sb.Append("\"");
        }
        else
        {
            Sb.Append("\"");
            StyleFormatter.Format(defaultValue, 0, Sb, formatString ?? ""); 
            Sb.Append("\"");
        }
        return ConditionalValueTypeSuffix();
    }

    public TExt ConditionalValueTypeSuffix()
    {
        if (ValueInComplexType) { this.AddGoToNext(); }
        return StyleTypeBuilder;
    }
}
