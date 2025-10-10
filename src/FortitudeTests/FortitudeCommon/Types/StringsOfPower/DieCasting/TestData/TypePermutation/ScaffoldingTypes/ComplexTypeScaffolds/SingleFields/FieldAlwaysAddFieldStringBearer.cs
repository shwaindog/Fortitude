// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.SingleFields;

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct)]
public class FieldBoolAlwaysAddStringBearer : IMoldSupportedValue<bool>
{
    public bool ComplexTypeFieldAlwaysAddBool
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddBool);
    public bool Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddBool), ComplexTypeFieldAlwaysAddBool)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsNullableStruct)]
public class FieldNullableBoolAlwaysAddStringBearer : IMoldSupportedValue<bool?>
{
    public bool? ComplexTypeFieldAlwaysAddNullableBool
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddNullableBool);
    public bool? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddNullableBool), ComplexTypeFieldAlwaysAddNullableBool)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableAlwaysAddStringBearer<TFmt> : IMoldSupportedValue<TFmt>, ISupportsValueFormatString
    where TFmt : ISpanFormattable
{
    public TFmt ComplexTypeFieldAlwaysAddSpanFormattable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddSpanFormattable);
    public TFmt Value { get; set; } = default!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddSpanFormattable), ComplexTypeFieldAlwaysAddSpanFormattable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldSpanFormattableWithHandlingAlwaysAddStringBearer<TFmt> : IMoldSupportedValue<TFmt>, ISupportsValueFormatString
  , ISupportsFieldHandling where TFmt : ISpanFormattable
{
    public TFmt ComplexTypeFieldAlwaysAddSpanFormattableAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddSpanFormattableAs);

    public TFmt Value { get; set; } = default!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddAs(nameof(ComplexTypeFieldAlwaysAddSpanFormattableAs), ComplexTypeFieldAlwaysAddSpanFormattableAs, FieldContentHandling
                            , ValueFormatString)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableAlwaysAddStringBearer<TFmtStruct> : IMoldSupportedValue<TFmtStruct?>
  , ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? ComplexTypeFieldAlwaysAddNullableSpanFormattable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddNullableSpanFormattable);
    public TFmtStruct? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddNullableSpanFormattable), ComplexTypeFieldAlwaysAddNullableSpanFormattable
                          , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldNullableSpanFormattableWithHandlingAlwaysAddStringBearer<TFmtStruct> : IMoldSupportedValue<TFmtStruct?>
  , ISupportsFieldHandling, ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct? ComplexTypeFieldAlwaysAddNullableSpanFormattableAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddNullableSpanFormattableAs);

    public TFmtStruct? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddAs(nameof(ComplexTypeFieldAlwaysAddNullableSpanFormattableAs), ComplexTypeFieldAlwaysAddNullableSpanFormattableAs
                            , FieldContentHandling
                            , ValueFormatString)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerAlwaysAddStringBearer<TCloaked, TCloakedBase> : IMoldSupportedValue<TCloaked?>
  , ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked? ComplexTypeFieldAlwaysAddCloakedBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddCloakedBearer);
    public TCloaked? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal(nameof(ComplexTypeFieldAlwaysAddCloakedBearer), ComplexTypeFieldAlwaysAddCloakedBearer, ValueRevealer)
           .Complete();

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer | SupportsCustomHandling)]
public class FieldCloakedBearerWithHandlingAlwaysAddStringBearer<TTCloaked, TCloakedBase> : IMoldSupportedValue<TTCloaked?>
  , ISupportsFieldHandling, ISupportsValueRevealer<TCloakedBase> where TTCloaked : TCloakedBase
{
    public TTCloaked? ComplexTypeFieldAlwaysAddCloakedBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddCloakedBearerAs);

    public TTCloaked? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysRevealAs(nameof(ComplexTypeFieldAlwaysAddCloakedBearerAs), ComplexTypeFieldAlwaysAddCloakedBearerAs, ValueRevealer
                               , FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerAlwaysAddStringBearer<TCloakedStruct> : IMoldSupportedValue<TCloakedStruct?>
  , ISupportsValueRevealer<TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct? ComplexTypeFieldAlwaysAddCloakedBearerStruct
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddCloakedBearerStruct);
    public TCloakedStruct? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal(nameof(ComplexTypeFieldAlwaysAddCloakedBearerStruct), ComplexTypeFieldAlwaysAddCloakedBearerStruct, ValueRevealer)
           .Complete();


    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer
                | SupportsValueRevealer | SupportsCustomHandling)]
