// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.SingleFields;

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonEmptyWrites | AcceptsStruct | SupportsValueFormatString)]
public class FieldBoolWhenNonDefaultStringBearer : IMoldSupportedValue<bool>, IMoldSupportedDefaultValue<bool>, ISupportsValueFormatString
{
    public bool ComplexTypeFieldWhenNonDefaultBool
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultBool);
    public bool DefaultValue { get; set; } = false;

    public bool Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(ComplexTypeFieldWhenNonDefaultBool), ComplexTypeFieldWhenNonDefaultBool, DefaultValue, ValueFormatString)
           .Complete();
    
    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonEmptyWrites | AcceptsStruct | AcceptsClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableWhenNonDefaultStringBearer<TFmt> : IMoldSupportedValue<TFmt>, IMoldSupportedDefaultValue<TFmt>
  , ISupportsValueFormatString where TFmt : ISpanFormattable
{
    public TFmt ComplexTypeFieldWhenNonDefaultSpanFormattable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultSpanFormattable);
    public TFmt DefaultValue { get; set; } = default!;

    public TFmt Value { get; set; } = default!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(ComplexTypeFieldWhenNonDefaultSpanFormattable), ComplexTypeFieldWhenNonDefaultSpanFormattable, DefaultValue
                                  , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonEmptyWrites | AcceptsStruct | AcceptsClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldSpanFormattableWithHandlingWhenNonDefaultStringBearer<TFmt> : IMoldSupportedValue<TFmt>, IMoldSupportedDefaultValue<TFmt>
  , ISupportsValueFormatString, ISupportsFieldHandling where TFmt : ISpanFormattable
{
    public TFmt ComplexTypeFieldWhenNonDefaultSpanFormattableAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultSpanFormattableAs);
    public TFmt DefaultValue { get; set; } = default!;

    public TFmt Value { get; set; } = default!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddAs(nameof(ComplexTypeFieldWhenNonDefaultSpanFormattableAs), ComplexTypeFieldWhenNonDefaultSpanFormattableAs
                                    , DefaultValue, FieldContentHandling, ValueFormatString)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }
    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonEmptyWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableWhenNonDefaultStringBearer<TFmtStruct> : IMoldSupportedValue<TFmtStruct?>
  , IMoldSupportedDefaultValue<TFmtStruct>, ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? ComplexTypeFieldWhenNonDefaultNullableSpanFormattable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultNullableSpanFormattable);
    public TFmtStruct DefaultValue { get; set; }

    public TFmtStruct? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(ComplexTypeFieldWhenNonDefaultNullableSpanFormattable)
                                  , ComplexTypeFieldWhenNonDefaultNullableSpanFormattable, DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonEmptyWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldNullableSpanFormattableWithHandlingWhenNonDefaultStringBearer<TFmtStruct> : IMoldSupportedValue<TFmtStruct?>
  , IMoldSupportedDefaultValue<TFmtStruct>, ISupportsValueFormatString, ISupportsFieldHandling where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? ComplexTypeFieldWhenNonDefaultNullableSpanFormattableAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultNullableSpanFormattableAs);
    public TFmtStruct DefaultValue { get; set; }

    public TFmtStruct? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddAs(nameof(ComplexTypeFieldWhenNonDefaultNullableSpanFormattableAs)
                                    , ComplexTypeFieldWhenNonDefaultNullableSpanFormattableAs, DefaultValue, FieldContentHandling, ValueFormatString)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }
    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonEmptyWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                 | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerWhenNonDefaultStringBearer<TCloaked, TCloakedBase> : IMoldSupportedValue<TCloaked>
  , IMoldSupportedDefaultValue<TCloaked>, ISupportsValueRevealer<TCloakedBase>
    where TCloaked : TCloakedBase
{
    public TCloaked ComplexTypeFieldWhenNonDefaultCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCloakedBearer);
    public TCloaked DefaultValue { get; set; } = default!;

    public TCloaked Value { get; set; } = default!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultReveal(nameof(ComplexTypeFieldWhenNonDefaultCloakedBearer), ComplexTypeFieldWhenNonDefaultCloakedBearer
                                     , ValueRevealer
                                     , DefaultValue)
           .Complete();

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonEmptyWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass 
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer | SupportsCustomHandling)]
public class FieldCloakedBearerWithHandlingWhenNonDefaultStringBearer<TCloaked, TCloakedBase>
    : IMoldSupportedValue<TCloaked>, IMoldSupportedDefaultValue<TCloaked>, ISupportsValueRevealer<TCloakedBase>
      , ISupportsFieldHandling where TCloaked : TCloakedBase
{
    public TCloaked ComplexTypeFieldWhenNonDefaultCloakedBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCloakedBearerAs);
    public TCloaked DefaultValue { get; set; } = default!;

    public TCloaked Value { get; set; } = default!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field
           .WhenNonDefaultRevealAs(nameof(ComplexTypeFieldWhenNonDefaultCloakedBearerAs)
                                 , ComplexTypeFieldWhenNonDefaultCloakedBearerAs
                                 , ValueRevealer, DefaultValue, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonEmptyWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerWhenNonDefaultStringBearer<TCloakedStruct> : IMoldSupportedValue<TCloakedStruct?>
  , IMoldSupportedDefaultValue<TCloakedStruct?>, ISupportsValueRevealer<TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct? ComplexTypeFieldWhenNonDefaultCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCloakedBearer);
    public TCloakedStruct? DefaultValue { get; set; }

    public TCloakedStruct? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultReveal
               (nameof(ComplexTypeFieldWhenNonDefaultCloakedBearer), ComplexTypeFieldWhenNonDefaultCloakedBearer, ValueRevealer, DefaultValue)
           .Complete();

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonEmptyWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer | SupportsCustomHandling)]
public class FieldNullableCloakedBearerWithHandlingWhenNonDefaultStringBearer<TCloakedStruct> : IMoldSupportedValue<TCloakedStruct?>
  , IMoldSupportedDefaultValue<TCloakedStruct?>, ISupportsValueRevealer<TCloakedStruct>, ISupportsFieldHandling where TCloakedStruct : struct
{
    public TCloakedStruct? ComplexTypeFieldWhenNonDefaultCloakedBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCloakedBearerAs);
    public TCloakedStruct? DefaultValue { get; set; }

    public TCloakedStruct? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultRevealAs(nameof(ComplexTypeFieldWhenNonDefaultCloakedBearerAs), ComplexTypeFieldWhenNonDefaultCloakedBearerAs
                                       , ValueRevealer, DefaultValue, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonEmptyWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerWhenNonDefaultStringBearer<TBearer> : IMoldSupportedValue<TBearer?>, IMoldSupportedDefaultValue<TBearer?>
    where TBearer : IStringBearer
{
    public TBearer? ComplexTypeFieldWhenNonDefaultStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultStringBearer);
    public TBearer? DefaultValue { get; set; }

    public TBearer? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultReveal(nameof(ComplexTypeFieldWhenNonDefaultStringBearer), ComplexTypeFieldWhenNonDefaultStringBearer, DefaultValue)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonEmptyWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer |
                  SupportsCustomHandling)]
