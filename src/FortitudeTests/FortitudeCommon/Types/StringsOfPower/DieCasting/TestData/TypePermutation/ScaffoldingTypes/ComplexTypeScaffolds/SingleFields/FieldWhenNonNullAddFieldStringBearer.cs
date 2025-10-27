using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.SingleFields;

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsNullableStruct | SupportsValueFormatString)]
public class FieldNullableBoolWhenNonNullStringBearer : IMoldSupportedValue<bool?>, ISupportsValueFormatString
{
    public bool? ComplexTypeFieldWhenNonNullAddNullableBool
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddNullableBool);
    public bool? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddNullableBool)
              , ComplexTypeFieldWhenNonNullAddNullableBool
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldSpanFormattableWhenNonNullStringBearer<TFmt> : IMoldSupportedValue<TFmt?>, ISupportsValueFormatString
  , ISupportsFieldHandling where TFmt : ISpanFormattable
{
    public TFmt? ComplexTypeFieldWhenNonNullAddSpanFormattableAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddSpanFormattableAs);
    public FieldContentHandling FieldContentHandling { get; set; }

    public TFmt? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddSpanFormattableAs)
              , ComplexTypeFieldWhenNonNullAddSpanFormattableAs
              , ValueFormatString, FieldContentHandling)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldNullableSpanFormattableWhenNonNullStringBearer<TFmtStruct> : IMoldSupportedValue<TFmtStruct?>, ISupportsFieldHandling
  , ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? ComplexTypeFieldWhenNonNullAddNullableSpanFormattableAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddNullableSpanFormattableAs);
    public FieldContentHandling FieldContentHandling { get; set; }

    public TFmtStruct? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddNullableSpanFormattableAs)
              , ComplexTypeFieldWhenNonNullAddNullableSpanFormattableAs
              , ValueFormatString, FieldContentHandling)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass 
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer | SupportsCustomHandling)]
public class FieldCloakedBearerWhenNonNullStringBearer<TTCloaked, TCloakedBase> : IMoldSupportedValue<TTCloaked?>
  , ISupportsFieldHandling, ISupportsValueRevealer<TCloakedBase> where TTCloaked : TCloakedBase
{
    public TTCloaked? ComplexTypeFieldWhenNonNullRevealCloakedBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullRevealCloakedBearerAs);
    public FieldContentHandling FieldContentHandling { get; set; }

    public TTCloaked? Value { get; set; }

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedBase>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullReveal
               (nameof(ComplexTypeFieldWhenNonNullRevealCloakedBearerAs)
              , ComplexTypeFieldWhenNonNullRevealCloakedBearerAs
              , ValueRevealer, FieldContentHandling)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer | SupportsCustomHandling)]
public class FieldNullableCloakedBearerWhenNonNullStringBearer<TCloakedStruct> : IMoldSupportedValue<TCloakedStruct?>
  , ISupportsValueRevealer<TCloakedStruct>, ISupportsFieldHandling where TCloakedStruct : struct
{
    public TCloakedStruct? ComplexTypeFieldWhenNonNullRevealNullableCloakedBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullRevealNullableCloakedBearerAs);
    public FieldContentHandling FieldContentHandling { get; set; }

    public TCloakedStruct? Value { get; set; }

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedStruct>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullReveal
               (nameof(ComplexTypeFieldWhenNonNullRevealNullableCloakedBearerAs),
                ComplexTypeFieldWhenNonNullRevealNullableCloakedBearerAs
              , ValueRevealer, FieldContentHandling)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer |
                  SupportsCustomHandling)]
public class FieldStringBearerWhenNonNullStringBearer<TBearer> : IMoldSupportedValue<TBearer?>, ISupportsFieldHandling
    where TBearer : IStringBearer
{
    public TBearer? ComplexTypeFieldWhenNonNullRevealStringBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullRevealStringBearerAs);
    public FieldContentHandling FieldContentHandling { get; set; }

    public TBearer? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullReveal
               (nameof(ComplexTypeFieldWhenNonNullRevealStringBearerAs)
              , ComplexTypeFieldWhenNonNullRevealStringBearerAs
              , FieldContentHandling)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsNullableStruct | AcceptsStringBearer |
                  SupportsCustomHandling)]
