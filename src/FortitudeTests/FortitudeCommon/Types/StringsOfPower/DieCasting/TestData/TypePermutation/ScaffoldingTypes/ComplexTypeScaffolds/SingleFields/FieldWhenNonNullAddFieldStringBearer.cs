using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.SingleFields;

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsNullableStruct)]
public class FieldNullableBoolWhenNonNullStringBearer : IMoldSupportedValue<bool?>
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
              , ComplexTypeFieldWhenNonNullAddNullableBool)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableWhenNonNullStringBearer<TFmt> : IMoldSupportedValue<TFmt?>, ISupportsValueFormatString
    where TFmt : ISpanFormattable
{
    public TFmt? ComplexTypeFieldWhenNonNullAddSpanFormattable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddSpanFormattable);
    public TFmt? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddSpanFormattable)
              , ComplexTypeFieldWhenNonNullAddSpanFormattable
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldSpanFormattableWithHandlingWhenNonNullStringBearer<TFmt> : IMoldSupportedValue<TFmt?>, ISupportsValueFormatString
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
           .Field.WhenNonNullAddAs
               (nameof(ComplexTypeFieldWhenNonNullAddSpanFormattableAs)
              , ComplexTypeFieldWhenNonNullAddSpanFormattableAs
              , FieldContentHandling, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableWhenNonNullStringBearer<TFmtStruct> : IMoldSupportedValue<TFmtStruct?>, ISupportsValueFormatString 
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? ComplexTypeFieldWhenNonNullAddNullableSpanFormattable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddNullableSpanFormattable);
    public TFmtStruct? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddNullableSpanFormattable)
              , ComplexTypeFieldWhenNonNullAddNullableSpanFormattable
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldNullableSpanFormattableWithHandlingWhenNonNullStringBearer<TFmtStruct> : IMoldSupportedValue<TFmtStruct?>, ISupportsFieldHandling
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
           .Field.WhenNonNullAddAs
               (nameof(ComplexTypeFieldWhenNonNullAddNullableSpanFormattableAs)
              , ComplexTypeFieldWhenNonNullAddNullableSpanFormattableAs, FieldContentHandling
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerWhenNonNullStringBearer<TTCloaked, TCloakedBase>
    : IMoldSupportedValue<TTCloaked?>, ISupportsValueRevealer<TCloakedBase>
    where TTCloaked : TCloakedBase
{
    public TTCloaked? ComplexTypeFieldWhenNonNullRevealCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullRevealCloakedBearer);
    public TTCloaked? Value { get; set; }

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullReveal
               (nameof(ComplexTypeFieldWhenNonNullRevealCloakedBearer)
              , ComplexTypeFieldWhenNonNullRevealCloakedBearer
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass 
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer | SupportsCustomHandling)]
public class FieldCloakedBearerWithHandlingWhenNonNullStringBearer<TTCloaked, TCloakedBase> : IMoldSupportedValue<TTCloaked?>
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullRevealAs
               (nameof(ComplexTypeFieldWhenNonNullRevealCloakedBearerAs)
              , ComplexTypeFieldWhenNonNullRevealCloakedBearerAs
              , ValueRevealer, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerWhenNonNullStringBearer<TCloakedStruct> : IMoldSupportedValue<TCloakedStruct?>
    , ISupportsValueRevealer<TCloakedStruct>
    where TCloakedStruct : struct
{
    public TCloakedStruct? ComplexTypeFieldWhenNonNullRevealNullableCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullRevealNullableCloakedBearer);
    public FieldContentHandling FieldContentHandling { get; set; }

    public TCloakedStruct? Value { get; set; }
    
    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullReveal
               (nameof(ComplexTypeFieldWhenNonNullRevealNullableCloakedBearer)
              , ComplexTypeFieldWhenNonNullRevealNullableCloakedBearer
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer | SupportsCustomHandling)]
public class FieldNullableCloakedBearerWithHandlingWhenNonNullStringBearer<TCloakedStruct> : IMoldSupportedValue<TCloakedStruct?>
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullRevealAs
               (nameof(ComplexTypeFieldWhenNonNullRevealNullableCloakedBearerAs),
                ComplexTypeFieldWhenNonNullRevealNullableCloakedBearerAs
              , ValueRevealer, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerWhenNonNullStringBearer<TBearer> : IMoldSupportedValue<TBearer?>
    where TBearer : IStringBearer
{
    public TBearer? ComplexTypeFieldWhenNonNullRevealStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullRevealStringBearer);
    public TBearer? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullReveal
               (nameof(ComplexTypeFieldWhenNonNullRevealStringBearer)
              , ComplexTypeFieldWhenNonNullRevealStringBearer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer |
                  SupportsCustomHandling)]