public class FieldStringBearerWithHandlingWhenNonDefaultStringBearer<TBearer> : IMoldSupportedValue<TBearer?>
  , IMoldSupportedDefaultValue<TBearer?>, ISupportsFieldHandling
    where TBearer : IStringBearer
{
    public TBearer? ComplexTypeFieldWhenNonDefaultStringBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultStringBearerAs);
    public TBearer? DefaultValue { get; set; }

    public TBearer? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultRevealAs(nameof(ComplexTypeFieldWhenNonDefaultStringBearerAs), ComplexTypeFieldWhenNonDefaultStringBearerAs
                                       , DefaultValue
                                       , FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonEmptyWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerWhenNonDefaultStringBearer<TBearerStruct> : IMoldSupportedValue<TBearerStruct?>
  , IMoldSupportedDefaultValue<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? ComplexTypeFieldWhenNonDefaultStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultStringBearer);
    public TBearerStruct? DefaultValue { get; set; }

    public TBearerStruct? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultReveal(nameof(ComplexTypeFieldWhenNonDefaultStringBearer), ComplexTypeFieldWhenNonDefaultStringBearer, DefaultValue)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonEmptyWrites | AcceptsNullableStruct | AcceptsStringBearer | SupportsCustomHandling)]
public class FieldNullableStringBearerWithHandlingWhenNonDefaultStringBearer<TBearerStruct> : IMoldSupportedValue<TBearerStruct?>
  , IMoldSupportedDefaultValue<TBearerStruct?>, ISupportsFieldHandling where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? ComplexTypeFieldWhenNonDefaultStringBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultStringBearerAs);
    public TBearerStruct? DefaultValue { get; set; }

    public TBearerStruct? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultRevealAs(nameof(ComplexTypeFieldWhenNonDefaultStringBearerAs), ComplexTypeFieldWhenNonDefaultStringBearerAs
                                       , DefaultValue
                                       , FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | CallsAsSpan | NonEmptyWrites | AcceptsCharArray | SupportsValueFormatString)]
