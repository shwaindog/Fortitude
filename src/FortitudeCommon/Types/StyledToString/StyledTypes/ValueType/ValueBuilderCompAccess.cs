// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.Mutable.Strings;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.ValueType;

public class ValueBuilderCompAccess<TExt> : InternalStyledTypeBuilderComponentAccess<TExt> where TExt : StyledTypeBuilder
{
    public bool ValueInComplexType { get; private set; }

    public ValueBuilderCompAccess<TExt> InitializeValueBuilderCompAccess
        (TExt externalTypeBuilder, StyledTypeBuilder.StyleTypeBuilderPortableState typeBuilderPortableState, bool isComplex)
    {
        Initialize(externalTypeBuilder, typeBuilderPortableState);

        ValueInComplexType          = isComplex && typeBuilderPortableState.OwningAppender.Style.AllowsUnstructured();
        OnFinishedWithStringBuilder = FinishUsingStringBuilder;
        
        return this;
    }
    
    private Action<IScopeDelimitedStringBuilder>? OnFinishedWithStringBuilder { get; set; }

    public bool NotJson => Style.IsNotJson();

    public TExt FieldValueNext(string nonJsonfieldName, bool? value)
    {
        (NotJson ? this.FieldNameJoin(nonJsonfieldName) : Sb).AddNullOrValue(value, this, false);
        return ConditionalCollectionSuffix();
    }

    public TExt FieldValueNext<TFmt>(string nonJsonfieldName, TFmt? value, string? formatString = null) where TFmt : ISpanFormattable
    {
        var sb= (NotJson ? this.FieldNameJoin(nonJsonfieldName) : Sb);
        if (formatString != null) sb.AppendFormattedOrNull(value, formatString);
        else sb.AddNullOrValue(value, this, false);
        return ConditionalCollectionSuffix();
    }

