using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexFields.SingleFields;

public readonly struct BoolWhenNonDefaultStringBearer(bool value, bool defaultValue = false) : IStringBearer
{
    public bool WhenNonDefaultBool { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(WhenNonDefaultBool), WhenNonDefaultBool, defaultValue)
           .Complete();
}

public readonly struct SpanFormattableWhenNonDefaultStringBearer<TFmt>(TFmt value, TFmt defaultValue, string? formatString = null) : IStringBearer
where TFmt : ISpanFormattable
{
    public TFmt WhenNonDefaultSpanFormattable { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(WhenNonDefaultSpanFormattable), WhenNonDefaultSpanFormattable, defaultValue, formatString)
           .Complete();
}

public readonly struct SpanFormattableWithHandlingWhenNonDefaultStringBearer<TFmt>(TFmt value, TFmt defaultValue
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) : IStringBearer
where TFmt : ISpanFormattable
{
    public TFmt WhenNonDefaultSpanFormattable { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddAs(nameof(WhenNonDefaultSpanFormattable), WhenNonDefaultSpanFormattable, defaultValue, flags, formatString)
           .Complete();
}

public readonly struct NullableSpanFormattableWhenNonDefaultStringBearer<TFmtStruct>(TFmtStruct? value, TFmtStruct defaultValue, string? formatString = null) : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?  WhenNonDefaultNullableSpanFormattable { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(WhenNonDefaultNullableSpanFormattable)
                                  , WhenNonDefaultNullableSpanFormattable, defaultValue, formatString)
           .Complete();
}

