using System.Text;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString.StyledTypes.ValueType;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.SimpleType;

public class SimpleValueTypeBuilder : TypedStyledTypeBuilder<SimpleValueTypeBuilder>
{
    public SimpleValueTypeBuilder InitializeSimpleValueTypeBuilder
        (IStyleTypeAppenderBuilderAccess owningAppender, TypeAppendSettings typeSettings, string typeName)
    {
        InitializeTypedStyledTypeBuilder(owningAppender, typeSettings, typeName);

        return this;
    }
    
    protected override string TypeOpeningDelimiter => Stb.ValueInComplexType ? "{" : "";
    protected override string TypeClosingDelimiter => Stb.ValueInComplexType ? "}" : "";

    protected ValueBuilderCompAccess<SimpleValueTypeBuilder> Stb =>
        (ValueBuilderCompAccess<SimpleValueTypeBuilder>)CompAccess;

    protected override void SourceBuilderComponentAccess()
    {
        var recycler = MeRecyclable.Recycler ?? PortableState.OwningAppender.Recycler;
        CompAccess = recycler.Borrow<ValueBuilderCompAccess<SimpleValueTypeBuilder>>()
                             .InitializeValueBuilderCompAccess(this, PortableState, true);
    }

    public SimpleValueTypeBuilder Boolean(string nonJsonfieldName, bool value) => Stb.FieldValueNext(nonJsonfieldName, value); 
    public SimpleValueTypeBuilder Boolean(string nonJsonfieldName, bool? value, bool defaultValue = false) => Stb.FieldValueNext(nonJsonfieldName, value ?? defaultValue); 
    public SimpleValueTypeBuilder Number(string nonJsonfieldName, byte value) => Stb.FieldValueNext(nonJsonfieldName, value); 
    public SimpleValueTypeBuilder Number(string nonJsonfieldName, sbyte value) => Stb.FieldValueNext(nonJsonfieldName, value);
    public SimpleValueTypeBuilder Number(string nonJsonfieldName, short value) => Stb.FieldValueNext(nonJsonfieldName, value);
    public SimpleValueTypeBuilder Number(string nonJsonfieldName, ushort value) => Stb.FieldValueNext(nonJsonfieldName, value);
    public SimpleValueTypeBuilder Number(string nonJsonfieldName, int value) => Stb.FieldValueNext(nonJsonfieldName, value);
    public SimpleValueTypeBuilder Number(string nonJsonfieldName, uint value) => Stb.FieldValueNext(nonJsonfieldName, value);
    public SimpleValueTypeBuilder Number(string nonJsonfieldName, long value) => Stb.FieldValueNext(nonJsonfieldName, value);
    public SimpleValueTypeBuilder Number(string nonJsonfieldName, ulong value) => Stb.FieldValueNext(nonJsonfieldName, value);
    public SimpleValueTypeBuilder Number(string nonJsonfieldName, byte? value, byte defaultValue = 0) => Stb.FieldValueNext(nonJsonfieldName, value ?? defaultValue); 
    public SimpleValueTypeBuilder Number(string nonJsonfieldName, sbyte? value, sbyte defaultValue = 0) => Stb.FieldValueNext(nonJsonfieldName, value ?? defaultValue); 
    public SimpleValueTypeBuilder Number(string nonJsonfieldName, short? value, short defaultValue = 0) => Stb.FieldValueNext(nonJsonfieldName, value ?? defaultValue);
    public SimpleValueTypeBuilder Number(string nonJsonfieldName, ushort? value, ushort defaultValue = 0) => Stb.FieldValueNext(nonJsonfieldName, value ?? defaultValue);
    public SimpleValueTypeBuilder Number(string nonJsonfieldName, int? value, int defaultValue = 0) => Stb.FieldValueNext(nonJsonfieldName, value ?? defaultValue);
    public SimpleValueTypeBuilder Number(string nonJsonfieldName, uint? value, uint defaultValue = 0) => Stb.FieldValueNext(nonJsonfieldName, value ?? defaultValue);
    public SimpleValueTypeBuilder Number(string nonJsonfieldName, long? value, long defaultValue = 0) => Stb.FieldValueNext(nonJsonfieldName, value ?? defaultValue);
    public SimpleValueTypeBuilder Number(string nonJsonfieldName, ulong? value, ulong defaultValue = 0) => Stb.FieldValueNext(nonJsonfieldName, value ?? defaultValue);
    public SimpleValueTypeBuilder Number(string nonJsonfieldName, decimal value, string? formatString = null) => Stb.FieldValueNext(nonJsonfieldName, value, formatString);
    public SimpleValueTypeBuilder Number(string nonJsonfieldName, double value, string? formatString = null) => Stb.FieldValueNext(nonJsonfieldName, value, formatString);
    public SimpleValueTypeBuilder Number(string nonJsonfieldName, float value, string? formatString = null) => Stb.FieldValueNext(nonJsonfieldName, value, formatString);

