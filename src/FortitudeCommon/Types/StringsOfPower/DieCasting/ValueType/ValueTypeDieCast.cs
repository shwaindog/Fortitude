// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ValueType;

public class ValueTypeDieCast<TVMold> : TypeMolderDieCast<TVMold> where TVMold : TypeMolder
{
    protected bool ValueInComplexType { get; private set; }

    public ValueTypeDieCast<TVMold> InitializeValueBuilderCompAccess
        (TVMold externalTypeBuilder, TypeMolder.StyleTypeBuilderPortableState typeBuilderPortableState, bool isComplex)
    {
        Initialize(externalTypeBuilder, typeBuilderPortableState);

        ValueInComplexType          = isComplex && typeBuilderPortableState.Master.Style.AllowsUnstructured();
        OnFinishedWithStringBuilder = FinishUsingStringBuilder;

        return this;
    }

    private Action<IScopeDelimitedStringBuilder>? OnFinishedWithStringBuilder { get; set; }

    protected bool NotJson => Style.IsNotJson();

    public IScopeDelimitedStringBuilder StartDelimitedStringBuilder()
    {
        if (Style.IsJson()) Sb.Append("\"");
        var scopedSb = (IScopeDelimitedStringBuilder)Sb;
        scopedSb.OnScopeEndedAction = OnFinishedWithStringBuilder;
        return scopedSb;
    }

