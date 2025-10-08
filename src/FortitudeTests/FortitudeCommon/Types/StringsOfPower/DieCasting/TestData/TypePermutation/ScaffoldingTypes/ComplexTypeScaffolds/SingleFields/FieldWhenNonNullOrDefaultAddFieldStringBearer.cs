// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.SingleFields;

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullAndPopulatedWrites | AcceptsStruct)]
public class FieldNullableBoolWhenNonNullOrDefaultStringBearer : IStringBearer, IMoldSupportedValue<bool?>, IMoldSupportedDefaultValue<bool>
{
    public bool? ComplexTypeFieldWhenNonNullOrDefaultBool
    {
        get => Value;
        set => Value = value;
    }

    public bool DefaultValue { get; set; } = false;

    public bool? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd(nameof(ComplexTypeFieldWhenNonNullOrDefaultBool), ComplexTypeFieldWhenNonNullOrDefaultBool, DefaultValue)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullAndPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass |
                  AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableWhenNonNullOrDefaultStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt?>
  , IMoldSupportedDefaultValue<TFmt?>, ISupportsValueFormatString where TFmt : ISpanFormattable
{
    public TFmt? ComplexTypeFieldWhenNonNullOrDefaultSpanFormattable
    {
        get => Value;
        set => Value = value;
    }

    public TFmt? DefaultValue { get; set; }

    public TFmt? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd(nameof(ComplexTypeFieldWhenNonNullOrDefaultSpanFormattable)
                                        , ComplexTypeFieldWhenNonNullOrDefaultSpanFormattable, DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullAndPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass |
                  AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString |
                  SupportsCustomHandling)]
public class FieldSpanFormattableWithHandlingWhenNonNullOrDefaultStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt?>
  , IMoldSupportedDefaultValue<TFmt?>, ISupportsValueFormatString, ISupportsFieldHandling where TFmt : ISpanFormattable
{
    public TFmt? ComplexTypeFieldWhenNonNullOrDefaultSpanFormattableAs
    {
        get => Value;
        set => Value = value;
    }

    public TFmt? DefaultValue { get; set; }

    public TFmt? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddAs
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultSpanFormattableAs)
              , ComplexTypeFieldWhenNonNullOrDefaultSpanFormattableAs
              , DefaultValue, FieldContentHandling, ValueFormatString)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullAndPopulatedWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableWhenNonNullOrDefaultStringBearer<TFmtStruct> : IStringBearer, IMoldSupportedValue<TFmtStruct?>
  , IMoldSupportedDefaultValue<TFmtStruct>, ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? ComplexTypeFieldWhenNonNullOrDefaultNullableSpanFormattable
    {
        get => Value;
        set => Value = value;
    }

    public TFmtStruct DefaultValue { get; set; }

    public TFmtStruct? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultNullableSpanFormattable)
              , ComplexTypeFieldWhenNonNullOrDefaultNullableSpanFormattable
              , DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullAndPopulatedWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldNullableSpanFormattableWithHandlingWhenNonNullOrDefaultStringBearer<TFmtStruct> : IStringBearer, IMoldSupportedValue<TFmtStruct?>
  , IMoldSupportedDefaultValue<TFmtStruct>, ISupportsValueFormatString, ISupportsFieldHandling
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? ComplexTypeFieldWhenNonNullOrDefaultNullableSpanFormattableAs
    {
        get => Value;
        set => Value = value;
    }

    public TFmtStruct DefaultValue { get; set; }

    public TFmtStruct? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddAs
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultNullableSpanFormattableAs)
              , ComplexTypeFieldWhenNonNullOrDefaultNullableSpanFormattableAs
              , DefaultValue, FieldContentHandling, ValueFormatString)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }
    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullAndPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsChars | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerWhenNonNullOrDefaultStringBearer<TCloaked, TCloakedBase> : IStringBearer, IMoldSupportedValue<TCloaked?>
  , IMoldSupportedDefaultValue<TCloaked>, ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked? ComplexTypeFieldWhenNonNullOrDefaultCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public TCloaked DefaultValue { get; set; } = default!;

    public TCloaked? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultReveal
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultCloakedBearer)
              , ComplexTypeFieldWhenNonNullOrDefaultCloakedBearer
              , ValueRevealer, DefaultValue)
           .Complete();

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullAndPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsChars
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer | SupportsCustomHandling)]
public class FieldCloakedBearerWithHandlingWhenNonNullOrDefaultStringBearer<TCloaked, TCloakedBase> : IStringBearer, IMoldSupportedValue<TCloaked?>
  , IMoldSupportedDefaultValue<TCloaked>, ISupportsValueRevealer<TCloakedBase>, ISupportsFieldHandling
    where TCloaked : TCloakedBase
{
    public TCloaked? ComplexTypeFieldWhenNonNullOrDefaultCloakedBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public TCloaked DefaultValue { get; set; } = default!;

    public TCloaked? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultRevealAs
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultCloakedBearerAs)
              , ComplexTypeFieldWhenNonNullOrDefaultCloakedBearerAs
              , ValueRevealer, DefaultValue, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullAndPopulatedWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerWhenNonNullOrDefaultStringBearer<TCloakedStruct> : IStringBearer, IMoldSupportedValue<TCloakedStruct?>
  , IMoldSupportedDefaultValue<TCloakedStruct?>, ISupportsValueRevealer<TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct? ComplexTypeFieldWhenNonNullOrDefaultCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public TCloakedStruct? DefaultValue { get; set; }

    public TCloakedStruct? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultReveal(nameof(ComplexTypeFieldWhenNonNullOrDefaultCloakedBearer)
                                           , ComplexTypeFieldWhenNonNullOrDefaultCloakedBearer, ValueRevealer, DefaultValue)
           .Complete();

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullAndPopulatedWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer | SupportsCustomHandling)]
public class FieldNullableCloakedBearerWithHandlingWhenNonNullOrDefaultStringBearer<TCloakedStruct> : IStringBearer
  , IMoldSupportedValue<TCloakedStruct?>, IMoldSupportedDefaultValue<TCloakedStruct?>, ISupportsValueRevealer<TCloakedStruct>, ISupportsFieldHandling
    where TCloakedStruct : struct
{
    public TCloakedStruct? ComplexTypeFieldWhenNonNullOrDefaultCustomBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public TCloakedStruct? DefaultValue { get; set; }

    public TCloakedStruct? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultRevealAs
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultCustomBearerAs)
              , ComplexTypeFieldWhenNonNullOrDefaultCustomBearerAs
              , ValueRevealer, DefaultValue, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullAndPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass |
                  AcceptsStringBearer)]
