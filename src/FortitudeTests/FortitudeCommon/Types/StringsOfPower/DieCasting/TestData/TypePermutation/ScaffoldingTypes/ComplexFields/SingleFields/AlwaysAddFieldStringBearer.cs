using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexFields.SingleFields;

public class BoolAlwaysAddStringBearer(bool value) : IStringBearer
{
    public bool AlwaysAddBool { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(AlwaysAddBool), AlwaysAddBool)
           .Complete();
}

public class NullableBoolAlwaysAddStringBearer(bool? value) : IStringBearer
{
    public bool? AlwaysAddNullableBool { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(AlwaysAddNullableBool), AlwaysAddNullableBool)
           .Complete();
}

public class SpanFormattableAlwaysAddStringBearer<TFmt>(TFmt value, string? formatString = null) : IStringBearer where TFmt : ISpanFormattable
{
    public TFmt AlwaysAddSpanFormattable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(AlwaysAddSpanFormattable), AlwaysAddSpanFormattable, formatString)
           .Complete();
}

public class SpanFormattableWithHandlingAlwaysAddStringBearer<TFmt>(TFmt value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType
  , string? formatString = null) : IStringBearer where TFmt : ISpanFormattable
{
    public TFmt AlwaysAddSpanFormattable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddAs(nameof(AlwaysAddSpanFormattable), AlwaysAddSpanFormattable, flags, formatString)
           .Complete();
}

public class NullableSpanFormattableAlwaysAddStringBearer<TFmtStruct>(TFmtStruct? value, string? formatString = null) : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? AlwaysAddNullableSpanFormattable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(AlwaysAddNullableSpanFormattable), AlwaysAddNullableSpanFormattable, formatString)
           .Complete();
}

public class NullableSpanFormattableWithHandlingAlwaysAddStringBearer<TFmtStruct>(TFmtStruct? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType
  , string? formatString = null) : IStringBearer where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? AlwaysAddNullableSpanFormattable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddAs(nameof(AlwaysAddNullableSpanFormattable), AlwaysAddNullableSpanFormattable, flags, formatString)
           .Complete();
}