public class FieldCharSpanWhenNonDefaultStringBearer : IMoldSupportedValue<char[]>, IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString
{
    public char[] ComplexTypeFieldWhenNonDefaultCharSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCharSpan);
    public string DefaultValue { get; set; } = "";

    public char[] Value { get; set; } = null!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(ComplexTypeFieldWhenNonDefaultCharSpan), ComplexTypeFieldWhenNonDefaultCharSpan.AsSpan(), DefaultValue
                                  , ValueFormatString)
           .Complete();

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }
    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | CallsAsSpan | NonEmptyWrites | AcceptsCharArray | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldCharSpanWithHandlingWhenNonDefaultStringBearer : IMoldSupportedValue<char[]>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public char[] ComplexTypeFieldWhenNonDefaultCharSpanAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCharSpanAs);
    public string DefaultValue { get; set; } = "";

    public char[] Value { get; set; } = null!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddAs(nameof(ComplexTypeFieldWhenNonDefaultCharSpanAs), ComplexTypeFieldWhenNonDefaultCharSpanAs.AsSpan()
                                    , DefaultValue, FieldContentHandling, ValueFormatString)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | CallsAsReadOnlySpan | NonEmptyWrites | AcceptsString | SupportsValueFormatString)]
public class FieldCharReadOnlySpanWhenNonDefaultStringBearer : IMoldSupportedValue<string>, IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString
{
    public string ComplexTypeFieldWhenNonDefaultCharReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCharReadOnlySpan);
    public string DefaultValue { get; set; } = "";

    public string Value { get; set; } = null!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(ComplexTypeFieldWhenNonDefaultCharReadOnlySpan), ComplexTypeFieldWhenNonDefaultCharReadOnlySpan.AsSpan()
                                  , DefaultValue, ValueFormatString)
           .Complete();

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | CallsAsReadOnlySpan | NonEmptyWrites | AcceptsString | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldCharReadOnlySpanWithHandlingWhenNonDefaultStringBearer : IMoldSupportedValue<string>
  , IMoldSupportedDefaultValue<string>, ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public string ComplexTypeFieldWhenNonDefaultCharReadOnlySpanAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCharReadOnlySpanAs);
    public string? ValueFormatString { get; set; }

    public string DefaultValue { get; set; } = "";

    public string Value { get; set; } = null!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddAs(nameof(ComplexTypeFieldWhenNonDefaultCharReadOnlySpanAs)
                                    , ComplexTypeFieldWhenNonDefaultCharReadOnlySpanAs.AsSpan()
                                    , DefaultValue, FieldContentHandling, ValueFormatString)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonEmptyWrites | AcceptsString | SupportsValueFormatString)]