public class FieldNullableStringBearerWhenNonNullStringBearer<TBearerStruct> : IMoldSupportedValue<TBearerStruct?>
  , ISupportsFieldHandling
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? ComplexTypeFieldWhenNonNullRevealNullableStringBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullRevealNullableStringBearerAs);
    public FieldContentHandling FieldContentHandling { get; set; }

    public TBearerStruct? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullReveal
               (nameof(ComplexTypeFieldWhenNonNullRevealNullableStringBearerAs)
              , ComplexTypeFieldWhenNonNullRevealNullableStringBearerAs
              , FieldContentHandling)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | CallsAsSpan | NonNullWrites | AcceptsCharArray | SupportsValueFormatString |
                  SupportsCustomHandling)]
public class FieldCharSpanWhenNonNullStringBearer : IMoldSupportedValue<char[]>
  , IMoldSupportedDefaultValue<string>, ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public char[] ComplexTypeFieldWhenNonNullAddCharSpanAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddCharSpanAs);
    public string DefaultValue { get; set; } = "";

    public char[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddCharSpanAs)
              , ComplexTypeFieldWhenNonNullAddCharSpanAs.AsSpan()
              , ValueFormatString, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | CallsAsReadOnlySpan | NonNullWrites | AcceptsString | SupportsValueFormatString |
                  SupportsCustomHandling)]
public class FieldCharReadOnlySpanWhenNonNullStringBearer : IMoldSupportedValue<string>
  , IMoldSupportedDefaultValue<string>, ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public string ComplexTypeFieldWhenNonNullAddReadOnlyCharSpanAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddReadOnlyCharSpanAs);
    public string DefaultValue { get; set; } = "";

    public string Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddReadOnlyCharSpanAs)
              , ComplexTypeFieldWhenNonNullAddReadOnlyCharSpanAs.AsSpan()
              , ValueFormatString, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsString | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldStringWhenNonNullStringBearer : IMoldSupportedValue<string?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public string? ComplexTypeFieldWhenNonNullAddStringAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddStringAs);
    public FieldContentHandling FieldContentHandling { get; set; }

    public string? Value { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddStringAs)
              , ComplexTypeFieldWhenNonNullAddStringAs
              , ValueFormatString, FieldContentHandling)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsString | SupportsValueFormatString | SupportsIndexSubRanges |
                  SupportsCustomHandling)]
public class FieldStringRangeWhenNonNullStringBearer : IMoldSupportedValue<string?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling
{
    public string? ComplexTypeFieldWhenNonNullAddStringRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddStringRangeAs);
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
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddStringRangeAs)
              , ComplexTypeFieldWhenNonNullAddStringRangeAs
              , FromIndex, Length, ValueFormatString, FieldContentHandling)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsArray | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldCharArrayWhenNonNullStringBearer : IMoldSupportedValue<char[]?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public char[]? ComplexTypeFieldWhenNonNullAddCharArrayAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddCharArrayAs);
    public FieldContentHandling FieldContentHandling { get; set; }

    public char[]? Value { get; set; }

    public string? StringValue
    {
        get => Value != null ? new string(Value) : null;
        set => Value = value?.ToCharArray();
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddCharArrayAs)
              , ComplexTypeFieldWhenNonNullAddCharArrayAs
              , ValueFormatString, FieldContentHandling)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsCharArray | SupportsValueFormatString | SupportsIndexSubRanges |
                  SupportsCustomHandling)]
