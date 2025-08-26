using System.Numerics;
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

    public TExt BooleanWithFallback(string nonJsonfieldName, bool? value, bool defaultValue = false) =>
        Stb.FieldValueNext(nonJsonfieldName, value ?? defaultValue);
    
    public TExt Boolean(string nonJsonfieldName, bool? value) =>
        Stb.FieldValueNext(nonJsonfieldName, value);
    
    public TExt NumberWithFallback<TNum>(string nonJsonfieldName, TNum? value, TNum fallbackValue )  where TNum : struct, INumber<TNum> => 
        Stb.FieldValueNext(nonJsonfieldName, value ?? fallbackValue);
    
    public TExt Number<TNum>(string nonJsonfieldName, TNum? value)  where TNum : struct, INumber<TNum> =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value);
    
    public TExt NumberFrom<TEnum>(string nonJsonfieldName, TEnum? value)  where TEnum : Enum => 
        Stb.FieldEnumValueOrNullNext(nonJsonfieldName, value);
    
    public TExt NumberFromWithFallback<TEnum>(string nonJsonfieldName, TEnum? value, TEnum fallbackValue)  where TEnum : Enum =>
        Stb.FieldEnumValueOrNullNext(nonJsonfieldName, value ?? fallbackValue);

    public TExt ValueWithFallback<T>(string nonJsonfieldName, T? value
      , CustomTypeStyler<T> customTypeStyler, T fallbackValue) where T : struct =>
        Stb.FieldValueNext(nonJsonfieldName, value ?? fallbackValue, customTypeStyler);

    public TExt Value<T, TBase>(string nonJsonfieldName, T value
      , CustomTypeStyler<TBase> customTypeStyler) where T : TBase =>
        Stb.FieldValueNext(nonJsonfieldName, value, customTypeStyler);

    public TExt ValueWithFallback<T, TBase>(string nonJsonfieldName, T? value
      , CustomTypeStyler<TBase> customTypeStyler, T fallbackValue) 
        where T : class, TBase where TBase : class =>
        Stb.FieldValueNext(nonJsonfieldName, value ?? fallbackValue, customTypeStyler);

    public TExt String<T, TBase>(string nonJsonfieldName, T value
      , CustomTypeStyler<TBase> customTypeStyler) where T : TBase  =>
        Stb.FieldStringNext(nonJsonfieldName, value, customTypeStyler);

    public TExt StringWithFallback<T>(string nonJsonfieldName, T? value
      , CustomTypeStyler<T> customTypeStyler, T fallbackValue) where T : struct =>
        Stb.FieldStringNext(nonJsonfieldName, value ?? fallbackValue, customTypeStyler);

    public TExt String(string nonJsonfieldName, ReadOnlySpan<char> value) => 
        Stb.FieldStringNext(nonJsonfieldName, value);

    public TExt StringWithFallback<TEnum>(string nonJsonfieldName, TEnum? value, TEnum fallback) where TEnum : Enum => 
        Stb.FieldEnumStringNext(nonJsonfieldName, value ?? fallback);

    public TExt StringEnum<TEnum>(string nonJsonfieldName, TEnum? value) where TEnum : Enum => 
        Stb.FieldEnumStringOrNullNext(nonJsonfieldName, value);
    
    public TExt StringWithFallback(string nonJsonfieldName, string? value, string fallbackValue) => 
        Stb.FieldStringNext(nonJsonfieldName, value ?? fallbackValue);
    
    public TExt String(string nonJsonfieldName, string? value) => 
        Stb.FieldStringOrNullNext(nonJsonfieldName, value);
    
    public TExt StringWithFallback(string nonJsonfieldName, string? value, int startIndex, int length, string fallbackValue = "") =>
        Stb.FieldStringNext(nonJsonfieldName, value, startIndex, length, fallbackValue);
    
    public TExt String(string nonJsonfieldName, string? value, int startIndex, int length) =>
        Stb.FieldStringOrNullNext(nonJsonfieldName, value, startIndex, length);
    
    public TExt StringWithFallback(string nonJsonfieldName, char[]? value, int startIndex, int length, string fallbackValue = "") =>
        Stb.FieldStringNext(nonJsonfieldName, value, startIndex, length, fallbackValue);
    
    public TExt String(string nonJsonfieldName, char[]? value, int startIndex, int length) =>
        Stb.FieldStringOrNullNext(nonJsonfieldName, value, startIndex, length);
    
    public TExt StringWithFallback(string nonJsonfieldName, ICharSequence? value, string fallbackValue = "") => 
        Stb.FieldStringNext(nonJsonfieldName, value, fallbackValue);
    
    public TExt String(string nonJsonfieldName, ICharSequence? value) => 
        Stb.FieldStringOrNullNext(nonJsonfieldName, value);

    public TExt StringWithFallback(string nonJsonfieldName, StringBuilder? value, string fallbackValue = "") =>
        Stb.FieldStringNext(nonJsonfieldName, value, fallbackValue);

    public TExt String(string nonJsonfieldName, StringBuilder? value) =>
        Stb.FieldStringOrNullNext(nonJsonfieldName, value);
    
    public TExt StringWithFallback(string nonJsonfieldName, IStyledToStringObject? value, string fallbackValue = "") =>
        Stb.FieldStringNext(nonJsonfieldName, value, fallbackValue);
    
    public TExt String(string nonJsonfieldName, IStyledToStringObject? value) =>
        Stb.FieldStringOrNullNext(nonJsonfieldName, value);
    
    public IScopeDelimitedStringBuilder StartDelimitedStringBuilder()  => Stb.StartDelimitedStringBuilder();

    public TExt NumberFrom(string nonJsonfieldName, ReadOnlySpan<char> value, decimal fallbackValue = decimal.Zero)  =>
        Stb.FieldValueNext(nonJsonfieldName, value, fallbackValue);

    public TExt NumberFrom(string nonJsonfieldName, string? value, int startIndex = 0, int length = int.MaxValue, decimal fallbackValue = decimal.Zero)  =>
        Stb.FieldValueNext(nonJsonfieldName, value, startIndex, length, fallbackValue);

    public TExt NumberFrom(string nonJsonfieldName, char[]? value, int fromIndex = 0, int length = int.MaxValue, decimal fallbackValue = decimal.Zero)  =>
        Stb.FieldValueNext(nonJsonfieldName, value, fromIndex, length, fallbackValue);

    public TExt NumberFrom(string nonJsonfieldName, ICharSequence value, int fromIndex = 0, int length = int.MaxValue, decimal fallbackValue = decimal.Zero)  =>
        Stb.FieldValueNext(nonJsonfieldName, value, fromIndex, length, fallbackValue);

    public TExt NumberFrom(string nonJsonfieldName, StringBuilder? value, int fromIndex = 0, int length = int.MaxValue, decimal fallbackValue = decimal.Zero) =>
        Stb.FieldValueNext(nonJsonfieldName, value, fromIndex, length, fallbackValue);
}