// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString.StyledTypes.ValueType;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.SimpleType;

public class ComplexValueTypeBuilder : MultiValueTypeBuilder<ComplexValueTypeBuilder>
{
    public ComplexValueTypeBuilder InitializeComplexValueTypeBuilder
        (IStyleTypeAppenderBuilderAccess owningAppender, TypeAppendSettings typeSettings, string typeName)
    {
        InitializeMultiValueTypeBuilder(owningAppender, typeSettings, typeName);

        return this;
    }
    
    protected override string TypeOpeningDelimiter => Stb.ValueInComplexType ? "{" : "";
    protected override string TypeClosingDelimiter => Stb.ValueInComplexType ? "}" : "";

    protected ValueBuilderCompAccess<ComplexValueTypeBuilder> Stb =>
        (ValueBuilderCompAccess<ComplexValueTypeBuilder>)CompAccess;

    protected override void SourceBuilderComponentAccess()
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.OwningAppender.Recycler;
        CompAccess = recycler.Borrow<ValueBuilderCompAccess<ComplexValueTypeBuilder>>()
                             .InitializeValueBuilderCompAccess(this, PortableState, true);
    }

    public ComplexValueTypeBuilder Boolean(string nonJsonfieldName, bool value) => Stb.FieldValueNext(nonJsonfieldName, value); 
    public ComplexValueTypeBuilder Boolean(string nonJsonfieldName, bool? value, bool defaultValue = false) => Stb.FieldValueNext(nonJsonfieldName, value ?? defaultValue); 
    public ComplexValueTypeBuilder Number(string nonJsonfieldName, byte value) => Stb.FieldValueNext(nonJsonfieldName, value); 
    public ComplexValueTypeBuilder Number(string nonJsonfieldName, sbyte value) => Stb.FieldValueNext(nonJsonfieldName, value);
    public ComplexValueTypeBuilder Number(string nonJsonfieldName, short value) => Stb.FieldValueNext(nonJsonfieldName, value);
    public ComplexValueTypeBuilder Number(string nonJsonfieldName, ushort value) => Stb.FieldValueNext(nonJsonfieldName, value);
    public ComplexValueTypeBuilder Number(string nonJsonfieldName, int value) => Stb.FieldValueNext(nonJsonfieldName, value);
    public ComplexValueTypeBuilder Number(string nonJsonfieldName, uint value) => Stb.FieldValueNext(nonJsonfieldName, value);
    public ComplexValueTypeBuilder Number(string nonJsonfieldName, long value) => Stb.FieldValueNext(nonJsonfieldName, value);
    public ComplexValueTypeBuilder Number(string nonJsonfieldName, ulong value) => Stb.FieldValueNext(nonJsonfieldName, value);
    public ComplexValueTypeBuilder Number(string nonJsonfieldName, byte? value, byte defaultValue = 0) => Stb.FieldValueNext(nonJsonfieldName, value ?? defaultValue); 
    public ComplexValueTypeBuilder Number(string nonJsonfieldName, sbyte? value, sbyte defaultValue = 0) => Stb.FieldValueNext(nonJsonfieldName, value ?? defaultValue); 
    public ComplexValueTypeBuilder Number(string nonJsonfieldName, short? value, short defaultValue = 0) => Stb.FieldValueNext(nonJsonfieldName, value ?? defaultValue);
    public ComplexValueTypeBuilder Number(string nonJsonfieldName, ushort? value, ushort defaultValue = 0) => Stb.FieldValueNext(nonJsonfieldName, value ?? defaultValue);
    public ComplexValueTypeBuilder Number(string nonJsonfieldName, int? value, int defaultValue = 0) => Stb.FieldValueNext(nonJsonfieldName, value ?? defaultValue);
    public ComplexValueTypeBuilder Number(string nonJsonfieldName, uint? value, uint defaultValue = 0) => Stb.FieldValueNext(nonJsonfieldName, value ?? defaultValue);
    public ComplexValueTypeBuilder Number(string nonJsonfieldName, long? value, long defaultValue = 0) => Stb.FieldValueNext(nonJsonfieldName, value ?? defaultValue);
    public ComplexValueTypeBuilder Number(string nonJsonfieldName, ulong? value, ulong defaultValue = 0) => Stb.FieldValueNext(nonJsonfieldName, value ?? defaultValue);
    public ComplexValueTypeBuilder Number(string nonJsonfieldName, decimal value, string? formatString = null) => Stb.FieldValueNext(nonJsonfieldName, value, formatString);
    public ComplexValueTypeBuilder Number(string nonJsonfieldName, double value, string? formatString = null) => Stb.FieldValueNext(nonJsonfieldName, value, formatString);
    public ComplexValueTypeBuilder Number(string nonJsonfieldName, float value, string? formatString = null) => Stb.FieldValueNext(nonJsonfieldName, value, formatString);

    public ComplexValueTypeBuilder StructAsValue<T>(string nonJsonfieldName, T value
      , StructStyler<T> structToString) where T : struct =>
        Stb.FieldValueNext(nonJsonfieldName, value, structToString);

    public ComplexValueTypeBuilder StructAsValue<T>(string nonJsonfieldName, T? value
      , StructStyler<T> structToString, T defaultValue = default) where T : struct =>
        Stb.FieldValueNext(nonJsonfieldName, value, structToString);

    public ComplexValueTypeBuilder StructAsString<T>(string nonJsonfieldName, T value
      , StructStyler<T> structToString) where T : struct  =>
        Stb.FieldStringNext(nonJsonfieldName, value, structToString);

    public ComplexValueTypeBuilder StructAsString<T>(string nonJsonfieldName, T? value
      , StructStyler<T> structToString, T defaultValue = default) where T : struct =>
        Stb.FieldStringNext(nonJsonfieldName, value, structToString);

    public ComplexValueTypeBuilder String(string nonJsonfieldName, ReadOnlySpan<char> value) => 
        Stb.FieldStringNext(nonJsonfieldName, value);
    
    public ComplexValueTypeBuilder String(string nonJsonfieldName, string? value, string defaultValue = "") => 
        Stb.FieldStringNext(nonJsonfieldName, value ?? defaultValue);
    
    public ComplexValueTypeBuilder String(string nonJsonfieldName, string? value, int startIndex, int length, string defaultValue = "") =>
        Stb.FieldStringNext(nonJsonfieldName, value, startIndex, length, defaultValue);
    
    public ComplexValueTypeBuilder String(string nonJsonfieldName, IStyledToStringObject? value, string defaultValue = "") =>
        Stb.FieldStringNext(nonJsonfieldName, value, defaultValue);
    
    public ComplexValueTypeBuilder String(string nonJsonfieldName, ICharSequence? value, string defaultValue = "") => 
        Stb.FieldStringNext(nonJsonfieldName, value, defaultValue);

    public ComplexValueTypeBuilder String(string nonJsonfieldName, StringBuilder? value, string defaultValue = "") =>
        Stb.FieldStringNext(nonJsonfieldName, value, defaultValue);

    public ComplexValueTypeBuilder ValueFromString(string nonJsonfieldName, ReadOnlySpan<char> value)  =>
        Stb.FieldValueNext(nonJsonfieldName, value);

    public ComplexValueTypeBuilder ValueFromString(string nonJsonfieldName, string? value, int startIndex = 0, int length = int.MaxValue)  =>
        Stb.FieldValueNext(nonJsonfieldName, value, startIndex, length);

    public ComplexValueTypeBuilder ValueFromString(string nonJsonfieldName, char[] value, int fromIndex = 0, int length = int.MaxValue)  =>
        Stb.FieldValueNext(nonJsonfieldName, value, fromIndex, length, "");

    public ComplexValueTypeBuilder ValueFromString(string nonJsonfieldName, ICharSequence value, int fromIndex = 0, int length = int.MaxValue)  =>
        Stb.FieldValueNext(nonJsonfieldName, value, fromIndex, length);

    public ComplexValueTypeBuilder FormattedString(string nonJsonfieldName, IStringBuilder? value, int fromIndex = 0, int length = int.MaxValue)  =>
        Stb.FieldValueNext(nonJsonfieldName, value, fromIndex, length);

    public ComplexValueTypeBuilder FormattedString(string nonJsonfieldName, StringBuilder? value, int fromIndex = 0, int length = int.MaxValue) =>
        Stb.FieldValueNext(nonJsonfieldName, value, fromIndex, length);
}
