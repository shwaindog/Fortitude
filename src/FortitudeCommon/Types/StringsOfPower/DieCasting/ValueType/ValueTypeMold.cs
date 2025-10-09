using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
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
    
    public TExt AsValue(ReadOnlySpan<char> nonJsonfieldName, bool value) =>
        Stb.FieldValueNext(nonJsonfieldName, value);
    
    public TExt AsValue(bool value) =>
        Stb.FieldValueNext("", value);
    
    public TExt AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, bool? value) =>
        Stb.FieldValueNext(nonJsonfieldName, value);
    
    public TExt AsValueOrNull(bool? value) =>
        Stb.FieldValueNext("", value);
    
    public TExt AsValue<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value, string? formatString = null )  where TFmt : ISpanFormattable => 
        Stb.FieldValueNext(nonJsonfieldName, value, formatString);
    
    public TExt AsValue<TFmt>(TFmt value, string? formatString = null ) where TFmt : ISpanFormattable => 
        Stb.FieldValueNext("", value, formatString);
    
    public TExt AsValueOrNull<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, string? formatString = null )  where TFmt : class, ISpanFormattable => 
        Stb.FieldValueNext(nonJsonfieldName, value, formatString);
    
    public TExt AsValueOrNull<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value, string? formatString = null )  where TFmtStruct : struct, ISpanFormattable => 
        Stb.FieldValueNext(nonJsonfieldName, value, formatString);
    
    public TExt AsValueOrNull<TFmt>(TFmt? value, string? formatString = null )  where TFmt : ISpanFormattable => 
        Stb.FieldValueNext("", value, formatString);
    
    public TExt AsValueOrNull<TFmt>(TFmt? value, string? formatString = null )  where TFmt : struct, ISpanFormattable => 
        Stb.FieldValueNext("", value, formatString);
    
    public TExt AsValueOrDefault<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, TFmt defaultValue, string? formatString = null ) 
        where TFmt : ISpanFormattable => 
        Stb.FieldValueNext(nonJsonfieldName, value ?? defaultValue, formatString);
    
    public TExt AsValueOrDefault<TFmt>(TFmt? value, TFmt defaultValue, string? formatString = null )  where TFmt : ISpanFormattable => 
        Stb.FieldValueOrDefaultNext("", value ?? defaultValue, formatString);
    
    public TExt AsValueOrDefault<TFmtStruct>(TFmtStruct? value, TFmtStruct defaultValue, string? formatString = null )  where TFmtStruct : struct, ISpanFormattable => 
        Stb.FieldValueOrDefaultNext("", value ?? defaultValue, formatString);
    
    public TExt AsValueOrDefault<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value, TFmtStruct defaultValue, string? formatString = null )  where TFmtStruct : struct, ISpanFormattable => 
        Stb.FieldValueOrDefaultNext("", value ?? defaultValue, formatString);
    
    public TExt AsValueOrDefault<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, string defaultValue = "", string? formatString = null ) 
        where TFmt : ISpanFormattable => 
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString);
    
    public TExt AsValueOrDefault<TFmt>(TFmt? value, string defaultValue = "", string? formatString = null )  where TFmt : ISpanFormattable => 
        Stb.FieldValueOrDefaultNext("", value, defaultValue, formatString);
    
    public TExt AsValueOrDefault<TFmtStruct>(TFmtStruct? value, string defaultValue = "", string? formatString = null )  where TFmtStruct : struct, ISpanFormattable => 
        Stb.FieldValueOrDefaultNext("", value, defaultValue, formatString);
    
    public TExt AsValueOrDefault<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value, string defaultValue = "", string? formatString = null )  where TFmtStruct : struct, ISpanFormattable => 
        Stb.FieldValueOrDefaultNext("", value, defaultValue, formatString);

    public TExt RevealAsValue<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked value , PalantírReveal<TCloakedBase> palantírReveal) where TCloaked : TCloakedBase =>
        Stb.FieldValueNext(nonJsonfieldName, value, palantírReveal);

    public TExt RevealAsValue<TCloaked, TCloakedBase>(TCloaked value , PalantírReveal<TCloakedBase> palantírReveal) where TCloaked : TCloakedBase =>
        Stb.FieldValueNext("", value, palantírReveal);

    public TExt RevealAsValueOrNull<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value, PalantírReveal<TCloakedBase> palantírReveal) where TCloaked : TCloakedBase =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, palantírReveal);

    public TExt RevealAsValueOrNull<TCloaked, TCloakedBase>(TCloaked? value , PalantírReveal<TCloakedBase> palantírReveal) where TCloaked : TCloakedBase =>
        Stb.FieldValueOrNullNext("", value, palantírReveal);

    public TExt RevealAsValueOrDefault<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value , PalantírReveal<TCloakedBase> palantírReveal, ReadOnlySpan<char> defaultValue) where TCloaked : TCloakedBase =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, palantírReveal, defaultValue);

    public TExt RevealAsValueOrDefault<TCloaked, TCloakedBase>(TCloaked? value , PalantírReveal<TCloakedBase> palantírReveal, ReadOnlySpan<char> defaultValue) where TCloaked : TCloakedBase =>
        Stb.FieldValueOrDefaultNext("", value, palantírReveal, defaultValue);
    
    public TExt RevealAsValue<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer value) where TBearer : IStringBearer =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value);
    
    public TExt RevealAsValueOrNull<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value) where TBearer : IStringBearer =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value);
    
    public TExt RevealAsValueOrDefault<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value, string defaultValue = "") where TBearer : IStringBearer =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue);
    
    public TExt AsValue(ReadOnlySpan<char> nonJsonfieldName, Span<char> value, string? formatString = null) => 
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, "0", formatString);
    
    public TExt AsValueOrZero(Span<char> value) => Stb.FieldValueOrDefaultNext("", value, "0");
    
    public TExt AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, Span<char> value, string? formatString = null) => 
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, formatString);
    
    public TExt AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, Span<char> value, ReadOnlySpan<char> defaultValue, string? formatString = null) => 
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString);
    
    public TExt AsValue(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value, string? formatString = null) => 
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, "0", formatString);
    
    public TExt AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value, string? formatString = null) => 
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, formatString);
    
    public TExt AsValueOrZero(ReadOnlySpan<char> value) => Stb.FieldValueOrDefaultNext("", value, "0");
    
    public TExt AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value, ReadOnlySpan<char> defaultValue, string? formatString = null) => 
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString);
    
    public TExt AsValue(Span<char> value) => 
        Stb.FieldValueOrDefaultNext("", value, "0", "");
    
    public TExt AsValue(ReadOnlySpan<char> value) => 
        Stb.FieldValueOrDefaultNext("", value, "0", "");
    
    public TExt AsValue(string value) => 
        Stb.FieldValueOrDefaultNext("", value, "0", "");
    
    public TExt AsValue(ReadOnlySpan<char> nonJsonfieldName, string value, string? formatString = null) => 
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, "0", formatString);
    
    public TExt AsValue(ReadOnlySpan<char> nonJsonfieldName, string value, int startIndex, int length = int.MaxValue, string? formatString = null) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, "0", formatString);
    
    public TExt AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length = int.MaxValue, string? formatString = null) =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, startIndex, length, formatString);
    
    public TExt AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length = int.MaxValue, string defaultValue = "",
        string? formatString = null) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue, formatString);
    
    public TExt AsValue(ReadOnlySpan<char> nonJsonfieldName, char[] value, string? formatString = null) =>
        Stb.FieldValueNext(nonJsonfieldName, value, 0, value.Length, formatString);
    
    public TExt AsValue(ReadOnlySpan<char> nonJsonfieldName, char[] value, int startIndex = 0, int length = int.MaxValue, string? formatString = null) =>
        Stb.FieldValueNext(nonJsonfieldName, value, startIndex, length, formatString);
    
    public TExt AsValue(char[] value, int startIndex = 0, int length = int.MaxValue, string? formatString = null) =>
        Stb.FieldValueNext("", value, startIndex, length, formatString);
    
    public TExt AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex = 0, int length = int.MaxValue, string? formatString = null) =>
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, startIndex, length, formatString);
    
    public TExt AsValueOrNull(char[]? value, int startIndex = 0, int length = int.MaxValue, string? formatString = null) =>
        Stb.FieldValueOrNullNext("", value, startIndex, length, formatString);
    
    public TExt AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex = 0, int length = int.MaxValue, string defaultValue = ""
      , string? formatString = null) =>
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue, formatString);
    
    public TExt AsValueOrDefault(char[]? value, int startIndex = 0, int length = int.MaxValue, string defaultValue = ""
      , string? formatString = null) =>
        Stb.FieldValueOrDefaultNext("", value, startIndex, length, defaultValue, formatString);
    
    public TExt AsValue(ReadOnlySpan<char> nonJsonfieldName, ICharSequence value, string? formatString = null) => 
        Stb.FieldValueNext(nonJsonfieldName, value, 0, int.MaxValue, formatString);
    
    public TExt AsValue(ICharSequence value, string? formatString = null) => 
        Stb.FieldValueNext("", value, 0, int.MaxValue, formatString);
    
    public TExt AsValue(ReadOnlySpan<char> nonJsonfieldName, ICharSequence value, int startIndex = 0, int length = int.MaxValue
      , string? formatString = null) => 
        Stb.FieldValueNext(nonJsonfieldName, value, startIndex, length, formatString);
    
    public TExt AsValue(ICharSequence value, int startIndex = 0, int length = int.MaxValue
      , string? formatString = null) => 
        Stb.FieldValueNext("", value, startIndex, length, formatString);
    
    public TExt AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex = 0, int length = int.MaxValue
      , string? formatString = null) => 
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, startIndex, length, formatString);
    
    public TExt AsValueOrNull(ICharSequence? value, int startIndex = 0, int length = int.MaxValue
      , string? formatString = null) => 
        Stb.FieldValueOrNullNext("", value, startIndex, length, formatString);
    
    public TExt AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex = 0, int length = int.MaxValue
      , string defaultValue = "", string? formatString = null) => 
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue, formatString);
    
    public TExt AsValueOrDefault(ICharSequence? value, int startIndex = 0, int length = int.MaxValue, string defaultValue = ""
      , string? formatString = null) => 
        Stb.FieldValueOrDefaultNext("", value, startIndex, length, defaultValue, formatString);
    
    public TExt AsValue(ReadOnlySpan<char> nonJsonfieldName, StringBuilder value, string? formatString = null) => 
        Stb.FieldValueNext(nonJsonfieldName, value, 0, int.MaxValue, formatString);
    
    public TExt AsValue(StringBuilder value, string? formatString = null) => 
        Stb.FieldValueNext("", value, 0, int.MaxValue, formatString);
    
    public TExt AsValue(ReadOnlySpan<char> nonJsonfieldName, StringBuilder value, int startIndex = 0, int length = int.MaxValue
      , string? formatString = null) => 
        Stb.FieldValueNext(nonJsonfieldName, value, startIndex, length, formatString);
    
    public TExt AsValue(StringBuilder value, int startIndex = 0, int length = int.MaxValue, string? formatString = null) => 
        Stb.FieldValueNext("", value, startIndex, length, formatString);
    
    public TExt AsValueOrNull(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex = 0, int length = int.MaxValue
      , string? formatString = null) => 
        Stb.FieldValueOrNullNext(nonJsonfieldName, value, startIndex, length, formatString);
    
    public TExt AsValueOrNull(StringBuilder? value, int startIndex = 0, int length = int.MaxValue, string? formatString = null) => 
        Stb.FieldValueOrNullNext("", value, startIndex, length, formatString);
    
    public TExt AsValueOrDefault(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex = 0, int length = int.MaxValue
      , string defaultValue = "", string? formatString = null) => 
        Stb.FieldValueOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue, formatString);
    
    public TExt AsValueOrDefault(StringBuilder? value, int startIndex = 0, int length = int.MaxValue, string defaultValue = ""
      , string? formatString = null) => 
        Stb.FieldValueOrDefaultNext("", value, startIndex, length, defaultValue, formatString);

    public TExt AsValueMatch<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value, string? formatString = null) =>
        Stb.ValueMatchOrDefaultNext(nonJsonfieldName, value, "0", formatString);

    public TExt AsValueMatchOrNull<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny? value, string? formatString = null) =>
        Stb.ValueMatchOrNullNext(nonJsonfieldName, value, formatString);

    public TExt AsValueMatchOrDefault<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny? value, string defaultValue = "", string? formatString = null) =>
        Stb.ValueMatchOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString);
    
    public TExt AsString(ReadOnlySpan<char> nonJsonfieldName, bool value) => 
        Stb.FieldStringNext(nonJsonfieldName, value);

    public TExt AsString(bool value) => 
        AsString("", value);
    
    public TExt AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, bool? value) => 
        Stb.FieldStringNext(nonJsonfieldName, value);
    
    public TExt AsStringOrNull(bool? value) => 
        AsStringOrNull("", value);
    
    public TExt AsString<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt value, string? formatString = null) where TFmt : ISpanFormattable => 
        Stb.FieldStringOrNullNext(nonJsonfieldName, value, formatString);

    public TExt AsString<TFmt>(TFmt value, string? formatString = null) where TFmt : ISpanFormattable => 
        AsString("", value, formatString);

    public TExt AsStringOrNull<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, string? formatString = null) where TFmt : class, ISpanFormattable => 
        Stb.FieldStringOrNullNext(nonJsonfieldName, value, formatString);

    public TExt AsStringOrNull<TFmt>(TFmt? value, string? formatString = null) where TFmt : class, ISpanFormattable => 
        AsStringOrNull("", value, formatString);

    public TExt AsStringOrDefault<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, TFmt defaultValue, string? formatString = null) where TFmt : class, ISpanFormattable => 
        Stb.FieldStringOrNullNext(nonJsonfieldName, value ?? defaultValue, formatString);

    public TExt AsStringOrDefault<TFmt>(TFmt? value, TFmt defaultValue, string? formatString = null) where TFmt : class, ISpanFormattable => 
        AsStringOrDefault("", value, defaultValue, formatString);

    public TExt AsStringOrDefault<TFmt>(ReadOnlySpan<char> nonJsonfieldName, TFmt? value, string defaultValue = "", string? formatString = null) where TFmt : class, ISpanFormattable => 
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString);

    public TExt AsStringOrDefault<TFmt>(TFmt? value, string defaultValue = "", string? formatString = null) where TFmt : class, ISpanFormattable => 
        AsStringOrDefault("", value, defaultValue, formatString);

    public TExt AsStringOrNull<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value, string? formatString = null) where TFmtStruct : struct, ISpanFormattable => 
        Stb.FieldStringOrNullNext(nonJsonfieldName, value, formatString);

    public TExt AsStringOrNull<TFmtStruct>(TFmtStruct? value, string? formatString = null) where TFmtStruct : struct, ISpanFormattable => 
        AsStringOrNull("", value, formatString);

    public TExt AsStringOrDefault<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value, TFmtStruct defaultValue, string? formatString = null) where TFmtStruct : struct, ISpanFormattable => 
        Stb.FieldStringOrNullNext(nonJsonfieldName, value ?? defaultValue, formatString);

    public TExt AsStringOrDefault<TFmtStruct>(TFmtStruct? value,  TFmtStruct defaultValue, string? formatString = null) where TFmtStruct : struct, ISpanFormattable => 
        AsStringOrDefault("", value, defaultValue, formatString);

    public TExt AsStringOrDefault<TFmtStruct>(ReadOnlySpan<char> nonJsonfieldName, TFmtStruct? value, string defaultValue = "", string? formatString = null) where TFmtStruct : struct, ISpanFormattable => 
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString);

    public TExt AsStringOrDefault<TFmtStruct>(TFmtStruct? value,  string defaultValue = "", string? formatString = null) where TFmtStruct : struct, ISpanFormattable => 
        AsStringOrDefault("", value, defaultValue, formatString);

    public TExt RevealAsString<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked value, PalantírReveal<TCloakedBase> palantírReveal) 
        where TCloaked : TCloakedBase  =>
        Stb.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, palantírReveal);

    public TExt RevealAsString<TCloaked, TCloakedBase>(TCloaked value, PalantírReveal<TCloakedBase> palantírReveal) where TCloaked : TCloakedBase  =>
        RevealAsString("", value, palantírReveal);

    public TExt RevealAsStringOrDefault<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value, PalantírReveal<TCloakedBase> palantírReveal
      , string defaultValue = "") where TCloaked : TCloakedBase =>
        Stb.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, palantírReveal, defaultValue);

    public TExt RevealAsStringOrDefault<TCloaked, TCloakedBase>(TCloaked? value, PalantírReveal<TCloakedBase> palantírReveal
      , string defaultValue = "") where TCloaked : TCloakedBase =>
        RevealAsStringOrDefault("", value, palantírReveal, defaultValue);

    public TExt RevealAsStringOrNull<TCloaked, TCloakedBase>(ReadOnlySpan<char> nonJsonfieldName, TCloaked? value, PalantírReveal<TCloakedBase> palantírReveal
      , string defaultValue = "") where TCloaked : TCloakedBase =>
        Stb.FieldStringRevealOrNullNext(nonJsonfieldName, value, palantírReveal);

    public TExt RevealAsStringOrNull<TCloaked, TCloakedBase>(TCloaked? value, PalantírReveal<TCloakedBase> palantírReveal) where TCloaked : TCloakedBase =>
        RevealAsStringOrNull("", value, palantírReveal);
    
    public TExt RevealAsString<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer value) where TBearer : IStringBearer =>
        Stb.FieldStringRevealOrDefaultNext(nonJsonfieldName, value);
    
    public TExt RevealAsStringOrNull<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value) where TBearer : IStringBearer =>
        Stb.FieldStringRevealOrNullNext(nonJsonfieldName, value);
    
    public TExt RevealAsStringOrDefault<TBearer>(ReadOnlySpan<char> nonJsonfieldName, TBearer? value, string defaultValue = "") 
        where TBearer : IStringBearer => Stb.FieldStringRevealOrDefaultNext(nonJsonfieldName, value, defaultValue);
    
    public TExt AsString(ReadOnlySpan<char> nonJsonfieldName, Span<char> value, string? formatString = null) => 
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, "", formatString);
    
    public TExt AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, Span<char> value, string defaultValue = "", string? formatString = null) => 
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString);
    
    public TExt AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, Span<char> value, string? formatString = null) => 
        Stb.FieldStringNext(nonJsonfieldName, value, formatString);
    
    public TExt AsStringOrNull(Span<char> value) => Stb.FieldStringNext("", value);
    
    public TExt AsString(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value, string? formatString = null) => 
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, "", formatString);
    
    public TExt AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value, string defaultValue = "", string? formatString = null) => 
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString);
    
    public TExt AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, ReadOnlySpan<char> value, string? formatString = null) => 
        Stb.FieldStringOrNullNext(nonJsonfieldName, value, formatString);
    
    public TExt AsStringOrNull(ReadOnlySpan<char> value) => 
        Stb.FieldStringNext("", value);
    
    public TExt AsString(Span<char> value) => 
        Stb.FieldStringOrDefaultNext("", value);
    
    public TExt AsString(ReadOnlySpan<char> value) => 
        Stb.FieldStringOrDefaultNext("", value);
    
    public TExt AsString(string value) => 
        Stb.FieldStringOrDefaultNext("", value);
    
    public TExt AsString(ReadOnlySpan<char> nonJsonfieldName, string value, string? formatString = null) => 
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, "", formatString);
    
    public TExt AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, string? value, string? formatString = null) => 
        Stb.FieldStringOrNullNext(nonJsonfieldName, value, formatString);
    
    public TExt AsString(ReadOnlySpan<char> nonJsonfieldName, string value, int startIndex, int length = int.MaxValue, string? formatString = null) =>
        Stb.FieldStringOrNullNext(nonJsonfieldName, value, startIndex, length, formatString);
    
    public TExt AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length = int.MaxValue, string? formatString = null) =>
        Stb.FieldStringOrNullNext(nonJsonfieldName, value, startIndex, length, formatString);
    
    public TExt AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, string? value, int startIndex, int length = int.MaxValue
      , string defaultValue = "", string? formatString = null) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue, formatString);
    
    public TExt AsString(ReadOnlySpan<char> nonJsonfieldName, char[] value,  string? formatString = null) =>
        Stb.FieldStringNext(nonJsonfieldName, value, 0, int.MaxValue, formatString);
    
    public TExt AsString(ReadOnlySpan<char> nonJsonfieldName, char[] value, int startIndex = 0, int length = int.MaxValue, string? formatString = null) =>
        Stb.FieldStringNext(nonJsonfieldName, value, startIndex, length, formatString);
    
    public TExt AsString(char[] value, int startIndex = 0, int length = int.MaxValue, string? formatString = null) =>
        Stb.FieldStringNext("", value, startIndex, length, formatString);
    
    public TExt AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex = 0, int length = int.MaxValue, string? formatString = null) =>
        Stb.FieldStringOrNullNext(nonJsonfieldName, value, startIndex, length, formatString);
    
    public TExt AsStringOrNull(char[]? value, int startIndex = 0, int length = int.MaxValue, string? formatString = null) =>
        Stb.FieldStringOrNullNext("", value, startIndex, length, formatString);
    
    public TExt AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, char[]? value, int startIndex = 0, int length = int.MaxValue
      , string defaultValue = "", string? formatString = null) =>
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue, formatString);
    
    public TExt AsStringOrDefault(char[]? value, int startIndex = 0, int length = int.MaxValue, string defaultValue = "", string? formatString = null) =>
        Stb.FieldStringOrDefaultNext("", value, startIndex, length, defaultValue, formatString);
    
    public TExt AsString(ReadOnlySpan<char> nonJsonfieldName, ICharSequence value, string? formatString = null) => 
        Stb.FieldStringNext(nonJsonfieldName, value, 0, int.MaxValue, formatString);
    
    public TExt AsString(ICharSequence value, string? formatString = null) => 
        Stb.FieldStringNext("", value, 0, int.MaxValue, formatString);
    
    public TExt AsString(ReadOnlySpan<char> nonJsonfieldName, ICharSequence value, int startIndex = 0, int length = int.MaxValue, string? formatString = null) => 
        Stb.FieldStringNext(nonJsonfieldName, value, startIndex, length, formatString);
    
    public TExt AsString(ICharSequence value, int startIndex = 0, int length = int.MaxValue, string? formatString = null) => 
        Stb.FieldStringNext("", value, startIndex, length, formatString);
    
    public TExt AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex = 0, int length = int.MaxValue, string? formatString = null) => 
        Stb.FieldStringOrNullNext(nonJsonfieldName, value, startIndex, length, formatString);
    
    public TExt AsStringOrNull(ICharSequence? value, int startIndex = 0, int length = int.MaxValue, string? formatString = null) => 
        Stb.FieldStringOrNullNext("", value, startIndex, length, formatString);
    
    public TExt AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, ICharSequence? value, int startIndex = 0, int length = int.MaxValue, string defaultValue = "", string? formatString = null) => 
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue, formatString);
    
    public TExt AsStringOrDefault(ICharSequence? value, int startIndex = 0, int length = int.MaxValue, string fallbackValue = "", string? formatString = null) => 
        Stb.FieldStringOrDefaultNext("", value, startIndex, length, fallbackValue, formatString);
    
    public TExt AsString(ReadOnlySpan<char> nonJsonfieldName, StringBuilder value, string? formatString = null) => 
        Stb.FieldStringNext(nonJsonfieldName, value, 0, int.MaxValue, formatString);
    
    public TExt AsString(StringBuilder value, string? formatString = null) => 
        Stb.FieldStringNext("", value, 0, int.MaxValue, formatString);
    
    public TExt AsString(ReadOnlySpan<char> nonJsonfieldName, StringBuilder value, int startIndex = 0, int length = int.MaxValue, string? formatString = null) => 
        Stb.FieldStringNext(nonJsonfieldName, value, startIndex, length, formatString);
    
    public TExt AsString(StringBuilder value, int startIndex = 0, int length = int.MaxValue, string? formatString = null) => 
        Stb.FieldStringNext("", value, startIndex, length, formatString);
    
    public TExt AsStringOrNull(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex = 0, int length = int.MaxValue, string? formatString = null) => 
        Stb.FieldStringOrNullNext(nonJsonfieldName, value, startIndex, length, formatString);
    
    public TExt AsStringOrNull(StringBuilder? value, int startIndex = 0, int length = int.MaxValue, string? formatString = null) => 
        Stb.FieldStringOrNullNext("", value, startIndex, length, formatString);
    
    public TExt AsStringOrDefault(ReadOnlySpan<char> nonJsonfieldName, StringBuilder? value, int startIndex = 0, int length = int.MaxValue
      , string defaultValue = "", string? formatString = null) => 
        Stb.FieldStringOrDefaultNext(nonJsonfieldName, value, startIndex, length, defaultValue, formatString);
    
    public TExt AsStringOrDefault(StringBuilder? value, int startIndex = 0, int length = int.MaxValue, string fallbackValue = "", string? formatString = null) => 
        Stb.FieldStringOrDefaultNext("", value, startIndex, length, fallbackValue, formatString);

    public TExt AsStringMatch<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny value, string? formatString = null) =>
        Stb.ValueMatchOrDefaultNext(nonJsonfieldName, value, "", formatString);

    public TExt AsStringMatchOrNull<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny? value, string? formatString = null) =>
        Stb.ValueMatchOrNullNext(nonJsonfieldName, value, formatString);

    public TExt AsStringMatchOrDefault<TAny>(ReadOnlySpan<char> nonJsonfieldName, TAny? value, string defaultValue = "", string? formatString = null) =>
        Stb.ValueMatchOrDefaultNext(nonJsonfieldName, value, defaultValue, formatString);
    
    public IScopeDelimitedStringBuilder StartDelimitedStringBuilder()  => Stb.StartDelimitedStringBuilder();
}