public class FieldNullableCloakedBearerWithHandlingAlwaysAddStringBearer<TCloakedStruct> : IMoldSupportedValue<TCloakedStruct?>
  , ISupportsValueRevealer<TCloakedStruct>, ISupportsFieldHandling where TCloakedStruct : struct
{
    public TCloakedStruct? ComplexTypeFieldAlwaysAddCloakedBearerStructAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddCloakedBearerStructAs);

    public TCloakedStruct? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysRevealAs(nameof(ComplexTypeFieldAlwaysAddCloakedBearerStructAs), ComplexTypeFieldAlwaysAddCloakedBearerStructAs
                               , ValueRevealer, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerAlwaysAddStringBearer<TBearer> : IMoldSupportedValue<TBearer?> where TBearer : IStringBearer
  , IMoldSupportedValue<TBearer?>
{
    public TBearer? ComplexTypeFieldAlwaysAddStringBearer
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddStringBearer);
    public TBearer? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal(nameof(ComplexTypeFieldAlwaysAddStringBearer), ComplexTypeFieldAlwaysAddStringBearer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer |
                  SupportsCustomHandling)]
public class FieldStringBearerWithHandlingAlwaysAddStringBearer<TBearer> : IMoldSupportedValue<TBearer?>, ISupportsFieldHandling
    where TBearer : IStringBearer
{
    public TBearer? ComplexTypeFieldAlwaysAddStringBearerAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddStringBearerAs);

    public TBearer? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysRevealAs(nameof(ComplexTypeFieldAlwaysAddStringBearerAs), ComplexTypeFieldAlwaysAddStringBearerAs, FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerAlwaysAddStringBearer<TBearerStruct> : IMoldSupportedValue<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? ComplexTypeFieldAlwaysAddStringBearerStruct
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddStringBearerStruct);
    public TBearerStruct? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysReveal(nameof(ComplexTypeFieldAlwaysAddStringBearerStruct), ComplexTypeFieldAlwaysAddStringBearerStruct)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsStruct | AcceptsNullableStruct | AcceptsStringBearer |
                  SupportsCustomHandling)]
public class FieldNullableStringBearerWithHandlingAlwaysAddStringBearer<TBearerStruct> : IMoldSupportedValue<TBearerStruct?>
  , ISupportsFieldHandling where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct? ComplexTypeFieldAlwaysAddStringBearerStructAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddStringBearerStructAs);

    public TBearerStruct? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysRevealAs(nameof(ComplexTypeFieldAlwaysAddStringBearerStructAs), ComplexTypeFieldAlwaysAddStringBearerStructAs
                               , FieldContentHandling)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | CallsAsSpan | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSpanAlwaysAddStringBearer : IMoldSupportedValue<char[]>, ISupportsValueFormatString
  , ISupportsSettingValueFromString
{
    public char[] ComplexTypeFieldAlwaysAddCharSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddCharSpan);
    public char[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddCharSpan), ComplexTypeFieldAlwaysAddCharSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | CallsAsSpan | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldCharSpanWithHandlingAlwaysAddStringBearer : IMoldSupportedValue<char[]>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public char[] ComplexTypeFieldAlwaysAddCharSpanAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddCharSpanAs);

    public char[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddAs(nameof(ComplexTypeFieldAlwaysAddCharSpanAs), ComplexTypeFieldAlwaysAddCharSpanAs.AsSpan()
                            , FieldContentHandling
                            , ValueFormatString)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => new(Value.AsSpan());
        set => Value = value?.ToCharArray()!;
    }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | CallsAsReadOnlySpan | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldCharReadOnlySpanAlwaysAddStringBearer : IMoldSupportedValue<string>, ISupportsValueFormatString
  , ISupportsSettingValueFromString
{
    public string ComplexTypeFieldAlwaysAddReadOnlyCharSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddReadOnlyCharSpan);
    public string Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddReadOnlyCharSpan), ComplexTypeFieldAlwaysAddReadOnlyCharSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? StringValue
    {
        get => Value;
        set => Value = value!;
    }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | CallsAsReadOnlySpan | AlwaysWrites | AcceptsChars | SupportsValueFormatString |
                  SupportsCustomHandling)]
