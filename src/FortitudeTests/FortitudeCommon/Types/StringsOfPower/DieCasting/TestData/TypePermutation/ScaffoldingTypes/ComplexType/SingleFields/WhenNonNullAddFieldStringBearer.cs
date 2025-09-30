using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexType.SingleFields;

public class NullableBoolWhenNonNullStringBearer(bool? value) : IStringBearer
{
    public bool? AlwaysAddNullableBool { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd(nameof(AlwaysAddNullableBool), AlwaysAddNullableBool)
           .Complete();
}

public class SpanFormattableWhenNonNullStringBearer<TFmt>(TFmt? value, string? formatString = null) : IStringBearer
    where TFmt : ISpanFormattable
{
    public TFmt? AlwaysAddSpanFormattable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd(nameof(AlwaysAddSpanFormattable), AlwaysAddSpanFormattable, formatString)
           .Complete();
}

public class SpanFormattableWithHandlingWhenNonNullStringBearer<TFmt>(TFmt? value
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) : IStringBearer
    where TFmt : ISpanFormattable
{
    public TFmt? AlwaysAddSpanFormattable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddAs(nameof(AlwaysAddSpanFormattable), AlwaysAddSpanFormattable, flags, formatString)
           .Complete();
}

public class NullableSpanFormattableWhenNonNullStringBearer<TFmtStruct>(TFmtStruct? value, string? formatString = null) : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? AlwaysAddNullableSpanFormattable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd(nameof(AlwaysAddNullableSpanFormattable), AlwaysAddNullableSpanFormattable, formatString)
           .Complete();
}

public class NullableSpanFormattableWithHandlingWhenNonNullStringBearer<TFmtStruct>(TFmtStruct? value
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? AlwaysAddNullableSpanFormattable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddAs(nameof(AlwaysAddNullableSpanFormattable), AlwaysAddNullableSpanFormattable, flags, formatString)
           .Complete();
}