    public SimpleValueTypeBuilder StructAsValue<T>(string nonJsonfieldName, T value
      , StructStyler<T> structToString) where T : struct =>
        Stb.FieldValueNext(nonJsonfieldName, value, structToString);

    public SimpleValueTypeBuilder StructAsValue<T>(string nonJsonfieldName, T? value
      , StructStyler<T> structToString, T defaultValue = default) where T : struct =>
        Stb.FieldValueNext(nonJsonfieldName, value, structToString);

    public SimpleValueTypeBuilder StructAsString<T>(string nonJsonfieldName, T value
      , StructStyler<T> structToString) where T : struct  =>
        Stb.FieldStringNext(nonJsonfieldName, value, structToString);

    public SimpleValueTypeBuilder StructAsString<T>(string nonJsonfieldName, T? value
      , StructStyler<T> structToString, T defaultValue = default) where T : struct =>
        Stb.FieldStringNext(nonJsonfieldName, value, structToString);

    public SimpleValueTypeBuilder String(string nonJsonfieldName, ReadOnlySpan<char> value) => 
        Stb.FieldStringNext(nonJsonfieldName, value);
    
    public SimpleValueTypeBuilder String(string nonJsonfieldName, string? value, string defaultValue = "") => 
        Stb.FieldStringNext(nonJsonfieldName, value ?? defaultValue);
    
    public SimpleValueTypeBuilder String(string nonJsonfieldName, string? value, int startIndex, int length, string defaultValue = "") =>
        Stb.FieldStringNext(nonJsonfieldName, value, startIndex, length, defaultValue);
    
    public SimpleValueTypeBuilder String(string nonJsonfieldName, char[]? value, int startIndex, int length, string defaultValue = "") =>
        Stb.FieldStringNext(nonJsonfieldName, value, startIndex, length, defaultValue);
    
    public SimpleValueTypeBuilder String(string nonJsonfieldName, IStyledToStringObject? value, string defaultValue = "") =>
        Stb.FieldStringNext(nonJsonfieldName, value, defaultValue);
    
    public SimpleValueTypeBuilder String(string nonJsonfieldName, ICharSequence? value, string defaultValue = "") => 
        Stb.FieldStringNext(nonJsonfieldName, value, defaultValue);

    public SimpleValueTypeBuilder String(string nonJsonfieldName, StringBuilder? value, string defaultValue = "") =>
        Stb.FieldStringNext(nonJsonfieldName, value, defaultValue);

    public SimpleValueTypeBuilder ValueFromString(string nonJsonfieldName, ReadOnlySpan<char> value)  =>
        Stb.FieldValueNext(nonJsonfieldName, value);

    public SimpleValueTypeBuilder ValueFromString(string nonJsonfieldName, string? value, int startIndex = 0, int length = int.MaxValue)  =>
        Stb.FieldValueNext(nonJsonfieldName, value, startIndex, length);

    public SimpleValueTypeBuilder ValueFromString(string nonJsonfieldName, ICharSequence value, int fromIndex = 0, int length = int.MaxValue)  =>
        Stb.FieldValueNext(nonJsonfieldName, value, fromIndex, length);

    public SimpleValueTypeBuilder FormattedString(string nonJsonfieldName, IStringBuilder? value, int fromIndex = 0, int length = int.MaxValue)  =>
        Stb.FieldValueNext(nonJsonfieldName, value, fromIndex, length);

    public SimpleValueTypeBuilder FormattedString(string nonJsonfieldName, StringBuilder? value, int fromIndex = 0, int length = int.MaxValue) =>
        Stb.FieldValueNext(nonJsonfieldName, value, fromIndex, length);
}