    public TVMold FieldValueNext(ReadOnlySpan<char> nonJsonfieldName, bool? value, string formatString = ""
        , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        (ValueInComplexType && nonJsonfieldName.Length > 0 ? this.FieldNameJoin(nonJsonfieldName) : this)
            .AppendFormattedOrNull(value, formatString);
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldFmtValueOrNullNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) 
        where TFmt : ISpanFormattable
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        this.AppendFormatted(value, formatString);
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldFmtValueOrNullNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) 
        where TFmt : struct, ISpanFormattable
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        this.AppendNullableFormattedOrNull(value, formatString);
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldValueOrDefaultNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, ReadOnlySpan<char> defaultValue
      , string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value == null) { StyleFormatter.Format(defaultValue, 0, Sb, formatString); }
        else
        {
            this.AppendFormatted(value, formatString);
        }
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldValueOrDefaultNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, ReadOnlySpan<char> defaultValue
      , string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmt : struct, ISpanFormattable
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value == null) { StyleFormatter.Format(defaultValue, 0, Sb, formatString); }
        else
        {
            this.AppendNullableFormattedOrNull(value, formatString);
        }
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldValueNext<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked value, PalantírReveal<TCloakedBase> palantírReveal
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCloaked : TCloakedBase
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value == null) { Sb.Append(Settings.NullStyle); }
        else { palantírReveal(value, Master); }
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldValueOrNullNext<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value == null) { Sb.Append(Settings.NullStyle); }
        else { palantírReveal(value.Value, Master); }
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldValueOrNullNext<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TCloakedBase
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value == null) { Sb.Append(Settings.NullStyle); }
        else { palantírReveal(value, Master); }
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldValueOrDefaultNext<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal, ReadOnlySpan<char> defaultValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TCloakedBase
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value == null) { StyleFormatter.Format(defaultValue, 0, Sb, ""); }
        else { palantírReveal(value, Master); }
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldValueOrDefaultNext<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, ReadOnlySpan<char> defaultValue, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value == null) { StyleFormatter.Format(defaultValue, 0, Sb, ""); }
        else { palantírReveal(value.Value, Master); }
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldValueOrDefaultNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value, string defaultValue = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    where TBearer : IStringBearer
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null) { value.RevealState(Master); }
        else { Sb.Append(defaultValue); }
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldValueOrDefaultNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value, string defaultValue = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    where TBearer : struct, IStringBearer
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null) { value.Value.RevealState(Master); }
        else { Sb.Append(defaultValue); }
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldValueOrNullNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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

    public TVMold FieldValueOrNullNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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

    public TVMold FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, Span<char> value, ReadOnlySpan<char> fallbackValue
      , string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        StyleFormatter.Format(value.Length != 0 ? value : fallbackValue, 0, Sb, formatString);
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value, ReadOnlySpan<char> fallbackValue
      , string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        StyleFormatter.Format(value.Length != 0 ? value : fallbackValue, 0, Sb, formatString);
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldValueOrNullNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value.Length != 0) { StyleFormatter.Format(value, 0, Sb, formatString); }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length
      , ReadOnlySpan<char> defaultValue, string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0) 
            { StyleFormatter.Format(value, capStart, Sb, formatString, capLength); }
            else
            {
                StyleFormatter.Format(defaultValue, 0, Sb, formatString);
            }
        }
        else { StyleFormatter.Format(defaultValue, 0, Sb, formatString); }
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldValueOrNullNext(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length
      , string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0) { StyleFormatter.Format(value, capStart, Sb, formatString, capLength); }
            else
            {
                if (formatString.Length > 0)
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

    public TVMold FieldValueNext(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex = 0, int length = int.MaxValue
      , string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            this.AppendFormattedOrNull(value, formatString, capStart, capLength);
        }
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex, int length, string defaultValue = ""
      , string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0) { StyleFormatter.Format(value, capStart, Sb, formatString, capLength); }
            else
            {
                StyleFormatter.Format(defaultValue,0, Sb, formatString);
            }
        }
        else { StyleFormatter.Format(defaultValue, 0, Sb, formatString); }
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldValueOrNullNext(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex, int length
      , string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0) { StyleFormatter.Format(value, capStart, Sb, formatString, capLength); }
            else
            {
                if (formatString.Length > 0)
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

    public TVMold FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex, int length
      , string defaultValue = "", string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0) { StyleFormatter.Format(value, capStart, Sb, formatString, capLength); }
            else
            {
                if (formatString.Length > 0)
                {
                    ((ReadOnlySpan<char>)formatString).ExtractExtendedStringFormatStages(out var prefix, out _, out _
                                                                                       , out _, out _, out _, out var suffix);
                    if (prefix.Length > 0 || suffix.Length > 0)
                    {
                        StyleFormatter.Format(defaultValue, 0, Sb, formatString);
                        return ConditionalValueTypeSuffix();
                    }
                }
                StyleFormatter.Format(defaultValue, 0, Sb, formatString);
            }
        }
        else { StyleFormatter.Format(defaultValue, 0, Sb, formatString); }
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldValueOrNullNext(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex, int length
      , string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0) { StyleFormatter.Format(value, capStart, Sb, formatString, capLength); }
            else
            {
                if (formatString.Length > 0)
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

    public TVMold FieldValueOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex, int length
      , string defaultValue = "", string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0)
            {
                StyleFormatter.Format(value, capStart, Sb, formatString, capLength);
            }
            else
            {
                if (formatString.Length > 0)
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
                StyleFormatter.Format(defaultValue, 0, Sb, formatString);
            }
        }
        else { StyleFormatter.Format(defaultValue, 0, Sb, formatString); }
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldValueOrNullNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex, int length
      , string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, value.Length - capStart);
            if (capLength > 0) { StyleFormatter.Format(value, capStart, Sb, formatString, capLength); }
            else
            {
                if (formatString.Length > 0)
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

    public TVMold ValueMatchOrNullNext<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (SkipBody) return StyleTypeBuilder;
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null) { this.AppendMatchFormattedOrNull(value, formatString, DisableAutoDelimiting); }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public TVMold ValueMatchOrDefaultNext<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value, string defaultValue = "", string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (SkipBody) return StyleTypeBuilder;
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            
            this.AppendMatchFormattedOrNull(value, formatString, DisableAutoDelimiting);
        }
        else
        {
            StyleFormatter.Format(defaultValue, 0, Sb, formatString); 
        }
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldStringNext(ReadOnlySpan<char> nonJsonfieldName, bool? value, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) 
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

    public TVMold FieldStringOrNullNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable
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

    public TVMold FieldStringOrDefaultNext<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, string defaultValue = ""
      , string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmt : ISpanFormattable
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value != null) { this.AppendFormattedOrNull(value, formatString); }
        else { StyleFormatter.Format(defaultValue, 0, Sb, formatString); }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldStringOrNullNext<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
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

    public TVMold FieldStringOrDefaultNext<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value, string defaultValue = ""
      , string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TFmtStruct : struct, ISpanFormattable
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value != null) { this.AppendFormattedOrNull(value, formatString); }
        else { StyleFormatter.Format(defaultValue, 0, Sb, formatString); }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold FieldStringRevealOrDefaultNext<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal, string defaultValue = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags) 
        where TCloaked : TCloakedBase
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value != null) { palantírReveal(value, Master); }
        else { StyleFormatter.Format(defaultValue, 0, Sb, ""); }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold FieldStringRevealOrDefaultNext<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string defaultValue = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags) 
        where TCloakedStruct : struct
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value != null) { palantírReveal(value.Value, Master); }
        else { StyleFormatter.Format(defaultValue, 0, Sb, ""); }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldStringRevealOrNullNext<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value
      , PalantírReveal<TCloakedBase> palantírReveal, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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

    public TVMold FieldStringRevealOrNullNext<TCloakedStruct>(ReadOnlySpan<char> nonJsonfieldName, TCloakedStruct? value
      , PalantírReveal<TCloakedStruct> palantírReveal, FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct
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
    
    public TVMold FieldStringRevealOrDefaultNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value, string defaultValue = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value != null) { value.RevealState(Master); }
        else { StyleFormatter.Format(defaultValue, 0, Sb, ""); }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }
    
    public TVMold FieldStringRevealOrDefaultNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value, string defaultValue = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TBearer : struct, IStringBearer
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value != null) { value.Value.RevealState(Master); }
        else { StyleFormatter.Format(defaultValue, 0, Sb, ""); }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldStringRevealOrNullNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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

    public TVMold FieldStringRevealOrNullNext<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) where TBearer : struct, IStringBearer
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

    public TVMold FieldStringNext(ReadOnlySpan<char> nonJsonfieldName, Span<char> value, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        this.AppendFormattedOrNull(value, formatString);
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldStringNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        this.AppendOrNull(value);
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value, string defaultValue = ""
      , string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value.Length > 0)
        {
            StyleFormatter.Format(value, 0, Sb, formatString, value.Length);
        }
        else
        {
            StyleFormatter.Format(defaultValue, 0, Sb, formatString);
        }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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

    public TVMold FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, Math.Max(0, value.Length - capStart));
            if (capLength > 0)
            {
                Sb.Append("\"");
                StyleFormatter.Format(value, capStart, Sb, formatString, capLength);
                Sb.Append("\"");
            }
            else
            {
                if (formatString.Length > 0)
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

    public TVMold FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length
      , string defaultValue = "", string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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
        else { StyleFormatter.Format(defaultValue, 0, Sb, formatString); }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldStringNext(ReadOnlySpan<char> nonJsonfieldName, char[] value, int startIndex, int length, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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

    public TVMold FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex, int length
      , string defaultValue = "", string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            Sb.Append("\"");
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, Math.Max(0, value.Length - capStart));
            if (capLength > 0) { this.AppendFormattedOrNull(value, formatString, capStart, capLength); }
            else { StyleFormatter.Format(defaultValue, 0, Sb, formatString); }
            Sb.Append("\"");
        }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex, int length, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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

    private void FinishUsingStringBuilder(IScopeDelimitedStringBuilder finishedBuilding)
    {
        if (Style.IsJson()) finishedBuilding.Append("\"");
    }

    public TVMold FieldStringNext(ReadOnlySpan<char> nonJsonfieldName, ICharSequence value, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        this.AppendFormattedOrNull(value, formatString);
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldStringNext(ReadOnlySpan<char> nonJsonfieldName, ICharSequence value, int startIndex, int length, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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

    public TVMold FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex, int length
      , string defaultValue = "", string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, Math.Max(0, value.Length - capStart));
            if (capLength > 0) { this.AppendFormattedOrNull(value, formatString, capStart, capLength); }
            else { StyleFormatter.Format(defaultValue, 0, Sb, formatString); }
        }
        else { StyleFormatter.Format(defaultValue, 0, Sb, formatString); }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex, int length, string formatString = ""
        , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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

    public TVMold FieldStringNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder value, int startIndex, int length, string formatString = ""
        , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
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

    public TVMold FieldStringOrDefaultNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex, int length
      , string defaultValue = "", string formatString = "", FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, Math.Max(0, value.Length - capStart));
            if (capLength > 0) { this.AppendFormattedOrNull(value, formatString, capStart, capLength); }
            else { StyleFormatter.Format(defaultValue, 0, Sb, formatString); }
        }
        else { StyleFormatter.Format(defaultValue, 0, Sb, formatString); }
        Sb.Append("\"");
        return ConditionalValueTypeSuffix();
    }

    public TVMold FieldStringOrNullNext(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex, int length, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            Sb.Append("\"");
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var capLength = Math.Clamp(length, 0, Math.Max(0, value.Length - capStart));
            if (capLength > 0) { this.AppendFormattedOrNull(value, formatString); }
            Sb.Append("\"");
        }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public TVMold StringMatchOrNullNext<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value, string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (SkipBody) return StyleTypeBuilder;
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var isStringLike = typeof(TAny).IsAnyTypeHoldingChars();
            if(!isStringLike) Sb.Append("\"");
            this.AppendMatchFormattedOrNull(value, formatString);
            if(!isStringLike) Sb.Append("\"");
        }
        else { Sb.Append(Settings.NullStyle); }
        return ConditionalValueTypeSuffix();
    }

    public TVMold StringMatchOrDefaultNext<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value, string defaultValue = "", string formatString = ""
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (SkipBody) return StyleTypeBuilder;
        if (ValueInComplexType && nonJsonfieldName.Length > 0) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var isStringLike = typeof(TAny).IsAnyTypeHoldingChars();
            if(!isStringLike) Sb.Append("\"");
            this.AppendMatchFormattedOrNull(value, formatString);
            if(!isStringLike) Sb.Append("\"");
        }
        else
        {
            Sb.Append("\"");
            StyleFormatter.Format(defaultValue, 0, Sb, formatString); 
            Sb.Append("\"");
        }
        return ConditionalValueTypeSuffix();
    }

    public TVMold ConditionalValueTypeSuffix()
    {
        if (ValueInComplexType) { this.AddGoToNext(); }
        return StyleTypeBuilder;
    }
}