public class CustomBearerAlwaysAddStringBearer<TCloaked, TCloakedBase>(TCloaked? value, PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer
    where TCloaked : TCloakedBase
{
    public TCloaked? AlwaysAddCustomBearer { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal(nameof(AlwaysAddCustomBearer), AlwaysAddCustomBearer, palantírReveal)
           .Complete();
}

public class CustomBearerWithHandlingAlwaysAddStringBearer<TTCloaked, TCloakedBase>(TTCloaked? value, PalantírReveal<TCloakedBase> palantírReveal
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType) : IStringBearer where TTCloaked : TCloakedBase
{
    public TTCloaked? AlwaysAddCustomBearer { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysRevealAs(nameof(AlwaysAddCustomBearer), AlwaysAddCustomBearer, palantírReveal, flags)
           .Complete();
}

public class NullableCustomBearerAlwaysAddStringBearer<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer where TCloakedStruct : struct
{
    public TCloakedStruct? AlwaysAddCustomBearer { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal(nameof(AlwaysAddCustomBearer), AlwaysAddCustomBearer, palantírReveal)
           .Complete();
}

public class NullableCustomBearerWithHandlingAlwaysAddStringBearer<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType) : IStringBearer where TCloakedStruct : struct
{
    public TCloakedStruct? AlwaysAddCustomBearer { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysRevealAs(nameof(AlwaysAddCustomBearer), AlwaysAddCustomBearer, palantírReveal, flags)
           .Complete();
}

public class StringBearerAlwaysAddStringBearer<TBearer>(TBearer? value) : IStringBearer where TBearer : IStringBearer
{
    public TBearer? AlwaysAddStringBearer { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal(nameof(AlwaysAddStringBearer), AlwaysAddStringBearer)
           .Complete();
}

public class StringBearerWithHandlingAlwaysAddStringBearer<TBearer>(TBearer? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType) : IStringBearer
    where TBearer : IStringBearer
{
    public TBearer? AlwaysAddStringBearer { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysRevealAs(nameof(AlwaysAddStringBearer), AlwaysAddStringBearer, flags)
           .Complete();
}

public class NullableStringBearerAlwaysAddStringBearer<TBearerStruct>(TBearerStruct? value) : IStringBearer where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? AlwaysAddStringBearer { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal(nameof(AlwaysAddStringBearer), AlwaysAddStringBearer)
           .Complete();
}

public class NullableStringBearerWithHandlingAlwaysAddStringBearer<TBearerStruct>(TBearerStruct? value
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType) : IStringBearer where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? AlwaysAddStringBearer { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysRevealAs(nameof(AlwaysAddStringBearer), AlwaysAddStringBearer, flags)
           .Complete();
}

public class CharSpanWithHandlingAlwaysAddStringBearer(string value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType
  , string? formatString = null) : IStringBearer
{
    public string AlwaysAddReadOnlyCharSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddAs(nameof(AlwaysAddReadOnlyCharSpan), AlwaysAddReadOnlyCharSpan.AsSpan(), flags, formatString)
           .Complete();
}

public class CharSpanAutoAlwaysAddStringBearer(string value, string? formatString = null) : IStringBearer
{
    public string AlwaysAddReadOnlyCharSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(AlwaysAddReadOnlyCharSpan), AlwaysAddReadOnlyCharSpan.AsSpan(), formatString)
           .Complete();
}

public class StringWithHandlingAlwaysAddStringBearer
    (string? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) : IStringBearer
{
    public string? AlwaysAddString { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddAs(nameof(AlwaysAddString), AlwaysAddString, flags, formatString)
           .Complete();
}

public class StringAutoAlwaysAddStringBearer(string? value, string? formatString = null) : IStringBearer
{
    public string? AlwaysAddString { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(AlwaysAddString), AlwaysAddString, formatString)
           .Complete();
}

public class StringRangeWithHandlingAlwaysAddStringBearer(string? value, int fromIndex, int length
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) : IStringBearer
{
    public string? AlwaysAddStringRange { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddAs(nameof(AlwaysAddStringRange), AlwaysAddStringRange, fromIndex, length, flags, formatString)
           .Complete();
}

public class StringRangeAutoAlwaysAddStringBearer(string? value, int fromIndex, int length, string? formatString = null) : IStringBearer
{
    public string? AlwaysAddStringRange { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(AlwaysAddStringRange), AlwaysAddStringRange, fromIndex, length, formatString)
           .Complete();
}

public class CharArrayWithHandlingAlwaysAddStringBearer
    (char[]? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) : IStringBearer
{
    public char[]? AlwaysAddCharArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddAs(nameof(AlwaysAddCharArray), AlwaysAddCharArray, flags, formatString)
           .Complete();
}

public class CharArrayAutoAlwaysAddStringBearer(char[]? value, string? formatString = null) : IStringBearer
{
    public char[]? AlwaysAddCharArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(AlwaysAddCharArray), AlwaysAddCharArray, formatString)
           .Complete();
}

public class CharArrayRangeWithHandlingAlwaysAddStringBearer
    (char[]? value, int fromIndex, int length, FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null)
    : IStringBearer
{
    public char[]? AlwaysAddCharArrayRange { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddAs(nameof(AlwaysAddCharArrayRange), AlwaysAddCharArrayRange, fromIndex, length, flags, formatString)
           .Complete();
}

public class CharArrayRangeAutoAlwaysAddStringBearer(char[]? value, int fromIndex, int length, string? formatString = null) : IStringBearer
{
    public char[]? AlwaysAddCharArrayRange { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(AlwaysAddCharArrayRange), AlwaysAddCharArrayRange, fromIndex, length, formatString)
           .Complete();
}

public class CharSequenceWithHandlingAlwaysAddStringBearer<TCharSeq>(TCharSeq? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType
  , string? formatString = null) : IStringBearer where TCharSeq : ICharSequence
{
    public TCharSeq? AlwaysAddCharSequence { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddCharSeqAs(nameof(AlwaysAddCharSequence), AlwaysAddCharSequence, flags, formatString)
           .Complete();
}

public class CharSequenceAutoAlwaysAddStringBearer<TCharSeq>(TCharSeq? value, string? formatString = null) : IStringBearer
    where TCharSeq : ICharSequence
{
    public TCharSeq? AlwaysAddCharSequence { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddCharSeq(nameof(AlwaysAddCharSequence), AlwaysAddCharSequence, formatString)
           .Complete();
}

public class CharSequenceRangeWithHandlingAlwaysAddStringBearer<TCharSeq>(TCharSeq? value, int fromIndex, int length
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) : IStringBearer where TCharSeq : ICharSequence
{
    public TCharSeq? AlwaysAddCharSequenceRange { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddCharSeqAs(nameof(AlwaysAddCharSequenceRange), AlwaysAddCharSequenceRange, fromIndex, length, flags, formatString)
           .Complete();
}

public class CharSequenceRangeAutoAlwaysAddStringBearer<TCharSeq>(TCharSeq? value, int fromIndex, int length, string? formatString = null) : IStringBearer
    where TCharSeq : ICharSequence
{
    public TCharSeq? AlwaysAddCharSequenceRange { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddCharSeq(nameof(AlwaysAddCharSequenceRange), AlwaysAddCharSequenceRange, fromIndex, length, formatString)
           .Complete();
}

public class StringBuilderWithHandlingAlwaysAddStringBearer(StringBuilder? value, FieldContentHandling flags = FieldContentHandling.DefaultForValueType
  , string? formatString = null) : IStringBearer
{
    public StringBuilder? AlwaysAddStringBuilder { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddAs(nameof(AlwaysAddStringBuilder), AlwaysAddStringBuilder, flags, formatString)
           .Complete();
}

public class StringBuilderAutoAlwaysAddStringBearer(StringBuilder? value, string? formatString = null) : IStringBearer
{
    public StringBuilder? AlwaysAddStringBuilder { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(AlwaysAddStringBuilder), AlwaysAddStringBuilder, formatString)
           .Complete();
}

public class StringBuilderRangeWithHandlingAlwaysAddStringBearer
    (StringBuilder? value, int fromIndex, int length, FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null)
    : IStringBearer
{
    public StringBuilder? AlwaysAddStringBuilderRange { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddAs(nameof(AlwaysAddStringBuilderRange), AlwaysAddStringBuilderRange, fromIndex, length, flags, formatString)
           .Complete();
}

public class StringBuilderRangeAutoAlwaysAddStringBearer(StringBuilder? value, int fromIndex, int length, string? formatString = null) : IStringBearer
{
    public StringBuilder? AlwaysAddStringBuilderRange { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(AlwaysAddStringBuilderRange), AlwaysAddStringBuilderRange, fromIndex, length, formatString)
           .Complete();
}

public class ObjectAlwaysAddStringBearer(object value, string? formatString = null) : IStringBearer
{
    public object? AlwaysAddObject { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddObject(nameof(AlwaysAddObject), AlwaysAddObject, formatString)
           .Complete();
}

public class MatchAlwaysAddStringBearer<T>(T value, string? formatString = null) : IStringBearer
{
    public T AlwaysAddMatch { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddMatch(nameof(AlwaysAddMatch), AlwaysAddMatch, formatString)
           .Complete();
}