public readonly struct NullableSpanFormattableWithHandlingWhenNonDefaultStringBearer<TFmtStruct>(TFmtStruct? value, TFmtStruct defaultValue
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?  WhenNonDefaultNullableSpanFormattable { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddAs(nameof(WhenNonDefaultNullableSpanFormattable)
                                  , WhenNonDefaultNullableSpanFormattable, defaultValue, flags, formatString)
           .Complete();
}

public readonly struct CustomBearerWhenNonDefaultStringBearer<TTCloaked, TCloakedBase>(TTCloaked value, PalantírReveal<TCloakedBase> palantírReveal
  , TTCloaked defaultValue) : IStringBearer where TTCloaked : TCloakedBase
{
    public TTCloaked WhenNonDefaultCustomBearer { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
           .Field.WhenNonDefaultReveal(nameof(WhenNonDefaultCustomBearer), WhenNonDefaultCustomBearer, palantírReveal, defaultValue)
           .Complete();
}

public readonly struct CustomBearerWithHandlingWhenNonDefaultStringBearer<TCloaked, TCloakedBase>(TCloaked value, PalantírReveal<TCloakedBase> palantírReveal
  , TCloaked defaultValue, FieldContentHandling flags = FieldContentHandling.DefaultForValueType) : IStringBearer where TCloaked : TCloakedBase
{
    public TCloaked WhenNonDefaultCustomBearer { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) => tos.StartComplexType(this)
           .Field.WhenNonDefaultRevealAs(nameof(WhenNonDefaultCustomBearer), WhenNonDefaultCustomBearer, palantírReveal, defaultValue, flags)
           .Complete();
}

public readonly struct NullableCustomBearerWhenNonDefaultStringBearer<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
  , TCloakedStruct defaultValue) : IStringBearer where TCloakedStruct : struct
{
    public TCloakedStruct? WhenNonDefaultCustomBearer { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
           .Field.WhenNonDefaultReveal(nameof(WhenNonDefaultCustomBearer), WhenNonDefaultCustomBearer, palantírReveal, defaultValue)
           .Complete();
}

public readonly struct NullableCustomBearerWithHandlingWhenNonDefaultStringBearer<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
  , TCloakedStruct defaultValue, FieldContentHandling flags = FieldContentHandling.DefaultForValueType) : IStringBearer where TCloakedStruct : struct
{
    public TCloakedStruct? WhenNonDefaultCustomBearer { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
           .Field.WhenNonDefaultRevealAs(nameof(WhenNonDefaultCustomBearer), WhenNonDefaultCustomBearer, palantírReveal, defaultValue, flags)
           .Complete();
}

public readonly struct StringBearerWhenNonDefaultStringBearer<TBearer>(TBearer? value, TBearer? defaultValue) : IStringBearer
    where TBearer : IStringBearer
{
    public TBearer? WhenNonDefaultStringBearer { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
           .Field.WhenNonDefaultReveal(nameof(WhenNonDefaultStringBearer), WhenNonDefaultStringBearer, defaultValue)
           .Complete();
}

public readonly struct StringBearerWithHandlingWhenNonDefaultStringBearer<TBearer>(TBearer? value, TBearer? defaultValue
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType) : IStringBearer
    where TBearer : IStringBearer
{
    public TBearer? WhenNonDefaultStringBearer { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
           .Field.WhenNonDefaultRevealAs(nameof(WhenNonDefaultStringBearer), WhenNonDefaultStringBearer, defaultValue, flags)
           .Complete();
}

public readonly struct NullableStringBearerWhenNonDefaultStringBearer<TBearerStruct>(TBearerStruct? value, TBearerStruct? defaultValue) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? WhenNonDefaultStringBearer { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
           .Field.WhenNonDefaultReveal(nameof(WhenNonDefaultStringBearer), WhenNonDefaultStringBearer, defaultValue)
           .Complete();
}

public readonly struct NullableStringBearerWithHandlingWhenNonDefaultStringBearer<TBearerStruct>(TBearerStruct? value, TBearerStruct? defaultValue
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType) : IStringBearer where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? WhenNonDefaultStringBearer { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
           .Field.WhenNonDefaultRevealAs(nameof(WhenNonDefaultStringBearer), WhenNonDefaultStringBearer, defaultValue, flags)
           .Complete();
}

public readonly struct CharSpanWhenNonDefaultAddStringBearer(string value, string defaultValue, string? formatString = null) : IStringBearer
{
    public string WhenNonDefaultString { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(WhenNonDefaultString), WhenNonDefaultString.AsSpan(), defaultValue, formatString)
           .Complete();
}

public readonly struct CharSpanWithHandlingWhenNonDefaultAddStringBearer(string value, string defaultValue
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) : IStringBearer
{
    public string WhenNonDefaultString { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddAs(nameof(WhenNonDefaultString), WhenNonDefaultString.AsSpan(), defaultValue, flags, formatString)
           .Complete();
}

public readonly struct StringWhenNonDefaultAddStringBearer(string value, string defaultValue, string? formatString = null) : IStringBearer
{
    public string WhenNonDefaultString { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(WhenNonDefaultString), WhenNonDefaultString, defaultValue, formatString)
           .Complete();
}

public readonly struct StringWithHandlingWhenNonDefaultAddStringBearer(string value, string defaultValue
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) : IStringBearer
{
    public string WhenNonDefaultString { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddAs(nameof(WhenNonDefaultString), WhenNonDefaultString, defaultValue, flags, formatString)
           .Complete();
}

public readonly struct StringRangeWhenNonDefaultAddStringBearer(string value, int fromIndex, int length, string defaultValue = "", string? formatString = null) : IStringBearer
{
    public string WhenNonDefaultStringRange { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(WhenNonDefaultStringRange), WhenNonDefaultStringRange
                                  , fromIndex, length, defaultValue, formatString)
           .Complete();
}

public readonly struct StringRangeWithHandlingWhenNonDefaultAddStringBearer(string value, int fromIndex, int length, string defaultValue = ""
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) : IStringBearer
{
    public string WhenNonDefaultStringRange { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddAs(nameof(WhenNonDefaultStringRange), WhenNonDefaultStringRange
                                  , fromIndex, length, defaultValue, flags, formatString)
           .Complete();
}

public readonly struct CharArrayWhenNonDefaultStringBearer(char[] value, string defaultValue = "", string? formatString = null) : IStringBearer
{
    public char[] WhenNonDefaultCharArray { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(WhenNonDefaultCharArray), WhenNonDefaultCharArray, defaultValue, formatString)
           .Complete();
}

public readonly struct CharArrayWithHandlingWhenNonDefaultStringBearer(char[] value, string defaultValue = ""
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) : IStringBearer
{
    public char[] WhenNonDefaultCharArray { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddAs(nameof(WhenNonDefaultCharArray), WhenNonDefaultCharArray, defaultValue, flags, formatString)
           .Complete();
}

public readonly struct CharArrayRangeWhenNonDefaultStringBearer(char[] value, int fromIndex, int length, string defaultValue = "", string? formatString = null) : IStringBearer
{
    public char[] WhenNonDefaultCharArrayRange { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(WhenNonDefaultCharArrayRange), WhenNonDefaultCharArrayRange
                                  , fromIndex, length, defaultValue, formatString)
           .Complete();
}

public readonly struct CharArrayRangeWithHandlingWhenNonDefaultStringBearer(char[] value, int fromIndex, int length, string defaultValue = ""
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) : IStringBearer
{
    public char[] WhenNonDefaultCharArrayRange { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddAs(nameof(WhenNonDefaultCharArrayRange), WhenNonDefaultCharArrayRange
                                  , fromIndex, length, defaultValue, flags, formatString)
           .Complete();
}

public readonly struct CharSequenceWhenNonDefaultStringBearer<TCharSeq>(TCharSeq value, string defaultValue = "", string? formatString = null) : IStringBearer
    where TCharSeq : ICharSequence
{
    public TCharSeq WhenNonDefaultCharSequence { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddCharSeq(nameof(WhenNonDefaultCharSequence), WhenNonDefaultCharSequence, defaultValue, formatString)
           .Complete();
}

public readonly struct CharSequenceWithHandlingWhenNonDefaultStringBearer<TCharSeq>(TCharSeq value, string defaultValue = ""
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) : IStringBearer
    where TCharSeq : ICharSequence
{
    public TCharSeq WhenNonDefaultCharSequence { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddCharSeqAs(nameof(WhenNonDefaultCharSequence), WhenNonDefaultCharSequence, defaultValue, flags, formatString)
           .Complete();
}

public readonly struct CharSequenceRangeWhenNonDefaultStringBearer<TCharSeq>(TCharSeq value, int fromIndex, int length, string defaultValue = "", string? formatString = null) : IStringBearer
    where TCharSeq : ICharSequence
{
    public TCharSeq WhenNonDefaultCharSequenceRange { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddCharSeq(nameof(WhenNonDefaultCharSequenceRange), WhenNonDefaultCharSequenceRange
                                  , fromIndex, length, defaultValue, formatString)
           .Complete();
}

public readonly struct CharSequenceRangeWithHandlingWhenNonDefaultStringBearer<TCharSeq>(TCharSeq value, int fromIndex, int length, string defaultValue = ""
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) : IStringBearer
    where TCharSeq : ICharSequence
{
    public TCharSeq WhenNonDefaultCharSequenceRange { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddCharSeqAs(nameof(WhenNonDefaultCharSequenceRange), WhenNonDefaultCharSequenceRange
                                  , fromIndex, length, defaultValue, flags, formatString)
           .Complete();
}

public class StringBuilderWhenNonDefaultStringBearer(StringBuilder value, string defaultValue = "", string? formatString = null) : IStringBearer
{
    public StringBuilder WhenNonDefaultAddStringBuilder { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(WhenNonDefaultAddStringBuilder), WhenNonDefaultAddStringBuilder, defaultValue, formatString)
           .Complete();
}

public class StringBuilderWithHandlingWhenNonDefaultStringBearer(StringBuilder value, string defaultValue = ""
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) : IStringBearer
{
    public StringBuilder WhenNonDefaultAddStringBuilder { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddAs(nameof(WhenNonDefaultAddStringBuilder), WhenNonDefaultAddStringBuilder, defaultValue, flags, formatString)
           .Complete();
}

public class StringBuilderRangeWhenNonDefaultStringBearer(StringBuilder value, int fromIndex, int length, string defaultValue = "", string? formatString = null) : IStringBearer
{
    public StringBuilder WhenNonDefaultAddStringBuilderRange { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(WhenNonDefaultAddStringBuilderRange), WhenNonDefaultAddStringBuilderRange, fromIndex, length, defaultValue, formatString)
           .Complete();
}

public class StringBuilderRangeWithHandlingWhenNonDefaultStringBearer(StringBuilder value, int fromIndex, int length, string defaultValue = ""
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) : IStringBearer
{
    public StringBuilder WhenNonDefaultAddStringBuilderRange { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddAs(nameof(WhenNonDefaultAddStringBuilderRange), WhenNonDefaultAddStringBuilderRange, fromIndex, length, defaultValue, flags, formatString)
           .Complete();
}

public readonly struct ObjectWhenNonDefaultStringBearer(object? value, object? defaultValue, string? formatString = null) : IStringBearer
{
    public object? WhenNonDefaultObject { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddObject(nameof(WhenNonDefaultObject), WhenNonDefaultObject, defaultValue, formatString)
           .Complete();
}

public readonly struct MatchWhenNonDefaultStringBearer<T>(T? value, T? defaultValue = default(T), string? formatString = null) : IStringBearer
{
    public T? WhenNonDefaultMatch { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) => 
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddMatch(nameof(WhenNonDefaultMatch), WhenNonDefaultMatch, defaultValue, formatString)
           .Complete();
}
