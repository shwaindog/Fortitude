// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.TypeGeneratePartFlags;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.SingleFields;

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct)]
public class BoolAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<bool>
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
public class NullableBoolAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<bool?>
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
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class SpanFormattableAlwaysAddStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt>, ISupportsSingleFormatString
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
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddSpanFormattable), ComplexTypeFieldAlwaysAddSpanFormattable, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString | SupportsCustomHandling)]
public class SpanFormattableWithHandlingAlwaysAddStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt>, ISupportsSingleFormatString
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
                            , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class NullableSpanFormattableAlwaysAddStringBearer<TFmtStruct> : IStringBearer, IMoldSupportedValue<TFmtStruct?>, ISupportsSingleFormatString
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
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddNullableSpanFormattable), ComplexTypeFieldAlwaysAddNullableSpanFormattable, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString | SupportsCustomHandling)]
public class NullableSpanFormattableWithHandlingAlwaysAddStringBearer<TFmtStruct> : IStringBearer, IMoldSupportedValue<TFmtStruct?>
  , ISupportsFieldHandling, ISupportsSingleFormatString where TFmtStruct : struct, ISpanFormattable
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
                            , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                 | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class CloakedBearerAlwaysAddStringBearer<TCloaked, TCloakedBase> : IStringBearer, IMoldSupportedValue<TCloaked?>
  , ISupportsSingleRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked? ComplexTypeFieldAlwaysAddCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public TCloaked? Value { get; set; }

    public PalantírReveal<TCloakedBase> PalantirRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal(nameof(ComplexTypeFieldAlwaysAddCloakedBearer), ComplexTypeFieldAlwaysAddCloakedBearer, PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass 
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | OnePalantirRevealer | SupportsCustomHandling)]
public class CloakedBearerWithHandlingAlwaysAddStringBearer<TTCloaked, TCloakedBase> : IStringBearer, IMoldSupportedValue<TTCloaked?>
  , ISupportsFieldHandling, ISupportsSingleRevealer<TCloakedBase> where TTCloaked : TCloakedBase
{
    public TTCloaked? ComplexTypeFieldAlwaysAddCloakedBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public TTCloaked? Value { get; set; }

    public PalantírReveal<TCloakedBase> PalantirRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysRevealAs(nameof(ComplexTypeFieldAlwaysAddCloakedBearerAs), ComplexTypeFieldAlwaysAddCloakedBearerAs, PalantirRevealer
                               , FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class NullableCloakedBearerAlwaysAddStringBearer<TCloakedStruct> : IStringBearer, IMoldSupportedValue<TCloakedStruct?>
  , ISupportsSingleRevealer<TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct? ComplexTypeFieldAlwaysAddCloakedBearerStruct
    {
        get => Value;
        set => Value = value;
    }

    public TCloakedStruct? Value { get; set; }


    public PalantírReveal<TCloakedStruct> PalantirRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal(nameof(ComplexTypeFieldAlwaysAddCloakedBearerStruct), ComplexTypeFieldAlwaysAddCloakedBearerStruct, PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | OnePalantirRevealer | SupportsCustomHandling)]
public class NullableCloakedBearerWithHandlingAlwaysAddStringBearer<TCloakedStruct> : IStringBearer, IMoldSupportedValue<TCloakedStruct?>
  , ISupportsSingleRevealer<TCloakedStruct>, ISupportsFieldHandling where TCloakedStruct : struct
{
    public TCloakedStruct? ComplexTypeFieldAlwaysAddCloakedBearerStructAs
    {
        get => Value;
        set => Value = value;
    }

    public FieldContentHandling FieldContentHandling { get; set; }

    public TCloakedStruct? Value { get; set; }

    public PalantírReveal<TCloakedStruct> PalantirRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysRevealAs(nameof(ComplexTypeFieldAlwaysAddCloakedBearerStructAs), ComplexTypeFieldAlwaysAddCloakedBearerStructAs
                               , PalantirRevealer, FieldContentHandling)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class StringBearerAlwaysAddStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer?> where TBearer : IStringBearer
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
public class StringBearerWithHandlingAlwaysAddStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer?>, ISupportsFieldHandling
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
public class NullableStringBearerAlwaysAddStringBearer<TBearerStruct> : IStringBearer, IMoldSupportedValue<TBearerStruct?>
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
public class NullableStringBearerWithHandlingAlwaysAddStringBearer<TBearerStruct> : IStringBearer, IMoldSupportedValue<TBearerStruct?>
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

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | OneFormatString)]
public class CharSpanAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<char[]>, ISupportsSingleFormatString, ISupportsSettingValueFromString
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
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddCharSpan), ComplexTypeFieldAlwaysAddCharSpan.AsSpan(), FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | OneFormatString | SupportsCustomHandling)]
public class CharSpanWithHandlingAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<char[]>, ISupportsSingleFormatString
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
                            , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | OneFormatString)]
