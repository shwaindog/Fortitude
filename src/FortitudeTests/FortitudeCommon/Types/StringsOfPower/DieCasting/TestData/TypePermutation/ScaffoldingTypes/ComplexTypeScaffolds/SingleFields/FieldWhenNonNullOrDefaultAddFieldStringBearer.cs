// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.SingleFields;

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsNullableStruct | SupportsValueFormatString)]
public class FieldNullableBoolWhenNonNullOrDefaultStringBearer : IMoldSupportedValue<bool?>, IMoldSupportedDefaultValue<bool?>
  , ISupportsValueFormatString
{
    public bool? ComplexTypeFieldWhenNonNullOrDefaultBool
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultBool);
    public bool? DefaultValue { get; set; } = false;

    public bool? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultBool)
              , ComplexTypeFieldWhenNonNullOrDefaultBool
              , DefaultValue.IfNullableGetNonNullableUnderlyingDefault(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass |
                  AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString |
                  SupportsCustomHandling)]
public class FieldSpanFormattableWhenNonNullOrDefaultStringBearer<TFmt> : IMoldSupportedValue<TFmt?>
  , IMoldSupportedDefaultValue<TFmt?>, ISupportsValueFormatString, ISupportsFieldHandling where TFmt : ISpanFormattable
{
    public TFmt? ComplexTypeFieldWhenNonNullOrDefaultSpanFormattableAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultSpanFormattableAs);
    public TFmt? DefaultValue { get; set; }

    public TFmt? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultSpanFormattableAs)
              , ComplexTypeFieldWhenNonNullOrDefaultSpanFormattableAs
              , DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldNullableSpanFormattableWhenNonNullOrDefaultStringBearer<TFmtStruct> : IMoldSupportedValue<TFmtStruct?>
  , IMoldSupportedDefaultValue<TFmtStruct?>, ISupportsValueFormatString, ISupportsFieldHandling
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? ComplexTypeFieldWhenNonNullOrDefaultNullableSpanFormattableAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultNullableSpanFormattableAs);
    public TFmtStruct? DefaultValue { get; set; } = default(TFmtStruct);

    public TFmtStruct? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultNullableSpanFormattableAs)
              , ComplexTypeFieldWhenNonNullOrDefaultNullableSpanFormattableAs
              , DefaultValue.IfNullableGetNonNullableUnderlyingDefault(), ValueFormatString, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }
    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer | SupportsCustomHandling)]
public class FieldCloakedBearerWhenNonNullOrDefaultStringBearer<TCloaked, TCloakedBase> : IMoldSupportedValue<TCloaked?>
  , IMoldSupportedDefaultValue<TCloaked>, ISupportsValueRevealer<TCloakedBase>, ISupportsFieldHandling
    where TCloaked : TCloakedBase
{
    public TCloaked? ComplexTypeFieldWhenNonNullOrDefaultCloakedBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultCloakedBearerAs);
    public TCloaked DefaultValue { get; set; } = default!;

    public TCloaked? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultReveal
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultCloakedBearerAs)
              , ComplexTypeFieldWhenNonNullOrDefaultCloakedBearerAs
              , ValueRevealer, DefaultValue, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedBase>)value;
    }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer | SupportsCustomHandling)]
public class FieldNullableCloakedBearerWhenNonNullOrDefaultStringBearer<TCloakedStruct> : IMoldSupportedValue<TCloakedStruct?>
  , IMoldSupportedDefaultValue<TCloakedStruct?>, ISupportsValueRevealer<TCloakedStruct>, ISupportsFieldHandling
    where TCloakedStruct : struct
{
    public TCloakedStruct? ComplexTypeFieldWhenNonNullOrDefaultCustomBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultCustomBearerAs);
    public TCloakedStruct? DefaultValue { get; set; }

    public TCloakedStruct? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultReveal
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultCustomBearerAs)
              , ComplexTypeFieldWhenNonNullOrDefaultCustomBearerAs
              , ValueRevealer, DefaultValue, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedStruct>)value;
    }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsStringBearer | SupportsCustomHandling)]
public class FieldStringBearerWhenNonNullOrDefaultStringBearer<TBearer> : IMoldSupportedValue<TBearer?>
  , IMoldSupportedDefaultValue<TBearer?>, ISupportsFieldHandling where TBearer : IStringBearer
{
    public TBearer? ComplexTypeFieldWhenNonNullOrDefaultStringBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultStringBearerAs);
    public TBearer? DefaultValue { get; set; }

    public TBearer? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultReveal
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultStringBearerAs)
              , ComplexTypeFieldWhenNonNullOrDefaultStringBearerAs
              , DefaultValue, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsNullableStruct | AcceptsStringBearer |
                  SupportsCustomHandling)]