public class FieldStringBearerWithHandlingWhenNonNullStringBearer<TBearer> : IMoldSupportedValue<TBearer?>, ISupportsFieldHandling
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
           .Field.WhenNonNullRevealAs
               (nameof(ComplexTypeFieldWhenNonNullRevealStringBearerAs)
              , ComplexTypeFieldWhenNonNullRevealStringBearerAs
              , FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsStruct | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerWhenNonNullStringBearer<TBearerStruct> : IMoldSupportedValue<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? ComplexTypeFieldWhenNonNullRevealNullableStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullRevealNullableStringBearer);
    public TBearerStruct? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullReveal
               (nameof(ComplexTypeFieldWhenNonNullRevealNullableStringBearer)
              , ComplexTypeFieldWhenNonNullRevealNullableStringBearer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsStruct | AcceptsNullableStruct | AcceptsStringBearer |
                  SupportsCustomHandling)]
public class FieldNullableStringBearerWithHandlingWhenNonNullStringBearer<TBearerStruct> : IMoldSupportedValue<TBearerStruct?>
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
           .Field.WhenNonNullRevealAs
               (nameof(ComplexTypeFieldWhenNonNullRevealNullableStringBearerAs)
              , ComplexTypeFieldWhenNonNullRevealNullableStringBearerAs
              , FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | CallsAsSpan | NonNullWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSpanWhenNonNullStringBearer : IMoldSupportedValue<char[]>, IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString
{
    public char[] ComplexTypeFieldAlwaysAddCharSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddCharSpan);
    public string DefaultValue { get; set; } = "";

    public char[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldAlwaysAddCharSpan)
              , ComplexTypeFieldAlwaysAddCharSpan.AsSpan()
              , ValueFormatString)
           .Complete();

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | CallsAsSpan | NonNullWrites | AcceptsChars | SupportsValueFormatString |
                  SupportsCustomHandling)]
public class FieldCharSpanWithHandlingWhenNonNullStringBearer : IMoldSupportedValue<char[]>
  , IMoldSupportedDefaultValue<string>, ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public char[] ComplexTypeFieldAlwaysAddCharSpanAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddCharSpanAs);
    public string DefaultValue { get; set; } = "";

    public char[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddAs
               (nameof(ComplexTypeFieldAlwaysAddCharSpanAs)
              , ComplexTypeFieldAlwaysAddCharSpanAs.AsSpan()
              , FieldContentHandling, ValueFormatString)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | CallsAsReadOnlySpan | NonNullWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldCharReadOnlySpanWhenNonNullStringBearer : IMoldSupportedValue<string>
  , IMoldSupportedDefaultValue<string>, ISupportsValueFormatString, ISupportsSettingValueFromString
{
    public string ComplexTypeFieldAlwaysAddReadOnlyCharSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddReadOnlyCharSpan);
    public string DefaultValue { get; set; } = "";

    public string Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldAlwaysAddReadOnlyCharSpan)
              , ComplexTypeFieldAlwaysAddReadOnlyCharSpan.AsSpan()
              , ValueFormatString)
           .Complete();

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | CallsAsReadOnlySpan | NonNullWrites | AcceptsChars | SupportsValueFormatString |
                  SupportsCustomHandling)]
public class FieldCharReadOnlySpanWithHandlingWhenNonNullStringBearer : IMoldSupportedValue<string>
  , IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public string ComplexTypeFieldAlwaysAddReadOnlyCharSpanAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddReadOnlyCharSpanAs);
    public string DefaultValue { get; set; } = "";

    public string Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAddAs
               (nameof(ComplexTypeFieldAlwaysAddReadOnlyCharSpanAs)
              , ComplexTypeFieldAlwaysAddReadOnlyCharSpanAs.AsSpan()
              , FieldContentHandling, ValueFormatString)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringWhenNonNullStringBearer : IMoldSupportedValue<string?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString
{
    public string? ComplexTypeFieldWhenNonNullAddString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddString);
    public string? Value { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddString)
              , ComplexTypeFieldWhenNonNullAddString
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldStringWithHandlingWhenNonNullStringBearer : IMoldSupportedValue<string?>, ISupportsValueFormatString
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
           .Field.WhenNonNullAddAs
               (nameof(ComplexTypeFieldWhenNonNullAddStringAs)
              , ComplexTypeFieldWhenNonNullAddStringAs
              , FieldContentHandling, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class FieldStringRangeWhenNonNullStringBearer : IMoldSupportedValue<string?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public string? ComplexTypeFieldWhenNonNullAddStringRange
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddStringRange);
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
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddStringRange)
              , ComplexTypeFieldWhenNonNullAddStringRange
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges |
                  SupportsCustomHandling)]