public class FieldStringBearerWhenNonNullOrDefaultStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer?>
  , IMoldSupportedDefaultValue<TBearer?>
    where TBearer : IStringBearer
{
    public TBearer? ComplexTypeFieldWhenNonNullOrDefaultStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public TBearer? DefaultValue { get; set; }

    public TBearer? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultReveal
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultStringBearer)
              , ComplexTypeFieldWhenNonNullOrDefaultStringBearer, DefaultValue)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullAndPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsStringBearer | SupportsCustomHandling)]
public class FieldStringBearerWithHandlingWhenNonNullOrDefaultStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer?>
  , IMoldSupportedDefaultValue<TBearer?>, ISupportsFieldHandling where TBearer : IStringBearer
{
    public TBearer? ComplexTypeFieldWhenNonNullOrDefaultStringBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public TBearer? DefaultValue { get; set; }

    public TBearer? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultRevealAs
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultStringBearerAs)
              , ComplexTypeFieldWhenNonNullOrDefaultStringBearerAs
              , DefaultValue, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullAndPopulatedWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerWhenNonNullOrDefaultStringBearer<TBearerStruct> : IStringBearer
  , IMoldSupportedValue<TBearerStruct?>, IMoldSupportedDefaultValue<TBearerStruct?> where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? ComplexTypeFieldWhenNonNullOrDefaultNullableStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public TBearerStruct? DefaultValue { get; set; }

    public TBearerStruct? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultReveal
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultNullableStringBearer)
              , ComplexTypeFieldWhenNonNullOrDefaultNullableStringBearer, DefaultValue)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullAndPopulatedWrites | AcceptsNullableStruct | AcceptsStringBearer |
                  SupportsCustomHandling)]