public class FieldStringWhenNonDefaultStringBearer : IMoldSupportedValue<string>, IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString
{
    public string ComplexTypeFieldWhenNonDefaultString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultString);
    public string DefaultValue { get; set; } = "";

    public string Value { get; set; } = null!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(ComplexTypeFieldWhenNonDefaultString), ComplexTypeFieldWhenNonDefaultString, DefaultValue, ValueFormatString)
           .Complete();

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }
    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonEmptyWrites | AcceptsString | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldStringWithHandlingWhenNonDefaultStringBearer : IMoldSupportedValue<string>, IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsFieldHandling, ISupportsSettingValueFromString
{
    public string ComplexTypeFieldWhenNonDefaultStringAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultStringAs);
    public string DefaultValue { get; set; } = "";

    public string Value { get; set; } = null!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddAs(nameof(ComplexTypeFieldWhenNonDefaultStringAs), ComplexTypeFieldWhenNonDefaultStringAs, DefaultValue
                                    , FieldContentHandling
                                    , ValueFormatString)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }
    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonEmptyWrites | AcceptsString | SupportsValueFormatString | SupportsIndexSubRanges)]
public class FieldStringRangeWhenNonDefaultStringBearer : IMoldSupportedValue<string>, IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public string ComplexTypeFieldWhenNonDefaultStringRange
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultStringRange);
    public string DefaultValue { get; set; } = "";

    public string Value { get; set; } = null!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(ComplexTypeFieldWhenNonDefaultStringRange), ComplexTypeFieldWhenNonDefaultStringRange
                                  , FromIndex, Length, DefaultValue, ValueFormatString)
           .Complete();

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }
    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonEmptyWrites | AcceptsString | SupportsValueFormatString | SupportsIndexSubRanges |
                  SupportsCustomHandling)]
public class FieldStringRangeWithHandlingWhenNonDefaultStringBearer : IMoldSupportedValue<string>, IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling
{
    public string ComplexTypeFieldWhenNonDefaultStringRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultStringRangeAs);
    public string DefaultValue { get; set; } = "";

    public string Value { get; set; } = null!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddAs(nameof(ComplexTypeFieldWhenNonDefaultStringRangeAs), ComplexTypeFieldWhenNonDefaultStringRangeAs
                                    , FromIndex, Length, DefaultValue, FieldContentHandling, ValueFormatString)
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
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonEmptyWrites | AcceptsCharArray | SupportsValueFormatString)]
public class FieldCharArrayWhenNonDefaultStringBearer : IMoldSupportedValue<char[]>, IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString
{
    public char[] ComplexTypeFieldWhenNonDefaultCharArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCharArray);
    public string DefaultValue { get; set; } = "";

    public char[] Value { get; set; } = null!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(ComplexTypeFieldWhenNonDefaultCharArray), ComplexTypeFieldWhenNonDefaultCharArray, DefaultValue
                                  , ValueFormatString)
           .Complete();

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray()!;
    }
    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonEmptyWrites | AcceptsCharArray | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldCharArrayWithHandlingWhenNonDefaultStringBearer : IMoldSupportedValue<char[]>, IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public char[] ComplexTypeFieldWhenNonDefaultCharArrayAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCharArrayAs);
    public string DefaultValue { get; set; } = "";

    public char[] Value { get; set; } = null!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddAs(nameof(ComplexTypeFieldWhenNonDefaultCharArrayAs), ComplexTypeFieldWhenNonDefaultCharArrayAs
                                    , DefaultValue, FieldContentHandling, ValueFormatString)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray()!;
    }
    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonEmptyWrites | AcceptsCharArray | SupportsValueFormatString | SupportsIndexSubRanges)]
