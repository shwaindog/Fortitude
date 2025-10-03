using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.TypeGeneratePartFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.SingleFields;

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsNullableStruct)]
public class NullableBoolWhenNonNullStringBearer : IStringBearer, IMoldSupportedValue<bool?>
{
    public bool? ComplexTypeFieldWhenNonNullAddNullableBool
    {
        get => Value;
        set => Value = value;
    }

    public bool? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd(nameof(ComplexTypeFieldWhenNonNullAddNullableBool), ComplexTypeFieldWhenNonNullAddNullableBool)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class SpanFormattableWhenNonNullStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt?>, ISupportsSingleFormatString
    where TFmt : ISpanFormattable
{
    public TFmt? ComplexTypeFieldWhenNonNullAddSpanFormattable
    {
        get => Value;
        set => Value = value;
    }

    public TFmt? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd(nameof(ComplexTypeFieldWhenNonNullAddSpanFormattable), ComplexTypeFieldWhenNonNullAddSpanFormattable, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString | SupportsCustomHandling)]
public class SpanFormattableWithHandlingWhenNonNullStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt?>, ISupportsSingleFormatString
  , ISupportsFieldHandling where TFmt : ISpanFormattable
{
    public TFmt? ComplexTypeFieldWhenNonNullAddSpanFormattableAs
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public TFmt? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddAs(nameof(ComplexTypeFieldWhenNonNullAddSpanFormattableAs), ComplexTypeFieldWhenNonNullAddSpanFormattableAs, FieldContentHandling
                                 , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class NullableSpanFormattableWhenNonNullStringBearer<TFmtStruct> : IStringBearer
  , IMoldSupportedValue<TFmtStruct?>, ISupportsSingleFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? ComplexTypeFieldWhenNonNullAddNullableSpanFormattable
    {
        get => Value;
        set => Value = value;
    }

    public TFmtStruct? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd(nameof(ComplexTypeFieldWhenNonNullAddNullableSpanFormattable), ComplexTypeFieldWhenNonNullAddNullableSpanFormattable
                               , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString | SupportsCustomHandling)]
public class NullableSpanFormattableWithHandlingWhenNonNullStringBearer<TFmtStruct> : IStringBearer
  , IMoldSupportedValue<TFmtStruct?>, ISupportsFieldHandling, ISupportsSingleFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? ComplexTypeFieldWhenNonNullAddNullableSpanFormattableAs
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public TFmtStruct? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddAs(nameof(ComplexTypeFieldWhenNonNullAddNullableSpanFormattableAs), ComplexTypeFieldWhenNonNullAddNullableSpanFormattableAs, FieldContentHandling
                                 , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsChars | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class CloakedBearerWhenNonNullStringBearer<TTCloaked, TCloakedBase>
    : IStringBearer, IMoldSupportedValue<TTCloaked?>, ISupportsSingleRevealer<TCloakedBase>
    where TTCloaked : TCloakedBase
{
    public TTCloaked? ComplexTypeFieldWhenNonNullRevealCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public TTCloaked? Value { get; set; }

    public PalantírReveal<TCloakedBase> PalantirRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullReveal(nameof(ComplexTypeFieldWhenNonNullRevealCloakedBearer), ComplexTypeFieldWhenNonNullRevealCloakedBearer, PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsChars
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | OnePalantirRevealer | SupportsCustomHandling)]
public class CloakedBearerWithHandlingWhenNonNullStringBearer<TTCloaked, TCloakedBase> : IStringBearer, IMoldSupportedValue<TTCloaked?>
  , ISupportsFieldHandling, ISupportsSingleRevealer<TCloakedBase> where TTCloaked : TCloakedBase
{
    public TTCloaked? ComplexTypeFieldWhenNonNullRevealCloakedBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public TTCloaked? Value { get; set; }

    public PalantírReveal<TCloakedBase> PalantirRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullRevealAs(nameof(ComplexTypeFieldWhenNonNullRevealCloakedBearerAs), ComplexTypeFieldWhenNonNullRevealCloakedBearerAs, PalantirRevealer
                                    , FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class NullableCloakedBearerWhenNonNullStringBearer<TCloakedStruct> : IStringBearer, IMoldSupportedValue<TCloakedStruct?>
    where TCloakedStruct : struct
{
    public TCloakedStruct? ComplexTypeFieldWhenNonNullRevealNullableCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public TCloakedStruct? Value { get; set; }


    public PalantírReveal<TCloakedStruct> PalantirRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullReveal(nameof(ComplexTypeFieldWhenNonNullRevealNullableCloakedBearer), ComplexTypeFieldWhenNonNullRevealNullableCloakedBearer, PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | OnePalantirRevealer | SupportsCustomHandling)]
public class NullableCloakedBearerWithHandlingWhenNonNullStringBearer<TCloakedStruct> : IStringBearer, IMoldSupportedValue<TCloakedStruct?>
  , ISupportsSingleRevealer<TCloakedStruct>, ISupportsFieldHandling where TCloakedStruct : struct
{
    public TCloakedStruct? ComplexTypeFieldWhenNonNullRevealNullableCloakedBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public TCloakedStruct? Value { get; set; }

    public PalantírReveal<TCloakedStruct> PalantirRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullRevealAs(nameof(ComplexTypeFieldWhenNonNullRevealNullableCloakedBearerAs), 
                                      ComplexTypeFieldWhenNonNullRevealNullableCloakedBearerAs, PalantirRevealer, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class StringBearerWhenNonNullStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer?>
    where TBearer : IStringBearer
{
    public TBearer? ComplexTypeFieldWhenNonNullRevealStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public TBearer? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullReveal(nameof(ComplexTypeFieldWhenNonNullRevealStringBearer), ComplexTypeFieldWhenNonNullRevealStringBearer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer |
                  SupportsCustomHandling)]
public class StringBearerWithHandlingWhenNonNullStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer?>, ISupportsFieldHandling 
    where TBearer : IStringBearer
{
    public TBearer? ComplexTypeFieldWhenNonNullRevealStringBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public TBearer? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullRevealAs(nameof(ComplexTypeFieldWhenNonNullRevealStringBearerAs), ComplexTypeFieldWhenNonNullRevealStringBearerAs, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsStruct |  AcceptsNullableStruct | AcceptsStringBearer)]
public class NullableStringBearerWhenNonNullStringBearer<TBearerStruct> : IStringBearer, IMoldSupportedValue<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? ComplexTypeFieldWhenNonNullRevealNullableStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public TBearerStruct? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullReveal(nameof(ComplexTypeFieldWhenNonNullRevealNullableStringBearer), ComplexTypeFieldWhenNonNullRevealNullableStringBearer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites |AcceptsStruct | AcceptsNullableStruct | AcceptsStringBearer | SupportsCustomHandling)]
public class NullableStringBearerWithHandlingWhenNonNullStringBearer<TBearerStruct> : IStringBearer, IMoldSupportedValue<TBearerStruct?>
  , ISupportsFieldHandling
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? ComplexTypeFieldWhenNonNullRevealNullableStringBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public TBearerStruct? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullRevealAs(nameof(ComplexTypeFieldWhenNonNullRevealNullableStringBearerAs), ComplexTypeFieldWhenNonNullRevealNullableStringBearerAs, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | OneFormatString)]
public class StringWhenNonNullStringBearer : IStringBearer, IMoldSupportedValue<string?>, ISupportsSingleFormatString, ISupportsSettingValueFromString
{
    public string? ComplexTypeFieldWhenNonNullAddString
    {
        get => Value;
        set => Value = value;
    }

    public string? Value { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd(nameof(ComplexTypeFieldWhenNonNullAddString), ComplexTypeFieldWhenNonNullAddString, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | OneFormatString | SupportsCustomHandling)]
public class StringWithHandlingWhenNonNullStringBearer : IStringBearer, IMoldSupportedValue<string?>, ISupportsSingleFormatString
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public string? ComplexTypeFieldWhenNonNullAddStringAs
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? Value { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddAs(nameof(ComplexTypeFieldWhenNonNullAddStringAs), ComplexTypeFieldWhenNonNullAddStringAs, FieldContentHandling, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | OneFormatString | SupportsIndexSubRanges)]
public class StringRangeWhenNonNullStringBearer : IStringBearer, ISupportsSingleFormatString
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public string? ComplexTypeFieldWhenNonNullAddStringRange
    {
        get => Value;
        set => Value = value;
    }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? Value { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd(nameof(ComplexTypeFieldWhenNonNullAddStringRange), ComplexTypeFieldWhenNonNullAddStringRange, FromIndex, Length, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | OneFormatString | SupportsIndexSubRanges | SupportsCustomHandling)]
public class StringRangeWithHandlingWhenNonNullStringBearer : IStringBearer, IMoldSupportedValue<string?>, ISupportsSingleFormatString
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling
{
    public string? ComplexTypeFieldWhenNonNullAddStringRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? Value { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddAs(nameof(ComplexTypeFieldWhenNonNullAddStringRangeAs), ComplexTypeFieldWhenNonNullAddStringRangeAs, FromIndex, Length
                                 , FieldContentHandling
                                 , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | OneFormatString)]
public class CharArrayWhenNonNullStringBearer : IStringBearer, IMoldSupportedValue<char[]?>, ISupportsSingleFormatString
  , ISupportsSettingValueFromString
{
    public char[]? ComplexTypeFieldWhenNonNullAddCharArray
    {
        get => Value;
        set => Value = value;
    }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public char[]? Value { get; set; }

    public string? StringValue
    {
        get => Value != null ? new string(Value) : null;
        set => Value = value?.ToCharArray();
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd(nameof(ComplexTypeFieldWhenNonNullAddCharArray), ComplexTypeFieldWhenNonNullAddCharArray, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | OneFormatString | SupportsCustomHandling)]
public class CharArrayWithHandlingWhenNonNullStringBearer: IStringBearer, IMoldSupportedValue<char[]?>, ISupportsSingleFormatString
, ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public char[]? ComplexTypeFieldWhenNonNullAddCharArrayAs
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public char[]? Value { get; set; }

    public string? StringValue
    {
        get => Value != null ? new string(Value) : null;
        set => Value = value?.ToCharArray();
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddAs(nameof(ComplexTypeFieldWhenNonNullAddCharArrayAs), ComplexTypeFieldWhenNonNullAddCharArrayAs, FieldContentHandling, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | OneFormatString | SupportsIndexSubRanges)]
public class CharArrayRangeWhenNonNullStringBearer : IStringBearer, IMoldSupportedValue<char[]?>, ISupportsSingleFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[]? ComplexTypeFieldWhenNonNullAddCharArrayRange
    {
        get => Value;
        set => Value = value;
    }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public char[]? Value { get; set; }

    public string? StringValue
    {
        get => Value != null ? new string(Value) : null;
        set => Value = value?.ToCharArray();
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd(nameof(ComplexTypeFieldWhenNonNullAddCharArrayRange), ComplexTypeFieldWhenNonNullAddCharArrayRange, FromIndex, Length, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | OneFormatString | SupportsIndexSubRanges | SupportsCustomHandling)]
public class CharArrayRangeWithHandlingWhenNonNullStringBearer : IStringBearer, IMoldSupportedValue<char[]?>, ISupportsSingleFormatString
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling
{
    public char[]? ComplexTypeFieldWhenNonNullAddCharArrayRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public char[]? Value { get; set; }

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value != null ? new string(Value) : null;
        set => Value = value?.ToCharArray();
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddAs(nameof(ComplexTypeFieldWhenNonNullAddCharArrayRangeAs), ComplexTypeFieldWhenNonNullAddCharArrayRangeAs, FromIndex, Length
                                 , FieldContentHandling
                                 , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | OneFormatString)]
public class CharSequenceWhenNonNullStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq?>, ISupportsSingleFormatString
  , ISupportsSettingValueFromString where TCharSeq : ICharSequence
{
    public TCharSeq? ComplexTypeFieldWhenNonNullAddCharSequence
    {
        get => Value;
        set => Value = value;
    }

    public TCharSeq? Value { get; set; }

    public string? StringValue
    {
        get => Value != null ? Value.ToString() : null;
        set
        {
            var typeOfCharSeq = typeof(TCharSeq);

            if (typeOfCharSeq == typeof(CharArrayStringBuilder)) { Value = (TCharSeq)(object)new CharArrayStringBuilder(value); }
            else { Value                                                 = (TCharSeq)(object)new MutableString(value); }
        }
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddCharSeq(nameof(ComplexTypeFieldWhenNonNullAddCharSequence), ComplexTypeFieldWhenNonNullAddCharSequence, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | OneFormatString | SupportsCustomHandling)]
public class CharSequenceWithHandlingWhenNonNullStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq?>, ISupportsSingleFormatString
  , ISupportsSettingValueFromString, ISupportsFieldHandling where TCharSeq : ICharSequence
{
    public TCharSeq? ComplexTypeFieldWhenNonNullAddCharSequenceAs
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public TCharSeq? Value { get; set; }

    public string? StringValue
    {
        get => Value != null ? Value.ToString() : null;
        set
        {
            var typeOfCharSeq = typeof(TCharSeq);

            if (typeOfCharSeq == typeof(CharArrayStringBuilder)) { Value = (TCharSeq)(object)new CharArrayStringBuilder(value); }
            else { Value                                                 = (TCharSeq)(object)new MutableString(value); }
        }
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddCharSeqAs(nameof(ComplexTypeFieldWhenNonNullAddCharSequenceAs), ComplexTypeFieldWhenNonNullAddCharSequenceAs, FieldContentHandling
                                        , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | OneFormatString | SupportsIndexSubRanges)]
public class CharSequenceRangeWhenNonNullStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq?>, ISupportsSingleFormatString
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq? ComplexTypeFieldWhenNonNullAddCharSequenceRange
    {
        get => Value;
        set => Value = value;
    }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public TCharSeq? Value { get; set; }

    public string? StringValue
    {
        get => Value != null ? Value.ToString() : null;
        set
        {
            var typeOfCharSeq = typeof(TCharSeq);

            if (typeOfCharSeq == typeof(CharArrayStringBuilder)) { Value = (TCharSeq)(object)new CharArrayStringBuilder(value); }
            else { Value                                                 = (TCharSeq)(object)new MutableString(value); }
        }
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddCharSeq(nameof(ComplexTypeFieldWhenNonNullAddCharSequenceRange), ComplexTypeFieldWhenNonNullAddCharSequenceRange, FromIndex, Length
                                      , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | OneFormatString | SupportsIndexSubRanges | SupportsCustomHandling)]
public class CharSequenceRangeWithHandlingWhenNonNullStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq?>, ISupportsSingleFormatString
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling where TCharSeq : ICharSequence
{
    public TCharSeq? ComplexTypeFieldWhenNonNullAddCharSequenceRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public FieldContentHandling FieldContentHandling { get; set; }

    public TCharSeq? Value { get; set; }

    public string? StringValue
    {
        get => Value != null ? Value.ToString() : null;
        set
        {
            var typeOfCharSeq = typeof(TCharSeq);

            if (typeOfCharSeq == typeof(CharArrayStringBuilder)) { Value = (TCharSeq)(object)new CharArrayStringBuilder(value); }
            else { Value                                                 = (TCharSeq)(object)new MutableString(value); }
        }
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddCharSeqAs(nameof(ComplexTypeFieldWhenNonNullAddCharSequenceRangeAs), ComplexTypeFieldWhenNonNullAddCharSequenceRangeAs, FromIndex, Length
                                        , FieldContentHandling, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | OneFormatString)]
public class StringBuilderWhenNonNullStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder?>
  , ISupportsSingleFormatString, ISupportsSettingValueFromString
{
    public StringBuilder? ComplexTypeFieldWhenNonNullAddStringBuilder
    {
        get => Value;
        set => Value = value;
    }

    public StringBuilder? Value { get; set; }

    public string? StringValue
    {
        get => Value?.ToString();
        set => Value = new StringBuilder(value);
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd(nameof(ComplexTypeFieldWhenNonNullAddStringBuilder), ComplexTypeFieldWhenNonNullAddStringBuilder, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | OneFormatString | SupportsCustomHandling)]
public class StringBuilderWithHandlingWhenNonNullStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder?>, ISupportsSingleFormatString
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public StringBuilder? ComplexTypeFieldWhenNonNullAddStringBuilderAs
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public StringBuilder? Value { get; set; }

    public string? StringValue
    {
        get => Value?.ToString();
        set => Value = new StringBuilder(value);
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddAs(nameof(ComplexTypeFieldWhenNonNullAddStringBuilderAs), ComplexTypeFieldWhenNonNullAddStringBuilderAs, FieldContentHandling
                                 , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | OneFormatString | SupportsIndexSubRanges)]
public class StringBuilderRangeWhenNonNullStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder?>, ISupportsSingleFormatString
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public StringBuilder? ComplexTypeFieldWhenNonNullAddStringBuilderRange
    {
        get => Value;
        set => Value = value;
    }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public StringBuilder? Value { get; set; }

    public string? StringValue
    {
        get => Value?.ToString();
        set => Value = new StringBuilder(value);
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd(nameof(ComplexTypeFieldWhenNonNullAddStringBuilderRange), ComplexTypeFieldWhenNonNullAddStringBuilderRange, FromIndex, Length
                               , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | OneFormatString | SupportsIndexSubRanges | SupportsCustomHandling)]
public class StringBuilderRangeWithHandlingWhenNonNullStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder?>, ISupportsSingleFormatString, ISupportsSettingValueFromString
  , ISupportsIndexRangeLimiting, ISupportsFieldHandling
{
    public StringBuilder? ComplexTypeFieldWhenNonNullAddStringBuilderRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public FieldContentHandling FieldContentHandling { get; set; }

    public StringBuilder? Value { get; set; }

    public string? StringValue
    {
        get => Value?.ToString();
        set => Value = new StringBuilder(value);
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddAs(nameof(ComplexTypeFieldWhenNonNullAddStringBuilderRangeAs), ComplexTypeFieldWhenNonNullAddStringBuilderRangeAs, FromIndex, Length
                                 , FieldContentHandling, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsAny | OneFormatString)]
public class MatchWhenNonNullStringBearer<T> : IStringBearer, IMoldSupportedValue<T?>, ISupportsSingleFormatString
{
    public T? ComplexTypeFieldWhenNonNullAddMatch
    {
        get => Value;
        set => Value = value;
    }

    public T? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddMatch(nameof(ComplexTypeFieldWhenNonNullAddMatch), ComplexTypeFieldWhenNonNullAddMatch, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsAny | OneFormatString)]
public class ObjectWhenNonNullStringBearer : IStringBearer, IMoldSupportedValue<object?>, ISupportsSingleFormatString
{
    public object? ComplexTypeFieldWhenNonNullAddObject
    {
        get => Value;
        set => Value = value;
    }

    public object? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddObject(nameof(ComplexTypeFieldWhenNonNullAddObject), ComplexTypeFieldWhenNonNullAddObject, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}