public class FieldNullableStringBearerWithHandlingWhenNonNullOrDefaultStringBearer<TBearerStruct> : IStringBearer, IMoldSupportedValue<TBearerStruct?>
  , IMoldSupportedDefaultValue<TBearerStruct?>, ISupportsFieldHandling where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? ComplexTypeFieldWhenNonNullOrDefaultNullableStringBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public TBearerStruct? DefaultValue { get; set; }

    public TBearerStruct? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultRevealAs
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultNullableStringBearerAs)
              , ComplexTypeFieldWhenNonNullOrDefaultNullableStringBearerAs
              , DefaultValue, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSpanWhenNonNullOrDefaultStringBearer : IStringBearer, IMoldSupportedValue<char[]>, IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString
{
    public char[] ComplexTypeFieldAlwaysAddCharSpan
    {
        get => Value;
        set => Value = value;
    }

    public string DefaultValue { get; set; } = "";

    public char[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd
               (nameof(ComplexTypeFieldAlwaysAddCharSpan)
              , ComplexTypeFieldAlwaysAddCharSpan.AsSpan()
              ,  DefaultValue, ValueFormatString)
           .Complete();

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString |
                  SupportsCustomHandling)]
public class FieldCharSpanWithHandlingWhenNonNullOrDefaultStringBearer : IStringBearer, IMoldSupportedValue<char[]>
  , IMoldSupportedDefaultValue<string>, ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public char[] ComplexTypeFieldAlwaysAddCharSpanAs
    {
        get => Value;
        set => Value = value;
    }

    public string DefaultValue { get; set; } = "";

    public char[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddAs
               (nameof(ComplexTypeFieldAlwaysAddCharSpanAs)
              , ComplexTypeFieldAlwaysAddCharSpanAs.AsSpan()
              , FieldContentHandling, DefaultValue, ValueFormatString)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldCharReadOnlySpanWhenNonNullOrDefaultStringBearer : IStringBearer, IMoldSupportedValue<string>
  , IMoldSupportedDefaultValue<string>, ISupportsValueFormatString, ISupportsSettingValueFromString
{
    public string ComplexTypeFieldAlwaysAddReadOnlyCharSpan
    {
        get => Value;
        set => Value = value;
    }

    public string DefaultValue { get; set; } = "";

    public string Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd
               (nameof(ComplexTypeFieldAlwaysAddReadOnlyCharSpan)
              , ComplexTypeFieldAlwaysAddReadOnlyCharSpan.AsSpan()
              , DefaultValue, ValueFormatString)
           .Complete();

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString |
                  SupportsCustomHandling)]
public class FieldCharReadOnlySpanWithHandlingWhenNonNullOrDefaultStringBearer : IStringBearer, IMoldSupportedValue<string>
  , IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public string ComplexTypeFieldAlwaysAddReadOnlyCharSpanAs
    {
        get => Value;
        set => Value = value;
    }

    public string DefaultValue { get; set; } = "";

    public string Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddAs
               (nameof(ComplexTypeFieldAlwaysAddReadOnlyCharSpanAs)
              , ComplexTypeFieldAlwaysAddReadOnlyCharSpanAs.AsSpan()
              , FieldContentHandling, DefaultValue, ValueFormatString)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringWhenNonNullOrDefaultStringBearer : IStringBearer
  , IMoldSupportedValue<string?>, IMoldSupportedDefaultValue<string>, ISupportsValueFormatString, ISupportsSettingValueFromString
{
    public string? ComplexTypeFieldWhenNonNullOrDefaultString
    {
        get => Value;
        set => Value = value;
    }

    public string DefaultValue { get; set; } = "";

    public string? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultString)
              , ComplexTypeFieldWhenNonNullOrDefaultString
              , DefaultValue, ValueFormatString)
           .Complete();

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldStringWithHandlingWhenNonNullOrDefaultStringBearer : IStringBearer, IMoldSupportedValue<string?>, IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public string? ComplexTypeFieldWhenNonNullOrDefaultStringAs
    {
        get => Value;
        set => Value = value;
    }

    public string DefaultValue { get; set; } = "";

    public string? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddAs
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultStringAs)
              , ComplexTypeFieldWhenNonNullOrDefaultStringAs
              , DefaultValue, FieldContentHandling, ValueFormatString)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }
    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class FieldStringRangeWhenNonNullOrDefaultStringBearer : IStringBearer, IMoldSupportedValue<string?>, IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public string? ComplexTypeFieldWhenNonNullOrDefaultStringRange
    {
        get => Value;
        set => Value = value;
    }

    public string DefaultValue { get; set; } = "";

    public string? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultStringRange)
              , ComplexTypeFieldWhenNonNullOrDefaultStringRange
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

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges |
                  SupportsCustomHandling)]