public class FieldCharArrayRangeWhenNonDefaultStringBearer : IMoldSupportedValue<char[]>, IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[] ComplexTypeFieldWhenNonDefaultCharArrayRange
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCharArrayRange);
    public string DefaultValue { get; set; } = "";

    public char[] Value { get; set; } = null!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(ComplexTypeFieldWhenNonDefaultCharArrayRange), ComplexTypeFieldWhenNonDefaultCharArrayRange
                                  , FromIndex, Length, DefaultValue, ValueFormatString)
           .Complete();

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray()!;
    }
    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonEmptyWrites | AcceptsCharArray | SupportsValueFormatString | SupportsIndexSubRanges |
                  SupportsCustomHandling)]
public class FieldCharArrayRangeWithHandlingWhenNonDefaultStringBearer : IMoldSupportedValue<char[]>, IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling
{
    public char[] ComplexTypeFieldWhenNonDefaultCharArrayRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCharArrayRangeAs);
    public string DefaultValue { get; set; } = "";

    public char[] Value { get; set; } = null!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddAs(nameof(ComplexTypeFieldWhenNonDefaultCharArrayRangeAs), ComplexTypeFieldWhenNonDefaultCharArrayRangeAs
                                    , FromIndex, Length, DefaultValue, FieldContentHandling, ValueFormatString)
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
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonEmptyWrites | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceWhenNonDefaultStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq>, IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString where TCharSeq : ICharSequence
{
    public TCharSeq ComplexTypeFieldWhenNonDefaultCharSequence
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCharSequence);
    public string DefaultValue { get; set; } = "";

    public TCharSeq Value { get; set; } = default!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddCharSeq(nameof(ComplexTypeFieldWhenNonDefaultCharSequence), ComplexTypeFieldWhenNonDefaultCharSequence
                                         , DefaultValue, ValueFormatString)
           .Complete();

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
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonEmptyWrites | AcceptsCharSequence | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldCharSequenceWithHandlingWhenNonDefaultStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq>
  , IMoldSupportedDefaultValue<string>, ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsFieldHandling
    where TCharSeq : ICharSequence
{
    public TCharSeq ComplexTypeFieldWhenNonDefaultCharSequenceAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCharSequenceAs);
    public string DefaultValue { get; set; } = "";

    public TCharSeq Value { get; set; } = default!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddCharSeqAs(nameof(ComplexTypeFieldWhenNonDefaultCharSequenceAs), ComplexTypeFieldWhenNonDefaultCharSequenceAs
                                           , DefaultValue, FieldContentHandling, ValueFormatString)
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
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonEmptyWrites | AcceptsCharSequence | SupportsValueFormatString | SupportsIndexSubRanges)]
public class FieldCharSequenceRangeWhenNonDefaultStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq>, IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq ComplexTypeFieldWhenNonDefaultCharSequenceRange
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCharSequenceRange);
    public string DefaultValue { get; set; } = "";

    public TCharSeq Value { get; set; } = default!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddCharSeq(nameof(ComplexTypeFieldWhenNonDefaultCharSequenceRange), ComplexTypeFieldWhenNonDefaultCharSequenceRange
                                         , FromIndex, Length, DefaultValue, ValueFormatString)
           .Complete();

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
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonEmptyWrites | AcceptsCharSequence | SupportsValueFormatString | SupportsIndexSubRanges |
                  SupportsCustomHandling)]
