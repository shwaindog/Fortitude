using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.DieCasting.ValueType;
using FortitudeCommon.Types.StringsOfPower.Forge;

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
          , int  existingRefId)
    {
        InitializeTypedStyledTypeBuilder(typeBeingBuilt, master, typeSettings, typeName, remainingGraphDepth, typeFormatting,  existingRefId);

        return this;
    }
    
    public override void Start()
    {
        if (PortableState.AppenderSettings.SkipTypeParts.HasTypeStartFlag()) return;
        if (Stb.ValueInComplexType)
        {
            CompAccess.StyleFormatter.AppendComplexTypeOpening(CompAccess, CompAccess.TypeBeingBuilt, CompAccess.TypeName);
        }
        else
        {
            CompAccess.StyleFormatter.AppendValueTypeOpening(CompAccess, CompAccess.TypeBeingBuilt);
        }
    }

    public override bool IsComplexType => false;
    
    public override void AppendOpening()
    {
        CompAccess.StyleFormatter.AppendValueTypeOpening(CompAccess, CompAccess.TypeBeingBuilt);
    }
    
    public override void AppendClosing()
    {
        CompAccess.StyleFormatter.AppendValueTypeClosing(CompAccess, CompAccess.TypeBeingBuilt);
    }


    protected ValueTypeDieCast<TExt> Stb => (ValueTypeDieCast<TExt>)CompAccess;

    public TExt BooleanWithFallback(string nonJsonfieldName, bool? value, bool defaultValue = false) =>
        Stb.FieldValueNext(nonJsonfieldName, value ?? defaultValue);
    
    public TExt Boolean(string nonJsonfieldName, bool? value) =>
        Stb.FieldValueNext(nonJsonfieldName, value);
    
    public TExt Number<TFmt>(string nonJsonfieldName, TFmt value, string? formatString = null )  where TFmt : ISpanFormattable => 
        Stb.FieldValueNext(nonJsonfieldName, value, formatString);
    
    public TExt NumberWithFallback<TFmt>(string nonJsonfieldName, TFmt? value, TFmt fallbackValue, string? formatString = null )  
        where TFmt : ISpanFormattable => 
        Stb.FieldValueNext(nonJsonfieldName, value ?? fallbackValue, formatString);

    public TExt ValueWithFallback<T>(string nonJsonfieldName, T? value
      , PalantírReveal<T> palantírReveal, T fallbackValue) where T : struct =>
        Stb.FieldValueNext(nonJsonfieldName, value ?? fallbackValue, palantírReveal);

    public TExt Value<T, TBase>(string nonJsonfieldName, T value
      , PalantírReveal<TBase> palantírReveal) where T : TBase =>
        Stb.FieldValueNext(nonJsonfieldName, value, palantírReveal);

    public TExt ValueMatch(object value, string? formatString = null) =>
        Stb.ValueMatchNext(value, formatString);

    public TExt ValueWithFallback<T, TBase>(string nonJsonfieldName, T? value
      , PalantírReveal<TBase> palantírReveal, T fallbackValue) 
        where T : class, TBase where TBase : class =>
        Stb.FieldValueNext(nonJsonfieldName, value ?? fallbackValue, palantírReveal);

    public TExt String<T, TBase>(string nonJsonfieldName, T value
      , PalantírReveal<TBase> palantírReveal) where T : TBase  =>
        Stb.FieldStringNext(nonJsonfieldName, value, palantírReveal);

    public TExt StringWithFallback<T, TBase>(string nonJsonfieldName, T? value
      , PalantírReveal<TBase> palantírReveal, T fallbackValue) where T : TBase =>
        Stb.FieldStringNext(nonJsonfieldName, value ?? fallbackValue, palantírReveal);

    public TExt String(string nonJsonfieldName, ReadOnlySpan<char> value) => 
        Stb.FieldStringNext(nonJsonfieldName, value);

    public TExt String<TFmt>(string nonJsonfieldName, TFmt? value, string? formatString) where TFmt : ISpanFormattable => 
        Stb.FieldStringNext(nonJsonfieldName, value, formatString);

    public TExt StringWithFallback<TFmt>(string nonJsonfieldName, TFmt? value, TFmt fallback, string? formatString) where TFmt : ISpanFormattable => 
        Stb.FieldStringNext(nonJsonfieldName, value ?? fallback, formatString);
    
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
    
    public TExt StringWithFallback(string nonJsonfieldName, IStringBearer? value, string fallbackValue = "") =>
        Stb.FieldStringNext(nonJsonfieldName, value, fallbackValue);
    
    public TExt String(string nonJsonfieldName, IStringBearer? value) =>
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