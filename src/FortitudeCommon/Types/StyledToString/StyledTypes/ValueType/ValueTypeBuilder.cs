using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable.Strings;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.ValueType;

public class ValueTypeBuilder<TExt> : TypedStyledTypeBuilder<TExt> where TExt : StyledTypeBuilder
{
    public ValueTypeBuilder<TExt> InitializeValueTypeBuilder
        (IStyleTypeAppenderBuilderAccess owningAppender, TypeAppendSettings typeSettings, string typeName)
    {
        InitializeTypedStyledTypeBuilder(owningAppender, typeSettings, typeName);

        return this;
    }
    
    public override void Start()
    {
        if ( !CompAccess.StyleTypeBuilder.Style.IsJson()
          && !PortableState.AppenderSettings.IgnoreWriteFlags.HasTypeNameFlag()
          && PortableState.TypeName.IsNotNullOrEmpty())
        {
            CompAccess.Sb.Append(PortableState.TypeName);
        }
        if (!PortableState.AppenderSettings.IgnoreWriteFlags.HasTypeStartFlag())
        {
            CompAccess.Sb.Append(TypeOpeningDelimiter);
            CompAccess.IncrementIndent();
        }
    }
    
    protected override string TypeOpeningDelimiter => Stb.ValueInComplexType && CompAccess.StyleTypeBuilder.Style.IsNotJson() ? "." : "";
    
    protected override string TypeClosingDelimiter => "";

    protected ValueBuilderCompAccess<TExt> Stb => (ValueBuilderCompAccess<TExt>)CompAccess;

    public TExt Boolean(string nonJsonfieldName, bool value) => Stb.FieldValueNext(nonJsonfieldName, value); 
    public TExt Boolean(string nonJsonfieldName, bool? value, bool defaultValue = false) => Stb.FieldValueNext(nonJsonfieldName, value ?? defaultValue); 
    public TExt Number(string nonJsonfieldName, byte value) => Stb.FieldValueNext(nonJsonfieldName, value); 
    public TExt Number(string nonJsonfieldName, sbyte value) => Stb.FieldValueNext(nonJsonfieldName, value);
    public TExt Number(string nonJsonfieldName, short value) => Stb.FieldValueNext(nonJsonfieldName, value);
    public TExt Number(string nonJsonfieldName, ushort value) => Stb.FieldValueNext(nonJsonfieldName, value);
    public TExt Number(string nonJsonfieldName, int value) => Stb.FieldValueNext(nonJsonfieldName, value);
    public TExt Number(string nonJsonfieldName, uint value) => Stb.FieldValueNext(nonJsonfieldName, value);
    public TExt Number(string nonJsonfieldName, long value) => Stb.FieldValueNext(nonJsonfieldName, value);
    public TExt Number(string nonJsonfieldName, ulong value) => Stb.FieldValueNext(nonJsonfieldName, value);
    public TExt Number(string nonJsonfieldName, byte? value, byte defaultValue = 0) => Stb.FieldValueNext(nonJsonfieldName, value ?? defaultValue); 
    public TExt Number(string nonJsonfieldName, sbyte? value, sbyte defaultValue = 0) => Stb.FieldValueNext(nonJsonfieldName, value ?? defaultValue); 
    public TExt Number(string nonJsonfieldName, short? value, short defaultValue = 0) => Stb.FieldValueNext(nonJsonfieldName, value ?? defaultValue);
    public TExt Number(string nonJsonfieldName, ushort? value, ushort defaultValue = 0) => Stb.FieldValueNext(nonJsonfieldName, value ?? defaultValue);
    public TExt Number(string nonJsonfieldName, int? value, int defaultValue = 0) => Stb.FieldValueNext(nonJsonfieldName, value ?? defaultValue);
    public TExt Number(string nonJsonfieldName, uint? value, uint defaultValue = 0) => Stb.FieldValueNext(nonJsonfieldName, value ?? defaultValue);
    public TExt Number(string nonJsonfieldName, long? value, long defaultValue = 0) => Stb.FieldValueNext(nonJsonfieldName, value ?? defaultValue);
    public TExt Number(string nonJsonfieldName, ulong? value, ulong defaultValue = 0) => Stb.FieldValueNext(nonJsonfieldName, value ?? defaultValue);
    public TExt Number(string nonJsonfieldName, decimal value, string? formatString = null) => Stb.FieldValueNext(nonJsonfieldName, value, formatString);
    public TExt Number(string nonJsonfieldName, double value, string? formatString = null) => Stb.FieldValueNext(nonJsonfieldName, value, formatString);
    public TExt Number(string nonJsonfieldName, float value, string? formatString = null) => Stb.FieldValueNext(nonJsonfieldName, value, formatString);