public class FieldNullableStringBearerWhenNonNullOrDefaultStringBearer<TBearerStruct> : IMoldSupportedValue<TBearerStruct?>
  , IMoldSupportedDefaultValue<TBearerStruct?>, ISupportsFieldHandling where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? ComplexTypeFieldWhenNonNullOrDefaultNullableStringBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultNullableStringBearerAs);
    public TBearerStruct? DefaultValue { get; set; }

    public TBearerStruct? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultReveal
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultNullableStringBearerAs)
              , ComplexTypeFieldWhenNonNullOrDefaultNullableStringBearerAs
              , DefaultValue, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsCharArray | SupportsValueFormatString |
                  SupportsCustomHandling)]
public class FieldCharSpanWhenNonNullOrDefaultStringBearer : IMoldSupportedValue<char[]>
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
           .Field.WhenNonNullOrDefaultAdd
               (nameof(ComplexTypeFieldAlwaysAddCharSpanAs)
              , ComplexTypeFieldAlwaysAddCharSpanAs.AsSpan()
              , DefaultValue, ValueFormatString, FieldContentHandling)
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

[TypeGeneratePart(ComplexType | SingleValueCardinality | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsString | SupportsValueFormatString |
                  SupportsCustomHandling)]
public class FieldCharReadOnlySpanWhenNonNullOrDefaultStringBearer : IMoldSupportedValue<string>
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
           .Field.WhenNonNullOrDefaultAdd
               (nameof(ComplexTypeFieldAlwaysAddReadOnlyCharSpanAs)
              , ComplexTypeFieldAlwaysAddReadOnlyCharSpanAs.AsSpan()
              , DefaultValue, ValueFormatString, FieldContentHandling)
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

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsString | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldStringWhenNonNullOrDefaultStringBearer : IMoldSupportedValue<string?>, IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public string? ComplexTypeFieldWhenNonNullOrDefaultStringAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultStringAs);
    public string DefaultValue { get; set; } = "";

    public string? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultStringAs)
              , ComplexTypeFieldWhenNonNullOrDefaultStringAs
              , DefaultValue, ValueFormatString, FieldContentHandling)
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

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsString | SupportsValueFormatString | SupportsIndexSubRanges |
                  SupportsCustomHandling)]
public class FieldStringRangeWhenNonNullOrDefaultStringBearer : IMoldSupportedValue<string?>
  , IMoldSupportedDefaultValue<string>, ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
  , ISupportsFieldHandling
{
    public string? ComplexTypeFieldWhenNonNullOrDefaultStringRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultStringRangeAs);
    public string DefaultValue { get; set; } = "";

    public string? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultStringRangeAs)
              , ComplexTypeFieldWhenNonNullOrDefaultStringRangeAs
              , FromIndex, Length, DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }
    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsCharArray | SupportsValueFormatString 
    | SupportsSettingDefaultValue | SupportsCustomHandling)]
public class FieldCharArrayWhenNonNullOrDefaultStringBearer : IMoldSupportedValue<char[]>, IMoldSupportedDefaultValue<string>
, ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public char[] ComplexTypeFieldWhenNonNullOrDefaultCharArrayAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultCharArrayAs);
    public string DefaultValue { get; set; } = "";

    public char[] Value { get; set; } = null!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultCharArrayAs)
              , ComplexTypeFieldWhenNonNullOrDefaultCharArrayAs
              , DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray()!;
    }
    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsCharArray | SupportsValueFormatString |
                  SupportsIndexSubRanges | SupportsCustomHandling)]
public class FieldCharArrayRangeWhenNonNullOrDefaultStringBearer : IMoldSupportedValue<char[]>
  , IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling
{
    public char[] ComplexTypeFieldWhenNonNullOrDefaultCharArrayRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultCharArrayRangeAs);
    public string DefaultValue { get; set; } = "";

    public char[] Value { get; set; } = null!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultCharArrayRangeAs)
              , ComplexTypeFieldWhenNonNullOrDefaultCharArrayRangeAs
              , FromIndex, Length, DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray()!;
    }
    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsCharSequence | SupportsValueFormatString |
                  SupportsCustomHandling)]