public class FieldStringRangeWithHandlingWhenNonNullStringBearer : IMoldSupportedValue<string?>, ISupportsValueFormatString
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
           .Field.WhenNonNullAddAs
               (nameof(ComplexTypeFieldWhenNonNullAddStringRangeAs)
              , ComplexTypeFieldWhenNonNullAddStringRangeAs
              , FromIndex, Length, FieldContentHandling, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldCharArrayWhenNonNullStringBearer : IMoldSupportedValue<char[]?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString
{
    public char[]? ComplexTypeFieldWhenNonNullAddCharArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddCharArray);
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
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddCharArray)
              , ComplexTypeFieldWhenNonNullAddCharArray
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldCharArrayWithHandlingWhenNonNullStringBearer : IMoldSupportedValue<char[]?>, ISupportsValueFormatString
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
           .Field.WhenNonNullAddAs
               (nameof(ComplexTypeFieldWhenNonNullAddCharArrayAs)
              , ComplexTypeFieldWhenNonNullAddCharArrayAs
              , FieldContentHandling, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class FieldCharArrayRangeWhenNonNullStringBearer : IMoldSupportedValue<char[]?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[]? ComplexTypeFieldWhenNonNullAddCharArrayRange
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddCharArrayRange);
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
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddCharArrayRange)
              , ComplexTypeFieldWhenNonNullAddCharArrayRange
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges |
                  SupportsCustomHandling)]
public class FieldCharArrayRangeWithHandlingWhenNonNullStringBearer : IMoldSupportedValue<char[]?>, ISupportsValueFormatString
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
           .Field.WhenNonNullAddAs
               (nameof(ComplexTypeFieldWhenNonNullAddCharArrayRangeAs)
              , ComplexTypeFieldWhenNonNullAddCharArrayRangeAs
              , FromIndex, Length, FieldContentHandling, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceWhenNonNullStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString where TCharSeq : ICharSequence
{
    public TCharSeq? ComplexTypeFieldWhenNonNullAddCharSequence
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddCharSequence);
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
               (nameof(ComplexTypeFieldWhenNonNullAddCharSequence)
              , ComplexTypeFieldWhenNonNullAddCharSequence
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldCharSequenceWithHandlingWhenNonNullStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq?>
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
           .Field.WhenNonNullAddCharSeqAs
               (nameof(ComplexTypeFieldWhenNonNullAddCharSequenceAs)
              , ComplexTypeFieldWhenNonNullAddCharSequenceAs
              , FieldContentHandling, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class FieldCharSequenceRangeWhenNonNullStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq? ComplexTypeFieldWhenNonNullAddCharSequenceRange
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddCharSequenceRange);
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
           .Field.WhenNonNullAddCharSeq
               (nameof(ComplexTypeFieldWhenNonNullAddCharSequenceRange)
              , ComplexTypeFieldWhenNonNullAddCharSequenceRange
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges |
                  SupportsCustomHandling)]
public class FieldCharSequenceRangeWithHandlingWhenNonNullStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq?>
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
           .Field.WhenNonNullAddCharSeqAs
               (nameof(ComplexTypeFieldWhenNonNullAddCharSequenceRangeAs)
              , ComplexTypeFieldWhenNonNullAddCharSequenceRangeAs
              , FromIndex, Length, FieldContentHandling, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderWhenNonNullStringBearer : IMoldSupportedValue<StringBuilder?>
  , ISupportsValueFormatString, ISupportsSettingValueFromString
{
    public StringBuilder? ComplexTypeFieldWhenNonNullAddStringBuilder
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddStringBuilder);
    public StringBuilder? Value { get; set; }

    public string? StringValue
    {
        get => Value?.ToString();
        set => Value = new StringBuilder(value);
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddStringBuilder)
              , ComplexTypeFieldWhenNonNullAddStringBuilder
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldStringBuilderWithHandlingWhenNonNullStringBearer : IMoldSupportedValue<StringBuilder?>, ISupportsValueFormatString
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
           .Field.WhenNonNullAddAs
               (nameof(ComplexTypeFieldWhenNonNullAddStringBuilderAs)
              , ComplexTypeFieldWhenNonNullAddStringBuilderAs
              , FieldContentHandling, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class FieldStringBuilderRangeWhenNonNullStringBearer : IMoldSupportedValue<StringBuilder?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public StringBuilder? ComplexTypeFieldWhenNonNullAddStringBuilderRange
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullAddStringBuilderRange);
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
           .Field.WhenNonNullAdd
               (nameof(ComplexTypeFieldWhenNonNullAddStringBuilderRange)
              , ComplexTypeFieldWhenNonNullAddStringBuilderRange
              , FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges |
                  SupportsCustomHandling)]
public class FieldStringBuilderRangeWithHandlingWhenNonNullStringBearer : IMoldSupportedValue<StringBuilder?>
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
           .Field.WhenNonNullAddAs
               (nameof(ComplexTypeFieldWhenNonNullAddStringBuilderRangeAs)
              , ComplexTypeFieldWhenNonNullAddStringBuilderRangeAs
              , FromIndex, Length, FieldContentHandling, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsAny | SupportsValueFormatString)]
public class FieldMatchWhenNonNullStringBearer<TAny> : IMoldSupportedValue<TAny?>, ISupportsValueFormatString
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
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullWrites | AcceptsAny | SupportsValueFormatString)]
public class FieldObjectWhenNonNullStringBearer : IMoldSupportedValue<object?>, ISupportsValueFormatString
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
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}
