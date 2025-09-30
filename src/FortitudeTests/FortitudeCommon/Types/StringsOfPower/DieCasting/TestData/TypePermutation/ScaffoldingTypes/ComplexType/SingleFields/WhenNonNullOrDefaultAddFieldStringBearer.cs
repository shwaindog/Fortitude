using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexType.SingleFields;

public readonly struct BoolWhenNonNullOrDefaultStringBearer(bool? value, bool defaultValue = false) : IStringBearer
{
    public bool? WhenNonNullOrDefaultBool { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd(nameof(WhenNonNullOrDefaultBool), WhenNonNullOrDefaultBool, defaultValue)
           .Complete();
}

public readonly struct SpanFormattableWhenNonNullOrDefaultStringBearer<TFmt>
    (TFmt? value, TFmt? defaultValue = default(TFmt), string? formatString = null) : IStringBearer
    where TFmt : ISpanFormattable
{
    public TFmt? WhenNonNullOrDefaultSpanFormattable { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd(nameof(WhenNonNullOrDefaultSpanFormattable), WhenNonNullOrDefaultSpanFormattable, defaultValue, formatString)
           .Complete();
}

public readonly struct SpanFormattableWithHandlingWhenNonNullOrDefaultStringBearer<TFmt>
( TFmt? value , TFmt? defaultValue = default(TFmt) , FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) 
    : IStringBearer where TFmt : ISpanFormattable
{
    public TFmt? WhenNonNullOrDefaultSpanFormattable { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddAs(nameof(WhenNonNullOrDefaultSpanFormattable), WhenNonNullOrDefaultSpanFormattable, defaultValue, flags, formatString)
           .Complete();
}

public readonly struct NullableSpanFormattableWhenNonNullOrDefaultStringBearer<TFmtStruct>
    (TFmtStruct? value, TFmtStruct defaultValue = default(TFmtStruct), string? formatString = null) : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? WhenNonNullOrDefaultNullableSpanFormattable { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd(nameof(WhenNonNullOrDefaultNullableSpanFormattable)
                                        , WhenNonNullOrDefaultNullableSpanFormattable, defaultValue, formatString)
           .Complete();
}

public readonly struct NullableSpanFormattableWithHandlingWhenNonNullOrDefaultStringBearer<TFmtStruct>
( TFmtStruct? value , TFmtStruct defaultValue = default(TFmtStruct), FieldContentHandling flags = FieldContentHandling.DefaultForValueType
  , string? formatString = null) : IStringBearer where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? WhenNonNullOrDefaultNullableSpanFormattable { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddAs(nameof(WhenNonNullOrDefaultNullableSpanFormattable), WhenNonNullOrDefaultNullableSpanFormattable, defaultValue, flags
                                          , formatString)
           .Complete();
}

public readonly struct CustomBearerWhenNonNullOrDefaultStringBearer<TCloaked, TCloakedBase>
    (TCloaked? value, PalantírReveal<TCloakedBase> palantírReveal, TCloaked? defaultValue = default(TCloaked)) : IStringBearer
    where TCloaked : TCloakedBase
{
    public TCloaked? WhenNonNullOrDefaultCustomBearer { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultReveal(nameof(WhenNonNullOrDefaultCustomBearer), WhenNonNullOrDefaultCustomBearer, palantírReveal, defaultValue)
           .Complete();
}

public readonly struct CustomBearerWithHandlingWhenNonNullOrDefaultStringBearer<TTCloaked, TCloakedBase>
    (TTCloaked? value, PalantírReveal<TCloakedBase> palantírReveal, TTCloaked? defaultValue = default(TTCloaked)
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType) : IStringBearer
    where TTCloaked : TCloakedBase
{
    public TTCloaked? WhenNonNullOrDefaultCustomBearer { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultRevealAs(nameof(WhenNonNullOrDefaultCustomBearer), WhenNonNullOrDefaultCustomBearer, palantírReveal, defaultValue, flags)
           .Complete();
}

public readonly struct NullableCustomBearerWhenNonNullOrDefaultStringBearer<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal, TCloakedStruct? defaultValue = null) 
    : IStringBearer where TCloakedStruct : struct
{
    public TCloakedStruct? WhenNonNullOrDefaultCustomBearer { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultReveal(nameof(WhenNonNullOrDefaultCustomBearer), WhenNonNullOrDefaultCustomBearer, palantírReveal, defaultValue)
           .Complete();
}

public readonly struct NullableCustomBearerWithHandlingWhenNonNullOrDefaultStringBearer<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
  , TCloakedStruct? defaultValue = null, FieldContentHandling flags = FieldContentHandling.DefaultForValueType) : IStringBearer where TCloakedStruct : struct
{
    public TCloakedStruct? WhenNonNullOrDefaultCustomBearer { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultRevealAs(nameof(WhenNonNullOrDefaultCustomBearer), WhenNonNullOrDefaultCustomBearer, palantírReveal, defaultValue, flags)
           .Complete();
}

public readonly struct StringBearerWhenNonNullOrDefaultStringBearer<TBearer>(TBearer? value, TBearer? defaultValue = default(TBearer)) : IStringBearer
    where TBearer : IStringBearer
{
    public TBearer? WhenNonNullOrDefaultStringBearer { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultReveal(nameof(WhenNonNullOrDefaultStringBearer), WhenNonNullOrDefaultStringBearer, defaultValue)
           .Complete();
}

public readonly struct StringBearerWithHandlingWhenNonNullOrDefaultStringBearer<TBearer>(TBearer? value, TBearer? defaultValue = default(TBearer)
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType) : IStringBearer
    where TBearer : IStringBearer
{
    public TBearer? WhenNonNullOrDefaultStringBearer { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultRevealAs(nameof(WhenNonNullOrDefaultStringBearer), WhenNonNullOrDefaultStringBearer, defaultValue, flags)
           .Complete();
}

public readonly struct NullableStringBearerWhenNonNullOrDefaultStringBearer<TBearerStruct>(TBearerStruct? value, TBearerStruct? defaultValue = null) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? WhenNonNullOrDefaultStringBearer { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultReveal(nameof(WhenNonNullOrDefaultStringBearer), WhenNonNullOrDefaultStringBearer, defaultValue)
           .Complete();
}

public readonly struct NullableStringBearerWithHandlingWhenNonNullOrDefaultStringBearer<TBearerStruct>(TBearerStruct? value, TBearerStruct? defaultValue = null
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? WhenNonNullOrDefaultStringBearer { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultRevealAs(nameof(WhenNonNullOrDefaultStringBearer), WhenNonNullOrDefaultStringBearer, defaultValue, flags)
           .Complete();
}

public readonly struct StringWhenNonNullOrDefaultAddStringBearer(string? value, string defaultValue = "", string? formatString = null) : IStringBearer
{
    public string? WhenNonNullOrDefaultString { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd(nameof(WhenNonNullOrDefaultString), WhenNonNullOrDefaultString, defaultValue, formatString)
           .Complete();
}

public readonly struct StringWithHandlingWhenNonNullOrDefaultAddStringBearer(string? value, string defaultValue = ""
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) : IStringBearer
{
    public string? WhenNonNullOrDefaultString { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddAs(nameof(WhenNonNullOrDefaultString), WhenNonNullOrDefaultString, defaultValue, flags, formatString)
           .Complete();
}

public readonly struct StringRangeWhenNonNullOrDefaultAddStringBearer
    (string? value, int fromIndex, int length, string defaultValue = "", string? formatString = null) : IStringBearer
{
    public string? WhenNonNullOrDefaultStringRange { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd(nameof(WhenNonNullOrDefaultStringRange), WhenNonNullOrDefaultStringRange
                                        , fromIndex, length, defaultValue, formatString)
           .Complete();
}

public readonly struct StringRangeWithHandlingWhenNonNullOrDefaultAddStringBearer
    (string? value, int fromIndex, int length, string defaultValue = ""
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) : IStringBearer
{
    public string? WhenNonNullOrDefaultStringRange { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddAs(nameof(WhenNonNullOrDefaultStringRange), WhenNonNullOrDefaultStringRange
                                        , fromIndex, length, defaultValue, flags, formatString)
           .Complete();
}

public readonly struct CharArrayWhenNonNullOrDefaultStringBearer(char[] value, string defaultValue = "", string? formatString = null) : IStringBearer
{
    public char[] WhenNonNullOrDefaultCharArray { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd(nameof(WhenNonNullOrDefaultCharArray), WhenNonNullOrDefaultCharArray, defaultValue, formatString)
           .Complete();
}

public readonly struct CharArrayWithHandlingWhenNonNullOrDefaultStringBearer(char[] value, string defaultValue = ""
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) : IStringBearer
{
    public char[] WhenNonNullOrDefaultCharArray { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddAs(nameof(WhenNonNullOrDefaultCharArray), WhenNonNullOrDefaultCharArray, defaultValue, flags, formatString)
           .Complete();
}

public readonly struct CharArrayRangeWhenNonNullOrDefaultStringBearer
    (char[] value, int fromIndex, int length, string defaultValue = "", string? formatString = null) : IStringBearer
{
    public char[] WhenNonNullOrDefaultCharArrayRange { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd(nameof(WhenNonNullOrDefaultCharArrayRange), WhenNonNullOrDefaultCharArrayRange
                                        , fromIndex, length, defaultValue, formatString)
           .Complete();
}

public readonly struct CharArrayRangeWithHandlingWhenNonNullOrDefaultStringBearer
    (char[] value, int fromIndex, int length, string defaultValue = "", FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , string? formatString = null) : IStringBearer
{
    public char[] WhenNonNullOrDefaultCharArrayRange { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddAs(nameof(WhenNonNullOrDefaultCharArrayRange), WhenNonNullOrDefaultCharArrayRange
                                        , fromIndex, length, defaultValue, flags, formatString)
           .Complete();
}

public readonly struct CharSequenceWhenNonNullOrDefaultStringBearer<TCharSeq>(TCharSeq value, string defaultValue = "", string? formatString = null) : IStringBearer
    where TCharSeq : ICharSequence
{
    public TCharSeq WhenNonNullOrDefaultCharSequence { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddCharSeq(nameof(WhenNonNullOrDefaultCharSequence), WhenNonNullOrDefaultCharSequence, defaultValue, formatString)
           .Complete();
}

public readonly struct CharSequenceWithHandlingWhenNonNullOrDefaultStringBearer<TCharSeq>(TCharSeq value, string defaultValue = ""
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) : IStringBearer
    where TCharSeq : ICharSequence
{
    public TCharSeq WhenNonNullOrDefaultCharSequence { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddCharSeqAs(nameof(WhenNonNullOrDefaultCharSequence), WhenNonNullOrDefaultCharSequence, defaultValue, flags, formatString)
           .Complete();
}

public readonly struct CharSequenceRangeWhenNonNullOrDefaultStringBearer<TCharSeq>
    (TCharSeq value, int fromIndex, int length, string defaultValue = "", string? formatString = null) : IStringBearer
    where TCharSeq : ICharSequence
{
    public TCharSeq WhenNonNullOrDefaultCharSequenceRange { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddCharSeq(nameof(WhenNonNullOrDefaultCharSequenceRange), WhenNonNullOrDefaultCharSequenceRange
                                               , fromIndex, length, defaultValue, formatString)
           .Complete();
}

public readonly struct CharSequenceRangeWithHandlingWhenNonNullOrDefaultStringBearer<TCharSeq>
    (TCharSeq value, int fromIndex, int length, string defaultValue = "", FieldContentHandling flags = FieldContentHandling.DefaultForValueType
      , string? formatString = null) : IStringBearer where TCharSeq : ICharSequence
{
    public TCharSeq WhenNonNullOrDefaultCharSequenceRange { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddCharSeqAs(nameof(WhenNonNullOrDefaultCharSequenceRange), WhenNonNullOrDefaultCharSequenceRange
                                               , fromIndex, length, defaultValue, flags, formatString)
           .Complete();
}

public class StringBuilderWhenNonNullOrDefaultStringBearer(StringBuilder value, string defaultValue = "", string? formatString = null) : IStringBearer
{
    public StringBuilder WhenNonNullOrDefaultAddStringBuilder { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd(nameof(WhenNonNullOrDefaultAddStringBuilder), WhenNonNullOrDefaultAddStringBuilder, defaultValue, formatString)
           .Complete();
}

public class StringBuilderWithHandlingWhenNonNullOrDefaultStringBearer(StringBuilder value, string defaultValue = ""
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) : IStringBearer
{
    public StringBuilder WhenNonNullOrDefaultAddStringBuilder { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddAs(nameof(WhenNonNullOrDefaultAddStringBuilder), WhenNonNullOrDefaultAddStringBuilder, defaultValue, flags, formatString)
           .Complete();
}

public class StringBuilderRangeWhenNonNullOrDefaultStringBearer
    (StringBuilder value, int fromIndex, int length, string defaultValue = "", string? formatString = null) : IStringBearer
{
    public StringBuilder WhenNonNullOrDefaultAddStringBuilderRange { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd(nameof(WhenNonNullOrDefaultAddStringBuilderRange), WhenNonNullOrDefaultAddStringBuilderRange, fromIndex, length, defaultValue, formatString)
           .Complete();
}

public class StringBuilderRangeWithHandlingWhenNonNullOrDefaultStringBearer(StringBuilder value, int fromIndex, int length, string defaultValue = ""
      , FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) : IStringBearer
{
    public StringBuilder WhenNonNullOrDefaultAddStringBuilderRange { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddAs(nameof(WhenNonNullOrDefaultAddStringBuilderRange), WhenNonNullOrDefaultAddStringBuilderRange, fromIndex, length, defaultValue, flags, formatString)
           .Complete();
}

public readonly struct ObjectWhenNonNullOrDefaultStringBearer(object? value, object? defaultValue, string? formatString = null) : IStringBearer
{
    public object? WhenNonNullOrDefaultObject { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddObject(nameof(WhenNonNullOrDefaultObject), WhenNonNullOrDefaultObject, defaultValue, formatString)
           .Complete();
}

public readonly struct MatchWhenNonNullOrDefaultStringBearer<T>(T? value, T? defaultValue = default(T), string? formatString = null) : IStringBearer
{
    public T? WhenNonNullOrDefaultMatch { get; } = value;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddMatch(nameof(WhenNonNullOrDefaultMatch), WhenNonNullOrDefaultMatch, defaultValue, formatString)
           .Complete();
}