public class FieldCharReadOnlySpanWithHandlingAlwaysAddStringBearer : IMoldSupportedValue<string>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public string ComplexTypeFieldAlwaysAddReadOnlyCharSpanAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddReadOnlyCharSpanAs);

    public string Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddAs(nameof(ComplexTypeFieldAlwaysAddReadOnlyCharSpanAs), ComplexTypeFieldAlwaysAddReadOnlyCharSpanAs.AsSpan()
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

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringAlwaysAddStringBearer : IMoldSupportedValue<string?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString
{
    public string? ComplexTypeFieldAlwaysAddString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddString);
    public string? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddString), ComplexTypeFieldAlwaysAddString, ValueFormatString)
           .Complete();

    public string? StringValue
    {
        get => Value;
        set => Value = value;
    }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldStringWithHandlingAlwaysAddStringBearer : IMoldSupportedValue<string?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public string? ComplexTypeFieldAlwaysAddStringAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddStringAs);

    public string? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddAs(nameof(ComplexTypeFieldAlwaysAddStringAs), ComplexTypeFieldAlwaysAddStringAs, FieldContentHandling, ValueFormatString)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value;
    }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class FieldStringRangeAlwaysAddStringBearer : IMoldSupportedValue<string?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public string? ComplexTypeFieldAlwaysAddStringRange
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddStringRange);

    public string? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddStringRange), ComplexTypeFieldAlwaysAddStringRange, FromIndex, Length, ValueFormatString)
           .Complete();

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value;
    }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges |
                  SupportsCustomHandling)]
public class FieldStringRangeWithHandlingAlwaysAddStringBearer : IMoldSupportedValue<string?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling
{
    public string? ComplexTypeFieldAlwaysAddStringRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddStringRangeAs);

    public string? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddAs(nameof(ComplexTypeFieldAlwaysAddStringRangeAs), ComplexTypeFieldAlwaysAddStringRangeAs, FromIndex, Length
                            , FieldContentHandling
                            , ValueFormatString)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value;
        set => Value = value;
    }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldCharArrayAlwaysAddStringBearer : IMoldSupportedValue<char[]?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString
{
    public char[]? ComplexTypeFieldAlwaysAddCharArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddCharArray);
    public char[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddCharArray), ComplexTypeFieldAlwaysAddCharArray, ValueFormatString)
           .Complete();

    public string? StringValue
    {
        get => Value != null ? new string(Value) : null;
        set => Value = value?.ToCharArray();
    }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldCharArrayWithHandlingAlwaysAddStringBearer : IMoldSupportedValue<char[]?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public char[]? ComplexTypeFieldAlwaysAddCharArrayAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddCharArrayAs);

    public char[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddAs(nameof(ComplexTypeFieldAlwaysAddCharArrayAs), ComplexTypeFieldAlwaysAddCharArrayAs, FieldContentHandling
                            , ValueFormatString)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value != null ? new string(Value) : null;
        set => Value = value?.ToCharArray();
    }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class FieldCharArrayRangeAlwaysAddStringBearer
    : IMoldSupportedValue<char[]?>, ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public char[]? ComplexTypeFieldAlwaysAddCharArrayRange
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddCharArrayRange);

    public char[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddCharArrayRange), ComplexTypeFieldAlwaysAddCharArrayRange, FromIndex, Length
                          , ValueFormatString)
           .Complete();

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null ? new string(Value) : null;
        set => Value = value?.ToCharArray();
    }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges |
                  SupportsCustomHandling)]
public class FieldCharArrayRangeWithHandlingAlwaysAddStringBearer : IMoldSupportedValue<char[]?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling
{
    public char[]? ComplexTypeFieldAlwaysAddCharArrayRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddCharArrayRangeAs);
    public char[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddAs(nameof(ComplexTypeFieldAlwaysAddCharArrayRangeAs), ComplexTypeFieldAlwaysAddCharArrayRangeAs, FromIndex, Length
                            , FieldContentHandling
                            , ValueFormatString)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null ? new string(Value) : null;
        set => Value = value?.ToCharArray();
    }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceAlwaysAddStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString where TCharSeq : ICharSequence
{
    public TCharSeq? ComplexTypeFieldAlwaysAddCharSequence
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddCharSequence);
    public TCharSeq? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddCharSeq(nameof(ComplexTypeFieldAlwaysAddCharSequence), ComplexTypeFieldAlwaysAddCharSequence, ValueFormatString)
           .Complete();

    public string? StringValue
    {
        get => Value != null ? Value.ToString() : null;
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

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldCharSequenceWithHandlingAlwaysAddStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsFieldHandling where TCharSeq : ICharSequence
{
    public TCharSeq? ComplexTypeFieldAlwaysAddCharSequenceAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddCharSequenceAs);

    public TCharSeq? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddCharSeqAs(nameof(ComplexTypeFieldAlwaysAddCharSequenceAs), ComplexTypeFieldAlwaysAddCharSequenceAs, FieldContentHandling
                                   , ValueFormatString)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value != null ? Value.ToString() : null;
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

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class FieldCharSequenceRangeAlwaysAddStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting where TCharSeq : ICharSequence
{
    public TCharSeq? ComplexTypeFieldAlwaysAddCharSequenceRange
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddCharSequenceRange);

    public TCharSeq? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddCharSeq(nameof(ComplexTypeFieldAlwaysAddCharSequenceRange), ComplexTypeFieldAlwaysAddCharSequenceRange, FromIndex, Length
                                 , ValueFormatString)
           .Complete();

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null ? Value.ToString() : null;
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

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges |
                  SupportsCustomHandling)]
