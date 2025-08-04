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

        ValueInComplexType = isComplex && typeBuilderPortableState.OwningAppender.Style.AllowsUnstructured();
        
        return this;
    }

    public void ConditionalCollectionPrefix()
    {
        if (ValueInComplexType)
        {
            Sb.Append("_value: ");
        }
    }

    public bool NotJson => Style.IsNotJson();

    public TExt FieldValueNext<TValue>(string nonJsonfieldName, TValue value)
    {
        (NotJson ? this.FieldNameJoin(nonJsonfieldName) : Sb).AppendOrNull(value, this, false);
        return ConditionalCollectionSuffix();
    }

    public TExt FieldValueNext<TValue>(string nonJsonfieldName, TValue value, string? formatString = null)
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        if (formatString != null) this.AppendFormattedOrNull(value, formatString);
        else this.AppendOrNull(value,  false);
        return ConditionalCollectionSuffix();
    }

    public TExt FieldValueNext<TStruct>(string nonJsonfieldName, TStruct value, StructStyler<TStruct> structToString) where TStruct : struct
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        structToString(value, OwningAppender);
        return ConditionalCollectionSuffix();
    }

    public TExt FieldValueNext<TStruct>(string nonJsonfieldName, TStruct? value, StructStyler<TStruct> structToString) where TStruct : struct
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        if (value == null)
        {
            Sb.Append(OwningAppender.NullStyle);
        }
        else
        {
            structToString(value.Value, OwningAppender);
        }
        return ConditionalCollectionSuffix();
    }

    public TExt FieldStringNext<TStruct>(string nonJsonfieldName, TStruct value, StructStyler<TStruct> structToString) where TStruct : struct
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"");
        structToString(value, OwningAppender);
        Sb.Append("\"");
        return ConditionalCollectionSuffix();
    }

    public TExt FieldStringNext<TStruct>(string nonJsonfieldName, TStruct? value, StructStyler<TStruct> structToString) where TStruct : struct
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        if (value == null)
        {
            Sb.Append(OwningAppender.NullStyle);
        }
        else
        {
            Sb.Append("\"");
            structToString(value.Value, OwningAppender);
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

    public TExt FieldValueNext(string nonJsonfieldName, ReadOnlySpan<char> value)
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append(value);
        return ConditionalCollectionSuffix();
    }

    public TExt FieldStringNext(string nonJsonfieldName, string value)
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        Sb.Append("\"").Append(value).Append("\"");
        return ConditionalCollectionSuffix();
    }

    public TExt FieldValueNext(string nonJsonfieldName, string? value, int startIndex, int length)
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

    public TExt FieldValueNext(string nonJsonfieldName, ICharSequence? value, int startIndex, int length)
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart = Math.Clamp(startIndex, 0, value.Length);
            var capEnd   = Math.Clamp(length, 0, value.Length);
            Sb.Append(value, capStart, capEnd);
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

    public TExt FieldValueNext(string nonJsonfieldName, StringBuilder? value, int startIndex, int length)
    {
        if (NotJson) this.FieldNameJoin(nonJsonfieldName);
        if (value != null)
        {
            var capStart = Math.Clamp(startIndex, 0, value.Length);
            var capEnd   = Math.Clamp(length, 0, value.Length);
            Sb.Append(value, capStart, capEnd);
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

public static class ValueBuilderCompAccessExtensions
{
    public static TExt MaybeFieldValueToMaybeNext<TValue, TExt>(this IStringBuilder sb, string nonJsonfieldName, TValue value
      , ValueBuilderCompAccess<TExt> stb) where TExt : StyledTypeBuilder
    {
        if(stb.NotJson) stb.FieldNameJoin(nonJsonfieldName, stb);
        stb.AppendOrNull(value, false);
        return stb.ConditionalCollectionSuffix();
    }
}