public class FieldCharSequenceWhenNonNullOrDefaultStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq>
  , IMoldSupportedDefaultValue<string>, ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsFieldHandling
    where TCharSeq : ICharSequence
{
    public TCharSeq ComplexTypeFieldWhenNonNullOrDefaultCharSequenceAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultCharSequenceAs);
    public string DefaultValue { get; set; } = "";

    public TCharSeq Value { get; set; } = default!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddCharSeq
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultCharSequenceAs)
              , ComplexTypeFieldWhenNonNullOrDefaultCharSequenceAs
              , DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value != null! ? Value.ToString() : null;
        set
        {
            var typeOfCharSeq = typeof(TCharSeq);

            if (typeOfCharSeq == typeof(CharArrayStringBuilder))
                Value = (TCharSeq)(object)new CharArrayStringBuilder(value);
            else
                Value = (TCharSeq)(object)new MutableString(value);
        }
    }

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsCharSequence | SupportsValueFormatString |
                  SupportsIndexSubRanges | SupportsCustomHandling)]
public class FieldCharSequenceRangeWhenNonNullOrDefaultStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq>
  , IMoldSupportedDefaultValue<string>, ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
  , ISupportsFieldHandling where TCharSeq : ICharSequence
{
    public TCharSeq ComplexTypeFieldWhenNonNullOrDefaultCharSequenceRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultCharSequenceRangeAs);
    public string DefaultValue { get; set; } = "";

    public TCharSeq Value { get; set; } = default!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddCharSeq
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultCharSequenceRangeAs)
              , ComplexTypeFieldWhenNonNullOrDefaultCharSequenceRangeAs
              , FromIndex, Length, DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null! ? Value.ToString() : null;
        set
        {
            var typeOfCharSeq = typeof(TCharSeq);

            if (typeOfCharSeq == typeof(CharArrayStringBuilder))
                Value = (TCharSeq)(object)new CharArrayStringBuilder(value);
            else
                Value = (TCharSeq)(object)new MutableString(value);
        }
    }
    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsStringBuilder | SupportsValueFormatString |
                  SupportsCustomHandling)]
public class FieldStringBuilderWhenNonNullOrDefaultStringBearer : IMoldSupportedValue<StringBuilder>
  , IMoldSupportedDefaultValue<string>, ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public StringBuilder ComplexTypeFieldWhenNonNullOrDefaultAddStringBuilderAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultAddStringBuilderAs);
    public string DefaultValue { get; set; } = "";

    public StringBuilder Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultAddStringBuilderAs)
              , ComplexTypeFieldWhenNonNullOrDefaultAddStringBuilderAs
              , DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value.ToString();
        set => Value = new StringBuilder(value);
    }

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsStringBuilder | SupportsValueFormatString |
                  SupportsIndexSubRanges | SupportsCustomHandling)]
public class FieldStringBuilderRangeWhenNonNullOrDefaultStringBearer : IMoldSupportedValue<StringBuilder>
  , IMoldSupportedDefaultValue<string>, ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsFieldHandling
  , ISupportsIndexRangeLimiting
{
    public StringBuilder ComplexTypeFieldWhenNonNullOrDefaultAddStringBuilderRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultAddStringBuilderRangeAs);
    public string DefaultValue { get; set; } = "";

    public StringBuilder Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultAddStringBuilderRangeAs)
              , ComplexTypeFieldWhenNonNullOrDefaultAddStringBuilderRangeAs
              , FromIndex, Length, DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value.ToString();
        set => Value = new StringBuilder(value);
    }
    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsAnyGeneric | SupportsValueFormatString 
                | SupportsCustomHandling)]
public class FieldMatchWhenNonNullOrDefaultStringBearer<TAny> : IMoldSupportedValue<TAny?>
  , IMoldSupportedDefaultValue<TAny?>, ISupportsValueFormatString, ISupportsFieldHandling
{
    public TAny? ComplexTypeFieldWhenNonNullOrDefaultMatch
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultMatch);
    public TAny? DefaultValue { get; set; }

    public TAny? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddMatch
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultMatch)
              , ComplexTypeFieldWhenNonNullOrDefaultMatch
              , DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public string? ValueFormatString { get; set; }

    public FieldContentHandling FieldContentHandling { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | SingleValueCardinality | NonNullAndPopulatedWrites | AcceptsAnyGeneric | SupportsValueFormatString 
                | SupportsCustomHandling)]
public class FieldObjectWhenNonNullOrDefaultStringBearer : IMoldSupportedValue<object?>, IMoldSupportedDefaultValue<object?>
  , ISupportsValueFormatString, ISupportsFieldHandling
{
    public object? ComplexTypeFieldWhenNonNullOrDefaultObject
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonNullOrDefaultObject);
    public object? DefaultValue { get; set; }

    public object? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddObject
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultObject)
              , ComplexTypeFieldWhenNonNullOrDefaultObject
              , DefaultValue, ValueFormatString, FieldContentHandling)
           .Complete();

    public string? ValueFormatString { get; set; }

    public FieldContentHandling FieldContentHandling { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}