public class CharReadOnlySpanAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<string>, ISupportsSingleFormatString
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
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddReadOnlyCharSpan), ComplexTypeFieldAlwaysAddReadOnlyCharSpan.AsSpan(), FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | OneFormatString | SupportsCustomHandling)]
public class CharReadOnlySpanWithHandlingAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<string>, ISupportsSingleFormatString
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
                            , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | OneFormatString)]
public class StringAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<string?>, ISupportsSingleFormatString, ISupportsSettingValueFromString
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
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddString), ComplexTypeFieldAlwaysAddString, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | OneFormatString | SupportsCustomHandling)]
public class StringWithHandlingAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<string?>, ISupportsSingleFormatString
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
           .Field.AlwaysAddAs(nameof(ComplexTypeFieldAlwaysAddStringAs), ComplexTypeFieldAlwaysAddStringAs, FieldContentHandling, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | OneFormatString | SupportsIndexSubRanges)]
public class StringRangeAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<string?>, ISupportsSingleFormatString
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
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddStringRange), ComplexTypeFieldAlwaysAddStringRange, FromIndex, Length, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | OneFormatString | SupportsIndexSubRanges | SupportsCustomHandling)]
public class StringRangeWithHandlingAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<string?>, ISupportsSingleFormatString
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
                            , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | OneFormatString)]
public class CharArrayAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<char[]?>, ISupportsSingleFormatString
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
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddCharArray), ComplexTypeFieldAlwaysAddCharArray, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | OneFormatString | SupportsCustomHandling)]
public class CharArrayWithHandlingAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<char[]?>, ISupportsSingleFormatString
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
           .Field.AlwaysAddAs(nameof(ComplexTypeFieldAlwaysAddCharArrayAs), ComplexTypeFieldAlwaysAddCharArrayAs, FieldContentHandling, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | OneFormatString | SupportsIndexSubRanges)]
public class CharArrayRangeAlwaysAddStringBearer
    : IStringBearer, IMoldSupportedValue<char[]?>, ISupportsSingleFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
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
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddCharArrayRange), ComplexTypeFieldAlwaysAddCharArrayRange, FromIndex, Length, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | OneFormatString | SupportsIndexSubRanges | SupportsCustomHandling)]
public class CharArrayRangeWithHandlingAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<char[]?>, ISupportsSingleFormatString
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
                            , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | OneFormatString)]
public class CharSequenceAlwaysAddStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq?>, ISupportsSingleFormatString
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
           .Field.AlwaysAddCharSeq(nameof(ComplexTypeFieldAlwaysAddCharSequence), ComplexTypeFieldAlwaysAddCharSequence, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | OneFormatString | SupportsCustomHandling)]
public class CharSequenceWithHandlingAlwaysAddStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq?>, ISupportsSingleFormatString
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
                                   , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | OneFormatString | SupportsIndexSubRanges)]
public class CharSequenceRangeAlwaysAddStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq?>, ISupportsSingleFormatString
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
                                 , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | OneFormatString | SupportsIndexSubRanges | SupportsCustomHandling)]
public class CharSequenceRangeWithHandlingAlwaysAddStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq?>
  , ISupportsSingleFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling
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
                                   , FieldContentHandling, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | OneFormatString)]
public class StringBuilderAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder?>, ISupportsSingleFormatString
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
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddStringBuilder), ComplexTypeFieldAlwaysAddStringBuilder, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | OneFormatString | SupportsCustomHandling)]
public class StringBuilderWithHandlingAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder?>, ISupportsSingleFormatString
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
                            , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | OneFormatString | SupportsIndexSubRanges)]
public class StringBuilderRangeAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder?>, ISupportsSingleFormatString
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
                          , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | OneFormatString | SupportsIndexSubRanges | SupportsCustomHandling)]
public class StringBuilderRangeWithHandlingAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder?>, ISupportsSingleFormatString, ISupportsSettingValueFromString
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
                            , FieldContentHandling, FormatString)
           .Complete();

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsAny | OneFormatString)]
public class MatchAlwaysAddStringBearer<T> : IStringBearer, IMoldSupportedValue<T?>, ISupportsSingleFormatString
{
    public T? ComplexTypeFieldAlwaysAddMatch
    {
        get => Value;
        set => Value = value;
    }

    public T? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddMatch(nameof(ComplexTypeFieldAlwaysAddMatch), ComplexTypeFieldAlwaysAddMatch, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsAny | OneFormatString)]
public class ObjectAlwaysAddStringBearer : IStringBearer, IMoldSupportedValue<object?>, ISupportsSingleFormatString
{
    public object? ComplexTypeFieldAlwaysAddObject
    {
        get => Value;
        set => Value = value;
    }

    public object? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddObject(nameof(ComplexTypeFieldAlwaysAddObject), ComplexTypeFieldAlwaysAddObject, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}