    public TExt FieldValueNext<TToStyle, TStylerType>(string nonJsonfieldName, TToStyle value, CustomTypeStyler<TStylerType> customTypeStyler) 
        where TToStyle : TStylerType
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        customTypeStyler(value, OwningAppender);
        return ConditionalCollectionSuffix();
    }

    public TExt FieldValueOrNullNext<TToStyle, TStylerType>(string nonJsonfieldName, TToStyle? value, CustomTypeStyler<TStylerType> customTypeStyler) 
        where TToStyle : TStylerType
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        if (value == null)
        {
            Sb.Append(Settings.NullStyle);
        }
        else
        {
            customTypeStyler(value, OwningAppender);
        }
        return ConditionalCollectionSuffix();
    }

    public TExt FieldStringNext<TToStyle, TStylerType>(string nonJsonfieldName, TToStyle value, CustomTypeStyler<TStylerType> customTypeStyler
      , string defaultValue = "") where TToStyle : TStylerType
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value != null)
        {
            customTypeStyler(value, OwningAppender);
        }
        else
        {
            Sb.Append(defaultValue);
        }
        Sb.Append("\"");
        return ConditionalCollectionSuffix();
    }

    public TExt FieldStringOrNullNext<TToStyle, TStylerType>(string nonJsonfieldName, TToStyle? value, CustomTypeStyler<TStylerType> customTypeStyler) 
        where TToStyle : TStylerType
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        if (value == null)
        {
            Sb.Append(Settings.NullStyle);
        }
        else
        {
            Sb.Append("\"");
            customTypeStyler(value, OwningAppender);
            Sb.Append("\"");
        }
        return ConditionalCollectionSuffix();
    }

    public TExt FieldStringNext<TFmt>(string nonJsonfieldName, TFmt? value, string? formatString = null) where TFmt : ISpanFormattable
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        if(formatString != null) Sb.AppendFormattedOrNull(value, formatString, true);
        else Sb.Append("\"").AppendOrNull(value).Append("\"");
        return ConditionalCollectionSuffix();
    }

    public TExt FieldEnumStringOrNullNext<TEnum>(string nonJsonfieldName, TEnum? value) where TEnum : Enum
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        if (value == null)
        {
            Sb.Append(Settings.NullStyle);
        }
        else
        {
            Sb.Append("\"");
            this.AppendOrNull(value);
            Sb.Append("\"");
        }
        return ConditionalCollectionSuffix();
    }

    public TExt FieldStringNext(string nonJsonfieldName, ReadOnlySpan<char> value)
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"").Append(value).Append("\"");
        return ConditionalCollectionSuffix();
    }

    public TExt FieldValueNext(string nonJsonfieldName, ReadOnlySpan<char> value, decimal fallbackValue = decimal.Zero)
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        if(value.Length != 0) Sb.Append(value);
        else Sb.Append(fallbackValue);
        return ConditionalCollectionSuffix();
    }

    public TExt FieldStringNext(string nonJsonfieldName, string value)
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"").Append(value).Append("\"");
        return ConditionalCollectionSuffix();
    }

    public TExt FieldStringOrNullNext(string nonJsonfieldName, string? value)
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            Sb.Append("\"");
            Sb.Append(value);
            Sb.Append("\"");
        }
        else
        {
            Sb.Append(Settings.NullStyle);
        }
        return ConditionalCollectionSuffix();
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

    public TExt FieldValueNext(string nonJsonfieldName, string? value, int startIndex, int length, decimal fallbackValue = decimal.Zero)
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart = Math.Clamp(startIndex, 0, value.Length);
            var caplength = Math.Clamp(length, 0, value.Length);
            if (caplength > 0)
            {
                Sb.Append(value, capStart, caplength);
            }
            else
            {
                Sb.Append(fallbackValue);
            }
        }
        else
        {
            Sb.Append(fallbackValue);
        }
        return ConditionalCollectionSuffix();
    }

    public TExt FieldValueNext(string nonJsonfieldName, char[]? value, int startIndex, int length, string defaultValue)
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart = Math.Clamp(startIndex, 0, value.Length);
            var capEnd = Math.Clamp(length, 0, value.Length);
            Sb.Append(value, capStart, capEnd);
        }
        return ConditionalCollectionSuffix();
    }

    public TExt FieldValueNext(string nonJsonfieldName, char[]? value, int startIndex, int length, decimal fallbackValue = decimal.Zero)
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var caplength = Math.Clamp(length, 0, value.Length);
            if (caplength > 0)
            {
                Sb.Append(value, capStart, caplength);
            }
            else
            {
                Sb.Append(fallbackValue);
            }
        }
        else
        {
            Sb.Append(fallbackValue);
        }
        return ConditionalCollectionSuffix();
    }

    public TExt FieldStringNext(string nonJsonfieldName, ICharSequence? value, string defaultValue)
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value != null)
        {
            Sb.Append(value);
        }
        else
        {
            Sb.Append(defaultValue);
        }
        Sb.Append("\"");
        return ConditionalCollectionSuffix();
    }

    public TExt FieldStringOrNullNext(string nonJsonfieldName, ICharSequence? value)
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            Sb.Append("\"");
            Sb.Append(value);
            Sb.Append("\"");
        }
        else
        {
            Sb.Append(Settings.NullStyle);
        }
        return ConditionalCollectionSuffix();
    }

    public TExt FieldValueNext(string nonJsonfieldName, ICharSequence? value, string defaultValue)
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            Sb.Append(value);
        }
        else
        {
            Sb.Append(defaultValue);
        }
        return ConditionalCollectionSuffix();
    }

    public TExt FieldValueNext(string nonJsonfieldName, ICharSequence? value, int startIndex, int length, decimal fallbackValue = decimal.Zero)
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var caplength = Math.Clamp(length, 0, value.Length);
            if (caplength > 0)
            {
                Sb.Append(value, capStart, caplength);
            }
            else
            {
                Sb.Append(fallbackValue);
            }
        }
        else
        {
            Sb.Append(fallbackValue);
        }
        return ConditionalCollectionSuffix();
    }

    public TExt FieldStringNext(string nonJsonfieldName, StringBuilder? value, string defaultValue = "")
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value != null)
        {
            Sb.Append(value);
        }
        else
        {
            Sb.Append(defaultValue);
        }
        Sb.Append("\"");
        return ConditionalCollectionSuffix();
    }

    public TExt FieldStringOrNullNext(string nonJsonfieldName, StringBuilder? value)
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            Sb.Append("\"");
            Sb.Append(value);
            Sb.Append("\"");
        }
        else
        {
            Sb.Append(Settings.NullStyle);
        }
        return ConditionalCollectionSuffix();
    }

    public TExt FieldStringNext(string nonJsonfieldName, string? value, int startIndex, int length, string defaultValue = "")
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value != null)
        {
            Sb.Append(value, startIndex, length);
        }
        else
        {
            Sb.Append(defaultValue);
        }
        Sb.Append("\"");
        return ConditionalCollectionSuffix();
    }

    public TExt FieldStringOrNullNext(string nonJsonfieldName, string? value, int startIndex, int length)
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            Sb.Append("\"");
            Sb.Append(value, startIndex, length);
            Sb.Append("\"");
        }
        else
        {
            Sb.Append(Settings.NullStyle);
        }
        return ConditionalCollectionSuffix();
    }

    public TExt FieldStringNext(string nonJsonfieldName, char[]? value, int startIndex, int length, string defaultValue = "")
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value != null)
        {
            Sb.Append(value, startIndex, length);
        }
        else
        {
            Sb.Append(defaultValue);
        }
        Sb.Append("\"");
        return ConditionalCollectionSuffix();
    }

    public TExt FieldStringOrNullNext(string nonJsonfieldName, char[]? value, int startIndex, int length)
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            Sb.Append("\"");
            Sb.Append(value, startIndex, length);
            Sb.Append("\"");
        }
        else
        {
            Sb.Append(Settings.NullStyle);
        }
        return ConditionalCollectionSuffix();
    }

    public TExt FieldValueNext(string nonJsonfieldName, StringBuilder? value, int startIndex, int length, decimal fallbackValue = decimal.Zero)
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart  = Math.Clamp(startIndex, 0, value.Length);
            var caplength = Math.Clamp(length, 0, value.Length);
            if (caplength > 0)
            {
                Sb.Append(value, capStart, caplength);
            }
            else
            {
                Sb.Append(fallbackValue);
            }
        }
        else
        {
            Sb.Append(fallbackValue);
        }
        return ConditionalCollectionSuffix();
    }

    public TExt FieldStringNext(string nonJsonfieldName, IStyledToStringObject? value, string defaultValue = "")
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        if (value != null)
        {
            value.ToString(OwningAppender);
        }
        else
        {
            Sb.Append(defaultValue);
        }
        Sb.Append("\"");
        return ConditionalCollectionSuffix();
    }

    public TExt FieldStringOrNullNext(string nonJsonfieldName, IStyledToStringObject? value)
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            Sb.Append("\"");
            value.ToString(OwningAppender);
            Sb.Append("\"");
        }
        else
        {
            Sb.Append(Settings.NullStyle);
        }
        return ConditionalCollectionSuffix();
    }

    public TExt ConditionalCollectionSuffix()
    {
        if (ValueInComplexType)
        {
            Sb.AddGoToNext(this);
            return StyleTypeBuilder;
        }
        return StyleTypeBuilder;
    }
}