public class FieldCharArrayRangeWhenNonNullStringBearer : IMoldSupportedValue<char[]?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling
{
    public char[]? ComplexTypeFieldWhenNonNullAddCharArrayRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddCharArrayRangeAs);
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
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddCharArrayRangeAs)
              , ComplexTypeFieldWhenNonNullAddCharArrayRangeAs
              , FromIndex, Length, ValueFormatString, FieldContentHandling)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsCharSequence | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldCharSequenceWhenNonNullStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq?>
  , ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsFieldHandling where TCharSeq : ICharSequence
{
    public TCharSeq? ComplexTypeFieldWhenNonNullAddCharSequenceAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddCharSequenceAs);
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
           .Field.WhenNonNullAddCharSeq
               (nameof(ComplexTypeFieldWhenNonNullAddCharSequenceAs)
              , ComplexTypeFieldWhenNonNullAddCharSequenceAs
              , ValueFormatString, FieldContentHandling)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsCharSequence | SupportsValueFormatString | SupportsIndexSubRanges |
                  SupportsCustomHandling)]
public class FieldCharSequenceRangeWhenNonNullStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq?>
  , ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling where TCharSeq : ICharSequence
{
    public TCharSeq? ComplexTypeFieldWhenNonNullAddCharSequenceRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddCharSequenceRangeAs);
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
           .Field.WhenNonNullAddCharSeq
               (nameof(ComplexTypeFieldWhenNonNullAddCharSequenceRangeAs)
              , ComplexTypeFieldWhenNonNullAddCharSequenceRangeAs
              , FromIndex, Length, ValueFormatString, FieldContentHandling)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsStringBuilder | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldStringBuilderWhenNonNullStringBearer : IMoldSupportedValue<StringBuilder?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public StringBuilder? ComplexTypeFieldWhenNonNullAddStringBuilderAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddStringBuilderAs);
    public FieldContentHandling FieldContentHandling { get; set; }

    public StringBuilder? Value { get; set; }

    public string? StringValue
    {
        get => Value?.ToString();
        set => Value = new StringBuilder(value);
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddStringBuilderAs)
              , ComplexTypeFieldWhenNonNullAddStringBuilderAs
              , ValueFormatString, FieldContentHandling)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsStringBuilder | SupportsValueFormatString | SupportsIndexSubRanges |
                  SupportsCustomHandling)]
public class FieldStringBuilderRangeWhenNonNullStringBearer : IMoldSupportedValue<StringBuilder?>
  , ISupportsValueFormatString, ISupportsSettingValueFromString
  , ISupportsIndexRangeLimiting, ISupportsFieldHandling
{
    public StringBuilder? ComplexTypeFieldWhenNonNullAddStringBuilderRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddStringBuilderRangeAs);
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
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddStringBuilderRangeAs)
              , ComplexTypeFieldWhenNonNullAddStringBuilderRangeAs
              , FromIndex, Length, ValueFormatString, FieldContentHandling)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsAnyGeneric | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldMatchWhenNonNullStringBearer<TAny> : IMoldSupportedValue<TAny?>, ISupportsValueFormatString, ISupportsFieldHandling
{
    public TAny? ComplexTypeFieldWhenNonNullAddMatch
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddMatch);
    public TAny? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddMatch
               (nameof(ComplexTypeFieldWhenNonNullAddMatch)
              , ComplexTypeFieldWhenNonNullAddMatch
              , ValueFormatString, FieldContentHandling)
           .Complete();

    public string? ValueFormatString { get; set; }

    public FieldContentHandling FieldContentHandling { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsAnyGeneric | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldObjectWhenNonNullStringBearer : IMoldSupportedValue<object?>, ISupportsValueFormatString, ISupportsFieldHandling
{
    public object? ComplexTypeFieldWhenNonNullAddObject
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddObject);
    public object? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddObject
               (nameof(ComplexTypeFieldWhenNonNullAddObject)
              , ComplexTypeFieldWhenNonNullAddObject
              , ValueFormatString, FieldContentHandling)
           .Complete();

    public string? ValueFormatString { get; set; }

    public FieldContentHandling FieldContentHandling { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}