public class CustomBearerWhenNonNullStringBearer<TTCloaked, TCloakedBase>(TTCloaked? value, PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer
    where TTCloaked : TCloakedBase
{
    public TTCloaked? AlwaysAddCustomBearer { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullReveal(nameof(AlwaysAddCustomBearer), AlwaysAddCustomBearer, palantírReveal)
           .Complete();
}

public class CustomBearerWithHandlingWhenNonNullStringBearer<TTCloaked, TCloakedBase>(TTCloaked? value, PalantírReveal<TCloakedBase> palantírReveal
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType) : IStringBearer
    where TTCloaked : TCloakedBase
{
    public TTCloaked? AlwaysAddCustomBearer { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullRevealAs(nameof(AlwaysAddCustomBearer), AlwaysAddCustomBearer, palantírReveal, flags)
           .Complete();
}

public class NullableCustomBearerWhenNonNullStringBearer<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer
    where TCloakedStruct : struct
{
    public TCloakedStruct? AlwaysAddCustomBearer { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullReveal(nameof(AlwaysAddCustomBearer), AlwaysAddCustomBearer, palantírReveal)
           .Complete();
}

public class NullableCustomBearerWithHandlingWhenNonNullStringBearer<TCloakedStruct>(TCloakedStruct? value, PalantírReveal<TCloakedStruct> palantírReveal
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType) : IStringBearer
    where TCloakedStruct : struct
{
    public TCloakedStruct? AlwaysAddCustomBearer { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullRevealAs(nameof(AlwaysAddCustomBearer), AlwaysAddCustomBearer, palantírReveal, flags)
           .Complete();
}

public class StringBearerWhenNonNullStringBearer<TBearer>(TBearer? value) : IStringBearer
    where TBearer : IStringBearer
{
    public TBearer? WhenNonNullStringBearer { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullReveal(nameof(WhenNonNullStringBearer), WhenNonNullStringBearer)
           .Complete();
}

public class StringBearerWithHandlingWhenNonNullStringBearer<TBearer>(TBearer? value
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType) : IStringBearer
    where TBearer : IStringBearer
{
    public TBearer? WhenNonNullStringBearer { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullRevealAs(nameof(WhenNonNullStringBearer), WhenNonNullStringBearer, flags)
           .Complete();
}

public class NullableStringBearerWhenNonNullStringBearer<TBearerStruct>(TBearerStruct? value) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? WhenNonNullStringBearer { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullReveal(nameof(WhenNonNullStringBearer), WhenNonNullStringBearer)
           .Complete();
}

public class NullableStringBearerWithHandlingWhenNonNullStringBearer<TBearerStruct>(TBearerStruct? value
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? WhenNonNullStringBearer { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullRevealAs(nameof(WhenNonNullStringBearer), WhenNonNullStringBearer, flags)
           .Complete();
}

public class StringWhenNonNullStringBearer(string? value, string? formatString = null) : IStringBearer
{
    public string? AlwaysAddString { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd(nameof(AlwaysAddString), AlwaysAddString, formatString)
           .Complete();
}

public class StringWithHandlingWhenNonNullStringBearer(string? value
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) : IStringBearer
{
    public string? AlwaysAddString { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddAs(nameof(AlwaysAddString), AlwaysAddString, flags, formatString)
           .Complete();
}

public class StringRangeWhenNonNullStringBearer(string? value, int fromIndex, int length, string? formatString = null) : IStringBearer
{
    public string? AlwaysAddStringRange { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd(nameof(AlwaysAddStringRange), AlwaysAddStringRange, fromIndex, length, formatString)
           .Complete();
}

public class StringRangeWithHandlingWhenNonNullStringBearer(string? value, int fromIndex, int length
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) : IStringBearer
{
    public string? AlwaysAddStringRange { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddAs(nameof(AlwaysAddStringRange), AlwaysAddStringRange, fromIndex, length, flags, formatString)
           .Complete();
}

public class CharArrayWhenNonNullStringBearer(char[]? value, string? formatString = null) : IStringBearer
{
    public char[]? AlwaysAddCharArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd(nameof(AlwaysAddCharArray), AlwaysAddCharArray, formatString)
           .Complete();
}

public class CharArrayWithHandlingWhenNonNullStringBearer(char[]? value
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) : IStringBearer
{
    public char[]? AlwaysAddCharArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddAs(nameof(AlwaysAddCharArray), AlwaysAddCharArray, flags, formatString)
           .Complete();
}

public class CharArrayRangeWhenNonNullStringBearer(char[]? value, int fromIndex, int length, string? formatString = null) : IStringBearer
{
    public char[]? AlwaysAddCharArrayRange { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd(nameof(AlwaysAddCharArrayRange), AlwaysAddCharArrayRange, fromIndex, length, formatString)
           .Complete();
}

public class CharArrayRangeWithHandlingWhenNonNullStringBearer(char[]? value, int fromIndex, int length
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) : IStringBearer
{
    public char[]? AlwaysAddCharArrayRange { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddAs(nameof(AlwaysAddCharArrayRange), AlwaysAddCharArrayRange, fromIndex, length, flags, formatString)
           .Complete();
}

public class CharSequenceWhenNonNullStringBearer<TCharSeq>(TCharSeq? value, string? formatString = null) : IStringBearer
    where TCharSeq : ICharSequence
{
    public TCharSeq? AlwaysAddCharSequence { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddCharSeq(nameof(AlwaysAddCharSequence), AlwaysAddCharSequence, formatString)
           .Complete();
}

public class CharSequenceWithHandlingWhenNonNullStringBearer<TCharSeq>(TCharSeq? value
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) : IStringBearer
    where TCharSeq : ICharSequence
{
    public TCharSeq? AlwaysAddCharSequence { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddCharSeqAs(nameof(AlwaysAddCharSequence), AlwaysAddCharSequence, flags, formatString)
           .Complete();
}

public class CharSequenceRangeWhenNonNullStringBearer<TCharSeq>(TCharSeq? value, int fromIndex, int length, string? formatString = null) : IStringBearer
    where TCharSeq : ICharSequence
{
    public TCharSeq? AlwaysAddCharSequenceRange { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddCharSeq(nameof(AlwaysAddCharSequenceRange), AlwaysAddCharSequenceRange, fromIndex, length, formatString)
           .Complete();
}

public class CharSequenceRangeWithHandlingWhenNonNullStringBearer<TCharSeq>(TCharSeq? value, int fromIndex, int length
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) : IStringBearer
    where TCharSeq : ICharSequence
{
    public TCharSeq? AlwaysAddCharSequenceRange { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddCharSeqAs(nameof(AlwaysAddCharSequenceRange), AlwaysAddCharSequenceRange, fromIndex, length, flags, formatString)
           .Complete();
}

public class StringBuilderWhenNonNullStringBearer(StringBuilder? value, string? formatString = null) : IStringBearer
{
    public StringBuilder? AlwaysAddStringBuilder { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd(nameof(AlwaysAddStringBuilder), AlwaysAddStringBuilder, formatString)
           .Complete();
}

public class StringBuilderWithHandlingWhenNonNullStringBearer(StringBuilder? value
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) : IStringBearer
{
    public StringBuilder? AlwaysAddStringBuilder { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddAs(nameof(AlwaysAddStringBuilder), AlwaysAddStringBuilder, flags, formatString)
           .Complete();
}

public class StringBuilderRangeAlwaysStringBearer(StringBuilder? value, int fromIndex, int length, string? formatString = null) : IStringBearer
{
    public StringBuilder? AlwaysAddStringBuilderRange { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd(nameof(AlwaysAddStringBuilderRange), AlwaysAddStringBuilderRange, fromIndex, length, formatString)
           .Complete();
}

public class StringBuilderRangeWithHandlingAlwaysStringBearer(StringBuilder? value, int fromIndex, int length
  , FieldContentHandling flags = FieldContentHandling.DefaultForValueType, string? formatString = null) : IStringBearer
{
    public StringBuilder? AlwaysAddStringBuilderRange { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddAs(nameof(AlwaysAddStringBuilderRange), AlwaysAddStringBuilderRange, fromIndex, length, flags, formatString)
           .Complete();
}

public class ObjectWhenNonNullStringBearer(object value, string? formatString = null) : IStringBearer
{
    public object? AlwaysAddObject { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddObject(nameof(AlwaysAddObject), AlwaysAddObject, formatString)
           .Complete();
}

public class MatchWhenNonNullStringBearer<T>(T value, string? formatString = null) : IStringBearer
{
    public T AlwaysAddMatch { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddMatch(nameof(AlwaysAddMatch), AlwaysAddMatch, formatString)
           .Complete();
}