    public TExt StructAsValue<T>(string nonJsonfieldName, T value
      , CustomTypeStyler<T> customTypeStyler) where T : struct =>
        Stb.FieldValueNext(nonJsonfieldName, value, customTypeStyler);

    public TExt StructAsValue<T>(string nonJsonfieldName, T? value
      , CustomTypeStyler<T> customTypeStyler, T defaultValue = default) where T : struct =>
        Stb.FieldValueNext(nonJsonfieldName, value, customTypeStyler);

    public TExt StructAsValue<T, TBase>(string nonJsonfieldName, T? value
      , CustomTypeStyler<TBase> customTypeStyler, T? defaultValue = null) 
        where T : class, TBase where TBase : class =>
        Stb.FieldValueNext(nonJsonfieldName, value, customTypeStyler);

    public TExt StructAsString<T>(string nonJsonfieldName, T value
      , CustomTypeStyler<T> customTypeStyler) where T : struct  =>
        Stb.FieldStringNext(nonJsonfieldName, value, customTypeStyler);

    public TExt StructAsString<T>(string nonJsonfieldName, T? value
      , CustomTypeStyler<T> customTypeStyler, T defaultValue = default) where T : struct =>
        Stb.FieldStringNext(nonJsonfieldName, value, customTypeStyler);

    public TExt String(string nonJsonfieldName, ReadOnlySpan<char> value) => 
        Stb.FieldStringNext(nonJsonfieldName, value);
    
    public TExt String(string nonJsonfieldName, string? value, string defaultValue = "") => 
        Stb.FieldStringNext(nonJsonfieldName, value ?? defaultValue);
    
    public TExt String(string nonJsonfieldName, string? value, int startIndex, int length, string defaultValue = "") =>
        Stb.FieldStringNext(nonJsonfieldName, value, startIndex, length, defaultValue);
    
    public TExt String(string nonJsonfieldName, char[]? value, int startIndex, int length, string defaultValue = "") =>
        Stb.FieldStringNext(nonJsonfieldName, value, startIndex, length, defaultValue);
    
    public TExt String(string nonJsonfieldName, ICharSequence? value, string defaultValue = "") => 
        Stb.FieldStringNext(nonJsonfieldName, value, defaultValue);

    public TExt String(string nonJsonfieldName, StringBuilder? value, string defaultValue = "") =>
        Stb.FieldStringNext(nonJsonfieldName, value, defaultValue);
    
    public TExt String(string nonJsonfieldName, IStyledToStringObject? value, string defaultValue = "") =>
        Stb.FieldStringNext(nonJsonfieldName, value, defaultValue);
    
    public TExt String<T, TBase>(string nonJsonfieldName, T? value, CustomTypeStyler<TBase> customTypeStyler, string defaultValue = "")
        where T : class, TBase where TBase : class =>
        Stb.FieldStringNext(nonJsonfieldName, value, customTypeStyler, defaultValue);
    
    public IScopeDelimitedStringBuilder StartDelimitedStringBuilder()  => Stb.StartDelimitedStringBuilder();

    public TExt ValueFromString(string nonJsonfieldName, ReadOnlySpan<char> value)  =>
        Stb.FieldValueNext(nonJsonfieldName, value);

    public TExt ValueFromString(string nonJsonfieldName, string? value, int startIndex = 0, int length = int.MaxValue)  =>
        Stb.FieldValueNext(nonJsonfieldName, value, startIndex, length);

    public TExt ValueFromString(string nonJsonfieldName, ICharSequence value, int fromIndex = 0, int length = int.MaxValue)  =>
        Stb.FieldValueNext(nonJsonfieldName, value, fromIndex, length);

    public TExt FormattedString(string nonJsonfieldName, IStringBuilder? value, int fromIndex = 0, int length = int.MaxValue)  =>
        Stb.FieldValueNext(nonJsonfieldName, value, fromIndex, length);

    public TExt FormattedString(string nonJsonfieldName, StringBuilder? value, int fromIndex = 0, int length = int.MaxValue) =>
        Stb.FieldValueNext(nonJsonfieldName, value, fromIndex, length);
}