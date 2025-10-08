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

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct)]
public class FieldBoolAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<bool>
{
    public bool ComplexTypeFieldAlwaysAddBool
    {
        get => Value;
        set => Value = value;
    }

    public bool Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddBool), ComplexTypeFieldAlwaysAddBool)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites| AcceptsStruct | AcceptsNullableStruct)]
public class FieldNullableBoolAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<bool?>
{
    public bool? ComplexTypeFieldAlwaysAddNullableBool
    {
        get => Value;
        set => Value = value;
    }

    public bool? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddNullableBool), ComplexTypeFieldAlwaysAddNullableBool)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableAlwaysAddStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt>, ISupportsValueFormatString
    where TFmt : ISpanFormattable
{
    public TFmt ComplexTypeFieldAlwaysAddSpanFormattable
    {
        get => Value;
        set => Value = value;
    }

    public TFmt Value { get; set; } = default!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddSpanFormattable), ComplexTypeFieldAlwaysAddSpanFormattable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldSpanFormattableWithHandlingAlwaysAddStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt>, ISupportsValueFormatString
  , ISupportsFieldHandling where TFmt : ISpanFormattable
{
    public TFmt ComplexTypeFieldAlwaysAddSpanFormattableAs
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public TFmt Value { get; set; } = default!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddAs(nameof(ComplexTypeFieldAlwaysAddSpanFormattableAs), ComplexTypeFieldAlwaysAddSpanFormattableAs, FieldContentHandling
                            , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableAlwaysAddStringBearer<TFmtStruct> : IStringBearer, IMoldSupportedValue<TFmtStruct?>, ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? ComplexTypeFieldAlwaysAddNullableSpanFormattable
    {
        get => Value;
        set => Value = value;
    }

    public TFmtStruct? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddNullableSpanFormattable), ComplexTypeFieldAlwaysAddNullableSpanFormattable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldNullableSpanFormattableWithHandlingAlwaysAddStringBearer<TFmtStruct> : IStringBearer, IMoldSupportedValue<TFmtStruct?>
  , ISupportsFieldHandling, ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? ComplexTypeFieldAlwaysAddNullableSpanFormattableAs
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public TFmtStruct? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddAs(nameof(ComplexTypeFieldAlwaysAddNullableSpanFormattableAs), ComplexTypeFieldAlwaysAddNullableSpanFormattableAs
                            , FieldContentHandling
                            , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                 | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerAlwaysAddStringBearer<TCloaked, TCloakedBase> : IStringBearer, IMoldSupportedValue<TCloaked?>
  , ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked? ComplexTypeFieldAlwaysAddCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public TCloaked? Value { get; set; }

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal(nameof(ComplexTypeFieldAlwaysAddCloakedBearer), ComplexTypeFieldAlwaysAddCloakedBearer, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass 
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer | SupportsCustomHandling)]
public class FieldCloakedBearerWithHandlingAlwaysAddStringBearer<TTCloaked, TCloakedBase> : IStringBearer, IMoldSupportedValue<TTCloaked?>
  , ISupportsFieldHandling, ISupportsValueRevealer<TCloakedBase> where TTCloaked : TCloakedBase
{
    public TTCloaked? ComplexTypeFieldAlwaysAddCloakedBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public TTCloaked? Value { get; set; }

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysRevealAs(nameof(ComplexTypeFieldAlwaysAddCloakedBearerAs), ComplexTypeFieldAlwaysAddCloakedBearerAs, ValueRevealer
                               , FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerAlwaysAddStringBearer<TCloakedStruct> : IStringBearer, IMoldSupportedValue<TCloakedStruct?>
  , ISupportsValueRevealer<TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct? ComplexTypeFieldAlwaysAddCloakedBearerStruct
    {
        get => Value;
        set => Value = value;
    }

    public TCloakedStruct? Value { get; set; }


    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal(nameof(ComplexTypeFieldAlwaysAddCloakedBearerStruct), ComplexTypeFieldAlwaysAddCloakedBearerStruct, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer | SupportsCustomHandling)]
public class FieldNullableCloakedBearerWithHandlingAlwaysAddStringBearer<TCloakedStruct> : IStringBearer, IMoldSupportedValue<TCloakedStruct?>
  , ISupportsValueRevealer<TCloakedStruct>, ISupportsFieldHandling where TCloakedStruct : struct
{
    public TCloakedStruct? ComplexTypeFieldAlwaysAddCloakedBearerStructAs
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public TCloakedStruct? Value { get; set; }

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysRevealAs(nameof(ComplexTypeFieldAlwaysAddCloakedBearerStructAs), ComplexTypeFieldAlwaysAddCloakedBearerStructAs
                               , ValueRevealer, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerAlwaysAddStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer?> where TBearer : IStringBearer
  , IMoldSupportedValue<TBearer?>
{
    public TBearer? ComplexTypeFieldAlwaysAddStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public TBearer? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal(nameof(ComplexTypeFieldAlwaysAddStringBearer), ComplexTypeFieldAlwaysAddStringBearer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer |
                  SupportsCustomHandling)]
public class FieldStringBearerWithHandlingAlwaysAddStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer?>, ISupportsFieldHandling
    where TBearer : IStringBearer
{
    public TBearer? ComplexTypeFieldAlwaysAddStringBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public TBearer? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysRevealAs(nameof(ComplexTypeFieldAlwaysAddStringBearerAs), ComplexTypeFieldAlwaysAddStringBearerAs, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerAlwaysAddStringBearer<TBearerStruct> : IStringBearer, IMoldSupportedValue<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? ComplexTypeFieldAlwaysAddStringBearerStruct
    {
        get => Value;
        set => Value = value;
    }

    public TBearerStruct? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal(nameof(ComplexTypeFieldAlwaysAddStringBearerStruct), ComplexTypeFieldAlwaysAddStringBearerStruct)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites |AcceptsStruct | AcceptsNullableStruct | AcceptsStringBearer | SupportsCustomHandling)]
public class FieldNullableStringBearerWithHandlingAlwaysAddStringBearer<TBearerStruct> : IStringBearer, IMoldSupportedValue<TBearerStruct?>
  , ISupportsFieldHandling where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? ComplexTypeFieldAlwaysAddStringBearerStructAs
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public TBearerStruct? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysRevealAs(nameof(ComplexTypeFieldAlwaysAddStringBearerStructAs), ComplexTypeFieldAlwaysAddStringBearerStructAs
                               , FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | CallsAsSpan | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSpanAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<char[]>, ISupportsValueFormatString, ISupportsSettingValueFromString
{
    public char[] ComplexTypeFieldAlwaysAddCharSpan
    {
        get => Value;
        set => Value = value;
    }

    public char[] Value { get; set; } = null!;

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddCharSpan), ComplexTypeFieldAlwaysAddCharSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | CallsAsSpan | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldCharSpanWithHandlingAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<char[]>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public char[] ComplexTypeFieldAlwaysAddCharSpanAs
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public char[] Value { get; set; } = null!;

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddAs(nameof(ComplexTypeFieldAlwaysAddCharSpanAs), ComplexTypeFieldAlwaysAddCharSpanAs.AsSpan()
                            , FieldContentHandling
                            , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | CallsAsReadOnlySpan | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldCharReadOnlySpanAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<string>, ISupportsValueFormatString
  , ISupportsSettingValueFromString
{
    public string ComplexTypeFieldAlwaysAddReadOnlyCharSpan
    {
        get => Value;
        set => Value = value;
    }

    public string Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddReadOnlyCharSpan), ComplexTypeFieldAlwaysAddReadOnlyCharSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | CallsAsReadOnlySpan | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldCharReadOnlySpanWithHandlingAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<string>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public string ComplexTypeFieldAlwaysAddReadOnlyCharSpanAs
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public string Value { get; set; } = null!;

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddAs(nameof(ComplexTypeFieldAlwaysAddReadOnlyCharSpanAs), ComplexTypeFieldAlwaysAddReadOnlyCharSpanAs.AsSpan()
                            , FieldContentHandling
                            , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<string?>, ISupportsValueFormatString, ISupportsSettingValueFromString
{
    public string? ComplexTypeFieldAlwaysAddString
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
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddString), ComplexTypeFieldAlwaysAddString, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldStringWithHandlingAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<string?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public string? ComplexTypeFieldAlwaysAddStringAs
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
           .Field.AlwaysAddAs(nameof(ComplexTypeFieldAlwaysAddStringAs), ComplexTypeFieldAlwaysAddStringAs, FieldContentHandling, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class FieldStringRangeAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<string?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public string? ComplexTypeFieldAlwaysAddStringRange
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
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddStringRange), ComplexTypeFieldAlwaysAddStringRange, FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges | SupportsCustomHandling)]
public class FieldStringRangeWithHandlingAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<string?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling
{
    public string? ComplexTypeFieldAlwaysAddStringRangeAs
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
           .Field.AlwaysAddAs(nameof(ComplexTypeFieldAlwaysAddStringRangeAs), ComplexTypeFieldAlwaysAddStringRangeAs, FromIndex, Length
                            , FieldContentHandling
                            , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldCharArrayAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<char[]?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString
{
    public char[]? ComplexTypeFieldAlwaysAddCharArray
    {
        get => Value;
        set => Value = value;
    }

    public char[]? Value { get; set; }

    public string? StringValue
    {
        get => Value != null ? new string(Value) : null;
        set => Value = value?.ToCharArray();
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddCharArray), ComplexTypeFieldAlwaysAddCharArray, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldCharArrayWithHandlingAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<char[]?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public char[]? ComplexTypeFieldAlwaysAddCharArrayAs
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
           .Field.AlwaysAddAs(nameof(ComplexTypeFieldAlwaysAddCharArrayAs), ComplexTypeFieldAlwaysAddCharArrayAs, FieldContentHandling, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class FieldCharArrayRangeAlwaysAddStringBearer
    : IStringBearer, IMoldSupportedValue<char[]?>, ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[]? ComplexTypeFieldAlwaysAddCharArrayRange
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
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddCharArrayRange), ComplexTypeFieldAlwaysAddCharArrayRange, FromIndex, Length, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges | SupportsCustomHandling)]
public class FieldCharArrayRangeWithHandlingAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<char[]?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling
{
    public char[]? ComplexTypeFieldAlwaysAddCharArrayRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public char[]? Value { get; set; }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value != null ? new string(Value) : null;
        set => Value = value?.ToCharArray();
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddAs(nameof(ComplexTypeFieldAlwaysAddCharArrayRangeAs), ComplexTypeFieldAlwaysAddCharArrayRangeAs, FromIndex, Length
                            , FieldContentHandling
                            , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceAlwaysAddStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString where TCharSeq : ICharSequence
{
    public TCharSeq? ComplexTypeFieldAlwaysAddCharSequence
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
           .Field.AlwaysAddCharSeq(nameof(ComplexTypeFieldAlwaysAddCharSequence), ComplexTypeFieldAlwaysAddCharSequence, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldCharSequenceWithHandlingAlwaysAddStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsFieldHandling where TCharSeq : ICharSequence
{
    public TCharSeq? ComplexTypeFieldAlwaysAddCharSequenceAs
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
           .Field.AlwaysAddCharSeqAs(nameof(ComplexTypeFieldAlwaysAddCharSequenceAs), ComplexTypeFieldAlwaysAddCharSequenceAs, FieldContentHandling
                                   , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class FieldCharSequenceRangeAlwaysAddStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq? ComplexTypeFieldAlwaysAddCharSequenceRange
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
           .Field.AlwaysAddCharSeq(nameof(ComplexTypeFieldAlwaysAddCharSequenceRange), ComplexTypeFieldAlwaysAddCharSequenceRange, FromIndex, Length
                                 , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges | SupportsCustomHandling)]
public class FieldCharSequenceRangeWithHandlingAlwaysAddStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq?>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling
    where TCharSeq : ICharSequence
{
    public TCharSeq? ComplexTypeFieldAlwaysAddCharSequenceRangeAs
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
           .Field.AlwaysAddCharSeqAs(nameof(ComplexTypeFieldAlwaysAddCharSequenceRangeAs), ComplexTypeFieldAlwaysAddCharSequenceRangeAs, FromIndex, Length
                                   , FieldContentHandling, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString
{
    public StringBuilder? ComplexTypeFieldAlwaysAddStringBuilder
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
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddStringBuilder), ComplexTypeFieldAlwaysAddStringBuilder, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldStringBuilderWithHandlingAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public StringBuilder? ComplexTypeFieldAlwaysAddStringBuilderAs
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
           .Field.AlwaysAddAs(nameof(ComplexTypeFieldAlwaysAddStringBuilderAs), ComplexTypeFieldAlwaysAddStringBuilderAs, FieldContentHandling
                            , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class FieldStringBuilderRangeAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public StringBuilder? ComplexTypeFieldAlwaysAddStringBuilderRange
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
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddStringBuilderRange), ComplexTypeFieldAlwaysAddStringBuilderRange, FromIndex, Length
                          , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges | SupportsCustomHandling)]
public class FieldStringBuilderRangeWithHandlingAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder?>, ISupportsValueFormatString, ISupportsSettingValueFromString
  , ISupportsIndexRangeLimiting, ISupportsFieldHandling
{
    public StringBuilder? ComplexTypeFieldAlwaysAddStringBuilderRangeAs
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

    public FieldContentHandling FieldContentHandling { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddAs(nameof(ComplexTypeFieldAlwaysAddStringBuilderRangeAs), ComplexTypeFieldAlwaysAddStringBuilderRangeAs, FromIndex, Length
                            , FieldContentHandling, ValueFormatString)
           .Complete();

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsAny | SupportsValueFormatString)]
public class FieldMatchAlwaysAddStringBearer<T> : IStringBearer, IMoldSupportedValue<T?>, ISupportsValueFormatString
{
    public T? ComplexTypeFieldAlwaysAddMatch
    {
        get => Value;
        set => Value = value;
    }

    public T? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddMatch(nameof(ComplexTypeFieldAlwaysAddMatch), ComplexTypeFieldAlwaysAddMatch, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsAny | SupportsValueFormatString)]
public class FieldObjectAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<object?>, ISupportsValueFormatString
{
    public object? ComplexTypeFieldAlwaysAddObject
    {
        get => Value;
        set => Value = value;
    }

    public object? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddObject(nameof(ComplexTypeFieldAlwaysAddObject), ComplexTypeFieldAlwaysAddObject, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}