public class FieldStringRangeWithHandlingWhenNonNullOrDefaultStringBearer : IStringBearer, IMoldSupportedValue<string?>
  , IMoldSupportedDefaultValue<string>, ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
  , ISupportsFieldHandling
{
    public string? ComplexTypeFieldWhenNonNullOrDefaultStringRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public string DefaultValue { get; set; } = "";

    public string? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddAs
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultStringRangeAs)
              , ComplexTypeFieldWhenNonNullOrDefaultStringRangeAs
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

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldCharArrayWhenNonNullOrDefaultStringBearer : IStringBearer, IMoldSupportedValue<char[]>, IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString
{
    public char[] ComplexTypeFieldWhenNonNullOrDefaultCharArray
    {
        get => Value;
        set => Value = value;
    }

    public string DefaultValue { get; set; } = "";

    public char[] Value { get; set; } = null!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultCharArray)
              , ComplexTypeFieldWhenNonNullOrDefaultCharArray
              , DefaultValue, ValueFormatString)
           .Complete();

    public string? StringValue
    {
        get => Value != null! ? new string(Value) : null;
        set => Value = value?.ToCharArray()!;
    }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldCharArrayWithHandlingWhenNonNullOrDefaultStringBearer : IStringBearer, IMoldSupportedValue<char[]>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public char[] ComplexTypeFieldWhenNonNullOrDefaultCharArrayAs
    {
        get => Value;
        set => Value = value;
    }

    public string DefaultValue { get; set; } = "";

    public char[] Value { get; set; } = null!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddAs
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultCharArrayAs)
              , ComplexTypeFieldWhenNonNullOrDefaultCharArrayAs
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

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class FieldCharArrayRangeWhenNonNullOrDefaultStringBearer : IStringBearer, IMoldSupportedValue<char[]>, IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[] ComplexTypeFieldWhenNonNullOrDefaultCharArrayRange
    {
        get => Value;
        set => Value = value;
    }

    public string DefaultValue { get; set; } = "";

    public char[] Value { get; set; } = null!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultCharArrayRange)
              , ComplexTypeFieldWhenNonNullOrDefaultCharArrayRange
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

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges |
                  SupportsCustomHandling)]
public class FieldCharArrayRangeWithHandlingWhenNonNullOrDefaultStringBearer : IStringBearer, IMoldSupportedValue<char[]>
  , IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling
{
    public char[] ComplexTypeFieldWhenNonNullOrDefaultCharArrayRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public string DefaultValue { get; set; } = "";

    public char[] Value { get; set; } = null!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddAs
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultCharArrayRangeAs)
              , ComplexTypeFieldWhenNonNullOrDefaultCharArrayRangeAs
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

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceWhenNonNullOrDefaultStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq>
  , IMoldSupportedDefaultValue<string>, ISupportsValueFormatString, ISupportsSettingValueFromString where TCharSeq : ICharSequence
{
    public TCharSeq ComplexTypeFieldWhenNonNullOrDefaultCharSequence
    {
        get => Value;
        set => Value = value;
    }

    public string DefaultValue { get; set; } = "";

    public TCharSeq Value { get; set; } = default!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddCharSeq
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultCharSequence)
              , ComplexTypeFieldWhenNonNullOrDefaultCharSequence, DefaultValue, ValueFormatString)
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

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldCharSequenceWithHandlingWhenNonNullOrDefaultStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq>
  , IMoldSupportedDefaultValue<string>, ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsFieldHandling
    where TCharSeq : ICharSequence
{
    public TCharSeq ComplexTypeFieldWhenNonNullOrDefaultCharSequenceAs
    {
        get => Value;
        set => Value = value;
    }

    public string DefaultValue { get; set; } = "";

    public TCharSeq Value { get; set; } = default!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddCharSeqAs
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultCharSequenceAs)
              , ComplexTypeFieldWhenNonNullOrDefaultCharSequenceAs
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

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class FieldCharSequenceRangeWhenNonNullOrDefaultStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq>
  , IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
    where TCharSeq : ICharSequence
{
    public TCharSeq ComplexTypeFieldWhenNonNullOrDefaultCharSequenceRange
    {
        get => Value;
        set => Value = value;
    }

    public string DefaultValue { get; set; } = "";

    public TCharSeq Value { get; set; } = default!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddCharSeq
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultCharSequenceRange)
              , ComplexTypeFieldWhenNonNullOrDefaultCharSequenceRange
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

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges |
                  SupportsCustomHandling)]