public class FieldCharSequenceRangeWithHandlingAlwaysAddStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq?>
  , ISupportsValueFormatString, ISupportsSettingValueFromString, ISupportsIndexRangeLimiting, ISupportsFieldHandling
    where TCharSeq : ICharSequence
{
    public TCharSeq? ComplexTypeFieldAlwaysAddCharSequenceRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddCharSequenceRangeAs);

    public TCharSeq? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddCharSeqAs(nameof(ComplexTypeFieldAlwaysAddCharSequenceRangeAs), ComplexTypeFieldAlwaysAddCharSequenceRangeAs, FromIndex
                                   , Length
                                   , FieldContentHandling, ValueFormatString)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }
    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value != null ? Value.ToString() : null;
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

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderAlwaysAddStringBearer : IMoldSupportedValue<StringBuilder?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString
{
    public StringBuilder? ComplexTypeFieldAlwaysAddStringBuilder
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddStringBuilder);
    public StringBuilder? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddStringBuilder), ComplexTypeFieldAlwaysAddStringBuilder, ValueFormatString)
           .Complete();

    public string? StringValue
    {
        get => Value?.ToString();
        set => Value = new StringBuilder(value);
    }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldStringBuilderWithHandlingAlwaysAddStringBearer : IMoldSupportedValue<StringBuilder?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsFieldHandling
{
    public StringBuilder? ComplexTypeFieldAlwaysAddStringBuilderAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddStringBuilderAs);

    public StringBuilder? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddAs(nameof(ComplexTypeFieldAlwaysAddStringBuilderAs), ComplexTypeFieldAlwaysAddStringBuilderAs, FieldContentHandling
                            , ValueFormatString)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public string? StringValue
    {
        get => Value?.ToString();
        set => Value = new StringBuilder(value);
    }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges)]
public class FieldStringBuilderRangeAlwaysAddStringBearer : IMoldSupportedValue<StringBuilder?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString, ISupportsIndexRangeLimiting
{
    public StringBuilder? ComplexTypeFieldAlwaysAddStringBuilderRange
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddStringBuilderRange);

    public StringBuilder? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(ComplexTypeFieldAlwaysAddStringBuilderRange), ComplexTypeFieldAlwaysAddStringBuilderRange, FromIndex, Length
                          , ValueFormatString)
           .Complete();

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value?.ToString();
        set => Value = new StringBuilder(value);
    }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsChars | SupportsValueFormatString | SupportsIndexSubRanges |
                  SupportsCustomHandling)]
public class FieldStringBuilderRangeWithHandlingAlwaysAddStringBearer : IMoldSupportedValue<StringBuilder?>, ISupportsValueFormatString
  , ISupportsSettingValueFromString
  , ISupportsIndexRangeLimiting, ISupportsFieldHandling
{
    public StringBuilder? ComplexTypeFieldAlwaysAddStringBuilderRangeAs
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddStringBuilderRangeAs);
    public StringBuilder? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddAs(nameof(ComplexTypeFieldAlwaysAddStringBuilderRangeAs), ComplexTypeFieldAlwaysAddStringBuilderRangeAs, FromIndex, Length
                            , FieldContentHandling, ValueFormatString)
           .Complete();

    public FieldContentHandling FieldContentHandling { get; set; }

    public int FromIndex { get; set; }

    public int Length { get; set; }

    public string? StringValue
    {
        get => Value?.ToString();
        set => Value = new StringBuilder(value);
    }

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsAny | SupportsValueFormatString)]
public class FieldMatchAlwaysAddStringBearer<TAny> : IMoldSupportedValue<TAny?>, ISupportsValueFormatString
{
    public TAny? ComplexTypeFieldAlwaysAddMatch
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddMatch);
    public TAny? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddMatch(nameof(ComplexTypeFieldAlwaysAddMatch), ComplexTypeFieldAlwaysAddMatch, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsAny | SupportsValueFormatString)]
public class FieldObjectAlwaysAddStringBearer : IMoldSupportedValue<object?>, ISupportsValueFormatString
{
    public object? ComplexTypeFieldAlwaysAddObject
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeFieldAlwaysAddObject);
    public object? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAddObject(nameof(ComplexTypeFieldAlwaysAddObject), ComplexTypeFieldAlwaysAddObject, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}