public class FieldCharSequenceRangeWithHandlingWhenNonDefaultStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq>
  , IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling
    where TCharSeq : ICharSequence
{
    public TCharSeq ComplexTypeFieldWhenNonDefaultCharSequenceRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultCharSequenceRangeAs);
    public string DefaultValue { get; set; } = "";

    public TCharSeq Value { get; set; } = default!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddCharSeqAs(nameof(ComplexTypeFieldWhenNonDefaultCharSequenceRangeAs)
                                           , ComplexTypeFieldWhenNonDefaultCharSequenceRangeAs
                                           , FromIndex, Length, DefaultValue, FieldContentHandling, ValueFormatString)
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
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonEmptyWrites | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderWhenNonDefaultStringBearer : IMoldSupportedValue<StringBuilder>, IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString
{
    public StringBuilder ComplexTypeFieldWhenNonDefaultAddStringBuilder
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultAddStringBuilder);
    public string DefaultValue { get; set; } = "";

    public StringBuilder Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(ComplexTypeFieldWhenNonDefaultAddStringBuilder), ComplexTypeFieldWhenNonDefaultAddStringBuilder
                                  , DefaultValue, ValueFormatString)
           .Complete();

    public string? StringValue
    {
        get => Value.ToString();
        set => Value = new StringBuilder(value);
    }
    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonEmptyWrites | AcceptsStringBuilder | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldStringBuilderWithHandlingWhenNonDefaultStringBearer : IMoldSupportedValue<StringBuilder>
  , IMoldSupportedDefaultValue<string>,
    ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public StringBuilder ComplexTypeFieldWhenNonDefaultAddStringBuilderAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultAddStringBuilderAs);
    public string DefaultValue { get; set; } = "";

    public StringBuilder Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddAs(nameof(ComplexTypeFieldWhenNonDefaultAddStringBuilderAs), ComplexTypeFieldWhenNonDefaultAddStringBuilderAs
                                    , DefaultValue, FieldContentHandling, ValueFormatString)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value.ToString();
        set => Value = new StringBuilder(value);
    }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonEmptyWrites | AcceptsStringBuilder | SupportsValueFormatString | SupportsIndexSubRanges)]
public class FieldStringBuilderRangeWhenNonDefaultStringBearer : IMoldSupportedValue<StringBuilder>, IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public StringBuilder ComplexTypeFieldWhenNonDefaultAddStringBuilderRange
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultAddStringBuilderRange);
    public string DefaultValue { get; set; } = "";

    public StringBuilder Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAdd(nameof(ComplexTypeFieldWhenNonDefaultAddStringBuilderRange), ComplexTypeFieldWhenNonDefaultAddStringBuilderRange
                                  , FromIndex, Length, DefaultValue, ValueFormatString)
           .Complete();

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value.ToString();
        set => Value = new StringBuilder(value);
    }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonEmptyWrites | AcceptsStringBuilder | SupportsValueFormatString | SupportsIndexSubRanges |
                  SupportsCustomHandling)]
public class FieldStringBuilderRangeWithHandlingWhenNonDefaultStringBearer : IMoldSupportedValue<StringBuilder>
  , IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsFieldHandling, ISupportsIndexRangeLimiting
{
    public StringBuilder ComplexTypeFieldWhenNonDefaultAddStringBuilderRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultAddStringBuilderRangeAs);
    public string DefaultValue { get; set; } = "";

    public StringBuilder Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddAs(nameof(ComplexTypeFieldWhenNonDefaultAddStringBuilderRangeAs)
                                    , ComplexTypeFieldWhenNonDefaultAddStringBuilderRangeAs
                                    , FromIndex, Length, DefaultValue, FieldContentHandling, ValueFormatString)
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
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonEmptyWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchWhenNonDefaultStringBearer<TAny> : IMoldSupportedValue<TAny?>, IMoldSupportedDefaultValue<TAny?>, ISupportsValueFormatString
{
    public TAny? ComplexTypeFieldWhenNonDefaultMatch
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultMatch);
    public TAny? DefaultValue { get; set; }

    public TAny? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddMatch(nameof(ComplexTypeFieldWhenNonDefaultMatch), ComplexTypeFieldWhenNonDefaultMatch, DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonEmptyWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldObjectWhenNonDefaultStringBearer : IMoldSupportedValue<object?>, IMoldSupportedDefaultValue<object?>
  , ISupportsValueFormatString
{
    public object? ComplexTypeFieldWhenNonDefaultObject
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldWhenNonDefaultObject);
    public object? DefaultValue { get; set; }

    public object? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonDefaultAddObject(nameof(ComplexTypeFieldWhenNonDefaultObject), ComplexTypeFieldWhenNonDefaultObject, DefaultValue
                                        , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}