public class FieldCharSequenceRangeWithHandlingWhenNonNullOrDefaultStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq>
  , IMoldSupportedDefaultValue<string>, ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
  , ISupportsFieldHandling
    where TCharSeq : ICharSequence
{
    public TCharSeq ComplexTypeFieldWhenNonNullOrDefaultCharSequenceRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public string DefaultValue { get; set; } = "";

    public TCharSeq Value { get; set; } = default!;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddCharSeqAs
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultCharSequenceRangeAs)
              , ComplexTypeFieldWhenNonNullOrDefaultCharSequenceRangeAs
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

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderWhenNonNullOrDefaultStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder>
  , IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString
{
    public StringBuilder ComplexTypeFieldWhenNonNullOrDefaultAddStringBuilder
    {
        get => Value;
        set => Value = value;
    }

    public string DefaultValue { get; set; } = "";

    public StringBuilder Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultAddStringBuilder)
              , ComplexTypeFieldWhenNonNullOrDefaultAddStringBuilder
              , DefaultValue, ValueFormatString)
           .Complete();

    public string? StringValue
    {
        get => Value.ToString();
        set => Value = new StringBuilder(value);
    }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldStringBuilderWithHandlingWhenNonNullOrDefaultStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder>
  , IMoldSupportedDefaultValue<string>, ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public StringBuilder ComplexTypeFieldWhenNonNullOrDefaultAddStringBuilderAs
    {
        get => Value;
        set => Value = value;
    }

    public string DefaultValue { get; set; } = "";

    public StringBuilder Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddAs
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultAddStringBuilderAs)
              , ComplexTypeFieldWhenNonNullOrDefaultAddStringBuilderAs
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

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class FieldStringBuilderRangeWhenNonNullOrDefaultStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder>
  , IMoldSupportedDefaultValue<string>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public StringBuilder ComplexTypeFieldWhenNonNullOrDefaultAddStringBuilderRange
    {
        get => Value;
        set => Value = value;
    }

    public string DefaultValue { get; set; } = "";

    public StringBuilder Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAdd
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultAddStringBuilderRange),
                ComplexTypeFieldWhenNonNullOrDefaultAddStringBuilderRange
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

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges |
                  SupportsCustomHandling)]
public class FieldStringBuilderRangeWithHandlingWhenNonNullOrDefaultStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder>
  , IMoldSupportedDefaultValue<string>, ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsFieldHandling
  , ISupportsIndexRangeLimiting
{
    public StringBuilder ComplexTypeFieldWhenNonNullOrDefaultAddStringBuilderRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public string DefaultValue { get; set; } = "";

    public StringBuilder Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddAs
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultAddStringBuilderRangeAs)
              , ComplexTypeFieldWhenNonNullOrDefaultAddStringBuilderRangeAs
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

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullAndPopulatedWrites | AcceptsAny | SupportsValueFormatString)]
public class FieldMatchWhenNonNullOrDefaultStringBearer<T> : IStringBearer, IMoldSupportedValue<T?>
  , IMoldSupportedDefaultValue<T?>, ISupportsValueFormatString
{
    public T? ComplexTypeFieldWhenNonNullOrDefaultMatch
    {
        get => Value;
        set => Value = value;
    }

    public T? DefaultValue { get; set; }

    public T? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddMatch
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultMatch)
              , ComplexTypeFieldWhenNonNullOrDefaultMatch
              , DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | NonNullAndPopulatedWrites | AcceptsAny | SupportsValueFormatString)]
public class FieldObjectWhenNonNullOrDefaultStringBearer : IStringBearer, IMoldSupportedValue<object?>, IMoldSupportedDefaultValue<object?>
  , ISupportsValueFormatString
{
    public object? ComplexTypeFieldWhenNonNullOrDefaultObject
    {
        get => Value;
        set => Value = value;
    }

    public object? DefaultValue { get; set; }

    public object? Value { get; set; }

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.WhenNonNullOrDefaultAddObject
               (nameof(ComplexTypeFieldWhenNonNullOrDefaultObject)
              , ComplexTypeFieldWhenNonNullOrDefaultObject
              , DefaultValue, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}
